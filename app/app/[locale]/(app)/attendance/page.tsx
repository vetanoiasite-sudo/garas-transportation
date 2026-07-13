"use client";

import { useState, useMemo } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { attendance, suppliers, lines, routes } from "@/lib/data";
import type { AttendanceRecord } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import Dialog from "@/components/ui/Dialog";
import { AttendanceText } from "@/components/ui/Badge";
import { IconDownload, IconEye } from "@/components/ui/Icons";

export default function AttendancePage() {
  const { t } = useLocale();
  const [line, setLine] = useState<string | undefined>();
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [attended, setAttended] = useState(true);
  const [absent, setAbsent] = useState(true);
  const [history, setHistory] = useState<AttendanceRecord | null>(null);

  const driverOptions = useMemo(() => {
    const s = suppliers.find((x) => x.id === supplier);
    return (s?.contacts ?? []).map((c) => ({ value: c.id, label: c.name }));
  }, [supplier]);

  const rows = attendance.filter((r) => (r.attended && attended) || (!r.attended && absent));

  const columns: Column<AttendanceRecord>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: "الاسم", render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.name}</b> },
    { key: "idCode", header: "الكود", priority: "secondary" },
    { key: "otherId", header: "معرّف آخر", priority: "secondary" },
    { key: "route", header: t("filter.route") },
    { key: "supervisor", header: "المشرف", priority: "secondary" },
    { key: "driver", header: t("filter.driver"), priority: "secondary" },
    {
      key: "attendance",
      header: "الحضور",
      render: (r) => (
        <div className="row" style={{ gap: 8 }}>
          <div className="col" style={{ gap: 2 }}>
            <AttendanceText attended={r.attended} time={r.checkIn ? `دخول ${r.checkIn}` : undefined} />
            {r.checkOut && <span className="text-xs muted">خروج {r.checkOut}</span>}
          </div>
          {r.history && r.history.length > 0 && (
            <button className="icon-btn" onClick={() => setHistory(r)} aria-label={`سجل الحضور: ${r.name}`}><IconEye /></button>
          )}
        </div>
      ),
    },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.attendance")} count={rows.length}>
        <button className="btn btn-secondary btn-sm"><IconDownload />{t("action.downloadExcel")}</button>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        emptyMessage={t("empty.attendance")}
        toolbar={
          <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
            <FilterField label={t("filter.line")}>
              <Combobox options={lines.map((l) => ({ value: l.id, label: l.name }))} value={line} onChange={setLine} placeholder={t("filter.all")} />
            </FilterField>
            <FilterField label={t("filter.supplier")}>
              <Combobox options={suppliers.map((s) => ({ value: s.id, label: s.name }))} value={supplier} onChange={(v) => { setSupplier(v); setDriver(undefined); }} placeholder={t("filter.all")} />
            </FilterField>
            <FilterField label={t("filter.driver")}>
              <Combobox options={driverOptions} value={driver} onChange={setDriver} placeholder={t("filter.driver")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
            </FilterField>
            <FilterField label={t("filter.from")}><Input type="date" /></FilterField>
            <FilterField label={t("filter.to")}><Input type="date" /></FilterField>
            <div className="row gap-3" style={{ paddingBottom: 6 }}>
              <label className="checkbox-row"><input type="checkbox" checked={attended} onChange={(e) => setAttended(e.target.checked)} />{t("status.attended")}</label>
              <label className="checkbox-row"><input type="checkbox" checked={absent} onChange={(e) => setAbsent(e.target.checked)} />{t("status.absent")}</label>
            </div>
          </div>
        }
      />

      {history && (
        <Dialog title={`سجل الحضور — ${history.name}`} onClose={() => setHistory(null)}>
          <div className="table-wrap">
            <table className="data">
              <thead><tr><th>التاريخ</th><th>دخول</th><th>خروج</th><th>الحالة</th></tr></thead>
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
