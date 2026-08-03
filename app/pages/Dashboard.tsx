"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { getDashboardNumbers, downloadAttendanceExcel, type DashboardNumbers } from "@/lib/services/attendance";
import { saveBlob } from "@/lib/download";
import { apiGet } from "@/lib/api/client";
import KPICard from "@/components/dashboard/KPICard";
import StatGroupCard from "@/components/dashboard/StatGroupCard";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import {
  IconBus, IconRoute, IconTruck, IconUsers, IconBuilding, IconCheck,
  IconTrend, IconUser, IconSwap, IconDownload, IconReceipt,
} from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);
const isoDay = (d: Date) => d.toISOString().slice(0, 10);

// Backend dropdown / rail row shapes (frozen PascalCase keys).
interface SupplierRow { Id: number; Name: string; contacts?: { Id: number; Name: string }[] }
interface RouteRow { Id: number; NameOfRoute: string; SupplierName?: string; Serial?: string; DirectionNum?: number }

const ZERO: DashboardNumbers = {
  linesNum: 0, vehiclesNum: 0, employeesNum: 0, allEmployeesNum: 0, suppliersNum: 0, allSuppliersNum: 0,
  vehicleTypeNum: 0, twoWayVehiclesNum: 0, oneWayVehiclesNum: 0,
  checkInVehicleNum: 0, checkOutVehicleNum: 0, oneWayVehicleNum: 0,
  checkInVehiclePercent: "0", checkOutVehiclePercent: "0", oneWayVehiclePercent: "0",
  employeesAttendanceNum: 0, employeesPercent: "0",
};

export default function DashboardPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const p = (path: string) => `/${locale}${path}`;

  const [nums, setNums] = useState<DashboardNumbers>(ZERO);
  const [routes, setRoutes] = useState<RouteRow[]>([]);
  const [supplierOpts, setSupplierOpts] = useState<SupplierRow[]>([]);

  const [activeRoute, setActiveRoute] = useState<string | null>(null);
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [serial, setSerial] = useState("");
  const [date, setDate] = useState(() => isoDay(new Date()));

  const driverOptions = useMemo(() => {
    const s = supplierOpts.find((x) => String(x.Id) === supplier);
    return (s?.contacts ?? []).map((c) => ({ value: String(c.Id), label: c.Name }));
  }, [supplier, supplierOpts]);

  // Load the route rail + supplier dropdown once.
  useEffect(() => {
    (async () => {
      try {
        const [rs, ss] = await Promise.all([
          apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 }),
          apiGet<SupplierRow[]>("getSuppliers", { PageNo: 1, NoOfItems: 500 }),
        ]);
        setRoutes(rs.Data ?? []);
        setSupplierOpts(ss.Data ?? []);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  // Load the dashboard numbers on mount + whenever the scope filters change.
  const load = useCallback(async () => {
    try {
      const n = await getDashboardNumbers({ date, supplierId: supplier, serial: serial || undefined, routeId: activeRoute ?? undefined });
      setNums(n);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  }, [date, supplier, serial, activeRoute, toast, t]);

  useEffect(() => { load(); }, [load]);

  const [downloading, setDownloading] = useState(false);
  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const blob = await downloadAttendanceExcel();
      saveBlob(blob, "attendance.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [toast, t]);

  const selectedRoute = routes.find((r) => String(r.Id) === activeRoute);
  const stationsSum = routes.reduce((a, r) => a + (r.DirectionNum ?? 0), 0);
  // Routes in the current scope: the backend's filtered two-way + one-way counts
  // (so the card narrows when a route/supplier/serial filter is applied). Falls
  // back to the full rail count before the first numbers load.
  const routesInScope = nums.twoWayVehiclesNum + nums.oneWayVehiclesNum || routes.length;
  const stationsInScope = selectedRoute ? (selectedRoute.DirectionNum ?? 0) : stationsSum;

  return (
    <div className="stack">
      <div className="row-between wrap">
        <h1 className="page-title">{t("dash.title")}</h1>
        <div className="header-actions">
          <Link to={p("/costs")} className="btn btn-secondary btn-sm"><IconReceipt />{t("dash.lineCostReport")}</Link>
          <Link to={p("/attendance")} className="btn btn-secondary btn-sm"><IconTrend />{t("dash.viewAttendance")}</Link>
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
            <button className={`rail-item${activeRoute === null ? " active" : ""}`} onClick={() => { setActiveRoute(null); setSerial(""); }}>
              <div className="name">{t("dash.allRoutes")}</div>
              <div className="meta">{routes.length} {t("nav.routes")}</div>
            </button>
            {routes.map((r) => (
              <button key={r.Id} className={`rail-item${activeRoute === String(r.Id) ? " active" : ""}`} onClick={() => { setActiveRoute(String(r.Id)); setSerial(r.Serial ?? ""); }}>
                <div className="name">{r.NameOfRoute}</div>
                <div className="meta">{r.SupplierName} · {r.Serial}</div>
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
                  options={supplierOpts.map((s) => ({ value: String(s.Id), label: s.Name }))}
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
                <Input
                  inputMode="numeric"
                  value={serial}
                  onChange={(e) => {
                    const v = e.target.value.replace(/\D/g, "");
                    setSerial(v);
                    // Typing a serial scopes the KPIs to the route with that serial.
                    const match = routes.find((r) => r.Serial === v);
                    setActiveRoute(match ? String(match.Id) : null);
                  }}
                  placeholder="1024"
                />
              </Field>
              <Field label={t("filter.date")} style={{ margin: 0, minWidth: 150 }}>
                <Input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
              </Field>
            </div>
            {selectedRoute && (
              <p className="field-hint mt-4">
                {t("board.routeKpis")} <b style={{ color: "var(--color-brand)" }}>{selectedRoute.NameOfRoute}</b>
              </p>
            )}
          </div>

          {/* KPI grid — NOTE: backend analytics partly stubbed (attendance figures are 0) */}
          <div className="kpi-grid">
            <KPICard icon={<IconBus />} value={nums.linesNum} label={t("nav.lines")} href={p("/lines")} />
            <KPICard icon={<IconRoute />} value={routesInScope} label={t("nav.routes")} href={p("/routes")} sub={`${stationsInScope} ${t("board.stationsWord")}`} />
            <KPICard icon={<IconTruck />} value={nums.vehiclesNum} label={t("nav.vehicles")} href={p("/vehicles")} sub={`${nums.vehicleTypeNum} ${t("board.activeWord")}`} tone="green" />
            <KPICard icon={<IconUsers />} value={nums.allEmployeesNum} label={t("nav.employees")} href={p("/passengers")} sub={`${nums.employeesNum} ${t("board.activeWord")}`} />
            <KPICard icon={<IconBuilding />} value={nums.allSuppliersNum} label={t("nav.suppliers")} href={p("/suppliers")} sub={`${nums.suppliersNum} ${t("board.enabledWord")}`} />
            <StatGroupCard
              icon={<IconCheck />}
              title={t("board.vehicleAttendance")}
              tone="green"
              stats={[
                { label: t("board.checkIn"), value: nums.checkInVehicleNum },
                { label: t("board.checkOut"), value: nums.checkOutVehicleNum },
                { label: t("board.oneWay"), value: nums.oneWayVehicleNum },
              ]}
            />
            <StatGroupCard
              icon={<IconTrend />}
              title={t("board.vehicleAttendancePct")}
              tone="amber"
              stats={[
                { label: t("board.checkIn"), value: `${nums.checkInVehiclePercent}%` },
                { label: t("board.checkOut"), value: `${nums.checkOutVehiclePercent}%` },
                { label: t("board.oneWay"), value: `${nums.oneWayVehiclePercent}%` },
              ]}
            />
            <KPICard icon={<IconUser />} value={nums.employeesAttendanceNum} label={t("board.userAttendance")} />
            <KPICard icon={<IconTrend />} value={`${nums.employeesPercent}%`} label={t("board.userAttendancePct")} tone="amber" />
            <StatGroupCard
              icon={<IconSwap />}
              title={t("board.routeTypes")}
              stats={[
                { label: t("board.roundTrip"), value: nums.twoWayVehiclesNum },
                { label: t("board.oneWay"), value: nums.oneWayVehiclesNum },
              ]}
            />
          </div>

          <div className="row-between">
            <span className="muted text-sm">{t("dash.viewAttendance")}</span>
            <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading}><IconDownload />{t("action.downloadExcel")}</button>
          </div>
        </div>
      </div>
    </div>
  );
}
