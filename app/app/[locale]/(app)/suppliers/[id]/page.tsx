"use client";

import { use, useState, useId } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getSupplier, suppliers } from "@/lib/data";
import type { ContactPerson } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import { IconChevronEnd, IconBuilding, IconPlus, IconTrash } from "@/components/ui/Icons";

type CheckState = "unchecked" | "unique" | "duplicate";

function checkDuplicate(field: "name" | "phone" | "mobile", value: string, selfId: string): CheckState {
  if (!value.trim()) return "unchecked";
  const dup = suppliers.some((s) => s.id !== selfId && (s[field] ?? "") === value);
  return dup ? "duplicate" : "unique";
}

function CheckDot({ state }: { state: CheckState }) {
  const map: Record<CheckState, { cls: string; label: string }> = {
    unchecked: { cls: "dot-gray", label: "لم يُفحص" },
    unique: { cls: "dot-green", label: "فريد" },
    duplicate: { cls: "dot-red", label: "مكرر" },
  };
  return <span className="row" style={{ gap: 4 }}><span className={`dot ${map[state].cls}`} /><span className="text-xs muted">{map[state].label}</span></span>;
}

export default function SupplierProfilePage({ params }: { params: Promise<{ id: string; locale: string }> }) {
  const { id, locale } = use(params);
  const { t } = useLocale();
  const { toast } = useToast();
  const isNew = id === "new";
  const existing = isNew ? undefined : getSupplier(id);
  const p = (path: string) => `/${locale}${path}`;

  const [name, setName] = useState(existing?.name ?? "");
  const [phone, setPhone] = useState(existing?.phone ?? "");
  const [mobile, setMobile] = useState(existing?.mobile ?? "");
  const [contacts, setContacts] = useState<ContactPerson[]>(existing?.contacts ?? []);
  const nameId = useId();
  const phoneId = useId();
  const mobileId = useId();

  const addContact = () => setContacts((c) => [...c, { id: `c${Date.now()}`, name: "", mobile: "" }]);
  const patchContact = (cid: string, field: "name" | "mobile", v: string) =>
    setContacts((cs) => cs.map((c) => (c.id === cid ? { ...c, [field]: v } : c)));

  const save = () => {
    if (!name.trim()) { toast("اسم المورد مطلوب", "error"); return; }
    toast(isNew ? "تم إنشاء المورد" : "تم حفظ التعديلات");
  };

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link href={p("/suppliers")} style={{ color: "var(--color-brand)" }}>{t("nav.suppliers")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{isNew ? t("action.addNew") : existing?.name ?? id}</span>
      </div>

      <PageHeader title={isNew ? `${t("action.add")} ${t("nav.suppliers")}` : existing?.name ?? ""}>
        <button className="btn btn-brand btn-sm" onClick={save}>{t("action.save")}</button>
      </PageHeader>

      <div className="two-panel">
        <div className="card card-pad">
          <div className="section-title mb-4">بيانات المورد</div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={nameId}>اسم المورد <span className="req" aria-hidden>*</span></label><CheckDot state={checkDuplicate("name", name, id)} /></div>
            <input id={nameId} aria-required className={`input${checkDuplicate("name", name, id) === "unique" ? " input--success" : ""}`} value={name} onChange={(e) => setName(e.target.value)} />
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={phoneId}>الهاتف</label><CheckDot state={checkDuplicate("phone", phone, id)} /></div>
            <input id={phoneId} className={`input${checkDuplicate("phone", phone, id) === "unique" ? " input--success" : ""}`} inputMode="numeric" value={phone} onChange={(e) => setPhone(e.target.value.replace(/\D/g, ""))} />
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={mobileId}>الجوال</label><CheckDot state={checkDuplicate("mobile", mobile, id)} /></div>
            <div className="row gap-2">
              <span className="input" style={{ width: 70, textAlign: "center", background: "var(--gray-50)" }} aria-hidden>+20</span>
              <input id={mobileId} className={`input${checkDuplicate("mobile", mobile, id) === "unique" ? " input--success" : ""}`} inputMode="numeric" value={mobile} onChange={(e) => setMobile(e.target.value.replace(/\D/g, ""))} />
            </div>
          </div>
        </div>

        <div className="stack">
          <div className="card card-pad col" style={{ alignItems: "center", gap: "var(--space-3)" }}>
            <div className="kpi-icon" style={{ width: 96, height: 96, borderRadius: "var(--radius-lg)" }}><IconBuilding style={{ width: 44, height: 44 }} /></div>
            <button className="btn btn-secondary btn-sm">رفع الشعار (مربّع)</button>
          </div>

          <div className="card card-pad">
            <div className="row-between mb-4">
              <div className="section-title">جهات الاتصال (السائقون)</div>
              <button className="btn btn-outline-brand btn-sm" onClick={addContact}><IconPlus />إضافة</button>
            </div>
            <p className="field-hint mb-4">تظهر جهات الاتصال كسائقين على المسارات.</p>
            <div className="stack" style={{ gap: "var(--space-2)" }}>
              {contacts.length === 0 && <span className="muted text-sm">لا توجد جهات اتصال.</span>}
              {contacts.map((c) => (
                <div key={c.id} className="row gap-2">
                  <input className="input" placeholder="الاسم" aria-label="اسم جهة الاتصال" value={c.name} onChange={(e) => patchContact(c.id, "name", e.target.value)} />
                  <input className="input" placeholder="الجوال" aria-label="جوال جهة الاتصال" inputMode="numeric" value={c.mobile} onChange={(e) => patchContact(c.id, "mobile", e.target.value.replace(/\D/g, ""))} />
                  <button className="icon-btn danger" aria-label={t("action.delete")} onClick={() => setContacts((cs) => cs.filter((x) => x.id !== c.id))}><IconTrash /></button>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
