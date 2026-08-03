import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, successList, successWrite } from '../../../common/response/base-response';
import { RepriceBody } from './repricing.controller';
import { NotificationsService } from '../notifications/notifications.service';

// Computes the new price for a route line: percentage bump multiplies, otherwise
// adds a flat cost; optionally snapped to the nearest 5 (ApproximateToFiveFlag).
function computePriceAfter(before: number, increaseCost: number, isPercent: boolean, approximateToFive: boolean): number {
  const after = isPercent ? before * (1 + increaseCost / 100) : before + increaseCost;
  return approximateToFive ? Math.round(after / 5) * 5 : after;
}

@Injectable()
export class RepricingService {
  constructor(
    private readonly prisma: PrismaService,
    private readonly notifications: NotificationsService,
  ) {}

  /** POST ModifyPriceOfTransportationLine — create a pending increase request + one line per route. */
  async modify(body: RepriceBody, userId: number) {
    const increaseCost = Number(body?.IncreaseCost) || 0;
    const isPercent = body?.IsPercent ?? false;
    const forAllLines = body?.ForAllLines ?? false;
    const approximateToFive = body?.ApproximateToFiveFlag ?? false;
    const startDate = body?.StartDate ? new Date(body.StartDate) : new Date();

    // Route ids: explicit list, or every active route when ForAllLines is set.
    let routeIds = Array.isArray(body?.TransportationLineIds)
      ? body!.TransportationLineIds!.map((n) => Number(n)).filter((n) => Number.isFinite(n) && n > 0)
      : [];
    if (forAllLines) {
      const all = await this.prisma.transportationVehicleRoute.findMany({
        where: { active: true },
        select: { id: true },
      });
      routeIds = all.map((r) => r.id);
    }
    if (routeIds.length === 0) return fail('Err101', 'TransportationLineIds is required');

    const routes = await this.prisma.transportationVehicleRoute.findMany({
      where: { id: { in: routeIds } },
      select: { id: true, lineCost: true },
    });

    const created = await this.prisma.transportationLineIncreaseRequest.create({
      data: {
        isPercent,
        increaseCost,
        forAllLines,
        approximateToFiveFlag: approximateToFive,
        startDate,
        approve: null,
        creationBy: userId,
        lines: {
          create: routes.map((r) => ({
            routeId: r.id,
            priceBefore: r.lineCost,
            priceAfter: computePriceAfter(r.lineCost, increaseCost, isPercent, approximateToFive),
          })),
        },
      },
    });

    await this.notifications.notifySuperAdmins(userId, {
      title: 'طلب تغيير أسعار الخطوط',
      description: 'بحاجة لاعتماد',
      entityType: 'repricing',
      entityId: created.id,
    });
    return successWrite(created.id);
  }

  /** Resolve a set of User ids → their full names, in one query. */
  private async userNames(ids: (number | null | undefined)[]): Promise<Map<number, string>> {
    const unique = [...new Set(ids.filter((id): id is number => !!id))];
    const map = new Map<number, string>();
    if (unique.length === 0) return map;
    const users = await this.prisma.user.findMany({
      where: { id: { in: unique } },
      select: { id: true, firstName: true, middleName: true, lastName: true, email: true },
    });
    for (const u of users) {
      map.set(u.id, [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ') || u.email);
    }
    return map;
  }

  /** GET getAllModifyPriceOfTransportationLine — paginated ModifiedPriceData with per-route details. */
  async getAll(pageNo = 1, noOfItems = 20, approve?: boolean) {
    const where: Record<string, unknown> = {};
    // Pending requests are stored as approve=null. Treat approve=false as
    // "not yet approved" (pending null OR rejected false) so pending requests
    // aren't hidden — faithful to the old semantics where pending == false.
    if (approve === true) where.approve = true;
    else if (approve === false) where.approve = { not: true };

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationLineIncreaseRequest.count({ where }),
      this.prisma.transportationLineIncreaseRequest.findMany({
        where,
        include: { lines: { include: { route: true } } },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const names = await this.userNames(rows.flatMap((r) => [r.creationBy, r.approvedBy]));

    const data = rows.map((r) => ({
      Id: r.id,
      IsPercent: r.isPercent,
      ApproximateToFiveFlag: r.approximateToFiveFlag,
      IncreaseCost: r.increaseCost,
      Approve: r.approve,
      ApprovedBy: r.approvedBy ?? null,
      ApprovedByName: names.get(r.approvedBy ?? -1) ?? '', // approver (User) full name
      ApprovedDate: r.approvedDate ? r.approvedDate.toISOString() : null,
      ForAllLines: r.forAllLines,
      CreationDate: r.creationDate.toISOString(),
      CreationBy: r.creationBy ?? 0,
      CreationName: names.get(r.creationBy ?? -1) ?? '', // creator (User) full name
      transportationLineDetails: r.lines.map((l) => ({
        RouteId: l.routeId,
        RouteName: l.route?.nameOfRoute ?? '',
        PriceBefore: l.priceBefore,
        PriceAfter: l.priceAfter,
      })),
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST UpdatePriceOfTransportationLine — approve: stamp request, apply each route's new lineCost, log exception price. */
  async approvePrice(id: number, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    const request = await this.prisma.transportationLineIncreaseRequest.findUnique({
      where: { id },
      include: { lines: true },
    });
    if (!request) return fail('Err404', 'Repricing request not found');

    await this.prisma.$transaction([
      this.prisma.transportationLineIncreaseRequest.update({
        where: { id },
        data: { approve: true, approvedBy: userId, approvedDate: new Date() },
      }),
      ...request.lines.map((l) =>
        this.prisma.transportationVehicleRoute.update({
          where: { id: l.routeId },
          data: { lineCost: l.priceAfter },
        }),
      ),
      ...request.lines.map((l) =>
        this.prisma.transportionLineExceptionPrice.create({
          data: { routeId: l.routeId, price: l.priceAfter, fromDate: request.startDate },
        }),
      ),
    ]);

    return successWrite(id);
  }

  /** POST RejectUpdatePrice — reject the pending increase request. */
  async rejectPrice(id: number, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationLineIncreaseRequest.update({
      where: { id },
      data: { approve: false, approvedBy: userId },
    });
    return successWrite(id);
  }
}
