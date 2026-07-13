"use client";

import { use, useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getPassenger } from "@/lib/data";
import PageHeader from "@/components/ui/PageHeader";
import { Field, Input, Select } from "@/components/ui/Field";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import AssignRoutesDialog from "@/components/passengers/AssignRoutesDialog";
import { IconChevronEnd, IconPin, IconPlus, IconUser } from "@/components/ui/Icons";

export default function PassengerProfilePage({ params }: { params: Promise<{ id: string; locale: string }> }) {
  const { id, locale } = use(params);
  const { t } = useLocale();
  const { toast } = useToast();
  const isNew = id === "new";
  const existing = isNew ? undefined : getPassenger(id);
  const p = (path: string) => `/${locale}${path}`;

  const [name, setName] = useState(existing?.name ?? "");
  const [identity, setIdentity] = useState(existing?.identityNumber ?? "");
  const [mobile, setMobile] = useState(existing?.mobile ?? "");
  const [identifier, setIdentifier] = useState(existing?.identifier ?? "");
  const [active, setActive] = useState(existing?.active ?? true);
  const [home, setHome] = useState<{ lat?: number; lng?: number }>({ lat: existing?.homeLat, lng: existing?.homeLng });
  const [mapOpen, setMapOpen] = useState(false);
  const [assignOpen, setAssignOpen] = useState(false);

  const save = () => {
    if (!name || !identity || !mobile || !identifier) {
      toast("يرجى إكمال الحقول المطلوبة", "error");
      return;
    }
    toast(isNew ? "تم إنشاء الراكب" : "تم حفظ التعديلات");
  };

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link href={p("/passengers")} style={{ color: "var(--color-brand)" }}>{t("nav.employees")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{isNew ? t("action.addNew") : existing?.name ?? id}</span>
      </div>

      <PageHeader title={isNew ? `${t("action.add")} ${t("nav.employees")}` : existing?.name ?? ""}>
        <label className="checkbox-row"><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />{active ? t("status.active") : t("status.inactive")}</label>
        {!isNew && <button className="btn btn-secondary btn-sm" onClick={() => setAssignOpen(true)}><IconPlus />تعيين مسارات</button>}
        <button className="btn btn-brand btn-sm" onClick={save}>{t("action.save")}</button>
      </PageHeader>

      <div className="two-panel">
        <div className="card card-pad">
          <div className="section-title mb-4">المعلومات الشخصية</div>
          <Field label="الاسم" required>
            <Input value={name} onChange={(e) => setName(e.target.value)} />
          </Field>
          <Field label="الرقم القومي" required>
            <Input value={identity} onChange={(e) => setIdentity(e.target.value)} />
          </Field>
          <Field label="الجوال" required>
            <Input inputMode="numeric" value={mobile} onChange={(e) => setMobile(e.target.value.replace(/\D/g, ""))} />
          </Field>
          <Field label="معرّف آخر" required>
            <Select value={identifier} onChange={(e) => setIdentifier(e.target.value)}>
              <option value="">اختر</option>
              <option value="أعزب">أعزب</option>
              <option value="متزوج">متزوج</option>
              <option value="متزوجة">متزوجة</option>
            </Select>
          </Field>
        </div>

        <div className="stack">
          <div className="card card-pad col" style={{ alignItems: "center", gap: "var(--space-3)" }}>
            <div className="avatar" style={{ width: 96, height: 96, fontSize: 36 }}>
              <IconUser style={{ width: 48, height: 48 }} />
            </div>
            <button className="btn btn-secondary btn-sm">رفع صورة</button>
          </div>
          <div className="card card-pad">
            <div className="section-title mb-4">موقع المنزل</div>
            <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
              <Field label="خط العرض" className="grow" style={{ margin: 0, minWidth: 110 }}>
                <Input value={home.lat ?? ""} readOnly placeholder="—" />
              </Field>
              <Field label="خط الطول" className="grow" style={{ margin: 0, minWidth: 110 }}>
                <Input value={home.lng ?? ""} readOnly placeholder="—" />
              </Field>
              <button className="btn btn-outline-brand" onClick={() => setMapOpen(true)}><IconPin />{t("action.pickLocation")}</button>
            </div>
          </div>
        </div>
      </div>

      {mapOpen && (
        <MapPickerDialog initial={home} onPick={(c) => { setHome(c); setMapOpen(false); }} onClose={() => setMapOpen(false)} />
      )}
      {assignOpen && <AssignRoutesDialog onClose={() => setAssignOpen(false)} />}
    </div>
  );
}
