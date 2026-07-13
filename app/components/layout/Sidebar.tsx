"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { canAdd } from "@/lib/types";
import {
  IconDashboard, IconBus, IconTruck, IconUsers, IconBuilding, IconReceipt,
  IconMoney, IconTag, IconSwap, IconSettings, IconClock, IconTrend, IconChevronDown, IconX,
} from "@/components/ui/Icons";

interface NavLink {
  href: string;
  labelKey: string;
  icon: React.ComponentType<{ style?: React.CSSProperties }>;
  adminOnly?: boolean;
}

export default function Sidebar({ open = false, onClose }: { open?: boolean; onClose?: () => void }) {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const pathname = usePathname();
  const [openGroup, setOpenGroup] = useState(true);
  const admin = canAdd(user?.role);

  const p = (path: string) => `/${locale}${path}`;
  const isActive = (path: string) => pathname === p(path) || pathname.startsWith(p(path) + "/");

  const main: NavLink[] = [
    { href: "/dashboard", labelKey: "nav.dashboard", icon: IconDashboard },
    { href: "/attendance", labelKey: "nav.attendance", icon: IconTrend },
    { href: "/lines", labelKey: "nav.lines", icon: IconBus },
    { href: "/vehicles", labelKey: "nav.vehicles", icon: IconTruck },
    { href: "/passengers", labelKey: "nav.employees", icon: IconUsers, adminOnly: true },
    { href: "/suppliers", labelKey: "nav.suppliers", icon: IconBuilding },
    { href: "/account-statement", labelKey: "nav.statement", icon: IconReceipt },
    { href: "/deductions", labelKey: "nav.deductions", icon: IconMoney },
    { href: "/repricing", labelKey: "nav.repricing", icon: IconTag },
    { href: "/exceptions", labelKey: "nav.exceptions", icon: IconSwap },
  ];

  return (
    <aside className={`sidebar${open ? " open" : ""}`} aria-label={t("appName")}>
      <div className="sidebar-brand">
        <span className="logo">
          <IconBus style={{ width: 22, height: 22, color: "var(--color-navy)" }} />
        </span>
        <div className="col">
          <b>{t("appName")}</b>
          <span style={{ fontSize: "var(--text-xs)", color: "var(--nav-muted)" }}>{t("appTagline")}</span>
        </div>
        <button className="icon-btn sidebar-close" style={{ color: "#cdd8ea" }} aria-label={t("action.close")} onClick={onClose}>
          <IconX />
        </button>
      </div>

      <nav className="sidebar-nav">
        {main
          .filter((l) => !l.adminOnly || admin)
          .map((l) => {
            const Icon = l.icon;
            return (
              <Link key={l.href} href={p(l.href)} className={`nav-item${isActive(l.href) ? " active" : ""}`} aria-current={isActive(l.href) ? "page" : undefined} onClick={onClose}>
                <Icon />
                <span>{t(l.labelKey)}</span>
              </Link>
            );
          })}

        {admin && (
          <>
            <button
              className="nav-item"
              style={{ justifyContent: "space-between", background: "transparent", width: "100%", marginTop: "var(--space-2)" }}
              onClick={() => setOpenGroup((v) => !v)}
              aria-expanded={openGroup}
            >
              <span className="row" style={{ gap: "var(--space-3)" }}>
                <IconSettings />
                <span>{t("nav.transportation")}</span>
              </span>
              <IconChevronDown style={{ width: 16, height: 16, transform: openGroup ? "rotate(180deg)" : "none", transition: "transform .15s" }} />
            </button>
            {openGroup && (
              <div className="nav-sub">
                <Link href={p("/shifts")} className={`nav-item${isActive("/shifts") ? " active" : ""}`} aria-current={isActive("/shifts") ? "page" : undefined} onClick={onClose}>
                  <IconClock />
                  <span>{t("nav.shifts")}</span>
                </Link>
              </div>
            )}
          </>
        )}
      </nav>
    </aside>
  );
}
