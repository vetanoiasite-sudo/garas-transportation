"use client";

import { useState, useMemo } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { can } from "@/lib/types";
import { routes, suppliers, lines, passengers, vehicles } from "@/lib/data";
import KPICard from "@/components/dashboard/KPICard";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import {
  IconBus, IconRoute, IconTruck, IconUsers, IconBuilding, IconCheck,
  IconTrend, IconUser, IconSwap, IconDownload, IconReceipt,
} from "@/components/ui/Icons";

export default function DashboardPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const p = (path: string) => `/${locale}${path}`;

  const [activeRoute, setActiveRoute] = useState<string | null>(null);
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [serial, setSerial] = useState("");

  const driverOptions = useMemo(() => {
    const s = suppliers.find((x) => x.id === supplier);
    return (s?.contacts ?? []).map((c) => ({ value: c.id, label: c.name }));
  }, [supplier]);

  const selectedRoute = routes.find((r) => r.id === activeRoute);
  const ridersTotal = passengers.length;
  const activeVehicles = vehicles.filter((v) => v.approved && v.active).length;
  const attendedPct = ((routes.reduce((a, r) => a + r.attended, 0) / routes.reduce((a, r) => a + r.usersInRoute, 0)) * 100).toFixed(1);

  return (
    <div className="stack">
      <div className="row-between wrap">
        <h1 className="page-title">{t("dash.title")}</h1>
        <div className="header-actions">
          <Link href={p("/account-statement")} className="btn btn-secondary btn-sm"><IconReceipt />{t("dash.lineCostReport")}</Link>
          <Link href={p("/attendance")} className="btn btn-secondary btn-sm"><IconTrend />{t("dash.viewAttendance")}</Link>
          {can(user?.role, "touch.attendance") && (
            <button className="btn btn-brand btn-sm"><IconCheck />{t("dash.touchAttendance")}</button>
          )}
        </div>
      </div>

      <div className="row kpi-dash" style={{ alignItems: "flex-start", gap: "var(--space-4)" }}>
        {/* Route rail */}
        <div className="rail">
          <div className="rail-head">{t("dash.routeRail")}</div>
          <div className="rail-list">
            <button className={`rail-item${activeRoute === null ? " active" : ""}`} onClick={() => setActiveRoute(null)}>
              <div className="name">{t("dash.allRoutes")}</div>
              <div className="meta">{routes.length} {t("nav.lines")}</div>
            </button>
            {routes.map((r) => (
              <button key={r.id} className={`rail-item${activeRoute === r.id ? " active" : ""}`} onClick={() => setActiveRoute(r.id)}>
                <div className="name">{r.name}</div>
                <div className="meta">{r.supplier} · {r.serial}</div>
              </button>
            ))}
          </div>
        </div>

        {/* Main */}
        <div className="grow stack">
          {/* Cascading filter bar */}
          <div className="card card-pad">
            <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
              <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
                <Combobox
                  options={suppliers.map((s) => ({ value: s.id, label: s.name }))}
                  value={supplier}
                  onChange={(v) => { setSupplier(v); setDriver(undefined); }}
                  placeholder={t("filter.supplier")}
                />
              </Field>
              <Field label={t("filter.driver")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
                <Combobox
                  options={driverOptions}
                  value={driver}
                  onChange={setDriver}
                  placeholder={t("filter.driver")}
                  disabled={!supplier}
                  disabledReason={t("filter.selectSupplierFirst")}
                />
              </Field>
              <Field label={t("filter.serial")} style={{ margin: 0, minWidth: 140 }}>
                <Input inputMode="numeric" value={serial} onChange={(e) => setSerial(e.target.value.replace(/\D/g, ""))} placeholder="1024" />
              </Field>
              <Field label={t("filter.date")} style={{ margin: 0, minWidth: 150 }}>
                <Input type="date" defaultValue="2026-07-09" />
              </Field>
            </div>
            {selectedRoute && (
              <p className="field-hint mt-4">
                عرض مؤشرات المسار: <b style={{ color: "var(--color-brand)" }}>{selectedRoute.name}</b>
              </p>
            )}
          </div>

          {/* KPI grid */}
          <div className="kpi-grid">
            <KPICard icon={<IconBus />} value={lines.length} label={t("nav.lines")} href={p("/lines")} />
            <KPICard icon={<IconRoute />} value={routes.length} label={t("filter.route")} href={p("/lines")} sub={`${routes.reduce((a, r) => a + r.stationCount, 0)} محطة`} />
            <KPICard icon={<IconTruck />} value={vehicles.length} label={t("nav.vehicles")} href={p("/vehicles")} sub={`${activeVehicles} نشط`} tone="green" />
            <KPICard icon={<IconUsers />} value={ridersTotal} label={t("nav.employees")} href={p("/passengers")} sub={`${passengers.filter((x) => x.active).length} نشط`} />
            <KPICard icon={<IconBuilding />} value={suppliers.length} label={t("nav.suppliers")} href={p("/suppliers")} sub={`${suppliers.filter((s) => s.activeRoutes > 0).length} فعّال`} />
            <KPICard icon={<IconCheck />} value={selectedRoute ? selectedRoute.attended : routes.reduce((a, r) => a + r.attended, 0)} label="حضور المركبات" tone="green" sub="تسجيل دخول / خروج" />
            <KPICard icon={<IconTrend />} value={`${attendedPct}%`} label="نسبة حضور المركبات" tone="amber" />
            <KPICard icon={<IconUser />} value={selectedRoute ? selectedRoute.usersInRoute : passengers.length} label="حضور المستخدمين" />
            <KPICard icon={<IconTrend />} value={`${attendedPct}%`} label="نسبة حضور المستخدمين" tone="amber" />
            <KPICard icon={<IconSwap />} value={`${routes.filter((r) => !r.oneWay).length}/${routes.filter((r) => r.oneWay).length}`} label="ذهاب وعودة / اتجاه واحد" />
          </div>

          <div className="row-between">
            <span className="muted text-sm">{t("dash.viewAttendance")}</span>
            <button className="btn btn-secondary btn-sm"><IconDownload />{t("action.downloadExcel")}</button>
          </div>
        </div>
      </div>
    </div>
  );
}
