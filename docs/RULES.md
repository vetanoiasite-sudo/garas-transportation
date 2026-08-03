# Transportation Module — Business Rules

**Module:** `lib/GUI/screens/transportationSystem` + `lib/Providers/transportation_system_provider.dart`
**Status:** As-built (reverse-engineered from the current implementation)
**Last updated:** 2026-07-05

This document is the consolidated rule catalog for the transportation module: who may do what, what the client validates, how money is computed, and the workflow/approval rules. It complements [PRD.md](PRD.md) (requirements) and [PROVIDERS.md](PROVIDERS.md) (implementation).

---

## 1. Roles & Permission Rules

Role flags are boolean fields on `UserProvider` (`lib/Providers/user_provider.dart`), resolved from the logged-in user's role IDs:
`isSystemAdmin`, `transportationSuperAdmin`, `transportationLineAdmin`, `transportationAdmin`, `transportationSupervisor`, `transportationReader`, `transportationPassenger`, plus the generic `addSupplier` flag.

Screens gate UI elements with `Visibility`/`if` checks on these flags — **enforcement is client-side UI gating**; there is no client-side route guard beyond hiding entry points, so the server is assumed to enforce authorization on the API.

### 1.1 Permission matrix

| Action | System Admin | Super Admin | Line Admin | Trans. Admin | Supervisor | Reader/Passenger |
|---|---|---|---|---|---|---|
| View dashboard, statements, deductions | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ (read-only) |
| Add/edit/delete line, route, station, passenger assignment | ✔ | ✔ | ✔ | (add tiles visible) | — | — |
| Approve line | ✔ | ✔ | — | — | — | — |
| Add/edit vehicle & vehicle type | ✔ | ✔ | ✔ | — | — | — |
| Approve vehicle | ✔ | ✔ | — | — | — | — |
| Manage shifts (working days/hours) | ✔ | ✔ | ✔ | — | — | — |
| Add payment / deduction | ✔ | ✔ | ✔ | — | — | — |
| Create repricing | ✔ | ✔ | ✔ | — | — | — |
| Approve repricing | ✔ | ✔ | — | — | — | — |
| Reject repricing | — | ✔ | — | — | — | — |
| Create supplier | ✔ | (via `addSupplier`) | — | ✔ | — | — |
| Take Electronic Touch Attendance (dashboard) | ✔ | ✔ | — | ✔ | — | — |
| Mobile attendance (bus + passenger check-in/out) | — | — | — | — | ✔ (own routes only) | — |

### 1.2 Drawer navigation gates (web)

From `lib/GUI/drawer/webDrawer/web_drawer_transportation.dart`:
- **Employees** entry: `transportationAdmin || transportationSuperAdmin || systemAdmin`
- **Users** entry: `transportationSuperAdmin || transportationAdmin || systemAdmin`
- **Transportation Management** section: `transportationSuperAdmin || systemAdmin || transportationLineAdmin || transportationAdmin`
  - **Shifts** (nested): `transportationSuperAdmin || systemAdmin || transportationLineAdmin`
  - **Supervisor & Admin** (nested): `transportationSuperAdmin || systemAdmin || transportationAdmin`
- Dashboard, Deductions, Suppliers Account Statement, Exceptions, Repricing entries: visible to all module users.

### 1.3 Data-visibility rules
- A **supervisor sees only routes they supervise** on mobile (`getTransportationRoutesByHrUserId(userId)`); empty list ⇒ "no routes to supervise".
- A passenger can edit **their own profile photo only** (owner check: `hrUserInfo.data.id == userData.userIdNo`).
- Several queries are **branch-scoped** via `branchId` from `UserProvider` (shifts in particular).

---

## 2. Operational Rules

### 2.1 Periods & route types
- Period enumeration is a **hard-coded client list**: `Go`, `Return`, `Both`.
- **Mobile hides "Both"** — supervisors operate on Go/Return only; route users load defaulting to **Go**.
- Selecting "Both" on assignment/exception sets `isGetInAndOut = true` (both get-in and get-out points collected).
- Route type checkboxes are **mutually exclusive**: round-trip (ذهاب وعودة) / go (ذهاب) / return (عودة).
  - Go-only clears the To-time; Return-only clears the From-time.
  - `oneWay = !roundTrip` (round-trip ⇒ `oneWay=false`; go-only or return-only ⇒ `oneWay=true`).

### 2.2 Attendance
- Attendance is recorded per event with **GPS coordinates**; device location is requested with permission and **defaults to 0.0** when unavailable.
- Two attendance subject types: `type='Bus'` (recorded against the **bus serial**) and `type='Employee'` (against `hrUserId`).
- **Go period governs check-in** (`checkInOrCheckOut=true`); **Return governs check-out** (`checkInOrCheckOut=false`).
- **No duplicate capture:** once a check-in/out exists, the button is replaced by the recorded time.
- Dashboard attendance status is derived: presence of a check-in in `attendanceHistory` ⇒ *Attended*, else *Absent* (red).
- Known caveat: the mobile bus-attendance dialog maps the "Attended" button to check-**out** and "Leave" to check-**in** — verify intended semantics before relying on labels.

### 2.3 Capacity
- Capacity numbers are **server-computed and display-only**; the client never blocks over-capacity assignment.
- Net capacity math accounts for cross-line exceptions: inflow `expectionNumFromOtherLines`, outflow `routeEmployeesToOtherLines`, plus `fullCapacity`, `actualCapacity`, `capacityWithoutExpection`.
- The capacity popup is shown per **route + period** (selected in the exceptions flow).

### 2.4 Shifts (working days & hours)
- Weekday IDs: **1=Sunday … 7=Saturday**.
- New shift groups are **auto-numbered**; each group has 7 weekday slots defaulting to **12:00–18:00, inactive**.
- **Only active days are persisted**; deleting a group = deactivating all its days.
- Shifts are bound to the current **branch** (`branchId`).

### 2.5 Exceptions
- An exception must reference a **passenger and a route** (both required).
- Exactly one of two shapes:
  - **Single date:** `exceptionDate` + `exceptionDatePeriod`, or
  - **Period:** `fromDate` + `toDate` + selected weekdays (`dayName`).
- Location mode is either a **station** on the target route or a **free map location** (lat/lng); "Both" period collects get-in **and** get-out points.
- `reason` is required; `contactNumber` accepts **digits only**.

### 2.6 Assignment lifecycle
- Passenger-to-route assignment requires **route and period**; station, validity dates, and custom pickup coordinates are optional.
- In the multi-row assignment dialog, saved rows are **read-only until "Edit"**; the Edit→Submit button enables only when **change detection** finds a real modification; only **new rows (empty id)** are posted on bulk submit.
- Deleting a saved assignment calls the API; deleting an unsaved row only removes it locally.

---

## 3. Financial Rules

### 3.1 Round-based billing
- Each day's activity per route is classified from its timestamps into **full round** (check-in + check-out), **half-go round**, or **half-return round**, each priced separately.
- Monthly due: `totalDue` = Σ(round-type count × round-type price), materialized per supplier per month in `AccountsAllMonthForSupplier`.

### 3.2 Payments
- Two payment types (`typeOfDebt`): **Advance** and **Normal**.
- **Advance:** amount is spread across `numberOfMonths` (**maximum 12**) starting from a chosen month; the server produces a per-month distribution (`{payment, monthNum, yearNum}`) viewable in the distribution screen.
- **Normal:** a single payment applied to one month.

### 3.3 Deductions
- Two deduction types: **Normal** and **Taxes**, tracked separately (`totalNormalDeduct`, `totalTaxesDeduct`) and summed into `totalDeduct`.
- Amount is entered as a **fixed value** or a **percentage**; percent deduction is computed client-side: `deductionAmount = routePrice × percent ÷ 100`, applied **per (half) round**.
- Selecting the route auto-fills bus serial and route price as **read-only** fields.
- A saved deduction immediately refreshes both the deductions list and the supplier's monthly account (reduces remaining).

### 3.4 Balances & net cost
- Remaining per month: `totalDueAfterPaid ≈ totalDue − totalDeduct − totalPaidNormal − totalPaidadvance` (server-computed; shown on screen and on the printed invoice).
- Cost report: `netCost` = line cost minus deducted rounds (`deductionRoundNum × deductionPerRound`).

### 3.5 Repricing workflow (approval required)
1. **Create** (line admin or above): scope = all lines or selected lines; mode = **percent** or **fixed amount**; optional **round to nearest 5** (`approximateToFiveFlag`); effective start date. Record is created **unapproved**.
2. **Review**: list filterable by approved/not-approved; detail view shows per-line `priceBefore → priceAfter`.
3. **Approve** (super/system admin): applies the new prices and stamps approver + date.
4. **Reject** (super admin only): discards the repricing.
- Prices **never change without approval**.

### 3.6 Other approval rules
- **Lines**: approve action available to super/system admin, only when not already approved.
- **Vehicles**: created **unapproved**; approval (super/system admin) is independent of the Active flag. Card color encodes state: green = approved+active, orange = approved+inactive, red = not approved.

---

## 4. Validation Rules (client-side)

| Form | Required / validated fields |
|---|---|
| Line | Line name |
| Route | Route name, shift, cost (numeric/double-only), supplier, vehicle; From/To time depending on route type; line required in the line-selection branch. Driver & supervisor optional |
| Station | Station name ("station_name_cannot_be_empty"); lat/lng double-format only |
| Passenger profile | Name, identity number, mobile (digits only), marital/"another identifier" |
| Passenger→route assignment | Route ("please_select_route"), period ("please_select_period"); dates/coordinates optional |
| Exception | Passenger, route, reason; contact number digits-only |
| Deduction | Supplier, route, date, reason; amount or percent (numeric) |
| Payment | Amount, date; if Advance: number of months (≤ 12) and start month |
| Supplier | Duplicate-checked server-side on name/email/phone/mobile/fax before create |
| Vehicle | Capacity (required), vehicle type (required); Active optional |
| Dashboard/attendance filters | Serial fields digits-only; driver dropdown requires a supplier to be selected first (snackbar otherwise) |

**Defaults:**
- Cost report and invoice date windows default to the **current calendar month** (1st → last day); year defaults to current year.
- Route `periodFrom` defaults to **2025-07-01** when unset.
- Dashboard list pagination defaults: page 1, 20 items per page.

---

## 5. Data-Integrity & UX Conventions

- **Cascading filters:** supplier → driver (contact person) → serial/route; changing a supplier clears the selected contact person.
- **Selecting a route** in financial forms auto-fills serial and price; these are not editable.
- **Pagination** everywhere via `PaginationHeader`; dropdowns use infinite scroll (50-item pages).
- **Excel round-trips:** passenger bulk load uses a downloaded template that must be re-uploaded unmodified in structure; attendance and cost reports export to Excel via server-generated URLs opened in a new tab.
- **Arabic-first output:** the supplier invoice PDF is rendered in Arabic with the company logo and signature lines; UI is RTL-aware.
- **Deletes are confirmed** with a warning dialog (routes, stations, passengers, vehicles, deductions).
- **Legacy fields** on Line (cost/currency/active/oneWay) remain in the model but are disabled in the UI — do not build new logic on them.

---

## 6. Rule Sources

| Rule area | Primary source files |
|---|---|
| Roles & gating | `lib/Providers/user_provider.dart`, `lib/GUI/drawer/webDrawer/web_drawer_transportation.dart`, screen-level `Visibility` checks |
| Periods, attendance | `transportation_routes/transportation_routes_mob/*`, `transportation_system_provider.dart` |
| Capacity | `transportation_exception/transportation_capacity_number_popup.dart`, capacity models |
| Financial math | `supplierTransportation/*`, `transportation_deduction/*`, `transportationCosts/*`, `accounts_all_month_for_supplier_model.dart` |
| Repricing workflow | `transportation_repriceing/*`, `transportation_line_reprice_model.dart` |
| Validation | Form validators in each add/edit dialog under `lib/GUI/screens/transportationSystem/` |
