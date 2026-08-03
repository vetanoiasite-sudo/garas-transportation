# Transportation Module — Architecture

> Related docs: [PRD.md](PRD.md) · [SCREENS.md](SCREENS.md) · [API.md](API.md) · [MODELS.md](MODELS.md) · [PROVIDERS.md](PROVIDERS.md) · [RULES.md](RULES.md)

This document describes how the **Transportation** feature of the Garas ERP Flutter app is structured, from the UI down to the REST API. It is derived entirely from the current source (no code was changed to produce it).

- App package: `garas` (see `pubspec.yaml`)
- Dart SDK constraint: `^3.11.3` (single Flutter/Dart codebase targeting **web + mobile**)
- Entry point: `lib/main.dart`

---

## 1. Layered architecture

The app follows a classic layered design. For the transportation module the layers map to concrete files:

| Layer | Responsibility | Key files |
|-------|----------------|-----------|
| **GUI (Presentation)** | Screens, cards, dialogs, drawers; reads provider state via `Provider`/`Consumer` and renders it | `lib/GUI/screens/transportationSystem/**` |
| **Providers (State / Application)** | `ChangeNotifier` holding transportation state, controllers, filters; orchestrates API calls and exposes typed results | `lib/Providers/transportation_system_provider.dart` |
| **DataAccessLayer (Data / Infrastructure)** | HTTP transport, response envelope handling, session checks, endpoint URLs, models, shared prefs | `lib/DataAccessLayer/**`, `lib/DataAccessLayer/model/transportation/**` |
| **REST API (Backend)** | ASP.NET Core "CoreApi" service (`/api/Transportation/*`) | Configured in `lib/DataAccessLayer/api_url.dart` + `end_points_constant.dart` |

```
┌───────────────────────────────────────────────────────────────────┐
│ GUI  lib/GUI/screens/transportationSystem/**                        │
│  transportation_routes_web/ · transportation_routes_mob/            │
│  transportation_lines/ · transportation_vehicles/                   │
│  transportation_exception/ · transportation_deduction/              │
│  transportation_dashboard/ · supplierTransportation/ ...            │
│                                                                     │
│   context.watch / Consumer / Provider.of(...)   ▲ notifyListeners() │
│                     │ user actions              │                   │
│                     ▼                           │                   │
├───────────────────────────────────────────────────────────────────┤
│ PROVIDER  TransportationProvider extends ChangeNotifier             │
│  - TextEditingControllers, filter flags, cached model instances     │
│  - API methods: getTransportationVehicles(), getTransportationLines(),│
│    addTransportationRoute(), getTransportationDashboardNumbers() ... │
│                     │ HttpHelper().getData / postData / putData      │
│                     ▼                                                │
├───────────────────────────────────────────────────────────────────┤
│ DATA ACCESS LAYER                                                   │
│  HttpHelper (http_helper.dart) ── injects headers, retries          │
│     ├─ CheckNetworkConnection (network_helper.dart)                 │
│     ├─ CheckResponse          (response_helper.dart)                │
│     ├─ SharedPrefHelper       (shared_preference_helper.dart)       │
│     ├─ EndPointsConst / ApiUrl (end_points_constant.dart/api_url)   │
│     └─ *Model.fromJson         (model/transportation/**)            │
│                     │ package:http (get/post/put/multipart/delete)  │
│                     ▼                                                │
├───────────────────────────────────────────────────────────────────┤
│ REST API   https://<CoreApi host>/api/Transportation/*              │
│  envelope: { Result, Errors:[{ErrorCode,ErrorMsg}], Data, ... }     │
└───────────────────────────────────────────────────────────────────┘
```

The dependency direction is strictly downward: GUI depends on the Provider, the Provider depends on the DataAccessLayer, and the DataAccessLayer depends on the REST API contract. Models flow back up.

---

## 2. Module folder tree

```
lib/
├── Providers/
│   └── transportation_system_provider.dart      # TransportationProvider (single ChangeNotifier)
├── DataAccessLayer/
│   ├── api_url.dart                             # host + path parts (apiUrlCorePart2Transportation = "/api/Transportation")
│   ├── end_points_constant.dart                 # Uri per endpoint (getAllTransportationRoute, AddException, ...)
│   ├── http_helper.dart                         # HttpHelper: getData/postData/putData/multipart/delete
│   ├── network_helper.dart                      # CheckNetworkConnection + network warning dialog
│   ├── response_helper.dart                     # CheckResponse + ResponseError (session expiry)
│   ├── shared_preference_helper.dart            # SharedPrefHelper (get/set/clear)
│   ├── shared_preference_constant.dart          # SharedPrefConst keys
│   ├── constants.dart                           # numOfTriesApi, apiRecallDuration, role ids, messages
│   ├── get_public_data.dart                     # getPublicData(): ensures user data before screen loads
│   └── model/transportation/                    # 24 response/DTO models
│       ├── transportation_routes_model.dart / transportation_route_model.dart
│       ├── transportation_lines_model.dart / transportation_line_reprice_model.dart
│       ├── transportation_vehicles_model.dart / transportation_vehicle_type_model.dart
│       ├── transportation_exception_model.dart / transportation_deductions_model.dart
│       ├── transportation_dashboard_model.dart / transportation_dashboard_attendance_model.dart
│       ├── route_users_model.dart / route_users_for_mob_model.dart
│       ├── shifts/shifts_model.dart
│       ├── all_supplier_payments_model.dart / accounts_all_month_for_supplier_model.dart
│       └── ... (excel/report/capacity/directions/days-in-month models)
└── GUI/screens/transportationSystem/
    ├── transportation_routes/
    │   ├── transportation_routes_web/           # add_edit_transportation_route, route_card_details,
    │   │                                          add_edit_transportation_station, add_edit_passenger_to_route ...
    │   └── transportation_routes_mob/           # transportation_routes_mob, transportation_route_mob,
    │                                              bus_attendance_dialog, transportation_period_mob_dropdown
    ├── transportation_lines/                    # transportation_lines, add_edit_transportation_line, line_card
    ├── transportation_vehicles/                 # vehicles list, add_edit_vehicle, add_vehicle_type, vehicle_card
    ├── transportation_exception/                # exceptions list/info/popup, capacity number popup, period dropdown
    ├── transportation_deduction/                # deductions, add_edit_deduction
    ├── transportation_repriceing/               # add/view repricing, view_reprice_lines_popup
    ├── transportation_passenger/                # create/view/update passenger, assign_routes_to_passenger
    ├── transportation_dashboard/                # dashboard, attendance, passenger_attendance
    ├── transportationCosts/                     # transportation_costs
    ├── shiftTransportation/                     # working_days, add_edit_working_days_and_hours
    └── supplierTransportation/                  # suppliers, payments, daily/month reports, distribution
```

---

## 3. State management — Provider / ChangeNotifier

- The whole app uses **`provider`** with a `MultiProvider` registered at the root in `lib/main.dart`. `TransportationProvider` is registered there as `ChangeNotifierProvider(create: (_) => TransportationProvider())`.
- `TransportationProvider extends ChangeNotifier` (`lib/Providers/transportation_system_provider.dart`). It holds:
  - **UI controllers** — dozens of `TextEditingController`s (vehicle, line, route, shift, exception, dashboard, supplier, repricing).
  - **Filter state** — booleans/ids such as `approvalVehicle`, `activeLine`, `oneWay`, `isPercent`, `vehicleTypeId`, `stationId`, plus pagination `currentPage` / `itemsPerPage`.
  - **Cached model instances** — e.g. `transportationDeductionsModel`, `transportationReportCostsModel`, and `filtered*List` collections used by the lists.
- **Read path:** screens obtain the provider via `Provider.of<TransportationProvider>(context)` / `Consumer`. Mutators call `notifyListeners()` (e.g. `updateCurrentPage`, `changeActiveVehicle`, `clearExceptionControllers`) to rebuild dependent widgets.
- **Write path:** each API method (`getVehicleTypes`, `getTransportationVehicles`, `getTransportationLines`, `addTransportationVehicle`, `addTransportationRoute`, `getTransportationDashboardNumbers`, `addException`, …) awaits `HttpHelper`, stores the parsed model on the provider, then `notifyListeners()` and returns `model.result`.
- **Screen data bootstrap:** stateful screens call `getPublicData(context, getData)` from `initState`. `getPublicData` (`get_public_data.dart`) first guarantees `UserProvider.fetchUserData` has run (so roles are loaded) and only then invokes the screen's `getData()` via a post-frame microtask.

---

## 4. Networking flow

All transportation calls go through the single `HttpHelper` (`lib/DataAccessLayer/http_helper.dart`).

**Request construction**
- Endpoint `Uri`s are pre-built in `EndPointsConst` from `ApiUrl` parts; transportation endpoints use `ApiUrl.apiUrlCorePart2Transportation` = `/api/Transportation` against the CoreApi host (`apiUrlCoreApiNewPart1`). GET uses `Uri.https(authority, path, parameters)`.
- Every authenticated call injects a base header set, then spreads the caller's headers:
  ```
  {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "CompanyName": <SharedPref companyName>,   // GET always; POST/PUT when addCompanyName == true
    "UserToken":  <SharedPref userToken>,       // GET always; POST/PUT when addToken == true
    ...headers
  }
  ```
- **Filters are passed as HTTP headers**, not query strings. For example `getTransportationVehicles` sends `PageNo`, `NoOfItems`, and optional `TransportionlineId`, `SupplierId`, `supplierContactPersonId`, `serialBus`, `DateSerach` (formatted `yyyy-MM-dd` via `intl`) only when non-empty. This is the standard pagination + filtering pattern across the module.

**Pre-flight checks**
- `CheckNetworkConnection.checkNetworkConnection()` (`network_helper.dart`) verifies connectivity (mobile/wifi/ethernet) and shows a blocking `NetworkWarningDialog` when offline.
- `getData` also requires `userToken` and `companyName` to be present (loaded from `SharedPrefHelper`).
- A global loader is toggled via `DashboardProvider.updateLoaderState(true/false)` unless `deActiveLoader` is set.

**Response envelope + parsing**
- On HTTP 200 the body is `json.decode`d and passed to the model's `fromJson` factory (`parseModel`). The response envelope is `{ Result, Errors:[{ErrorCode, ErrorMsg}], Data, ... }`.
- `CheckResponse.checkResponse(model)` (`response_helper.dart`) inspects the parsed model:
  - `result == true && errors.isEmpty` → success (`ResponseError(status:true)`).
  - Otherwise it concatenates all `errorCode`/`errorMsg` into a `ResponseError(status:false)`.

**Session-expiry handling (Err-P1 / Err-P2 / Err-P200)**
- If any error code is `Err-P1`, `Err-P2`, or `Err-P200`, the session is treated as expired: `CheckResponse` navigates to `signInRoute` via `WebNavigation`, clears `SharedPreferences`, and (if "remember me" was set) restores `companyName` / `email` / `password` / `rememberMe`. The error is surfaced to the user.

**Retry with backoff (×3)**
- `numOfTriesApi = 3` and `apiRecallDuration = 10` seconds (`constants.dart`).
- In `getData`, a failed/error response (non-200, decode failure, or a non-`Err-P2` business error) schedules a retry after a 10-second delay with `tryNum + 1`, up to the limit. `Err-P2` (session) is **not** retried. When retries are exhausted, `getData` returns `parseModel({})` (an empty model with `result:false`) so the UI degrades gracefully.
- `postData` / `putData` / multipart / delete do **not** auto-retry; they show a snackbar on error and return an empty model.
- Multipart uploads (`postMultipartRequestData`) are used for endpoints that send files (e.g. shift add/update, Excel user-with-routes import).

---

## 5. Web vs mobile responsive strategy

The app is a **single codebase serving both web and mobile from the same routes**. There is no per-platform build; instead the module ships parallel widget trees:

- The `transportation_routes` feature has explicit **`transportation_routes_web/`** and **`transportation_routes_mob/`** folders. The web variant (`TransportationRoutes` in `transportation_routes_web/transportation_routes.dart`) is the one registered on the named route `transportationRoutesRoute` in `lib/navigationHelper/routing/router.dart`. The mobile variant (`TransportationRoutesMob`) is pushed directly from the mobile drawer (`lib/GUI/drawer/mob_drawer/mob_drawer.dart`) via `MaterialPageRoute`, gated by the user's transportation roles.
- Web screens use web chrome (`app_bar_web.dart`, web pagination, `no_data_widget_web`), while mob screens use `app_bar_mob.dart` and the mobile drawer.
- **Global width guard:** `LayoutTemplate` (`lib/navigationHelper/layout_template/layout_template.dart`) wraps every page (installed as `MaterialApp.builder`). On web, if the viewport width drops below `740` (`minWebWidth` in `constants.dart`), it redirects to `homeRoute` — i.e. the web experience assumes a desktop-class width.
- Widgets also branch inline on `kIsWeb` and `MediaQuery.of(context).size.width` breakpoints (e.g. `pagination_widget.dart` at 750/900/1200) to adapt layout density.

---

## 6. Localization

- Uses **`easy_localization`**. Initialized in `main()` (`await EasyLocalization.ensureInitialized()`), configured for `en-US` and `ar-EG` with translation JSON under `lib/translation/` (`en-US.json`, `ar-EG.json`) and generated keys in `lib/translation/locale_keys.dart`.
- Transportation screens do **not** call `.tr()` directly; they go through a thin wrapper: `Localize().get(LocaleKeys.<key>)` (`lib/utilities/localize.dart`), where `Localize.get` calls `text.tr()`. `Localize().isArabic(context)` is available for RTL/format decisions.
- `MaterialApp` wires `context.localizationDelegates`, `context.supportedLocales`, and `context.locale` from easy_localization.

---

## 7. Navigation

- **Named routes** are declared as constants in `lib/navigationHelper/routing/route_names.dart` (e.g. `transportationRoutesRoute`, `transportationLinesRoute`, `transportationVehiclesRoute`, `transportationDashboardRoute`, `transportationExceptionsRoute`, `transportationDeductionsRoute`, `transportationRepricingRoute`, `createTransportationPassengerRoute`, `transportationSuppliersRoute`, …).
- `MaterialApp.onGenerateRoute = generateRoute` (`lib/navigationHelper/routing/router.dart`) switches on the route name, persists relevant query params to shared prefs (`updateSharedPreferences`), and returns the target screen.
- **`WebNavigation`** (`lib/navigationHelper/navigation.dart`) is the app's navigation façade. It reads a set of `routeKeys`, base64-encodes their shared-pref values into URL query params, records a `routesHistory` stack, and delegates to `NavigationService` (registered through `get_it` via `setupLocator()` / `locator<NavigationService>()`). It supports deep-linkable web navigation (`webNavigateTo`, new-tab helpers, `webNavigatePop`, drawer close). The DataAccessLayer uses `WebNavigation().webNavigateTo(signInRoute)` to force re-login on session expiry.

---

## 8. Cross-cutting concerns

- **Shared preferences** — `SharedPrefHelper` (`shared_preference_helper.dart`) is the single wrapper over `shared_preferences`. It stores auth (`userToken`, `companyName`, `email`, `password`, `rememberMe`) and transportation navigation context (e.g. `transportationLineId`, `transportationLineName`). Keys live in `shared_preference_constant.dart`.
- **Roles / permissions** — role ids in `constants.dart` gate transportation UI: `transportationLineAdminId (210)`, `transportationAdminId (213)`, `transportationSupervisorId (214)`, `transportationPassengerId (215)`, `transportationSuperAdminId (216)`, `transportationReaderId (221)`. Drawers show entries based on flags such as `up.transportationAdmin` / `up.transportationSupervisor` derived from these.
- **Real-time (SignalR)** — `lib/notificationHubSignalR/conniction_signal_r_hub.dart` (`NotificationHubSignalR`) initializes the hub via `NotificationProvider.hubInitialize(context)` using the `signalr_netcore` package. It is app-wide (notifications) rather than transportation-specific, but role-scoped screens live behind the same user context.
- **PDF / Excel export** — the module produces documents with `pdf` + `printing` (see report/costs models `transportation_report_costs_model.dart`, `transportation_costs_excel_model.dart`, `transportation_attendance_excel_model.dart`). Excel import/export endpoints (`downloadExcelUserWithRoutesTemplete`, `InsertUsersWithRoutesExcel`, `AttendanceExcell`, `ReportCostOfLinesExcell`) are called through `HttpHelper` (multipart for uploads).
- **File & location** — `file_picker` (imports/attachments), `geolocator` + `google_maps_flutter` (station lat/long, exception get-in/get-out locations), `country_code_picker` (contact numbers).
- **Global loader & network dialog** — `DashboardProvider.activeLoader` drives the overlay in `LayoutTemplate`; `network_helper.dart` shows a blocking retry dialog when offline.

See [DEPENDENCIES.md](DEPENDENCIES.md) for the exact package list and versions.
