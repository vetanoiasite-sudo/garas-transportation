"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import type { CostReportRow } from "@/lib/types";
import { getCostReport, downloadCostReportExcel } from "@/lib/services/costs";
import { openFileUrl } from "@/lib/download";
import { apiGet } from "@/lib/api/client";
import { useSupplierOptions, useDriverOptions } from "@/lib/hooks/useSuppliers";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { IconDownload, IconChevronEnd } from "@/components/ui/Icons";

const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";
const isoDay = (d: Date) => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Backend dropdown row shapes (frozen PascalCase keys).
interface LineRow { Id: number; Name: string }
interface RouteRow { Id: number; NameOfRoute: string; Serial?: string }

// Line Cost Report (ReportCostOfLines). Reached from the dashboard "Line Cost
// Report" button. Date range defaults to the current month (RULES §4).
export default function LineCostReportPage() {
  const { t, locale } = useLocale();
  const { toast } = useToast();
  const p = (path: string) => `/${locale}${path}`;

  const [rows, setRows] = useState<CostReportRow[]>([]);
  const [loading, setLoading] = useState(true);

  const [lineOpts, setLineOpts] = useState<LineRow[]>([]);
  const [routeOpts, setRouteOpts] = useState<RouteRow[]>([]);

  const [line, setLine] = useState<string | undefined>();
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  const [serial, setSerial] = useState("");
  // Current month by default (the report only counts rounds when both ends are set).
  const [from, setFrom] = useState(() => { const d = new Date(); return isoDay(new Date(d.getFullYear(), d.getMonth(), 1)); });
  const [to, setTo] = useState(() => { const d = new Date(); return isoDay(new Date(d.getFullYear(), d.getMonth() + 1, 0)); });

  const onLoadError = useCallback((e: unknown) => toast(errMsg(e, t("empty.generic")), "error"), [toast, t]);
  const supplierOpts = useSupplierOptions(onLoadError);
  const driverOptions = useDriverOptions(supplier, onLoadError);

  // Load the filter dropdowns once.
  useEffect(() => {
    (async () => {
      try {
        const [ls, rs] = await Promise.all([
          apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 500 }),
          apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 }),
        ]);
        setLineOpts(ls.Data ?? []);
        setRouteOpts(rs.Data ?? []);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  // The cost report filters by supplier / line / serial / date. A chosen route
  // maps to its serial (the report has no direct route filter).
  const routeSerial = routeOpts.find((r) => String(r.Id) === route)?.Serial;
  const effectiveSerial = serial || routeSerial || undefined;

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getCostReport({ driverId: driver, lineId: line, supplierId: supplier, serial: effectiveSerial, from, to });
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [line, supplier, effectiveSerial, from, to, toast, t]);

  useEffect(() => { load(); }, [load]);

  const [downloading, setDownloading] = useState(false);
  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const url = await downloadCostReportExcel({ driverId: driver, lineId: line, supplierId: supplier, serial: effectiveSerial, from, to });
      openFileUrl(url, "line-cost-report.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [line, supplier, effectiveSerial, from, to, toast, t]);

  const columns: Column<CostReportRow>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "lineName", header: t("costs.lineName"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.lineName}</b> },
    { key: "serial", header: t("filter.serial"), priority: "secondary", render: (r) => r.serial || "—" },
    { key: "routeName", header: t("filter.route") },
    { key: "supplier", header: t("costs.supplierName"), priority: "secondary" },
    { key: "cost", header: t("costs.cost"), render: (r) => money(r.cost) },
    { key: "oneWay", header: t("board.oneWay"), align: "center", priority: "secondary", render: (r) => (r.oneWay ? <span className="badge badge-blue">{t("common.yes")}</span> : <span className="badge badge-gray">—</span>) },
    { key: "rounds", header: t("costs.rounds"), align: "center" },
    { key: "deductionRounds", header: t("costs.deductionRounds"), align: "center", priority: "secondary" },
    { key: "deductionCost", header: t("costs.deductionCost"), priority: "secondary", render: (r) => <span style={{ color: "var(--color-danger)" }}>{money(r.deductionCost)}</span> },
    { key: "netCost", header: t("costs.netCost"), render: (r) => <b style={{ color: "var(--color-success)" }}>{money(r.netCost)}</b> },
  ];

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link to={p("/dashboard")} style={{ color: "var(--color-brand)" }}>{t("nav.dashboard")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{t("costs.title")}</span>
      </div>

      <PageHeader title={t("costs.title")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("action.downloadExcel")}</button>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        loading={loading}
        emptyMessage={t("costs.empty")}
        toolbar={
          <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
            <FilterField label={t("filter.line")}>
              <Combobox options={lineOpts.map((l) => ({ value: String(l.Id), label: l.Name }))} value={line} onChange={setLine} placeholder={t("filter.all")} />
            </FilterField>
            <FilterField label={t("filter.supplier")}>
              <Combobox options={supplierOpts.map((s) => ({ value: String(s.Id), label: s.Name }))} value={supplier} onChange={(v) => { setSupplier(v); setDriver(undefined); }} placeholder={t("filter.all")} />
            </FilterField>
            <FilterField label={t("filter.driver")}>
              <Combobox options={driverOptions} value={driver} onChange={setDriver} placeholder={t("filter.driver")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
            </FilterField>
            <FilterField label={t("filter.route")}>
              <Combobox options={routeOpts.map((r) => ({ value: String(r.Id), label: r.NameOfRoute }))} value={route} onChange={(v) => { setRoute(v); setSerial(""); }} placeholder={t("filter.all")} />
            </FilterField>
            <FilterField label={t("filter.serial")}>
              <Input inputMode="numeric" value={serial} onChange={(e) => { setSerial(e.target.value.replace(/\D/g, "")); setRoute(undefined); }} placeholder="—" />
            </FilterField>
            <FilterField label={t("filter.from")}><Input type="date" value={from} onChange={(e) => setFrom(e.target.value)} /></FilterField>
            <FilterField label={t("filter.to")}><Input type="date" value={to} onChange={(e) => setTo(e.target.value)} /></FilterField>
          </div>
        }
      />
    </div>
  );
}

function FilterField({ label, children }: { label: string; children: React.ReactNode }) {
  return (
    <Field label={label} style={{ margin: 0, minWidth: 150, flex: 1 }}>
      {children}
    </Field>
  );
}
