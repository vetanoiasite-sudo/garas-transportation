/* Notifications service — approval notices (routes/lines/vehicles created +
   repricing requested).

   These live on the CoreApi's own /Notification controller, not under
   /api/Transportation. The API has no unread-count or mark-all endpoints: a
   notice is unread while `New` is true, and `EditNotifications` writes one row
   back — so the count is derived and mark-all fans out over the unread rows. */
import { apiRaw, type Envelope } from "@/lib/api/client";

export interface NotificationItem {
  id: string;
  title: string;
  description: string;
  entityType: string; // route | line | vehicle | repricing
  entityId: number;
  isRead: boolean;
  createdAt: string;
}

/** UserNotification — the CoreApi row (frozen PascalCase; `New` = unread). */
interface NotificationRow {
  ID?: number | string;
  Title?: string;
  Description?: string;
  URL?: string;
  NotificationProcessID?: number;
  New?: boolean;
  Date?: string;
  FromUserID?: number;
  ToUserID?: number;
}

interface NotificationsResponse { UserNotificationsList?: NotificationRow[] }

const toItem = (n: NotificationRow): NotificationItem => ({
  id: String(n.ID ?? ""),
  title: n.Title ?? "",
  description: n.Description ?? "",
  // The CoreApi has no entity-type column; the deep link rides in URL.
  entityType: n.URL ?? "",
  entityId: Number(n.NotificationProcessID ?? 0),
  isRead: !n.New,
  createdAt: n.Date ?? "",
});

/** GET /Notification/GetNotifications — the caller's notices, newest first. */
export async function getNotifications(pageNo = 1, noOfItems = 50): Promise<NotificationItem[]> {
  const res = (await apiRaw<unknown>("GET", "/Notification/GetNotifications", {
    headers: { CurrentPage: pageNo, NumberOfItemsPerPage: noOfItems },
  })) as Envelope<unknown> & NotificationsResponse;
  return (res.UserNotificationsList ?? []).map(toItem);
}

/** Unread badge count — derived, since the API exposes no count endpoint. */
export async function getUnreadCount(): Promise<number> {
  const items = await getNotifications(1, 200);
  return items.filter((n) => !n.isRead).length;
}

/** POST /Notification/EditNotifications — flip one notice to read (New: false). */
export async function markRead(id: string): Promise<void> {
  await apiRaw("POST", "/Notification/EditNotifications", { body: { ID: Number(id), New: false } });
}

/** Mark every unread notice read — one EditNotifications call per row. */
export async function markAllRead(): Promise<void> {
  const unread = (await getNotifications(1, 200)).filter((n) => !n.isRead);
  await Promise.all(unread.map((n) => markRead(n.id)));
}
