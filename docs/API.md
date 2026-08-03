# Transportation Module — API Reference

> Related docs: [PRD.md](PRD.md) · [SCREENS.md](SCREENS.md) · [MODELS.md](MODELS.md) · [RULES.md](RULES.md) · [DEPENDENCIES.md](DEPENDENCIES.md) · [ARCHITECTURE.md](ARCHITECTURE.md)

This document is the HTTP reference for the Transportation module. Every endpoint below is invoked from `TransportationProvider` (`lib/Providers/transportation_system_provider.dart`) through `HttpHelper` (`lib/DataAccessLayer/http_helper.dart`). Provider-side state mapping is in [PROVIDERS.md](PROVIDERS.md).

---

## 1. Base URL construction

Endpoints are declared in `lib/DataAccessLayer/end_points_constant.dart` as `Uri.https(host, path)`. Hosts and path prefixes come from `lib/DataAccessLayer/api_url.dart` (`ApiUrl`).

```
Uri.https(ApiUrl.apiUrlCoreApiNewPart1, '${ApiUrl.apiUrlCorePart2Transportation}/<Action>')
```

| Part | `ApiUrl` field | Current value |
|---|---|---|
| Host | `apiUrlCoreApiNewPart1` | `testcoreapi.garassolutions.com` (Test — Garas prod, St Mark, and a dedicated transportation host are commented alternatives) |
| Transportation prefix | `apiUrlCorePart2Transportation` | `/api/Transportation` |
| Shifts prefix | `apiUrlCoreApiPart2HR` | `/HR` (shifts live under `/HR/BranchSchedule`) |

So a typical Transportation request resolves to:

```
https://testcoreapi.garassolutions.com/api/Transportation/<Action>
```

All requests use HTTPS (`Uri.https`). The scheme, host and path are fixed in the constant; **filters and pagination are added as request headers, not query parameters** (see §3).

---

## 2. Authentication & standard headers

Auth is credential-based (company + user token stored in `SharedPreferences`), not bearer OAuth. `HttpHelper` injects the standard headers on every call and merges the caller-supplied `headers` map on top:

```dart
{
  "Accept": "application/json",
  "Content-Type": "application/json",
  "CompanyName": <SharedPref companyName>,   // GET: always; POST/PUT/DELETE: when addCompanyName == true
  "UserToken":   <SharedPref userToken>,     // GET: always; POST/PUT/DELETE: when addToken == true
  ...callerHeaders,
}
```

- **GET** (`getData`): always sends `CompanyName` + `UserToken`; the request is only attempted when both are non-empty and network is available.
- **POST / PUT / MULTIPART / DELETE**: send `CompanyName` only if `addCompanyName == true`, `UserToken` only if `addToken == true`. Every Transportation write passes both flags `true`. If a required credential is empty the call aborts, shows `"Please, Login."`, and redirects to the sign-in route.

Credentials are read from `SharedPrefHelper` keys `companyName` and `userToken` (`SharedPrefConst`).

---

## 3. Request conventions

**GET filters & pagination are sent as HTTP headers, not query strings.** `getData` forwards its `headers` map straight into `http.get`. There are effectively no query parameters in this module (`parameters` is unused by Transportation calls). Common header keys:

| Header | Meaning |
|---|---|
| `PageNo` | 1-based page index |
| `NoOfItems` | page size |
| `RouteId` / `RouteID` / `routeId` | route filter (casing varies per endpoint — see tables) |
| `HrUserId` | HR user filter |
| `SupplierId`, `supplierContactPersonId` | supplier / contact filters |
| `TransportionlineId` / `pTransportionlineId` | line filter (note the misspelling in the API contract; casing varies) |
| `serialBus` | bus serial filter |
| `DateSerach` | single-date filter, `yyyy-MM-dd` (misspelled in the API contract) |
| `FromDate` / `ToDate` / `dateFrom` / `dateTo` | date-range filters, `yyyy-MM-dd` |
| `Month`, `Year`, `Period` | period filters |
| `Approve`, `AttendaceFlag` | boolean-as-string filters |
| `BranchId` | branch filter (shifts) |

**Deletes are POSTs with an `Id` header.** There is no HTTP `DELETE` used in this module for CRUD deletes. Every delete calls `HttpHelper.postData` with an empty JSON body `{}` and the target id in a header (`headers: {"Id": <id>}`). Approvals follow the same style (`{"Id": id, "Approve": "true"}`).

**Bulk create/nested payloads** (stations, employees, passenger routes) POST a JSON body containing a `Data` array, with the parent id passed as a header (e.g. `TransportationVehicleRouteId`, `hrUserId`).

**Multipart** (`postMultipartRequestData`) is used for shifts (`shiftDtos[i].*` string fields) and Excel import (`file` field). `Content-Type` is still declared as `application/json` in the helper but the body is a `MultipartRequest`.

**Dates** are formatted with `intl`'s `DateFormat("yyyy-MM-dd")`, except route `PeriodFrom` which is sent as `toUtc().toIso8601String()`.

---

## 4. Response envelope & models

All endpoints return a JSON envelope decoded by a `*Model.fromJson` factory. The shared shape is:

```jsonc
{
  "result": true,
  "errors": [ { "errorCode": "...", "errorMsg": "..." } ],
  "data": ... ,                 // object, array, or base64 string
  "paginationHeader": { "currentPage": 1, "itemsPerPage": 20, "totalItems": 0, "totalPages": 0 } // list endpoints only
}
```

- Write endpoints return `PostResponseModel` (`result`, `errors`, `id`, `message`).
- Excel endpoints return `TransportationExcelModel` / `TransportationAttendanceExcelModel` with the file as a base64 string in `data`.
- List endpoints carry a `paginationHeader`.

---

## 5. Error handling & retry conventions

Implemented in `HttpHelper.getData` / `postData` and `CheckResponse` (`lib/DataAccessLayer/response_helper.dart`):

- **Global loader**: `DashboardProvider.updateLoaderState(true)` before the request; `(false)` on every terminal path. (There is no per-feature loading flag — see [PROVIDERS.md](PROVIDERS.md).)
- **Network check first**: `CheckNetworkConnection` (`network_helper.dart`) verifies connectivity; if offline it shows a warning dialog and the call resolves to an empty model.
- **Session-expiry codes** `Err-P1`, `Err-P2`, `Err-P200`: `CheckResponse` treats these as auth failure — it clears `SharedPreferences` (preserving remember-me credentials when set) and redirects to `signInRoute`. GET does not retry on `Err-P2`.
- **Retry ×3**: `numOfTriesApi = 3` (`constants.dart`). On a failed GET (non-200, decode error, or a non-session error), `getData` waits `apiRecallDuration = 10` seconds and recursively retries with `tryNum + 1`, up to 3 attempts. POST/PUT/DELETE do **not** auto-retry.
- **User feedback**: failures raise a `showSnackBar` with the server `errorMsg`, or `"An Error Occurred : <status>"` for non-200, or `"Please check Your Internet Connection"` on network exhaustion.
- Non-200 POST/PUT/DELETE resolve to `parseModel({})` (an empty model with `result == false`).

---

## 6. Endpoint reference

Host = `testcoreapi.garassolutions.com`. Prefix `/api/Transportation` unless noted. "Method" is the HTTP verb actually issued by `HttpHelper`. Provider methods are documented in [PROVIDERS.md](PROVIDERS.md).

### 6.1 Vehicles & vehicle types

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationVehicleType` | `getAllVehicleType` | GET | — | — | `VehicleTypesModel` | `getVehicleTypes` |
| `getTransportationVehicle` | `getAllTransportationVehicle` | GET | `PageNo`, `NoOfItems`, opt `TransportionlineId`, `SupplierId`, `supplierContactPersonId`, `serialBus`, `DateSerach` | — | `TransportationVehiclesModel` | `getTransportationVehicles` |
| `addTransportationVehicle` | `AddTransportationVehicle` | POST | auth | `VehicleTypeId`, `Capacity`, `Active` | `PostResponseModel` | `addTransportationVehicle` |
| `updateTransportationVehicle` | `UpdateTransportationVehicle` | POST | auth | `Id`, `VehicleTypeId`, `Capacity`, `Active` | `PostResponseModel` | `updateTransportationVehicle` |
| `deleteTransportationVehicle` | `DeleteTransportationVehicle` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationVehicle` |
| `approveTransportationVehicle` | `ApproveTransportationVehicle` | POST | `Id`, `Approve` | `{}` | `PostResponseModel` | `approveTransportationVehicle` |
| `addTypeVehicle` | `AddVehicleType` | POST | auth | `Type`, `Active` | `PostResponseModel` | `addVehicleType` |

### 6.2 Lines

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getAllTransportationLine` | `getAllTransportationLine` | GET | `PageNo`, `NoOfItems` | — | `TransportationLinesModel` | `getTransportationLines` |
| `addTransportationLine` | `AddTransportationLine` | POST | auth | `LineName` | `PostResponseModel` | `addTransportationLine` |
| `updateTransportationLine` | `UpdateTransportationLine` | POST | auth | `Id`, `LineName` | `PostResponseModel` | `updateTransportationLine` |
| `deleteTransportationLine` | `DeleteTransportationLine` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationLine` |
| `approveTransportationLine` | `ApproveTransportationLine` | POST | `Id`, `Approve` | `{}` | `PostResponseModel` | `approveTransportationLine` |

### 6.3 Repricing

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `rePriceTransportationLine` | `ModifyPriceOfTransportationLine` | POST | auth | `IncreaseCost`, `IsPercent`, `ForAllLines`, `ApproximateToFiveFlag`, `TransportationLineIds` (route ids), `StartDate` | `PostResponseModel` | `rePricingTransportationRoute` |
| `getAllModifiedTransportationLines` | `getAllModifyPriceOfTransportationLine` | GET | `PageNo`, `NoOfItems`, opt `Approve` | — | `TransportationLineRepriceModel` | `getTransportationLineRePricing` |
| `updatePriceOfTransportationLine` | `UpdatePriceOfTransportationLine` | POST | `Id` | `{}` | `PostResponseModel` | `updatePriceOfTransportationLine` |
| `rejectRepriceTransportationLine` | `RejectUpdatePrice` | POST | `Id` | `{}` | `PostResponseModel` | `rejectRepriceTransportationLine` |

### 6.4 Routes

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getAllTransportationRoute` | `getAllTransportationRoute` | GET | `PageNo`, `NoOfItems`, opt `pTransportionlineId`, `SupplierId`, `supplierContactPersonId`, `serialBus`, `DateSerach` | — | `TransportationAllRouteModel` | `getAllTransportationRoute` |
| `getTransportationRoute` | `getTransportationRoute` | GET | `RouteId` | — | `TransportationRouteModel` | `getTransportationRoute` |
| `getRoutesByHrUserId` | `getTransportationRouteByHrUser` | GET | `HrUserId` | — | `TransportationRoutesModel` | `getTransportationRoutesByHrUserId` |
| `addTransportationRoute` | `AddTransportationRoute` | POST | auth | `SupplierId`, opt `SupplierContactPersonId`, `BranchScheduleId`, opt `TransportationLineId`, opt `HrUserId`, `PeriodFrom` (ISO-UTC), `Active`, opt `FromDate`/`ToDate`, `NameOfRoute`, `LineCost`, `OneWay`, `TransportationVehicleId`, opt `TransportationLineName` | `PostResponseModel` | `addTransportationRoute` |
| `updateTransportationRoute` | `UpdateTransportationRoute` | POST | auth | Same as add + `Id` | `PostResponseModel` | `updateTransportationRoute` |
| `deleteTransportationRoute` | `DeleteTransportationRoute` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationRoute` |

### 6.5 Stations (Directions)

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationDirection` | `GetTransportationDirection` | GET | `routeId` | — | `TransportationDirectionsModel` | `getTransportationRouteStations` |
| `addTransportationDirection` | `AddTransportationDirection` | POST | `TransportationVehicleRouteId` | `Data:[{ RouteDirection, Description, opt Latitude, opt Longtitud, Active }]` | `PostResponseModel` | `addTransportationStation` |
| `updateTransportationDirection` | `UpdateTransportationDirection` | POST | auth | `Id`, `TransportationVehicleRouteId`, `RouteDirection`, `Description`, opt `Latitude`, opt `Longtitud`, `Active` | `PostResponseModel` | `updateTransportationStation` |
| `deleteTransportationDirection` | `DeleteTransportationDirection` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationDirection` |

### 6.6 Passengers (route employees)

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationEmployeeList` | `GetTransportationEmployeeList` | GET | `RouteId` | — | `RouteUsersModel` | `getRouteUsers` |
| `getTransportationEmployeeListForMob` | `GetTransportationEmployees` | GET | `RouteId`, `Period` | — | `RouteUsersForMobModel` | `getRouteUsersForMob` |
| `addTransportationEmployee` | `AddTransportationEmployee` | POST | `TransportationVehicleRouteId` | `Data:[{ hrUserId, opt TransportationVehicleRouteDirectionId, Active, period }]` | `PostResponseModel` | `addTransportationEmployee` |
| `updateTransportationEmployee` | `UpdateTransportationEmployee` | POST | auth | `Id`, `hrUserId`, `TransportationVehicleRouteId`, `TransportationVehicleRouteDirectionId`, `Active` | `PostResponseModel` | `updateTransportationEmployee` |
| `deleteTransportationEmployee` | `DeleteTransportationEmployee` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationEmployee` |
| `getTransportationRoutesListForPassenger` | `getTransportationRoutesForHrUser` | GET | `HrUserId` | — | `TransportationRoutesListForPassengerModel` | `getTransportationRoutesListForPassenger` |
| `addTransportationRoutesListForPassenger` | `AddRoutesForEmployee` | POST | `hrUserId` | `Data:[{ RouteId, Active, opt FromDate/ToDate/Period/DurationLatitude/DurationLongtitud/TransportationVehicleRouteDirectionId }]` | `PostResponseModel` | `addTransportationRouteForPassenger` |
| `updateTransportationRoutesListForPassenger` | `UpdateRouteEmployee` | POST | auth | `Id`, `TransportationVehicleRouteId`, `HrUserId`, `Active`, opt direction/date/period/lat/long | `PostResponseModel` | `updateTransportationRouteForPassenger` |
| `deleteTransportationRoutesListForPassenger` | `DeleteRouteEmployee` | POST | `Id` | `{}` | `PostResponseModel` | `deleteTransportationRouteForPassenger` |

### 6.7 Shifts  (prefix `/HR/BranchSchedule`)

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getShifts` | `GetShifts` | GET | opt `BranchId` | — | `ShiftsModel` | `getShifts` |
| `addListOfShifts` | `AddListOfShifts` | MULTIPART | auth | `shiftDtos[i].{Id,From,To,ShiftNumber,WeekDayId,BranchId,Active}` per active day | `PostResponseModel` | `addShifts` |
| `updateShifts` | `UpdateShift` | MULTIPART | auth | leading `Id` field + `shiftDtos[i].*` fields | `PostResponseModel` | `updateShifts` |

### 6.8 Exceptions

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationExceptions` | `GetEmployeeException` | GET | opt `HrUserId`, opt `Id`, `PageNo`, `NoOfItems` | — | `TransportationExceptionModel` | `getTransportationExceptions` |
| `addException` | `AddException` | POST | auth | `transportationVehicleRouteId`, `hrUserId`, `Active`, `reasonException`, `contactNumber`, `Period`; period-mode `FromDate`/`ToDate`/`DayName` **or** `ExceptionDate`; location-mode `Latitude`/`Longtitud` **or** `TransportationVehicleRouteDirectionId` | `PostResponseModel` | `addException` |
| `updateTransportationException` | `UpdateException` | POST | auth | Same as add + `id` | `PostResponseModel` | `updateTransportationException` |
| `getTransportationCapacityNumber` | `GetCapacityNumbers` | GET | `RouteID`, `Period` | — | `TransportationCapacityNumberModel` | `getTransportationCapacityNumber` |

### 6.9 Attendance & Dashboard

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationDashboardNumbers` | `DashBoard` | GET | `DateSerach`, opt `TransportionlineId`, `SupplierId`, `supplierContactPersonId`, `serialBus` | — | `TransportationDashboardModel` | `getDashboardNumbers` |
| `getTransportationDashboardAttendance` | `DashBoardForAttendenceDuration` | GET | `FromDate`, `ToDate`, opt `DateSerach`, `TransportionlineId`, `SupplierId`, `supplierContactPersonId`, `RouteId`, `serialBus`, `AttendaceFlag`, `PageNo`, `NoOfItems` | — | `TransportationDashboardAttendanceModel` | `getDashboardAttendance` |
| `getTransportationDashboardAttendanceExcel` | `AttendanceExcell` | GET | `DateSerach`, opt line/supplier/contact/serial, opt `Year` | — | `TransportationAttendanceExcelModel` (base64) | `getDashboardAttendanceExcel` |
| `addTransportationUsersAttendance` | `AddUsersAttedance` | POST | auth | `type`, `serial`, `CheckInOrCheckOut`, opt `CheckIn/OutLatitude`, `CheckIn/OutLongtitud`, `CheckIn/OutRouteDirectionId` | `PostResponseModel` | `addUsersAttendance` |
| `getTouchAttendance44` | `AddUsersAttedance44` | POST | auth | `{}` | `PostResponseModel` | `getTouchAttendance44` |

### 6.10 Deductions

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getAllTransportationDeduction` | `getAllDeduction` | GET | `PageNo`, `NoOfItems`, opt `SupplierId`, `RouteId`, `FromDate`, `ToDate` | — | `TransportationDeductionsModel` | `getDeductions` |
| `addTransportationDeduction` | `AddDeduction` | POST | auth | `SupplierId`, `Serial`, `TransportationVehicleRouteId`, `DeductPerRound`, `Cause`, `DateOfDeduction`, `TypeOfDedct` (`Taxes`/`Normal`) | `PostResponseModel` | `addDeduction` |
| `updateTransportationDeduction` | `UpdateDeduction` | POST | auth | Same as add + `Id` | `PostResponseModel` | `updateDeduction` |

### 6.11 Costs

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getTransportationReportCostLines` | `ReportCostOfLines` | GET | `PageNo`, `NoOfItems`, opt `SupplierId`, `TransportionlineId`, `supplierContactPersonId`, `serialBus`, `dateFrom`, `dateTo`, `DateSerach` | — | `TransportationReportCostsModel` | `getReportCosts` |
| `getTransportationReportCostLinesExcel` | `ReportCostOfLinesExcell` | GET | Same filters as above | — | `TransportationExcelModel` (base64) | `getReportCostsExcel` |

### 6.12 Suppliers & payments

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `getAllSupplierPayment` | `getAllSupplierPayment` | GET | `PageNo`, `NoOfItems`, opt `FromDate`, `ToDate`, `SupplierId` | — | `AllSupplierPaymentsModel` | `getAllSupplierPayment` |
| `addSupplierPayment` | `AddSupplierPayment` | POST | auth | `SupplierId`, `Payment`, `DatePayment`, opt `StartDate`, `TypeOfDebt`, opt `NumberOfMonths` | `PostResponseModel` | `addSuplierPayment` |
| `getAccountsAllMonthForSupplier` | `AccountsAllMonthsForSupplier` | GET | `PageNo`, `NoOfItems`, opt `SupplierId`, `RouteId`, `Month`, `Year` | — | `AccountsAllMonthForSupplierModel` | `getAccountsAllMonthForSupplier` |
| `accountsAllRoundsForSupplier` | `AccountsAllRoundsForSupplier` | GET | `year`, `PageNo`, `NoOfItems`, `Month`, `SupplierId`, opt `RouteId` | — | `TransportationDaysInMonthModel` | `getTransportationDaysInMonth` |

### 6.13 Excel import / export

| Constant | Path suffix | HTTP | Key headers | Body | Response model | Provider method |
|---|---|---|---|---|---|---|
| `downloadExcelUserWithRoutesTemplate` | `downloadExcelUserWithRoutesTemplete` | GET | — | — | `TransportationExcelModel` (base64) | `downloadExcelUserWithRoutesTemplete` |
| `downloadExcelUserActiveTemplate` | `downloadExcelUserActiveTemplete` | GET | — | — | `TransportationExcelModel` (base64) | `downloadExcelUserActiveTemplete` |
| `insertUsersWithRoutesExcel` | `InsertUsersWithRoutesExcel` | MULTIPART | auth | `file` (xlsx/xls) | `PostResponseModel` | `insertUsersWithRoutesExcel` |
| `insertUserNotActiveExcel` | `InsertUserNotActiveExcel` | MULTIPART | auth | `file` (xlsx/xls) | `PostResponseModel` | `insertNotActive` |

---

## 7. Endpoint totals

- **60 Transportation endpoints** consumed by `TransportationProvider`, of which:
  - **25 GET** (reads, all filters/pagination via headers)
  - **31 POST** (writes; includes deletes-as-POST with `Id` header and approvals)
  - **4 MULTIPART POST** (2 shift writes, 2 Excel imports)
- 57 endpoints under `/api/Transportation`; 3 under `/HR/BranchSchedule` (shifts).
- Provider-side method mapping and state fields: see [PROVIDERS.md](PROVIDERS.md).
