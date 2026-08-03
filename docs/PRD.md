# Product Requirements Document — Transportation System Module

**Product:** Garas App (Flutter)
**Module:** `lib/GUI/screens/transportationSystem`
**Document type:** Business Requirements & Product Requirements (PRD)
**Status:** As-built (reverse-engineered from the current implementation)
**Last updated:** 2026-07-05

**Companion documents:** [SCREENS.md](SCREENS.md) · [API.md](API.md) · [MODELS.md](MODELS.md) · [PROVIDERS.md](PROVIDERS.md) · [RULES.md](RULES.md) · [DEPENDENCIES.md](DEPENDENCIES.md) · [ARCHITECTURE.md](ARCHITECTURE.md)

---

## 1. Overview

The Transportation System is an **employee (staff) transportation management** module inside the Garas ERP/HR platform. It lets an organization plan bus/vehicle routes for its employees, assign employees to those routes and pickup stations, supervise daily boarding via a mobile app (GPS check‑in/check‑out), and manage the **financial relationship with external transportation suppliers (contractors)** — computing what is owed per round driven, applying deductions and taxes, recording payments, repricing routes, and producing Arabic invoices/reports.

Passengers are **HR users (employees)**, not external customers. Drivers are modeled as **supplier contact persons** — data subjects, not app logins. The UI is Arabic-first (RTL), multi-branch aware, and role-gated.

### 1.1 Problem Statement
Organizations that transport large numbers of employees via contracted vehicles need to:
- Know **who** rides **which** bus from **which** pickup point, and whether they actually boarded.
- Track vehicle **capacity vs. actual occupancy**, including employees temporarily borrowed to/from other lines (exceptions).
- Pay contractors **accurately** based on rounds actually driven, minus agreed deductions and taxes, with an auditable, approvable trail.
- Give management **real-time visibility** (attendance %, vehicle utilization, cost per line) across branches.

### 1.2 Goals
- Single source of truth for lines → routes → stations → passengers.
- Reliable, GPS-backed attendance capture on mobile for supervisors.
- Automated round-based cost accrual and supplier settlement with an approval workflow for price changes.
- Management dashboard with attendance and utilization KPIs, filterable and exportable.

### 1.3 Non-Goals
- Public/consumer ticketing or fare collection (passengers are employees; there is no rider payment).
- Live GPS fleet tracking / turn-by-turn navigation (only point check-in/out coordinates are captured).
- Payroll integration or driver payroll (only supplier settlement is in scope).
- Route optimization / automatic seat allocation (assignment is manual).

---

## 2. Scope

### 2.1 In Scope (Sub-modules)

| # | Sub-module | Folder | Purpose |
|---|-----------|--------|---------|
| 1 | **Lines** | `transportation_lines` | Top-level grouping of routes; create/edit/approve lines |
| 2 | **Routes (Web)** | `transportation_routes/transportation_routes_web` | CRUD routes, stations, and passenger assignments |
| 3 | **Routes (Mobile)** | `transportation_routes/transportation_routes_mob` | Supervisor daily attendance (bus + passenger, GPS) |
| 4 | **Passengers** | `transportation_passenger` | Create passenger profiles; assign passengers to routes; Excel bulk load |
| 5 | **Vehicles** | `transportation_vehicles` | Register vehicles & vehicle types; approve; capacity |
| 6 | **Shifts (Working Days & Hours)** | `shiftTransportation` | Per-branch weekday working-hour windows |
| 7 | **Exceptions** | `transportation_exception` | Temporary route/station/period changes per employee; capacity view |
| 8 | **Suppliers & Financials** | `supplierTransportation` | Suppliers, monthly account statements, payments, distribution, invoices |
| 9 | **Costs** | `transportationCosts` | Line/route cost report with net cost & Excel export |
| 10 | **Repricing** | `transportation_repriceing` | Percentage/fixed price changes with approval workflow |
| 11 | **Deductions** | `transportation_deduction` | Normal & tax deductions against supplier dues |
| 12 | **Dashboard** | `transportation_dashboard` | KPIs, attendance list, per-passenger history, touch attendance |

### 2.2 Platform Scope
- **Web** — full administrative surface (lines, routes, passengers, vehicles, shifts, exceptions, financials, dashboard).
- **Mobile** — focused supervisor workflow: view "my routes to supervise" and take bus + passenger attendance. Mobile intentionally exposes only **Go / Return** periods (no "Both").

### 2.3 Cross-cutting
- **Multi-branch:** shifts and several queries are scoped by `branchId` from `UserProvider`.
- **Localization:** Arabic-first RTL; PDF invoices rendered in Arabic with company logo and signature lines.
- **Pagination:** list endpoints return a `PaginationHeader` (currentPage, itemsPerPage, totalItems, totalPages).

---

## 3. Users, Roles & Permissions

Roles are resolved from `UserProvider` role flags. Most **write** actions (add/edit/delete/approve, payments, deductions, repricing) are gated; other users are effectively **read-only**. See [RULES.md](RULES.md) for the full permission matrix.

| Role | Capabilities |
|------|--------------|
| **System Admin** (`systemAdmin` / `isSystemAdmin`) | Superset of all; approves lines, vehicles, and repricing |
| **Transportation Super Admin** (`transportationSuperAdmin`) | Full transportation control; add payments/deductions; create repricing; **approve & reject** repricing; approve lines/vehicles |
| **Transportation Line Admin** (`transportationLineAdmin`) | Add/edit lines, routes, stations, passengers; add payments/deductions; create repricing (cannot approve/reject repricing) |
| **Transportation Admin** (`transportationAdmin`) | Create suppliers, lines, routes (add tiles visible) |
| **Transportation Supervisor** (`transportationSupervisor`) | The field/bus supervisor; core actor of the **mobile attendance** flow for routes they supervise |
| **Transportation Reader / Passenger** (`transportationReader`, `transportationPassenger`) | Read-only views |
| **Add Supplier** (`addSupplier`) | Generic permission to create a supplier |

**Non-login data actors:**
- **Driver** = a supplier's contact person (`SupplierContactPerson`) surfaced as "driver" on routes and reports.
- **Passenger** = an HR employee assigned to a route; the passive subject of attendance.

> There is **no parent/guardian role** — this is a staff transportation system, not a school system.

---

## 4. Domain Model (Business Entities)

```
Branch
  └── Shift (weekday working-hour windows)
Line ──< Route >── Supplier
              │        └── Contact Person (Driver)
              ├── Vehicle (─ Vehicle Type)
              ├── Supervisor (HR user)
              ├──< Station (Direction)
              └──< Passenger assignment (HR user @ station, period, validity window)
Route ──< Round (daily) ── Attendance (check-in/out, GPS)
Supplier ──< Monthly Account ── Deductions / Payments / Repricing
Exception (per employee: temporary route/station/period/location change)
```

Field-level details for every entity are documented in [MODELS.md](MODELS.md).

### 4.1 Line
Top-level grouping; a line contains many routes.
- `id`, `lineName`, `routesNum` (count of child routes)
- Has an **approval** concept (approve by super/system admin).
- *(Legacy, now disabled in UI: lineCost, currency, active, oneWay.)*

### 4.2 Route
A specific contracted bus trip belonging to a line.
- **Identity/link:** `id`, `serial` (bus serial, used for attendance), `lineId`/`lineName`, `branchScheduleId` (shift), `nameOfRoute`
- **Supplier/vehicle:** `supplierId`/`supplierName`, `supplierContactPersonId`/`Name` (**driver**), `transportationVehId`/`Name`, `lineCost`
- **Supervision:** `busSupervisorId`/`busSupervisor` (HR user)
- **Timing:** `periodFrom` (start date; defaults to 2025‑07‑01 if unset), `periodTo` (nullable end), `fromDate` (arrival time), `toDate` (departure time), `oneWay`
- **Occupancy (server-computed, read-only):** `fullCapacity`, `usersInRoute`, `actualCapacity`, `actualUserAttendance`, `passengerFromOtherLines`, `passengerToOtherLines`, `stationsNum`
- **Demographics:** `muslimNum`, `christianNum` (displayed as "M:" / "C:" chips)

### 4.3 Station (Direction)
A pickup/drop point on a route ("station" and "direction" are used interchangeably).
- `id`, `routeDirection` (station name), `description` (time/notes), `latitude`, `longitude`, `active`
- Linked to a route via `TransportationVehicleRouteId`

### 4.4 Passenger (Route User) & Assignment
Passenger = HR employee.
- **Profile:** `id`, `hrUserId`, `firstName`, `middleName`, `lastName`, `mobile`, `email`, `photo`, station coords, `directionId`/`directionName`
- **Mobile extras:** `registeredInLine`, `checkIn`, `checkOut`, live `latitude`/`longtitud`
- **Assignment (join record):** `routeId`, station (`transportationVehicleRouteDirectionId`), `period`, validity window (`fromDate`/`toDate`), custom pickup coordinates (`DurationLatitude`/`DurationLongtitud`), `active`

### 4.5 Vehicle & Vehicle Type
- **Vehicle:** `id`, `vehicleTypeId`/`vehicleTypeName`, `capacity` (seat count), `isApproved`/`approvedBy`, `active`, `transportationLines[]`, audit. *(No plate-number field.)*
- **Vehicle Type:** `id`, `type` (name), `active`, `transportationVehicles[]`

### 4.6 Shift (Working Days & Hours)
- `ShiftsData`: `shiftNumber` groups a set of weekday entries
- `Shift`: `weekDayId` (1=Sunday … 7=Saturday), `from`/`to` (HH:mm), `active`, `branchId`, audit
- Default window **12:00–18:00**; only active days are persisted

### 4.7 Exception
A temporary, per-employee deviation from the normal route/station/period.
- Passenger (`hrUserId`/`hrUserName`), target route/line, exception station (`...DirectionId/Name`)
- **Type A — single date:** `exceptionDate`, `exceptionDatePeriod`
- **Type B — period/range:** `fromDate`, `toDate`, `dayName` (weekdays)
- `period` (Go/Return/Both), coordinates (`latitude/longtitud`, `latitudeExceptional/longtitudExceptional`)
- `reason`, `contactNumber`, `active`, audit

### 4.8 Capacity (per route + period)
- `fullCapacity`, `actualCapacity`, `capacityWithoutExpection`, `expectionNumFromOtherLines` (inflow), `routeEmployeesToOtherLines` (outflow)

### 4.9 Supplier & Financial Entities
- **Supplier:** reuses the general supplier entity (name, email, phones, fax, specialities, contact persons/drivers); duplicate-checked on create.
- **Monthly Account (`AccountsAllMonthForSupplier`):** one row per supplier per month — round counts (`countOfRounds`, `countOfHalfGoRounds`, `countOfHalfReturnRounds`), dues (`totalDue` and per round-type), deductions (`totalDeduct`, `totalTaxesDeduct`, `totalNormalDeduct`), payments (`totalPaidadvance`, `totalPaidNormal`), `totalDueAfterPaid` (remaining), `note`.
- **Daily Round (`TransportationDaysInMonthDate`):** `nameOfRoute`, `dateOfRound`, `dateOfCheckIn`, `dateOfCheckOut`, `dateOfOneWay`, `roundsNum`, `totalPriceOfDay`.
- **Payment (`AllSupplierPayments`):** amount, dates, `typeOfDebt` (Advance / Normal), `numberOfMonths`, and an advance **distribution** across months (`{payment, monthNum, yearNum}`).
- **Deduction (`TransportationDeduction`):** supplier, route, serial, driver, `deductionAmount`, `typeOfDeduction` (Taxes / Normal), `routePrice`, reason, date window, creator.
- **Repricing (`ModifiedPriceData`):** `isPercent`, `increaseCost`, `approximateToFiveFlag`, `forAllLines`, approval fields, and per-line before/after detail (`priceBefore` → `priceAfter`).
- **Cost report (`TransportationReportCostData`):** `lineCost`, `onWay`, `countOfRounds`, `deductionRoundNum`, `deductionPerRound`, `netCost`.

### 4.10 Enumerations
- **Period / route type:** `['Go', 'Return', 'Both']` — hard-coded client list. "Both" ⇒ `isGetInAndOut = true`. **Mobile hides "Both".**
- **Payment type (`typeOfDebt`):** `Advance` | `Normal`
- **Deduction type:** `Taxes` | `Normal`
- **Weekday id:** 1=Sunday … 7=Saturday

---

## 5. Functional Requirements & User Flows

### 5.1 Lines
**FR-L1** Admins can create a line (name required), edit (rename), delete, and — for super/system admin — **approve** a line (only when not already approved).
**FR-L2** From a line, the user can navigate to its routes; a line shows its `routesNum`.

**Flow — Create Line:** Lines screen → *Add new line* → enter name → save → list refreshes.

### 5.2 Routes (Web)
**FR-R1** Admins can create, edit, delete a route, view a **Route Details** popup, and open the route detail screen (stations + passengers).
**FR-R2** Required fields on create: Route Name, Shift, Cost (numeric), Supplier, Vehicle, and From/To time depending on route type. Driver and Supervisor are optional. Line is required only in the free-text-line branch.
**FR-R3** Route type is mutually exclusive: **round trip (ذهاب وعودة)** / **go (ذهاب)** / **return (عودة)**. Selecting *go* clears To-time; *return* clears From-time. `oneWay = !roundTrip`.

**Flow — Create Route:** Routes/Lines screen → *Add new route* → pick line (or free-text), fill required fields → pick supplier → then driver → pick vehicle/supervisor → choose type & times → save.

### 5.3 Stations
**FR-S1** Within a route, admins can add/edit/delete a station: Name (required), Description/Time, Active, Latitude/Longitude (numeric) or **Pick Location** on a map.
**FR-S2** Coordinates can be copied to clipboard; a station can be viewed on a map.

### 5.4 Passengers
**FR-P1** Create a transportation passenger profile (name, identity number, mobile, marital/"another identifier", map location).
**FR-P2** **Assign to route (from route):** pick passenger, station, and period (required) → save.
**FR-P3** **Assign to routes (from passenger):** add multiple rows; each row = Route (required), Station, Period (required), From/To validity dates, and custom pickup coordinates or map pick. Existing rows are read-only until *Edit*; only new rows (empty id) are posted on submit.
**FR-P4** **Bulk load:** download an Excel template of users-with-routes, fill, and upload (success toast on completion).

### 5.5 Attendance (Mobile — core operational flow)
**FR-A1** A supervisor opens the app and sees **only routes they supervise** (empty ⇒ "no routes to supervise"). Route users load defaulting to the **Go** period.
**FR-A2** **Bus/vehicle attendance:** capture GPS → record check-in/out against the **bus serial** (`type='Bus'`).
**FR-A3** **Passenger attendance:** in **Go**, if no check-in, show *Check In* → capture GPS → record (`type='Employee'`, checkIn=true); otherwise show the recorded time. In **Return**, show check-in status and offer *Check Out* → capture GPS → record (checkIn=false).
**FR-A4** Location services required; coordinates default to 0.0 if unavailable. A recorded check-in shows a time instead of a button (prevents duplicates). A map view plots all passenger station coordinates.

### 5.6 Vehicles
**FR-V1** Admins add a vehicle (Capacity required, Type required, Active optional), edit, and delete (with warning).
**FR-V2** Vehicle types can be created inline (name only; created active).
**FR-V3** Vehicles start **unapproved**; super/system admin can **Approve** (separate from Active). Card color: green = approved+active, orange = approved+inactive, red = not approved.

### 5.7 Shifts (Working Days & Hours)
**FR-SH1** Per branch, create shift groups (auto-numbered). Each group has 7 weekday slots (default 12:00–18:00, all inactive).
**FR-SH2** Toggle each day active/inactive, set per-day from/to, or bulk "set time for all". Only **active** days are persisted.
**FR-SH3** Delete a shift group = deactivate all its days.

### 5.8 Exceptions
**FR-E1** Admins add/edit an exception: select Passenger and Route (both required).
**FR-E2** Choose type — **single Exception Date** or **Period (From/To + weekdays)** — and period value (Go/Return/Both).
**FR-E3** Choose location mode — **Station** (pick from route stations) or **Location** (map lat/lng). "Both" collects both get-in and get-out points.
**FR-E4** Enter Reason and Contact Number (digits only).
**FR-E5** When route + period are selected, a **Route Details / Capacity** popup shows capacity math for that route+period.

### 5.9 Suppliers & Financials
**FR-F1** Browse/filter suppliers; add supplier (duplicate-checked on name/email/phone/mobile/fax).
**FR-F2** **Account Statement:** filter by supplier/route/month/year; per month view Details (rounds), Add Payment, Add Deduction, Show Payments, **Print Invoice** (Arabic PDF).
**FR-F3** **Add Payment:** amount + date; choose **Advance** (also number-of-months ≤ 12 + start date, produces a per-month distribution) or **Normal (monthly)**.
**FR-F4** View all payments and, for an advance, its distribution across months.

**Flow — Pay a supplier:** Account Statement → month row → *Add Payment* → enter amount/date → choose Advance (months + start date) or Normal → *Pay* → statement reloads.

### 5.10 Costs
**FR-C1** Line Cost Report filterable by line, supplier, driver, route, serial, date range (defaults to current month).
**FR-C2** Shows per line-route: line cost, one-way flag, rounds count, deducted-rounds count, deduction total, and **net cost**. Exportable to Excel.

### 5.11 Repricing (with approval)
**FR-RP1** Create a repricing: scope **all lines** or **selected lines**; mode **percent** or **fixed amount**; optional **round to nearest 5**; a start date. Created **unapproved**.
**FR-RP2** View list filtered by approved / not-approved; open a record to see per-line **before → after** prices.
**FR-RP3** **Approve** (super/system admin) applies new prices and stamps approver/date. **Reject** (super admin only) discards.

### 5.12 Deductions
**FR-D1** Add/edit a deduction: select Supplier then Route (auto-fills bus serial and route price, read-only).
**FR-D2** Choose **Normal** vs **Taxes**; choose **percentage** (deduction = routePrice × percent ÷ 100, computed client-side, per (half) round) or fixed amount.
**FR-D3** Enter Date and Reason (required). On save, the deductions list and the supplier month account both refresh (deduction immediately reduces the month's remaining).

### 5.13 Dashboard
**FR-DB1** Show KPI cards (see §7), filterable by supplier → driver → serial → date, and by selecting a route in the left rail.
**FR-DB2** KPI cards navigate to their detail screens (Lines, Routes, Vehicles, Employees, Suppliers).
**FR-DB3** **Attendance view:** list passengers with latest attendance (check-in/out or "Absent") for a date range, filter by line/supplier/driver/route/serial and Attended/Absent, open per-passenger history, and download Excel.
**FR-DB4** **Take Electronic Touch Attendance** (admin only).

---

## 6. Business Rules

The authoritative, expanded rule catalog lives in [RULES.md](RULES.md). Summary:

### 6.1 Operational
- **Period semantics:** Go governs check-in; Return governs check-out; "Both" collects get-in and get-out points. Mobile offers only Go/Return.
- **Route type ⇒ oneWay:** round trip ⇒ `oneWay=false`; go-only or return-only ⇒ `oneWay=true`.
- **Attendance integrity:** an existing check-in shows a time (no duplicate capture); GPS captured per event, defaulting to 0.0 when unavailable.
- **Capacity math:** net capacity accounts for cross-line exception **inflow** (`expectionNumFromOtherLines`) and **outflow** (`routeEmployeesToOtherLines`); values are server-computed and display-only (no client-side over-capacity block observed).
- **Shifts:** weekdays 1–7 (1=Sunday), default 12:00–18:00, only active days saved, bound to branch, numbers auto-increment.

### 6.2 Financial
- **Round-based billing:** each day's activity is classified as **full round**, **half-go round**, or **half-return round** (from check-in/out/one-way timestamps), each priced separately and summed into `totalDue`.
- **Two payment types:** Advance (installment prepayment spread across N≤12 months from a start date → server produces a monthly distribution) vs Normal (single monthly payment).
- **Two deduction types:** Normal and Taxes, tracked separately (`totalNormalDeduct` vs `totalTaxesDeduct`) and summed into `totalDeduct`.
- **Percent deduction:** `routePrice × percent ÷ 100`, applied per (half) round.
- **Remaining balance:** `totalDueAfterPaid ≈ totalDue − totalDeduct − totalPaidNormal − totalPaidadvance` (server-computed; reflected in the printed invoice summary).
- **Net cost:** `netCost` = line cost after deducted rounds (`deductionRoundNum` × `deductionPerRound`).
- **Repricing:** percent or fixed, optionally rounded to nearest 5, scoped globally or per selected lines, effective from a start date, and **must be approved** before prices take effect.

### 6.3 Validation
- Required: Line name; Route (name, shift, cost-numeric, supplier, vehicle, times per type); Station name; Passenger assignment (route, period); Deduction (date, reason); Exception (passenger, route, reason, contact-number digits-only).
- Advance payment months capped at 12.
- Supplier creation is duplicate-checked (name/email/phone/mobile/fax).
- Default date windows: cost report and invoices default to the current calendar month (1st → last day); year defaults to current year.

---

## 7. Dashboard KPIs

Values come from `TransportationDashboardData` (all filterable by supplier/driver/serial/date and route selection):

| KPI card | Fields |
|----------|--------|
| Lines & Routes | `transportLinesNum`, `vehiclesNum` |
| Vehicles / Types | `vehicleTypeNum` (also shown as "Total Capacity" — see §9) |
| Employees | `hrUsersNum` (of `allHrUsersNum`) |
| Suppliers | active `suppliersNum` / all `allSuppliersNum` |
| Vehicle Attendance | check-in / check-out / one-way counts |
| Vehicle Percent | check-in % / check-out % / one-way % |
| Users Attendance | `hrUsersAttendanceNum` |
| Users Attendance % | `hrUsersPercent` |
| Routes | round-trip `twoWayVehiclesNum` / one-way `oneWayVehiclesNum` |

Attendance status per passenger is derived from `attendanceHistory` (presence of check-in ⇒ attended, else absent).

---

## 8. Reporting Outputs
- **Supplier Monthly Account Statement** — on-screen table + **Arabic PDF invoice** (company logo, trips table, financial summary: Total Due / Total Deductions / Taxes / Normal Deduction / Advance Payment / Normal Payment / Remaining, notes, supplier + company signature lines).
- **Daily/Month Rounds Detail** — per-route days with check-in/out, one-way, rounds count, total price of day.
- **Line Cost Report** — per line-route cost & net cost, **Excel export**.
- **Attendance Report** — per-passenger attendance over a date range, **Excel export**.

---

## 9. Known Issues / Data Caveats
*(Flagged for backlog; observed in current code.)*
- **Route detail religion chips:** the route detail header renders `christianNum` for **both** the "C:" and "M:" chips — the Muslim count is not shown (display bug).
- **Dashboard "Total Capacity":** the vehicle-count card and the "Total Capacity" subtitle both bind to `vehicleTypeNum`, so total seat capacity is not a distinct metric in the current data contract.
- **Legacy line fields** (cost/currency/active/oneWay) remain in models but are disabled in the UI.
- **Mobile bus-attendance button labels** map "attended" → check-out and "leave" → check-in (a swap worth verifying against intended semantics).

---

## 10. Key Technical References
- **Provider (business logic & API payloads):** `lib/Providers/transportation_system_provider.dart` — see [PROVIDERS.md](PROVIDERS.md)
- **Roles:** `lib/Providers/user_provider.dart`
- **Models:** `lib/DataAccessLayer/model/transportation/` — see [MODELS.md](MODELS.md)
- **Screens:** `lib/GUI/screens/transportationSystem/<sub-module>/` — see [SCREENS.md](SCREENS.md)
- **Endpoints:** `lib/DataAccessLayer/end_points_constant.dart` — see [API.md](API.md)

---

## 11. Glossary
| Term | Meaning |
|------|---------|
| **Line** | Top-level grouping of routes |
| **Route** | A specific contracted bus trip on a line |
| **Station / Direction** | A pickup/drop point on a route |
| **Passenger** | An HR employee assigned to a route |
| **Driver** | A supplier's contact person operating a route |
| **Supplier** | External transportation contractor |
| **Round** | One trip; classified full / half-go / half-return for billing |
| **Period** | Go / Return / Both — trip direction |
| **Exception** | Temporary per-employee route/station/period change |
| **Advance vs Normal** | Prepaid installment vs single monthly supplier payment |
| **Reprice** | Bulk % or fixed price change to routes/lines, requires approval |
