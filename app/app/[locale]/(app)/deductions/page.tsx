"use client";

import { useState, useMemo } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { canAdd } from "@/lib/types";
import { deductions as seed, suppliers, routes } from "@/lib/data";
import type { Deduction } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus } from "@/components/ui/Icons";

const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";

export default function DeductionsPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const [rows, setRows] = useState<Deduction[]>(seed);
  const [open, setOpen] = useState(false);

  const columns: Column<Deduction>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "routeName", header: t("filter.route") },
    { key: "supplier", header: t("filter.supplier") },
    { key: "driver", header: t("filter.driver") },
    { key: "day", header: "يوم الخصم" },
    { key: "amount", header: "المبلغ", render: (r) => <b style={{ color: "var(--color-danger)" }}>{money(r.amount)}</b> },
    { key: "type", header: "النوع", render: (r) => (r.type === "tax" ? <span className="badge badge-amber">ضريبة</span> : <span className="badge badge-gray">خصم عادي</span>) },
    { key: "reason", header: "السبب" },
    { key: "createdBy", header: "أنشئ بواسطة" },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.deductions")} count={rows.length}>
        {canAdd(user?.role) && <button className="btn btn-brand btn-sm" onClick={() => setOpen(true)}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        emptyMessage="لا توجد خصومات مسجّلة."
        emptyAction={canAdd(user?.role) ? <button className="btn btn-brand btn-sm" onClick={() => setOpen(true)}><IconPlus />{t("action.addNew")}</button> : undefined}
      />

      {open && <DeductionForm onClose={() => setOpen(false)} onSaved={(d) => setRows((r) => [d, ...r])} />}
    </div>
  );
}

function DeductionForm({ onClose, onSaved }: { onClose: () => void; onSaved: (d: Deduction) => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [supplier, setSupplier] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  const [type, setType] = useState<"normal" | "tax">("normal");
  const [percentMode, setPercentMode] = useState(false);
  const [percent, setPercent] = useState("");
  const [perRound, setPerRound] = useState("");
  const [date, setDate] = useState("");
  const [reason, setReason] = useState("");

  const selectedRoute = routes.find((r) => r.id === route);
  const price = selectedRoute?.cost ?? 0;

  // live calc: per-round = price × % ÷ 100
  const computedPerRound = useMemo(() => {
    if (!percentMode || !percent) return perRound;
    return String(((price * Number(percent)) / 100).toFixed(2));
  }, [percentMode, percent, price, perRound]);

  const submit = () => {
    if (!supplier || !route || !date || !reason.trim() || !computedPerRound) { toast("أكمل الحقول المطلوبة", "error"); return; }
    onSaved({
      id: `d${Date.now()}`,
      routeName: selectedRoute?.name ?? "",
      supplier: suppliers.find((s) => s.id === supplier)?.name ?? "",
      driver: selectedRoute?.driver ?? "—",
      day: date, amount: Number(computedPerRound), createdBy: "المستخدم الحالي", createdAt: "2026-07-09", reason, type,
    });
    toast("تم تسجيل الخصم — تم تحديث كشف الحساب");
    onClose();
  };

  return (
    <Dialog
      title="إضافة خصم"
      size="lg"
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-brand" onClick={submit}>{t("action.save")}</button></>}
    >
      <div className="two-panel">
        <Field label={t("filter.supplier")} required>
          <Combobox options={suppliers.map((s) => ({ value: s.id, label: s.name }))} value={supplier} onChange={(v) => { setSupplier(v); setRoute(undefined); }} placeholder={t("filter.supplier")} />
        </Field>
        <Field label={t("filter.route")} required>
          <Combobox options={routes.filter((r) => !supplier || r.supplier === suppliers.find((s) => s.id === supplier)?.name).map((r) => ({ value: r.id, label: r.name }))} value={route} onChange={setRoute} placeholder={t("filter.route")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
        </Field>
      </div>

      {selectedRoute && (
        <div className="row gap-3 wrap mb-4">
          <span className="info-chip">الرقم: {selectedRoute.serial}</span>
          <span className="info-chip">سعر المسار: {money(price)}</span>
        </div>
      )}

      <div className="field">
        <span className="label" id="ded-type-label">النوع <span className="req" aria-hidden>*</span></span>
        <SegmentedControl<"normal" | "tax">
          value={type}
          onChange={setType}
          ariaLabelledby="ded-type-label"
          segments={[
            { value: "normal", label: "خصم عادي" },
            { value: "tax", label: "ضريبة" },
          ]}
        />
      </div>

      <label className="checkbox-row mb-4"><input type="checkbox" checked={percentMode} onChange={(e) => setPercentMode(e.target.checked)} />الخصم كنسبة مئوية</label>

      <div className="two-panel">
        {percentMode && (
          <Field label="النسبة (%)">
            <Input inputMode="decimal" value={percent} onChange={(e) => setPercent(e.target.value)} placeholder="10" />
          </Field>
        )}
        <Field label="الخصم لكل (نصف) جولة" required hint={percentMode ? "يُحسب تلقائيًا: السعر × النسبة ÷ ١٠٠" : undefined}>
          <Input inputMode="decimal" value={computedPerRound} onChange={(e) => setPerRound(e.target.value)} readOnly={percentMode} style={percentMode ? { background: "var(--gray-100)" } : undefined} />
        </Field>
      </div>

      <div className="two-panel">
        <Field label="التاريخ" required><Input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></Field>
        <Field label="السبب" required><Input value={reason} onChange={(e) => setReason(e.target.value)} /></Field>
      </div>
    </Dialog>
  );
}
