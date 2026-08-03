/* Notifications service — approval notices for super admins (routes/lines/
   vehicles created + repricing requested by the transportation/HR admins).
   Real calls to /api/Transportation. */
import { apiGet, apiPost } from "@/lib/api/client";

export interface NotificationItem {
  id: string;
  title: string;
  description: string;
  entityType: string; // route | line | vehicle | repricing
  entityId: number;
  isRead: boolean;
  createdAt: string;
}

interface NotificationRow {
  Id: number;
  Title: string;
  Description: string;
  EntityType: string;
  EntityId: number;
  IsRead: boolean;
  CreatedAt: string;
}

/** GET GetNotifications — the caller's approval notices, newest first. */
export async function getNotifications(pageNo = 1, noOfItems = 50): Promise<NotificationItem[]> {
  const res = await apiGet<NotificationRow[]>("GetNotifications", { PageNo: pageNo, NoOfItems: noOfItems });
  return (res.Data ?? []).map((n) => ({
    id: String(n.Id),
    title: n.Title,
    description: n.Description,
    entityType: n.EntityType,
    entityId: n.EntityId,
    isRead: n.IsRead,
    createdAt: n.CreatedAt,
  }));
}

/** GET GetUnreadNotificationsCount — badge count for the bell. */
export async function getUnreadCount(): Promise<number> {
  const res = await apiGet<number>("GetUnreadNotificationsCount");
  return Number(res.Data ?? 0);
}

/** POST MarkNotificationRead — mark a single notice read. */
export async function markRead(id: string): Promise<void> {
  await apiPost("MarkNotificationRead", {}, { Id: id });
}

/** POST MarkAllNotificationsRead — mark every notice read. */
export async function markAllRead(): Promise<void> {
  await apiPost("MarkAllNotificationsRead", {});
}
