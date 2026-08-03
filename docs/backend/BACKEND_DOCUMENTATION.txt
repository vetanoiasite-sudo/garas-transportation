# GARAS TRANSPORTATION MODULE — BACKEND DOCUMENTATION
## Internal Backend Engineering Reference Guide

> **Document Purpose:** Single source of truth for backend engineers working on the Transportation module of the Garas Core API. Describes the as-built architecture, conventions, endpoint contract, business rules, and known pitfalls — reverse-engineered from the actual code, not from an idealized template.
>
> **Sources analyzed:**
> - `NewGarasAPI/Controllers/TransportationModule/TransportationController.cs` (2,447 lines, 69 endpoints)
> - `NewGaras.Infrastructure/Interfaces/ServicesInterfaces/TransportationLine/ITransportationLineService.cs` (99 lines)
> - `NewGaras.Domain/Services/TransportationLineService/TransportationLineService.cs` (9,278 lines)
> - `NewGarasAPI/Controllers/UserController.cs` (1,958 lines) — **login / user & role management** (Section 4)
> - `NewGaras.Domain/Services/UserService.cs` (1,440 lines) + `IUserService.cs` (29 lines)
> - `NewGaras.Infrastructure/Entities/User.cs` — the login/identity table (admins + bus supervisors)
> - Solution-wide: `Program.cs`, `Helper/Helper.cs`, `GarasTestContext`, `UnitOfWork`, entities, tenant services
>
> **Last updated:** 2026-07-06

---

# 1. EXECUTIVE SUMMARY

The Transportation module is one vertical of **Garas Core API** — a large multi-tenant, multi-vertical ERP backend (**.NET 8**, EF Core 8, SQL Server) that also hosts HR/payroll, inventory, accounting, CRM, hotel, medical, library, and e-invoicing modules (84 controllers total).

The module implements an **employee bus-transportation domain**:

- Master data: lines → routes (with bus serial, supplier, driver, vehicle, supervisor) → stations → passenger memberships
- **Fingerprint-driven attendance**: an external device feed (`VBusProgram` SQL view) is synced into attendance rows and billable "rounds"
- **Supplier settlement**: monthly accounts, round-based billing (full / half-go / half-return), deductions (Normal/Taxes), payments (Normal/Advance with monthly distribution)
- **Effective-dated repricing** with an approval workflow
- Dashboards and EPPlus Excel import/export

**Key architectural facts (this system ≠ classic clean architecture):**

- **Three projects, inverted layering**: service *interfaces* live in `NewGaras.Infrastructure`, *implementations* in `NewGaras.Domain` — Domain depends on Infrastructure, not vice versa. Infrastructure is a shared kernel (entities + DTOs/VMs + interfaces + UoW).
- **No ASP.NET authentication**: hand-rolled per-action header validation (`CompanyName` + encrypted session-ID `UserToken`), which also **switches the database connection per tenant at runtime**.
- **Envelope responses, always HTTP 200** (mostly): `BaseResponse { Result, Errors[] }`; clients must check `Result`, not the status code.
- **One god-service**: `TransportationLineService` (9.3k lines) implements the entire domain; no transactions; LINQ-only via `IUnitOfWork` repositories.
- **The API contract contains frozen misspellings** (`AddUsersAttedance`, `Longtitud`, `FromoDate`, `DateSerach`, `TypeOfDedct`, …). These are load-bearing — clients (the Flutter app) depend on them byte-for-byte. **Never "fix" a name without versioning the endpoint.**

**Login is not part of `TransportationController`.** Admins and bus supervisors authenticate through the shared **`UserController` / `UserService`** against the central **`User`** table. The token they receive there is the same `UserToken` that every transportation endpoint validates. The login flow and User/role endpoints are documented in **§3.5–§3.7**; the `User`, `UserSession`, and role tables are in the data model (**§5.5**). A bus supervisor is a **`User` (login identity)** whose `Id` is also referenced by a route's `SupervisorId` — see §5.5 for the User↔HrUser distinction.

---

# 2. SOLUTION ARCHITECTURE OVERVIEW

## 2.1 Project Map & Dependency Flow

All projects target **net8.0**. Solution root: `NewGarasAPI.sln`.

```
NewGarasAPI  (ASP.NET Core Web API host)
    │  Controllers (84), Program.cs, Helper (auth), SignalR hub,
    │  wwwroot (attachments, generated Excel files), Quartz jobs
    ↓ depends on
NewGaras.Domain  (≈55 service classes = business logic)
    │  Services/TransportationLineService/, helpers (Encrypt_Decrypt,
    │  Common, LicenseMiddleware), AutoMapper profiles
    ↓ depends on
NewGaras.Infrastructure  (shared kernel)
    │  GarasTestContext (DbContext, ~597 DbSets), 605 entity files,
    │  UnitOfWork + BaseRepository, Models (DTOs/VMs), service INTERFACES,
    │  TenantService, PagedList
    │
EInvoice (legacy e-invoicing client) → NewGaras.Infrastructure
```

**Critical departure from clean architecture:** the dependency arrow between Domain and Infrastructure is inverted. Interfaces (`ITransportationLineService`), view models, and entities all live in Infrastructure; Domain implements them. There is no separate Application or Core project.

| Project | Key packages |
|---|---|
| NewGarasAPI | Swashbuckle, Scalar.AspNetCore, SignalR 8.0.3, EF Core SqlServer 8.0.1, EPPlus 7.1.2, iTextSharp, OpenXml, QRCoder, Quartz 3.18 |
| NewGaras.Domain | AutoMapper 13, EPPlus, ClosedXML, ExcelDataReader, IronPdf, MailKit, Microsoft.Identity.Client, Quartz |
| NewGaras.Infrastructure | EF Core 8.0.1, Quartz, FrameworkReference to ASP.NET Core |

## 2.2 Request Lifecycle (as actually implemented)

```
1. HTTP request arrives with headers: CompanyName, UserToken (+ filters/pagination as headers)
2. Controller action builds an empty BaseResponse[WithData<T>]
3. Action calls _helper.ValidateHeader(Request.Headers, ref _Context)
      → validates CompanyName against a hardcoded whitelist
      → REBINDS the DbContext connection string for that tenant
      → decrypts UserToken (AES, static key) → session row ID
      → checks UserSessions (Active && EndDate > now)
      → returns HearderVaidatorOutput { result, userID, CompanyName, errors }
4. If invalid → response.Result=false with Err-P200/Err-P2, returned as HTTP 200
5. If valid → controller sets service.Validation = validation (current-user context)
6. Controller delegates to ITransportationLineService method
7. Service queries/mutates via IUnitOfWork repositories (LINQ, string includes)
8. Service calls _unitOfWork.Complete() (SaveChanges — NO transactions)
9. Service returns the envelope; controller wraps in Ok(...) or returns it directly
10. Any exception → catch → ErrorCode "Err10", ErrorMSG = raw exception message
```

## 2.3 Startup / DI (`Program.cs` — no Startup.cs)

- **CORS**: default policy `AllowAnyOrigin/AnyHeader/AnyMethod` (fully open)
- ~90 scoped service registrations grouped by module banner comments; `ITransportationLineService → TransportationLineService` under `//____ TransportationLine ____`
- `AddTransient<IUnitOfWork, UnitOfWork>`; AutoMapper assembly scan; SignalR `NotificationsHub` at `/Notifications`
- JSON: `PropertyNamingPolicy = null` → **PascalCase responses** (matches the Flutter client's field names)
- **Quartz `AttendanceJob`** runs every 12h **only when** `AppSettings:ServerName == "PCOMPLAINS"` (machine-name-gated background job for the transportation tenant)
- Swagger + Scalar exposed unconditionally (no environment check)
- Pipeline quirks: `UseHttpsRedirection` appears twice; `UseAuthentication` is placed after `MapControllers` and no auth scheme is registered — it is inert
- `appsettings.json` sections: `ConnectionStrings` (`GarasTest`, `PharmaTransportation`), `AppSettings` (`ServerName`, `baseURL`), `EPPlus` license, `GreenApiSettings` (WhatsApp), `MailSettings`, `TenantSettings` (`Defaults` + `Tenants[]`), `AzureAd`

---

# 3. AUTHENTICATION, USERS & MULTI-TENANCY

There is **no `[Authorize]`, no JWT, no ASP.NET Identity** anywhere. Auth is a homegrown scheme: users log in through `UserController` to obtain an encrypted session-ID token, and every other endpoint validates that token manually via `Helper.ValidateHeader`.

## 3.1 Token Scheme

- **Login** (`UserController.Login` / `LoginForWeb`): password compared via `Encrypt(password)` against the stored **encrypted (reversible!)** password; success inserts a `UserSession` row (via stored proc `proc_UserSessionInsert`, 24-hour `EndDate`); the returned token is `UrlEncode(Encrypt(sessionRowId))`
- **Crypto**: Rijndael/AES-256 with a **static passphrase (`"SalesGarasPass"`) and static IV compiled into the source** (`NewGaras.Domain/Helper/Encrypt_Decrypt.cs`) — used for both tokens and stored passwords
- **Per-request validation** (`NewGarasAPI/Helper/Helper.cs → ValidateHeader`):
  1. `CompanyName` header checked against a **hardcoded whitelist** of ~25 tenant codes (`pharma`, `proauto`, `marinaplt`, `royaltent`, …)
  2. The DbContext connection is **switched at runtime**: `_Context.Database.SetConnectionString(GetConnectonString(CompanyName))` — company → hardcoded connection string (e.g., `pharma` → database `GARASTransportation`)
  3. `UserToken` is URL-decoded, decrypted → numeric session ID → `UserSessions` lookup (`Active && EndDate > now`)
  4. Returns `HearderVaidatorOutput { result, userID, CompanyName, errors }` (type name misspelled — frozen)

## 3.2 Failure Codes (returned inside HTTP 200 envelopes)

| Code | Meaning |
|---|---|
| `Err-P200` | Invalid/unknown CompanyName |
| `Err-P2` | Invalid or expired UserToken |
| `Err10` | Unhandled exception (raw `ex.Message` leaked to client) |
| `Err11` | Not-found / missing Excel template |
| `Err12` | Exceptions inside repricing methods |

> **Client-side note:** the Flutter app string-matches these codes; `Err-P2` matching also catches `Err-P200` (contains-check). Do not add new codes starting with `Err-P2…` unless they mean "session expired".

## 3.3 Tenant Resolution — Two Competing Mechanisms

1. **DI path**: `TenantService` reads the `CompanyName` header via `IHttpContextAccessor`, resolves connection from `TenantSettings` in appsettings (default tenant `pharma`); used by `GarasTestContext.OnConfiguring`
2. **Legacy path**: `Helper.GetConnectonString` — hardcoded switch, applied via `SetConnectionString` inside `ValidateHeader`

Both exist simultaneously and can disagree. The controllers rely on the Helper path. The TransportationController additionally **instantiates its own context by hand** (`_Context = new GarasTestContext(_tenantService)`) alongside the DI-injected UnitOfWork — two context instances per request are normal here.

## 3.4 Authorization

**There is none at the API level.** Any valid session can call any transportation endpoint. Role-based restrictions (Super Admin approve/reject, etc.) are enforced **only in the Flutter client UI**. The only role usage in the service is *notification routing*: on route creation and repricing requests, users in the hardcoded role **"Transportation Super Admin"** receive an in-app notification (SignalR) + email.

## 3.5 Login & Session Flow (`UserController` / `UserService`)

This is where admins and bus supervisors obtain the `UserToken`. `UserController` is **mid-refactor**: the login endpoints keep all logic in the controller (direct `GarasTestContext` + raw stored procedures), while newer read endpoints delegate to `UserService` (UnitOfWork). Roughly 40–50% of both files is commented-out legacy code kept inline.

**`POST /User/Login` (main login, unauthenticated) — exact flow:**
```
1. Body: UserLogin { Email, Password, CompanyName, ExternalLoginFrom? }
2. ValidateCompanyName.ValidateInput(CompanyName) → else Err-P… ; swap DbContext connection
3. Look up User by Email (+ Active). If ExternalLoginFrom == "office365":
      ⚠ password check SKIPPED — email match alone authenticates
   else: compare Encrypt(inputPassword) == User.Password  (reversible encryption, not a hash)
4. If CompanyName == "pharma": hardware-lock license check
      (motherboard/UUID hashes, hardcoded expiry 2027-01-01)
5. Insert session: proc_UserSessionInsert → new UserSession { UserId, Active=1, EndDate = now+24h }
6. Token = UrlEncode(Encrypt(sessionId))  → returned as LoginResponse.Data
7. Side effect: back-fills missing DailyReport rows for users in the "SalesMen" group (skips weekends)
8. Return LoginResponse (see §3.6)
```

**Variants:**
- `POST /User/LoginForWeb` — slimmer login against the `VUserInfos` view; same session proc; returns basic identity fields only.
- `POST /User/LoginForClient` — **client-portal** login (not staff): identifies a `Client` by email or the last 7 digits of mobile; the "password" is a `MaintenanceFors.ProductSerial`; creates a **`ClientSession`** (separate table). Relevant only because `ValidateHeader` accepts `FromClient=true` to check `ClientSessions` instead of `UserSessions`.
- `POST /User/Logout` — decrypts the token, loads the session (`proc_UserSessionLoadByPrimaryKey`), deactivates it (`proc_UserSessionUpdate_DiActivate`). No client-side "remember me" or refresh token exists; the Flutter client stores the raw token (and, per the frontend audit, the plaintext password for its own remember-me).

## 3.6 `LoginResponse` Payload

What a successful staff login returns (consumed by the Flutter client to build its permission flags):

| Group | Fields |
|---|---|
| Identity | `UserID` (encrypted), `UserIDNO` (plain), name, `PhotoUrl`, job title, department, branch, `EmplyeeId`⚠ (the linked `HrUser` id) |
| Session | `Data` = the `UserToken` |
| **Roles** | `RoleList` from view `VUserRoles` → `{ RoleID, RoleName }` — **this is what the transportation role flags are derived from** (§5.6) |
| Groups | `GroupList` from view `VGroupUserBranches` |
| Other | `SpecialityList`, `NotificationCount`, `TaskCount`, open check-ins (`OpenTaskCheckIn`, `OpenAttendanceCheckIn`, `LastWorkingHourCheckIn`), local currency, company profile (owner `Client` where `OwnerCoProfile==true`), `AllowLocationTracking` (from current `ContractDetails`) |

## 3.7 User / Roles Endpoint Inventory (`/User`, 11 endpoints)

Route base `[Route("[controller]")]` → **`/User`** (note: **no `api/` prefix**, unlike `api/Transportation`). "Auth" = calls `ValidateHeader`.

| Verb | Route | Auth | Delegates to | Purpose |
|---|---|---|---|---|
| POST | `/User/Login` | none (creds in body) | Controller (inline) | Main staff login; session + `LoginResponse` |
| POST | `/User/LoginForWeb` | none | Controller | Slimmer login via `VUserInfos` |
| POST | `/User/LoginForClient` | none | Controller | Client-portal login → `ClientSession` |
| POST | `/User/Logout` | header | Controller | Deactivate session |
| GET | `/User/GetUserData` | header | `GetUserData(userID, token)` | Rebuild `LoginResponse` for an active session |
| GET | `/User/GetUserList` | header | `GetUserList` | User DDL; filters BranchId, RoleId, GroupId, CSV JobTitleId, NotActiveUser, WithTeam; includes each user's role & group lists |
| GET | `/User/GetUserWithJobTitleDDL` | header | `GetUserWithJobTitleDDL` | Name-search DDL (space-insensitive), branch/project filters |
| GET | `/User/GetUserTargetDistribution` | header | service | Sales targets (SalesMen group) — not transportation-related |
| POST | `/User/AddSupportRequest` | header | service | Support email (hardcoded recipients) |
| GET | `/User/GetDateTimeEGPZone` | **none** | Controller | Server time in Egypt Standard Time |
| GET | `/User/GetTeamDDL` | **none** | `GetTeamDDL` | All teams as ID/Name |
| GET | `/User/GetEmployeeInfo` | **none** ⚠ | `GetEmployeeInfo` | Full employee profile — **returns the user's DECRYPTED plaintext password** (`Encrypt_Decrypt.Decrypt(...)`) and a Base64 id |

User-module error codes: `Err-P6…Err-P15` (login/validation), `Err101` (required fields), `Err142` (UserId required), plus the shared `Err10`/`E-1` exception passthrough (raw `ex.Message`). Several catch blocks dereference `ex.InnerException.Message` with no null check and can themselves throw.

---

# 4. LAYER-BY-LAYER BREAKDOWN

## 4.1 API Layer — `TransportationController`

```csharp
[Route("api/[controller]")]
[ApiController]
public class TransportationController : ControllerBase
```

- Base route: **`api/Transportation/{action}`**; namespace says `Controllers.TransportationLine` while the folder says `TransportationModule` (mismatch, harmless)
- **Constructor**: injects `IUnitOfWork`, `IWebHostEnvironment` (never used), `ITenantService`, two inventory services (**never stored — dead params**), and `ITransportationLineService`; manually constructs `Helper` and `GarasTestContext`
- **Every action** follows the identical template: build envelope → `ValidateHeader` → on success optionally set `service.Validation = validation` → delegate → catch-all `Err10`
- `Validation` is set on all mutating actions (~30); most GETs and simple deletes don't set it
- **Verbs**: GET 31 / POST 38. **No PUT/DELETE/PATCH** — updates, deletes, and approvals are POSTs with `[FromHeader]` ids
- **Binding convention**: filters, ids, dates, booleans, and pagination (`PageNo`, `NoOfItems`, defaults 1/20) come from **HTTP headers**; bodies are implicit complex types; 4 endpoints use `[FromForm]` (Excel uploads / bulk user creation)
- Two `bool Active = true` params lack a binding attribute → they bind from the **query string**, unlike their header siblings

## 4.2 Service Layer — `TransportationLineService`

```csharp
public class TransportationLineService : ITransportationLineService
// ctor: IUnitOfWork, IMapper (never used), IWebHostEnvironment,
//        GarasTestContext, IHostingEnvironment (legacy, also kept),
//        INotificationService, IMailService, IHrUserService
```

- **Current user**: `public HearderVaidatorOutput Validation { get; set; }` — set by the controller per request; `validation.userID` stamps `CreationBy/ModifiedBy/ApprovedBy`
- **No tenant awareness inside the service** — tenancy is entirely the connection-string switch
- **No transactions.** Pattern: mutate → `_unitOfWork.Complete()`. Several methods save multiple times mid-flow (partial-write risk): `AddTransportationRoute`, `ModifyPriceOfTransportationLine`, `CreateHrUserWithAllRoutes`, `AddUsersAttedanceUpdated` (saves inside a loop)
- **Data access**: 100% LINQ through `IUnitOfWork` repositories; eager loading via string include arrays (`new[] { "TransportationVehicleRoute.TransportationVehicle.VehicleType" }`); no raw SQL or stored procedures; one SQL view (`VBusProgram`)
- **Validation style**: errors are *added* to the envelope but often **do not short-circuit** — a request can return `Result = true` *and* a populated `Errors` list (e.g., `AddRoutesForEmployee` reports a duplicate but inserts anyway)
- **Method versioning by numeric suffix**: superseded generations are kept live side-by-side — `AddUsersAttedance44/66/Updated/AlterAddUsersAttendance`, `DashBoard22`, `AccountsAllMonths/22`, `DashBoardForAttendence/22/88/Duration`. The controller decides which generation a route maps to (e.g., route `AddUsersAttedance44` → service `AddUsersAttedance66`)

## 4.3 Infrastructure Layer

- **DbContext**: `GarasTestContext` — single context, ~597 `DbSet`s, DB-first partial class, `Arabic_CI_AS` collation, connection resolved per request
- **UnitOfWork**: `NewGaras.Infrastructure/UnitOfWorkClass/` — hundreds of `IBaseRepository<TEntity, TKey>` properties (all 13 transportation repos + `VBusProgram` view); `BaseRepository<T,DT>` wraps `Set<T>()` with `GetAll/GetById/FindAll/FindAllQueryable(predicate, string[] includes)/Add/Update/Delete/Complete` (sync + async)
- **Models/VMs**: `NewGaras.Infrastructure/Models/TransportationLineModel/` (~37 view models)
- **Pagination**: `PagedList<T>.Create(IQueryable, PageNo, NoOfItems)` → `PaginationHeader { CurrentPage, TotalPages, ItemsPerPage, TotalItems }` returned **inside the response body** (`BaseResponseWithDataAndHeader<T>`), not as HTTP headers

## 4.4 Response Envelopes

| Type | Shape | Used for |
|---|---|---|
| `BaseResponse` | `{ Result: bool, Errors: [{ErrorCode, ErrorMSG}] }` | writes |
| `BaseResponseWithData<T>` | + `Data: T` | single reads, file URLs |
| `BaseResponseWithDataAndHeader<T>` | + `PaginationHeader` | paginated lists |

- Envelope-returning actions always yield **HTTP 200** — even auth failures. `IActionResult` actions wrap success in `Ok(...)` and exceptions in `BadRequest(response)` (HTTP 400 for server faults)
- Clients must branch on `Result`, never on the status code

---

# 5. DATA MODEL (Transportation Entities)

13 entities + 1 view, all in the flat `NewGaras.Infrastructure/Entities/` folder. **Misspelled names are frozen** (marked ⚠).

```
TransportationLine 1──N TransportationVehicleRoute N──1 Supplier
                                   │ N──1 SupplierContactPerson ("driver")
                                   │ N──1 TransportationVehicle N──1 VehicleType
                                   │ N──1 HrUser (SupervisorId)
                                   │ N──1 BranchSchedule (shift)
                                   ├──N TransportationVehicleRouteDirection (stations)
                                   ├──N TransportationVehicleRouteEmployee (memberships)
                                   ├──N TransportationVehicleRouteEmployeeException
                                   ├──N TransportationVehicleRouteDeduction
                                   ├──N TransportationVehicleRouteAccount (monthly rollup)
                                   ├──N TransportationRoundForRoute (per-round ledger)
                                   └──N TransportionLineExceptionPrice ⚠ (price history)
TransportationLineIncreaseRequest 1──N TransportationLineIncreaseRequestLine (repricing)
TransportionLineSupplierPayment ⚠ 1──N DistributionSupplierPayment (advance allocation)
TransprotationUserAttedance ⚠ (attendance rows — linked by Serial STRINGS, no FK)
VBusProgram (SQL view — external fingerprint feed)
```

| Entity | Key columns | Notes |
|---|---|---|
| `TransportationLine` | Id, LineName, audit | |
| `TransportationVehicleRoute` | Id, TransportationLineId, TransportationVehicleId, SupplierId?, SupplierContactPersonId?, BranchScheduleId?, SupervisorId?, **Serial** (string), PeriodFrom/To, **FromoDate**⚠/ToDate (scheduled go/return times), NameOfRoute, **LineCost** (decimal), **OneWay** (bool?), Active, IsApproved?, audit | Serial auto-generated: last serial + 1, **seed `"70000000"`** |
| `TransportationVehicle` | VehicleTypeId, Capacity, Active, IsApproved, ApprovedBy (string) | created `IsApproved=false` |
| `VehicleType` | Type, Active | |
| `TransportationVehicleRouteDirection` | RouteDirection, Description, Active, Latitude, **Longtitud**⚠ | stations |
| `TransportationVehicleRouteEmployee` | HrUserId, DirectionId, FromDate/ToDate, **Period** ("Go"/"Return"/"Both"), DurationLatitude, **DurationLongtitud**⚠, Active | passenger membership |
| `TransportationVehicleRouteEmployeeException` | HrUserId, target RouteId, DirectionId?, FromDate/ToDate + **DayName** (English weekday) OR ExceptionDate, Period, Latitude/**Longtitud**⚠, ReasonException, ContactNumber | two shapes: recurring or single-day |
| `TransprotationUserAttedance`⚠ | **Type** ("Person"/"Bus"), **Serial** (Person → HrUser.**Email**; Bus → route Serial), CheckIn, CheckOut, OneWayDate, CheckIn/OutRouteId, coords, CreationDate/By | **no FK — string-matched** |
| `TransportationVehicleRouteAccount` | RouteId, SupplierId, Serial, **DateOfMonth**, CountOfRounds, PriceOfRounds?, DeductPerRounds | monthly rollup; matched by **month only (year ignored)** in several methods |
| `TransportationRoundForRoute` | RouteId, SupplierId, Serial, CheckInDate/CheckOutDate/OneWayDate, **PriceRound?** | per-round ledger; newest sync generation leaves PriceRound **null** |
| `TransportationVehicleRouteDeduction` | SupplierId?, RouteId, Serial, DateOfDeduction, DeductPerRound, Cause, **TypeOfDedct**⚠ ("Normal"/"Taxes") | |
| `TransportationLineIncreaseRequest`(+`Line`) | IsPercent, IncreaseCost, ForAllLines, **ApproximateToFiveFlag**, StartDate, Approve, ApprovedBy/Date | repricing workflow |
| `TransportionLineExceptionPrice`⚠ | RouteId, Price, FromDate | effective-dated route price history |
| `TransportionLineSupplierPayment`⚠ (+`DistributionSupplierPayment`) | SupplierId, Payment, DatePayment, StartDate?, NumberOfMonths?, **TypeOfDebt** ("Normal"/"Advance") | advance → N distribution rows |
| `VBusProgram` (view) | EmployeeNumber, CheckDate (**string**), CheckTime (**string**), Type ("I"/"O"), BusType | external feed; dates parsed with fallback `DateTime.Now.Date` |

**Semantic surprises to know before touching anything:**
- `HrUser.Email` stores the **numeric employee/fingerprint number**, not an email — it is the join key to attendance `Serial` (via `long.Parse`)
- Religion is stored in `MaritalStatus.Name` (`"C"` / `"M"`) and surfaced as christian/muslim counts
- Route `FromoDate`/`ToDate` are **times of day** for go/return trips, not validity dates (those are `PeriodFrom`/`PeriodTo`)
- `ApproveRoute` toggles **`Active`**, not `IsApproved`

## 5.5 Identity, Session & Role Tables (the login side)

These live outside the transportation entity set but are what admins and bus supervisors authenticate against. `User` is the ERP-wide **audit anchor**: its class file (`Entities/User.cs`) is ~2,000 lines, almost entirely EF navigation collections pointing at nearly every other module's `CreatedBy`/`ModifiedBy` — only ~19 columns are actual scalar fields.

```
User (login identity) 1──N UserRole N──1 Role          (view: VUserRoles → RoleId + RoleName)
                      1──N GroupUser N──1 Group         (view: VGroupUserBranches)
                      1──N UserSession                  (active token rows)
                      1──1 HrUser (HrUser.UserId → User.Id)   ← supervisor / employee link
Client (portal, separate) 1──N ClientSession            (FromClient=true path)
```

| Table | Key columns | Notes |
|---|---|---|
| **`User`** (login table) | **Id** (long, PK), **Password** (⚠ reversibly **encrypted**, not hashed), **Email** (login id), FirstName / MiddleName / LastName, Mobile, **Active** (bool — the login gate; there is **no separate "IsUser" flag**), Gender, Age, **BranchId**, DepartmentId, JobTitleId, PhotoUrl, CreatedBy, CreationDate, ModifiedBy, Modified, OldId | The identity of every admin, supervisor, and back-office user. No `CompanyName` column — tenancy is by database, not by row |
| `UserSession` | Id, UserId, Active, StartDate, **EndDate** (login sets now + 24h), (token derived from Id) | Created by `proc_UserSessionInsert`; validated by `ValidateHeader`; the encrypted `Id` **is** the `UserToken` |
| `ClientSession` | Id, ClientId, Active, EndDate | Parallel table for the client portal (`FromClient=true`) — not staff |
| `UserRole` | Id, **UserId**, **RoleId**, Active | Join table: attaches roles to a user |
| `Role` / view `VUserRoles` | RoleId, RoleName (+ UserId in the view) | The `RoleList` in `LoginResponse` comes from `VUserRoles` |
| `GroupUser` / view `VGroupUserBranches` | Id, UserId, GroupId, Active | Groups (e.g., "SalesMen", "Transportation Super Admin") |
| `HrUser` | Id, **UserId** (→ User.Id), Email (=fingerprint number), names, MaritalStatusId, ImgPath, coords, Active | The **employee** record. A bus supervisor is a `User` **and** an `HrUser`; routes reference the supervisor by `SupervisorId` (which the transportation service treats as an `HrUserId`) |

### User ↔ HrUser distinction (important for the transportation module)

- **`User`** = a login identity (email + password + roles). Admins and bus supervisors have one.
- **`HrUser`** = an employee/passenger record. Every rider is an `HrUser`; not every `HrUser` has a `User` login.
- The link is `HrUser.UserId → User.Id`.
- A route's `SupervisorId` is stored as an **HrUser id**; the mobile "my routes to supervise" query (`getTransportationRouteByHrUser`) matches routes where `SupervisorId == HrUserId`. So a supervisor logs in as a **User**, but the routes find them by their **HrUser** id. When creating a supervisor account both records must exist and be linked, or their routes won't appear on mobile.

## 5.6 How Transportation Roles Are Resolved

The API does **not** enforce transportation roles. At login, `LoginResponse.RoleList` (from `VUserRoles`) carries every `{ RoleID, RoleName }` attached to the user via `UserRole`. The **Flutter client** maps those role IDs to its permission flags. The role IDs are hardcoded on the client side (`lib/DataAccessLayer/constants.dart`), and the same numbers exist as `Role` rows in the database:

| Role | ID | Grants (enforced client-side only) |
|---|---|---|
| System Admin | 137 | everything (mapped to both `systemAdmin` and `isSystemAdmin` flags) |
| Transportation Line Admin | 210 | lines/routes/stations/passengers, payments, deductions, create repricing |
| Transportation Admin | 213 | create suppliers/lines/routes |
| Transportation Supervisor | 214 | mobile attendance for own routes |
| Transportation Passenger | 215 | read-only |
| Transportation Super Admin | 216 | full control; approve/reject repricing; the notification target on the server |
| Transportation Reader | 221 | read-only |
| Add Supplier | 30 | create supplier |

> **Server-side gap:** because none of these are checked in the controller or service, any authenticated `User` — regardless of role — can call any transportation endpoint directly (approve repricing, bulk-change prices, delete routes). The role IDs above gate the **UI**, not the API. This is the single most important authorization finding (§10.1).

---

# 6. ENDPOINT REFERENCE (69 endpoints)

All routes relative to **`api/Transportation/`**. Auth = `CompanyName` + `UserToken` headers on every call. Pagination = `PageNo`/`NoOfItems` headers (defaults 1/20). "Env" column: B = `BaseResponse`, D = `BaseResponseWithData<T>`, H = `BaseResponseWithDataAndHeader<T>`.

## 6.1 Vehicle Types

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `getAllVehicleType` | — | D | ⚠ runs an unused `VBusProgram` full read **before** auth validation |
| GET | `Bus_Program` | — | D | debug/smoke-test of the fingerprint view |
| POST | `AddVehicleType` | body `VehicleTypeVM` | B | |
| POST | `UpdateVehicleType` | body `VehicleTypeVM` | B | |
| POST | `DeleteVehicleType` | header `Id` | B | **hard delete, no reference check** |

## 6.2 Lines

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `getAllTransportationLine` | — | D | returns `{Id, Name, RouteNum}`; per-line count query (N+1) |
| POST | `AddTransportationLine` | body `TransportationLineVM` | B | no duplicate check |
| POST | `UpdateTransportationLine` | body | B | LineName only |
| POST | `DeleteTransportationLine` | header `Id` | B | **hard delete** (soft delete commented out) |
| GET | `getAllRoutes` | headers `RouteId, Name, TransportionlineId, SupplierId, supplierContactPersonId, serialBus, PageNo, NoOfItems`; query `Active` | H | despite the name, pages **routes** as `TransportationLineVM` |

## 6.3 Routes

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `AddTransportationRoute` | body `TransportationVehicleRouteVM` | B (async) | accepts LineId OR new LineName (creates line inline); serial = last+1 (seed 70000000); notifies "Transportation Super Admin" (SignalR + email) |
| POST | `UpdateTransportationRoute` | body `TransportationRouteUpdateVM` | B | full overwrite incl. IsApproved; only finds Active routes |
| POST | `DeleteTransportationRoute` | header `Id` | B | blocked if passengers exist (`"يجب مسح الموظفين اولا في هذا الخط"`); else hard-deletes stations + route |
| POST | `ApproveRoute` | headers `Id, Approve` | B | sets **Active** (not IsApproved) |
| GET | `getAllTransportationRoute` | same filter set as getAllRoutes | H | includes per-route capacity math for **today (Egypt time)**; ⚠ filters gate only the error message, **not** the paged results |
| GET | `getTransportationRoute` | header `RouteId` | D | single route detail + C/M religion counts |
| GET | `getTransportationRouteByHrUser` | header `HrUserId` | D | routes **supervised** by this user (mobile app entry) |
| GET | `getTransportationRouteDetails` | headers `RouteId, FromDate, ToDate` | D | attendance/absence counts in window |

## 6.4 Stations (Directions)

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `AddTransportationDirection` | body `TransportationDirectionDto` + header `TransportationVehicleRouteId` | B | bulk insert; mixed body+header binding |
| POST | `UpdateTransportationDirection` | body | B | |
| POST | `DeleteTransportationDirection` | header `Id` | B | blocked if assigned to a passenger (`"المحطة مسجلة لمستخدم"`) |
| GET | `GetTransportationDirection` | header `routeId` | D | |

## 6.5 Passengers (Route Users)

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `AddTransportationEmployee` | body + header `TransportationVehicleRouteId` | B | closes any existing open membership (ToDate=now, Active=false) before adding |
| POST | `AddRoutesForEmployee` | body `AddRoutesForHrUserDto` + header `HrUserId` | B | ⚠ duplicate reports an error **but inserts anyway** with Result=true |
| POST | `UpdateTransportationEmployee` / `UpdateRouteEmployee` | body | B | overwrite membership |
| POST | `DeleteTransportationEmployee` / `DeleteRouteEmployee` | header `Id` (long / int) | B | duplicate endpoints, both hard delete |
| GET | `GetTransportationEmployeeList` | header `RouteId` | D | photo URL = `baseURL + "/" + ImgPath`; field `FirstNane`⚠ |
| GET | `GetTransportationEmployees` | headers `RouteID, Period, DateSearch?` | D | "who rides today": members − excepted-out ∪ excepted-in (`RegisteredInline=false`); ⚠ bug: `Longtitud = DurationLatitude` |
| GET | `getTransportationRoutesForHrUser` | header `HrUserId` | D | passenger's subscriptions |
| GET | `GetCapacityNumbers` | headers `RouteID, Period, DateSearch?` | D | see §7.2 |
| POST | `CreateHrUserWithAllRoutes` | **form** `CreateHrUserWithAllRoutesDto` | B (async) | delegates to HrUserService then links routes; only endpoint passing userID+CompanyName explicitly |

## 6.6 Attendance

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `AddUsersAttedance` | body `TransprotationUserAttedanceVM` | B | manual check-in/out (mobile app); `Type` "Person"/"Bus"; Bus upserts monthly account — see §7.3 quirks |
| POST | `AddUsersAttedance44` | — | B | batch fingerprint sync; ⚠ route says 44, calls service `AddUsersAttedance66` |
| GET | `AlterAddUsersAttendance` | — | B (async) | ⚠ **state-changing GET**; newest sync generation |
| POST | `AddUsersAttedanceUpdated` | — | B | older sync generation, still live |

## 6.7 Exceptions

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `GetEmployeeException` | headers `HrUserId?, Id?, PageNo, NoOfItems` | H | |
| POST | `AddException` | body = **EF entity** `TransportationVehicleRouteEmployeeException` | B | ⚠ over-posting risk; requires route+user AND (coords OR direction) |
| POST | `UpdateException` | body = EF entity | B | ⚠ also overwrites CreationDate/By |

## 6.8 Deductions

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `AddDeduction` | body = **EF entity** `TransportationVehicleRouteDeduction` | B | also accumulates into monthly account `DeductPerRounds` (month-only match) |
| POST | `UpdateDeduction` | body = EF entity | B | ⚠ account row NOT re-synced after edit |
| GET | `getAllDeduction` | headers `Id, FromDate?, ToDate?, SupplierId, RouteId, PageNo, NoOfItems` | H | fields `SupplierContentPersonId(Name)`⚠ |

## 6.9 Repricing

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| POST | `ModifyPriceOfTransportationLine` | body `ModifyTransportationLinePriceVM` (field `IsPrecent`⚠) | B (async) | creates request `Approve=false` (+ per-route lines); notifies Super Admins — ⚠ except when `ForAllLines=true` (no notification) |
| GET | `getAllModifyPriceOfTransportationLine` | headers `Approve?, PageNo, NoOfItems` | H | before/after preview; ⚠ percent preview shows the increase, not the total |
| POST | `UpdatePriceOfTransportationLine` | header `Id` | B | **approve**: applies new LineCost + writes `TransportionLineExceptionPrice` history row (FromDate = request StartDate) — see §7.4 |
| POST | `RejectUpdatePrice` | header `Id` | B | sets Approve=false + ApprovedBy — ⚠ indistinguishable from "pending" |
| GET | `UpdateRoutePrice` | — | B | ⚠ **state-changing GET**: batch-overwrites every route's LineCost from its **earliest** effective exception price (inconsistent with calculators, which use latest) |

## 6.10 Costs & Supplier Accounts / Payments

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `ReportCostOfLines` | headers `TransportionlineId, SupplierId, supplierContactPersonId, dateSearch?, dateFrom?, dateTo?, serialBus, PageNo, NoOfItems` | H | service method is `CostOfLine`; **filters are mutually exclusive** (else-if chain); NetCost math §7.5 |
| GET | `ReportCostOfLinesExcell` | same | D(string) | Excel URL; fixed filename, overwritten each run; backslash URL separators |
| GET | `AccountsAllMonthsForSupplier` | headers `Year, SupplierId, RouteId, Month, PageNo, NoOfItems` | H | calls `AccountsAllMonths22`; the monthly statement — §7.5 |
| GET | `AccountsAllRoundsForSupplier` | same | H | per-round ledger; rounds 1 / 0.5; ⚠ only method that **rethrows** exceptions; PaginationHeader never set |
| GET | `getAllSupplierPayment` | headers `SupplierId, FromDate?, ToDate?, PageNo, NoOfItems` | H | embeds advance distribution rows |
| POST | `AddSupplierPayment` | body = **EF entity** `TransportionLineSupplierPayment` | B | Advance → N `DistributionSupplierPayment` rows of `Payment/NumberOfMonths`; Normal must have no StartDate/months |

## 6.11 Dashboards

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `DashBoard` | headers `RouteId, TransportionlineId, SupplierId, supplierContactPersonId, DateSerach?, serialBus` | D | calls `DashBoard22`; **requires DateSerach** (`"يرجى إدخال التاريخ"`); KPI counts + attendance percentages |
| GET | `DashBoardForAttendence` | above + `FromDate?, ToDate?, AttendaceFlag?, HrUser, PageNo, NoOfItems` | H | per-person attendance list; heavy N+1; ⚠ post-pagination filter can return short pages |
| GET | `DashBoardForAttendenceDuration` | same | H | per-user `AttendanceHistory` day list; C# method `DashBoardForAttendence99` |

## 6.12 Excel Import/Export & Bulk Users

| Verb | Route | Input | Env | Notes |
|---|---|---|---|---|
| GET | `AttendanceExcell` | dashboard filter set + `Year` | D(string) (async) | sheet per line, RTL, Arabic headers ✔/✘, **timestamped filename** |
| GET | `AttendanceExcellDur` | same | D(string) (async) | fixed filename (overwritten); ⚠ accepts `TransportionlineId` but **never forwards it** to the service |
| POST | `InsertPharmaExcel` | **form** `InsertHrUserExcelVM` | D(string) (async) | bulk HrUser import; **Excel Serial column → stored as `HrUser.Email`**; errors → `Logs/PharmaErrorLog.txt` URL |
| POST | `InsertUsersWithRoutesExcel` | **form** `IFormFile` | D(string) (async) | users + up to 4 route links (cols 7/9/11/13); ⚠ first invalid row **stops** the whole import (blank row = EOF marker) |
| POST | `InsertUserNotActiveExcel` | **form** `IFormFile` | D(string) (async) | ⚠⚠ **calls the same service method as InsertUsersWithRoutesExcel** (copy-paste bug); the intended activate/deactivate service method also has an inverted null-check that NREs on unknown users |
| GET | `downloadExcelUserActiveTemplete` | — | D(string) | template from `wwwroot/Attachments/PharmaTemplate/`; `Err11` if missing |
| GET | `downloadExcelUserWithRoutesTemplete` | — | D(string) | route lookup list hidden at **row 1000, cols 27–28** (Excel data-validation source) |

---

# 7. BUSINESS RULES & CALCULATIONS

## 7.1 Periods & Ridership

- Membership `Period` ∈ `"Go" | "Return" | "Both"`; all period filters use `Period == X || Period == "Both"`
- **Who rides route R on date D** = active members whose `FromDate ≤ D ≤ ToDate` (or open) and period matches, **minus** members with an exception elsewhere that day, **plus** exceptions into R (matched by date window + `DayName == D.ToString("dddd")` English weekday, or exact `ExceptionDate`)

## 7.2 Capacity

```
FullCapacity              = vehicle.Capacity
CapacityWithoutExpection  = active in-window members matching Period
ExpectionNumFromOtherLines = exceptions INTO this route that day
RouteEmployeesToOtherLines = members excepted OUT that day
ActualCapacity = (CapacityWithoutExpection + ExpectionNumFromOtherLines)
                 − RouteEmployeesToOtherLines
```
"Today" is **Egypt Standard Time** (`TimeZoneInfo.ConvertTimeFromUtc`). Capacity is informational — nothing blocks over-capacity.

## 7.3 Attendance & Round Recognition (the heart of billing)

**Manual (mobile):** `AddUsersAttedance` — `CheckInOrCheckOut=true` always creates a check-in row; `false` updates today's row (matched by Type+Serial+date) or creates a checkout-only row. For `Type="Bus"` it upserts the monthly `TransportationVehicleRouteAccount` — ⚠ month matched **ignoring year**; ⚠ `CountOfRounds` can **double-increment** (conditional then unconditional `++`); ⚠ price ternary is **inverted** vs. the batch jobs (`OneWay ? LineCost/2 : LineCost`).

**Batch fingerprint sync** (current generation `AddUsersAttedance66`, exposed via route `AddUsersAttedance44`; newest refactor `AlterAddUsersAttendance` on a GET):
1. Read `VBusProgram` logs since the last attendance `CreationDate` (string dates parsed with formats `dd/MM/yyyy HH:mm:ss` etc.; **parse failure falls back to today**)
2. Match employee by `HrUser.Email == EmployeeNumber` (email holds the fingerprint number)
3. `Type="I"` (in): match a route where `|log time − route.FromoDate.TimeOfDay| ≤ 59 min`; one check-in per person/day
4. `Type="O"` (out): close an open record with `CheckIn ≥ log − 14h`; route matched within 30 min of `ToDate`
5. **A bus round is recognized when `checkInCount > capacity / 2`** (more than half the seats fingerprinted) → creates a `Type="Bus"` attendance row + a `TransportationRoundForRoute` row + upserts the monthly account
6. ⚠ Generation 66 leaves `PriceRound = null` (pricing now resolved at read time via exception-price history); `CreationBy = 1` hard-coded; debug stubs remain (`if (logDateTime.Date == new DateTime(2026,9,3)) { }`, `if (route.Id > 154) { }`); the newest refactor hard-codes `PriceOfRounds = 1` with the comment `// مش صح` ("not correct")
7. The Quartz `AttendanceJob` triggers this sync every 12h, only on the server named `PCOMPLAINS`

## 7.4 Repricing (approval workflow)

1. **Create** (`ModifyPriceOfTransportationLine`): request stored `Approve=false` with `IsPercent`, `IncreaseCost`, `ForAllLines`, `ApproximateToFiveFlag`, `StartDate` (+ per-route lines when not for-all); Super Admins notified — ⚠ *not* when `ForAllLines=true`
2. **Approve** (`UpdatePriceOfTransportationLine`):
   - fixed: `newCost = LineCost + IncreaseCost`
   - percent: `newCost = LineCost + (LineCost × IncreaseCost / 100)`
   - `ApproximateToFiveFlag`: round to nearest multiple of 5 — **remainder ≥ 2.50 rounds up**
   - writes new `LineCost` AND inserts a `TransportionLineExceptionPrice { RouteId, Price, FromDate = StartDate }` history row
3. **Reject** (`RejectUpdatePrice`): `Approve=false` + ApprovedBy — ⚠ state-identical to pending
4. **Effective-dated pricing at read time**: `CalculatePricePerRouteFromExceptionPrice(routeId, date)` = latest history row with `FromDate ≤ date`, else current `LineCost`; **two-way routes bill each round at price/2**

## 7.5 Supplier Settlement Math

**Monthly statement** (`AccountsAllMonths22`, grouped per supplier+month+year):
```
round price(r)         = CalculatePricePerRouteFromExceptionPrice(route, r.date)
                         halved when route is two-way
CountOfcompleteRounds  = twoWayRounds.Count / 2
TotalDue               = completeRoundsDue + halfGoDue + halfReturnDue
TotalDeduct            = TotalNormalDeduct ("Normal") + TotalTaxesDeduct ("Taxes")
TotalDueAfterPaid      = TotalDue − (TotalDeduct + TotalPaidNormal + TotalPaidadvance)
```
- **Month attribution hack**: `CheckOutDate`/`OneWayDate` are shifted **−16 hours** (one older method −14h) so an after-midnight return counts in the departure month
- **Advance payments**: `AddSupplierPayment` with `TypeOfDebt="Advance"` requires StartDate + NumberOfMonths and creates N `DistributionSupplierPayment` rows of `Payment / NumberOfMonths` each; Arabic validation error otherwise
- **Deductions**: `AddDeduction` writes the row AND accumulates the monthly account's `DeductPerRounds`; ⚠ `UpdateDeduction` does not re-sync the accumulated value

**Cost report** (`CostOfLine`):
```
two-way: countOfRounds = checkIns + checkOuts ; NetCost = (LineCost/2) × rounds − deductions
one-way: countOfRounds = checkIns             ; NetCost =  LineCost   × rounds − deductions
```
⚠ loop variables are captured outside the lambda — a route with no matching rows can inherit the previous route's counts.

## 7.6 Dashboard KPIs

`DashBoard22`: vehicle/line/type/supplier counts within the filter scope; riders = distinct active members; attendance matched by intersecting attendance `Serial` (parsed long) with riders' `HrUser.Email` (parsed long) — ⚠ throws if an email is non-numeric. Percentages: `checkInBuses / TwoWayVehiclesNum × 100`, `attendedRiders / HrUsersNum × 100` (numerator zero-guarded, denominator not).

---

# 8. CONVENTIONS (follow these when extending the module)

## 8.1 Adding a New Endpoint — the As-Built Workflow

1. **VM/DTO** → `NewGaras.Infrastructure/Models/TransportationLineModel/` (prefer a dedicated VM over binding an EF entity — the existing entity-bound endpoints are a known defect, don't copy it)
2. **Interface method** → `ITransportationLineService.cs` (Infrastructure)
3. **Implementation** → `TransportationLineService.cs` (Domain): return an envelope type; stamp audit fields from `validation.userID`; use `_unitOfWork.X.FindAllQueryable(predicate, includes)` + `PagedList<T>.Create` for lists; call `Complete()` once at the end
4. **Controller action** → copy the standard template (envelope → `ValidateHeader` → set `Validation` for writes → delegate → catch `Err10`)
5. No DI registration needed (the service is already registered); no migration workflow — the DB is **DB-first** (entities scaffolded from the existing SQL Server database)

## 8.2 Naming & Contract Rules

- Routes are verb-prefixed action names (`getAll…`, `Add…`, `Update…`, `Delete…`, `Approve…`) with **inconsistent casing** — match the exact existing casing when the Flutter client already calls it
- Writes are POST; reads are GET (except the two legacy state-changing GETs — never add another)
- Filters/ids/pagination ride in **headers**; keep `PageNo`/`NoOfItems` names and 1/20 defaults
- Responses are PascalCase JSON envelopes; pagination inside the body
- **Never rename existing routes, parameters, or JSON fields** — the misspellings in §9 are the contract

## 8.3 File Outputs

Excel files are generated with EPPlus (`LicenseContext = Commercial`) under `wwwroot/` (`TransportLineExcel/`, `TransportLineCostExcel/`, `Logs/`) and returned as `Globals.baseURL`-based URL strings — mostly with **backslash** separators (the Flutter client tolerates this). Prefer timestamped filenames (as `AttendanceExcell` does) over fixed names that are overwritten per request (as `AttendanceExcellDur`/`ReportCostOfLinesExcell` do — a concurrency hazard).

---

# 9. QUIRKS CATALOG — FROZEN CONTRACT MISSPELLINGS

These names appear in routes, JSON payloads, entities, and columns. **They are the API surface; fixing them is a breaking change.**

| Misspelling | Where |
|---|---|
| `TransprotationUserAttedance` | attendance **entity/table** + repository |
| `AddUsersAttedance` (+`44`/`Updated`), `TransprotationUserAttedanceVM` | routes + DTO |
| `HearderVaidatorOutput` | auth validation output type |
| `Longtitud`, `DurationLongtitud`, `CheckInLongtitud`, `CheckOutLongtitud` | coordinate columns everywhere |
| `FromoDate` | route go-time column + VMs |
| `DateSerach` (also `dateSearch`, `DateSearch` — three spellings coexist) | filter headers |
| `TransportionlineId`, `TransportionlineName`, `transportionLineNameVn`, `TranspotionLineName` | params + VMs |
| `TransportionLineSupplierPayment`, `TransportionLineExceptionPrice` | **entities** |
| `TypeOfDedct` | deduction column |
| `AttendaceFlag`, `ISAttendace`, `Attedance`, `ActualUserAttedance`, `DashBoardForAttendence` | params, VM fields, routes |
| `Expection` (`ExpectionNumFromOtherLines`, `CapacityWithoutExpection`) | capacity VM |
| `FirstNane` | `EmployeeBasicInfoVM` |
| `IsPrecent` | repricing request VM (entity is `IsPercent`) |
| `SupplierContentPersonId(Name)` | deduction VM |
| `BuSupervisorId` | route VM |
| `AttendanceExcell`, `ReportCostOfLinesExcell`, `downloadExcelUser…Templete`, `"The Templete File is not exsists"` | routes + error string |
| `Bus_Program` | snake_case route |
| `EmplyeeId` | `LoginResponse` field (the linked HrUser id) |
| `Mounth`, `resuilt`, `supplierContactPersionDb` | internal names (safe to fix — not serialized) |

**Magic values that are business rules:**
- Serial seed `"70000000"`; bus-round threshold `> capacity/2`; fingerprint windows 59/60/120 min (in), 30 min (out), 14h/22h (open record); month-attribution shift −16h; advance months distribution `Payment/N`; round-to-5 threshold 2.50; role literal `"Transportation Super Admin"`; religion codes `"C"`/`"M"` in MaritalStatus; `CreationBy = 1` in batch jobs; Quartz gate `ServerName == "PCOMPLAINS"`

---

# 10. KNOWN ISSUES & TECHNICAL DEBT REGISTER

### Transportation module
1. **No authorization** — any valid session can call any endpoint, including approvals and bulk price changes; role checks exist only in the client UI (see §5.6)
2. **Static AES key + IV in source** (`"SalesGarasPass"`) for both tokens and (reversible) stored passwords
3. **Raw exception messages returned to clients** (`Err10`) — information disclosure
4. **EF entities bound directly from request bodies** (`AddException`, `UpdateException`, `AddDeduction`, `UpdateDeduction`, `AddSupplierPayment`) — over-posting/mass-assignment
5. **State-changing GETs**: `AlterAddUsersAttendance`, `UpdateRoutePrice` (crawler/prefetch-triggerable bulk writes)
6. **Pre-auth query** in `getAllVehicleType` (unused full view read before header validation)
7. File uploads have no extension/size/content-type checks in the controller; CORS fully open; Swagger/Scalar exposed in production

### User / login module (`UserController` / `UserService`)
8. **Passwords are reversibly encrypted, not hashed** — a database read (or a bug) exposes every plaintext password; the same static key decrypts them all
9. **`GET /User/GetEmployeeInfo` returns the user's DECRYPTED plaintext password over the wire** — and it skips `ValidateHeader`, so it is **unauthenticated**
10. **Office365 login path (`ExternalLoginFrom == "office365"`) performs no password/token check** — knowing a valid email is sufficient to log in
11. **Unauthenticated endpoints**: `GetEmployeeInfo`, `GetTeamDDL`, `GetDateTimeEGPZone` never call `ValidateHeader`
12. **Predictable token**: the `UserToken` is just `Encrypt(sessionRowId)` with the static key — session IDs are sequential, so tokens are guessable if the key ever leaks
13. **Hardcoded license/hardware lock** (motherboard/UUID hashes, expiry 2027-01-01) and hardcoded support-email recipients embedded in code
14. **Plaintext password persisted client-side** by the Flutter "remember me" (per the frontend audit) — compounds issue #8

## 10.2 Correctness Bugs (verified in code)

1. `InsertUserNotActiveExcel` **calls the wrong service method** (`InsertUsersWithRoutesExcel`) — the not-active import runs the with-routes import; the intended method also has an inverted null-check (`if (user != null) → "Id Not Found"`) guaranteeing an NRE path
2. `AddRoutesForEmployee` inserts the duplicate it just reported as an error
3. `AddUsersAttedance` (manual, Bus): month matched ignoring year; `CountOfRounds` double-increment; OneWay price ternary inverted vs. batch jobs
4. `getAllRoutes.RouteNum` compares a line id to a route id; `getAllTransportationRoute` filters don't filter the paged output
5. `GetTransportationEmployees` returns `Longtitud = DurationLatitude`
6. `CostOfLine` leaks loop-captured counters between rows; repricing percent **preview** shows the increase instead of the new total
7. `RejectUpdatePrice` produces a state identical to "pending"; `UpdateRoutePrice` uses the *earliest* effective price while calculators use the *latest*
8. `AttendanceExcellDur` silently drops its `TransportionlineId` filter; `(DateTime)DateSerach` cast NREs when null
9. `SuppliersAccount` never assigns its computed data to the response (always returns empty); `AccountsAllRoundsForSupplier` rethrows instead of returning the envelope and never sets its PaginationHeader
10. `UpdateDeduction` does not re-sync the monthly account's accumulated `DeductPerRounds`

## 10.3 Design Debt

- 9.3k-line god service / 2.4k-line controller / 597-DbSet single context / 605-file flat entity folder
- Numeric-suffix method generations (`22/44/66/88/99/Updated/Dur`) shipped side-by-side; controller-to-service name mismatches (`AddUsersAttedance44 → 66`, `DashBoard → DashBoard22`, `DashBoardForAttendenceDuration → DashBoardForAttendence99`)
- No transactions around multi-step writes; multiple `Complete()` calls mid-flow
- Pervasive N+1 (per-row subqueries, two queries per round in settlement, dashboard-per-line Excel)
- Attendance linked by parsing strings (`Serial` ↔ `HrUser.Email`) instead of foreign keys
- Two tenant-resolution mechanisms; hand-`new`ed DbContext in the controller alongside DI
- Dead code: unused injected services, unused `_mapper`, `DeviceIdentifier.GetUniqueId()` discarded, `IQueryable` leaked through the interface (`GetVehicleRoutes`), debug date stubs

---

# 11. QUICK REFERENCE CARD

```
Login:           POST /User/Login  { Email, Password, CompanyName }  → LoginResponse.Data = UserToken
                 (password = reversibly encrypted; office365 path skips the check)
                 LoginResponse.RoleList → client maps role IDs to permissions
Base URL:        {host}/api/Transportation      (note: /User has NO api/ prefix)
Auth headers:    CompanyName: <tenant code>     UserToken: <urlencoded AES(sessionId)>
Session:         UserSession row, EndDate = login + 24h ; Logout deactivates it
Pagination:      PageNo / NoOfItems headers (defaults 1 / 20); header returned in body
Success check:   response.Result == true   (HTTP status is almost always 200)
Session expiry:  Errors[].ErrorCode ∈ { Err-P2, Err-P200 }  → re-login
Server errors:   Err10 (+ raw exception text), Err11 (not found/template), Err12 (repricing)
Login errors:    Err-P6…Err-P15, Err101 (required), Err142 (UserId required)
Identity:        User = login (Id, Email, Password, Active, BranchId, roles via UserRole)
                 HrUser = employee (HrUser.UserId → User.Id) ; route.SupervisorId = HrUser id
Roles (client):  137 SysAdmin · 210 LineAdmin · 213 Admin · 214 Supervisor ·
                 215 Passenger · 216 SuperAdmin · 221 Reader · 30 AddSupplier
Timezone:        business "today" = Egypt Standard Time; audit dates = server local
Periods:         "Go" | "Return" | "Both"      Deductions: "Normal" | "Taxes"
Payments:        "Normal" | "Advance" (+ StartDate + NumberOfMonths → monthly distribution)
Bus round:       recognized when fingerprint check-ins > vehicle capacity / 2
Route price:     effective-dated via TransportionLineExceptionPrice; two-way rounds = price/2
Statement:       TotalDueAfterPaid = TotalDue − (Deductions + NormalPaid + AdvancePaid)
```

---

*Generated from as-built code analysis of the Garas Core API Transportation module. Companion frontend documentation: `docs/` (PRD, SCREENS, API, MODELS, PROVIDERS, RULES, ARCHITECTURE) and `docs/second-pass/` in the Flutter repository.*
