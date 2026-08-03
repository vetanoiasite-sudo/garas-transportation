import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';

// The super-admin role id (مسؤول عام). Only super admins receive approval
// notifications, mirroring the old backend which fanned "Need Approval" notices
// out to every "Transportation Super Admin" user.
const SUPER_ADMIN_ROLE_ID = 216;

export interface NotifyOptions {
  title: string;
  description?: string;
  entityType?: string; // route | line | vehicle | repricing
  entityId?: number;
}

@Injectable()
export class NotificationsService {
  constructor(private readonly prisma: PrismaService) {}

  private superAdminIds(): Promise<{ userId: number }[]> {
    return this.prisma.userRole.findMany({
      where: { roleId: SUPER_ADMIN_ROLE_ID, active: true },
      select: { userId: true },
    });
  }

  private async isSuperAdmin(userId: number): Promise<boolean> {
    const r = await this.prisma.userRole.findFirst({ where: { userId, roleId: SUPER_ADMIN_ROLE_ID } });
    return !!r;
  }

  /**
   * Fan an approval notice out to every super admin. Skips the case where the
   * actor is themselves a super admin (their own actions don't need to notify
   * the approval group) — faithful to "the two admins' edits notify the super".
   */
  async notifySuperAdmins(actorUserId: number, opts: NotifyOptions): Promise<void> {
    if (await this.isSuperAdmin(actorUserId)) return;
    const supers = await this.superAdminIds();
    const recipientIds = [...new Set(supers.map((r) => r.userId))].filter((id) => id !== actorUserId);
    if (recipientIds.length === 0) return;
    await this.prisma.notification.createMany({
      data: recipientIds.map((userId) => ({
        userId,
        title: opts.title,
        description: opts.description ?? null,
        entityType: opts.entityType ?? null,
        entityId: opts.entityId ?? null,
        fromUserId: actorUserId,
      })),
    });
  }

  /** GET GetNotifications — the caller's notifications, newest first. */
  async list(userId: number, pageNo = 1, noOfItems = 20) {
    const where = { userId };
    const [total, rows] = await this.prisma.$transaction([
      this.prisma.notification.count({ where }),
      this.prisma.notification.findMany({
        where,
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);
    const data = rows.map((n) => ({
      Id: n.id,
      Title: n.title,
      Description: n.description ?? '',
      EntityType: n.entityType ?? '',
      EntityId: n.entityId ?? 0,
      FromUserId: n.fromUserId ?? 0,
      IsRead: n.isRead,
      CreatedAt: n.createdAt.toISOString(),
    }));
    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET GetUnreadNotificationsCount — number of unread notices for the caller. */
  async unreadCount(userId: number) {
    const count = await this.prisma.notification.count({ where: { userId, isRead: false } });
    return success(count);
  }

  /** POST MarkNotificationRead — mark one notice read (only the caller's own). */
  async markRead(userId: number, id: number) {
    await this.prisma.notification.updateMany({ where: { id, userId }, data: { isRead: true } });
    return successWrite(id);
  }

  /** POST MarkAllNotificationsRead — mark all the caller's notices read. */
  async markAllRead(userId: number) {
    const res = await this.prisma.notification.updateMany({ where: { userId, isRead: false }, data: { isRead: true } });
    return successWrite(undefined, `${res.count}`);
  }
}
