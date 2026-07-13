"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { canAdd } from "@/lib/types";
import { exceptions as seed, routes, passengers, getStations } from "@/lib/data";
import type { Exception, Period } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import { Field, Input, Textarea } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus, IconEdit } from "@/components/ui/Icons";

const periodLabel: Record<Period, string> = { go: "period.go", return: "period.return", both: "period.both" };

export default function ExceptionsPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const [rows, setRows] = useState<Exception[]>(seed);
  const [formOpen, setFormOpen] = useState(false);

  const columns: Column<Exception>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "passenger", header: "الراكب", render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.passenger}</b> },
    { key: "routeName", header: t("filter.route") },
    { key: "station", header: "المحطة", priority: "secondary" },
    { key: "date", header: "التاريخ", render: (r) => r.date || `${r.fromDate} → ${r.toDate}` },
    { key: "period", header: "الاتجاه", render: (r) => <span className="badge badge-blue">{t(periodLabel[r.period])}</span> },
    { key: "reason", header: "السبب", priority: "secondary" },
    { key: "contact", header: "رقم التواصل", priority: "secondary" },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.exceptions")} count={rows.length}>
        {canAdd(user?.role) && <button className="btn btn-brand btn-sm" onClick={() => setFormOpen(true)}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        emptyMessage="لا توجد استثناءات مسجّلة."
        emptyAction={canAdd(user?.role) ? <button className="btn btn-brand btn-sm" onClick={() => setFormOpen(true)}><IconPlus />{t("action.addNew")}</button> : undefined}
      />

      {formOpen && <ExceptionForm onClose={() => setFormOpen(false)} onSaved={(e) => setRows((r) => [...r, e])} />}
    </div>
  );
}

function ExceptionForm({ onClose, onSaved }: { onClose: () => void; onSaved: (e: Exception) => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [passenger, setPassenger] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  const [type, setType] = useState<"single" | "period">("single");
  const [locType, setLocType] = useState<"station" | "free">("station");
  const [station, setStation] = useState<string | undefined>();
  const [period, setPeriod] = useState<Period>("go");
  const [reason, setReason] = useState("");
  const [contact, setContact] = useState("");
  const [date, setDate] = useState("");
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [coords, setCoords] = useState<{ lat?: number; lng?: number }>({});
  const [mapOpen, setMapOpen] = useState(false);
  const [capOpen, setCapOpen] = useState(false);
  const [errors, setErrors] = useState<Record<string, boolean>>({});

  const stationOptions = route ? getStations(route).map((s) => ({ value: s.id, label: s.name })) : [];

  const submit = () => {
    const e: Record<string, boolean> = {};
    if (!passenger) e.passenger = true;
    if (!route) e.route = true;
    if (!reason.trim()) e.reason = true;
    if (!contact.trim()) e.contact = true;
    setErrors(e);
    if (Object.keys(e).length) return;

    onSaved({
      id: `e${Date.now()}`,
      routeName: routes.find((r) => r.id === route)?.name ?? "",
      passenger: passengers.find((p) => p.id === passenger)?.name ?? "",
      station: locType === "station" ? getStations(route!).find((s) => s.id === station)?.name ?? "" : "موقع حر",
      lat: coords.lat, lng: coords.lng,
      date: type === "single" ? date : "",
      fromDate: type === "period" ? fromDate : undefined,
      toDate: type === "period" ? toDate : undefined,
      period, reason, contact,
    });
    toast("تمت إضافة الاستثناء");
    onClose();
  };

  return (
    <>
      <Dialog
        title="إضافة استثناء"
        size="lg"
        onClose={onClose}
        footer={
          <>
            <button className="btn btn-secondary" style={{ marginInlineEnd: "auto" }} disabled={!route || !period} onClick={() => setCapOpen(true)}>سعة المسار</button>
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submit}>{t("action.save")}</button>
          </>
        }
      >
        <div className="two-panel">
          <Field label="الراكب" required error={errors.passenger ? "يرجى اختيار راكب" : undefined}>
            <Combobox options={passengers.map((p) => ({ value: p.id, label: p.name }))} value={passenger} onChange={setPassenger} placeholder="اختر راكبًا" />
          </Field>
          <Field label={t("filter.route")} required error={errors.route ? "يرجى اختيار مسار" : undefined}>
            <Combobox options={routes.map((r) => ({ value: r.id, label: r.name }))} value={route} onChange={(v) => { setRoute(v); setStation(undefined); }} placeholder="اختر مسارًا" />
          </Field>
        </div>

        {/* TYPE radio group */}
        <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
          <legend className="label" style={{ padding: 0 }}>النوع</legend>
          <div className="row gap-4 wrap">
            <label className="checkbox-row"><input type="radio" name="exc-type" checked={type === "single"} onChange={() => setType("single")} />تاريخ استثناء واحد</label>
            <label className="checkbox-row"><input type="radio" name="exc-type" checked={type === "period"} onChange={() => setType("period")} />فترة</label>
          </div>
          <div className="row gap-3 wrap mt-4">
            {type === "single" ? (
              <input className="input" type="date" aria-label="تاريخ الاستثناء" value={date} onChange={(e) => setDate(e.target.value)} style={{ maxWidth: 200 }} />
            ) : (
              <>
                <input className="input" type="date" aria-label="من تاريخ" value={fromDate} onChange={(e) => setFromDate(e.target.value)} style={{ maxWidth: 180 }} />
                <input className="input" type="date" aria-label="إلى تاريخ" value={toDate} onChange={(e) => setToDate(e.target.value)} style={{ maxWidth: 180 }} />
                <input className="input" aria-label="أيام الأسبوع" placeholder="أيام الأسبوع" style={{ maxWidth: 180 }} />
              </>
            )}
          </div>
        </fieldset>

        {/* LOCATION radio group */}
        <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
          <legend className="label" style={{ padding: 0 }}>الموقع</legend>
          <div className="row gap-4 wrap">
            <label className="checkbox-row"><input type="radio" name="exc-loc" checked={locType === "station"} onChange={() => setLocType("station")} />محطة</label>
            <label className="checkbox-row"><input type="radio" name="exc-loc" checked={locType === "free"} onChange={() => setLocType("free")} />موقع حر</label>
          </div>
          <div className="mt-4">
            {locType === "station" ? (
              <Combobox options={stationOptions} value={station} onChange={setStation} placeholder="اختر محطة" disabled={!route} disabledReason="اختر مسارًا أولًا" />
            ) : (
              <button className="btn btn-outline-brand" onClick={() => setMapOpen(true)}>{coords.lat != null ? `${coords.lat}, ${coords.lng}` : t("action.pickLocation")}</button>
            )}
          </div>
        </fieldset>

        {/* PERIOD */}
        <div className="field">
          <span className="label" id="exc-period-label">الاتجاه <span className="req" aria-hidden>*</span></span>
          <SegmentedControl<Period>
            value={period}
            onChange={setPeriod}
            ariaLabelledby="exc-period-label"
            segments={[
              { value: "go", label: t("period.go") },
              { value: "return", label: t("period.return") },
              { value: "both", label: t("period.both") },
            ]}
          />
          {period === "both" && <p className="field-hint mt-4">اتجاه "ذهاب وعودة" يتطلب تحديد نقطة نزول إضافية.</p>}
        </div>

        <div className="two-panel">
          <Field label="السبب" required error={errors.reason ? t("common_required") : undefined}>
            <Textarea value={reason} onChange={(e) => setReason(e.target.value)} />
          </Field>
          <Field label="رقم التواصل" required error={errors.contact ? t("common_required") : undefined}>
            <Input inputMode="numeric" value={contact} onChange={(e) => setContact(e.target.value.replace(/\D/g, ""))} />
          </Field>
        </div>
      </Dialog>

      {mapOpen && <MapPickerDialog initial={coords} onPick={(c) => { setCoords(c); setMapOpen(false); }} onClose={() => setMapOpen(false)} />}

      {capOpen && (
        <Dialog title="سعة المسار" onClose={() => setCapOpen(false)}>
          <div className="stack" style={{ gap: "var(--space-2)" }}>
            <CapRow label="السعة الكاملة" value="40" />
            <CapRow label="السعة الفعلية" value="36" />
            <CapRow label="من خطوط أخرى" value="+3" tone="green" />
            <CapRow label="إلى خطوط أخرى" value="−1" tone="red" />
            <CapRow label="السعة بدون استثناءات" value="34" strong />
          </div>
          <p className="field-hint mt-4">السعة إرشادية فقط — النظام لا يمنع تجاوزها.</p>
        </Dialog>
      )}
    </>
  );
}

function CapRow({ label, value, tone, strong }: { label: string; value: string; tone?: "green" | "red"; strong?: boolean }) {
  return (
    <div className="row-between" style={{ padding: "8px 0", borderBottom: "1px solid var(--gray-100)" }}>
      <span className="muted">{label}</span>
      <span style={{ fontWeight: strong ? 700 : 500, color: tone === "green" ? "var(--color-success)" : tone === "red" ? "var(--color-danger)" : "var(--text-heading)" }}>{value}</span>
    </div>
  );
}
