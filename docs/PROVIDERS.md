# Transportation Module — State Management (Providers)

> Related docs: [PRD.md](PRD.md) · [SCREENS.md](SCREENS.md) · [MODELS.md](MODELS.md) · [RULES.md](RULES.md) · [DEPENDENCIES.md](DEPENDENCIES.md) · [ARCHITECTURE.md](ARCHITECTURE.md)

This document describes the state-management layer of the Transportation module. All API calls in this document are cross-referenced in [API.md](API.md).

---

## 1. Overview

| | |
|---|---|
| **Class** | `TransportationProvider` |
| **File** | `lib/Providers/transportation_system_provider.dart` |
| **Base class** | `ChangeNotifier` (Provider / `provider` package) |
| **Data-access helper** | `HttpHelper` (`lib/DataAccessLayer/http_helper.dart`) |
| **Endpoint registry** | `EndPointsConst` (`lib/DataAccessLayer/end_points_constant.dart`) |

`TransportationProvider` is the single ChangeNotifier that backs every Transportation screen (vehicles, lines, routes, stations, passengers, repricing, deductions, exceptions, attendance/dashboard, costs, shifts, supplier payments). It owns:

- **Response models** returned by API calls (each feature has one or more strongly-typed model fields).
- **Filtered/accumulated lists** used for paginated lazy-loading UIs (`filteredTransportation*List`).
- **Form state**: dozens of `TextEditingController`s plus boolean toggles and selected-id strings used by add/edit dialogs.
- **API methods**: each performs one HTTP call through `HttpHelper`, assigns the parsed model to a state field, calls `notifyListeners()`, and returns the boolean `result` flag.

### Loading indicator

There is **no per-feature loading flag** on this provider. The global loader is toggled centrally inside `HttpHelper` via `DashboardProvider.updateLoaderState(true/false)`: it is switched on before a request starts and off when the request resolves (success, error, or session expiry). Individual API methods can opt out by passing `deActiveLoader: true` to the `HttpHelper` call (none of the Transportation methods currently do).

### notifyListeners convention

Almost every method — API calls, setters, controller-population helpers, list mutators — ends with `notifyListeners()`. For API methods it fires **once, after** the response model has been assigned to the state field, so the UI rebuilds with the new data. Pure setters fire immediately after mutating the field. `updateCurrentPage(int)` is the only mutator that does **not** call `notifyListeners()`.

---

## 2. State fields by feature area

### Pagination / shared
- `int currentPage = 1`, `int itemsPerPage = 20`
- `TextEditingController numOfItemsController` (default `"20"`)
- Selected-id scratch strings shared across forms: `vehicleTypeId`, `stationId`, `hrUserId`, `supervisorId`, `supplierId`, `supplierContactPersonId`, `transportationLineId`, `branchId`, `shiftId`, `fromTime`, `toTime`

### Vehicles
- `TransportationVehiclesModel transportationVehicles` — paginated raw response
- `List<TransportationVehicleData> filteredTransportationVehiclesList` — accumulated across pages
- `VehicleTypesModel vehicleTypes`
- Response holders: `addTransportationVehicleResponse`, `updateTransportationVehicleResponse`, `deleteTransportationVehicleResponse`, `approveTransportationVehicleResponse` (all `PostResponseModel`)
- Form state: `vehicleTypeController`, `vehicleCapacityController`, `vehicleTypeId`, `approvalVehicle`, `activeVehicle`, `typeNameController`, `typeActive`

### Lines
- `TransportationLinesModel transportationLines`
- `List<TransportationLineData> filteredTransportationLinesList`
- Response holders: `addTransportationLineResponse`, `updateTransportationLineResponse`, `deleteTransportationLineResponse`, `approveTransportationLineResponse`
- Form state: `lineNameController`, `lineCostController`, `activeLine`, `approvalLine`

### Repricing
- `TransportationLineRepriceModel rePricedTransportationLines`
- `List<TransportationRouteData> transportationRouteForRePricing` — items selected for a repricing batch
- Toggles: `isPercent`, `forAllLines`, `approximateToFiveFlag`, `viewApprovedRePricing`, `viewRejectedRePricing`
- Form/paging: `rePrintingAmountController`, `repricingStartDateController`, `repricingStartDateTime`, `viewRepriceNumOfitems`, `viewRepriceCurrentPage`, `viewRepriceNumOfitemsController`

### Routes
- `TransportationAllRouteModel transportationRoutes`
- `List<TransportationRouteData> filteredTransportationRoutesList`
- `TransportationRouteModel transportationRoute` (single route detail)
- `TransportationRouteData routeForScreen`, `monthDetailsRoute` (selected route passed between screens)
- `TransportationRoutesModel transportationRoutesByHrUserId`
- `deleteTransportationRouteResponse`
- Form state: `transportationRouteNameController`, `routeFromTimeController`, `routeToTimeController`, `currencyController` (default `"EGP"`), `oneWay`, `activeDirection`, `selectedPeriodForm`, `routeIdToBeGet`, route-type toggles `isGo`/`isReturn`/`isGoAndReturn`, `chooseTransportationLine`

### Stations (a.k.a. Directions)
- `TransportationDirectionsModel transportationRouteStations`
- Response holders: `addTransportationStationResponse`, `updateTransportationStationResponse`, `deleteTransportationDirectionResponse`
- Form state: `stationNameController`, `descriptionController`, `stationLatitudeController`, `stationLongitudeController`

### Passengers (route employees)
- `RouteUsersModel routeUsers`, `RouteUsersForMobModel routeUsersForMob`
- `TransportationRoutesListForPassengerModel transportationRoutesListForPassenger` + backing `transportationRoutesListForPassengerData`
- Response holders: `addTransportationEmployeeResponse`, `updateTransportationEmployeeResponse`, `deleteTransportationEmployeeResponse`, `addTransportationRouteForPassengerResponse`, `updateTransportationRouteForPassengerResponse`, `deleteTransportationRouteForPassengerResponse`
- Form state: `hrUserController`, `hrUserId`, `stationId`

### Shifts
- `ShiftsModel shifts`, `List<ShiftsData> addEditShiftsList`
- Response holders: `addShiftResponse`, `updateShiftResponse`
- Form state: `shiftNameController`

### Deductions
- `TransportationDeductionsModel transportationDeductionsModel`
- Response holders: `addTransportationDeductionResponse`, `updateTransportationDeductionResponse`
- Toggles: `isDeductionTaxes`, `isDeductionPercent`
- Form state: `deductionSupplierController`, `deductionDateController`, `deductionVehicleController`, `deductionRoutePriceController`, `deductionRoutePercentController`, `deductionrouteController`, `deductionPerRoundController`, `deductionReasonController`, `deductionSupplierId`, `deductionVehicleSerial`, `deductionRouteId`, `deductionDate`

### Exceptions
- `TransportationExceptionModel transportationExceptions`
- `TransportationCapacityNumberModel transportationCapacityNumberModel`
- Response holders: `transportationAddExceptionModel`, `transportationUpdateExceptionModel`
- Toggles: `isExceptionsPeriod`, `isExceptionLocation`, `isGetInAndOut`
- Selected ids: `exceptionId`, `exceptionHrUserId`, `exceptionRouteId`, `exceptionGetInStationId`, `exceptionGetOutStationId`, `exceptionWeekdaysId`, plus `exceptionGetIn/Out Latitude/Longitude`
- ~16 exception `TextEditingController`s (`hrUserExceptionController`, `routeExceptionController`, `weekDaysExceptionController`, `periodExceptionController`, `exceptionDayController`, `durationFrom/ToExceptionController`, `getIn/OutLocationLat/LongExceptionController`, `reasonExceptionController`, `contactNumberExceptionController` [default `"+20"`], `getIn/OutStationExceptionController`)
- `List<String> transportationShiftsList = ['Go', 'Return', 'Both']`

### Attendance / Dashboard
- `TransportationDashboardModel transportationDashboardNumbersResponse`
- `TransportationDashboardAttendanceModel transportationDashboardAttendanceResponse`
- `TransportationAttendanceExcelModel transportationAttendanceExcel`
- `PostResponseModel transportationAddUsersAttendace`, `touchAttendanceModel`
- Filters: `dashboardTransportationId`, `dashboardSupplierId`, `dashboardContactPersonId`, `dashboardRouteId`, `busSerialDashboard`, `dashBoardDateTime`, `dashBoardFromDateTime`, `dashBoardToDateTime`, `fromDate`, `toDate`
- Attendance paging/flags: `dashboardAttendanceNoOfItems`, `dashboardAttendancePageNo`, `dashboardAttendanceAttended`, `dashboardAttendanceAbsent`, computed getter `dashboardAttendanceFlag` (null when both/neither selected)
- ~12 dashboard `TextEditingController`s (vehicle, contact person, transport line, supplier, route, date, from/to date, etc.)

### Costs
- `TransportationReportCostsModel transportationReportCostsModel`
- `TransportationExcelModel transportationCostsExcel`

### Suppliers / Payments
- `AllSupplierPaymentsModel allSupplierPaymentsModel`
- `AccountsAllMonthForSupplierModel accountsAllMonthForSupplierModel`
- `TransportationDaysInMonthModel transportationDaysInMonthModel`
- `PostResponseModel addSupplierPayment`
- Toggle: `isSupplierPaymentAdvanced`
- Form state: `supplierNameController`, `supplierContactPersonController`, `addPaymentsMonthNumController`, `addPaymentsStartDateController`, `addPaymentsPaymentDateController`, `addPaymentsPaymentController`, `accountSupplierRouteController`, `accountSupplierController`, `accountSupplierMonthController`, `accountSupplierYearController` (default current year), `accountSupplierRouteId`, `accountSupplierId`

### Excel import/export
- `TransportationExcelModel downloadExcelUserWithRoutes`, `downloadExcelUserActive`
- `PostResponseModel insertUsersRoutesExcel`, `insertNotActiveExcel`

---

## 3. Public methods by feature

Legend: **(GET)** = read via `HttpHelper.getData` · **(POST)** = write via `HttpHelper.postData` · **(MULTIPART)** = `HttpHelper.postMultipartRequestData`. All methods take `required BuildContext context` and return `Future<bool>` (the model's `result`) unless noted. See [API.md](API.md) for full endpoint paths, headers and payloads.

### Vehicles
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getVehicleTypes` **(GET)** | Load vehicle-type dropdown | `getTransportationVehicleType` | `vehicleTypes` |
| `getTransportationVehicles` **(GET)** | Paginated list (filters: line/supplier/contact/serial/date) — resets or appends to `filteredTransportationVehiclesList` by page | `getTransportationVehicle` | `transportationVehicles`, `filteredTransportationVehiclesList` |
| `addTransportationVehicle` **(POST)** | Create vehicle (type, capacity, active) | `addTransportationVehicle` | `addTransportationVehicleResponse` |
| `updateTransportationVehicle` **(POST)** | Edit vehicle | `updateTransportationVehicle` | `updateTransportationVehicleResponse` |
| `deleteTransportationVehicle` **(POST)** | Delete (Id header) | `deleteTransportationVehicle` | `deleteTransportationVehicleResponse` |
| `approveTransportationVehicle` **(POST)** | Approve/unapprove (Id + Approve headers) | `approveTransportationVehicle` | `approveTransportationVehicleResponse` |
| `addVehicleType` **(POST)** | Create vehicle type | `addTypeVehicle` | (local response) |

### Lines
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getTransportationLines` **(GET)** | Paginated lines — resets/appends `filteredTransportationLinesList` | `getAllTransportationLine` | `transportationLines`, `filteredTransportationLinesList` |
| `addTransportationLine` **(POST)** | Create line (name) | `addTransportationLine` | `addTransportationLineResponse` |
| `updateTransportationLine` **(POST)** | Edit line | `updateTransportationLine` | `updateTransportationLineResponse` |
| `deleteTransportationLine` **(POST)** | Delete (Id header) | `deleteTransportationLine` | `deleteTransportationLineResponse` |
| `approveTransportationLine` **(POST)** | Approve/unapprove (Id + Approve headers) | `approveTransportationLine` | `approveTransportationLineResponse` |

### Repricing
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `rePricingTransportationRoute` **(POST)** | Batch-reprice selected routes (amount, isPercent, forAllLines, approximateToFive, startDate, route id list) | `rePriceTransportationLine` (`ModifyPriceOfTransportationLine`) | returns bool only |
| `getTransportationLineRePricing` **(GET)** | Paginated list of repricing requests (optional Approve filter) | `getAllModifiedTransportationLines` | `rePricedTransportationLines` |
| `updatePriceOfTransportationLine` **(POST)** | Approve/apply a repricing (Id header) | `updatePriceOfTransportationLine` | returns bool only |
| `rejectRepriceTransportationLine` **(POST)** | Reject a repricing (Id header) | `rejectRepriceTransportationLine` (`RejectUpdatePrice`) | returns bool only |

### Routes
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getAllTransportationRoute` **(GET)** | Paginated routes (filters) — resets/appends `filteredTransportationRoutesList` | `getAllTransportationRoute` | `transportationRoutes`, `filteredTransportationRoutesList` |
| `getTransportationRoute` **(GET)** | Single route detail (RouteId header) | `getTransportationRoute` | `transportationRoute` |
| `addTransportationRoute` **(POST)** | Create route (supplier, line, vehicle, shift, cost, period, oneWay…) | `addTransportationRoute` | (local response) |
| `updateTransportationRoute` **(POST)** | Edit route | `updateTransportationRoute` | (local response) |
| `deleteTransportationRoute` **(POST)** | Delete (Id header) | `deleteTransportationRoute` | `deleteTransportationRouteResponse` |
| `getTransportationRoutesByHrUserId` **(GET)** | Routes for an HR user (HrUserId header) | `getRoutesByHrUserId` | `transportationRoutesByHrUserId` |

### Stations (Directions)
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getTransportationRouteStations` **(GET)** | Stations for a route (routeId header) | `getTransportationDirection` | `transportationRouteStations` |
| `addTransportationStation` **(POST)** | Add station under a route (`Data` array + route header) | `addTransportationDirection` | `addTransportationStationResponse` |
| `updateTransportationStation` **(POST)** | Edit station | `updateTransportationDirection` | `updateTransportationStationResponse` |
| `deleteTransportationDirection` **(POST)** | Delete station (Id header) | `deleteTransportationDirection` | `deleteTransportationDirectionResponse` |

### Passengers (route employees)
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getRouteUsers` **(GET)** | Employees on a route (RouteId header) | `getTransportationEmployeeList` | `routeUsers` |
| `getRouteUsersForMob` **(GET)** | Employees on a route for mobile (RouteId + Period) | `getTransportationEmployeeListForMob` | `routeUsersForMob` |
| `addTransportationEmployee` **(POST)** | Assign employee to route (`Data` array + route header) | `addTransportationEmployee` | `addTransportationEmployeeResponse` |
| `updateTransportationEmployee` **(POST)** | Edit assignment | `updateTransportationEmployee` | `updateTransportationEmployeeResponse` |
| `deleteTransportationEmployee` **(POST)** | Remove assignment (Id header) | `deleteTransportationEmployee` | `deleteTransportationEmployeeResponse` |
| `getTransportationRoutesListForPassenger` **(GET)** | All route assignments for one HR user (HrUserId header) | `getTransportationRoutesListForPassenger` | `transportationRoutesListForPassenger` |
| `addTransportationRouteForPassenger` **(POST)** | Bulk-add new route assignments for an HR user (hrUserId header, `Data` array; only items with empty/`0` id) | `addTransportationRoutesListForPassenger` | `addTransportationRouteForPassengerResponse` |
| `updateTransportationRouteForPassenger` **(POST)** | Edit one route assignment | `updateTransportationRoutesListForPassenger` | `updateTransportationRouteForPassengerResponse` |
| `deleteTransportationRouteForPassenger` **(POST)** | Delete one assignment (Id header) | `deleteTransportationRoutesListForPassenger` | `deleteTransportationRouteForPassengerResponse` |

### Shifts
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getShifts` **(GET)** | Load shifts (optional BranchId header); sorts each group's days by weekDayId | `getShifts` | `shifts` |
| `addShifts` **(MULTIPART)** | Create shifts — flattens active days into `shiftDtos[i].*` multipart fields | `addListOfShifts` | `addShiftResponse` |
| `updateShifts` **(MULTIPART)** | Update shifts — multipart with leading `Id` field | `updateShifts` | `updateShiftResponse` |

### Deductions
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getDeductions` **(GET)** | Paginated deductions (filters: supplier/route/from/to) | `getAllTransportationDeduction` | `transportationDeductionsModel` |
| `addDeduction` **(POST)** | Create deduction (supplier, serial, route, per-round, cause, date, Taxes/Normal) | `addTransportationDeduction` | `addTransportationDeductionResponse` |
| `updateDeduction` **(POST)** | Edit deduction | `updateTransportationDeduction` | `updateTransportationDeductionResponse` |

### Exceptions
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getTransportationExceptions` **(GET)** | Paginated exceptions (optional HrUserId/Id) | `getTransportationExceptions` (`GetEmployeeException`) | `transportationExceptions` |
| `addException` **(POST)** | Add employee exception (period vs. date, location vs. station variants) | `addException` | `transportationAddExceptionModel` |
| `updateTransportationException` **(POST)** | Edit exception | `updateTransportationException` | `transportationUpdateExceptionModel` |
| `getTransportationCapacityNumber` **(GET)** | Capacity figures for a route+period (RouteID + Period) | `getTransportationCapacityNumber` (`GetCapacityNumbers`) | `transportationCapacityNumberModel` |

### Attendance / Dashboard
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getDashboardNumbers` **(GET)** | KPI numbers for a date (+ filters) | `getTransportationDashboardNumbers` (`DashBoard`) | `transportationDashboardNumbersResponse` |
| `getDashboardAttendance` **(GET)** | Paginated attendance rows (FromDate/ToDate, filters, AttendaceFlag) | `getTransportationDashboardAttendance` (`DashBoardForAttendenceDuration`) | `transportationDashboardAttendanceResponse` |
| `getDashboardAttendanceExcel` **(GET)** | Attendance export (base64 in `data`) | `getTransportationDashboardAttendanceExcel` (`AttendanceExcell`) | `transportationAttendanceExcel` |
| `addUsersAttendance` **(POST)** | Record check-in/out (serial/type, lat/long, station ids) | `addTransportationUsersAttendance` (`AddUsersAttedance`) | `transportationAddUsersAttendace` |
| `getTouchAttendance44` **(POST)** | Touch-attendance trigger (empty body) | `getTouchAttendance44` (`AddUsersAttedance44`) | `touchAttendanceModel` |

### Costs
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getReportCosts` **(GET)** | Paginated cost-of-lines report (filters + date range) | `getTransportationReportCostLines` (`ReportCostOfLines`) | `transportationReportCostsModel` |
| `getReportCostsExcel` **(GET)** | Cost report export (base64) | `getTransportationReportCostLinesExcel` (`ReportCostOfLinesExcell`) | `transportationCostsExcel` |

### Suppliers / Payments
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `getAllSupplierPayment` **(GET)** | Paginated supplier payments (supplier/from/to) | `getAllSupplierPayment` | `allSupplierPaymentsModel` |
| `addSuplierPayment` **(POST)** | Add supplier payment (payment, date, debt type, optional start/months) | `addSupplierPayment` | `addSupplierPayment` |
| `getAccountsAllMonthForSupplier` **(GET)** | Monthly account rows for a supplier (month/year/route) | `getAccountsAllMonthForSupplier` (`AccountsAllMonthsForSupplier`) | `accountsAllMonthForSupplierModel` |
| `getTransportationDaysInMonth` **(GET)** | Per-round detail within a month (year/month/supplier/route) | `accountsAllRoundsForSupplier` (`AccountsAllRoundsForSupplier`) | `transportationDaysInMonthModel` |

### Excel import/export
| Method | Does | Endpoint const | Populates |
|---|---|---|---|
| `downloadExcelUserWithRoutesTemplete` **(GET)** | Download users-with-routes template (base64) | `downloadExcelUserWithRoutesTemplate` | `downloadExcelUserWithRoutes` |
| `downloadExcelUserActiveTemplete` **(GET)** | Download active-users template (base64) | `downloadExcelUserActiveTemplate` | `downloadExcelUserActive` |
| `insertUsersWithRoutesExcel` **(MULTIPART)** | Pick `.xlsx/.xls` and upload users-with-routes | `insertUsersWithRoutesExcel` | `insertUsersRoutesExcel` |
| `insertNotActive` **(MULTIPART)** | Pick `.xlsx/.xls` and upload not-active users | `insertUserNotActiveExcel` | `insertNotActiveExcel` |

---

## 4. Non-API (local) methods

These only mutate state and call `notifyListeners()` (no HTTP):

- **Pagination/reset**: `updateCurrentPage` (no notify), `updateItemsPerPage`, `resetTransportationLinesList`, `resetTransportationRoutesList`, `resetTransportationRoutesListForPassenger`
- **Toggle setters**: `changeApprovalVehicle`, `changeActiveVehicle`, `changeActiveLine`, `changeApprovalLine`, `changeOneWay`, `updateIsPercent`, `updateForAllLines`, `updateApproximateToFiveFlag`, `updateViewApprovedRePricing`, `updateViewRejectedRePricing`, `updateActiveDirection`, `updateTypeActive`, `updateIsExceptionsPeriod`, `updateIsExceptionLocation`, `updateIsGetInAndOut`, `toggleIsSupplierPaymentAdvanced`, `toggleIsDeductionPercent`, `toggleIsDeductionTaxes`, `changechooseTransportationLine`, `setRouteType`, `setRouteTypeFromApi`
- **Repricing list**: `addItemtoTransLineRePricing`, `removeItemFromTransLineRePricing`, `clearRepricingControllers`, `updateViewRepriceNumOfitems`, `updateViewRepricePageNo`
- **Form population / clearing**: `updateTransportationLineControllers`, `updateTransportationVehicleControllers`, `clearTransportationRoute`, `clearDashboardFilters`, `clearExceptionControllers`, `setTransportationDataToUpdate`, `clearDeductionControllers`, `setDeductionController`, `clearAddPaymentsControllers`, `updateRouteIdToBeget`, `updateRouteIdForScreen`, `updateSelectedPeriodFormDate`, `updaateBranchID`
- **Shift builders**: `generateShiftList`, `generateShiftListToUpdate`, `updateShiftDay`, `updateShiftDays`
- **Passenger list mutators**: `addTransportationRouteForPassengerItem`, `deleteTransportationRouteForPassengerItem`, `deleteTransportationRouteForPassengerItemFromList`
- **Attendance filters**: `updateDashboardAttendancePageNo`, `updateDashboardAttendaceNoOfItems`, `updateDashboardAttendanceAttended`, `updateDashboardAttendanceAbsent`
- **Geolocation**: `getCurrentLocationWithPermission()` → `Future<Position?>` (uses `geolocator`; no HTTP)

Two small helper classes live in the same file: `TransportationRouteStation` and `TransportationEmployee` (both are payload builders with `toJson()`).

---

## 5. Summary table — method → endpoint → returns

| # | Provider method | HTTP | Endpoint const → path suffix | Returns / model |
|---|---|---|---|---|
| 1 | `getVehicleTypes` | GET | `getTransportationVehicleType` → `getAllVehicleType` | `VehicleTypesModel` |
| 2 | `getTransportationVehicles` | GET | `getTransportationVehicle` → `getAllTransportationVehicle` | `TransportationVehiclesModel` |
| 3 | `addTransportationVehicle` | POST | `addTransportationVehicle` → `AddTransportationVehicle` | `PostResponseModel` |
| 4 | `updateTransportationVehicle` | POST | `updateTransportationVehicle` → `UpdateTransportationVehicle` | `PostResponseModel` |
| 5 | `deleteTransportationVehicle` | POST | `deleteTransportationVehicle` → `DeleteTransportationVehicle` | `PostResponseModel` |
| 6 | `approveTransportationVehicle` | POST | `approveTransportationVehicle` → `ApproveTransportationVehicle` | `PostResponseModel` |
| 7 | `addVehicleType` | POST | `addTypeVehicle` → `AddVehicleType` | `PostResponseModel` |
| 8 | `getTransportationLines` | GET | `getAllTransportationLine` → `getAllTransportationLine` | `TransportationLinesModel` |
| 9 | `addTransportationLine` | POST | `addTransportationLine` → `AddTransportationLine` | `PostResponseModel` |
| 10 | `updateTransportationLine` | POST | `updateTransportationLine` → `UpdateTransportationLine` | `PostResponseModel` |
| 11 | `deleteTransportationLine` | POST | `deleteTransportationLine` → `DeleteTransportationLine` | `PostResponseModel` |
| 12 | `approveTransportationLine` | POST | `approveTransportationLine` → `ApproveTransportationLine` | `PostResponseModel` |
| 13 | `rePricingTransportationRoute` | POST | `rePriceTransportationLine` → `ModifyPriceOfTransportationLine` | `PostResponseModel` |
| 14 | `getTransportationLineRePricing` | GET | `getAllModifiedTransportationLines` → `getAllModifyPriceOfTransportationLine` | `TransportationLineRepriceModel` |
| 15 | `updatePriceOfTransportationLine` | POST | `updatePriceOfTransportationLine` → `UpdatePriceOfTransportationLine` | `PostResponseModel` |
| 16 | `rejectRepriceTransportationLine` | POST | `rejectRepriceTransportationLine` → `RejectUpdatePrice` | `PostResponseModel` |
| 17 | `getShifts` | GET | `getShifts` → `/HR/BranchSchedule/GetShifts` | `ShiftsModel` |
| 18 | `addShifts` | MULTIPART | `addListOfShifts` → `/HR/BranchSchedule/AddListOfShifts` | `PostResponseModel` |
| 19 | `updateShifts` | MULTIPART | `updateShifts` → `/HR/BranchSchedule/UpdateShift` | `PostResponseModel` |
| 20 | `addTransportationRoute` | POST | `addTransportationRoute` → `AddTransportationRoute` | `PostResponseModel` |
| 21 | `updateTransportationRoute` | POST | `updateTransportationRoute` → `UpdateTransportationRoute` | `PostResponseModel` |
| 22 | `deleteTransportationRoute` | POST | `deleteTransportationRoute` → `DeleteTransportationRoute` | `PostResponseModel` |
| 23 | `getAllTransportationRoute` | GET | `getAllTransportationRoute` → `getAllTransportationRoute` | `TransportationAllRouteModel` |
| 24 | `getTransportationRoute` | GET | `getTransportationRoute` → `getTransportationRoute` | `TransportationRouteModel` |
| 25 | `getTransportationRoutesByHrUserId` | GET | `getRoutesByHrUserId` → `getTransportationRouteByHrUser` | `TransportationRoutesModel` |
| 26 | `addTransportationStation` | POST | `addTransportationDirection` → `AddTransportationDirection` | `PostResponseModel` |
| 27 | `updateTransportationStation` | POST | `updateTransportationDirection` → `UpdateTransportationDirection` | `PostResponseModel` |
| 28 | `deleteTransportationDirection` | POST | `deleteTransportationDirection` → `DeleteTransportationDirection` | `PostResponseModel` |
| 29 | `getTransportationRouteStations` | GET | `getTransportationDirection` → `GetTransportationDirection` | `TransportationDirectionsModel` |
| 30 | `addTransportationEmployee` | POST | `addTransportationEmployee` → `AddTransportationEmployee` | `PostResponseModel` |
| 31 | `updateTransportationEmployee` | POST | `updateTransportationEmployee` → `UpdateTransportationEmployee` | `PostResponseModel` |
| 32 | `deleteTransportationEmployee` | POST | `deleteTransportationEmployee` → `DeleteTransportationEmployee` | `PostResponseModel` |
| 33 | `getRouteUsers` | GET | `getTransportationEmployeeList` → `GetTransportationEmployeeList` | `RouteUsersModel` |
| 34 | `getRouteUsersForMob` | GET | `getTransportationEmployeeListForMob` → `GetTransportationEmployees` | `RouteUsersForMobModel` |
| 35 | `getTransportationRoutesListForPassenger` | GET | `getTransportationRoutesListForPassenger` → `getTransportationRoutesForHrUser` | `TransportationRoutesListForPassengerModel` |
| 36 | `addTransportationRouteForPassenger` | POST | `addTransportationRoutesListForPassenger` → `AddRoutesForEmployee` | `PostResponseModel` |
| 37 | `updateTransportationRouteForPassenger` | POST | `updateTransportationRoutesListForPassenger` → `UpdateRouteEmployee` | `PostResponseModel` |
| 38 | `deleteTransportationRouteForPassenger` | POST | `deleteTransportationRoutesListForPassenger` → `DeleteRouteEmployee` | `PostResponseModel` |
| 39 | `getDashboardNumbers` | GET | `getTransportationDashboardNumbers` → `DashBoard` | `TransportationDashboardModel` |
| 40 | `getDashboardAttendance` | GET | `getTransportationDashboardAttendance` → `DashBoardForAttendenceDuration` | `TransportationDashboardAttendanceModel` |
| 41 | `getDashboardAttendanceExcel` | GET | `getTransportationDashboardAttendanceExcel` → `AttendanceExcell` | `TransportationAttendanceExcelModel` |
| 42 | `addUsersAttendance` | POST | `addTransportationUsersAttendance` → `AddUsersAttedance` | `PostResponseModel` |
| 43 | `getTouchAttendance44` | POST | `getTouchAttendance44` → `AddUsersAttedance44` | `PostResponseModel` |
| 44 | `addException` | POST | `addException` → `AddException` | `PostResponseModel` |
| 45 | `updateTransportationException` | POST | `updateTransportationException` → `UpdateException` | `PostResponseModel` |
| 46 | `getTransportationExceptions` | GET | `getTransportationExceptions` → `GetEmployeeException` | `TransportationExceptionModel` |
| 47 | `getTransportationCapacityNumber` | GET | `getTransportationCapacityNumber` → `GetCapacityNumbers` | `TransportationCapacityNumberModel` |
| 48 | `addDeduction` | POST | `addTransportationDeduction` → `AddDeduction` | `PostResponseModel` |
| 49 | `updateDeduction` | POST | `updateTransportationDeduction` → `UpdateDeduction` | `PostResponseModel` |
| 50 | `getDeductions` | GET | `getAllTransportationDeduction` → `getAllDeduction` | `TransportationDeductionsModel` |
| 51 | `getReportCosts` | GET | `getTransportationReportCostLines` → `ReportCostOfLines` | `TransportationReportCostsModel` |
| 52 | `getReportCostsExcel` | GET | `getTransportationReportCostLinesExcel` → `ReportCostOfLinesExcell` | `TransportationExcelModel` |
| 53 | `addSuplierPayment` | POST | `addSupplierPayment` → `AddSupplierPayment` | `PostResponseModel` |
| 54 | `getAllSupplierPayment` | GET | `getAllSupplierPayment` → `getAllSupplierPayment` | `AllSupplierPaymentsModel` |
| 55 | `getAccountsAllMonthForSupplier` | GET | `getAccountsAllMonthForSupplier` → `AccountsAllMonthsForSupplier` | `AccountsAllMonthForSupplierModel` |
| 56 | `getTransportationDaysInMonth` | GET | `accountsAllRoundsForSupplier` → `AccountsAllRoundsForSupplier` | `TransportationDaysInMonthModel` |
| 57 | `downloadExcelUserWithRoutesTemplete` | GET | `downloadExcelUserWithRoutesTemplate` → `downloadExcelUserWithRoutesTemplete` | `TransportationExcelModel` |
| 58 | `downloadExcelUserActiveTemplete` | GET | `downloadExcelUserActiveTemplate` → `downloadExcelUserActiveTemplete` | `TransportationExcelModel` |
| 59 | `insertUsersWithRoutesExcel` | MULTIPART | `insertUsersWithRoutesExcel` → `InsertUsersWithRoutesExcel` | `PostResponseModel` |
| 60 | `insertNotActive` | MULTIPART | `insertUserNotActiveExcel` → `InsertUserNotActiveExcel` | `PostResponseModel` |

**Total: 60 API methods across 60 endpoints.** Paths 1–16, 20–60 are served under `/api/Transportation`; paths 17–19 (shifts) under `/HR/BranchSchedule`. Full request/response details in [API.md](API.md).
