"use client";

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { type Locale } from "@/lib/i18n";
import { roleLabelKey, can } from "@/lib/types";
import { getUnreadCount } from "@/lib/services/notifications";
import { IconLogout, IconMenu, IconBell } from "@/components/ui/Icons";

export default function Topbar({ onMenu }: { onMenu?: () => void }) {
  const { t, locale, switchLocale } = useLocale();
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const initials = user?.name?.trim().charAt(0) ?? "؟";
  // Only the approval group (super admin, via manage.users) receives notices.
  const showBell = can(user?.role, "manage.users");
  const [unread, setUnread] = useState(0);

  useEffect(() => {
    if (!showBell) return;
    let alive = true;
    const tick = () => getUnreadCount().then((n) => { if (alive) setUnread(n); }).catch(() => {});
    tick();
    const id = setInterval(tick, 60000); // keep the badge fresh
    return () => { alive = false; clearInterval(id); };
  }, [showBell]);

  return (
    <header className="topbar">
      <div className="row gap-3">
        <button className="icon-btn menu-btn" aria-label={t("action.menu")} onClick={onMenu}>
          <IconMenu />
        </button>
      </div>

      <div className="row gap-3 wrap">
        {showBell && (
          <button
            className="icon-btn"
            aria-label={t("nav.notifications")}
            onClick={() => navigate(`/${locale}/notifications`)}
            style={{ position: "relative" }}
          >
            <IconBell />
            {unread > 0 && (
              <span
                aria-hidden
                style={{
                  position: "absolute", top: -2, insetInlineEnd: -2, minWidth: 16, height: 16,
                  padding: "0 4px", borderRadius: 999, background: "var(--red-500, #ef4444)",
                  color: "#fff", fontSize: 10, fontWeight: 700, lineHeight: "16px", textAlign: "center",
                }}
              >
                {unread > 99 ? "99+" : unread}
              </span>
            )}
          </button>
        )}

        <div className="lang-switch">
          {(["ar", "en"] as Locale[]).map((l) => (
            <button key={l} className={locale === l ? "active" : ""} onClick={() => switchLocale(l)}>
              {l === "ar" ? "عربي" : "EN"}
            </button>
          ))}
        </div>

        <div className="row" style={{ gap: "var(--space-2)" }}>
          <div className="avatar">{initials}</div>
          <div className="col" style={{ lineHeight: 1.2 }}>
            <span style={{ fontWeight: 600, color: "var(--text-heading)", fontSize: "var(--text-sm)" }}>{user?.name}</span>
            <span className="badge badge-blue" style={{ padding: "0 6px" }}>{user ? t(roleLabelKey[user.role]) : ""}</span>
          </div>
        </div>

        <button
          className="icon-btn"
          aria-label={t("action.logout")}
          onClick={() => {
            logout();
            navigate(`/${locale}/login`);
          }}
        >
          <IconLogout />
        </button>
      </div>
    </header>
  );
}
