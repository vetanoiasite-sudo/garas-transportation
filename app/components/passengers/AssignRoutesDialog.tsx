"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { routes } from "@/lib/data";
import type { Period } from "@/lib/types";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import { Field, Input, Select } from "@/components/ui/Field";
import { IconPlus, IconEdit, IconTrash, IconPin } from "@/components/ui/Icons";

interface Row {
  id: string;
  routeId?: string;
  station?: string;
  period: Period;
  fromDate?: string;
  toDate?: string;
  lat?: number;
  lng?: number;
  saved: boolean;
  editing: boolean;
  dirty: boolean;
}

/* Multi-row assignment: saved rows are read-only until Edit; per-row Submit
   enables only when something changed (change detection); only new rows are
   posted on the dialog-level Submit. (§7.2) */
export default function AssignRoutesDialog({ onClose }: { onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [rows, setRows] = useState<Row[]>([
    { id: "r-seed", routeId: routes[0]?.id, period: "both", saved: true, editing: false, dirty: false },
  ]);
  const [mapFor, setMapFor] = useState<string | null>(null);

  const addRow = () =>
    setRows((rs) => [...rs, { id: `row${Date.now()}`, period: "both", saved: false, editing: true, dirty: false }]);

  const patch = (id: string, p: Partial<Row>) =>
    setRows((rs) => rs.map((r) => (r.id === id ? { ...r, ...p, dirty: true } : r)));

  const removeRow = (id: string) => {
    const row = rows.find((r) => r.id === id);
    setRows((rs) => rs.filter((r) => r.id !== id));
    if (row?.saved) toast("تم حذف التعيين", "info");
  };

  const saveRow = (id: string) => {
    setRows((rs) => rs.map((r) => (r.id === id ? { ...r, saved: true, editing: false, dirty: false } : r)));
    toast("تم حفظ التعيين");
  };

  const submitAll = () => {
    const newRows = rows.filter((r) => !r.saved);
    if (newRows.length) toast(`تم حفظ ${newRows.length} تعيين جديد`);
    setRows((rs) => rs.map((r) => ({ ...r, saved: true, editing: false, dirty: false })));
    onClose();
  };

  return (
    <>
      <Dialog
        title="تعيين مسارات للراكب"
        size="lg"
        onClose={onClose}
        footer={
          <>
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submitAll}>{t("action.submit")}</button>
          </>
        }
      >
        <div className="mb-4">
          <button className="btn btn-outline-brand btn-sm" onClick={addRow}><IconPlus />إضافة مسار</button>
        </div>

        <div className="stack" style={{ gap: "var(--space-3)" }}>
          {rows.map((r) => {
            const readOnly = r.saved && !r.editing;
            return (
              <div key={r.id} className="card card-pad" style={{ background: readOnly ? "var(--gray-50)" : "#fff" }}>
                <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
                  <Field label={t("filter.route")} required className="grow" style={{ margin: 0, minWidth: 160 }}>
                    <Combobox options={routes.map((x) => ({ value: x.id, label: x.name }))} value={r.routeId} onChange={(v) => patch(r.id, { routeId: v })} placeholder="اختر مسارًا" disabled={readOnly} />
                  </Field>
                  <Field label="المحطة" style={{ margin: 0, minWidth: 130 }}>
                    <Input value={r.station ?? ""} onChange={(e) => patch(r.id, { station: e.target.value })} disabled={readOnly} placeholder="—" />
                  </Field>
                  <Field label="الاتجاه" required style={{ margin: 0, minWidth: 150 }}>
                    <Select value={r.period} onChange={(e) => patch(r.id, { period: e.target.value as Period })} disabled={readOnly}>
                      <option value="both">{t("period.both")}</option>
                      <option value="go">{t("period.go")}</option>
                      <option value="return">{t("period.return")}</option>
                    </Select>
                  </Field>
                </div>
                <div className="row gap-3 wrap mt-4" style={{ alignItems: "flex-end" }}>
                  <Field label="من تاريخ" style={{ margin: 0, minWidth: 130 }}>
                    <Input type="date" value={r.fromDate ?? ""} onChange={(e) => patch(r.id, { fromDate: e.target.value })} disabled={readOnly} />
                  </Field>
                  <Field label="إلى تاريخ" style={{ margin: 0, minWidth: 130 }}>
                    <Input type="date" value={r.toDate ?? ""} onChange={(e) => patch(r.id, { toDate: e.target.value })} disabled={readOnly} />
                  </Field>
                  <div className="field" style={{ margin: 0 }}>
                    <span className="label">الإحداثيات</span>
                    <button className="btn btn-secondary btn-sm" onClick={() => setMapFor(r.id)} disabled={readOnly}>
                      <IconPin />{r.lat != null ? `${r.lat}, ${r.lng}` : t("action.pickLocation")}
                    </button>
                  </div>
                  <div className="row gap-2" style={{ marginInlineStart: "auto" }}>
                    {readOnly ? (
                      <button className="icon-btn brand" aria-label={t("action.edit")} onClick={() => patch(r.id, { editing: true, dirty: false })}><IconEdit /></button>
                    ) : (
                      <button className="btn btn-success btn-sm" disabled={!r.dirty && r.saved} onClick={() => saveRow(r.id)}>{t("action.save")}</button>
                    )}
                    <button className="icon-btn danger" aria-label={t("action.delete")} onClick={() => removeRow(r.id)}><IconTrash /></button>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </Dialog>

      {mapFor && (
        <MapPickerDialog
          onPick={(c) => { patch(mapFor, { lat: c.lat, lng: c.lng }); setMapFor(null); }}
          onClose={() => setMapFor(null)}
        />
      )}
    </>
  );
}
