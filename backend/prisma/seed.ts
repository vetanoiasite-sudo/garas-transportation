import 'dotenv/config';
import { PrismaClient } from '@prisma/client';
import * as bcrypt from 'bcryptjs';

// Demo seed — roles + admin login, plus a full slice of transportation data
// (suppliers, vehicles, shifts, lines, routes, stations, passengers, memberships,
// attendance, rounds, deductions) so every screen shows real numbers.
const prisma = new PrismaClient();

// The system has exactly four panel roles + two mobile-only roles (supervisor,
// passenger). Ids are the backend transportation role ids the frontend maps.
const ROLES = [
  { id: 216, name: 'Transportation Super Admin' }, // مسؤول عام
  { id: 213, name: 'Transportation Admin' }, // مسئول المواصلات
  { id: 220, name: 'Transportation HR Admin' }, // مسئول الموظفين
  { id: 221, name: 'Transportation Reader' }, // مشاهد
  { id: 214, name: 'Transportation Supervisor' }, // mobile only
  { id: 215, name: 'Transportation Passenger' }, // mobile only
];
// Legacy roles that were consolidated away — dropped from the DB on seed.
const REMOVED_ROLE_IDS = [137, 210, 30];

// One login account per panel role (all share the demo password).
const ROLE_ACCOUNTS = [
  { email: 'admin@garas.co', firstName: 'أحمد', lastName: 'النظام', roleId: 216 },
  { email: 'transport@garas.co', firstName: 'محمد', lastName: 'المواصلات', roleId: 213 },
  { email: 'hr@garas.co', firstName: 'خالد', lastName: 'الموظفين', roleId: 220 },
  { email: 'viewer@garas.co', firstName: 'سلمى', lastName: 'المشاهدة', roleId: 221 },
];

async function main() {
  for (const r of ROLES) {
    await prisma.role.upsert({ where: { id: r.id }, update: { name: r.name }, create: r });
  }
  // Purge the consolidated-away roles and any links to them.
  await prisma.userRole.deleteMany({ where: { roleId: { in: REMOVED_ROLE_IDS } } });
  await prisma.role.deleteMany({ where: { id: { in: REMOVED_ROLE_IDS } } });

  const password = await bcrypt.hash('demo1234', 10);
  // Create/ensure one login per panel role, each with exactly its own role.
  let admin = null as Awaited<ReturnType<typeof prisma.user.upsert>> | null;
  for (const acc of ROLE_ACCOUNTS) {
    const u = await prisma.user.upsert({
      where: { email: acc.email },
      update: { firstName: acc.firstName, lastName: acc.lastName, active: true },
      create: { email: acc.email, password, firstName: acc.firstName, lastName: acc.lastName, active: true, branchId: 1 },
    });
    // Ensure the account carries ONLY its designated role.
    await prisma.userRole.deleteMany({ where: { userId: u.id, roleId: { notIn: [acc.roleId] } } });
    const has = await prisma.userRole.findFirst({ where: { userId: u.id, roleId: acc.roleId } });
    if (!has) await prisma.userRole.create({ data: { userId: u.id, roleId: acc.roleId, active: true } });
    if (acc.roleId === 216) admin = u;
  }
  if (!admin) throw new Error('super-admin account missing');

  // "Other Identifier" lookup — stored as opaque codes C / M (kept as-is in the DB
  // per business requirement; the UI never labels them). Fixed ids, idempotent.
  const MARITAL = [
    { id: 1, name: 'C' },
    { id: 2, name: 'M' },
  ];
  for (const m of MARITAL) {
    await prisma.maritalStatus.upsert({ where: { id: m.id }, update: { name: m.name }, create: m });
  }

  // Supervisors are LOGIN USERS (User table) with the Transportation Supervisor
  // role (214) — NOT HrUser/passenger records. Seeded idempotently so the route
  // form's supervisor dropdown always has real users to pick from.
  const SUPERVISORS = [
    { email: 'ali.supervisor@garas.co', firstName: 'علي', lastName: 'المشرف' },
    { email: 'mona.supervisor@garas.co', firstName: 'منى', lastName: 'صابر' },
    { email: 'khaled.supervisor@garas.co', firstName: 'خالد', lastName: 'فؤاد' },
  ];
  const supervisorUsers = [];
  for (const su of SUPERVISORS) {
    const u = await prisma.user.upsert({
      where: { email: su.email },
      update: { firstName: su.firstName, lastName: su.lastName, active: true },
      create: { email: su.email, password, firstName: su.firstName, lastName: su.lastName, active: true, branchId: 1 },
    });
    const hasRole = await prisma.userRole.findFirst({ where: { userId: u.id, roleId: 214 } });
    if (!hasRole) await prisma.userRole.create({ data: { userId: u.id, roleId: 214, active: true } });
    supervisorUsers.push(u);
  }

  // Only seed the demo dataset once (fresh DB).
  if ((await prisma.supplier.count()) > 0) {
    console.log('Demo data already present — roles/admin ensured.');
    return;
  }

  // Fixed ids seeded above (1 = أعزب, 2 = متزوج).
  const cStatus = { id: 1 };
  const mStatus = { id: 2 };

  const s1 = await prisma.supplier.create({
    data: { name: 'شركة النقل السريع', email: 'info@fast.eg', phone: '0223456789', mobile: '01001234567', address: 'المعادي', contacts: { create: [{ name: 'محمود سعيد', mobile: '01001112223' }, { name: 'عمرو فتحي', mobile: '01004445556' }] } },
    include: { contacts: true },
  });
  const s2 = await prisma.supplier.create({
    data: { name: 'مؤسسة الوفاء', email: 'info@wafaa.eg', phone: '0227654321', mobile: '01112223334', address: 'مدينة نصر', contacts: { create: [{ name: 'حسن علي', mobile: '01007778889' }] } },
    include: { contacts: true },
  });

  const t50 = await prisma.vehicleType.create({ data: { type: 'أتوبيس ٥٠ راكب' } });
  const t28 = await prisma.vehicleType.create({ data: { type: 'كوستر ٢٨ راكب' } });
  const v1 = await prisma.transportationVehicle.create({ data: { vehicleTypeId: t50.id, capacity: 50, isApproved: true, active: true } });
  const v2 = await prisma.transportationVehicle.create({ data: { vehicleTypeId: t28.id, capacity: 28, isApproved: true, active: true } });

  const shiftNumber = 1;
  for (let d = 1; d <= 5; d++) {
    await prisma.branchSchedule.create({ data: { shiftNumber, weekDayId: d, from: '08:00', to: '16:00', active: true, branchId: 1, createdBy: admin.id } });
  }
  const shift = await prisma.branchSchedule.findFirst({ where: { shiftNumber } });

  const lineMaadi = await prisma.transportationLine.create({ data: { lineName: 'خط المعادي', isApproved: true, creationBy: admin.id, modifiedBy: admin.id } });
  const lineNasr = await prisma.transportationLine.create({ data: { lineName: 'خط مدينة نصر', isApproved: true, creationBy: admin.id, modifiedBy: admin.id } });

  // Route supervisor = a login User (seeded above), not an HrUser.
  const supervisor = supervisorUsers[0];

  const r1 = await prisma.transportationVehicleRoute.create({
    data: { transportationLineId: lineMaadi.id, transportationVehicleId: v1.id, supplierId: s1.id, supplierContactPersonId: s1.contacts[0].id, branchScheduleId: shift?.id, supervisorId: supervisor.id, serial: '1024', nameOfRoute: 'المعادي - المصنع (صباحي)', lineCost: 850, oneWay: false, fromTime: '07:00', toTime: '17:00', active: true, isApproved: true, creationBy: admin.id, modifiedBy: admin.id },
  });
  const r2 = await prisma.transportationVehicleRoute.create({
    data: { transportationLineId: lineNasr.id, transportationVehicleId: v2.id, supplierId: s2.id, supplierContactPersonId: s2.contacts[0].id, branchScheduleId: shift?.id, supervisorId: supervisor.id, serial: '2011', nameOfRoute: 'مدينة نصر - المقر الرئيسي', lineCost: 1100, oneWay: false, fromTime: '06:30', toTime: '16:30', active: true, isApproved: true, creationBy: admin.id, modifiedBy: admin.id },
  });

  const dirMaadi1 = await prisma.transportationVehicleRouteDirection.create({ data: { routeId: r1.id, routeDirection: 'ميدان المعادي', description: '٦:٤٥ ص', latitude: 29.9603, longitude: 31.2569, active: true } });
  const dirMaadi2 = await prisma.transportationVehicleRouteDirection.create({ data: { routeId: r1.id, routeDirection: 'كورنيش المعادي', description: '٦:٥٥ ص', latitude: 29.953, longitude: 31.229, active: true } });
  const dirNasr1 = await prisma.transportationVehicleRouteDirection.create({ data: { routeId: r2.id, routeDirection: 'عباس العقاد', description: '٦:٢٠ ص', latitude: 30.057, longitude: 31.34, active: true } });

  // passengers (HrUser) with numeric email = fingerprint number
  const passengers = await Promise.all([
    prisma.hrUser.create({ data: { firstName: 'أحمد', lastName: 'محمد', email: '10001', mobile: '01001234567', identityNumber: '29001011234567', maritalStatusId: mStatus.id, active: true } }),
    prisma.hrUser.create({ data: { firstName: 'سارة', lastName: 'علي', email: '10002', mobile: '01112223334', identityNumber: '29505054321987', maritalStatusId: cStatus.id, active: true } }),
    prisma.hrUser.create({ data: { firstName: 'محمد', lastName: 'إبراهيم', email: '10003', mobile: '01223334445', identityNumber: '28812129876543', maritalStatusId: mStatus.id, active: true } }),
    prisma.hrUser.create({ data: { firstName: 'ليلى', lastName: 'حسن', email: '10004', mobile: '01055667788', identityNumber: '29303031122334', maritalStatusId: cStatus.id, active: true } }),
  ]);

  await prisma.transportationVehicleRouteEmployee.createMany({
    data: [
      { routeId: r1.id, hrUserId: passengers[0].id, directionId: dirMaadi1.id, period: 'Both', active: true },
      { routeId: r1.id, hrUserId: passengers[1].id, directionId: dirMaadi2.id, period: 'Go', active: true },
      { routeId: r2.id, hrUserId: passengers[2].id, directionId: dirNasr1.id, period: 'Both', active: true },
      { routeId: r2.id, hrUserId: passengers[3].id, directionId: dirNasr1.id, period: 'Return', active: true },
    ],
  });

  // attendance today (Person rows keyed by fingerprint serial + a Bus row)
  const today = new Date();
  const at = (h: number, m: number) => new Date(today.getFullYear(), today.getMonth(), today.getDate(), h, m);
  await prisma.userAttendance.createMany({
    data: [
      { type: 'Person', serial: '10001', checkIn: at(7, 42), checkOut: at(17, 10), checkInRouteId: r1.id, creationBy: admin.id },
      { type: 'Person', serial: '10002', checkIn: at(7, 38), checkInRouteId: r1.id, creationBy: admin.id },
      { type: 'Person', serial: '10004', checkIn: at(6, 35), checkOut: at(16, 33), checkInRouteId: r2.id, creationBy: admin.id },
      { type: 'Bus', serial: '1024', checkIn: at(7, 0), checkOut: at(17, 0), checkInRouteId: r1.id, creationBy: admin.id },
      { type: 'Bus', serial: '2011', checkIn: at(6, 30), checkOut: at(16, 30), checkInRouteId: r2.id, creationBy: admin.id },
    ],
  });

  // Billable rounds this month + a deduction (for cost report / statement).
  // Leg model (faithful to the old round generator + AddUsersAttedance): each
  // two-way trip is stored as TWO rows — an inbound (checkIn) leg and an outbound
  // (checkOut) leg — each priced at LineCost/2. This is what makes the Cost report
  // (counts checkIns + checkOuts) and the Account statement (complete rounds =
  // legs/2) reconcile to the same figure. r1: one full trip = 850; r2: one full
  // trip = 1100.
  await prisma.transportationRoundForRoute.createMany({
    data: [
      { routeId: r1.id, supplierId: s1.id, serial: '1024', checkInDate: at(7, 0), priceRound: 425 },
      { routeId: r1.id, supplierId: s1.id, serial: '1024', checkOutDate: at(17, 0), priceRound: 425 },
      { routeId: r2.id, supplierId: s2.id, serial: '2011', checkInDate: at(6, 30), priceRound: 550 },
      { routeId: r2.id, supplierId: s2.id, serial: '2011', checkOutDate: at(16, 30), priceRound: 550 },
    ],
  });
  await prisma.transportationVehicleRouteDeduction.create({
    data: { supplierId: s1.id, routeId: r1.id, serial: '1024', dateOfDeduction: at(0, 0), deductPerRound: 150, cause: 'تأخير', typeOfDedct: 'Normal', creationBy: admin.id },
  });

  // Monthly account rows (this month) so the supplier account statement has data.
  // Use a UTC mid-month date so it falls inside the statement's UTC month range.
  const monthStart = new Date(Date.UTC(today.getFullYear(), today.getMonth(), 15));
  await prisma.transportationVehicleRouteAccount.createMany({
    data: [
      { routeId: r1.id, supplierId: s1.id, serial: '1024', dateOfMonth: monthStart, countOfRounds: 2, priceOfRounds: 850, deductPerRounds: 150 },
      { routeId: r2.id, supplierId: s2.id, serial: '2011', dateOfMonth: monthStart, countOfRounds: 2, priceOfRounds: 1100, deductPerRounds: 0 },
    ],
  });

  // A passenger exception (opt-out) tied to a direction, so the exceptions screen
  // shows a row with a resolved direction + creator name.
  const dirMaadi = await prisma.transportationVehicleRouteDirection.findFirst({ where: { routeId: r1.id } });
  await prisma.transportationVehicleRouteEmployeeException.create({
    data: {
      hrUserId: passengers[1].id,
      routeId: r1.id,
      directionId: dirMaadi?.id ?? null,
      period: 'Go',
      exceptionDate: at(0, 0),
      dayName: 'الأحد',
      reasonException: 'إجازة',
      contactNumber: '01112223334',
      active: true,
      creationBy: admin.id,
    },
  });

  // A pending line-repricing request (10% increase) so the repricing screen has a row.
  await prisma.transportationLineIncreaseRequest.create({
    data: {
      isPercent: true,
      increaseCost: 10,
      forAllLines: true,
      approximateToFiveFlag: false,
      startDate: monthStart,
      approve: null,
      creationBy: admin.id,
      lines: {
        create: [
          { routeId: r1.id, priceBefore: r1.lineCost, priceAfter: Math.round(r1.lineCost * 1.1) },
          { routeId: r2.id, priceBefore: r2.lineCost, priceAfter: Math.round(r2.lineCost * 1.1) },
        ],
      },
    },
  });

  console.log('Seed complete. Login: admin@garas.co / demo1234 (CompanyName: demo). Full demo dataset created.');
}

main()
  .catch((e) => {
    console.error(e);
    process.exit(1);
  })
  .finally(() => prisma.$disconnect());
