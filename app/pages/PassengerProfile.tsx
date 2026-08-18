"use client";

import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import {
  getPassenger,
  addPassenger,
  updatePassenger,
  getMaritalStatusOptions,
  downloadPassengersExport,
  uploadPassengersExcel,
  fileToBase64,
  type PassengerInput,
} from "@/lib/services/passengers";
import { openFileUrl } from "@/lib/download";
import type { RouteOption } from "@/lib/services/passengers";
import PageHeader from "@/components/ui/PageHeader";
import { Field, Input, Select } from "@/components/ui/Field";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import AssignRoutesDialog from "@/components/passengers/AssignRoutesDialog";
import { IconChevronEnd, IconPin, IconPlus, IconUser, IconDownload, IconUpload } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function PassengerProfilePage() {
  const { id = "", locale = "ar" } = useParams();
  const { t } = useLocale();
  const { toast } = useToast();
  const navigate = useNavigate();
  const isNew = id === "new";
  const p = (path: string) => `/${locale}${path}`;

  const [name, setName] = useState("");
  const [identity, setIdentity] = useState("");
  const [mobile, setMobile] = useState("");
  const [maritalStatusId, setMaritalStatusId] = useState("");
  const [active, setActive] = useState(true);
  const [home, setHome] = useState<{ lat?: number; lng?: number }>({});
  const [photo, setPhoto] = useState<string>("");
  const [mapOpen, setMapOpen] = useState(false);
  const [assignOpen, setAssignOpen] = useState(false);
  const [maritalOptions, setMaritalOptions] = useState<RouteOption[]>([]);
  const [loading, setLoading] = useState(!isNew);
  const [saving, setSaving] = useState(false);
  const [title, setTitle] = useState(isNew ? "" : id);
  const [excelBusy, setExcelBusy] = useState(false);
  const photoRef = useRef<HTMLInputElement>(null);
  const excelRef = useRef<HTMLInputElement>(null);

  // ADD-via-Excel: export the current passengers (doubles as the fillable template).
  const onExportPassengers = async () => {
    setExcelBusy(true);
    try {
      openFileUrl(await downloadPassengersExport(), "passengers.xlsx");
    } catch (e) {
      toast(errMsg(e, t("pass.emptyList")), "error");
    } finally {
      setExcelBusy(false);
    }
  };

  // ADD-via-Excel: upload a file to create passengers, then go to the list.
  const onAddExcelPicked = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    e.target.value = "";
    if (!file) return;
    setExcelBusy(true);
    try {
      // A URL comes back only when some rows failed — it points at an error log.
      const { errorLogUrl } = await uploadPassengersExcel(file);
      toast(t("pass.fileUploaded"), errorLogUrl ? "info" : "success");
      if (errorLogUrl) openFileUrl(errorLogUrl);
      navigate(p("/passengers"));
    } catch (err) {
      toast(errMsg(err, t("pass.fileUploaded")), "error");
    } finally {
      setExcelBusy(false);
    }
  };

  // Read an image file, downscale it to a small square data-URI (kept small so it
  // fits in HrUser.imgPath and the JSON payload), and store it as the photo.
  const onPhotoPicked = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    e.target.value = "";
    if (!file) return;
    if (!file.type.startsWith("image/")) {
      toast(t("pass.photoInvalid"), "error");
      return;
    }
    const reader = new FileReader();
    reader.onload = () => {
      const img = new Image();
      img.onload = () => {
        const MAX = 256;
        const scale = Math.min(1, MAX / Math.max(img.width, img.height));
        const w = Math.round(img.width * scale);
        const h = Math.round(img.height * scale);
        const canvas = document.createElement("canvas");
        canvas.width = w;
        canvas.height = h;
        const ctx = canvas.getContext("2d");
        if (!ctx) {
          setPhoto(String(reader.result));
          return;
        }
        ctx.drawImage(img, 0, 0, w, h);
        setPhoto(canvas.toDataURL("image/jpeg", 0.85));
      };
      img.onerror = () => toast(t("pass.photoInvalid"), "error");
      img.src = String(reader.result);
    };
    reader.onerror = () => toast(t("pass.photoInvalid"), "error");
    reader.readAsDataURL(file);
  };

  // Load the marital-status dropdown options once.
  useEffect(() => {
    getMaritalStatusOptions().then(setMaritalOptions).catch(() => setMaritalOptions([]));
  }, []);

  // Load the existing passenger for edit mode (from the real backend, not mock).
  useEffect(() => {
    if (isNew) return;
    let cancelled = false;
    (async () => {
      try {
        const existing = await getPassenger(id);
        if (cancelled || !existing) return;
        setName(existing.name);
        setIdentity(existing.identityNumber);
        setMobile(existing.mobile);
        setMaritalStatusId(existing.maritalStatusId ?? "");
        setActive(existing.active);
        setHome({ lat: existing.homeLat, lng: existing.homeLng });
        setPhoto(existing.photo ?? "");
        setTitle(existing.name);
      } catch (e) {
        if (!cancelled) toast(errMsg(e, t("pass.emptyList")), "error");
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, [id, isNew, toast, t]);

  const save = async () => {
    if (!name || !identity || !mobile || !maritalStatusId) {
      toast(t("common.completeFieldsPlease"), "error");
      return;
    }
    const input: PassengerInput = { name, identityNumber: identity, mobile, maritalStatusId, active, lat: home.lat, lng: home.lng, photo: photo || undefined };
    setSaving(true);
    try {
      if (isNew) {
        await addPassenger(input);
        toast(t("pass.created"));
      } else {
        await updatePassenger(id, input);
        toast(t("pass.changesSaved"));
      }
      navigate(p("/passengers"));
    } catch (e) {
      toast(errMsg(e, t("common.completeFieldsPlease")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link to={p("/passengers")} style={{ color: "var(--color-brand)" }}>{t("nav.employees")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{isNew ? t("action.addNew") : title}</span>
      </div>

      <PageHeader title={isNew ? `${t("action.add")} ${t("nav.employees")}` : title}>
        <label className="checkbox-row"><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />{active ? t("status.active") : t("status.inactive")}</label>
        {isNew && <button className="btn btn-secondary btn-sm" onClick={onExportPassengers} disabled={excelBusy} title={t("pass.addExcelHint")}><IconDownload />{t("pass.exportList")}</button>}
        {isNew && <button className="btn btn-secondary btn-sm" onClick={() => excelRef.current?.click()} disabled={excelBusy} title={t("pass.addExcelHint")}><IconUpload />{t("pass.uploadAdd")}</button>}
        {isNew && <input ref={excelRef} type="file" accept=".xlsx,.xls" hidden onChange={onAddExcelPicked} />}
        {!isNew && <button className="btn btn-secondary btn-sm" onClick={() => setAssignOpen(true)}><IconPlus />{t("pass.assignRoutes")}</button>}
        <button className="btn btn-brand btn-sm" onClick={save} disabled={saving || loading}>{saving ? <span className="spinner" /> : t("action.save")}</button>
      </PageHeader>
      {isNew && <p className="field-hint" style={{ marginTop: "calc(-1 * var(--space-2))" }}>{t("pass.addExcelHint")}</p>}

      <div className="two-panel">
        <div className="card card-pad">
          <div className="section-title mb-4">{t("pass.personalInfo")}</div>
          <Field label={t("common.name")} required>
            <Input value={name} onChange={(e) => setName(e.target.value)} disabled={loading} />
          </Field>
          <Field label={t("pass.nationalId")} required>
            <Input value={identity} onChange={(e) => setIdentity(e.target.value)} disabled={loading} />
          </Field>
          <Field label={t("common.mobile")} required>
            <Input inputMode="numeric" value={mobile} onChange={(e) => setMobile(e.target.value.replace(/\D/g, ""))} disabled={loading} />
          </Field>
          <Field label={t("pass.otherIdentifier")} required>
            <Select value={maritalStatusId} onChange={(e) => setMaritalStatusId(e.target.value)} disabled={loading}>
              <option value="">{t("pass.choose")}</option>
              {maritalOptions.map((o) => (
                <option key={o.value} value={o.value}>{o.label}</option>
              ))}
            </Select>
          </Field>
        </div>

        <div className="stack">
          <div className="card card-pad col" style={{ alignItems: "center", gap: "var(--space-3)" }}>
            <div className="avatar" style={{ width: 96, height: 96, fontSize: 36, overflow: "hidden" }}>
              {photo ? (
                <img src={photo} alt={name || t("pass.uploadPhoto")} style={{ width: "100%", height: "100%", objectFit: "cover" }} />
              ) : (
                <IconUser style={{ width: 48, height: 48 }} />
              )}
            </div>
            <input ref={photoRef} type="file" accept="image/*" hidden onChange={onPhotoPicked} />
            <div className="row gap-2">
              <button type="button" className="btn btn-secondary btn-sm" onClick={() => photoRef.current?.click()}>{t("pass.uploadPhoto")}</button>
              {photo && <button type="button" className="btn btn-secondary btn-sm" onClick={() => setPhoto("")}>{t("action.remove")}</button>}
            </div>
          </div>
          <div className="card card-pad">
            <div className="section-title mb-4">{t("pass.homeLocation")}</div>
            <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
              <Field label={t("pass.latitude")} className="grow" style={{ margin: 0, minWidth: 110 }}>
                <Input value={home.lat ?? ""} readOnly placeholder="—" />
              </Field>
              <Field label={t("pass.longitude")} className="grow" style={{ margin: 0, minWidth: 110 }}>
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
      {assignOpen && <AssignRoutesDialog passengerId={id} onClose={() => setAssignOpen(false)} />}
    </div>
  );
}
