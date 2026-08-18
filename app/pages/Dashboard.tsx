"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import type { AttendanceRecord } from "@/lib/types";
import { getDashboardNumbers, getAttendance, downloadAttendanceExcel, type DashboardNumbers } from "@/lib/services/attendance";
import { openFileUrl } from "@/lib/download";
import { formatDateTime } from "@/lib/datetime";
import { apiGet } from "@/lib/api/client";
import { useSupplierOptions, useDriverOptions } from "@/lib/hooks/useSuppliers";
import KPICard from "@/components/dashboard/KPICard";
import StatGroupCard from "@/components/dashboard/StatGroupCard";
import Combobox from "@/components/ui/Combobox";
import Dialog from "@/components/ui/Dialog";
import DataTable, { type Column } from "@/components/ui/DataTable";
import { Field, Input } from "@/components/ui/Field";
import {
  IconBus, IconRoute, IconTruck, IconUsers, IconBuilding, IconCheck,
  IconTrend, IconUser, IconSwap, IconDownload, IconReceipt,
} from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);
const isoDay = (d: Date) => d.toISOString().slice(0, 10);

// Backend dropdown / rail row shapes (frozen PascalCase keys).
interface RouteRow { Id: number; NameOfRoute: string; SupplierName?: string; Serial?: string; DirectionNum?: number }

const ZERO: DashboardNumbers = {
  linesNum: 0, vehiclesNum: 0, employeesNum: 0, allEmployeesNum: 0, suppliersNum: 0, allSuppliersNum: 0,
  vehicleTypeNum: 0, twoWayVehiclesNum: 0, oneWayVehiclesNum: 0,
  checkInVehicleNum: 0, checkOutVehicleNum: 0, oneWayVehicleNum: 0,
  checkInVehiclePercent: "0", checkOutVehiclePercent: "0", oneWayVehiclePercent: "0",
  employeesAttendanceNum: 0, employeesPercent: "0",
  fullCapacity: 0, employeesGoNum: 0, employeesReturnNum: 0,
  capacityPercent: "0", oneWayGoPercent: "0", oneWayReturnPercent: "0",
};

export default function DashboardPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const p = (path: string) => `/${locale}${path}`;

  const [nums, setNums] = useState<DashboardNumbers>(ZERO);
  const [routes, setRoutes] = useState<RouteRow[]>([]);

  const [activeRoute, setActiveRoute] = useState<string | null>(null);
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [serial, setSerial] = useState("");
  const [date, setDate] = useState(() => isoDay(new Date()));

  const onLoadError = useCallback((e: unknown) => toast(errMsg(e, t("empty.generic")), "error"), [toast, t]);
  const supplierOpts = useSupplierOptions(onLoadError);
  const driverOptions = useDriverOptions(supplier, onLoadError);

  // Load the route rail once.
  useEffect(() => {
    (async () => {
      try {
        const rs = await apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 });
        setRoutes(rs.Data ?? []);
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

  // User-attendance popup: same attendance API, scoped to the ONE dashboard day.
  const [usersOpen, setUsersOpen] = useState(false);
  const [attRows, setAttRows] = useState<AttendanceRecord[]>([]);
  const [attLoading, setAttLoading] = useState(false);
  useEffect(() => {
    if (!usersOpen) return;
    (async () => {
      setAttLoading(true);
      try {
        const res = await getAttendance({
          from: date,
          to: date,
          supplierId: supplier,
          serial: serial || undefined,
          routeId: activeRoute ?? undefined,
          noOfItems: 500,
        });
        setAttRows(res.items);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      } finally {
        setAttLoading(false);
      }
    })();
  }, [usersOpen, date, supplier, serial, activeRoute, toast, t]);

  const [downloading, setDownloading] = useState(false);
  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const url = await downloadAttendanceExcel();
      openFileUrl(url, "attendance.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [toast, t]);

  const selectedRoute = routes.find((r) => String(r.Id) === activeRoute);
  const stationsSum = routes.reduce((a, r) => a + (r.DirectionNum ?? 0), 0);

  // Driver is a client-side refinement (the attendance API has no driver filter).
  const driverName = driverOptions.find((d) => d.value === driver)?.label;
  const dialogRows = attRows.filter((r) => !driverName || r.driver === driverName);

  const attColumns: Column<AttendanceRecord>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: t("common.name"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.name}</b> },
    { key: "idCode", header: t("att.identityNumber") },
    { key: "route", header: t("filter.route") },
    { key: "serial", header: t("filter.serial"), priority: "secondary" },
    { key: "supplier", header: t("filter.supplier"), priority: "secondary" },
    { key: "driver", header: t("filter.driver"), priority: "secondary" },
    {
      key: "checkIn",
      header: t("att.checkIn"),
      render: (r) => <span style={{ color: r.attended ? "var(--text-heading)" : "var(--color-danger)" }}>{r.checkIn ? formatDateTime(r.checkIn) : (r.attended ? "—" : t("status.absent"))}</span>,
    },
    { key: "checkOut", header: t("att.checkOut"), render: (r) => <span>{r.checkOut ? formatDateTime(r.checkOut) : "—"}</span> },
  ];

  // Hand the active dashboard scope to the passengers screen via query params.
  const passengerParams = new URLSearchParams();
  if (supplier) passengerParams.set("supplierId", supplier);
  if (driver) passengerParams.set("driverId", driver);
  if (activeRoute) passengerParams.set("routeId", activeRoute);
  else if (serial) passengerParams.set("serial", serial);
  const passengerQs = passengerParams.toString();
  const passengersHref = p("/passengers") + (passengerQs ? `?${passengerQs}` : "");
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
          {can(user?.role, "view.financialReports") && (
            <Link to={p("/costs")} className="btn btn-secondary btn-sm"><IconReceipt />{t("dash.lineCostReport")}</Link>
          )}
          <Link to={p("/attendance")} className="btn btn-secondary btn-sm"><IconTrend />{t("dash.viewAttendance")}</Link>
          <Link to={p("/bus-attendance")} className="btn btn-secondary btn-sm"><IconBus />{t("dash.busAttendance")}</Link>
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
              <Field label={t("filter.route")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
                <Combobox
                  options={routes.map((r) => ({ value: String(r.Id), label: r.NameOfRoute }))}
                  value={activeRoute ?? undefined}
                  onChange={(v) => {
                    // Two-way link with the serial field: picking a route fills its serial.
                    setActiveRoute(v ?? null);
                    const match = routes.find((r) => String(r.Id) === v);
                    setSerial(match?.Serial ?? "");
                  }}
                  placeholder={t("filter.route")}
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
            <KPICard icon={<IconTruck />} value={nums.vehiclesNum} label={t("nav.vehicles")} href={p("/vehicles")} sub={`${t("board.totalCapacity")}: ${nums.fullCapacity}`} tone="green" />
            {/* The headline number is the ACTIVE count. The "All…" figures the API
                also returns are unreliable (AllSuppliersNum comes back as 0), so
                they are not shown as a sub-line. */}
            <KPICard icon={<IconUsers />} value={nums.employeesNum} label={t("nav.employees")} href={passengersHref} />
            <KPICard icon={<IconBuilding />} value={nums.suppliersNum} label={t("nav.suppliers")} href={p("/suppliers")} />
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
                { label: t("board.oneWayGo"), value: `${nums.oneWayGoPercent}%` },
                { label: t("board.oneWayReturn"), value: `${nums.oneWayReturnPercent}%` },
              ]}
            />
            <StatGroupCard
              icon={<IconUser />}
              title={t("board.userAttendance")}
              onClick={() => setUsersOpen(true)}
              stats={[
                { label: t("board.goWord"), value: nums.employeesGoNum },
                { label: t("board.returnWord"), value: nums.employeesReturnNum },
              ]}
            />
            <StatGroupCard
              icon={<IconTrend />}
              title={t("board.userAttendancePct")}
              tone="amber"
              stats={[
                { label: t("board.usersPct"), value: `${nums.employeesPercent}%` },
                { label: t("board.capacityPct"), value: `${nums.capacityPercent}%` },
              ]}
            />
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

      {usersOpen && (
        <Dialog title={`${t("board.userAttendance")} — ${date}`} size="xl" onClose={() => setUsersOpen(false)}>
          <DataTable columns={attColumns} rows={dialogRows} loading={attLoading} emptyMessage={t("empty.attendance")} />
        </Dialog>
      )}
    </div>
  );
}
