/* Routes + stations + route-employee service — real calls to /api/Transportation.
   Maps the backend's frozen PascalCase envelope keys → the app's RouteItem /
   Station / PassengerAssignment types (see backend routes/stations/passengers
   services for the exact keys, including the intentional typos: FromoDate,
   ActualUserAttedance, christianNum, Longtitud, FirstNane). */
import type { RouteItem, Station, PassengerAssignment, Period } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";
import { base64ToBlob } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

/** GET RoutesWithUsersExcel — every route with its passengers as an .xlsx Blob. */
export async function downloadRoutesWithUsersExcel(): Promise<Blob> {
  const res = await apiGet<string>("RoutesWithUsersExcel");
  return base64ToBlob(res.Data ?? "");
}

const num = (v: unknown): number => {
  const x = Number(v);
  return Number.isFinite(x) ? x : 0;
};
const optNum = (v?: string): number | undefined => {
  if (v === undefined || v === null || v === "") return undefined;
  const x = Number(v);
  return Number.isFinite(x) ? x : undefined;
};

/* ---- Routes ---- */

// Tolerant of both the list shape (numbers) and the detail shape (all strings).
interface RouteRow {
  Id?: number | string;
  NameOfRoute?: string;
  Serial?: string;
  BusSupervisor?: string;
  SupplierName?: string;
  SupplierContactPersonName?: string;
  LineName?: string;
  LineId?: number | string;
  SupplierId?: number | string;
  SupplierContactPersonId?: number | string;
  branchScheduleId?: number | string;
  BuSupervisorId?: number | string; // frozen typo key (supervisor id, detail only)
  TransportationVehicleId?: number | string;
  FullCapacity?: number | string;
  UserInRoute?: number | string;
  ActualCapacity?: number | string;
  ActualUserAttedance?: number | string; // frozen typo key
  DirectionNum?: number | string;
  OneWay?: boolean;
  FromoDate?: string; // frozen typo key (go time)
  ToDate?: string;
  LineCost?: number | string;
  MuslimNum?: number | string;
  christianNum?: number | string; // frozen lowercase key
}

// A foreign-key id → string, but undefined for missing/zero (so the edit form's
// dropdowns stay empty instead of selecting a bogus "0").
const idStr = (v?: number | string): string | undefined => {
  if (v === undefined || v === null || v === "" || v === 0 || v === "0") return undefined;
  return String(v);
};

function toRoute(r: RouteRow): RouteItem {
  const fromTime = r.FromoDate ? String(r.FromoDate) : "";
  const toTime = r.ToDate ? String(r.ToDate) : "";
  const oneWay = typeof r.OneWay === "boolean" ? r.OneWay : !(fromTime && toTime);
  return {
    id: r.Id != null ? String(r.Id) : "",
    lineId: r.LineId != null ? String(r.LineId) : "",
    supplierId: idStr(r.SupplierId),
    contactPersonId: idStr(r.SupplierContactPersonId),
    branchScheduleId: idStr(r.branchScheduleId),
    supervisorId: idStr(r.BuSupervisorId),
    vehicleId: idStr(r.TransportationVehicleId),
    lineName: r.LineName ?? "",
    name: r.NameOfRoute ?? "",
    serial: r.Serial ?? "",
    supervisor: r.BusSupervisor ?? "",
    supplier: r.SupplierName ?? "",
    driver: r.SupplierContactPersonName ?? "",
    fullCapacity: num(r.FullCapacity),
    usersInRoute: num(r.UserInRoute),
    actualCapacity: num(r.ActualCapacity),
    attended: num(r.ActualUserAttedance),
    stationCount: num(r.DirectionNum),
    oneWay,
    fromTime: fromTime || undefined,
    toTime: toTime || undefined,
    cost: num(r.LineCost),
    religionM: num(r.MuslimNum),
    religionC: num(r.christianNum),
  };
}

export interface RouteQuery {
  pageNo?: number;
  noOfItems?: number;
  lineId?: string;
  supplierId?: string;
  serial?: string;
  name?: string;
}

/** GET getAllTransportationRoute — paginated routes (all filters ride in headers → server-side). */
export async function getRoutes(query: RouteQuery = {}): Promise<Paginated<RouteItem>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<RouteRow[]>("getAllTransportationRoute", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    pTransportionlineId: query.lineId,
    SupplierId: query.supplierId,
    serialBus: query.serial,
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
    Name: query.name ? encodeURIComponent(query.name) : undefined,
  });
  const items = (res.Data ?? []).map(toRoute);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET getTransportationRoute — single route detail (RouteId header). */
export async function getRouteDetail(routeId: string): Promise<RouteItem | undefined> {
  const res = await apiGet<RouteRow | null>("getTransportationRoute", { RouteId: routeId });
  return res.Data ? toRoute(res.Data) : undefined;
}

/** The Add/Update payload (documented PascalCase keys, §6.4). */
export interface RouteInput {
  name: string;
  lineId?: string;
  vehicleId?: string;
  supplierId?: string;
  contactPersonId?: string;
  branchScheduleId?: string;
  supervisorId?: string;
  cost: number;
  oneWay: boolean;
  fromTime?: string;
  toTime?: string;
  active?: boolean;
}

function routeBody(i: RouteInput): Record<string, unknown> {
  return {
    TransportationLineId: optNum(i.lineId) ?? 0,
    TransportationVehicleId: optNum(i.vehicleId) ?? 0,
    SupplierId: optNum(i.supplierId),
    SupplierContactPersonId: optNum(i.contactPersonId),
    BranchScheduleId: optNum(i.branchScheduleId),
    HrUserId: optNum(i.supervisorId),
    NameOfRoute: i.name,
    LineCost: i.cost,
    OneWay: i.oneWay,
    FromDate: i.fromTime,
    ToDate: i.toTime,
    Active: i.active ?? true,
  };
}

/** POST AddTransportationRoute — create a route; returns the new id. */
export async function addRoute(input: RouteInput): Promise<string> {
  const res = await apiPost<unknown>("AddTransportationRoute", routeBody(input));
  return String((res as { Id?: string | number }).Id ?? "");
}

/** POST UpdateTransportationRoute — edit a route (Id in the body). */
export async function updateRoute(id: string, input: RouteInput): Promise<void> {
  await apiPost("UpdateTransportationRoute", { Id: Number(id), ...routeBody(input) });
}

/** POST DeleteTransportationRoute — delete (Id header). */
export async function deleteRoute(id: string): Promise<void> {
  await apiPost("DeleteTransportationRoute", {}, { Id: id });
}

/** POST ApproveRoute — toggle active (Id + Approve headers). */
export async function approveRoute(id: string, approve = true): Promise<void> {
  await apiPost("ApproveRoute", {}, { Id: id, Approve: String(approve) });
}

/* ---- Stations (Directions) ---- */

interface StationRow {
  Id: string;
  RouteDirection?: string;
  Description?: string;
  Latitude?: string;
  Longtitud?: string; // frozen typo key
  Active?: boolean;
}

function toStation(routeId: string, d: StationRow): Station {
  const lat = d.Latitude ? Number(d.Latitude) : undefined;
  const lng = d.Longtitud ? Number(d.Longtitud) : undefined;
  return {
    id: String(d.Id),
    routeId,
    name: d.RouteDirection ?? "",
    description: d.Description || undefined,
    active: d.Active ?? true,
    lat: lat != null && Number.isFinite(lat) ? lat : undefined,
    lng: lng != null && Number.isFinite(lng) ? lng : undefined,
  };
}

export interface StationInput {
  name: string;
  description?: string;
  active?: boolean;
  lat?: number;
  lng?: number;
}

/** GET GetTransportationDirection — stations for a route (routeId header). */
export async function getStations(routeId: string): Promise<Station[]> {
  const res = await apiGet<StationRow[]>("GetTransportationDirection", { routeId });
  return (res.Data ?? []).map((d) => toStation(routeId, d));
}

/** POST AddTransportationDirection — add a station under a route (Data[] + route header). */
export async function addStation(routeId: string, input: StationInput): Promise<void> {
  await apiPost(
    "AddTransportationDirection",
    {
      Data: [
        {
          RouteDirection: input.name,
          Description: input.description ?? "",
          Latitude: input.lat != null ? String(input.lat) : "",
          Longtitud: input.lng != null ? String(input.lng) : "",
          Active: input.active ?? true,
        },
      ],
    },
    { TransportationVehicleRouteId: routeId },
  );
}

/** POST UpdateTransportationDirection — edit a station. */
export async function updateStation(id: string, routeId: string, input: StationInput): Promise<void> {
  await apiPost("UpdateTransportationDirection", {
    Id: Number(id),
    TransportationVehicleRouteId: Number(routeId),
    RouteDirection: input.name,
    Description: input.description ?? "",
    Latitude: input.lat != null ? String(input.lat) : "",
    Longtitud: input.lng != null ? String(input.lng) : "",
    Active: input.active ?? true,
  });
}

/** POST DeleteTransportationDirection — delete a station (Id header). */
export async function deleteStation(id: string): Promise<void> {
  await apiPost("DeleteTransportationDirection", {}, { Id: id });
}

/* ---- Route employees (passengers on a route) ---- */

interface RouteUserRow {
  ID: number;
  HrUserId: number;
  FirstNane?: string; // frozen typo key
  MiddleName?: string;
  LastName?: string;
  DirectionId?: number;
  DirectionName?: string;
  Period?: string; // Go | Return | Both (case-insensitive)
}

// Stored period code → app Period (tolerant of casing: "go"/"Go", etc.).
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

function toAssignment(m: RouteUserRow): PassengerAssignment {
  const name = [m.FirstNane, m.MiddleName, m.LastName].filter(Boolean).join(" ");
  return {
    id: String(m.ID),
    passengerId: String(m.HrUserId),
    passengerName: name,
    stationName: m.DirectionName || undefined,
    period: toFrontPeriod(m.Period),
  };
}

export interface RouteUserInput {
  hrUserId: string;
  directionId?: string;
  period?: Period;
}

// App Period → stored code (capitalized, matching the seed and the mobile-app query).
const periodKey = (p?: Period): string => (p === "go" ? "Go" : p === "return" ? "Return" : "Both");

/** GET GetTransportationEmployeeList — passengers assigned to a route (RouteId header). */
export async function getRouteUsers(routeId: string): Promise<PassengerAssignment[]> {
  const res = await apiGet<RouteUserRow[]>("GetTransportationEmployeeList", { RouteId: routeId });
  return (res.Data ?? []).map(toAssignment);
}

/** POST AddTransportationEmployee — assign a passenger to a route (Data[] + route header). */
export async function addRouteUser(routeId: string, input: RouteUserInput): Promise<void> {
  await apiPost(
    "AddTransportationEmployee",
    {
      Data: [
        {
          hrUserId: Number(input.hrUserId),
          TransportationVehicleRouteDirectionId: input.directionId ? Number(input.directionId) : undefined,
          Active: true,
          period: periodKey(input.period),
        },
      ],
    },
    { TransportationVehicleRouteId: routeId },
  );
}

/** POST DeleteTransportationEmployee — remove a passenger from a route (Id header). */
export async function deleteRouteUser(id: string): Promise<void> {
  await apiPost("DeleteTransportationEmployee", {}, { Id: id });
}
