import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { ExceptionBody } from './exceptions.controller';

// Joins an HrUser name from its parts.
function fullName(u?: { firstName?: string | null; middleName?: string | null; lastName?: string | null } | null): string {
  if (!u) return '';
  return [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');
}

// Rich include so list rows can be flattened to the frozen JSON keys.
const exceptionInclude = {
  hrUser: true,
  route: { include: { line: true } },
} as const;

@Injectable()
export class ExceptionsService {
  constructor(private readonly prisma: PrismaService) {}

  /** GET GetEmployeeException — paginated exceptions (TransportationExceptionData rows). */
  async getAll(pageNo = 1, noOfItems = 20, filters: { hrUserId?: number; id?: number } = {}) {
    const where: Record<string, unknown> = { active: true };
    if (filters.hrUserId) where.hrUserId = filters.hrUserId;
    if (filters.id) where.id = filters.id;

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRouteEmployeeException.count({ where }),
      this.prisma.transportationVehicleRouteEmployeeException.findMany({
        where,
        include: exceptionInclude,
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    // Resolve creator (User) names and direction names — neither has a direct
    // relation on the exception model, so batch-fetch both by id.
    const creatorMap = new Map<number, string>();
    const creatorIds = [...new Set(rows.map((e) => e.creationBy).filter((id): id is number => !!id))];
    if (creatorIds.length) {
      const users = await this.prisma.user.findMany({
        where: { id: { in: creatorIds } },
        select: { id: true, firstName: true, middleName: true, lastName: true, email: true },
      });
      for (const u of users) creatorMap.set(u.id, fullName(u) || u.email);
    }
    const directionMap = new Map<number, string>();
    const directionIds = [...new Set(rows.map((e) => e.directionId).filter((id): id is number => !!id))];
    if (directionIds.length) {
      const dirs = await this.prisma.transportationVehicleRouteDirection.findMany({
        where: { id: { in: directionIds } },
        select: { id: true, routeDirection: true },
      });
      for (const d of dirs) directionMap.set(d.id, d.routeDirection);
    }

    const data = rows.map((e) => ({
      Id: String(e.id),
      HrUserId: String(e.hrUserId),
      TransportationVehicleRouteId: e.routeId ?? 0,
      TransportationLineName: e.route?.line?.lineName ?? '',
      Active: e.active,
      CreationDate: e.creationDate ? e.creationDate.toISOString() : '',
      CreationBy: e.creationBy != null ? String(e.creationBy) : '',
      CreationName: e.creationBy != null ? (creatorMap.get(e.creationBy) ?? '') : '', // creator (User) full name
      TransportationVehicleRouteDirectionId: e.directionId != null ? String(e.directionId) : '',
      TransportationVehicleRouteDirectionName: e.directionId != null ? (directionMap.get(e.directionId) ?? '') : '', // direction/station name
      FromDate: e.fromDate ? e.fromDate.toISOString() : '',
      ToDate: e.toDate ? e.toDate.toISOString() : '',
      Period: e.period ?? '',
      DayName: e.dayName ?? '',
      LatitudeExceptional: 0, // TODO: no separate exceptional latitude field
      LongtitudExceptional: 0, // TODO: no separate exceptional longitude field (frozen typo key)
      ExceptionDate: e.exceptionDate ? e.exceptionDate.toISOString() : '',
      ExceptionDatePeriod: '', // TODO
      Latitude: e.latitude ?? 0,
      Longtitud: e.longitude ?? 0, // frozen typo key
      ExceptionDirectionId: '', // TODO: no separate exception direction field
      ExceptionDirectionName: '', // TODO
      HrUserName: fullName(e.hrUser),
      ReasonException: e.reasonException ?? '',
      ContactNumber: e.contactNumber ?? '',
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST AddException — create an exception. creationBy = caller. */
  async add(body: ExceptionBody, userId: number) {
    const routeId = Number(body?.transportationVehicleRouteId) || 0;
    const hrUserId = Number(body?.hrUserId) || 0;
    if (!routeId) return fail('Err101', 'transportationVehicleRouteId is required');
    if (!hrUserId) return fail('Err101', 'hrUserId is required');

    const created = await this.prisma.transportationVehicleRouteEmployeeException.create({
      data: this.toData(body, hrUserId, routeId, { creationBy: userId }),
    });
    return successWrite(created.id);
  }

  /** POST UpdateException — update an exception. */
  async update(id: number, body: ExceptionBody, userId: number) {
    if (!id) return fail('Err101', 'id is required');
    const routeId = Number(body?.transportationVehicleRouteId) || 0;
    const hrUserId = Number(body?.hrUserId) || 0;
    await this.prisma.transportationVehicleRouteEmployeeException.update({
      where: { id },
      data: this.toData(body, hrUserId, routeId, { creationBy: userId }),
    });
    return successWrite(id);
  }

  /**
   * GET GetCapacityNumbers — capacity breakdown for a route on a given day
   * (TransportationCapacityData). Faithful to the old GetCapacityNumbers
   * (svc:5642-5717): counts the route's scheduled riders for the day, then
   *   ActualCapacity = CapacityWithoutExpection + ExpectionNumFromOtherLines
   *                    - RouteEmployeesToOtherLines
   * where exceptions moving a rider off this route subtract, and exceptions
   * bringing a rider onto this route add.
   */
  async getCapacity(routeId: number, period?: string, dateSearch?: string) {
    if (!routeId) return fail('Err101', 'RouteID is required');
    const route = await this.prisma.transportationVehicleRoute.findUnique({
      where: { id: routeId },
      include: { vehicle: true },
    });
    if (!route) return fail('Err404', 'Route not found');

    // Evaluation day (old defaulted to "now" in Egypt time). Bound to the day.
    const target = dateSearch ? new Date(dateSearch) : new Date();
    const dayStart = new Date(Date.UTC(target.getUTCFullYear(), target.getUTCMonth(), target.getUTCDate()));
    const dayEnd = new Date(Date.UTC(target.getUTCFullYear(), target.getUTCMonth(), target.getUTCDate(), 23, 59, 59, 999));
    const dayName = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'][target.getUTCDay()];
    const periodValues = period ? [period, 'Both'] : ['Both'];

    // Riders scheduled on THIS route for the day: active membership, period
    // matches (or Both), and the day falls inside the membership window (when set).
    const members = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: {
        routeId,
        active: true,
        period: { in: periodValues },
        AND: [
          { OR: [{ fromDate: null }, { fromDate: { lte: dayEnd } }] },
          { OR: [{ toDate: null }, { toDate: { gte: dayStart } }] },
        ],
      },
      select: { hrUserId: true },
    });

    // Matching exceptions for that exact period on that day (old requires an
    // exact Period, not Both): either a date-range+weekday rule, or a one-off
    // ExceptionDate. Without a period we can't match, so none apply.
    const exceptions = period
      ? await this.prisma.transportationVehicleRouteEmployeeException.findMany({
          where: {
            active: true,
            OR: [
              {
                period,
                dayName,
                AND: [
                  { OR: [{ fromDate: null }, { fromDate: { lte: dayEnd } }] },
                  { OR: [{ toDate: null }, { toDate: { gte: dayStart } }] },
                ],
              },
              { period, exceptionDate: { gte: dayStart, lte: dayEnd } },
            ],
          },
          select: { hrUserId: true, routeId: true },
        })
      : [];

    const memberIds = new Set(members.map((m) => m.hrUserId));
    const exceptedUserIds = new Set(exceptions.map((e) => e.hrUserId));
    // This route's riders who have a matching exception (moved off the route).
    const routeEmployeesToOtherLines = [...memberIds].filter((id) => exceptedUserIds.has(id)).length;
    // Exceptions that target this route (riders brought onto the route).
    const expectionNumFromOtherLines = exceptions.filter((e) => e.routeId === routeId).length;
    const capacityWithoutException = members.length;

    const data = {
      FullCapacity: route.vehicle?.capacity ?? 0,
      ActualCapacity: capacityWithoutException + expectionNumFromOtherLines - routeEmployeesToOtherLines,
      CapacityWithoutExpection: capacityWithoutException, // frozen typo key
      ExpectionNumFromOtherLines: expectionNumFromOtherLines, // frozen typo key
      RouteEmployeesToOtherLines: routeEmployeesToOtherLines,
    };
    return success(data);
  }

  /** Maps the documented payload to the exception model columns. */
  private toData(body: ExceptionBody, hrUserId: number, routeId: number, extra: { creationBy: number }) {
    return {
      hrUserId,
      routeId,
      period: body.Period ?? '',
      fromDate: body.FromDate ? new Date(body.FromDate) : null,
      toDate: body.ToDate ? new Date(body.ToDate) : null,
      dayName: body.DayName ?? null,
      exceptionDate: body.ExceptionDate ? new Date(body.ExceptionDate) : null,
      latitude: body.Latitude != null ? Number(body.Latitude) : null,
      longitude: body.Longtitud != null ? Number(body.Longtitud) : null,
      directionId: body.TransportationVehicleRouteDirectionId
        ? Number(body.TransportationVehicleRouteDirectionId)
        : null,
      reasonException: body.reasonException ?? null,
      contactNumber: body.contactNumber ?? null,
      active: body.Active ?? true,
      creationBy: extra.creationBy,
    };
  }
}
