"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import type { Station } from "@/lib/types";
import Dialog from "@/components/ui/Dialog";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import { Field, Input } from "@/components/ui/Field";
import { IconPin } from "@/components/ui/Icons";

export default function StationForm({
  routeId,
  existing,
  onClose,
  onSaved,
}: {
  routeId: string;
  existing?: Station;
  onClose: () => void;
  onSaved: (s: Station) => void;
}) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [name, setName] = useState(existing?.name ?? "");
  const [description, setDescription] = useState(existing?.description ?? "");
  const [active, setActive] = useState(existing?.active ?? true);
  const [lat, setLat] = useState<number | undefined>(existing?.lat);
  const [lng, setLng] = useState<number | undefined>(existing?.lng);
  const [error, setError] = useState(false);
  const [mapOpen, setMapOpen] = useState(false);

  const submit = () => {
    if (!name.trim()) { setError(true); return; }
    onSaved({ id: existing?.id ?? `st${Date.now()}`, routeId, name, description, active, lat, lng });
    toast(existing ? "تم تحديث المحطة" : "تمت إضافة المحطة");
    onClose();
  };

  return (
    <>
      <Dialog
        title={existing ? "تعديل المحطة" : "إضافة محطة"}
        onClose={onClose}
        footer={
          <>
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submit}>{t("action.save")}</button>
          </>
        }
      >
        <Field label="اسم المحطة" required error={error ? t("common_required") : undefined}>
          <Input value={name} onChange={(e) => setName(e.target.value)} autoFocus />
        </Field>
        <Field label="الوصف / الوقت">
          <Input value={description} onChange={(e) => setDescription(e.target.value)} placeholder="٧:٠٠ ص" />
        </Field>
        <label className="checkbox-row mb-4">
          <input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />
          <span>{t("status.active")}</span>
        </label>
        <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
          <Field label="خط العرض" className="grow" style={{ margin: 0, minWidth: 120 }}>
            <Input value={lat ?? ""} readOnly placeholder="—" />
          </Field>
          <Field label="خط الطول" className="grow" style={{ margin: 0, minWidth: 120 }}>
            <Input value={lng ?? ""} readOnly placeholder="—" />
          </Field>
          <button className="btn btn-outline-brand" onClick={() => setMapOpen(true)}><IconPin />{t("action.pickLocation")}</button>
        </div>
      </Dialog>

      {mapOpen && (
        <MapPickerDialog
          initial={{ lat, lng }}
          onPick={(c) => { setLat(c.lat); setLng(c.lng); setMapOpen(false); }}
          onClose={() => setMapOpen(false)}
        />
      )}
    </>
  );
}
