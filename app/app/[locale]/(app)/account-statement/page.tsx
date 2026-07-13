"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { statement as seed, suppliers, monthNamesAr } from "@/lib/data";
import type { StatementRow } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input, Select, Textarea } from "@/components/ui/Field";
import { IconMoney, IconReceipt, IconEye, IconPrint } from "@/components/ui/Icons";

const remaining = (r: StatementRow) => r.totalDue - r.totalDeductions - r.normalPayments - r.advancePayments;
const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";

export default function StatementPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const canPay = can(user?.role, "add.payment");
  const canPrint = can(user?.role, "print.invoice");

  const [rows] = useState<StatementRow[]>(seed);
  const [pay, setPay] = useState<StatementRow | null>(null);
  const [deduct, setDeduct] = useState<StatementRow | null>(null);
  const [invoice, setInvoice] = useState<StatementRow | null>(null);
  const [supplier, setSupplier] = useState<string | undefined>();

  const filtered = supplier ? rows.filter((r) => r.supplierId === supplier) : rows;

  const columns: Column<StatementRow>[] = [
    { key: "month", header: t("filter.month"), render: (r) => monthNamesAr[r.month - 1] },
    { key: "supplier", header: t("filter.supplier"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.supplier}</b> },
    { key: "routesCount", header: "المسارات", align: "center", priority: "secondary" },
    { key: "rounds", header: "الجولات (كاملة/ذهاب/عودة)", priority: "secondary", render: (r) => `${r.roundsFull} / ${r.roundsHalfGo} / ${r.roundsHalfReturn}` },
    { key: "totalDue", header: "الإجمالي المستحق", render: (r) => money(r.totalDue) },
    { key: "totalDeductions", header: "الخصومات", priority: "secondary", render: (r) => <span style={{ color: "var(--color-danger)" }}>{money(r.totalDeductions)}</span> },
    { key: "normalPayments", header: "دفعات عادية", priority: "secondary", render: (r) => <span style={{ color: "var(--color-success)" }}>{money(r.normalPayments)}</span> },
    { key: "advancePayments", header: "دفعات مقدمة", priority: "secondary", render: (r) => money(r.advancePayments) },
    { key: "remaining", header: "المتبقّي", render: (r) => <b style={{ color: remaining(r) > 0 ? "var(--color-danger)" : "var(--color-success)" }}>{money(remaining(r))}</b> },
    {
      key: "actions", header: "", align: "end",
      render: (r) => (
        <div className="cell-actions">
          {canPay && <button className="icon-btn" style={{ color: "var(--color-success)" }} title="إضافة دفعة" onClick={() => setPay(r)}><IconMoney /></button>}
          {canPay && <button className="icon-btn danger" title="إضافة خصم" onClick={() => setDeduct(r)}><IconReceipt /></button>}
          <button className="icon-btn brand" title="الدفعات" onClick={() => setPay(r)}><IconEye /></button>
          {canPrint && <button className="icon-btn" title={t("action.print")} onClick={() => setInvoice(r)}><IconPrint /></button>}
        </div>
      ),
    },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.statement")} count={filtered.length} />

      <DataTable
        columns={columns}
        rows={filtered}
        emptyMessage={t("empty.statement")}
        toolbar={
          <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
            <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 200 }}>
              <Combobox options={suppliers.map((s) => ({ value: s.id, label: s.name }))} value={supplier} onChange={setSupplier} placeholder={t("filter.all")} />
            </Field>
            <Field label={t("filter.month")} style={{ margin: 0, minWidth: 150 }}>
              <Select><option>يونيو</option><option>مايو</option></Select>
            </Field>
            <Field label={t("filter.year")} style={{ margin: 0, minWidth: 120 }}>
              <Input defaultValue="2026" />
            </Field>
          </div>
        }
      />

      {pay && <AddPaymentDialog row={pay} onClose={() => setPay(null)} />}
      {deduct && <AddDeductionDialog row={deduct} onClose={() => setDeduct(null)} />}
      {invoice && <InvoiceDialog row={invoice} onClose={() => setInvoice(null)} />}
    </div>
  );
}

function AddPaymentDialog({ row, onClose }: { row: StatementRow; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [amount, setAmount] = useState("");
  const [date, setDate] = useState("2026-07-09");
  const [type, setType] = useState<"advance" | "normal">("normal");
  const [months, setMonths] = useState("3");
  const [startDate, setStartDate] = useState("");

  const submit = () => {
    if (!amount) { toast("أدخل المبلغ", "error"); return; }
    if (type === "advance" && (Number(months) < 1 || Number(months) > 12)) { toast("عدد الأشهر من ١ إلى ١٢", "error"); return; }
    toast("تم تسجيل الدفعة");
    onClose();
  };

  return (
    <Dialog
      title={`💰 دفعة — ${row.supplier}`}
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-success" onClick={submit}>دفع</button></>}
    >
      <Field label="المبلغ" required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} /></Field>
      <Field label="تاريخ الدفع"><Input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></Field>
      <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
        <legend className="label" style={{ padding: 0 }}>النوع</legend>
        <div className="row gap-4">
          <label className="checkbox-row"><input type="radio" name="pay-type" checked={type === "advance"} onChange={() => setType("advance")} />دفعة مقدمة (أقساط)</label>
          <label className="checkbox-row"><input type="radio" name="pay-type" checked={type === "normal"} onChange={() => setType("normal")} />دفعة شهرية</label>
        </div>
      </fieldset>
      {type === "advance" && (
        <div className="row gap-3 wrap">
          <Field label="عدد الأشهر (بحد أقصى ١٢)" className="grow"><Input inputMode="numeric" value={months} onChange={(e) => setMonths(e.target.value.replace(/\D/g, ""))} /></Field>
          <Field label="تاريخ البدء" className="grow"><Input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} /></Field>
        </div>
      )}
    </Dialog>
  );
}

function AddDeductionDialog({ row, onClose }: { row: StatementRow; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [amount, setAmount] = useState("");
  const [reason, setReason] = useState("");
  return (
    <Dialog
      title={`خصم — ${row.supplier}`}
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-danger" onClick={() => { if (!amount) { toast("أدخل المبلغ", "error"); return; } toast("تم تسجيل الخصم"); onClose(); }}>حفظ الخصم</button></>}
    >
      <Field label="المبلغ" required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} /></Field>
      <Field label="السبب"><Textarea value={reason} onChange={(e) => setReason(e.target.value)} /></Field>
    </Dialog>
  );
}

function InvoiceDialog({ row, onClose }: { row: StatementRow; onClose: () => void }) {
  const { t } = useLocale();
  const rem = remaining(row);
  return (
    <Dialog
      title="معاينة فاتورة الطباعة"
      size="lg"
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.close")}</button><button className="btn btn-brand" onClick={() => window.print()}><IconPrint />{t("action.print")}</button></>}
    >
      <div className="invoice-print card card-pad">
        <div className="row-between mb-4" style={{ borderBottom: "2px solid var(--color-navy)", paddingBottom: "var(--space-3)" }}>
          <div><div className="page-title">غراس للنقل</div><div className="muted text-sm">فاتورة خدمات نقل</div></div>
          <div className="text-sm" style={{ textAlign: "end" }}><div>الشهر: {monthNamesAr[row.month - 1]} {row.year}</div><div>المورد: <b>{row.supplier}</b></div></div>
        </div>
        <table className="data" style={{ marginBottom: "var(--space-4)" }}>
          <thead><tr><th>البند</th><th>العدد</th></tr></thead>
          <tbody>
            <tr><td>جولات كاملة</td><td>{row.roundsFull}</td></tr>
            <tr><td>نصف ذهاب</td><td>{row.roundsHalfGo}</td></tr>
            <tr><td>نصف عودة</td><td>{row.roundsHalfReturn}</td></tr>
            <tr><td>عدد المسارات</td><td>{row.routesCount}</td></tr>
          </tbody>
        </table>
        <div className="stack" style={{ gap: 6, maxWidth: 340, marginInlineStart: "auto" }}>
          <div className="row-between"><span>الإجمالي المستحق</span><b>{money(row.totalDue)}</b></div>
          <div className="row-between"><span>إجمالي الخصومات</span><span style={{ color: "var(--color-danger)" }}>{money(row.totalDeductions)}</span></div>
          <div className="row-between"><span>الدفعات العادية</span><span style={{ color: "var(--color-success)" }}>{money(row.normalPayments)}</span></div>
          <div className="row-between"><span>الدفعات المقدمة</span><span>{money(row.advancePayments)}</span></div>
          <div className="row-between" style={{ borderTop: "1px solid var(--border-base)", paddingTop: 6 }}><b>المتبقّي</b><b style={{ color: rem > 0 ? "var(--color-danger)" : "var(--color-success)" }}>{money(rem)}</b></div>
        </div>
        <div className="row-between mt-4" style={{ marginTop: "var(--space-12)" }}>
          <div className="col" style={{ gap: 24 }}><span className="muted">توقيع المورد</span><span>_______________</span></div>
          <div className="col" style={{ gap: 24, textAlign: "end" }}><span className="muted">توقيع الشركة</span><span>_______________</span></div>
        </div>
      </div>
    </Dialog>
  );
}
