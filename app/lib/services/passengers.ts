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
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

export interface PaginatedPassengers { items: Passenger[]; pagination: PaginationHeader }

interface HrUserRow {
  Id: number;
  Name?: string;
  Mobile?: string;
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
    name: u.Name ?? "",
    identityNumber: u.IdentityNumber ?? "",
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
}

/** POST AddHrUser — create a passenger profile. Returns the new id (from the Id field). */
export async function addPassenger(input: PassengerInput): Promise<string> {
  const res = await apiPost<number>("AddHrUser", {
    ...splitName(input.name),
    Mobile: input.mobile,
    IdentityNumber: input.identityNumber,
    MaritalStatusId: input.maritalStatusId ? Number(input.maritalStatusId) : undefined,
    Latitude: input.lat,
    Longitude: input.lng,
    ImgPath: input.photo,
    Active: input.active,
  });
  return String(res.Id ?? res.Data ?? "");
}

/** POST UpdateHrUser — edit an existing passenger profile. */
export async function updatePassenger(id: string, input: PassengerInput): Promise<void> {
  await apiPost("UpdateHrUser", {
    Id: Number(id),
    ...splitName(input.name),
    Mobile: input.mobile,
    IdentityNumber: input.identityNumber,
    MaritalStatusId: input.maritalStatusId ? Number(input.maritalStatusId) : undefined,
    Latitude: input.lat,
    Longitude: input.lng,
    ImgPath: input.photo,
    Active: input.active,
  });
}

/** GET GetMaritalStatus — options for the "Other Identifier" dropdown. */
export async function getMaritalStatusOptions(): Promise<RouteOption[]> {
  const res = await apiGet<{ Id: number; Name: string }[]>("GetMaritalStatus");
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
export async function downloadPassengersExport(): Promise<Blob> {
  const res = await apiGet<string>("downloadExcelUsersList");
  return base64ToBlob(res.Data ?? "", XLSX_MIME);
}

/** POST InsertUsersWithRoutesExcel — upload a file to ADD passengers; returns counts. */
export async function uploadPassengersExcel(base64: string): Promise<{ created: number; assigned: number; errors: string[] }> {
  const res = await apiPost<{ Created?: number; Assigned?: number; Errors?: string[] }>("InsertUsersWithRoutesExcel", { File: base64 });
  return { created: res.Data?.Created ?? 0, assigned: res.Data?.Assigned ?? 0, errors: res.Data?.Errors ?? [] };
}

/* ---- ACTIVATION workflow (Passengers list) ---- */

/** GET downloadExcelUserActiveTemplete — the activation template (ID + نشط) as a Blob. */
export async function downloadActivationTemplate(): Promise<Blob> {
  const res = await apiGet<string>("downloadExcelUserActiveTemplete");
  return base64ToBlob(res.Data ?? "", XLSX_MIME);
}

/** POST InsertUserNotActiveExcel — upload to activate/deactivate EXISTING passengers
 *  (matched by ID); returns how many were updated and any unmatched ids. */
export async function uploadActivationExcel(base64: string): Promise<{ updated: number; notFound: string[]; errors: string[] }> {
  const res = await apiPost<{ Updated?: number; NotFound?: string[]; Errors?: string[] }>("InsertUserNotActiveExcel", { File: base64 });
  return { updated: res.Data?.Updated ?? 0, notFound: res.Data?.NotFound ?? [], errors: res.Data?.Errors ?? [] };
}

/** GET getAllHrUsers — paginated passenger profiles. `active` filters by status
 *  ("true"/"false"); omit to get ALL (active + inactive). */
export async function getPassengers(pageNo = 1, noOfItems = 100, name?: string, active?: boolean): Promise<PaginatedPassengers> {
  const res = await apiGet<HrUserRow[]>("getAllHrUsers", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
    Name: name ? encodeURIComponent(name) : undefined,
    Active: active === undefined ? undefined : String(active),
  });
  const items = (res.Data ?? []).map(toPassenger);
  return { items, pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 } };
}

/** GET getHrUser — a single passenger profile (Id header). */
export async function getPassenger(id: string): Promise<Passenger | undefined> {
  const res = await apiGet<HrUserRow | null>("getHrUser", { Id: id });
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
