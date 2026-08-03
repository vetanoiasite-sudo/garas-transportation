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
import { base64ToBlob } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

const num = (v: unknown): number => {
  const x = Number(v);
  return Number.isFinite(x) ? x : 0;
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
    checkInVehiclePercent: d.CheckInVehiclePercent ?? "0",
    checkOutVehiclePercent: d.CheckOutVehiclePercent ?? "0",
    oneWayVehiclePercent: d.OneWayVehiclePercent ?? "0",
    employeesAttendanceNum: num(d.HrUsersAttendanceNum),
    employeesPercent: d.HrUsersPercent ?? "0",
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
    idCode: r.EmployeeId ?? "", // passenger/employee id (backend key: EmployeeId)
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
    serialBus: query.serial,
    RouteId: query.routeId,
    EmployeeId: query.employeeId, // passenger/employee id filter (fingerprint no.)
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

/** GET AttendanceExcell — the filtered attendance roster as an .xlsx Blob. */
export async function downloadAttendanceExcel(query: AttendanceQuery = {}): Promise<Blob> {
  const res = await apiGet<string>("AttendanceExcell", {
    FromDate: query.from,
    ToDate: query.to,
    TransportionlineId: query.lineId, // frozen typo header
    SupplierId: query.supplierId,
    serialBus: query.serial,
    RouteId: query.routeId,
    EmployeeId: query.employeeId, // passenger/employee id filter (fingerprint no.)
  });
  return base64ToBlob(res.Data ?? "");
}
