import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { RouteBody } from './routes.controller';
import { NotificationsService } from '../notifications/notifications.service';

// Joins a supervisor (HrUser) name from its parts.
function fullName(u?: { firstName?: string | null; middleName?: string | null; lastName?: string | null } | null): string {
  if (!u) return '';
  return [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');
}

// Rich include so list/detail rows can be flattened to the frozen JSON keys.
const routeInclude = {
  line: true,
  vehicle: { include: { vehicleType: true } },
  supplier: true,
  contactPerson: true,
  branchSchedule: true,
  supervisor: true,
  _count: { select: { directions: true, employees: { where: { active: true } } } },
} as const;

// Calendar-day boundaries [start, nextDay) for "today", to count today's check-ins.
function todayRange(): { gte: Date; lt: Date } {
  const now = new Date();
  const gte = new Date(now.getFullYear(), now.getMonth(), now.getDate());
  const lt = new Date(gte);
  lt.setDate(lt.getDate() + 1);
  return { gte, lt };
}

@Injectable()
export class RoutesService {
  constructor(
    private readonly prisma: PrismaService,
    private readonly notifications: NotificationsService,
  ) {}

  /** Tally each route's active passengers by their "other identifier" code (C / M).
   *  Returns a map routeId → { C, M }. Codes are kept opaque — no religion labels. */
  private async religionCounts(routeIds: number[]): Promise<Map<number, { C: number; M: number }>> {
    const map = new Map<number, { C: number; M: number }>();
    if (routeIds.length === 0) return map;
    const emps = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: { routeId: { in: routeIds }, active: true },
      select: { routeId: true, hrUser: { select: { maritalStatus: { select: { name: true } } } } },
    });
    for (const e of emps) {
      const t = map.get(e.routeId) ?? { C: 0, M: 0 };
      const name = e.hrUser?.maritalStatus?.name;
      if (name === 'C') t.C++;
      else if (name === 'M') t.M++;
      map.set(e.routeId, t);
    }
    return map;
  }

  /** Count today's attended passengers per route (distinct fingerprints with a
   *  Person check-in today whose checkInRouteId is the route). */
  private async attendedTodayByRoute(routeIds: number[]): Promise<Map<number, number>> {
    const map = new Map<number, number>();
    if (routeIds.length === 0) return map;
    const range = todayRange();
    const rows = await this.prisma.userAttendance.findMany({
      where: { type: 'Person', checkInRouteId: { in: routeIds }, checkIn: range },
      select: { serial: true, checkInRouteId: true },
    });
    const perRoute = new Map<number, Set<string>>();
    for (const a of rows) {
      if (a.checkInRouteId == null) continue;
      let set = perRoute.get(a.checkInRouteId);
      if (!set) perRoute.set(a.checkInRouteId, (set = new Set()));
      set.add(a.serial);
    }
    for (const [id, set] of perRoute) map.set(id, set.size);
    return map;
  }

  /** GET getAllTransportationRoute — paginated routes (TransportationRouteData rows). */
  async getAll(pageNo = 1, noOfItems = 20, filters: { lineId?: number; supplierId?: number; serialBus?: string; name?: string } = {}) {
    const where: Record<string, unknown> = { active: true };
    if (filters.lineId) where.transportationLineId = filters.lineId;
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.serialBus) where.serial = filters.serialBus;
    if (filters.name) where.nameOfRoute = { contains: filters.name };

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRoute.count({ where }),
      this.prisma.transportationVehicleRoute.findMany({
        where,
        include: routeInclude,
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const routeIds = rows.map((r) => r.id);
    const religion = await this.religionCounts(routeIds);
    const attended = await this.attendedTodayByRoute(routeIds);

    const data = rows.map((r) => {
      const userInRoute = r._count?.employees ?? 0; // registered passengers (active)
      return {
      Id: r.id,
      LineName: r.line?.lineName ?? '',
      LineId: r.transportationLineId,
      LineCost: r.lineCost,
      SupplierName: r.supplier?.name ?? '',
      SupplierId: r.supplierId ?? 0,
      SupplierContactPersonName: r.contactPerson?.name ?? '',
      SupplierContactPersonId: r.supplierContactPersonId ?? 0,
      Serial: r.serial,
      branchScheduleId: r.branchScheduleId ?? 0,
      PeriodFrom: r.periodFrom ? r.periodFrom.toISOString() : '',
      PeriodTo: r.periodTo ? r.periodTo.toISOString() : null,
      BusSupervisor: fullName(r.supervisor),
      FullCapacity: r.vehicle?.capacity ?? 0, // vehicle seat count
      UserInRoute: userInRoute, // registered passengers on the route
      // Net expected riders. Equals registered until the exception (opt-in/opt-out)
      // flow is wired; then adjust by Expection/RouteEmployeesToOtherLines.
      ActualCapacity: userInRoute,
      ActualUserAttedance: attended.get(r.id) ?? 0, // checked-in today (frozen typo key)
      ExpectionNumFromOtherLines: 0, // TODO (frozen typo key)
      RouteEmployeesToOtherLines: 0, // TODO
      DirectionNum: r._count?.directions ?? 0,
      MuslimNum: religion.get(r.id)?.M ?? 0, // count of "M" other-identifier
      christianNum: religion.get(r.id)?.C ?? 0, // count of "C" (lowercase key)
      FromoDate: r.fromTime ?? '', // frozen typo key
      ToDate: r.toTime ?? '',
      NameOfRoute: r.nameOfRoute,
      TransportationVehicleId: r.transportationVehicleId,
      TransportationVehicleName: r.vehicle?.vehicleType?.type ?? '',
      };
    });

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET getTransportationRoute — single route detail (TransportationRouteInfo, all strings). */
  async getOne(routeId: number) {
    if (!routeId) return fail('Err101', 'RouteId is required');
    const r = await this.prisma.transportationVehicleRoute.findUnique({
      where: { id: routeId },
      include: routeInclude,
    });
    if (!r) return fail('Err404', 'Route not found');

    const rc = (await this.religionCounts([r.id])).get(r.id) ?? { C: 0, M: 0 };
    const s = (v: unknown): string => (v === null || v === undefined ? '' : String(v));
    const data = {
      Id: s(r.id),
      LineId: s(r.transportationLineId),
      LineName: s(r.line?.lineName),
      SupplierId: s(r.supplierId),
      SupplierName: s(r.supplier?.name),
      SupplierContactPersonId: s(r.supplierContactPersonId),
      SupplierContactPersonName: s(r.contactPerson?.name),
      Serial: s(r.serial),
      branchScheduleId: s(r.branchScheduleId),
      branchSchedule: r.branchSchedule ? `${r.branchSchedule.from} - ${r.branchSchedule.to}` : '',
      PeriodFrom: r.periodFrom ? r.periodFrom.toISOString() : '',
      PeriodTo: r.periodTo ? r.periodTo.toISOString() : '',
      BuSupervisorId: s(r.supervisorId), // frozen typo key (BuSupervisorId)
      BusSupervisor: fullName(r.supervisor),
      FromoDate: s(r.fromTime), // frozen typo key
      ToDate: s(r.toTime),
      christianNum: s(rc.C), // count of "C" other-identifier (lowercase key)
      MuslimNum: s(rc.M), // count of "M" other-identifier
      NameOfRoute: s(r.nameOfRoute),
      LineCost: s(r.lineCost),
      TransportationVehicleId: s(r.transportationVehicleId),
      TransportationVehicleName: s(r.vehicle?.vehicleType?.type),
    };
    return success(data);
  }

  /** GET getTransportationRouteByHrUser — routes supervised by an HR user (TransportationRoutesByHrUserIdData rows). */
  async getByHrUser(hrUserId: number) {
    if (!hrUserId) return fail('Err101', 'HrUserId is required');
    const rows = await this.prisma.transportationVehicleRoute.findMany({
      where: { supervisorId: hrUserId, active: true },
      include: routeInclude,
      orderBy: { id: 'desc' },
    });

    const attended = await this.attendedTodayByRoute(rows.map((r) => r.id));

    const data = rows.map((r) => {
      const userInRoute = r._count?.employees ?? 0;
      return {
        Id: r.id,
        LineName: r.line?.lineName ?? '',
        LineId: r.transportationLineId,
        SupplierName: r.supplier?.name ?? '',
        SupplierId: r.supplierId ?? 0,
        SupplierContactPersonName: r.contactPerson?.name ?? '',
        SupplierContactPersonId: r.supplierContactPersonId ?? 0,
        Serial: r.serial,
        branchScheduleId: r.branchScheduleId ?? 0,
        PeriodFrom: r.periodFrom ? r.periodFrom.toISOString() : '',
        PeriodTo: r.periodTo ? r.periodTo.toISOString() : null,
        BusSupervisor: fullName(r.supervisor),
        FullCapacity: r.vehicle?.capacity ?? 0, // vehicle seat count
        UserInRoute: userInRoute, // registered passengers on the route
        ActualCapacity: userInRoute,
        ActualUserAttedance: attended.get(r.id) ?? 0, // checked-in today (frozen typo key)
        ExpectionNumFromOtherLines: 0, // TODO: exception (opt-in/opt-out) math (frozen typo key)
        RouteEmployeesToOtherLines: 0, // TODO: exception (opt-in/opt-out) math
        DirectionNum: r._count?.directions ?? 0,
      };
    });
    return success(data);
  }

  /** GET RoutesWithUsersExcel — every active route with its passengers as a
   *  base64 (.xlsx) file (RTL, Arabic headers). One row per (route, passenger);
   *  routes with no passengers still get a single row. */
  async routesWithUsersExcel() {
    const routes = await this.prisma.transportationVehicleRoute.findMany({
      where: { active: true },
      include: {
        line: true,
        supplier: true,
        employees: { where: { active: true }, include: { hrUser: true, direction: true } },
      },
      orderBy: { id: 'desc' },
    });

    const periodLabel: Record<string, string> = { Go: 'ذهاب', Return: 'عودة', Both: 'ذهاب وعودة' };
    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('الخطوط مع الموظفين');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'الرقم المسلسل', key: 'serial', width: 14 },
      { header: 'خط السير', key: 'route', width: 24 },
      { header: 'محطة التجمع', key: 'line', width: 20 },
      { header: 'المورد', key: 'supplier', width: 20 },
      { header: 'الراكب', key: 'passenger', width: 24 },
      { header: 'رقم هوية الراكب', key: 'employeeId', width: 18 },
      { header: 'الجوال', key: 'mobile', width: 16 },
      { header: 'المحطة', key: 'station', width: 20 },
      { header: 'الاتجاه', key: 'period', width: 14 },
    ];
    sheet.getRow(1).font = { bold: true };

    for (const r of routes) {
      const base = { serial: r.serial, route: r.nameOfRoute, line: r.line?.lineName ?? '', supplier: r.supplier?.name ?? '' };
      if (r.employees.length === 0) {
        sheet.addRow(base);
        continue;
      }
      for (const e of r.employees) {
        sheet.addRow({
          ...base,
          passenger: fullName(e.hrUser),
          employeeId: e.hrUser?.email ?? '', // passenger/employee id (fingerprint no.)
          mobile: e.hrUser?.mobile ?? '',
          station: e.direction?.routeDirection ?? '',
          period: periodLabel[e.period] ?? e.period,
        });
      }
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /** Resolve the route's gathering-station (line) id: use the picked one, or
   *  auto-create a new station named after the route when none was chosen. */
  private async resolveLineId(body: RouteBody, userId: number): Promise<number> {
    const id = Number(body.TransportationLineId) || 0;
    if (id > 0) return id;
    const line = await this.prisma.transportationLine.create({
      data: {
        lineName: body.NameOfRoute?.trim() || 'خط',
        isApproved: false,
        creationBy: userId,
        modifiedBy: userId,
      },
    });
    return line.id;
  }

  /** POST AddTransportationRoute — create a route (auto serial, seed 70000000). */
  async add(body: RouteBody, userId: number) {
    if (!body?.NameOfRoute?.trim()) return fail('Err101', 'NameOfRoute is required');
    const serial = await this.nextSerial();
    const transportationLineId = await this.resolveLineId(body, userId);
    const created = await this.prisma.transportationVehicleRoute.create({
      data: {
        transportationLineId,
        transportationVehicleId: Number(body.TransportationVehicleId) || 0,
        supplierId: body.SupplierId ? Number(body.SupplierId) : null,
        supplierContactPersonId: body.SupplierContactPersonId ? Number(body.SupplierContactPersonId) : null,
        branchScheduleId: body.BranchScheduleId ? Number(body.BranchScheduleId) : null,
        supervisorId: body.HrUserId ? Number(body.HrUserId) : null,
        serial,
        periodFrom: body.PeriodFrom ? new Date(body.PeriodFrom) : null,
        fromTime: body.FromDate ?? null,
        toTime: body.ToDate ?? null,
        nameOfRoute: body.NameOfRoute,
        lineCost: Number(body.LineCost) || 0,
        oneWay: body.OneWay ?? false,
        active: body.Active ?? true,
        creationBy: userId,
        modifiedBy: userId,
      },
    });
    await this.notifications.notifySuperAdmins(userId, {
      title: 'إضافة خط جديد',
      description: 'بحاجة لاعتماد',
      entityType: 'route',
      entityId: created.id,
    });
    return successWrite(created.id);
  }

  /** POST UpdateTransportationRoute — update a route (serial preserved). */
  async update(id: number, body: RouteBody, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    const transportationLineId = await this.resolveLineId(body, userId);
    await this.prisma.transportationVehicleRoute.update({
      where: { id },
      data: {
        transportationLineId,
        transportationVehicleId: Number(body.TransportationVehicleId) || 0,
        supplierId: body.SupplierId ? Number(body.SupplierId) : null,
        supplierContactPersonId: body.SupplierContactPersonId ? Number(body.SupplierContactPersonId) : null,
        branchScheduleId: body.BranchScheduleId ? Number(body.BranchScheduleId) : null,
        supervisorId: body.HrUserId ? Number(body.HrUserId) : null,
        periodFrom: body.PeriodFrom ? new Date(body.PeriodFrom) : null,
        fromTime: body.FromDate ?? null,
        toTime: body.ToDate ?? null,
        nameOfRoute: body.NameOfRoute ?? '',
        lineCost: Number(body.LineCost) || 0,
        oneWay: body.OneWay ?? false,
        active: body.Active ?? true,
        modifiedBy: userId,
      },
    });
    return successWrite(id);
  }

  /** POST DeleteTransportationRoute — block if the route has employees, else delete directions + route. */
  async remove(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const employees = await this.prisma.transportationVehicleRouteEmployee.count({ where: { routeId: id } });
    if (employees > 0) {
      return fail('Err102', 'لا يمكن حذف المسار لوجود موظفين مرتبطين به');
    }
    await this.prisma.$transaction([
      this.prisma.transportationVehicleRouteDirection.deleteMany({ where: { routeId: id } }),
      this.prisma.transportationVehicleRoute.delete({ where: { id } }),
    ]);
    return successWrite(id);
  }

  /** POST ApproveRoute — toggles `active` (per docs ApproveRoute sets Active). */
  async approve(id: number, approve: boolean, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRoute.update({
      where: { id },
      data: { active: approve, modifiedBy: userId },
    });
    return successWrite(id);
  }

  /** Next serial = (max numeric serial) + 1, seeded at 70000000 when none exist. */
  private async nextSerial(): Promise<string> {
    const rows = await this.prisma.transportationVehicleRoute.findMany({ select: { serial: true } });
    const max = rows.reduce((m, r) => {
      const n = Number(r.serial);
      return Number.isFinite(n) && n > m ? n : m;
    }, 0);
    return String(max > 0 ? max + 1 : 70000000);
  }
}
