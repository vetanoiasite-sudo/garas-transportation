"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getBusAttendance, downloadBusAttendanceExcel, type BusAttendanceRecord } from "@/lib/services/attendance";
import { openFileUrl } from "@/lib/download";
import { formatDate, formatDateTime } from "@/lib/datetime";
import { apiGet } from "@/lib/api/client";
import { useSupplierOptions, useDriverOptions } from "@/lib/hooks/useSuppliers";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import Dialog from "@/components/ui/Dialog";
import { IconBus, IconDownload } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Backend dropdown row shapes (frozen PascalCase keys).
interface RouteRow { Id: number; NameOfRoute: string; Serial?: string }

const isoDay = (d: Date) => d.toISOString().slice(0, 10);

// LOCAL calendar day of a timestamp (the backend returns UTC instants; a
// check-in at 00:10 local must land on the local day, not the UTC one).
const localDay = (v: string | Date) => {
  const d = new Date(v);
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
};

/** Same logic as the employee attendance screen, but each row is a BUS (route). */
export default function BusAttendancePage() {
  const { t } = useLocale();
  const { toast } = useToast();

  const [records, setRecords] = useState<BusAttendanceRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [routeOpts, setRouteOpts] = useState<RouteRow[]>([]);

  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  const [serial, setSerial] = useState("");
  const [name, setName] = useState("");
  const [from, setFrom] = useState(() => isoDay(new Date()));
  const [to, setTo] = useState(() => isoDay(new Date()));
  const [history, setHistory] = useState<BusAttendanceRecord | null>(null);
  const [downloading, setDownloading] = useState(false);

  const onLoadError = useCallback((e: unknown) => toast(errMsg(e, t("empty.generic")), "error"), [toast, t]);
  const supplierOpts = useSupplierOptions(onLoadError);
  const driverOptions = useDriverOptions(supplier, onLoadError);

  // Load the filter dropdowns once.
  useEffect(() => {
    (async () => {
      try {
        const [rs] = await Promise.all([
          apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 }),
        ]);
        setRouteOpts(rs.Data ?? []);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getBusAttendance({
        from,
        to,
        supplierId: supplier,
        driverId: driver,
        serial: serial || undefined,
        routeId: route,
        name: name || undefined,
        noOfItems: 200,
      });
      setRecords(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [from, to, supplier, driver, serial, route, name, toast, t]);

  useEffect(() => { load(); }, [load]);

  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const url = await downloadBusAttendanceExcel({
        from, to, supplierId: supplier, driverId: driver, serial: serial || undefined, routeId: route, name: name || undefined,
      });
      openFileUrl(url, "bus-attendance.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [from, to, supplier, driver, serial, route, name, toast, t]);

  // Single day (from == to) → split into check-in / check-out columns.
  // A date range → one "attendance" column with the bus-icon history popup.
  const singleDay = from === to;

  const dayEntry = (r: BusAttendanceRecord) => r.history[r.history.length - 1];

  const absent = <span style={{ color: "var(--color-danger)" }}>{t("status.absent")}</span>;

  const present = <span style={{ color: "var(--green-700)" }}>{t("att.present")}</span>;

  // The API reports per-day LEGS as booleans (did the bus run), plus how many
  // passengers rode each leg — there are no timestamps and no supplier/driver.
  const baseColumns: Column<BusAttendanceRecord>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "line", header: t("filter.line") },
    { key: "route", header: t("filter.route"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.route}</b> },
    { key: "serial", header: t("filter.serial") },
  ];

  const singleDayColumns: Column<BusAttendanceRecord>[] = [
    {
      key: "checkIn",
      header: t("board.checkIn"),
      render: (r) => (dayEntry(r)?.checkIn ? present : absent),
    },
    {
      key: "checkOut",
      header: t("board.checkOut"),
      render: (r) => (dayEntry(r)?.checkOut ? present : absent),
    },
    {
      key: "riders",
      header: t("board.userAttendance"),
      render: (r) => <span>{(dayEntry(r)?.riddenIn ?? 0) + (dayEntry(r)?.riddenOut ?? 0)}</span>,
    },
  ];

  const rangeColumn: Column<BusAttendanceRecord> = {
    key: "attendance",
    header: t("att.attendance"),
    render: (r) =>
      r.attended ? (
        <button
          className="icon-btn brand"
          onClick={() => setHistory(r)}
          aria-label={`${t("att.historyLabel")}: ${r.route}`}
          style={{ fontSize: 22 }}
        >
          <IconBus />
        </button>
      ) : (
        absent
      ),
  };

  const columns: Column<BusAttendanceRecord>[] = [
    ...baseColumns,
    ...(singleDay ? singleDayColumns : [rangeColumn]),
  ];

  // The popup lists EVERY day in the range (absent days included, like the old system).
  const historyDays = useMemo(() => {
    if (!history) return [];
    const byDay = new Map(history.history.map((h) => [localDay(h.date), h]));
    const days: { date: string; checkIn: boolean; checkOut: boolean }[] = [];
    const start = new Date(`${from}T00:00:00`);
    const end = new Date(`${to}T00:00:00`);
    for (let d = new Date(start); d <= end && days.length < 400; d.setDate(d.getDate() + 1)) {
      const key = localDay(d);
      const h = byDay.get(key);
      days.push({ date: key, checkIn: !!h?.checkIn, checkOut: !!h?.checkOut });
    }
    return days;
  }, [history, from, to]);

  return (
    <div className="stack">
      <PageHeader title={`${t("busAtt.title")} ${from} → ${to}`} count={records.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("action.downloadExcel")}</button>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={records}
        loading={loading}
        emptyMessage={t("empty.attendance")}
        toolbar={
          <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
            <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 160, flex: 1 }}>
              <Combobox
                options={supplierOpts.map((s) => ({ value: String(s.Id), label: s.Name }))}
                value={supplier}
                onChange={(v) => { setSupplier(v); setDriver(undefined); }}
                placeholder={t("filter.all")}
              />
            </Field>
            <Field label={t("filter.driver")} style={{ margin: 0, minWidth: 160, flex: 1 }}>
              <Combobox
                options={driverOptions}
                value={driver}
                onChange={setDriver}
                placeholder={t("filter.all")}
                disabled={!supplier}
                disabledReason={t("filter.selectSupplierFirst")}
              />
            </Field>
            <Field label={t("filter.route")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
              <Combobox
                options={routeOpts.map((r) => ({ value: String(r.Id), label: r.NameOfRoute }))}
                value={route}
                onChange={(v) => {
                  // Two-way link with the serial field (same as the dashboard pair).
                  setRoute(v);
                  const match = routeOpts.find((r) => String(r.Id) === v);
                  setSerial(match?.Serial ?? "");
                }}
                placeholder={t("filter.all")}
              />
            </Field>
            <Field label={t("common.name")} style={{ margin: 0, minWidth: 150 }}>
              <Input value={name} onChange={(e) => setName(e.target.value)} placeholder={t("common.name")} />
            </Field>
            <Field label={t("filter.serial")} style={{ margin: 0, minWidth: 130 }}>
              <Input
                inputMode="numeric"
                value={serial}
                onChange={(e) => {
                  const v = e.target.value.replace(/\D/g, "");
                  setSerial(v);
                  const match = routeOpts.find((r) => r.Serial === v);
                  setRoute(match ? String(match.Id) : undefined);
                }}
                placeholder="1024"
              />
            </Field>
            <Field label={t("filter.from")} style={{ margin: 0, minWidth: 140 }}>
              <Input type="date" value={from} onChange={(e) => setFrom(e.target.value)} />
            </Field>
            <Field label={t("filter.to")} style={{ margin: 0, minWidth: 140 }}>
              <Input type="date" value={to} onChange={(e) => setTo(e.target.value)} />
            </Field>
          </div>
        }
      />

      {history && (
        <Dialog title={`${history.route} : ${t("att.attendance")}`} size="lg" onClose={() => setHistory(null)}>
          <div style={{ overflowX: "auto" }}>
            <table className="table">
              <thead>
                <tr>
                  <th>{t("common.date")}</th>
                  <th>{t("board.checkIn")}</th>
                  <th>{t("board.checkOut")}</th>
                </tr>
              </thead>
              <tbody>
                {historyDays.map((h) => (
                  <tr key={h.date}>
                    <td>{formatDate(h.date)}</td>
                    <td>
                      {h.checkIn
                        ? <span style={{ color: "var(--green-700)" }}>{t("att.present")}</span>
                        : <span style={{ color: "var(--color-danger)" }}>{t("status.absent")}</span>}
                    </td>
                    <td>
                      {h.checkOut
                        ? <span style={{ color: "var(--green-700)" }}>{t("att.present")}</span>
                        : <span style={{ color: "var(--color-danger)" }}>{t("status.absent")}</span>}
                    </td>
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
