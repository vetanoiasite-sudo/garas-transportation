import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';

// Calendar-day boundaries [start, nextDay) for a parsed date, used to compare
// attendance timestamps "by calendar day" (§7.3 / §7.6).
function dayRange(d: Date): { gte: Date; lt: Date } {
  const gte = new Date(d.getFullYear(), d.getMonth(), d.getDate());
  const lt = new Date(gte);
  lt.setDate(lt.getDate() + 1);
  return { gte, lt };
}

// Filters accepted by the two dashboard reads (documented header filters, §6.9).
interface DashboardFilters {
  lineId?: number;
  supplierId?: number;
  serialBus?: string;
  routeId?: number;
}

interface AttendanceFilters extends DashboardFilters {
  fromDate?: string;
  toDate?: string;
  attendanceFlag?: string;
  employeeId?: string; // passenger/employee id — the fingerprint no. stored in HrUser.email
}

// Body of AddUsersAttedance (documented PascalCase keys, §6.9). CheckInOrCheckOut
// is a flag: true → record a check-in, false → record a check-out.
export interface AddAttendanceBody {
  type?: string;
  serial?: string;
  CheckInOrCheckOut?: boolean;
  CheckInLatitude?: number;
  CheckInLongtitud?: number; // frozen typo key
  CheckOutLatitude?: number;
  CheckOutLongtitud?: number; // frozen typo key
  CheckInRouteDirectionId?: number;
  CheckOutRouteDirectionId?: number;
}

@Injectable()
export class DashboardService {
  constructor(private readonly prisma: PrismaService) {}

  /**
   * GET DashBoard — single TransportationDashboardData object. Every value is a
   * String. Counters come from DB counts; attendance & percentage figures are
   * "0" until the attendance aggregation is wired.
   */
  async dashboard(dateSearch?: string, filters: DashboardFilters = {}) {
    if (!dateSearch || !dateSearch.trim()) {
      return fail('Err101', 'يرجى إدخال التاريخ');
    }

    const routeWhere: Record<string, unknown> = { active: true };
    if (filters.lineId) routeWhere.transportationLineId = filters.lineId;
    if (filters.supplierId) routeWhere.supplierId = filters.supplierId;
    if (filters.serialBus) routeWhere.serial = filters.serialBus;
    if (filters.routeId) routeWhere.id = filters.routeId;

    // Faithful to old DashBoard22: every scope counter is derived from the
    // FILTERED route set, so picking a route on the rail (or any filter) narrows
    // the KPIs — instead of returning global table counts that ignore the filter.
    const scopeRoutes = await this.prisma.transportationVehicleRoute.findMany({
      where: routeWhere,
      select: {
        id: true,
        serial: true,
        transportationLineId: true,
        transportationVehicleId: true,
        supplierId: true,
        oneWay: true,
        vehicle: { select: { vehicleTypeId: true } },
      },
    });
    const routeIds = scopeRoutes.map((r) => r.id);
    const distinctCount = (xs: (number | null | undefined)[]) =>
      new Set(xs.filter((x): x is number => x != null)).size;

    const transportLinesNum = distinctCount(scopeRoutes.map((r) => r.transportationLineId));
    const vehiclesNum = distinctCount(scopeRoutes.map((r) => r.transportationVehicleId));
    const suppliersNum = distinctCount(scopeRoutes.map((r) => r.supplierId));
    const allSuppliersNum = suppliersNum; // suppliers serving the in-scope routes
    const vehicleTypeNum = distinctCount(scopeRoutes.map((r) => r.vehicle?.vehicleTypeId));
    const twoWayVehiclesNum = scopeRoutes.filter((r) => !(r.oneWay ?? false)).length;
    const oneWayVehiclesNum = scopeRoutes.filter((r) => r.oneWay ?? false).length;

    // ---- Attendance figures for DateSearch (old system §7.3 / §7.6) ---------
    const parsed = new Date(dateSearch);
    if (isNaN(parsed.getTime())) return fail('Err101', 'يرجى إدخال تاريخ صحيح');
    const range = dayRange(parsed);

    // Bus (vehicle-round) attendance: Type='Bus' rows on the search day, scoped
    // to the in-scope route serials.
    const scopeSerials = scopeRoutes.map((r) => r.serial).filter((s): s is string => !!s);
    const busBase = { type: 'Bus', serial: { in: scopeSerials } };
    const [checkInBuses, checkOutBuses, oneWayBuses] = await this.prisma.$transaction([
      this.prisma.userAttendance.count({ where: { ...busBase, checkIn: range } }),
      this.prisma.userAttendance.count({ where: { ...busBase, checkOut: range } }),
      this.prisma.userAttendance.count({ where: { ...busBase, oneWayDate: range } }),
    ]);

    // Riders on the in-scope routes: distinct active/all memberships, resolved to
    // their fingerprint number (HrUser.email) for the attendance match (§7.6).
    const [activeMembers, allMembers] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRouteEmployee.findMany({
        where: { active: true, routeId: { in: routeIds } },
        select: { hrUserId: true, hrUser: { select: { email: true } } },
      }),
      this.prisma.transportationVehicleRouteEmployee.findMany({
        where: { routeId: { in: routeIds } },
        select: { hrUserId: true },
      }),
    ]);
    const hrUsersNum = new Set(activeMembers.map((m) => m.hrUserId)).size;
    const allHrUsersNum = new Set(allMembers.map((m) => m.hrUserId)).size;

    const riderEmailByUser = new Map<number, string>();
    for (const m of activeMembers) {
      if (m.hrUser?.email) riderEmailByUser.set(m.hrUserId, m.hrUser.email);
    }
    const riderEmails = [...new Set(riderEmailByUser.values())];

    // Person attendance rows with a check-in that day, restricted to rider emails.
    const attendedSerialsRows = riderEmails.length
      ? await this.prisma.userAttendance.findMany({
          where: { type: 'Person', checkIn: range, serial: { in: riderEmails } },
          select: { serial: true },
          distinct: ['serial'],
        })
      : [];
    const attendedSerials = new Set(attendedSerialsRows.map((r) => r.serial));
    let hrUsersAttendance = 0;
    for (const email of riderEmailByUser.values()) {
      if (attendedSerials.has(email)) hrUsersAttendance++;
    }
    const ridersInScope = hrUsersNum; // denominator for the user-attendance rate

    // Percentages: numerator/denominator*100, rounded to 1 decimal (§5 KPIs),
    // divide-by-zero guarded.
    const pct = (n: number, den: number) => (den > 0 ? String(Math.round((n / den) * 1000) / 10) : '0');

    const data = {
      TransportLinesNum: String(transportLinesNum),
      VehiclesNum: String(vehiclesNum),
      TwoWayVehiclesNum: String(twoWayVehiclesNum),
      OneWayVehiclesNum: String(oneWayVehiclesNum),
      HrUsersNum: String(hrUsersNum),
      SuppliersNum: String(suppliersNum),
      HrUsersPercent: pct(hrUsersAttendance, ridersInScope),
      CheckInVehiclePercent: pct(checkInBuses, twoWayVehiclesNum),
      CheckOutVehiclePercent: pct(checkOutBuses, twoWayVehiclesNum),
      OneWayVehiclePercent: pct(oneWayBuses, twoWayVehiclesNum),
      CheckInVehicleAttendanceNum: String(checkInBuses), // frozen singular Vehicle key
      CheckOutVehicleAttendanceNum: String(checkOutBuses), // frozen singular Vehicle key
      OneWayVehicleAttendanceNum: String(oneWayBuses), // frozen singular Vehicle key
      HrUsersAttendanceNum: String(hrUsersAttendance),
      AllHrUsersNum: String(allHrUsersNum),
      AllSuppliersNum: String(allSuppliersNum),
      VehiclesTypeNum: String(vehicleTypeNum), // frozen key VehiclesTypeNum
    };

    return success(data);
  }

  /**
   * GET DashBoardForAttendenceDuration — paginated TransportationDashboardAttendanceData
   * built from active route memberships (joined to hrUser / route / line / supplier).
   * AttendanceHistory is empty until userAttendance matching (by serial == hrUser.email)
   * is wired.
   */
  /** Shared WHERE builder for the attendance list + its Excel export (same filters). */
  private buildAttendanceWhere(filters: AttendanceFilters): Record<string, unknown> {
    const where: Record<string, unknown> = { active: true };
    if (filters.routeId) where.routeId = filters.routeId;
    // Passenger/employee id filter — the fingerprint no. is stored in HrUser.email.
    if (filters.employeeId) where.hrUser = { is: { email: { contains: filters.employeeId } } };
    const routeFilter: Record<string, unknown> = {};
    if (filters.lineId) routeFilter.transportationLineId = filters.lineId;
    if (filters.supplierId) routeFilter.supplierId = filters.supplierId;
    if (filters.serialBus) routeFilter.serial = filters.serialBus;
    if (Object.keys(routeFilter).length > 0) where.route = { is: routeFilter };
    return where;
  }

  async dashboardAttendance(pageNo = 1, noOfItems = 20, filters: AttendanceFilters = {}) {
    const where = this.buildAttendanceWhere(filters);

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRouteEmployee.count({ where }),
      this.prisma.transportationVehicleRouteEmployee.findMany({
        where,
        include: {
          hrUser: { include: { maritalStatus: true } },
          route: {
            include: {
              line: true,
              supplier: true,
              contactPerson: true,
              supervisor: true,
              vehicle: { include: { vehicleType: true } },
            },
          },
        },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const fullName = (u?: { firstName?: string | null; middleName?: string | null; lastName?: string | null } | null) =>
      u ? [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ') : '';

    // AttendanceHistory: userAttendance matched by serial == hrUser.email over
    // [FromDate, ToDate], one entry per calendar day (§7.3). Fetch once for the
    // whole page then group per serial+day.
    const from = filters.fromDate ? new Date(filters.fromDate) : undefined;
    const to = filters.toDate ? new Date(filters.toDate) : undefined;
    const lowerBound = from && !isNaN(from.getTime()) ? dayRange(from).gte : undefined;
    const upperBound = to && !isNaN(to.getTime()) ? dayRange(to).lt : undefined; // inclusive-of-day
    const bounds: Record<string, Date> = {};
    if (lowerBound) bounds.gte = lowerBound;
    if (upperBound) bounds.lt = upperBound;
    const hasBounds = Object.keys(bounds).length > 0;
    const dateCond = hasBounds ? bounds : { not: null };

    const serials = [...new Set(rows.map((m) => m.hrUser?.email).filter((e): e is string => !!e))];
    const attRows = serials.length
      ? await this.prisma.userAttendance.findMany({
          where: {
            type: 'Person',
            serial: { in: serials },
            OR: [{ checkIn: dateCond }, { checkOut: dateCond }, { oneWayDate: dateCond }],
          },
          orderBy: { id: 'asc' },
        })
      : [];

    // Group attendance rows into one { Date, CheckIn, CheckOut, ISAttendace } per serial+day.
    const dayKey = (d: Date) => `${d.getFullYear()}-${d.getMonth()}-${d.getDate()}`;
    type Entry = { Date: string; CheckIn: string; CheckOut: string; ISAttendace: boolean };
    const historyBySerial = new Map<string, Map<string, Entry>>();
    for (const a of attRows) {
      const anchor = a.checkIn ?? a.oneWayDate ?? a.checkOut;
      if (!anchor) continue;
      if (!historyBySerial.has(a.serial)) historyBySerial.set(a.serial, new Map());
      const perDay = historyBySerial.get(a.serial)!;
      const key = dayKey(anchor);
      const existing = perDay.get(key);
      const entry: Entry = existing ?? {
        Date: new Date(anchor.getFullYear(), anchor.getMonth(), anchor.getDate()).toISOString(),
        CheckIn: '',
        CheckOut: '',
        ISAttendace: false,
      };
      if (a.checkIn) {
        entry.CheckIn = a.checkIn.toISOString();
        entry.ISAttendace = true; // ISAttendace = has a check-in that day
      }
      if (a.checkOut) entry.CheckOut = a.checkOut.toISOString();
      perDay.set(key, entry);
    }
    const historyFor = (email?: string | null): Entry[] =>
      email && historyBySerial.has(email)
        ? [...historyBySerial.get(email)!.values()].sort((x, y) => x.Date.localeCompare(y.Date))
        : [];

    const data = rows.map((m) => ({
      Id: String(m.hrUserId),
      EmployeeId: m.hrUser?.email ?? '', // passenger/employee id (fingerprint no.)
      FirstName: m.hrUser?.firstName ?? '',
      MiddleName: m.hrUser?.middleName ?? '',
      LastName: m.hrUser?.lastName ?? '',
      TransportionlineName: m.route?.line?.lineName ?? '', // frozen typo key
      NameOfRoute: m.route?.nameOfRoute ?? '',
      SupplierName: m.route?.supplier?.name ?? '',
      supplierContactPersonName: m.route?.contactPerson?.name ?? '', // frozen lowercase key
      VehicleType: m.route?.vehicle?.vehicleType?.type ?? '',
      SupervisorName: fullName(m.route?.supervisor),
      MaritalStatus: m.hrUser?.maritalStatus?.name ?? '',
      AttendanceHistory: historyFor(m.hrUser?.email),
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET AttendanceExcell — base64 (.xlsx) attendance roster export (RTL, Arabic
   *  headers). Honours the same filters as the attendance list. */
  async attendanceExcel(filters: AttendanceFilters = {}) {
    const rows = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: this.buildAttendanceWhere(filters),
      include: {
        hrUser: { include: { maritalStatus: true } },
        route: {
          include: { line: true, supplier: true, contactPerson: true, supervisor: true, vehicle: { include: { vehicleType: true } } },
        },
      },
      orderBy: { id: 'desc' },
    });

    const fullName = (u?: { firstName?: string | null; middleName?: string | null; lastName?: string | null } | null) =>
      u ? [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ') : '';

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('الحضور');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'الاسم الأول', key: 'first', width: 18 },
      { header: 'الاسم الأوسط', key: 'middle', width: 18 },
      { header: 'الاسم الأخير', key: 'last', width: 18 },
      { header: 'رقم هوية الراكب', key: 'employeeId', width: 18 },
      { header: 'الخط', key: 'line', width: 20 },
      { header: 'خط السير', key: 'route', width: 20 },
      { header: 'المورد', key: 'supplier', width: 20 },
      { header: 'الشخص المسؤول', key: 'contact', width: 20 },
      { header: 'نوع المركبة', key: 'vehicleType', width: 16 },
      { header: 'المشرف', key: 'supervisor', width: 20 },
      { header: 'الحالة الاجتماعية', key: 'marital', width: 16 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const m of rows) {
      sheet.addRow({
        first: m.hrUser?.firstName ?? '',
        middle: m.hrUser?.middleName ?? '',
        last: m.hrUser?.lastName ?? '',
        employeeId: m.hrUser?.email ?? '', // passenger/employee id (fingerprint no.)
        line: m.route?.line?.lineName ?? '',
        route: m.route?.nameOfRoute ?? '',
        supplier: m.route?.supplier?.name ?? '',
        contact: m.route?.contactPerson?.name ?? '',
        vehicleType: m.route?.vehicle?.vehicleType?.type ?? '',
        supervisor: fullName(m.route?.supervisor),
        marital: m.hrUser?.maritalStatus?.name ?? '',
      });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /**
   * POST AddUsersAttedance — record a check-in or check-out for a serial. A
   * check-in opens a new attendance row; a check-out closes the most recent open
   * row for the serial (or opens one if none is pending).
   */
  async addUsersAttendance(body: AddAttendanceBody, userId: number) {
    if (!body?.serial?.trim()) return fail('Err101', 'serial is required');
    const type = body.type ?? 'Person';
    const serial = body.serial;
    const now = new Date();
    const isCheckIn = !!body.CheckInOrCheckOut;

    if (isCheckIn) {
      await this.prisma.userAttendance.create({
        data: {
          type,
          serial,
          checkIn: now,
          checkInRouteId: body.CheckInRouteDirectionId ?? null,
          checkInLatitude: body.CheckInLatitude ?? null,
          checkInLongitude: body.CheckInLongtitud ?? null,
          creationBy: userId,
        },
      });
    } else {
      const open = await this.prisma.userAttendance.findFirst({
        where: { serial, checkOut: null },
        orderBy: { id: 'desc' },
      });
      if (open) {
        await this.prisma.userAttendance.update({
          where: { id: open.id },
          data: {
            checkOut: now,
            checkOutRouteId: body.CheckOutRouteDirectionId ?? null,
            checkOutLatitude: body.CheckOutLatitude ?? null,
            checkOutLongitude: body.CheckOutLongtitud ?? null,
          },
        });
      } else {
        await this.prisma.userAttendance.create({
          data: {
            type,
            serial,
            checkOut: now,
            checkOutRouteId: body.CheckOutRouteDirectionId ?? null,
            checkOutLatitude: body.CheckOutLatitude ?? null,
            checkOutLongitude: body.CheckOutLongtitud ?? null,
            creationBy: userId,
          },
        });
      }
    }

    // A Bus punch bills a round leg: it records a TransportationRoundForRoute row
    // and feeds the monthly supplier account. (old AddUsersAttedance Bus branch,
    // svc:693-728, using the authoritative one-way=full / two-way=half pricing.)
    if (type === 'Bus') await this.recordBusRound(serial, isCheckIn, now);

    return successWrite();
  }

  /**
   * Record one bus leg for a serial's active route: a round row (two-way legs
   * carry checkIn/checkOut, one-way legs carry oneWayDate) plus an upsert of the
   * month's TransportationVehicleRouteAccount. A full two-way trip is two legs
   * (check-in + check-out), each priced at LineCost/2; a one-way leg is full.
   */
  private async recordBusRound(serial: string, isCheckIn: boolean, when: Date) {
    const route = await this.prisma.transportationVehicleRoute.findFirst({
      where: { serial, active: true },
      select: { id: true, supplierId: true, lineCost: true, oneWay: true },
    });
    if (!route) return; // no active route for this serial → nothing to bill
    const oneWay = route.oneWay ?? false;
    const price = oneWay ? route.lineCost : route.lineCost / 2;

    await this.prisma.transportationRoundForRoute.create({
      data: {
        routeId: route.id,
        supplierId: route.supplierId ?? null,
        serial,
        checkInDate: !oneWay && isCheckIn ? when : null,
        checkOutDate: !oneWay && !isCheckIn ? when : null,
        oneWayDate: oneWay ? when : null,
        priceRound: price,
      },
    });

    const monthStart = new Date(Date.UTC(when.getUTCFullYear(), when.getUTCMonth(), 1));
    const monthEnd = new Date(Date.UTC(when.getUTCFullYear(), when.getUTCMonth() + 1, 1));
    const account = await this.prisma.transportationVehicleRouteAccount.findFirst({
      where: { routeId: route.id, dateOfMonth: { gte: monthStart, lt: monthEnd } },
    });
    if (account) {
      await this.prisma.transportationVehicleRouteAccount.update({
        where: { id: account.id },
        data: { countOfRounds: { increment: 1 }, priceOfRounds: { increment: price } },
      });
    } else {
      await this.prisma.transportationVehicleRouteAccount.create({
        data: {
          routeId: route.id,
          supplierId: route.supplierId ?? null,
          serial,
          dateOfMonth: monthStart,
          countOfRounds: 1,
          priceOfRounds: price,
          deductPerRounds: 0,
        },
      });
    }
  }

  /** POST AddUsersAttedance44 — fingerprint-device sync ack. */
  async addUsersAttendance44() {
    // TODO: pull touch records from the fingerprint devices and persist them.
    return success('done');
  }
}
