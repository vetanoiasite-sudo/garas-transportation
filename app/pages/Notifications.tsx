"use client";

import { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getNotifications, markRead, markAllRead, type NotificationItem } from "@/lib/services/notifications";
import { formatDateTime } from "@/lib/datetime";
import PageHeader from "@/components/ui/PageHeader";
import { LoadingState, EmptyState } from "@/components/ui/EmptyState";
import { IconBell, IconCheck } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// The approval screen each notification links to.
const ENTITY_PATH: Record<string, string> = {
  route: "/routes",
  line: "/lines",
  vehicle: "/vehicles",
  repricing: "/repricing",
};

export default function NotificationsPage() {
  const { t, locale } = useLocale();
  const { toast } = useToast();
  const navigate = useNavigate();
  const [rows, setRows] = useState<NotificationItem[]>([]);
  const [loading, setLoading] = useState(true);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setRows(await getNotifications());
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);

  useEffect(() => { load(); }, [load]);

  const unread = rows.filter((n) => !n.isRead).length;

  const open = async (n: NotificationItem) => {
    if (!n.isRead) {
      try { await markRead(n.id); } catch { /* non-blocking */ }
      setRows((rs) => rs.map((r) => (r.id === n.id ? { ...r, isRead: true } : r)));
    }
    const path = ENTITY_PATH[n.entityType];
    if (path) navigate(`/${locale}${path}`);
  };

  const onMarkAll = async () => {
    try {
      await markAllRead();
      setRows((rs) => rs.map((r) => ({ ...r, isRead: true })));
      toast(t("notif.allRead"));
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const fmt = (iso: string) => formatDateTime(iso);

  return (
    <div className="stack">
      <PageHeader title={t("nav.notifications")} count={rows.length}>
        {unread > 0 && (
          <button className="btn btn-outline-brand btn-sm" onClick={onMarkAll}>
            <IconCheck />{t("notif.markAllRead")}
          </button>
        )}
      </PageHeader>

      {loading ? (
        <LoadingState />
      ) : rows.length === 0 ? (
        <div className="card"><EmptyState message={t("notif.empty")} /></div>
      ) : (
        <div className="stack" style={{ gap: "var(--space-2)" }}>
          {rows.map((n) => (
            <button
              key={n.id}
              className="card"
              onClick={() => open(n)}
              style={{
                display: "flex", gap: "var(--space-3)", alignItems: "flex-start", textAlign: "start",
                width: "100%", cursor: "pointer",
                borderInlineStart: n.isRead ? undefined : "3px solid var(--color-brand)",
                background: n.isRead ? undefined : "var(--color-brand-softer)",
              }}
            >
              <span className="row" style={{ color: n.isRead ? "var(--text-muted)" : "var(--color-brand)", marginTop: 2 }}>
                <IconBell />
              </span>
              <div className="col" style={{ gap: 2, flex: 1 }}>
                <span style={{ fontWeight: 600, color: "var(--text-heading)" }}>
                  {n.title}
                  {!n.isRead && <span className="dot dot-green" style={{ marginInlineStart: 8 }} />}
                </span>
                {n.description && <span className="text-sm muted">{n.description}</span>}
                <span className="text-xs muted">{fmt(n.createdAt)}</span>
              </div>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
