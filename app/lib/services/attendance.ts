/* Dashboard numbers + attendance service — real calls to /api/Transportation.
   Maps the backend's frozen PascalCase keys → the app's DashboardNumbers /
   AttendanceRecord shapes (see backend dashboard.service.ts for the exact keys,
   including the frozen typos: TransportionlineName, supplierContactPersonName,
   ISAttendace, AttendaceFlag).

   NOTE: backend analytics partly stubbed — the DashBoard attendance/percentage
   figures, DashBoardForAttendenceDuration AttendanceHistory, and the Excel export
   all come back as 0/""/[] until the attendance aggregation is wired. Wire it
   anyway; the UI populates once those figures are filled in. */
import type { AttendanceRecord } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";
import { fileUrl } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

const num = (v: unknown): number => {
  const x = Number(v);
  return Number.isFinite(x) ? x : 0;
};

// The API returns raw doubles for the percentages (69.39769707705935) — printing
// them verbatim overflows the KPI tiles and the columns run into each other.
const pct = (v: unknown): string => {
  const x = Number(v);
  if (!Number.isFinite(x)) return "0";
  return String(Math.round(x * 10) / 10);
};

/* ---- Dashboard numbers (DashBoard) ---- */

// TransportationDashboardData — every value comes back as a String.
interface DashboardData {
  TransportLinesNum?: string;
  VehiclesNum?: string;
  TwoWayVehiclesNum?: string;
  OneWayVehiclesNum?: string;
  HrUsersNum?: string;
  SuppliersNum?: string;
  HrUsersPercent?: string;
  CheckInVehiclePercent?: string;
  CheckOutVehiclePercent?: string;
  OneWayVehiclePercent?: string;
  CheckInVehicleAttendanceNum?: string;
  CheckOutVehicleAttendanceNum?: string;
  OneWayVehicleAttendanceNum?: string;
  HrUsersAttendanceNum?: string;
  AllHrUsersNum?: string;
  AllSuppliersNum?: string;
  VehiclesTypeNum?: string;
  // The old-dashboard parity cards. These DO exist on DashBoardVM, just under
  // different names than the ones the cards were first written against:
  //   capacity            → Capacity            (not FullCapacity)
  //   users % of capacity → HrUsersOfCapacityPercent
  //   users return leg    → HrUsersAttendanceNumCheckOut
  //   one-way return leg  → OneWayVehicleAttendanceNumCheckOut / …PercentCheckOut
  Capacity?: string;
  HrUsersOfCapacityPercent?: string;
  HrUsersAttendanceNumCheckOut?: string;
  OneWayVehicleAttendanceNumCheckOut?: string;
  OneWayVehiclePercentCheckOut?: string;
}

export interface DashboardNumbers {
  linesNum: number;
  vehiclesNum: number;
  employeesNum: number;
  allEmployeesNum: number;
  suppliersNum: number;
  allSuppliersNum: number;
  vehicleTypeNum: number;
  twoWayVehiclesNum: number;
  oneWayVehiclesNum: number;
  // Attendance breakdown (stubbed 0 on the backend for now).
  checkInVehicleNum: number;
  checkOutVehicleNum: number;
  oneWayVehicleNum: number;
  checkInVehiclePercent: string;
  checkOutVehiclePercent: string;
  oneWayVehiclePercent: string;
  employeesAttendanceNum: number;
  employeesPercent: string;
  fullCapacity: number;
  employeesGoNum: number;
  employeesReturnNum: number;
  capacityPercent: string;
  oneWayGoPercent: string;
  oneWayReturnPercent: string;
}

export interface DashboardQuery {
  date: string; // DateSerach — required by the backend
  lineId?: string;
  supplierId?: string;
  serial?: string;
  routeId?: string;
}

/** GET DashBoard — the single dashboard numbers object (requires a date). */
export async function getDashboardNumbers(query: DashboardQuery): Promise<DashboardNumbers> {
  const res = await apiGet<DashboardData>("DashBoard", {
    DateSerach: query.date, // frozen typo header
    TransportionlineId: query.lineId, // frozen typo header
    SupplierId: query.supplierId,
    serialBus: query.serial,
    RouteId: query.routeId,
  });
  const d = res.Data ?? {};
  return {
    linesNum: num(d.TransportLinesNum),
    vehiclesNum: num(d.VehiclesNum),
    employeesNum: num(d.HrUsersNum),
    allEmployeesNum: num(d.AllHrUsersNum),
    suppliersNum: num(d.SuppliersNum),
    allSuppliersNum: num(d.AllSuppliersNum),
    vehicleTypeNum: num(d.VehiclesTypeNum),
    twoWayVehiclesNum: num(d.TwoWayVehiclesNum),
    oneWayVehiclesNum: num(d.OneWayVehiclesNum),
    checkInVehicleNum: num(d.CheckInVehicleAttendanceNum),
    checkOutVehicleNum: num(d.CheckOutVehicleAttendanceNum),
    oneWayVehicleNum: num(d.OneWayVehicleAttendanceNum),
    checkInVehiclePercent: pct(d.CheckInVehiclePercent),
    checkOutVehiclePercent: pct(d.CheckOutVehiclePercent),
    oneWayVehiclePercent: pct(d.OneWayVehiclePercent),
    employeesAttendanceNum: num(d.HrUsersAttendanceNum),
    employeesPercent: pct(d.HrUsersPercent),
    fullCapacity: num(d.Capacity),
    // HrUsersAttendanceNum is the outbound (check-in) leg; the return leg has
    // its own CheckOut-suffixed field.
    employeesGoNum: num(d.HrUsersAttendanceNum),
    employeesReturnNum: num(d.HrUsersAttendanceNumCheckOut),
    capacityPercent: pct(d.HrUsersOfCapacityPercent),
    oneWayGoPercent: pct(d.OneWayVehiclePercent),
    oneWayReturnPercent: pct(d.OneWayVehiclePercentCheckOut),
  };
}

/* ---- Attendance history (DashBoardForAttendenceDuration) ---- */

interface AttendanceHistoryRow {
  Date?: string;
  CheckIn?: string;
  CheckOut?: string;
  ISAttendace?: boolean; // frozen typo key
}

// TransportationDashboardAttendanceData row (frozen PascalCase / typo keys).
interface AttendanceRow {
  Id?: string | number;
  EmployeeId?: string; // passenger/employee id (fingerprint no.)
  IdentityNumber?: string; // national id (additive key)
  FirstName?: string;
  MiddleName?: string;
  LastName?: string;
  TransportionlineName?: string; // frozen typo key
  NameOfRoute?: string;
  SupplierName?: string;
  supplierContactPersonName?: string; // frozen lowercase key
  SupervisorName?: string;
  Serial?: string; // may be absent
  MaritalStatus?: string; // the "other identifier" opaque code (C/M)
  AttendanceHistory?: AttendanceHistoryRow[];
}

function toAttendance(r: AttendanceRow, index: number): AttendanceRecord {
  const name = [r.FirstName, r.MiddleName, r.LastName].filter(Boolean).join(" ");
  const history = (r.AttendanceHistory ?? []).map((h) => ({
    date: h.Date ?? "",
    checkIn: h.CheckIn || undefined,
    checkOut: h.CheckOut || undefined,
    attended: !!h.ISAttendace,
  }));
  const latest = history[history.length - 1];
  return {
    // Backend Id is the passenger (hrUser) id, which repeats when the same
    // passenger rides more than one route — so suffix the row position to keep
    // React keys unique without changing the frozen API contract.
    id: `${r.Id != null ? String(r.Id) : "row"}-${index}`,
    name,
    // Fingerprint no. when set, else the national id — so the ID column is
    // never blank for passengers that have no fingerprint code yet.
    idCode: r.EmployeeId || r.IdentityNumber || "",
    otherId: r.MaritalStatus ?? "", // the "other identifier" (C/M) from the backend
    line: r.TransportionlineName ?? "",
    route: r.NameOfRoute ?? "",
    serial: r.Serial ?? "",
    supervisor: r.SupervisorName ?? "",
    supplier: r.SupplierName ?? "",
    driver: r.supplierContactPersonName ?? "",
    checkIn: latest?.checkIn,
    checkOut: latest?.checkOut,
    attended: latest?.attended ?? false,
    history,
  };
}

export interface AttendanceQuery {
  from?: string;
  to?: string;
  lineId?: string;
  supplierId?: string;
  serial?: string;
  routeId?: string;
  driverId?: string;
  employeeId?: string; // passenger/employee id (fingerprint no.)
  attended?: boolean;
  absent?: boolean;
  pageNo?: number;
  noOfItems?: number;
}

/** GET DashBoardForAttendenceDuration — paginated attendance rows for a range. */
export async function getAttendance(query: AttendanceQuery = {}): Promise<Paginated<AttendanceRecord>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  // attended-only → "true", absent-only → "false", both/neither → unset.
  const flag =
    query.attended && !query.absent ? "true" : !query.attended && query.absent ? "false" : undefined;
  const res = await apiGet<AttendanceRow[]>("DashBoardForAttendenceDuration", {
    FromDate: query.from,
    ToDate: query.to,
    TransportionlineId: query.lineId, // frozen typo header
    SupplierId: query.supplierId,
    supplierContactPersonId: query.driverId,
    serialBus: query.serial,
    RouteId: query.routeId,
    HrUser: query.employeeId, // the backend filters on HrUser (an HrUser id), not EmployeeId
    AttendaceFlag: flag, // frozen typo header
    PageNo: pageNo,
    NoOfItems: noOfItems,
  });
  const items = (res.Data ?? []).map(toAttendance);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/* ---- Writes ---- */

/** POST AddUsersAttedance — record a check-in (checkIn:true) or check-out. */
export async function recordAttendance(payload: {
  type?: string;
  serial: string;
  checkIn: boolean;
  lat?: number;
  lng?: number;
}): Promise<void> {
  await apiPost("AddUsersAttedance", {
    type: payload.type ?? "Person",
    serial: payload.serial,
    CheckInOrCheckOut: payload.checkIn,
    CheckInLatitude: payload.checkIn ? payload.lat : undefined,
    CheckInLongtitud: payload.checkIn ? payload.lng : undefined, // frozen typo key
    CheckOutLatitude: payload.checkIn ? undefined : payload.lat,
    CheckOutLongtitud: payload.checkIn ? undefined : payload.lng, // frozen typo key
  });
}

/* ---- Bus attendance (BusAttendance) ---- */

/* The backend groups by route and returns ONE ROW PER DAY inside each group;
   CheckIn/CheckOut are booleans (did the bus run that leg), not timestamps:

     Data: [ { RouteName, Attendance: [ { LineName, RouteName, Serial, Date,
                                          CheckIn, CheckOut, OneWay,
                                          CheckInUsersCount, … } ] } ]

   The screen wants one row per BUS with a per-day history, so the group is
   flattened here. Supplier/driver are not part of this response. */
interface BusAttendanceDayRow {
  LineName?: string;
  RouteName?: string;
  Serial?: string;
  Date?: string;
  CheckIn?: boolean;
  CheckOut?: boolean;
  OneWay?: boolean;
  CheckInUsersCount?: number | null;
  CheckOutUsersCount?: number | null;
  OneWayUsersCount?: number | null;
}
interface BusAttendanceGroup {
  RouteName?: string;
  Attendance?: BusAttendanceDayRow[];
}

export interface BusAttendanceDay {
  date: string;
  /** The bus ran the outbound leg that day. */
  checkIn: boolean;
  /** The bus ran the return leg that day. */
  checkOut: boolean;
  riddenIn: number;
  riddenOut: number;
  attended: boolean;
}

export interface BusAttendanceRecord {
  id: string;
  line: string;
  route: string;
  serial: string;
  attended: boolean;
  /** Days the bus ran at least one leg, within the filtered range. */
  daysAttended: number;
  history: BusAttendanceDay[];
}

function toBusAttendance(g: BusAttendanceGroup, index: number): BusAttendanceRecord {
  const rows = g.Attendance ?? [];
  const history: BusAttendanceDay[] = rows.map((d) => ({
    date: d.Date ?? "",
    checkIn: !!d.CheckIn,
    checkOut: !!d.CheckOut,
    riddenIn: d.CheckInUsersCount ?? 0,
    riddenOut: d.CheckOutUsersCount ?? 0,
    attended: !!(d.CheckIn || d.CheckOut || d.OneWay),
  }));
  const first = rows[0];
  return {
    // The response carries no route id — the group position keeps React keys stable.
    id: `${first?.Serial || g.RouteName || "bus"}-${index}`,
    line: first?.LineName ?? "",
    route: g.RouteName ?? first?.RouteName ?? "",
    serial: first?.Serial ?? "",
    attended: history.some((h) => h.attended),
    daysAttended: history.filter((h) => h.attended).length,
    history,
  };
}

export interface BusAttendanceQuery {
  from?: string;
  to?: string;
  lineId?: string;
  supplierId?: string;
  driverId?: string;
  serial?: string;
  routeId?: string;
  name?: string;
  pageNo?: number;
  noOfItems?: number;
}

const busAttendanceHeaders = (query: BusAttendanceQuery) => ({
  FromDate: query.from,
  ToDate: query.to,
  TransportionlineId: query.lineId, // frozen typo header
  SupplierId: query.supplierId,
  supplierContactPersonId: query.driverId,
  serialBus: query.serial,
  RouteId: query.routeId,
  // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
  Name: query.name ? encodeURIComponent(query.name) : undefined,
});

/** GET BusAttendance — paginated per-route BUS attendance rows. */
export async function getBusAttendance(query: BusAttendanceQuery = {}): Promise<Paginated<BusAttendanceRecord>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<BusAttendanceGroup[]>("BusAttendance", {
    ...busAttendanceHeaders(query),
    PageNo: pageNo,
    NoOfItems: noOfItems,
  });
  const items = (res.Data ?? []).map(toBusAttendance);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET BusAttendanceExcel — returns a URL to the generated file, not base64. */
export async function downloadBusAttendanceExcel(query: BusAttendanceQuery = {}): Promise<string> {
  const res = await apiGet<string>("BusAttendanceExcel", busAttendanceHeaders(query));
  return fileUrl(res.Data);
}

/** GET AttendanceExcell — the filtered attendance roster as an .xlsx Blob. */
export async function downloadAttendanceExcel(query: AttendanceQuery = {}): Promise<string> {
  const res = await apiGet<string>("AttendanceExcell", {
    FromDate: query.from,
    ToDate: query.to,
    TransportionlineId: query.lineId, // frozen typo header
    SupplierId: query.supplierId,
    supplierContactPersonId: query.driverId,
    serialBus: query.serial,
    RouteId: query.routeId,
    HrUser: query.employeeId, // the backend filters on HrUser (an HrUser id), not EmployeeId
  });
  return fileUrl(res.Data);
}
