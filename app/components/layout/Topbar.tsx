"use client";

import { useRouter } from "next/navigation";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { type Locale } from "@/lib/i18n";
import { roleLabelKey } from "@/lib/types";
import RoleSwitcher from "./RoleSwitcher";
import { IconLogout, IconBuilding, IconMenu } from "@/components/ui/Icons";

export default function Topbar({ onMenu }: { onMenu?: () => void }) {
  const { t, locale, switchLocale } = useLocale();
  const { user, logout } = useAuth();
  const router = useRouter();

  const initials = user?.name?.trim().charAt(0) ?? "؟";

  return (
    <header className="topbar">
      <div className="row gap-3">
        <button className="icon-btn menu-btn" aria-label={t("action.menu")} onClick={onMenu}>
          <IconMenu />
        </button>
        <span className="info-chip">
          <IconBuilding style={{ width: 14, height: 14 }} />
          {user?.branch}
        </span>
      </div>

      <div className="row gap-3 wrap">
        <RoleSwitcher />

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
            router.push(`/${locale}/login`);
          }}
        >
          <IconLogout />
        </button>
      </div>
    </header>
  );
}
