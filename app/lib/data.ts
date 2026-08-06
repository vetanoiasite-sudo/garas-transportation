/* Mock data layer. Each dataset stands in for a typed API service module.
   TODO: replace the in-memory arrays with real API calls (axios) — the
   function signatures are shaped to match the documented endpoints. */

import type {
  Line, RouteItem, Station, Passenger, PassengerAssignment, Vehicle,
  Supplier, StatementRow, SupplierPayment, Deduction, Repricing, Exception,
  AttendanceRecord, SystemUser, CostReportRow,
} from "./types";

export const suppliers: Supplier[] = [
  { id: "s1", name: "شركة النقل السريع", createdAt: "2025-01-12", phone: "0223456789", mobile: "01001234567", email: "info@fast-trans.eg", fax: "0223456780", address: "المعادي، القاهرة", activeRoutes: 8, contacts: [
    { id: "c1", name: "محمود سعيد", mobile: "01001112223" },
    { id: "c2", name: "عمرو فتحي", mobile: "01004445556" },
  ]},
  { id: "s2", name: "مؤسسة الوفاء للمواصلات", createdAt: "2025-03-04", phone: "0227654321", mobile: "01112223334", email: "info@alwafaa.eg", fax: "0227654320", address: "مدينة نصر، القاهرة", activeRoutes: 5, contacts: [
    { id: "c3", name: "حسن علي", mobile: "01007778889" },
  ]},
  { id: "s3", name: "النخبة للنقل الجماعي", createdAt: "2024-11-20", phone: "0225551234", mobile: "01223334445", email: "info@elite-trans.eg", fax: "0225551230", address: "٦ أكتوبر، الجيزة", activeRoutes: 11, contacts: [
    { id: "c4", name: "طارق منصور", mobile: "01099887766" },
    { id: "c5", name: "سامي رضا", mobile: "01055443322" },
  ]},
];

export const lines: Line[] = [
  { id: "l1", name: "خط المعادي", routesCount: 4, approved: true },
  { id: "l2", name: "خط مدينة نصر", routesCount: 3, approved: true },
  { id: "l3", name: "خط ٦ أكتوبر", routesCount: 2, approved: false },
  { id: "l4", name: "خط حلوان", routesCount: 0, approved: true },
  { id: "l5", name: "خط المهندسين", routesCount: 5, approved: true },
];

export const routes: RouteItem[] = [
  { id: "r1", lineId: "l1", lineName: "خط المعادي", name: "المعادي - المصنع (صباحي)", serial: "1024", supervisor: "علي المشرف", supplier: "شركة النقل السريع", driver: "محمود سعيد", fullCapacity: 40, usersInRoute: 36, actualCapacity: 40, attended: 33, stationCount: 6, oneWay: false, fromTime: "07:00 AM", toTime: "05:00 PM", cost: 850, religionM: 30, religionC: 6 },
  { id: "r2", lineId: "l1", lineName: "خط المعادي", name: "المعادي - المصنع (مسائي)", serial: "1025", supervisor: "خالد فؤاد", supplier: "شركة النقل السريع", driver: "عمرو فتحي", fullCapacity: 28, usersInRoute: 22, actualCapacity: 28, attended: 20, stationCount: 4, oneWay: true, fromTime: "03:00 PM", cost: 600, religionM: 18, religionC: 4 },
  { id: "r3", lineId: "l2", lineName: "خط مدينة نصر", name: "مدينة نصر - المقر الرئيسي", serial: "2011", supervisor: "منى صابر", supplier: "مؤسسة الوفاء للمواصلات", driver: "حسن علي", fullCapacity: 50, usersInRoute: 47, actualCapacity: 50, attended: 45, stationCount: 8, oneWay: false, fromTime: "06:30 AM", toTime: "04:30 PM", cost: 1100, religionM: 40, religionC: 7 },
  { id: "r4", lineId: "l5", lineName: "خط المهندسين", name: "المهندسين - المصنع", serial: "3301", supervisor: "سعاد كمال", supplier: "النخبة للنقل الجماعي", driver: "طارق منصور", fullCapacity: 33, usersInRoute: 30, actualCapacity: 33, attended: 28, stationCount: 5, oneWay: false, fromTime: "07:15 AM", toTime: "05:15 PM", cost: 780, religionM: 25, religionC: 5 },
];

export const stationsByRoute: Record<string, Station[]> = {
  r1: [
    { id: "st1", routeId: "r1", name: "ميدان المعادي", description: "٦:٤٥ ص", active: true, lat: 29.9603, lng: 31.2569 },
    { id: "st2", routeId: "r1", name: "كورنيش المعادي", description: "٦:٥٥ ص", active: true, lat: 29.9530, lng: 31.2290 },
    { id: "st3", routeId: "r1", name: "طرة", description: "٧:٠٥ ص", active: true, lat: 29.9200, lng: 31.2700 },
    { id: "st4", routeId: "r1", name: "المعصرة", description: "٧:١٥ ص", active: false, lat: 29.8900, lng: 31.3000 },
  ],
  r3: [
    { id: "st5", routeId: "r3", name: "عباس العقاد", description: "٦:٢٠ ص", active: true, lat: 30.0570, lng: 31.3400 },
    { id: "st6", routeId: "r3", name: "مكرم عبيد", description: "٦:٣٠ ص", active: true, lat: 30.0500, lng: 31.3450 },
  ],
};

export const assignmentsByRoute: Record<string, PassengerAssignment[]> = {
  r1: [
    { id: "pa1", passengerId: "p1", passengerName: "أحمد محمد", stationName: "ميدان المعادي", period: "both" },
    { id: "pa2", passengerId: "p2", passengerName: "سارة علي", stationName: "كورنيش المعادي", period: "go" },
    { id: "pa3", passengerId: "p3", passengerName: "محمد إبراهيم", stationName: "طرة", period: "return" },
  ],
  r3: [
    { id: "pa4", passengerId: "p4", passengerName: "ليلى حسن", stationName: "عباس العقاد", period: "both" },
  ],
};

export const passengers: Passenger[] = [
  { id: "p1", name: "أحمد محمد", identityNumber: "29001011234567", mobile: "01001234567", identifier: "أعزب", active: true, routesCount: 2, homeLat: 29.96, homeLng: 31.25 },
  { id: "p2", name: "سارة علي", identityNumber: "29505054321987", mobile: "01112223334", identifier: "متزوجة", active: true, routesCount: 1 },
  { id: "p3", name: "محمد إبراهيم", identityNumber: "28812129876543", mobile: "01223334445", identifier: "متزوج", active: false, routesCount: 1 },
  { id: "p4", name: "ليلى حسن", identityNumber: "29303031122334", mobile: "01055667788", identifier: "أعزب", active: true, routesCount: 3 },
  { id: "p5", name: "خالد عبد الله", identityNumber: "29011119988776", mobile: "01099001122", identifier: "متزوج", active: true, routesCount: 2 },
];

/* Login users (the `User` table — admins & supervisors), NOT HR employees.
   Each user's `role` drives permission gating. */
export const users: SystemUser[] = [
  { id: "u1", name: "أحمد النظام", email: "admin@garas.co", mobile: "01000000001", role: "super_admin", roles: ["super_admin"], active: true },
  { id: "u2", name: "سارة المشرفة", email: "super@garas.co", mobile: "01000000002", role: "super_admin", roles: ["super_admin"], active: true },
  { id: "u3", name: "محمد المواصلات", email: "transport@garas.co", mobile: "01000000003", role: "transportation_admin", roles: ["transportation_admin"], active: true },
  { id: "u4", name: "خالد الموظفين", email: "hr@garas.co", mobile: "01000000004", role: "hr_admin", roles: ["hr_admin"], active: true },
  { id: "u5", name: "علي المشرف", email: "ali.sup@garas.co", mobile: "01000000005", role: "supervisor", roles: ["supervisor"], active: true },
  { id: "u6", name: "منى صابر", email: "mona.sup@garas.co", mobile: "01000000006", role: "supervisor", roles: ["supervisor"], active: true },
  { id: "u7", name: "قارئ التقارير", email: "reader@garas.co", mobile: "01000000007", role: "reader", roles: ["reader"], active: false },
];

export const vehicles: Vehicle[] = [
  { id: "v1", type: "أتوبيس ٥٠ راكب", capacity: 50, approved: true, active: true },
  { id: "v2", type: "ميكروباص ١٤ راكب", capacity: 14, approved: true, active: false },
  { id: "v3", type: "كوستر ٢٨ راكب", capacity: 28, approved: false, active: false },
  { id: "v4", type: "أتوبيس ٣٣ راكب", capacity: 33, approved: true, active: true },
  { id: "v5", type: "هايس ١٢ راكب", capacity: 12, approved: false, active: true },
];

export const vehicleTypes = ["أتوبيس ٥٠ راكب", "ميكروباص ١٤ راكب", "كوستر ٢٨ راكب", "أتوبيس ٣٣ راكب", "هايس ١٢ راكب"];

export const statement: StatementRow[] = [
  { id: "m1", month: 6, year: 2026, supplierId: "s1", supplier: "شركة النقل السريع", routesCount: 8, roundsFull: 120, roundsHalfGo: 14, roundsHalfReturn: 9, totalDue: 102000, totalDeductions: 8400, normalPayments: 60000, advancePayments: 20000 },
  { id: "m2", month: 6, year: 2026, supplierId: "s2", supplier: "مؤسسة الوفاء للمواصلات", routesCount: 5, roundsFull: 88, roundsHalfGo: 6, roundsHalfReturn: 4, totalDue: 71500, totalDeductions: 3200, normalPayments: 50000, advancePayments: 0 },
  { id: "m3", month: 6, year: 2026, supplierId: "s3", supplier: "النخبة للنقل الجماعي", routesCount: 11, roundsFull: 165, roundsHalfGo: 20, roundsHalfReturn: 12, totalDue: 143000, totalDeductions: 12500, normalPayments: 90000, advancePayments: 30000 },
];

/* Supplier payments (with per-month distribution for advances) — feeds the
   account-statement "View Payments" + distribution dialogs. */
export const supplierPayments: SupplierPayment[] = [
  { id: "sp1", supplierId: "s1", supplier: "شركة النقل السريع", amount: 60000, paymentDate: "2026-06-05", type: "normal" },
  { id: "sp2", supplierId: "s1", supplier: "شركة النقل السريع", amount: 20000, paymentDate: "2026-04-01", startDate: "2026-04-01", months: 4, type: "advance", distribution: [
    { month: 4, year: 2026, amount: 5000 },
    { month: 5, year: 2026, amount: 5000 },
    { month: 6, year: 2026, amount: 5000 },
    { month: 7, year: 2026, amount: 5000 },
  ]},
  { id: "sp3", supplierId: "s2", supplier: "مؤسسة الوفاء للمواصلات", amount: 50000, paymentDate: "2026-06-10", type: "normal" },
  { id: "sp4", supplierId: "s3", supplier: "النخبة للنقل الجماعي", amount: 90000, paymentDate: "2026-06-02", type: "normal" },
  { id: "sp5", supplierId: "s3", supplier: "النخبة للنقل الجماعي", amount: 30000, paymentDate: "2026-05-01", startDate: "2026-05-01", months: 3, type: "advance", distribution: [
    { month: 5, year: 2026, amount: 10000 },
    { month: 6, year: 2026, amount: 10000 },
    { month: 7, year: 2026, amount: 10000 },
  ]},
];

export function getSupplierPayments(supplierId?: string) {
  return supplierId ? supplierPayments.filter((p) => p.supplierId === supplierId) : supplierPayments;
}

export const deductions: Deduction[] = [
  { id: "d1", routeName: "المعادي - المصنع (صباحي)", supplier: "شركة النقل السريع", driver: "محمود سعيد", day: "2026-06-03", amount: 300, createdBy: "سارة المشرفة", createdAt: "2026-06-04", reason: "تأخير في موعد الوصول", type: "normal" },
  { id: "d2", routeName: "مدينة نصر - المقر الرئيسي", supplier: "مؤسسة الوفاء للمواصلات", driver: "حسن علي", day: "2026-06-10", amount: 1500, createdBy: "أحمد النظام", createdAt: "2026-06-11", reason: "ضريبة القيمة المضافة", type: "tax" },
  { id: "d3", routeName: "المهندسين - المصنع", supplier: "النخبة للنقل الجماعي", driver: "طارق منصور", day: "2026-06-15", amount: 450, createdBy: "محمد الخطوط", createdAt: "2026-06-16", reason: "عدم اكتمال العدد", type: "normal" },
];

export const repricings: Repricing[] = [
  { id: "rp1", amount: 10, mode: "percent", forAllLines: true, createdBy: "محمد الخطوط", createdAt: "2026-06-20", approved: false, startDate: "2026-08-01", lines: [] },
  { id: "rp2", amount: 50, mode: "fixed", forAllLines: false, createdBy: "سارة المشرفة", createdAt: "2026-05-11", approved: true, approvedBy: "أحمد النظام", startDate: "2026-06-01", lines: [
    { lineName: "المعادي - المصنع (صباحي)", before: 850, after: 900 },
    { lineName: "مدينة نصر - المقر الرئيسي", before: 1100, after: 1150 },
  ]},
];

export const exceptions: Exception[] = [
  { id: "e1", routeName: "المعادي - المصنع (صباحي)", passenger: "أحمد محمد", station: "كورنيش المعادي", lat: 29.953, lng: 31.229, date: "2026-07-01", period: "go", reason: "ظرف مؤقت", contact: "01001234567" },
  { id: "e2", routeName: "مدينة نصر - المقر الرئيسي", passenger: "ليلى حسن", station: "موقع حر", lat: 30.05, lng: 31.34, date: "", fromDate: "2026-07-05", toDate: "2026-07-20", period: "both", weekdays: "الأحد، الثلاثاء", reason: "تدريب خارجي", contact: "01055667788" },
];

export const attendance: AttendanceRecord[] = [
  { id: "a1", name: "أحمد محمد", idCode: "29001011234567", otherId: "أعزب", line: "خط المعادي", route: "المعادي - المصنع (صباحي)", serial: "1024", supervisor: "علي المشرف", supplier: "شركة النقل السريع", driver: "محمود سعيد", checkIn: "07:42 AM", checkOut: "05:10 PM", attended: true, history: [
    { date: "2026-07-08", checkIn: "07:40 AM", checkOut: "05:05 PM", attended: true },
    { date: "2026-07-07", checkIn: "07:45 AM", checkOut: "05:12 PM", attended: true },
    { date: "2026-07-06", attended: false },
  ]},
  { id: "a2", name: "سارة علي", idCode: "29505054321987", otherId: "متزوجة", line: "خط المعادي", route: "المعادي - المصنع (صباحي)", serial: "1024", supervisor: "علي المشرف", supplier: "شركة النقل السريع", driver: "محمود سعيد", checkIn: "07:38 AM", attended: true },
  { id: "a3", name: "محمد إبراهيم", idCode: "28812129876543", otherId: "متزوج", line: "خط المعادي", route: "المعادي - المصنع (مسائي)", serial: "1025", supervisor: "خالد فؤاد", supplier: "شركة النقل السريع", driver: "عمرو فتحي", attended: false },
  { id: "a4", name: "ليلى حسن", idCode: "29303031122334", otherId: "أعزب", line: "خط مدينة نصر", route: "مدينة نصر - المقر الرئيسي", serial: "2011", supervisor: "منى صابر", supplier: "مؤسسة الوفاء للمواصلات", driver: "حسن علي", checkIn: "06:35 AM", checkOut: "04:33 PM", attended: true },
];

/* Line Cost Report rows (transportationCosts). Net cost per docs §7.5:
   one-way: netCost = lineCost × rounds − deductions;
   two-way: netCost = (lineCost / 2) × rounds − deductions. */
export const costReport: CostReportRow[] = routes.map((r, i) => {
  const rounds = 20 + i * 6;
  const deductionRounds = i; // 0, 1, 2, ...
  const deductionCost = deductionRounds * 150;
  const perRound = r.oneWay ? r.cost : r.cost / 2;
  const netCost = Math.round(perRound * rounds - deductionCost);
  return {
    id: r.id,
    lineName: r.lineName,
    routeName: r.name,
    serial: r.serial,
    supplier: r.supplier,
    driver: r.driver,
    cost: r.cost,
    oneWay: r.oneWay,
    rounds,
    deductionRounds,
    deductionCost,
    netCost,
  };
});

/* --- helper getters (async to mimic API latency) --- */
export function getLine(id: string) { return lines.find((l) => l.id === id); }
export function getRoute(id: string) { return routes.find((r) => r.id === id); }
export function getRoutesForLine(lineId: string) { return routes.filter((r) => r.lineId === lineId); }
export function getSupplier(id: string) { return suppliers.find((s) => s.id === id); }
export function getPassenger(id: string) { return passengers.find((p) => p.id === id); }
export function getStations(routeId: string) { return stationsByRoute[routeId] ?? []; }
export function getAssignments(routeId: string) { return assignmentsByRoute[routeId] ?? []; }

export const monthNamesAr = ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"];
export const weekdayNamesAr = ["الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"];
