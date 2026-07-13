"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { repricings as seed, lines } from "@/lib/data";
import type { Repricing } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { Field, Input } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus, IconEye, IconAlert, IconPercent, IconMoney } from "@/components/ui/Icons";

export default function RepricingPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const canCreate = can(user?.role, "create.repricing");
  const canApprove = can(user?.role, "approve.repricing");
  const canReject = can(user?.role, "reject.repricing");

  const [rows, setRows] = useState<Repricing[]>(seed);
  const [creating, setCreating] = useState(false);
  const [review, setReview] = useState<Repricing | null>(null);
  const [confirmApprove, setConfirmApprove] = useState<Repricing | null>(null);

  const pendingCount = rows.filter((r) => !r.approved).length;

  const columns: Column<Repricing>[] = [
    { key: "amount", header: "مقدار الزيادة", render: (r) => <b>{r.amount}{r.mode === "percent" ? "%" : " ج.م"}</b> },
    { key: "createdBy", header: "أنشئ بواسطة" },
    { key: "createdAt", header: "تاريخ الإنشاء" },
    { key: "forAllLines", header: "كل الخطوط", render: (r) => (r.forAllLines ? <span className="badge badge-blue">نعم</span> : <span className="badge badge-gray">محدّد</span>) },
    { key: "approved", header: "الاعتماد", render: (r) => (r.approved ? <span className="badge badge-green">{t("status.approved")}</span> : <span className="badge badge-amber">{t("status.pending")}</span>) },
    { key: "approvedBy", header: "المعتمد", render: (r) => r.approvedBy ?? <span className="muted">—</span> },
    { key: "actions", header: "", align: "end", render: (r) => <button className="icon-btn brand" aria-label={t("action.view")} onClick={() => setReview(r)}><IconEye /></button> },
  ];

  const approve = (r: Repricing) => { setRows((rs) => rs.map((x) => (x.id === r.id ? { ...x, approved: true, approvedBy: user?.name } : x))); toast("تم اعتماد إعادة التسعير وتطبيق الأسعار"); setReview(null); };
  const reject = (r: Repricing) => { setRows((rs) => rs.filter((x) => x.id !== r.id)); toast("تم رفض إعادة التسعير", "info"); setReview(null); };

  return (
    <div className="stack">
      <PageHeader title={t("nav.repricing")} count={rows.length}>
        {canCreate && <button className="btn btn-brand btn-sm" onClick={() => setCreating(true)}><IconPlus />إنشاء إعادة تسعير</button>}
      </PageHeader>

      {pendingCount > 0 && (
        <div className="banner-pending"><IconAlert style={{ width: 18, height: 18 }} />يوجد {pendingCount} طلب إعادة تسعير بانتظار الاعتماد — لن تتغيّر الأسعار قبل الاعتماد.</div>
      )}

      <DataTable columns={columns} rows={rows} emptyMessage={t("empty.repricing")} />

      {creating && <CreateRepricing onClose={() => setCreating(false)} onCreated={(r) => setRows((rs) => [r, ...rs])} />}

      {review && (
        <Dialog
          title="مراجعة إعادة التسعير"
          size="lg"
          onClose={() => setReview(null)}
          footer={
            !review.approved && (canApprove || canReject) ? (
              <>
                {canReject && <button className="btn btn-danger" style={{ marginInlineEnd: "auto" }} onClick={() => reject(review)}>{t("action.reject")}</button>}
                <button className="btn btn-secondary" onClick={() => setReview(null)}>{t("action.close")}</button>
                {canApprove && <button className="btn btn-success" onClick={() => setConfirmApprove(review)}>{t("action.approve")}</button>}
              </>
            ) : (
              <button className="btn btn-secondary" onClick={() => setReview(null)}>{t("action.close")}</button>
            )
          }
        >
          {!review.approved && <div className="banner-pending mb-4"><IconAlert style={{ width: 18, height: 18 }} />هذا الطلب قيد الاعتماد — الأسعار الحالية لم تتغيّر بعد.</div>}
          {review.forAllLines ? (
            <p style={{ lineHeight: "var(--leading-6)" }}>سيتم زيادة أسعار <b>جميع خطوط النقل</b> بمقدار <b style={{ color: "var(--color-brand)" }}>{review.amount}{review.mode === "percent" ? "%" : " ج.م"}</b> اعتبارًا من {review.startDate}.</p>
          ) : (
            <div className="table-wrap">
              <table className="data">
                <thead><tr><th>الخط</th><th>قبل</th><th>بعد</th></tr></thead>
                <tbody>
                  {review.lines.map((l, i) => (
                    <tr key={i}><td>{l.lineName}</td><td style={{ color: "var(--color-danger)" }}>{l.before} ج.م</td><td style={{ color: "var(--color-success)" }}>{l.after} ج.م</td></tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </Dialog>
      )}

      {confirmApprove && (
        <ConfirmDialog
          title="تأكيد الاعتماد"
          tone="success"
          message={confirmApprove.forAllLines
            ? `سيتم زيادة أسعار جميع الخطوط بمقدار ${confirmApprove.amount}${confirmApprove.mode === "percent" ? "%" : " ج.م"} اعتبارًا من ${confirmApprove.startDate}.`
            : `سيتم تحديث أسعار ${confirmApprove.lines.length} خط اعتبارًا من ${confirmApprove.startDate}.`}
          confirmLabel={t("action.approve")}
          onConfirm={() => approve(confirmApprove)}
          onClose={() => setConfirmApprove(null)}
        />
      )}
    </div>
  );
}

function CreateRepricing({ onClose, onCreated }: { onClose: () => void; onCreated: (r: Repricing) => void }) {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [scope, setScope] = useState<"all" | "selected">("all");
  const [mode, setMode] = useState<"percent" | "fixed">("percent");
  const [round5, setRound5] = useState(false);
  const [amount, setAmount] = useState("");
  const [startDate, setStartDate] = useState("");
  const [selected, setSelected] = useState<string[]>([]);
  const [confirm, setConfirm] = useState(false);

  const toggleLine = (id: string) => setSelected((s) => (s.includes(id) ? s.filter((x) => x !== id) : [...s, id]));

  const preview = (before: number) => {
    let after = mode === "percent" ? before * (1 + Number(amount) / 100) : before + Number(amount);
    if (round5) after = Math.round(after / 5) * 5;
    return Math.round(after);
  };

  const create = () => {
    if (!amount) { toast("أدخل المقدار", "error"); return; }
    if (scope === "selected" && selected.length === 0) { toast("اختر خطًا واحدًا على الأقل", "error"); return; }
    const chosen = scope === "all" ? lines : lines.filter((l) => selected.includes(l.id));
    onCreated({
      id: `rp${Date.now()}`,
      amount: Number(amount), mode, forAllLines: scope === "all",
      createdBy: user?.name ?? "—", createdAt: "2026-07-09", approved: false, startDate: startDate || "2026-08-01",
      lines: chosen.map((l) => { const before = 800; return { lineName: l.name, before, after: preview(before) }; }),
    });
    toast("تم إنشاء طلب إعادة التسعير (غير معتمد)");
    onClose();
  };

  return (
    <>
      <Dialog
        title="إنشاء إعادة تسعير"
        size="lg"
        onClose={onClose}
        footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-brand" onClick={() => setConfirm(true)}>{t("action.save")}</button></>}
      >
        <div className="two-panel">
          <div className="field">
            <span className="label" id="rp-scope-label">النطاق</span>
            <SegmentedControl<"all" | "selected">
              value={scope}
              onChange={setScope}
              ariaLabelledby="rp-scope-label"
              segments={[
                { value: "all", label: "كل الخطوط" },
                { value: "selected", label: "خطوط محدّدة" },
              ]}
            />
          </div>
          <div className="field">
            <span className="label" id="rp-mode-label">الطريقة</span>
            <SegmentedControl<"percent" | "fixed">
              value={mode}
              onChange={setMode}
              ariaLabelledby="rp-mode-label"
              segments={[
                { value: "percent", label: <><IconPercent style={{ width: 14, height: 14 }} /> نسبة %</> },
                { value: "fixed", label: <><IconMoney style={{ width: 14, height: 14 }} /> مبلغ ثابت</> },
              ]}
            />
          </div>
        </div>
        <div className="two-panel">
          <Field label="المقدار" required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} placeholder={mode === "percent" ? "10" : "50"} /></Field>
          <Field label="تاريخ التطبيق"><Input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} /></Field>
        </div>
        <label className="checkbox-row mb-4"><input type="checkbox" checked={round5} onChange={(e) => setRound5(e.target.checked)} />تقريب الناتج لأقرب ٥ (مثال: {preview(823)})</label>

        {scope === "selected" && (
          <div className="card card-pad">
            <div className="section-title mb-4">اختر الخطوط</div>
            <div className="row wrap gap-2">
              {lines.map((l) => (
                <label key={l.id} className="checkbox-row info-chip" style={{ padding: "6px 10px", cursor: "pointer" }}>
                  <input type="checkbox" checked={selected.includes(l.id)} onChange={() => toggleLine(l.id)} />{l.name}
                </label>
              ))}
            </div>
          </div>
        )}
      </Dialog>

      {confirm && (
        <ConfirmDialog
          title="تأكيد إنشاء إعادة التسعير"
          tone="brand"
          message="هذا الإجراء يغيّر أسعار العقود (بعد الاعتماد). هل تريد المتابعة؟"
          confirmLabel={t("action.confirm")}
          onConfirm={create}
          onClose={() => setConfirm(false)}
        />
      )}
    </>
  );
}
