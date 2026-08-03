# Transportation Module — Screens & UI

**Module:** `lib/GUI/screens/transportationSystem`
**Status:** As-built (reverse-engineered from the current implementation)
**Last updated:** 2026-07-05

This document catalogs every screen, dialog, and reusable widget in the transportation module: what the user sees, what they can do, which provider methods each screen calls, and how screens connect. Companion docs: [PRD.md](PRD.md), [PROVIDERS.md](PROVIDERS.md), [RULES.md](RULES.md).

**Provider abbreviations used below:** TRP = `TransportationProvider` (`transportation_system_provider.dart`), UP = `UserProvider`, FP = `FiltersProvider`, SAP = `SystemAdminProvider`, PSP = `PurchasingAndSuppliersProvider`, SCP = `SalesForceClientsProvider`, DP = `DrawerProvider`.

---

## 1. Navigation Overview

### 1.1 Web drawer entry points

[web_drawer_transportation.dart](../lib/GUI/drawer/webDrawer/web_drawer_transportation.dart) (`WebDrawerTransportation`) is the module's navigation hub. All entries navigate via `WebNavigation().webNavigateTo(route)`:

| Drawer entry | Route | Visible to |
|---|---|---|
| Transportation Dashboard | `transportationDashboardRoute` | everyone in module |
| Transportation Deductions | `transportationDeductionsRoute` | everyone |
| Suppliers Account Statement | `suppliersTransportationReportRoute` | everyone |
| Employees | `employeesRoute` | transportationAdmin / transportationSuperAdmin / systemAdmin |
| Users | `usersRoute` | transportationSuperAdmin / transportationAdmin / systemAdmin |
| Routes Exceptions | `transportationExceptionsRoute` | everyone |
| Transportation Repricing | `transportationRepricingRoute` | everyone |
| **Transportation Management** (collapsible, `DrawerProvider.transportationManagement`) | — | superAdmin / systemAdmin / lineAdmin / admin |
| ├─ Shifts | `transportationShiftsRoute` | superAdmin / systemAdmin / lineAdmin |
| └─ Supervisor & Admin | `usersRoute` | superAdmin / systemAdmin / admin |

### 1.2 Screen hierarchy

```
Dashboard ──► Attendance list ──► Passenger attendance history (dialog)
          ──► Line Cost Report
          ──► KPI cards → Lines / Routes / Vehicles / Employees / Suppliers
Lines ──► Routes (web) ──► Route detail ──► Add/Edit Station (dialog)
                       │                ──► Add/Edit Passenger-to-route (dialog)
                       ──► Add/Edit Route (dialog) / Route Details (dialog)
Passengers: Create/View/Update profile ──► Assign Routes (dialog) ──► per-row item widget
Mobile: My Routes (supervisor) ──► Route attendance ──► Bus attendance (dialog), Map view
Suppliers ──► Add/View supplier ──► contact info & contact person dialogs
Account Statement ──► Month Details / Add Payment / Add Deduction / All Payments ──► Distribution
Repricing: Create ──► View list ──► Reprice lines popup (approve/reject)
Exceptions: List ──► Exception form (popup or full screen) ──► Capacity popup
Vehicles / Shifts / Deductions: list + add/edit dialogs
```

### 1.3 Summary table

| Screen / widget | File (under `lib/GUI/screens/transportationSystem/`) | Platform | Type | Purpose |
|---|---|---|---|---|
| TransportationDashboard | `transportation_dashboard/transportation_dashboard.dart` | Web | Screen | KPI cards + route rail + filters |
| TransportationAttendance | `transportation_dashboard/transportation_attendance.dart` | Web | Screen | Passenger attendance table + Excel |
| PassengerAttendance | `transportation_dashboard/passenger_attendance.dart` | Web | Dialog | Per-passenger attendance history |
| TransportationLines | `transportation_lines/transportation_lines.dart` | Web | Screen | Lines grid |
| TransportationLineCard | `transportation_lines/transportation_line_card.dart` | Web | Card | Line card |
| AddEditTransportationLine | `transportation_lines/add_edit_transportation_line.dart` | Web | Dialog | Line CRUD + approve |
| TransportationRoutes | `transportation_routes/transportation_routes_web/transportation_routes.dart` | Web | Screen | Routes grid |
| TransportationRoutesCard | `.../transportation_routes_card.dart` | Web | Card | Route card |
| TransportationRoute | `.../transportation_route.dart` | Web | Screen | Route detail: stations + passengers |
| RouteCardDetails | `.../route_card_details.dart` | Web | Dialog | Route info popup |
| AddEditTransportationRoute | `.../add_edit_transportation_route.dart` | Web | Dialog | Route CRUD form |
| AddEditTransportationStation | `.../add_edit_transportation_station.dart` | Web | Dialog | Station CRUD + map picker |
| AddEditPassengerToRoute | `.../add_edit_passenger_to_route.dart` | Web | Dialog | Assign passenger to route |
| TransportationRoutesMob | `transportation_routes/transportation_routes_mob/transportation_routes_mob.dart` | Mobile | Screen | Supervisor's routes list |
| TransportationRouteMob | `.../transportation_route_mob.dart` | Mobile | Screen | GPS attendance (bus + passengers) |
| BusAttendanceDialog | `.../bus_attendance_dialog.dart` | Mobile | Dialog | Bus check-in/out confirm |
| TransportationPeriodMobDropdown | `.../transportation_period_mob_dropdown.dart` | Mobile | Dropdown | Go/Return picker |
| CreateTransportationPassenger | `transportation_passenger/create_transportation_passenger.dart` | Web | Screen | Create/edit passenger + Excel bulk |
| ViewTransportationPassenger | `transportation_passenger/view_transportation_passenger.dart` | Web | Screen | Passenger profile viewer |
| UpdateTransportationPassenger | `transportation_passenger/update_transportation_passenger.dart` | Web | Wrapper | Prefill + delegate to Create |
| AssignRoutesToPassenger | `transportation_passenger/assign_routes_to_passenger.dart` | Both | Dialog | Multi-row route assignment |
| AssignToRouteItem | `transportation_passenger/assign_to_route_item.dart` | Both | Sub-widget | One assignment row (edit-in-place) |
| TransportationVehicles | `transportation_vehicles/transportation_vehicles.dart` | Web | Screen | Vehicles grid |
| TransportationVehicleCard | `transportation_vehicles/transportation_vehicle_card.dart` | Web | Card | Vehicle card (status colors) |
| AddEditTransportationVehicle | `transportation_vehicles/add_edit_transportation_vehicle.dart` | Web | Dialog | Vehicle CRUD + approve |
| AddVehicleType | `transportation_vehicles/add_vehicle_type.dart` | Web | Dialog | Inline vehicle-type create |
| WorkingDaysWeb | `shiftTransportation/working_days.dart` | Web | Screen | Shift groups (7-day view) |
| AddEditWorkingDaysAndHours | `shiftTransportation/add_edit_working_days_and_hours.dart` | Web | Dialog | Shift add/edit |
| TransportationExceptions | `transportation_exception/transportation_exceptions.dart` | Web | Screen | Exceptions table |
| TransportationExceptionInfo | `transportation_exception/transportation_exception_info.dart` | Web | Screen | Exception form (full screen) |
| TransportationExceptionInfoPopup | `transportation_exception/transportation_exception_info_popup.dart` | Web | Dialog | Exception form (dialog) |
| TransportationCapacityNumberPopup | `transportation_exception/transportation_capacity_number_popup.dart` | Web | Dialog | Capacity math per route+period |
| TransportationPeriodDropdown | `transportation_exception/transportationPeriodDropdown.dart` | Both | Dropdown | Go/Return/Both picker |
| TransportationSuppliers | `supplierTransportation/transportation_suppliers.dart` | Web | Screen | Suppliers grid + filter drawer |
| AddViewSuppliersTransportationWeb | `supplierTransportation/add_view_suppliers_transportation_web.dart` | Web | Screen | Create/view supplier + logo |
| SupplierAddInformation | `supplierTransportation/supplier_add_Information.dart` | Both | Sub-widget | New-supplier form + dup check |
| ViewSupplierInformation | `supplierTransportation/view_supplier_information.dart` | Both | Sub-widget | Read-only supplier info |
| SuppliersTransportationReport | `supplierTransportation/supplier_daily_report_web.dart` | Web | Screen | Monthly account statement + PDF invoice |
| MonthReportDetails | `supplierTransportation/month_report_details.dart` | Both | Dialog | Daily rounds of a month |
| AddSupplierPayment | `supplierTransportation/add_supplier_payment.dart` | Both | Dialog | Advance/Normal payment |
| AllSupplierPayments | `supplierTransportation/all_supplier_payments.dart` | Both | Dialog | Payments list |
| DistributionSupplierPaymentsDialog | `supplierTransportation/distribution_supplier_payments.dart` | Both | Dialog | Advance distribution per month |
| TransportationMultiSelectChips | `supplierTransportation/multi_select_chips.dart` | Both | Sub-widget | Route filter chips (infinite scroll) |
| TransportationLineCosts | `transportationCosts/transportation_costs.dart` | Web | Screen | Line cost report + Excel |
| AddTransportationRepricing | `transportation_repriceing/add_transportation_repricing.dart` | Web | Screen | Create repricing |
| ViewTransportationRepricing | `transportation_repriceing/view_transportation_repricing.dart` | Web | Screen | Repricing records list |
| ViewRepriceLinesPopup | `transportation_repriceing/view_reprice_lines_popup.dart` | Web | Dialog | Before/after prices; approve/reject |
| TransportationDeductions | `transportation_deduction/deductions.dart` | Web | Screen | Deductions table |
| AddEditDeduction | `transportation_deduction/add_edit_deduction.dart` | Web | Dialog | Deduction CRUD |

Shared dropdown widgets used by the module (in `lib/GUI/widgets/dropdown_widgets/`): `TransportationLineDropdown`, `TransportationRouteDropdown`, `TransportationStationsDropdown`, `TransportationVehiclesDropdown`, `TransportationVehicleTypeDropdown`, `TransportationRouteUsersDropdown` — see §11.

---

## 2. Dashboard

### TransportationDashboard — [transportation_dashboard.dart](../lib/GUI/screens/transportationSystem/transportation_dashboard/transportation_dashboard.dart)
Web KPI screen. Left rail (≈15% width) lists routes (infinite scroll via `scrollTransportationRoutes()`); clicking a route filters the KPIs. Filter row: supplier dropdown → driver dropdown (requires supplier first; snackbar otherwise) → serial (digits only) → date picker. Nine KPI cards bind to `transportationDashboardNumbersResponse.data` (lines, routes/vehicles, vehicle types, employees, suppliers active/all, vehicle attendance counts and percents, user attendance count and percent, two-way/one-way route counts); most cards navigate to their detail screens (the Employees card also sets `SharedPrefConst.isTransportation = true`).

- **TRP calls:** `getDashboardNumbers(...)`, `getAllTransportationRoute(...)`, `clearDashboardFilters()`, pagination updaters.
- **Buttons:** "Line Cost Report" → `transportationLineCostRoute`; "View Attendance" → `transportationDashboardAttendanceRoute`; "Take Electronic Touch Attendance" — gated to `transportationAdmin || transportationSuperAdmin || systemAdmin`.

### TransportationAttendance — [transportation_attendance.dart](../lib/GUI/screens/transportationSystem/transportation_dashboard/transportation_attendance.dart)
Attendance table for a date range ("View Attendance {from} to {to}"). Filters: line, supplier, driver (requires supplier), route (selecting one also fills the serial), serial, from/to dates, plus **Attended** / **Absent** checkboxes. Table columns: name, ID code, another identifier, line, route, supervisor, supplier, driver, attendance (check-in/out times, black = attended / red = absent; expandable when history has multiple records → opens `PassengerAttendance`). "Download Excel" calls `getDashboardAttendanceExcel(...)` and opens the returned URL in a new tab. Paginated via `dashboardAttendancePageNo`.

- **TRP calls:** `getDashboardAttendance(...)`, `getDashboardAttendanceExcel(...)`, `updateDashboardAttendanceAttended/Absent(...)`.

### PassengerAttendance — [passenger_attendance.dart](../lib/GUI/screens/transportationSystem/transportation_dashboard/passenger_attendance.dart)
850×700 dialog: per-passenger history table (date, check-in, check-out, attended/absent badge — green/red). Pure display; data passed in via constructor (`List<AttendanceHistory>`).

---

## 3. Lines

### TransportationLines — [transportation_lines.dart](../lib/GUI/screens/transportationSystem/transportation_lines/transportation_lines.dart)
Paginated grid of `TransportationLineCard`s with a "Transportation Routes" shortcut button. Add card/link visible to systemAdmin / lineAdmin / superAdmin. Navigating to a line's routes stores `transportationLineId`/`transportationLineName` in shared prefs first.

### TransportationLineCard — [transportation_line_card.dart](../lib/GUI/screens/transportationSystem/transportation_lines/transportation_line_card.dart)
Shows line name + bus icon; **Update** button (role-gated) and a routes button that shows the route count (or "Assign to Route" when the line has none).

### AddEditTransportationLine — [add_edit_transportation_line.dart](../lib/GUI/screens/transportationSystem/transportation_lines/add_edit_transportation_line.dart)
Dialog with a single required line-name field. Edit mode adds **Delete** (with `WarningDialog`) and — for superAdmin/systemAdmin when not yet approved — **Approve** (`approveTransportationLine`). Saves via `addTransportationLine` / `updateTransportationLine`, then refreshes the list.

---

## 4. Routes (Web)

### TransportationRoutes — [transportation_routes.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/transportation_routes.dart)
Paginated, responsive grid of routes (optionally scoped to a line). Admins (lineAdmin/superAdmin/systemAdmin) see an add button + add card → `AddEditTransportationRoute`. Card "Route Details" → `RouteCardDetails` dialog; card "Actions" stores `transportationRouteId` in shared prefs and navigates to the route detail screen.

### TransportationRoutesCard — [transportation_routes_card.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/transportation_routes_card.dart)
Gradient card: route name, supervisor, full capacity / users-in-route, actual capacity / attendance, station count; **Actions** (role-gated) and **Route Details** buttons.

### TransportationRoute — [transportation_route.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/transportation_route.dart)
Route detail screen. Header wraps line name, arrival/departure times (12h), supplier, driver, supervisor, and religion chips ("C:" / "M:" — known bug: both bind to `christianNum`). Two side-by-side tables:
- **Stations:** name, description/time, lat/lng (map-view button + copy-to-clipboard + tooltip), edit, delete (warning dialog → `deleteTransportationDirection`).
- **Passengers:** name, station, edit, delete (`deleteTransportationEmployee`).
Buttons: Add Station, Add Passenger, Update Route.
- **TRP calls:** `getTransportationRoute`, `getTransportationRouteStations`, `getRouteUsers`, deletes above.

### RouteCardDetails — [route_card_details.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/route_card_details.dart)
600px info dialog: supplier, driver, creation date, capacities, times, religion counts, exceptions from/to other lines, station count, supervisor. Display-only (data via constructor).

### AddEditTransportationRoute — [add_edit_transportation_route.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/add_edit_transportation_route.dart)
700px form dialog. Fields: line (dropdown or free-text branch toggled by the "اختيار محطة تجمع" checkbox), route name*, shift*, cost* (double-only), supplier*, driver (enabled after supplier), supervisor, vehicle*, route-type checkboxes (round-trip/go/return — mutually exclusive; go clears To-time, return clears From-time), From/To time pickers (12h, conditional). Edit mode adds Delete (warning dialog → navigates back to routes list).
- **TRP calls:** `addTransportationRoute`, `updateTransportationRoute`, `deleteTransportationRoute`, `setRouteType*`, `clearTransportationRoute`, refresh via `getAllTransportationRoute`.

### AddEditTransportationStation — [add_edit_transportation_station.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/add_edit_transportation_station.dart)
800px form dialog: station name* ("station_name_cannot_be_empty"), description/time, Active checkbox, lat/lng (double-only) or **Pick Location** map dialog. Saves via `addTransportationStation` / `updateTransportationStation`, refreshes stations.

### AddEditPassengerToRoute — [add_edit_passenger_to_route.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_web/add_edit_passenger_to_route.dart)
600px form dialog: passenger (HR users dropdown)*, station dropdown, period dropdown* ("please_select_period"; "Both" sets `isGetInAndOut`). Saves via `addTransportationEmployee` / `updateTransportationEmployee`, refreshes `getRouteUsers`.

---

## 5. Routes (Mobile) — Supervisor Attendance

### TransportationRoutesMob — [transportation_routes_mob.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_mob/transportation_routes_mob.dart)
Mobile list of routes the logged-in user supervises (`getTransportationRoutesByHrUserId(userId)`); empty state: "This user has no routes to supervise". Each card (line name, supplier, supervisor, driver, bus image) pushes `TransportationRouteMob` with the route + bus serial.

### TransportationRouteMob — [transportation_route_mob.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_mob/transportation_route_mob.dart)
The core operational mobile screen. Period selector (defaults **Go**; mobile list has no "Both"). Passenger list shows name/email/mobile/photo and, per period:
- **Go:** *Check In* button if not checked in (GPS captured → `addUsersAttendance(type:'Employee', checkInOrCheckOut:true)`), else the recorded time.
- **Return:** check-in status shown; *Check Out* button (`checkInOrCheckOut:false`) or recorded time.
Top actions: **bus attendance** (opens `BusAttendanceDialog`, records against the bus serial with `type:'Bus'`) and **map view** (plots non-empty passenger coordinates, sorted by latitude).
- **TRP calls:** `getRouteUsersForMob(period)`, `addUsersAttendance(...)`, `getCurrentLocationWithPermission()`.

### BusAttendanceDialog — [bus_attendance_dialog.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_mob/bus_attendance_dialog.dart)
Confirmation dialog with two buttons: "Attended" → `onCheckOut` callback, "Leave" → `onCheckIn` callback (note the label/semantics swap flagged in [PRD.md](PRD.md)).

### TransportationPeriodMobDropdown — [transportation_period_mob_dropdown.dart](../lib/GUI/screens/transportationSystem/transportation_routes/transportation_routes_mob/transportation_period_mob_dropdown.dart)
Searchable, keyboard-navigable period picker fed by `transportationShiftsList` (mobile variant excludes "Both" via item count − 1).

---

## 6. Passengers

### CreateTransportationPassenger — [create_transportation_passenger.dart](../lib/GUI/screens/transportationSystem/transportation_passenger/create_transportation_passenger.dart)
Create/edit passenger profile (2,728 lines; many HR sections commented out). Fields: name*, identity number*, mobile* (digits only), marital/"another identifier"* (dropdown), map-picked lat/lng (read-only), profile photo upload, Active toggle. Header buttons: **Download Excel** template (`downloadExcelUserWithRoutesTemplete` → new tab) and **Upload Excel** (`insertUsersWithRoutesExcel` → success snackbar). Sidebar: **Assign to Route** opens `AssignRoutesToPassenger` (isAdd = creating). Submit → `SAP.addEmployeeForTransportation()` or `SAP.editEmployee()` (includes assigned routes), then navigates to the view screen.

### ViewTransportationPassenger — [view_transportation_passenger.dart](../lib/GUI/screens/transportationSystem/transportation_passenger/view_transportation_passenger.dart)
Read-only profile: personal info, location, photo (zoomable; editable only by the owner). Edit button gated to HR/transportation admin roles; **Assign to Route** gated to transportationAdmin/lineAdmin/superAdmin/systemAdmin. Data via `SAP.getHrUserInfo(hrUserId)`.

### UpdateTransportationPassenger — [update_transportation_passenger.dart](../lib/GUI/screens/transportationSystem/transportation_passenger/update_transportation_passenger.dart)
Thin wrapper: fetches/receives the passenger, prefills via `SAP.getDataToEditUserProfile`, renders `CreateTransportationPassenger` in edit mode.

### AssignRoutesToPassenger — [assign_routes_to_passenger.dart](../lib/GUI/screens/transportationSystem/transportation_passenger/assign_routes_to_passenger.dart)
700×700 dialog managing a list of `AssignToRouteItem` rows. **Add Route** appends a blank row. Two modes: `isAdd=true` (new passenger — rows kept locally, copied into `SAP.transportationRoutesHrUser` on submit) and `isAdd=false` (existing — loads via `getTransportationRoutesListForPassenger`, submit posts new rows via `addTransportationRouteForPassenger`). Removing a saved row calls the delete API; an unsaved row is removed locally.

### AssignToRouteItem — [assign_to_route_item.dart](../lib/GUI/screens/transportationSystem/transportation_passenger/assign_to_route_item.dart)
One assignment row: route* ("please_select_route"), station (depends on route), period* ("please_select_period"), from/to validity dates, lat/lng + map picker. Saved rows are read-only until **Edit**; a change-detection listener enables **Submit** (`updateTransportationRouteForPassenger`) only when values actually changed. New rows (empty id) are always editable.

---

## 7. Vehicles

### TransportationVehicles — [transportation_vehicles.dart](../lib/GUI/screens/transportationSystem/transportation_vehicles/transportation_vehicles.dart)
Paginated grid of vehicle cards; add card/edit gated to superAdmin/lineAdmin/systemAdmin. Card tap opens the edit dialog.

### TransportationVehicleCard — [transportation_vehicle_card.dart](../lib/GUI/screens/transportationSystem/transportation_vehicles/transportation_vehicle_card.dart)
Status-colored card (green = approved+active, orange = approved+inactive, red = not approved) showing type, capacity/seats, approval and active indicators.

### AddEditTransportationVehicle — [add_edit_transportation_vehicle.dart](../lib/GUI/screens/transportationSystem/transportation_vehicles/add_edit_transportation_vehicle.dart)
Dialog: capacity* and vehicle type* ("please_complete_the_data"), Active checkbox, **Add Vehicle Type** shortcut, Delete (warning dialog), and **Approve** (visible when unapproved, superAdmin/systemAdmin only → `approveTransportationVehicle`).

### AddVehicleType — [add_vehicle_type.dart](../lib/GUI/screens/transportationSystem/transportation_vehicles/add_vehicle_type.dart)
Minimal dialog: type name → `addVehicleType(active: true)`.

---

## 8. Shifts (Working Days & Hours)

### WorkingDaysWeb — [working_days.dart](../lib/GUI/screens/transportationSystem/shiftTransportation/working_days.dart)
Branch-scoped (`UP.branchId`) read-only view of shift groups: each shift shows its number, a delete button, and 7 day columns (orange = active) with from/to times in 12h format. Delete = set all days inactive then `updateShifts`. **Add** / **Edit** open the dialog below. Data: `getShifts(branchId)`.

### AddEditWorkingDaysAndHours — [add_edit_working_days_and_hours.dart](../lib/GUI/screens/transportationSystem/shiftTransportation/add_edit_working_days_and_hours.dart)
Dialog. Add mode generates the next shift number (`generateShiftList`); edit mode loads all shifts (`generateShiftListToUpdate`). Per day: toggle active, pick from/to times (defaults 12:00–18:00); a "set time for all" row applies one window to a whole shift (`updateShiftDays`). Submit → `addShifts` / `updateShifts` (branch-scoped), refresh, close.

---

## 9. Exceptions

### TransportationExceptions — [transportation_exceptions.dart](../lib/GUI/screens/transportationSystem/transportation_exception/transportation_exceptions.dart)
Paginated table: route, passenger, exception station, lat/lng, exception date, from/to dates, period, weekdays, reason, contact number. Add/Update gated to superAdmin/lineAdmin/systemAdmin. Add clears `SharedPrefConst.transportationExceptionId`; Edit stores the ID — then both open `TransportationExceptionInfoPopup` (which reads the ID to decide add vs. update).

### TransportationExceptionInfo / TransportationExceptionInfoPopup — [transportation_exception_info.dart](../lib/GUI/screens/transportationSystem/transportation_exception/transportation_exception_info.dart) / [transportation_exception_info_popup.dart](../lib/GUI/screens/transportationSystem/transportation_exception/transportation_exception_info_popup.dart)
The same form in full-screen and dialog variants:
1. Passenger* + route* dropdowns; **Route Details** button (visible once route + period chosen) → capacity popup.
2. Exception type — mutually exclusive: single **Exception Date** vs **Period** (from/to dates + weekdays dropdown).
3. Location mode — mutually exclusive: **Station** (route stations dropdown; requires route first) vs **Location** (map picker → read-only lat/lng).
4. Period dropdown* (Go/Return/Both); "Both" reveals a second get-out station/location block.
5. Reason (multiline) + contact number (digits only).
Submit → `addException` / `updateTransportationException`; full screen navigates back to the exceptions route, popup pops and refreshes the list.

### TransportationCapacityNumberPopup — [transportation_capacity_number_popup.dart](../lib/GUI/screens/transportationSystem/transportation_exception/transportation_capacity_number_popup.dart)
Read-only dialog per route+period via `getTransportationCapacityNumber`: full capacity, actual capacity, exceptions from other lines, route passengers to other lines, capacity without exceptions.

### TransportationPeriodDropdown — [transportationPeriodDropdown.dart](../lib/GUI/screens/transportationSystem/transportation_exception/transportationPeriodDropdown.dart)
Searchable list picker over `transportationShiftsList` (Go/Return/Both), keyboard navigation, pops on select.

---

## 10. Suppliers & Financials

### TransportationSuppliers — [transportation_suppliers.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/transportation_suppliers.dart)
Suppliers grid (name, creation date, logo) + filter end-drawer (name/phone/mobile, find/reset, pagination). Uses **PSP** (`getSuppliersCards`, `resetFilterSupplier`) rather than TRP — suppliers are the shared purchasing entity. "Add new supplier" card gated to `addSupplier || transportationSuperAdmin || transportationAdmin`; navigates to `addNewSupplierTransportationWebRoute`.

### AddViewSuppliersTransportationWeb — [add_view_suppliers_transportation_web.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/add_view_suppliers_transportation_web.dart)
Create/view supplier screen: left column is `SupplierAddInformation` (new) or `ViewSupplierInformation` (existing), right column is logo upload (140×140, `PSP.getImageForWeb`). New: Submit → `addNewSupplierFirstScreen` then `postNewSupplierContactPersonThirdScreen`, stores supplier id, re-opens in view mode. Existing: Edit dialog + Finish → suppliers list.

### SupplierAddInformation — [supplier_add_Information.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/supplier_add_Information.dart)
New-supplier form with **live duplicate checks** (`PSP.checkSupplierExist` on name/email/phone/mobile/fax; red = duplicate, blue = unique), mobile with country-code picker (default EG +20), add-contact-info actions, and a duplicates dropdown.

### ViewSupplierInformation — [view_supplier_information.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/view_supplier_information.dart)
Read-only supplier panel: contact-info counts + "Show All" dialogs, add landline/mobile/contact-person dialogs, first contact person (the "driver"), and specialities chips.

### SuppliersTransportationReport — [supplier_daily_report_web.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/supplier_daily_report_web.dart)
**Monthly account statement.** Filters: supplier, route, month (≤ 12), year (year-only picker). Table per supplier-month: routes count, rounds (full / half-go / half-return), total due, total deductions, normal payments, advance payments, remaining. Row actions:
- **Details** → `MonthReportDetails`
- **Add Payment** → `AddSupplierPayment` — gated superAdmin/lineAdmin/systemAdmin
- **Add Deduction** → `AddEditDeduction` (pre-filled supplier) — same gate
- **View Payments** → `AllSupplierPayments`
- **Print Invoice** — same gate; builds an **Arabic PDF** in-client (`pw.Document`, Arabic font asset, trips table + financial summary).
Data: `getAccountsAllMonthForSupplier(pageNo, noOfItems, supplierId, routeId, month, year)`.

### MonthReportDetails — [month_report_details.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/month_report_details.dart)
Dialog: daily rounds for a supplier-month (date, route, check-in/out, one-way timestamps, rounds count, total cost of day) with route filter chips (`TransportationMultiSelectChips`) and pagination. Data: `getTransportationDaysInMonth`.

### AddSupplierPayment — [add_supplier_payment.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/add_supplier_payment.dart)
Payment dialog: amount (double-only), payment date, type toggle **Advance Payment Installment** vs **Monthly Payment**; Advance reveals number-of-months (≤ 12) + start date. Pay → `addSuplierPayment(typeOfDebt: "Advance"|"Normal")`, refreshes the statement, closes.

### AllSupplierPayments — [all_supplier_payments.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/all_supplier_payments.dart)
Paginated payments table (amount, payment date, start date, months, type); **Show Details** (Advance rows only) → `DistributionSupplierPaymentsDialog`. Data: `getAllSupplierPayment(supplierId, fromDate, toDate, ...)`.

### DistributionSupplierPaymentsDialog — [distribution_supplier_payments.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/distribution_supplier_payments.dart)
Read-only table of an advance's per-month distribution (month, year, payment).

### TransportationMultiSelectChips — [multi_select_chips.dart](../lib/GUI/screens/transportationSystem/supplierTransportation/multi_select_chips.dart)
Horizontal `FilterChip` list of a supplier's routes with infinite-scroll loading (`getAllTransportationRoute(supplierId)`); single-select with deselect.

---

## 11. Costs, Repricing, Deductions

### TransportationLineCosts — [transportation_costs.dart](../lib/GUI/screens/transportationSystem/transportationCosts/transportation_costs.dart)
Line cost report. Filters: line, supplier, driver (requires supplier), route (fills serial), serial (digits), from/to dates — **defaulting to the current calendar month**. Table: line, route, supplier, cost, one-way, rounds number, deduction number, deduction total, **net cost**. "Download Excel" → `getReportCostsExcel` → new tab. Data: `getReportCosts(...)`. No role gates (read for all).

### AddTransportationRepricing — [add_transportation_repricing.dart](../lib/GUI/screens/transportationSystem/transportation_repriceing/add_transportation_repricing.dart)
Repricing creation screen. Controls: route/line picker (adds rows to the selected-lines table), **all-lines vs selected-lines** toggle, **round-to-5** toggle (`approximateToFiveFlag`), **percent vs fixed amount** toggle (`isPercent`), amount input, start-date picker. Table preview shows either all lines (paginated) or the selected lines (with remove action). Save → confirmation dialog → `rePricingTransportationRoute(amount, isPercent, forAllLines, approximateToFiveFlag, lineIds, startDate)` → navigate to the repricing list. Error snackbar when no lines selected in selected-lines mode.

### ViewTransportationRepricing — [view_transportation_repricing.dart](../lib/GUI/screens/transportationSystem/transportation_repriceing/view_transportation_repricing.dart)
Repricing records table (increase cost, created by/date, approved status green/red, approved by or "pending", for-all-lines flag) with **approved-only / not-approved** filter checkboxes (both or neither ⇒ no filter). **View** → `ViewRepriceLinesPopup`. **Add** button gated to superAdmin/lineAdmin. Data: `getTransportationLineRePricing(approve?)`.

### ViewRepriceLinesPopup — [view_reprice_lines_popup.dart](../lib/GUI/screens/transportationSystem/transportation_repriceing/view_reprice_lines_popup.dart)
For all-lines repricing: a message "all lines will be increased by {amount}". Otherwise a table of per-line **before (red) → after (green)** prices. When unapproved and user is superAdmin/systemAdmin: **Approve** (`updatePriceOfTransportationLine`) and — superAdmin only — **Reject** (`rejectRepriceTransportationLine`); both refresh the list.

### TransportationDeductions — [deductions.dart](../lib/GUI/screens/transportationSystem/transportation_deduction/deductions.dart)
Paginated deductions table (route, supplier, driver, deduction day/amount, created by, creation date, reason). Add/Update gated to superAdmin/lineAdmin/systemAdmin → `AddEditDeduction`.

### AddEditDeduction — [add_edit_deduction.dart](../lib/GUI/screens/transportationSystem/transportation_deduction/add_edit_deduction.dart)
Deduction dialog: supplier* → route* (auto-fills serial and price — read-only), type checkboxes **Normal (خصم عادي)** vs **Tax (ضريبة)** (mutually exclusive), optional **percentage mode** (deduction-per-round computed as `price × percent / 100`, read-only result), date*, deduction-per-half-round*, reason*. Save → `addDeduction` / `updateDeduction`, then refreshes both the deductions list and the supplier month statement (`getAccountsAllMonthForSupplier`).

---

## 12. Shared Dropdown Widgets

All in `lib/GUI/widgets/dropdown_widgets/`, opened via `FiltersProvider.openDropDownWidget(...)`; all support keyboard navigation and pop on select, returning a `DropDownModel`:

| Widget | Data source (TRP) | Returns | Notes |
|---|---|---|---|
| `TransportationLineDropdown` | `getTransportationLines` (paginated) | id + lineName | infinite scroll |
| `TransportationRouteDropdown` | `getAllTransportationRoute` (paginated) | id + nameOfRoute + **serial + price** | also feeds the repricing selection list |
| `TransportationStationsDropdown` | `getTransportationRouteStations(routeId)` | id + routeDirection | requires a route |
| `TransportationVehiclesDropdown` | `getTransportationVehicles` (paginated) | id + "type capacity" | infinite scroll |
| `TransportationVehicleTypeDropdown` | `getVehicleTypes` | id + type | |
| `TransportationRouteUsersDropdown` | `getRouteUsers(routeId)` | id + **stationLatitude as name / stationLongitude as description** | unusual payload — coordinates ride in name/description |

---

## 13. Cross-cutting UI Conventions

- **Screen bootstrap:** every screen calls `getPublicData(context, getData)` in `initState`, and `getApis()` re-runs on any filter change.
- **Pagination:** `PaginationWidget` + TRP `currentPage`/`itemsPerPage` (defaults 1 / 20); dropdown lists use infinite scroll.
- **Dialog-first CRUD:** all add/edit forms are `AlertDialog`/`SimpleDialog` popups except the passenger profile, repricing creation, and exception full-screen form.
- **Deletes** always confirm through `WarningDialog`.
- **Dialog context passing** sometimes goes through shared preferences (`transportationRouteId`, `transportationLineId`, `transportationExceptionId`) rather than constructor args.
- **Web vs mobile:** only the routes feature has a mobile variant (supervisor attendance); everything else is web-only (`MyAppBarWeb`, fixed table widths, `WebNavigation`).
- **Excel/PDF:** Excel exports are server-generated URLs opened in a new tab; the supplier invoice PDF is generated client-side in Arabic.
