/* Passengers (route employees) service — real calls to /api/Transportation.
 *
 * NOTE: the backend exposes NO passenger-PROFILE list endpoint, so the
 * passengers LIST page (passengers/page.tsx) still reads the mock lib/data.
 * What IS wired here is a single passenger's route memberships — the
 * "assign routes" flow (getTransportationRoutesForHrUser / AddRoutesForEmployee /
 * UpdateRouteEmployee / DeleteRouteEmployee) plus the route dropdown options
 * (getAllTransportationRoute). Maps the backend's frozen PascalCase envelope
 * keys → the app's types. */
import type { Period, Passenger } from "@/lib/types";
import { apiGet, apiPost, apiForm, apiRaw, type PaginationHeader } from "@/lib/api/client";
import { fileUrl } from "@/lib/download";

export interface PaginatedPassengers { items: Passenger[]; pagination: PaginationHeader }

/* HrUserCardDto (list rows) / GetHrUserDto (profile).
 *
 * IMPORTANT — where the two identifiers actually live:
 *   • The passenger ID (the fingerprint no.) is stored in **Email**. The
 *     attendance lookup is literally `attendance.Serial == HrUser.Email`, and
 *     the HrUser entity has NO IdentityNumber column at all.
 *   • The "other identifier" (C / M) is **MaritalStatusId → MaritalStatus.Name**.
 * The list DTO carries Email but not the marital status; the profile carries both. */
interface HrUserRow {
  Id: number;
  Name?: string;
  FirstName?: string;
  MiddleName?: string;
  LastName?: string;
  Mobile?: string;
  Email?: string;
  IdentityNumber?: string;
  MaritalStatusId?: number | null;
  MaritalStatusName?: string;
  Latitude?: number | null;
  Longitude?: number | null;
  Active?: boolean;
  RoutesCount?: number;
  ImgPath?: string;
}
function toPassenger(u: HrUserRow): Passenger {
  return {
    id: String(u.Id),
    name: u.Name ?? [u.FirstName, u.MiddleName, u.LastName].filter(Boolean).join(" "),
    // The ID lives in Email (see the note above) — IdentityNumber does not exist.
    identityNumber: u.Email ?? "",
    mobile: u.Mobile ?? "",
    identifier: u.MaritalStatusName ?? "",
    maritalStatusId: u.MaritalStatusId != null ? String(u.MaritalStatusId) : undefined,
    active: u.Active ?? true,
    photo: u.ImgPath || undefined, // backend field is ImgPath (not Photo)
    homeLat: u.Latitude ?? undefined,
    homeLng: u.Longitude ?? undefined,
    routesCount: u.RoutesCount ?? 0,
  };
}

/** Split a single "name" field into the backend's First/Middle/Last parts. */
function splitName(name: string): { FirstName: string; MiddleName: string; LastName: string } {
  const parts = name.trim().split(/\s+/).filter(Boolean);
  const FirstName = parts.shift() ?? "";
  const LastName = parts.length ? (parts.pop() as string) : "";
  return { FirstName, MiddleName: parts.join(" "), LastName };
}

/** Passenger profile form payload (single name field is split before sending). */
export interface PassengerInput {
  name: string;
  identityNumber: string;
  mobile: string;
  maritalStatusId?: string;
  active: boolean;
  lat?: number;
  lng?: number;
  photo?: string; // data-URI or url stored in HrUser.imgPath
  /** Round-tripped so an edit doesn't clear it (the API rejects a null Email). */
  email?: string;
}

/** Build the CoreApi HrUserDto from the app's single-field form input.
 *
 *  Both CreateHrUser and EditHrEmployee hard-fail when `ARLastName` or `Email`
 *  is null ("please, Enter a valid ArlastName"/"…Email"), and this screen
 *  collects neither: the Arabic name parts mirror the Latin split, and the
 *  passenger's national id doubles as the login-less e-mail placeholder (the
 *  same column the attendance endpoints read as the fingerprint no.). */
function hrUserDto(input: PassengerInput) {
  const parts = splitName(input.name);
  return {
    ...parts,
    ARFirstName: parts.FirstName,
    ARMiddleName: parts.MiddleName,
    ARLastName: parts.LastName || parts.FirstName,
    // The passenger ID IS the Email column — that is what attendance matches on.
    Email: input.identityNumber || input.email,
    Mobile: input.mobile,
    MaritalStatusId: input.maritalStatusId ? Number(input.maritalStatusId) : undefined,
    Latitude: input.lat,
    Longtitud: input.lng, // frozen typo key — `Longitude` bound to nothing and nulled the column
    Active: input.active,
  };
}

/** POST CreateHrUserWithAllRoutes — create a passenger (optionally with routes).
 *  The endpoint is [FromForm], so the DTO rides as multipart form fields. */
export async function addPassenger(input: PassengerInput): Promise<string> {
  const res = await apiForm<number>("CreateHrUserWithAllRoutes", {
    HrUserDto: hrUserDto(input),
    Data: [],
  });
  return String(res.Id ?? res.Data ?? "");
}

/** POST /HrUser/EditHrEmployee — edit an existing passenger profile ([FromForm]). */
export async function updatePassenger(id: string, input: PassengerInput): Promise<void> {
  await apiForm("/HrUser/EditHrEmployee", { HrUserId: Number(id), ...hrUserDto(input) });
}

/** GET /DDL/MaritalStatus — options for the "Other Identifier" dropdown. */
export async function getMaritalStatusOptions(): Promise<RouteOption[]> {
  const res = await apiRaw<{ Id: number; Name: string }[]>("GET", "/DDL/MaritalStatus");
  return (res.Data ?? []).map((m) => ({ value: String(m.Id), label: m.Name }));
}

/* ---- Excel bulk import/export (passengers WITH routes) ---- */

const XLSX_MIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

function base64ToBlob(base64: string, mime: string): Blob {
  const bytes = atob(base64);
  const arr = new Uint8Array(bytes.length);
  for (let i = 0; i < bytes.length; i++) arr[i] = bytes.charCodeAt(i);
  return new Blob([arr], { type: mime });
}

/** Read a File into a bare base64 string (no data-URI prefix). */
export function fileToBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => {
      const result = String(reader.result);
      resolve(result.includes(",") ? result.slice(result.indexOf(",") + 1) : result);
    };
    reader.onerror = () => reject(new Error("file-read-failed"));
    reader.readAsDataURL(file);
  });
}

/* ---- ADD workflow (Add-Passenger screen) ---- */

/** GET downloadExcelUsersList — export the current passengers as an .xlsx Blob
 *  (also serves as the fillable template for the add-upload below). */
export async function downloadPassengersExport(): Promise<string> {
  const res = await apiGet<string>("downloadExcelUserWithRoutesTemplete");
  return fileUrl(res.Data);
}

/** POST InsertUsersWithRoutesExcel — [FromForm] IFormFile. Returns a URL to an
 *  error-log .txt when some rows failed, or nothing when all of them imported. */
export async function uploadPassengersExcel(file: File): Promise<{ errorLogUrl?: string }> {
  const res = await apiForm<string>("InsertUsersWithRoutesExcel", { file });
  return { errorLogUrl: res.Data ? fileUrl(res.Data) : undefined };
}

/* ---- ACTIVATION workflow (Passengers list) ---- */

/** GET downloadExcelUserActiveTemplete — the activation template (ID + نشط) as a Blob. */
export async function downloadActivationTemplate(): Promise<string> {
  const res = await apiGet<string>("downloadExcelUserActiveTemplete");
  return fileUrl(res.Data);
}

/** POST InsertUserNotActiveExcel — [FromForm] IFormFile.
 *
 *  ⚠ The controller action for this route calls InsertUsersWithRoutesExcel (the
 *  CREATE importer), not InsertUserNotActiveExcel — uploading here would create
 *  passengers instead of activating them. Left wired but the screen must keep
 *  the button hidden until the backend routes it correctly. */
export async function uploadActivationExcel(file: File): Promise<{ errorLogUrl?: string }> {
  const res = await apiForm<string>("InsertUserNotActiveExcel", { file });
  return { errorLogUrl: res.Data ? fileUrl(res.Data) : undefined };
}

/** GET getAllHrUsers — paginated passenger profiles. `active` filters by status
 *  ("true"/"false"); omit to get ALL (active + inactive). */
/** Scope filters handed off from the dashboard (all optional, additive backend headers). */
export interface PassengerScopeFilters {
  supplierId?: string;
  routeId?: string;
  serial?: string;
  driverId?: string;
}

/** GET /HrUser/GetUserCards — the passenger list.
 *
 *  NOT `getAllUsers`: that one starts from TransportationVehicleRouteEmployees
 *  and only returns passengers who already sit on an ACTIVE route, so a newly
 *  created passenger never showed up. GetUserCards lists HrUsers themselves and
 *  still accepts the same transportation scope filters.
 *
 *  The row is an HrUserCardDto: the name comes back split, and the profile-only
 *  fields (identity no., marital status, home point) aren't on the card — the
 *  profile screen fetches those per passenger. */
export async function getPassengers(pageNo = 1, noOfItems = 100, name?: string, active?: boolean, scope: PassengerScopeFilters = {}): Promise<PaginatedPassengers> {
  const res = await apiRaw<HrUserRow[]>("GET", "/HrUser/GetUserCards", { headers: {
    currentPage: pageNo,
    numberOfItemsPerPage: noOfItems,
    isDeleted: "false",
    active: active === undefined ? undefined : String(active),
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
    userName: name ? encodeURIComponent(name) : undefined,
    // The backend resolves the scope with a chain of early returns
    // (route → supplier → driver → serial): only the FIRST non-zero filter is
    // applied. Sending several would advertise a combined filter that never
    // happens, so only the most specific one goes out.
    ...(scope.routeId
      ? { RouteId: scope.routeId }
      : scope.supplierId
        ? { SupplierId: scope.supplierId }
        : scope.driverId
          ? { supplierContactPersonId: scope.driverId }
          : scope.serial
            ? { serialBus: scope.serial }
            : {}),
  } });
  const items = (res.Data ?? []).map(toPassenger);
  return { items, pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 } };
}

/** GET /HrUser/GetHrUser — a single passenger profile (HrUserId header). */
export async function getPassenger(id: string): Promise<Passenger | undefined> {
  const res = await apiRaw<HrUserRow | null>("GET", "/HrUser/GetHrUser", { headers: { HrUserId: id, systemUserId: 0 } });
  return res.Data ? toPassenger(res.Data) : undefined;
}

/** Backend TransportationRoutesListForPassengerItem row (frozen keys; some typos). */
interface PassengerRouteRow {
  Id: number;
  RouteId: number;
  TransportationVehicleRouteDirectionId: number;
  Serial: string;
  NameOfRoute: string;
  Active: boolean;
  FromDate: string;
  ToDate: string;
  Period: string;
  DurationLatitude: number;
  DurationLongtitud: number; // ⚠ frozen typo
}

/** One route membership for a passenger (mapped to app shape). */
export interface PassengerRoute {
  id: string; // membership Id (DeleteRouteEmployee / UpdateRouteEmployee target)
  routeId: string; // TransportationVehicleRoute Id
  directionId: number;
  routeName: string;
  serial: string;
  period: Period;
  fromDate?: string;
  toDate?: string;
  lat?: number;
  lng?: number;
  active: boolean;
}

/** A new/edited membership coming from the assign-routes dialog. */
export interface PassengerRouteInput {
  routeId: string;
  directionId?: number;
  period: Period;
  fromDate?: string;
  toDate?: string;
  lat?: number;
  lng?: number;
}

/** Route dropdown option (getAllTransportationRoute). */
export interface RouteOption {
  value: string;
  label: string;
}
interface RouteListRow {
  Id: number;
  NameOfRoute: string;
}

const toFrontPeriod = (p?: string): Period => {
  switch ((p ?? "").toLowerCase()) {
    case "go":
      return "go";
    case "return":
      return "return";
    default:
      return "both";
  }
};
const toBackPeriod = (p: Period): string => (p === "go" ? "Go" : p === "return" ? "Return" : "Both");
const day = (d?: string): string | undefined => (d ? String(d).slice(0, 10) : undefined);

function toPassengerRoute(r: PassengerRouteRow): PassengerRoute {
  return {
    id: String(r.Id),
    routeId: String(r.RouteId),
    directionId: r.TransportationVehicleRouteDirectionId ?? 0,
    routeName: r.NameOfRoute ?? "",
    serial: r.Serial ?? "",
    period: toFrontPeriod(r.Period),
    fromDate: day(r.FromDate),
    toDate: day(r.ToDate),
    lat: r.DurationLatitude || undefined,
    lng: r.DurationLongtitud || undefined,
    active: !!r.Active,
  };
}

/** GET getTransportationRoutesForHrUser — a passenger's route memberships (HrUserId header). */
export async function getPassengerRoutes(passengerId: string): Promise<PassengerRoute[]> {
  const res = await apiGet<PassengerRouteRow[]>("getTransportationRoutesForHrUser", { HrUserId: passengerId });
  return (res.Data ?? []).map(toPassengerRoute);
}

/** POST AddRoutesForEmployee — bulk-add route memberships for one passenger (HrUserId header). */
export async function addRoutesForPassenger(passengerId: string, rows: PassengerRouteInput[]): Promise<void> {
  const Data = rows.map((r) => ({
    RouteId: Number(r.routeId),
    TransportationVehicleRouteDirectionId: r.directionId ? Number(r.directionId) : undefined,
    Period: toBackPeriod(r.period),
    Active: true,
    FromDate: r.fromDate || undefined,
    ToDate: r.toDate || undefined,
    DurationLatitude: r.lat,
    DurationLongtitud: r.lng, // ⚠ frozen typo key
  }));
  await apiPost("AddRoutesForEmployee", { Data }, { HrUserId: passengerId });
}

/** POST UpdateRouteEmployee — update one existing membership from the passenger side. */
export async function updateRouteForPassenger(
  passengerId: string,
  membershipId: string,
  row: PassengerRouteInput,
): Promise<void> {
  await apiPost("UpdateRouteEmployee", {
    Id: Number(membershipId),
    HrUserId: Number(passengerId),
    TransportationVehicleRouteId: Number(row.routeId),
    TransportationVehicleRouteDirectionId: row.directionId ? Number(row.directionId) : undefined,
    Active: true,
    Period: toBackPeriod(row.period),
    FromDate: row.fromDate || undefined,
    ToDate: row.toDate || undefined,
    DurationLatitude: row.lat,
    DurationLongtitud: row.lng, // ⚠ frozen typo key
  });
}

/** POST DeleteRouteEmployee — delete one membership (Id header). */
export async function deleteRouteForPassenger(id: string): Promise<void> {
  await apiPost("DeleteRouteEmployee", {}, { Id: id });
}

/** GET getAllTransportationRoute — route options for the assign-routes dropdown. */
export async function getRouteOptions(): Promise<RouteOption[]> {
  const res = await apiGet<RouteListRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 200 });
  return (res.Data ?? []).map((r) => ({ value: String(r.Id), label: r.NameOfRoute ?? String(r.Id) }));
}
