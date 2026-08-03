"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import type { AttendanceRecord } from "@/lib/types";
import { getAttendance, downloadAttendanceExcel } from "@/lib/services/attendance";
import { saveBlob } from "@/lib/download";
import { apiGet } from "@/lib/api/client";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import Dialog from "@/components/ui/Dialog";
import { AttendanceText } from "@/components/ui/Badge";
import { IconDownload, IconEye } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Backend dropdown row shapes (frozen PascalCase keys).
interface LineRow { Id: number; Name: string }
interface SupplierRow { Id: number; Name: string; contacts?: { Id: number; Name: string }[] }
interface RouteRow { Id: number; NameOfRoute: string }

const isoDay = (d: Date) => d.toISOString().slice(0, 10);

// Render a check-in/out timestamp as a short readable "yyyy-MM-dd HH:mm".
// Falls back to the raw value if it isn't a parseable ISO date.
const fmtStamp = (v?: string): string => {
  if (!v) return "";
  const d = new Date(v);
  if (isNaN(d.getTime())) return v;
  const pad = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())} ${pad(d.getHours())}:${pad(d.getMinutes())}`;
};

export default function AttendancePage() {
  const { t } = useLocale();
  const { toast } = useToast();

  const [records, setRecords] = useState<AttendanceRecord[]>([]);
  const [loading, setLoading] = useState(true);

  const [lineOpts, setLineOpts] = useState<LineRow[]>([]);
  const [supplierOpts, setSupplierOpts] = useState<SupplierRow[]>([]);
  const [routeOpts, setRouteOpts] = useState<RouteRow[]>([]);

  const [line, setLine] = useState<string | undefined>();
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  const [serial, setSerial] = useState("");
  const [employeeId, setEmployeeId] = useState(""); // passenger national/fingerprint id filter
  const [from, setFrom] = useState(() => { const d = new Date(); return isoDay(new Date(d.getFullYear(), d.getMonth(), 1)); });
  const [to, setTo] = useState(() => isoDay(new Date()));
  const [attended, setAttended] = useState(true);
  const [absent, setAbsent] = useState(true);
  const [history, setHistory] = useState<AttendanceRecord | null>(null);
  const [downloading, setDownloading] = useState(false);

  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const blob = await downloadAttendanceExcel({ from, to, lineId: line, supplierId: supplier, serial: serial || undefined, routeId: route, employeeId: employeeId || undefined });
      saveBlob(blob, "attendance.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [from, to, line, supplier, serial, route, employeeId, toast, t]);

  const driverOptions = useMemo(() => {
    const s = supplierOpts.find((x) => String(x.Id) === supplier);
    return (s?.contacts ?? []).map((c) => ({ value: String(c.Id), label: c.Name }));
  }, [supplier, supplierOpts]);

  // Load the filter dropdowns once.
  useEffect(() => {
    (async () => {
      try {
        const [ls, ss, rs] = await Promise.all([
          apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 500 }),
          apiGet<SupplierRow[]>("getSuppliers", { PageNo: 1, NoOfItems: 500 }),
          apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 }),
        ]);
        setLineOpts(ls.Data ?? []);
        setSupplierOpts(ss.Data ?? []);
        setRouteOpts(rs.Data ?? []);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  // Server-side filters: line / supplier / serial / route / date range.
  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getAttendance({ from, to, lineId: line, supplierId: supplier, serial: serial || undefined, routeId: route, employeeId: employeeId || undefined });
      setRecords(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [from, to, line, supplier, serial, route, employeeId, toast, t]);

  useEffect(() => { load(); }, [load]);

  // Driver + attended/absent are refined client-side (no dedicated server filters).
  const driverName = driverOptions.find((d) => d.value === driver)?.label;
  const rows = records.filter(
    (r) =>
      ((r.attended && attended) || (!r.attended && absent)) &&
      (!driverName || r.driver === driverName)
  );

  // Single day (from == to) → split into check-in / check-out columns.
  // A date range → one "attendance" column with the full history popup.
  const singleDay = from === to;

  const baseColumns: Column<AttendanceRecord>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: t("common.name"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.name}</b> },
    { key: "idCode", header: t("att.identityNumber") },
    { key: "otherId", header: t("att.otherId"), priority: "secondary" },
    { key: "line", header: t("filter.line"), priority: "secondary" },
    { key: "route", header: t("filter.route") },
    { key: "supervisor", header: t("common.supervisor"), priority: "secondary" },
    { key: "supplier", header: t("filter.supplier"), priority: "secondary" },
    { key: "driver", header: t("filter.driver"), priority: "secondary" },
  ];

  const singleDayColumns: Column<AttendanceRecord>[] = [
    {
      key: "checkIn",
      header: t("att.checkIn"),
      render: (r) => <span style={{ color: r.attended ? "var(--text-heading)" : "var(--color-danger)" }}>{r.checkIn ? fmtStamp(r.checkIn) : (r.attended ? "—" : t("status.absent"))}</span>,
    },
    {
      key: "checkOut",
      header: t("att.checkOut"),
      render: (r) => <span>{r.checkOut ? fmtStamp(r.checkOut) : "—"}</span>,
    },
  ];

  const rangeColumn: Column<AttendanceRecord> = {
    key: "attendance",
    header: t("att.attendance"),
    render: (r) => (
      <div className="row" style={{ gap: 8 }}>
        <div className="col" style={{ gap: 2 }}>
          <AttendanceText attended={r.attended} time={r.checkIn ? `${t("att.checkIn")} ${r.checkIn}` : undefined} />
          {r.checkOut && <span className="text-xs muted">{t("att.checkOut")} {r.checkOut}</span>}
        </div>
        {r.history && r.history.length > 0 && (
          <button className="icon-btn" onClick={() => setHistory(r)} aria-label={`${t("att.historyLabel")}: ${r.name}`}><IconEye /></button>
        )}
      </div>
    ),
  };

  const columns: Column<AttendanceRecord>[] = [
    ...baseColumns,
    ...(singleDay ? singleDayColumns : [rangeColumn]),
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.attendance")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("action.downloadExcel")}</button>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        loading={loading}
        emptyMessage={t("empty.attendance")}
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
            <FilterField label={t("filter.passengerId")}>
              <Input inputMode="numeric" value={employeeId} onChange={(e) => setEmployeeId(e.target.value.replace(/\D/g, ""))} placeholder="—" />
            </FilterField>
            <FilterField label={t("filter.from")}><Input type="date" value={from} onChange={(e) => setFrom(e.target.value)} /></FilterField>
            <FilterField label={t("filter.to")}><Input type="date" value={to} onChange={(e) => setTo(e.target.value)} /></FilterField>
            <div className="row gap-3" style={{ paddingBottom: 6 }}>
              <label className="checkbox-row"><input type="checkbox" checked={attended} onChange={(e) => setAttended(e.target.checked)} />{t("status.attended")}</label>
              <label className="checkbox-row"><input type="checkbox" checked={absent} onChange={(e) => setAbsent(e.target.checked)} />{t("status.absent")}</label>
            </div>
          </div>
        }
      />

      {history && (
        <Dialog title={`${t("att.historyLabel")} — ${history.name}`} onClose={() => setHistory(null)}>
          <div className="table-wrap">
            <table className="data">
              <thead><tr><th>{t("common.date")}</th><th>{t("att.checkIn")}</th><th>{t("att.checkOut")}</th><th>{t("att.status")}</th></tr></thead>
              <tbody>
                {history.history!.map((h, i) => (
                  <tr key={i}>
                    <td>{h.date}</td>
                    <td>{h.checkIn ?? "—"}</td>
                    <td>{h.checkOut ?? "—"}</td>
                    <td>{h.attended ? <span className="badge badge-green">{t("status.attended")}</span> : <span className="badge badge-red">{t("status.absent")}</span>}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </Dialog>
      )}
    </div>
  );
}

function FilterField({ label, children }: { label: string; children: React.ReactNode }) {
  return (
    <Field label={label} style={{ margin: 0, minWidth: 160, flex: 1 }}>
      {children}
    </Field>
  );
}
