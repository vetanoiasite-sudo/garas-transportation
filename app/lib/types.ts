/* Domain types + role/permission model (from the business documentation §2). */

/* Four panel roles + two mobile-only roles (supervisor & passenger) that never
 * appear in the admin panel but still exist as login accounts for the app.  */
export type Role =
  | "super_admin" // مسؤول عام — everything, incl. approvals & user management
  | "transportation_admin" // مسئول مواصلات — transportation + money
  | "hr_admin" // مسئول الموظفين — passengers + attendance + exceptions
  | "reader" // مشاهد — read-only
  | "supervisor" // mobile only (route supervisor / mobile attendance)
  | "passenger"; // mobile only

/** The four roles the admin panel exposes (create-user dropdown, etc.). */
export const PANEL_ROLES: Role[] = ["super_admin", "transportation_admin", "hr_admin", "reader"];

export const roleLabelKey: Record<Role, string> = {
  super_admin: "role.superAdmin",
  transportation_admin: "role.transportationAdmin",
  hr_admin: "role.hrAdmin",
  reader: "role.reader",
  supervisor: "role.supervisor",
  passenger: "role.passenger",
};

export type Permission =
  | "crud.entities" // add/edit/delete line, route, station (transportation)
  | "approve.line"
  | "manage.vehicles"
  | "approve.vehicle"
  | "add.payment"
  | "create.repricing"
  | "approve.repricing"
  | "reject.repricing"
  | "create.supplier"
  | "print.invoice"
  | "manage.passengers" // employees/passengers + their route assignments
  | "touch.attendance"
  | "manage.exceptions"
  | "mobile.attendance"
  | "manage.users" // create/edit login users & assign their transportation role
  | "view.financialReports"; // supplier account statement + line cost report (super admin only)

// Super admin's full permission set. The transportation admin (مسئول مواصلات)
// shares it EXACTLY (business decision 2026-08-18): both form the approval
// group and both receive approval notifications.
const SUPER_PERMS: Permission[] = [
  "crud.entities", "approve.line", "manage.vehicles", "approve.vehicle",
  "add.payment", "create.repricing", "approve.repricing", "reject.repricing",
  "create.supplier", "print.invoice", "manage.passengers", "touch.attendance",
  "manage.exceptions", "manage.users", "view.financialReports",
];

const matrix: Record<Role, Permission[]> = {
  super_admin: SUPER_PERMS,
  transportation_admin: SUPER_PERMS,
  // Employees + their route assignment + attendance + exceptions.
  hr_admin: ["manage.passengers", "touch.attendance", "manage.exceptions"],
  reader: [],
  supervisor: ["mobile.attendance"],
  passenger: [],
};

export function can(role: Role | undefined, perm: Permission): boolean {
  if (!role) return false;
  return matrix[role]?.includes(perm) ?? false;
}

/** True if the role may add/edit transportation entities (lines/routes/stations). */
export function canAdd(role: Role | undefined): boolean {
  return can(role, "crud.entities");
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
  // Ids for the edit form (populated by the single-route detail endpoint).
  supplierId?: string;
  contactPersonId?: string;
  branchScheduleId?: string;
  supervisorId?: string;
  vehicleId?: string;
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
  maritalStatusId?: string;
  active: boolean;
  photo?: string;
  homeLat?: number;
  homeLng?: number;
  routesCount: number;
}

/** A login user (the `User` table — admins & supervisors), distinct from a
 *  Passenger (HrUser employee). Its `role` drives the app's permission gating. */
export interface SystemUser {
  id: string;
  name: string;
  email: string;
  mobile: string;
  /** Strongest role (display/back-compat) — first of `roles` by rank. */
  role: Role;
  /** All active roles (multi-role accounts). */
  roles: Role[];
  active: boolean;
}

export interface Vehicle {
  id: string;
  type: string;
  capacity: number;
  approved: boolean;
  active: boolean;
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
  email?: string;
  fax?: string;
  address?: string;
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

export interface SupplierPaymentDistribution {
  month: number;
  year: number;
  amount: number;
}
export interface SupplierPayment {
  id: string;
  supplierId: string;
  supplier: string;
  amount: number;
  paymentDate: string;
  startDate?: string;
  months?: number;
  type: "advance" | "normal";
  distribution?: SupplierPaymentDistribution[];
}

/** A row of the Line Cost Report (transportationCosts / TransportationReportCostData). */
export interface CostReportRow {
  id: string;
  lineName: string;   // محطة التجمع
  routeName: string;  // الخط
  serial: string;
  supplier: string;
  driver: string;
  cost: number;
  oneWay: boolean;
  rounds: number;
  deductionRounds: number;
  deductionCost: number;
  netCost: number;
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
  idCode: string; // passenger identity number (الرقم القومي)
  otherId: string; // passenger "another identifier"
  line: string;
  route: string;
  serial: string; // the route's serial (used by the serial filter)
  supervisor: string;
  supplier: string;
  driver: string;
  checkIn?: string;
  checkOut?: string;
  attended: boolean;
  history?: { date: string; checkIn?: string; checkOut?: string; attended: boolean }[];
}
