/* Domain types + role/permission model (from the business documentation §2). */

export type Role =
  | "system_admin"
  | "super_admin"
  | "line_admin"
  | "trans_admin"
  | "supervisor"
  | "reader";

export const roleLabelKey: Record<Role, string> = {
  system_admin: "role.systemAdmin",
  super_admin: "role.superAdmin",
  line_admin: "role.lineAdmin",
  trans_admin: "role.transAdmin",
  supervisor: "role.supervisor",
  reader: "role.reader",
};

export type Permission =
  | "crud.entities" // add/edit/delete line, route, station, passenger assignment
  | "add.entities" // add only (trans admin)
  | "approve.line"
  | "manage.vehicles"
  | "approve.vehicle"
  | "manage.shifts"
  | "add.payment"
  | "create.repricing"
  | "approve.repricing"
  | "reject.repricing"
  | "create.supplier"
  | "print.invoice"
  | "touch.attendance"
  | "mobile.attendance";

const matrix: Record<Role, Permission[]> = {
  system_admin: [
    "crud.entities", "approve.line", "manage.vehicles", "approve.vehicle",
    "manage.shifts", "add.payment", "create.repricing", "approve.repricing",
    "create.supplier", "print.invoice", "touch.attendance",
  ],
  super_admin: [
    "crud.entities", "approve.line", "manage.vehicles", "approve.vehicle",
    "manage.shifts", "add.payment", "create.repricing", "approve.repricing",
    "reject.repricing", "create.supplier", "print.invoice", "touch.attendance",
  ],
  line_admin: [
    "crud.entities", "manage.vehicles", "manage.shifts", "add.payment",
    "create.repricing", "print.invoice",
  ],
  trans_admin: ["add.entities", "create.supplier", "touch.attendance"],
  supervisor: ["mobile.attendance"],
  reader: [],
};

export function can(role: Role | undefined, perm: Permission): boolean {
  if (!role) return false;
  return matrix[role]?.includes(perm) ?? false;
}

/** True if the role may add entities (either full CRUD or add-only). */
export function canAdd(role: Role | undefined): boolean {
  return can(role, "crud.entities") || can(role, "add.entities");
}

export type Period = "go" | "return" | "both";
export type ApprovalState = "approved" | "not_approved";

export interface Line {
  id: string;
  name: string;
  routesCount: number;
  approved: boolean;
}

export interface RouteItem {
  id: string;
  lineId: string;
  lineName: string;
  name: string;
  serial: string;
  supervisor: string;
  supplier: string;
  driver: string;
  fullCapacity: number;
  usersInRoute: number;
  actualCapacity: number;
  attended: number;
  stationCount: number;
  oneWay: boolean;
  fromTime?: string;
  toTime?: string;
  cost: number;
  religionM: number;
  religionC: number;
}

export interface Station {
  id: string;
  routeId: string;
  name: string;
  description?: string;
  active: boolean;
  lat?: number;
  lng?: number;
}

export interface PassengerAssignment {
  id: string;
  passengerId: string;
  passengerName: string;
  stationName?: string;
  period: Period;
}

export interface Passenger {
  id: string;
  name: string;
  identityNumber: string;
  mobile: string;
  identifier: string;
  active: boolean;
  photo?: string;
  homeLat?: number;
  homeLng?: number;
  routesCount: number;
}

export interface Vehicle {
  id: string;
  type: string;
  capacity: number;
  approved: boolean;
  active: boolean;
}

export interface ShiftDay {
  day: number; // 1=Sun .. 7=Sat
  active: boolean;
  from: string;
  to: string;
}
export interface ShiftGroup {
  id: string;
  number: number;
  days: ShiftDay[];
}

export interface ContactPerson {
  id: string;
  name: string;
  mobile: string;
}
export interface Supplier {
  id: string;
  name: string;
  createdAt: string;
  phone?: string;
  mobile?: string;
  logo?: string;
  activeRoutes: number;
  contacts: ContactPerson[];
}

export interface StatementRow {
  id: string;
  month: number;
  year: number;
  supplierId: string;
  supplier: string;
  routesCount: number;
  roundsFull: number;
  roundsHalfGo: number;
  roundsHalfReturn: number;
  totalDue: number;
  totalDeductions: number;
  normalPayments: number;
  advancePayments: number;
}

export interface Deduction {
  id: string;
  routeName: string;
  supplier: string;
  driver: string;
  day: string;
  amount: number;
  createdBy: string;
  createdAt: string;
  reason: string;
  type: "normal" | "tax";
}

export interface Repricing {
  id: string;
  amount: number;
  mode: "percent" | "fixed";
  forAllLines: boolean;
  createdBy: string;
  createdAt: string;
  approved: boolean;
  approvedBy?: string;
  startDate: string;
  lines: { lineName: string; before: number; after: number }[];
}

export interface Exception {
  id: string;
  routeName: string;
  passenger: string;
  station: string;
  lat?: number;
  lng?: number;
  date: string;
  fromDate?: string;
  toDate?: string;
  period: Period;
  weekdays?: string;
  reason: string;
  contact: string;
}

export interface AttendanceRecord {
  id: string;
  name: string;
  idCode: string;
  otherId: string;
  line: string;
  route: string;
  supervisor: string;
  supplier: string;
  driver: string;
  checkIn?: string;
  checkOut?: string;
  attended: boolean;
  history?: { date: string; checkIn?: string; checkOut?: string; attended: boolean }[];
}
