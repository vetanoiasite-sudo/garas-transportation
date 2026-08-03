# Transportation Module — Dependencies

> Related docs: [PRD.md](PRD.md) · [SCREENS.md](SCREENS.md) · [API.md](API.md) · [MODELS.md](MODELS.md) · [PROVIDERS.md](PROVIDERS.md) · [RULES.md](RULES.md)

This lists the dependencies the **Transportation** module actually pulls in. The pub packages below were collected by scanning `import 'package:...'` across:

- `lib/GUI/screens/transportationSystem/**`
- `lib/Providers/transportation_system_provider.dart`
- `lib/DataAccessLayer/model/transportation/**`

Versions are quoted from `pubspec.yaml`. The `garas` app itself is not published; these are the external packages the module touches (plus the shared internal layers it depends on).

---

## SDK constraint

- **Dart SDK:** `sdk: ^3.11.3` (from `pubspec.yaml` → `environment`). Single Dart/Flutter codebase targeting web + mobile.
- The Flutter SDK is pulled via `flutter: { sdk: flutter }`; no separate explicit Flutter version pin.

### ⚠️ Dependency overrides
`pubspec.yaml` declares a `dependency_overrides` block that **pins two transitive/direct packages to older majors** than what the main `dependencies` request:

| Package | `dependencies` requests | `dependency_overrides` forces (effective) |
|---------|-------------------------|-------------------------------------------|
| `intl`  | `^0.20.3`               | **`^0.18.1`** |
| `http`  | `^1.6.0`                | **`^0.13.6`** |

Both are used directly by the transportation module (`intl` for date formatting, `http` for transport). The **effective** versions the module compiles and runs against are the overridden ones (`intl 0.18.x`, `http 0.13.x`), so any API usage must stay compatible with those older majors — not the newer versions listed under `dependencies`.

---

## 1. Pub packages imported by the module

| Package | Version (pubspec) | Used for (in this module) | Where used (example files) |
|---------|-------------------|---------------------------|----------------------------|
| `flutter` (Material) | SDK | All widgets, `ChangeNotifier`, `TextEditingController`, theming | every screen; `transportation_system_provider.dart` |
| `provider` | `^6.1.5+1` | Reading/updating `TransportationProvider` state (`Provider.of`, `Consumer`) | `transportation_routes_web/transportation_routes.dart`, `transportation_routes_mob/transportation_routes_mob.dart`, most screens |
| `http` | `^1.6.0` → **override `^0.13.6`** | REST transport (`MultipartFile` for uploads); indirectly via `HttpHelper` | `transportation_system_provider.dart` (`import 'package:http/http.dart' as http`) |
| `http_parser` | `^4.1.2` | `MediaType` for multipart file parts (Excel/image uploads) | `transportation_system_provider.dart` |
| `intl` | `^0.20.3` → **override `^0.18.1`** | Date formatting for filters/reports (`DateFormat('yyyy-MM-dd')`) | `transportation_system_provider.dart` (vehicle date filter, dashboard) |
| `geolocator` | `^14.0.2` | Current GPS position for stations & exception get-in/get-out locations | `transportation_system_provider.dart` |
| `google_maps_flutter` | `^2.17.1` | Map display / station coordinate picking | transportation route/station screens; provider map wiring |
| `file_picker` | `^10.3.10` | Selecting files for Excel import / attachments | `transportation_system_provider.dart` |
| `country_code_picker` | `^3.4.1` | Country dial-code selection for contact numbers (e.g. exceptions) | transportation exception/passenger screens |
| `pdf` | `^3.13.0` | Building PDF documents for transportation reports | report/cost export paths |
| `printing` | `^5.15.0` | Rendering / previewing / sharing generated PDFs | report/cost export paths |
| `hovering` | `^1.0.4` | Hover effects on interactive cards/rows (web) | `transportation_routes_mob/transportation_routes_mob.dart` and card widgets |

Notes:
- `http`, `http_parser`, `intl`, `geolocator` are imported **directly in the provider**; most GUI screens rely on the DataAccessLayer instead of importing `http` themselves.
- `pdf`/`printing` appear via the module's export flows and shared export widgets; the Excel report/attendance models (`transportation_costs_excel_model.dart`, `transportation_attendance_excel_model.dart`) feed those.

---

## 2. Internal dependencies (other app layers used by the module)

These are in-repo (`package:garas/...`) modules the transportation screens/provider/models depend on.

| Internal dependency | Path | Role for the module |
|---------------------|------|---------------------|
| `HttpHelper` | `lib/DataAccessLayer/http_helper.dart` | All REST calls (get/post/put/multipart/delete) with header injection + retry |
| `EndPointsConst` / `ApiUrl` | `lib/DataAccessLayer/end_points_constant.dart`, `api_url.dart` | Transportation endpoint `Uri`s under `/api/Transportation` |
| `CheckResponse` / `ResponseError` | `lib/DataAccessLayer/response_helper.dart` | Envelope validation + `Err-P1/P2/P200` session handling |
| `CheckNetworkConnection` | `lib/DataAccessLayer/network_helper.dart` | Connectivity gate + offline dialog |
| `SharedPrefHelper` / `SharedPrefConst` | `lib/DataAccessLayer/shared_preference_helper.dart`, `shared_preference_constant.dart` | Auth token/company + transportation nav context (line id/name) |
| `constants.dart` | `lib/DataAccessLayer/constants.dart` | `numOfTriesApi`, `apiRecallDuration`, `minWebWidth`, role ids (210–221), messages |
| `getPublicData` | `lib/DataAccessLayer/get_public_data.dart` | Ensures `UserProvider` data (roles) is loaded before a screen's `getData()` |
| Transportation models | `lib/DataAccessLayer/model/transportation/**` (24 files) + shared `model/pagination.dart`, `model/post_response_model.dart` | DTOs / `fromJson` parsing for every endpoint |
| `UserProvider` | `lib/Providers/user_provider.dart` | Current user, roles/flags (`transportationAdmin`, `transportationSupervisor`) gating UI |
| `DashboardProvider` | `lib/Providers/dashboard_provider.dart` | Global loading overlay (`updateLoaderState`) toggled by `HttpHelper` |
| `TransportationProvider` | `lib/Providers/transportation_system_provider.dart` | The module's own state container (registered in `main.dart`) |
| `WebNavigation` / `NavigationService` / route names | `lib/navigationHelper/navigation.dart`, `services/navigation_service.dart`, `routing/route_names.dart`, `routing/router.dart` | Named-route navigation, web deep-link params, forced re-login on expiry |
| Localization | `lib/utilities/localize.dart` (`Localize().get(...)`), `lib/translation/locale_keys.dart`, `lib/translation/*.json` | Wrapper over `easy_localization` used by all screens |
| Shared UI widgets | `lib/GUI/widgets/**` (e.g. `pagination_widget.dart`, `no_data_widget_web.dart`, `app_title_widget.dart`, `snack_bar.dart`, button widgets), `lib/GUI/app_bar/app_bar_web.dart` & `app_bar_mob.dart`, `lib/GUI/drawer/drawer.dart` & `drawer/mob_drawer/mob_drawer.dart` | Chrome, dropdowns, pagination, empty states, snackbars, drawers (mob drawer pushes `TransportationRoutesMob`) |
| Utilities / theming | `lib/utilities/my_colors.dart`, `my_fonts.dart`, `my_images.dart`, `style.dart` | Colors, fonts, images, shared styles |

> Note: `easy_localization`, `signalr_netcore`, `get_it`, `shared_preferences`, `connectivity_plus`, and `url_launcher` are not imported directly inside `transportationSystem/**` but are reached transitively through the shared layers above (localization wrapper, SignalR hub, service locator, prefs helper, network helper, web navigation).

---

See [ARCHITECTURE.md](ARCHITECTURE.md) for how these pieces fit together across the layers.
