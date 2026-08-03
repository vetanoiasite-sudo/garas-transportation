import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, successList, successWrite } from '../../../common/response/base-response';
import { NotificationsService } from '../notifications/notifications.service';

@Injectable()
export class LinesService {
  constructor(
    private readonly prisma: PrismaService,
    private readonly notifications: NotificationsService,
  ) {}

  /** GET getAllTransportationLine — paginated lines ({ Id, Name, RouteNum }); optional name search. */
  async getAll(pageNo = 1, noOfItems = 20, name?: string) {
    const where: Record<string, unknown> = { active: true };
    if (name) where.lineName = { contains: name };
    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationLine.count({ where }),
      this.prisma.transportationLine.findMany({
        where,
        include: { _count: { select: { routes: { where: { active: true } } } } },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);
    const data = rows.map((l) => ({
      Id: l.id,
      Name: l.lineName,
      RouteNum: l._count.routes, // count of active child routes
      IsApproved: l.isApproved ?? false,
    }));
    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST AddTransportationLine — create a line. */
  async add(lineName: string, userId: number) {
    if (!lineName?.trim()) return fail('Err101', 'LineName is required');
    const created = await this.prisma.transportationLine.create({
      data: { lineName, creationBy: userId, modifiedBy: userId },
    });
    await this.notifications.notifySuperAdmins(userId, {
      title: 'إضافة محطة تجمع جديدة',
      description: 'بحاجة لاعتماد',
      entityType: 'line',
      entityId: created.id,
    });
    return successWrite(created.id);
  }

  /** POST UpdateTransportationLine — rename a line. */
  async update(id: number, lineName: string, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    if (!lineName?.trim()) return fail('Err101', 'LineName is required');
    await this.prisma.transportationLine.update({ where: { id }, data: { lineName, modifiedBy: userId } });
    return successWrite(id);
  }

  /** POST DeleteTransportationLine — delete (Id header). Blocks with a friendly
   *  message when the gathering station still has routes (FK guard). */
  async remove(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const routeCount = await this.prisma.transportationVehicleRoute.count({
      where: { transportationLineId: id },
    });
    if (routeCount > 0) {
      return fail(
        'Err102',
        'لا يمكن حذف محطة التجمع لارتباطها بخطوط. احذف الخطوط المرتبطة بها أولاً.',
      );
    }
    await this.prisma.transportationLine.delete({ where: { id } });
    return successWrite(id);
  }

  /** POST ApproveTransportationLine — approve/unapprove (Id + Approve headers). */
  async approve(id: number, approve: boolean, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationLine.update({
      where: { id },
      data: { isApproved: approve, approvedBy: userId },
    });
    return successWrite(id);
  }
}
