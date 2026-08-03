"use client";

import { useState, useId, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useNavigate, useParams } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { suppliers } from "@/lib/data";
import { addSupplier, getSupplier as fetchSupplier, updateSupplier } from "@/lib/services/suppliers";
import type { ContactPerson } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import { IconChevronEnd, IconBuilding, IconPlus, IconTrash } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

type CheckState = "unchecked" | "unique" | "duplicate";

function checkDuplicate(field: "name" | "phone" | "mobile" | "email" | "fax", value: string, selfId: string): CheckState {
  if (!value.trim()) return "unchecked";
  const dup = suppliers.some((s) => s.id !== selfId && (s[field] ?? "") === value);
  return dup ? "duplicate" : "unique";
}

function CheckDot({ state }: { state: CheckState }) {
  const { t } = useLocale();
  const map: Record<CheckState, { cls: string; label: string }> = {
    unchecked: { cls: "dot-gray", label: t("supp.notChecked") },
    unique: { cls: "dot-green", label: t("supp.unique") },
    duplicate: { cls: "dot-red", label: t("supp.duplicate") },
  };
  return <span className="row" style={{ gap: 4 }}><span className={`dot ${map[state].cls}`} /><span className="text-xs muted">{map[state].label}</span></span>;
}

export default function SupplierProfilePage() {
  const { id = "", locale = "ar" } = useParams();
  const navigate = useNavigate();
  const { t } = useLocale();
  const { toast } = useToast();
  const isNew = id === "new";
  const p = (path: string) => `/${locale}${path}`;

  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [mobile, setMobile] = useState("");
  const [email, setEmail] = useState("");
  const [fax, setFax] = useState("");
  const [address, setAddress] = useState("");
  const [contacts, setContacts] = useState<ContactPerson[]>([]);

  // Load the existing supplier from the backend (getSupplier).
  const loadSupplier = useCallback(async () => {
    if (isNew) return;
    try {
      const s = await fetchSupplier(id);
      if (s) {
        setName(s.name); setPhone(s.phone ?? ""); setMobile(s.mobile ?? "");
        setEmail(s.email ?? ""); setFax(s.fax ?? ""); setAddress(s.address ?? "");
        setContacts(s.contacts);
      }
    } catch (e) {
      toast(errMsg(e, t("supp.nameRequired")), "error");
    }
  }, [id, isNew, toast, t]);
  useEffect(() => { loadSupplier(); }, [loadSupplier]);
  const nameId = useId();
  const phoneId = useId();
  const mobileId = useId();
  const emailId = useId();
  const faxId = useId();
  const addressId = useId();

  const addContact = () => setContacts((c) => [...c, { id: `c${Date.now()}`, name: "", mobile: "" }]);
  const patchContact = (cid: string, field: "name" | "mobile", v: string) =>
    setContacts((cs) => cs.map((c) => (c.id === cid ? { ...c, [field]: v } : c)));

  const [saving, setSaving] = useState(false);
  const save = async () => {
    if (!name.trim()) { toast(t("supp.nameRequired"), "error"); return; }
    setSaving(true);
    try {
      if (isNew) {
        await addSupplier({ name, email, phone, mobile, fax, address, contacts });
        toast(t("supp.created"));
        navigate(p("/suppliers"));
      } else {
        await updateSupplier(id, { name, email, phone, mobile, fax, address, contacts });
        toast(t("supp.changesSaved"));
        await loadSupplier(); // refresh so new contacts pick up their real ids
      }
    } catch (e) {
      toast(errMsg(e, t("supp.nameRequired")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link to={p("/suppliers")} style={{ color: "var(--color-brand)" }}>{t("nav.suppliers")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{isNew ? t("action.addNew") : name || id}</span>
      </div>

      <PageHeader title={isNew ? `${t("action.add")} ${t("nav.suppliers")}` : name}>
        <button className="btn btn-brand btn-sm" onClick={save} disabled={saving}>{t("action.save")}</button>
      </PageHeader>

      <div className="two-panel">
        <div className="card card-pad">
          <div className="section-title mb-4">{t("supp.data")}</div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={nameId}>{t("supp.name")} <span className="req" aria-hidden>*</span></label><CheckDot state={checkDuplicate("name", name, id)} /></div>
            <input id={nameId} aria-required className={`input${checkDuplicate("name", name, id) === "unique" ? " input--success" : ""}`} value={name} onChange={(e) => setName(e.target.value)} />
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={phoneId}>{t("common.phone")}</label><CheckDot state={checkDuplicate("phone", phone, id)} /></div>
            <input id={phoneId} className={`input${checkDuplicate("phone", phone, id) === "unique" ? " input--success" : ""}`} inputMode="numeric" value={phone} onChange={(e) => setPhone(e.target.value.replace(/\D/g, ""))} />
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={mobileId}>{t("common.mobile")}</label><CheckDot state={checkDuplicate("mobile", mobile, id)} /></div>
            <div className="row gap-2">
              <span className="input" style={{ width: 70, textAlign: "center", background: "var(--gray-50)" }} aria-hidden>+20</span>
              <input id={mobileId} className={`input${checkDuplicate("mobile", mobile, id) === "unique" ? " input--success" : ""}`} inputMode="numeric" value={mobile} onChange={(e) => setMobile(e.target.value.replace(/\D/g, ""))} />
            </div>
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={emailId}>{t("login.email")}</label><CheckDot state={checkDuplicate("email", email, id)} /></div>
            <input id={emailId} type="email" className={`input${checkDuplicate("email", email, id) === "unique" ? " input--success" : ""}`} value={email} onChange={(e) => setEmail(e.target.value)} />
          </div>
          <div className="field">
            <div className="row-between"><label className="label" htmlFor={faxId}>{t("supp.fax")}</label><CheckDot state={checkDuplicate("fax", fax, id)} /></div>
            <input id={faxId} className={`input${checkDuplicate("fax", fax, id) === "unique" ? " input--success" : ""}`} inputMode="numeric" value={fax} onChange={(e) => setFax(e.target.value.replace(/\D/g, ""))} />
          </div>
          <div className="field">
            <label className="label" htmlFor={addressId}>{t("supp.address")}</label>
            <textarea id={addressId} className="textarea" rows={2} value={address} onChange={(e) => setAddress(e.target.value)} />
          </div>
        </div>

        <div className="stack">
          <div className="card card-pad col" style={{ alignItems: "center", gap: "var(--space-3)" }}>
            <div className="kpi-icon" style={{ width: 96, height: 96, borderRadius: "var(--radius-lg)" }}><IconBuilding style={{ width: 44, height: 44 }} /></div>
            <button className="btn btn-secondary btn-sm">{t("supp.uploadLogo")}</button>
          </div>

          <div className="card card-pad">
            <div className="row-between mb-4">
              <div className="section-title">{t("supp.contacts")}</div>
              <button className="btn btn-outline-brand btn-sm" onClick={addContact}><IconPlus />{t("action.add")}</button>
            </div>
            <p className="field-hint mb-4">{t("supp.contactsHint")}</p>
            <div className="stack" style={{ gap: "var(--space-2)" }}>
              {contacts.length === 0 && <span className="muted text-sm">{t("supp.noContacts")}</span>}
              {contacts.map((c) => (
                <div key={c.id} className="row gap-2">
                  <input className="input" placeholder={t("common.name")} aria-label={t("supp.contactName")} value={c.name} onChange={(e) => patchContact(c.id, "name", e.target.value)} />
                  <input className="input" placeholder={t("common.mobile")} aria-label={t("supp.contactMobile")} inputMode="numeric" value={c.mobile} onChange={(e) => patchContact(c.id, "mobile", e.target.value.replace(/\D/g, ""))} />
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
