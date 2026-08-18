"use client";

import { Link } from "react-router-dom";
import { useLocation } from "react-router-dom";
import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { canAdd, can, type Permission } from "@/lib/types";
import {
  IconDashboard, IconBus, IconRoute, IconTruck, IconUsers, IconUser, IconBuilding, IconReceipt,
  IconMoney, IconTag, IconSwap, IconSettings, IconTrend, IconChevronDown, IconX, IconBell,
} from "@/components/ui/Icons";

interface NavLink {
  href: string;
  labelKey: string;
  icon: React.ComponentType<{ style?: React.CSSProperties }>;
  adminOnly?: boolean;
  perm?: Permission; // when set, the item shows only if the role has this permission
}

export default function Sidebar({ open = false, onClose }: { open?: boolean; onClose?: () => void }) {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const pathname = useLocation().pathname;
  const [openGroup, setOpenGroup] = useState(true);
  const admin = canAdd(user?.role);
  const canUsers = can(user?.role, "manage.users");

  const p = (path: string) => `/${locale}${path}`;
  const isActive = (path: string) => pathname === p(path) || pathname.startsWith(p(path) + "/");

  const main: NavLink[] = [
    { href: "/dashboard", labelKey: "nav.dashboard", icon: IconDashboard },
    { href: "/attendance", labelKey: "nav.attendance", icon: IconTrend },
    { href: "/bus-attendance", labelKey: "busAtt.title", icon: IconBus },
    { href: "/lines", labelKey: "nav.lines", icon: IconBus },
    { href: "/routes", labelKey: "nav.routes", icon: IconRoute },
    { href: "/vehicles", labelKey: "nav.vehicles", icon: IconTruck },
    { href: "/passengers", labelKey: "nav.employees", icon: IconUsers, perm: "manage.passengers" },
    { href: "/suppliers", labelKey: "nav.suppliers", icon: IconBuilding },
    { href: "/account-statement", labelKey: "nav.statement", icon: IconReceipt, perm: "view.financialReports" },
    { href: "/deductions", labelKey: "nav.deductions", icon: IconMoney },
    { href: "/repricing", labelKey: "nav.repricing", icon: IconTag },
    { href: "/exceptions", labelKey: "nav.exceptions", icon: IconSwap },
    { href: "/notifications", labelKey: "nav.notifications", icon: IconBell, perm: "manage.users" },
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
          .filter((l) => (l.perm ? can(user?.role, l.perm) : !l.adminOnly || admin))
          .map((l) => {
            const Icon = l.icon;
            return (
              <Link key={l.href} to={p(l.href)} className={`nav-item${isActive(l.href) ? " active" : ""}`} aria-current={isActive(l.href) ? "page" : undefined} onClick={onClose}>
                <Icon />
                <span>{t(l.labelKey)}</span>
              </Link>
            );
          })}

        {canUsers && (
          <Link to={p("/users")} className={`nav-item${isActive("/users") ? " active" : ""}`} aria-current={isActive("/users") ? "page" : undefined} onClick={onClose}>
            <IconUser />
            <span>{t("nav.users")}</span>
          </Link>
        )}

        {admin && canUsers && (
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
                <Link to={p("/users")} className={`nav-item${isActive("/users") ? " active" : ""}`} aria-current={isActive("/users") ? "page" : undefined} onClick={onClose}>
                  <IconUsers />
                  <span>{t("nav.supervisorAdmin")}</span>
                </Link>
              </div>
            )}
          </>
        )}
      </nav>
    </aside>
  );
}
