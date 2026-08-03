# Transportation Module — Data Models

Companion docs: [PRD.md](PRD.md) · [SCREENS.md](SCREENS.md) · [API.md](API.md) · [PROVIDERS.md](PROVIDERS.md) · [RULES.md](RULES.md) · [DEPENDENCIES.md](DEPENDENCIES.md) · [ARCHITECTURE.md](ARCHITECTURE.md)

## Introduction

This document catalogs every Dart data model used by the Transportation module. All models live under
`lib/DataAccessLayer/model/transportation/` (with the shifts sub-folder at
`lib/DataAccessLayer/model/transportation/shifts/`). They form the **Model** layer of the app's
`DataAccessLayer`, which sits below the repository/provider layers: HTTP responses returned by the
backend API are decoded from JSON into these classes and then consumed by providers and the GUI.

### Serialization convention

- Every top-level response model follows the same envelope shape:
  - `result` — mapped from the JSON key `Result` (bool, defaults to `false`).
  - `errors` — mapped from the JSON key `Errors`, a list decoded into `ErrorsModel` objects
    (`lib/DataAccessLayer/model/errors_model.dart`).
  - `data` — mapped from the JSON key `Data`; either a single object or a list of row models.
  - Paginated list responses additionally carry `paginationHeader`, mapped from `PaginationHeader`
    (`lib/DataAccessLayer/model/pagination.dart`).
- Deserialization is done through `factory X.fromJson(Map<String, dynamic> json)` constructors. Only a
  few models define `toJson()` (noted per model below).
- Backend JSON keys are **PascalCase** (e.g. `LineName`), though several keys are lowercase/camelCase
  (e.g. `branchScheduleId`, `christianNum`, `typeOfDeduction`). Numeric IDs are frequently coerced to
  `String` via `.toString()` with `""` fallbacks; missing values default to `0`, `0.0`, `false`, or
  empty string/list.
- **Backend key typos are preserved verbatim** in the code and are flagged in the notes below with a
  ⚠️ marker. Do not "fix" these names in the app without a coordinated backend change.

### Shared envelope helpers

| Class | File | Fields (JSON key) |
| --- | --- | --- |
| `ErrorsModel` | `lib/DataAccessLayer/model/errors_model.dart` | `errorCode` (`ErrorCode`), `errorMsg` (`ErrorMSG`) |
| `PaginationHeader` | `lib/DataAccessLayer/model/pagination.dart` | `currentPage` (`CurrentPage`), `itemsPerPage` (`ItemsPerPage`), `totalItems` (`TotalItems`), `totalPages` (`TotalPages`) — all `int`, **non-null-safe** parse (throws if keys missing) |

---

## Summary table

| Model class | File | Purpose |
| --- | --- | --- |
| `TransportationRoutesModel` | `transportation_routes_model.dart` | Routes for a given HR user (route summary rows). |
| `TransportationAllRouteModel` | `transportation_all_route_model.dart` | Full paginated list of all routes with capacity/cost. |
| `TransportationRouteModel` | `transportation_route_model.dart` | Single route detail (edit/view one route). |
| `TransportationDirectionsModel` | `transportation_directions_model.dart` | Route directions / stations with geo coordinates. |
| `TransportationRoutesListForPassengerModel` | `transportation_routes_list_for_passenger_model.dart` | Routes list bound to editable form controllers for passenger assignment. |
| `RouteUsersModel` | `route_users_model.dart` | Passengers (HR users) assigned to a route. |
| `RouteUsersForMobModel` | `route_users_for_mob_model.dart` | Passengers for the mobile app with check-in/out + registration flags. |
| `TransportationVehiclesModel` | `transportation_vehicles_model.dart` | Paginated list of vehicles. |
| `VehicleTypesModel` | `transportation_vehicle_type_model.dart` | Vehicle types lookup. |
| `TransportationCapacityNumberModel` | `transportation_capacity_number_model.dart` | Capacity breakdown for a route. |
| `TransportationLinesModel` | `transportation_lines_model.dart` | Paginated list of transportation lines. |
| `TransportationLineRepriceModel` | `transportation_line_reprice_model.dart` | Line reprice / cost-increase records with before/after detail. |
| `AccountsAllMonthForSupplierModel` | `accounts_all_month_for_supplier_model.dart` | Monthly financial account summary per supplier. |
| `AllSupplierPaymentsModel` | `all_supplier_payments_model.dart` | Supplier payments with per-month distribution. |
| `TransportationDaysInMonthModel` | `transportation_days_in_month_model.dart` | Daily rounds/cost breakdown within a month. |
| `TransportationDashboardModel` | `transportation_dashboard_model.dart` | Aggregate dashboard counters/percentages. |
| `TransportationDashboardAttendanceModel` | `transportation_dashboard_attendance_model.dart` | Per-passenger attendance history for the dashboard. |
| `TouchAttendanceModel` | `touch_attendance_model.dart` | Simple string-payload response for touch attendance. |
| `TransportationAttendanceExcelModel` | `transportation_attendance_excel_model.dart` | Excel export payload (attendance). |
| `TransportationExcelModel` | `transportation_costs_excel_model.dart` | Excel export payload (costs). |
| `TransportationReportCostsModel` | `transportation_report_costs_model.dart` | Paginated cost report rows per route. |
| `TransportationExceptionModel` | `transportation_exception_model.dart` | Passenger transport exceptions (temporary route/station changes). |
| `TransportationDeductionsModel` | `transportation_deductions_model.dart` | Supplier deduction records. |
| `ShiftsModel` | `shifts/shifts_model.dart` | Branch shifts grouped by shift number. |

---

## 1. Routes & stations

### `TransportationRoutesModel`
`lib/DataAccessLayer/model/transportation/transportation_routes_model.dart`

Envelope: `result` (`Result`), `errors` (`Errors` → `ErrorsModel`), `data` (`Data` → list of
`TransportationRoutesByHrUserIdData`). No pagination header.

**`TransportationRoutesByHrUserIdData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `int` | `Id` | default 0 |
| `lineName` | `String` | `LineName` | |
| `lineId` | `int` | `LineId` | |
| `supplierName` | `String` | `SupplierName` | |
| `supplierId` | `int` | `SupplierId` | |
| `supplierContactPersonName` | `String` | `SupplierContactPersonName` | |
| `supplierContactPersonId` | `int` | `SupplierContactPersonId` | |
| `serial` | `String` | `Serial` | `.toString()` |
| `branchScheduleId` | `int` | `branchScheduleId` | lowercase key |
| `periodFrom` | `DateTime` | `PeriodFrom` | `DateTime.parse` (non-null required — throws if absent) |
| `periodTo` | `DateTime?` | `PeriodTo` | nullable |
| `busSupervisor` | `String` | `BusSupervisor` | |
| `fullCapacity` | `int` | `FullCapacity` | |
| `usersInRoute` | `int` | `UserInRoute` | note singular `User` in key |
| `actualCapacity` | `int` | `ActualCapacity` | |
| `actualUserAttendance` | `int` | `ActualUserAttedance` | ⚠️ typo `ActualUserAttedance` (missing `n`) |
| `passengerFromOtherLines` | `int` | `ExpectionNumFromOtherLines` | ⚠️ typo `Expection` (should be `Exception`) |
| `passengerToOtherLines` | `int` | `RouteEmployeesToOtherLines` | |
| `stationsNum` | `int` | `DirectionNum` | stations are "directions" |

Relationships: aggregates line, supplier, contact-person and capacity data by ID/name (no nested
objects — flattened row).

### `TransportationAllRouteModel`
`lib/DataAccessLayer/model/transportation/transportation_all_route_model.dart`

Envelope with `paginationHeader` (`PaginationHeader`). `data` (`Data`) → list of
`TransportationRouteData`.

**`TransportationRouteData`** — a superset of `TransportationRoutesByHrUserIdData`.

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `int` | `Id` | |
| `lineName` | `String` | `LineName` | |
| `lineId` | `int` | `LineId` | |
| `lineCost` | `int` | `LineCost` | |
| `supplierName` | `String` | `SupplierName` | |
| `supplierId` | `int` | `SupplierId` | |
| `supplierContactPersonName` | `String` | `SupplierContactPersonName` | |
| `supplierContactPersonId` | `int` | `SupplierContactPersonId` | |
| `serial` | `String` | `Serial` | `.toString()` |
| `branchScheduleId` | `int` | `branchScheduleId` | lowercase key |
| `periodFrom` | `DateTime` | `PeriodFrom` | `DateTime.parse` (required) |
| `periodTo` | `DateTime?` | `PeriodTo` | nullable |
| `busSupervisor` | `String` | `BusSupervisor` | |
| `fullCapacity` | `int` | `FullCapacity` | |
| `usersInRoute` | `int` | `UserInRoute` | |
| `actualCapacity` | `int` | `ActualCapacity` | |
| `actualUserAttendance` | `int` | `ActualUserAttedance` | ⚠️ typo |
| `passengerFromOtherLines` | `int` | `ExpectionNumFromOtherLines` | ⚠️ typo `Expection` |
| `passengerToOtherLines` | `int` | `RouteEmployeesToOtherLines` | |
| `stationsNum` | `int` | `DirectionNum` | |
| `muslimNum` | `int` | `MuslimNum` | |
| `christianNum` | `int` | `christianNum` | lowercase key |
| `fromDate` | `String` | `FromoDate` | ⚠️ typo `FromoDate` (extra `o`) |
| `toDate` | `String` | `ToDate` | |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `transportationVehId` | `int` | `TransportationVehicleId` | |
| `transportationVehName` | `String` | `TransportationVehicleName` | |

Relationships: same flattened line/supplier/vehicle references; adds passenger religion counts and
vehicle assignment.

### `TransportationRouteModel`
`lib/DataAccessLayer/model/transportation/transportation_route_model.dart`

Envelope with a **single** `data` object (`Data` → `TransportationRouteInfo`); falls back to a fully
empty-string `TransportationRouteInfo` when `Data` is null.

**`TransportationRouteInfo`** (all fields `String`)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `lineId` | `String` | `LineId` | `.toString()` |
| `lineName` | `String` | `LineName` | |
| `supplierId` | `String` | `SupplierId` | `.toString()` |
| `supplierName` | `String` | `SupplierName` | |
| `supplierContactPersonId` | `String` | `SupplierContactPersonId` | `.toString()` |
| `supplierContactPersonName` | `String` | `SupplierContactPersonName` | |
| `serial` | `String` | `Serial` | |
| `branchScheduleId` | `String` | `branchScheduleId` | lowercase key |
| `branchSchedule` | `String` | `branchSchedule` | lowercase key |
| `periodFrom` | `String` | `PeriodFrom` | |
| `periodTo` | `String` | `PeriodTo` | |
| `busSupervisorId` | `String` | `BuSupervisorId` | ⚠️ typo `BuSupervisorId` (missing `s` → should be `BusSupervisorId`) |
| `busSupervisor` | `String` | `BusSupervisor` | |
| `fromDate` | `String` | `FromoDate` | ⚠️ typo `FromoDate` |
| `toDate` | `String` | `ToDate` | |
| `christianNum` | `String` | `christianNum` | lowercase key |
| `muslimicNum` | `String` | `MuslimNum` | Dart field is `muslimicNum`; key is `MuslimNum` |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `cost` | `String` | `LineCost` | field renamed to `cost` |
| `vehId` | `String` | `TransportationVehicleId` | |
| `vehName` | `String` | `TransportationVehicleName` | |

### `TransportationDirectionsModel`
`lib/DataAccessLayer/model/transportation/transportation_directions_model.dart`

Envelope; `data` (`Data`) → list of `TransportationDirectionsData`. "Directions" == route
stations/stops.

**`TransportationDirectionsData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `routeDirection` | `String` | `RouteDirection` | |
| `description` | `String` | `Description` | |
| `latitude` | `String` | `Latitude` | `.toString()` |
| `longitude` | `String` | `Longtitud` | ⚠️ typo `Longtitud` (should be `Longitude`) |

### `TransportationRoutesListForPassengerModel`
`lib/DataAccessLayer/model/transportation/transportation_routes_list_for_passenger_model.dart`

Note: imports `package:flutter/cupertino.dart` because each row embeds `TextEditingController`s — this
model is UI-form aware (used for editing passenger route assignments). `fromJson` takes
`Map<dynamic, dynamic>`. Envelope has no typed `errors` (`errors` is a raw `List`) and no pagination.

**`TransportationRoutesListForPassengerItem`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `routeId` | `String` | `RouteId` | `.toString()` |
| `transportationVehicleRouteDirectionId` | `String` | `TransportationVehicleRouteDirectionId` | `.toString()` |
| `serial` | `String` | `Serial` | |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `periodFrom` | `String` | `PeriodFrom` | |
| `periodTo` | `String?` | `PeriodTo` | nullable |
| `active` | `bool` | `Active` | |
| `creationDate` | `String` | `CreationDate` | |
| `modifiedDate` | `String` | `ModifiedDate` | |
| `toDate` | `String` | `ToDate` | |
| `fromoDate` | `String` | `FromDate` | Dart field spelled `fromoDate`; JSON key here is the correct `FromDate` |
| `transportationRouteController` | `TextEditingController` | — | seeded with `NameOfRoute` |
| `transportationDirectionController` | `TextEditingController` | — | seeded with `TransportationVehicleRouteDirectionId` |
| `transportationPeriodController` | `TextEditingController` | — | seeded with `Period` |
| `transportationLatitudeController` | `TextEditingController` | — | seeded with `DurationLatitude` |
| `transportationLongtitudController` | `TextEditingController` | — | seeded with `DurationLongtitud` (⚠️ typo `Longtitud`; also `Duration…` not `Direction…`) |
| `transportationToDateController` | `TextEditingController` | — | seeded with `ToDate` |
| `transportationFromDateController` | `TextEditingController` | — | seeded with `FromDate` |

Computed logic: controllers are constructed from JSON at parse time (side-effecting model — holds
disposable UI resources).

---

## 2. Passengers

### `RouteUsersModel`
`lib/DataAccessLayer/model/transportation/route_users_model.dart`

Envelope; `data` (`Data`) → list of `RouteUser`. Defines `toJson()`.

**`RouteUser`**

| Field | Type | JSON key (fromJson) | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `ID` | `.toString()` |
| `hrUserId` | `String` | `HrUserId` | `.toString()` |
| `firstName` | `String` | `FirstNane` | ⚠️ typo `FirstNane` (should be `FirstName`) — same key in `toJson` |
| `middleName` | `String` | `MiddleName` | |
| `lastName` | `String` | `LastName` | |
| `mobile` | `String` | `Mobile` | |
| `email` | `String` | `Email` | |
| `photo` | `String` | `Photo` | |
| `stationLatitude` | `String` | `DirectionLatitude` | `.toString()`; field renamed to `stationLatitude` |
| `stationLongitude` | `String` | `DirectionLongtitud` | ⚠️ typo `Longtitud`; field renamed to `stationLongitude` |
| `directionId` | `String` | `DirectionId` | `.toString()` |
| `directionName` | `String` | `DirectionName` | |

`toJson()` emits: `ID`, `FirstNane` (⚠️), `MiddleName`, `LastName`, `Mobile`, `Email`, `Photo`,
`Latitude` (from `stationLatitude`), `Longtitud` (⚠️, from `stationLongitude`), `DirectionName`.
Note the read keys (`DirectionLatitude`/`DirectionLongtitud`) differ from the write keys
(`Latitude`/`Longtitud`), and `hrUserId`/`directionId` are not written back.

### `RouteUsersForMobModel`
`lib/DataAccessLayer/model/transportation/route_users_for_mob_model.dart`

Mobile variant of the above; adds live location + attendance/registration fields. Defines `toJson()`.

**`RouteUserForMob`**

| Field | Type | JSON key (fromJson) | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `ID` | `.toString()` |
| `hrUserId` | `String` | `HrUserId` | `.toString()` |
| `firstName` | `String` | `FirstNane` | ⚠️ typo `FirstNane` |
| `middleName` | `String` | `MiddleName` | |
| `lastName` | `String` | `LastName` | |
| `mobile` | `String` | `Mobile` | |
| `email` | `String` | `Email` | |
| `photo` | `String` | `Photo` | |
| `stationLatitude` | `String` | `DirectionLatitude` | `.toString()` |
| `stationLongitude` | `String` | `DirectionLongtitud` | ⚠️ typo `Longtitud` |
| `directionId` | `String` | `DirectionId` | `.toString()` |
| `directionName` | `String` | `DirectionName` | |
| `latitude` | `String` | `Latitude` | live/passenger latitude, `.toString()` |
| `longtitud` | `String` | `Longtitud` | ⚠️ typo `Longtitud`; Dart field also spelled `longtitud` |
| `registeredInLine` | `bool` | `RegisteredInLine` | |
| `checkIn` | `String` | `CheckIn` | `.toString()` |
| `checkOut` | `String` | `CheckOut` | `.toString()` |

`toJson()` emits: `ID`, `FirstNane` (⚠️), `MiddleName`, `LastName`, `Mobile`, `Email`, `Photo`,
`Latitude` (from `stationLatitude`), `Longtitud` (⚠️, from `stationLongitude`), `DirectionName`
(mirrors `RouteUser.toJson`; the mobile-only fields are not serialized back).

---

## 3. Vehicles

### `TransportationVehiclesModel`
`lib/DataAccessLayer/model/transportation/transportation_vehicles_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationVehicleData`.

**`TransportationVehicleData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `vehicleTypeId` | `int` | `VehicleTypeId` | default 0 |
| `capacity` | `int` | `Capacity` | default 0 |
| `isApproved` | `bool` | `IsApproved` | |
| `approvedBy` | `String?` | `ApprovedBy` | defaults to `""` |
| `active` | `bool` | `Active` | |
| `creationDate` | `DateTime?` | `CreationDate` | `DateTime.tryParse`, nullable |
| `creationBy` | `int` | `CreationBy` | |
| `modifiedDate` | `DateTime?` | `ModifiedDate` | `DateTime.tryParse`, nullable |
| `modifiedBy` | `int` | `ModifiedBy` | |
| `creationByNavigation` | `dynamic` | `CreationByNavigation` | raw passthrough (EF navigation prop) |
| `modifiedByNavigation` | `dynamic` | `ModifiedByNavigation` | raw passthrough |
| `transportationLines` | `List<dynamic>` | `TransportationLines` | untyped list, default `[]` |
| `vehicleTypeName` | `String` | `VehicleTypeName` | |

Relationships: references vehicle type by `vehicleTypeId`/`vehicleTypeName`; `transportationLines`
carries related lines untyped.

### `VehicleTypesModel`
`lib/DataAccessLayer/model/transportation/transportation_vehicle_type_model.dart`

The only model whose `errors` is `List<String>` (from `Errors`) rather than `List<ErrorsModel>`.
`data` (`Data`) → list of `TransportationVehicleTypeData`.

**`TransportationVehicleTypeData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `type` | `String` | `Type` | |
| `active` | `bool` | `Active` | |
| `transportationVehicles` | `List<dynamic>` | `TransportationVehicles` | untyped, default `[]` |

### `TransportationCapacityNumberModel`
`lib/DataAccessLayer/model/transportation/transportation_capacity_number_model.dart`

Envelope with **single** `data` object (`Data` → `TransportationCapacityData`); falls back to an
all-zero instance when null.

**`TransportationCapacityData`** (all `int`)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `fullCapacity` | `int` | `FullCapacity` | |
| `actualCapacity` | `int` | `ActualCapacity` | |
| `capacityWithoutExpection` | `int` | `CapacityWithoutExpection` | ⚠️ typo `Expection` |
| `expectionNumFromOtherLines` | `int` | `ExpectionNumFromOtherLines` | ⚠️ typo `Expection` |
| `routeEmployeesToOtherLines` | `int` | `RouteEmployeesToOtherLines` | |

---

## 4. Lines & pricing

### `TransportationLinesModel`
`lib/DataAccessLayer/model/transportation/transportation_lines_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationLineData`.

**`TransportationLineData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `lineName` | `String` | `Name` | field renamed to `lineName` |
| `routesNum` | `int` | `RouteNum` | default 0 |

### `TransportationLineRepriceModel`
`lib/DataAccessLayer/model/transportation/transportation_line_reprice_model.dart`

Envelope with `paginationHeader`; top-level `errors` field typed as `List<dynamic>` but populated with
`ErrorsModel`. `data` (`Data`) → list of `ModifiedPriceData`. Contains a commented-out local
`PaginationHeader` (uses the shared one instead).

**`ModifiedPriceData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `int` | `Id` | non-null (throws if missing) |
| `isPercent` | `bool` | `IsPercent` | non-null |
| `approximateToFiveFlag` | `bool` | `ApproximateToFiveFlag` | non-null |
| `increaseCost` | `double` | `IncreaseCost` | `(num).toDouble()` |
| `approve` | `bool?` | `Approve` | nullable |
| `approvedBy` | `int?` | `ApprovedBy` | nullable |
| `approvedByName` | `String` | `ApprovedByName` | default `""` |
| `approvedDate` | `DateTime?` | `ApprovedDate` | `DateTime.tryParse`, nullable |
| `forAllLines` | `bool` | `ForAllLines` | non-null |
| `creationDate` | `DateTime` | `CreationDate` | `DateTime.parse` (required) |
| `creationBy` | `int` | `CreationBy` | |
| `creationName` | `String` | `CreationName` | default `""` |
| `transportationLineDetails` | `List<TransportationLineModifiedDetail>?` | `transportationLineDetails` | lowercase key, nullable nested list |

**`TransportationLineModifiedDetail`** (nested)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `transportationLineId` | `int` | `RouteId` | field named `transportationLineId`, key is `RouteId` |
| `transportationLineName` | `String` | `RouteName` | field named `transportationLineName`, key is `RouteName` |
| `priceBefore` | `double` | `PriceBefore` | default 0.0 |
| `priceAfter` | `double` | `PriceAfter` | default 0.0 |

Relationship: `ModifiedPriceData` 1→N `TransportationLineModifiedDetail` (per-line before/after price).

---

## 5. Suppliers & payments

### `AccountsAllMonthForSupplierModel`
`lib/DataAccessLayer/model/transportation/accounts_all_month_for_supplier_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `AccountsAllMonthForSupplierData`.

**`AccountsAllMonthForSupplierData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `monthName` | `String` | `MonthName` | |
| `accountId` | `int` | `AccountId` | |
| `supplierId` | `int` | `SupplierId` | |
| `supplierName` | `String` | `SupplierName` | |
| `month` | `int` | `MonthNum` | field named `month` |
| `routesNum` | `int` | `RoutesNum` | |
| `countOfRounds` | `int` | `CountOfcompleteRounds` | field named `countOfRounds` |
| `countOfHalfGoRounds` | `int` | `CountOfHalfGoRounds` | |
| `countOfHalfReturnRounds` | `int` | `CountOfHalfReturnRounds` | |
| `totalDue` | `double` | `TotalDue` | `.toDouble()` |
| `totalDeduct` | `double` | `TotalDeduct` | `.toDouble()` |
| `totalTaxesDeduct` | `double` | `TotalTaxesDeduct` | `.toDouble()` |
| `totalNormalDeduct` | `double` | `TotalNormalDeduct` | `.toDouble()` |
| `totalPaidadvance` | `double` | `TotalPaidadvance` | `.toDouble()` |
| `totalPaidNormal` | `double` | `TotalPaidNormal` | `.toDouble()` |
| `totalDueAfterPaid` | `double` | `TotalDueAfterPaid` | `.toDouble()` |
| `totalDuecompleteRound` | `double` | `TotalDuecompleteRound` | `.toDouble()` |
| `totalDueHalfGoRound` | `double` | `TotalDueHalfGoRound` | `.toDouble()` |
| `totalDueHalfReturnRound` | `double` | `TotalDueHalfReturnRound` | `.toDouble()` |
| `note` | `String` | `Note` | |

### `AllSupplierPaymentsModel`
`lib/DataAccessLayer/model/transportation/all_supplier_payments_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `AllSupplierPaymentsData`.

**`AllSupplierPaymentsData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `supplierName` | `String` | `SupplierName` | |
| `payment` | `String` | `Payment` | `.toString()` |
| `datePayment` | `String` | `DatePayment` | |
| `startDate` | `String` | `StartDate` | |
| `numberOfMonths` | `String` | `NumberOfMonths` | `.toString()` |
| `typeOfDebt` | `String` | `TypeOfDebt` | |
| `distributionSupplierPayments` | `List<DistributionSupplierPayments>` | `DistributionSupplierPayments` | nested list |

**`DistributionSupplierPayments`** (nested)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `payment` | `String` | `Payment` | `.toString()` |
| `monthNum` | `String` | `MonthNum` | `.toString()` |
| `yearNum` | `String` | `YearNum` | `.toString()` |

Relationship: `AllSupplierPaymentsData` 1→N `DistributionSupplierPayments` (payment split across
months/years).

### `TransportationDaysInMonthModel`
`lib/DataAccessLayer/model/transportation/transportation_days_in_month_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationDaysInMonthDate`. Defines
`toJson()`.

**`TransportationDaysInMonthDate`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `dateOfRound` | `String` | `DateOfRound` | |
| `dateOfCheckIn` | `String?` | `DateOfCheckIn` | defaults `""` |
| `dateOfCheckOut` | `String?` | `DateOfCheckOut` | defaults `""` |
| `dateOfOneWay` | `String?` | `DateOfOneWay` | defaults `""` |
| `roundsNum` | `double` | `RoundsNum` | `.toDouble()`, default 0 |
| `totalPriceOfDay` | `double` | `TotalPriceOfDay` | `.toDouble()`, default 0 |

`toJson()` re-emits the same PascalCase keys.

---

## 6. Attendance & dashboard

### `TransportationDashboardModel`
`lib/DataAccessLayer/model/transportation/transportation_dashboard_model.dart`

Envelope with **single** `data` object (`Data` → `TransportationDashboardData`); falls back to an
all-`"0"` instance when null. **All fields are `String`** (numeric counters stringified via
`.toString()`).

**`TransportationDashboardData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `transportLinesNum` | `String` | `TransportLinesNum` | default `"0"` |
| `vehiclesNum` | `String` | `VehiclesNum` | |
| `twoWayVehiclesNum` | `String` | `TwoWayVehiclesNum` | |
| `oneWayVehiclesNum` | `String` | `OneWayVehiclesNum` | |
| `hrUsersNum` | `String` | `HrUsersNum` | |
| `suppliersNum` | `String` | `SuppliersNum` | |
| `hrUsersPercent` | `String` | `HrUsersPercent` | |
| `checkInVehiclePercent` | `String` | `CheckInVehiclePercent` | |
| `checkOutVehiclePercent` | `String` | `CheckOutVehiclePercent` | |
| `oneWayVehiclePercent` | `String` | `OneWayVehiclePercent` | |
| `checkInVehiclesAttendanceNum` | `String` | `CheckInVehicleAttendanceNum` | key singular `Vehicle` |
| `checkOutVehiclesAttendanceNum` | `String` | `CheckOutVehicleAttendanceNum` | key singular `Vehicle` |
| `oneWayVehiclesAttendanceNum` | `String` | `OneWayVehicleAttendanceNum` | key singular `Vehicle` |
| `hrUsersAttendanceNum` | `String` | `HrUsersAttendanceNum` | |
| `allHrUsersNum` | `String` | `AllHrUsersNum` | |
| `allSuppliersNum` | `String` | `AllSuppliersNum` | |
| `vehicleTypeNum` | `String` | `VehiclesTypeNum` | field `vehicleTypeNum`, key `VehiclesTypeNum` |

### `TransportationDashboardAttendanceModel`
`lib/DataAccessLayer/model/transportation/transportation_dashboard_attendance_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationDashboardAttendanceData`.

**`TransportationDashboardAttendanceData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `firstName` | `String` | `FirstName` | correct spelling here |
| `middleName` | `String` | `MiddleName` | |
| `lastName` | `String` | `LastName` | |
| `transportationLineName` | `String` | `TransportionlineName` | ⚠️ typo `TransportionlineName` (missing `ta`, lowercase `line`) |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `supplierName` | `String` | `SupplierName` | |
| `supplierContactPersonName` | `String` | `supplierContactPersonName` | lowercase key |
| `vehicleType` | `String` | `VehicleType` | |
| `supervisorName` | `String` | `SupervisorName` | |
| `martialStatus` | `String` | `MaritalStatus` | Dart field spelled `martialStatus`; key correct `MaritalStatus` |
| `attendanceHistory` | `List<AttendanceHistory>` | `AttendanceHistory` | nested list |

**`AttendanceHistory`** (nested)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `date` | `String` | `Date` | `.toString()` |
| `checkIn` | `String` | `CheckIn` | `.toString()` |
| `checkOut` | `String` | `CheckOut` | `.toString()` |
| `attendanceStatus` | `bool` | `ISAttendace` | ⚠️ typo `ISAttendace` (should be `IsAttendance`) |

Relationship: `TransportationDashboardAttendanceData` 1→N `AttendanceHistory` (daily check-in/out per
passenger).

### `TouchAttendanceModel`
`lib/DataAccessLayer/model/transportation/touch_attendance_model.dart`

Envelope only; `data` is a plain `String` (`Data`, default `""`). Used for simple ack-style touch
attendance responses.

### `TransportationAttendanceExcelModel`
`lib/DataAccessLayer/model/transportation/transportation_attendance_excel_model.dart`

Envelope only; `data` is a plain `String` (`Data`, default `""`) — carries the exported attendance
Excel payload (e.g. base64/URL string).

---

## 7. Exceptions

### `TransportationExceptionModel`
`lib/DataAccessLayer/model/transportation/transportation_exception_model.dart`

Envelope with `paginationHeader` (built from `PaginationHeader` or `{}`); `data` (`Data`) → list of
`TransportationExceptionData`.

**`TransportationExceptionData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `String` | `Id` | `.toString()` |
| `hrUserId` | `String` | `HrUserId` | `.toString()` |
| `transportationVehicleRouteId` | `int` | `TransportationVehicleRouteId` | default 0 |
| `transportationLineName` | `String` | `TransportationLineName` | correct spelling here |
| `active` | `bool` | `Active` | |
| `creationDate` | `String` | `CreationDate` | |
| `creationBy` | `String` | `CreationBy` | `.toString()` |
| `creationName` | `String` | `CreationName` | |
| `transportationVehicleRouteDirectionId` | `String` | `TransportationVehicleRouteDirectionId` | `.toString()` |
| `transportationVehicleRouteDirectionName` | `String` | `TransportationVehicleRouteDirectionName` | |
| `fromDate` | `String` | `FromDate` | correct spelling here |
| `toDate` | `String` | `ToDate` | |
| `period` | `String` | `Period` | |
| `dayName` | `String` | `DayName` | |
| `latitudeExceptional` | `double` | `LatitudeExceptional` | `.toDouble()` |
| `longtitudExceptional` | `double` | `LongtitudExceptional` | ⚠️ typo `Longtitud` |
| `exceptionDate` | `String` | `ExceptionDate` | |
| `exceptionDatePeriod` | `String` | `ExceptionDatePeriod` | |
| `latitude` | `double` | `Latitude` | `.toDouble()` |
| `longtitud` | `double` | `Longtitud` | ⚠️ typo `Longtitud`; Dart field also `longtitud` |
| `exceptionDirectionId` | `String` | `ExceptionDirectionId` | `.toString()` |
| `exceptionDirectionName` | `String` | `ExceptionDirectionName` | |
| `hrUserName` | `String` | `HrUserName` | |
| `reason` | `String` | `ReasonException` | field `reason`, key `ReasonException` |
| `contactNumber` | `String` | `ContactNumber` | |

Relationship: links an HR user (`hrUserId`) to a vehicle route (`transportationVehicleRouteId`) and an
alternate exception direction/station.

---

## 8. Deductions

### `TransportationDeductionsModel`
`lib/DataAccessLayer/model/transportation/transportation_deductions_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationDeductionData`.

**`TransportationDeductionData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `id` | `int` | `Id` | default 0 |
| `supplierName` | `String` | `SupplierName` | |
| `supplierId` | `int` | `SupplierId` | |
| `routeName` | `String` | `TransportationVehicleRouteName` | field `routeName` |
| `routeId` | `int` | `TransportationVehicleRouteId` | field `routeId` |
| `serial` | `String` | `Serial` | default `"0"` |
| `deductionDate` | `String` | `Date` | field `deductionDate` |
| `deductionCreationDate` | `String` | `CreationDate` | field `deductionCreationDate` |
| `deductionAmount` | `double` | `DeductPerRound` | field `deductionAmount`, default 0.0 |
| `deductionReason` | `String` | `Cause` | field `deductionReason` |
| `creatorName` | `String` | `CreationName` | |
| `creatorId` | `int` | `CreationBy` | |
| `driverName` | `String` | `SupplierContentPersonIdName` | ⚠️ key `SupplierContentPersonIdName` (`Content` vs `Contact`) → mapped to `driverName` |
| `fromDate` | `String` | `FromDate` | |
| `toDate` | `String` | `ToDate` | |
| `typeOfDeduction` | `String` | `typeOfDeduction` | lowercase key |
| `routePrice` | `double` | `routePrice` | lowercase key, default 0.0 |

Relationship: ties a deduction to a supplier and a route.

---

## 9. Shifts

### `ShiftsModel`
`lib/DataAccessLayer/model/transportation/shifts/shifts_model.dart`

`fromJson` takes `Map<dynamic, dynamic>`. Envelope; `data` (`Data`) → list of `ShiftsData`. Shifts are
grouped by shift number, each group holding a list of individual `Shift` rows.

**`ShiftsData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `shiftNumber` | `String` | `shiftNumber` | lowercase key, `.toString()` |
| `shifts` | `List<Shift>` | `shifts` | lowercase key, nested list |

**`Shift`** (nested)

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `creationDate` | `String` | `CreationDate` | |
| `weekDayId` | `String` | `WeekDayId` | `.toString()` |
| `createdBy` | `String` | `CreatedBy` | `.toString()` |
| `id` | `String` | `Id` | `.toString()` |
| `from` | `String` | `From` | |
| `to` | `String` | `To` | |
| `branchId` | `String` | `BranchId` | `.toString()` |
| `modifiedDate` | `String` | `ModifiedDate` | |
| `modifiedBy` | `String` | `ModifiedBy` | `.toString()` |
| `shiftNumber` | `String` | `ShiftNumber` | `.toString()` |
| `active` | `bool` | — | **hardcoded `true`** (not read from JSON) |

Computed/default logic: `Shift.active` is always set to `true` regardless of payload.

Relationship: `ShiftsData` 1→N `Shift`; both carry `shiftNumber` (grouping key at the `ShiftsData`
level; each `Shift` also echoes its own `ShiftNumber`).

---

## 10. Reports & Excel exports

### `TransportationReportCostsModel`
`lib/DataAccessLayer/model/transportation/transportation_report_costs_model.dart`

Envelope with `paginationHeader`; `data` (`Data`) → list of `TransportationReportCostData`.

**`TransportationReportCostData`**

| Field | Type | JSON key | Notes |
| --- | --- | --- | --- |
| `lineName` | `String` | `LineName` | |
| `nameOfRoute` | `String` | `NameOfRoute` | |
| `supplierName` | `String` | `SupplierName` | |
| `lineCost` | `double` | `LineCost` | default 0.0 |
| `onWay` | `bool` | `OneWay` | field `onWay`, key `OneWay` |
| `countOfRounds` | `int` | `CountOfround` | ⚠️ odd casing `CountOfround` (lowercase `round`) |
| `deductionPerRound` | `int` | `DeductPerRound` | |
| `deductionRoundNum` | `int` | `DeductRoundNum` | |
| `netCost` | `double` | `NetCost` | default 0.0 |

### `TransportationExcelModel`
`lib/DataAccessLayer/model/transportation/transportation_costs_excel_model.dart`

Note: file is named `transportation_costs_excel_model.dart` but the class is `TransportationExcelModel`.
Envelope only; `data` is a plain `String` (`Data`, default `""`) — carries the exported costs Excel
payload.

---

## Cross-cutting notes

- **Preserved backend key typos** (do not silently rename): `FirstNane`, `Longtitud` /
  `DirectionLongtitud` / `LongtitudExceptional` / `DurationLongtitud`, `ActualUserAttedance`,
  `FromoDate`, `ISAttendace`, `TransportionlineName`, `Expection` (`CapacityWithoutExpection`,
  `ExpectionNumFromOtherLines`), `BuSupervisorId`, `SupplierContentPersonIdName`, `CountOfround`.
- **Envelope inconsistencies**: `VehicleTypesModel` uses `List<String>` errors; `TransportationLineRepriceModel`
  declares `List<dynamic>` errors; `TransportationRoutesListForPassengerModel` keeps errors as a raw
  `List`. `ShiftsModel`, `TransportationRoutesListForPassengerModel`, and `ErrorsModel`/`PaginationHeader`
  parse from `Map<dynamic, dynamic>`.
- **Null-safety hazards**: `PaginationHeader.fromJson` and the `DateTime.parse(json['PeriodFrom'])` /
  `DateTime.parse(json['CreationDate'])` calls (in `TransportationRoutesModel`,
  `TransportationAllRouteModel`, `TransportationLineRepriceModel`) will throw if those keys are missing
  or malformed — they are not defensively guarded.
- **UI-coupled model**: `TransportationRoutesListForPassengerModel` embeds `TextEditingController`s and
  imports Flutter — atypical for the DataAccessLayer, and its controllers should be disposed by the
  consumer.
