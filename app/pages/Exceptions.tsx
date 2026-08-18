"use client";

import { useState, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import type { Exception, Period } from "@/lib/types";
import { getExceptions, addException, getCapacityNumbers, periodToApi, type ExceptionInput, type CapacityNumbers } from "@/lib/services/exceptions";
import { formatDate } from "@/lib/datetime";
import { apiGet } from "@/lib/api/client";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox, { type Option } from "@/components/ui/Combobox";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import { Field, Input, Textarea } from "@/components/ui/Field";
import { LoadingState } from "@/components/ui/EmptyState";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus } from "@/components/ui/Icons";

const periodLabel: Record<Period, string> = { go: "period.go", return: "period.return", both: "period.both" };
const WEEKDAYS = ["Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Dropdown row shapes from /api/Transportation list endpoints.
interface RouteRow { Id: number; NameOfRoute: string }
interface DirectionRow { Id: string; RouteDirection: string }
interface RouteEmployeeRow { HrUserId: number; FirstNane: string; MiddleName: string; LastName: string }

export default function ExceptionsPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [rows, setRows] = useState<Exception[]>([]);
  const [loading, setLoading] = useState(true);
  const [formOpen, setFormOpen] = useState(false);
  const [routeOptions, setRouteOptions] = useState<Option[]>([]);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getExceptions();
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("exc.empty")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);

  const loadRoutes = useCallback(async () => {
    try {
      const res = await apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 200 });
      setRouteOptions((res.Data ?? []).map((r) => ({ value: String(r.Id), label: r.NameOfRoute })));
    } catch (e) {
      toast(errMsg(e, t("exc.empty")), "error");
    }
  }, [toast, t]);

  useEffect(() => { load(); loadRoutes(); }, [load, loadRoutes]);

  const columns: Column<Exception>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "passenger", header: t("common.passenger"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.passenger}</b> },
    { key: "routeName", header: t("filter.route") },
    { key: "station", header: t("common.station"), priority: "secondary" },
    { key: "date", header: t("common.date"), render: (r) => r.date ? formatDate(r.date) : `${formatDate(r.fromDate)} → ${formatDate(r.toDate)}` },
    { key: "period", header: t("common.direction"), render: (r) => <span className="badge badge-blue">{t(periodLabel[r.period])}</span> },
    { key: "reason", header: t("common.reason"), priority: "secondary" },
    { key: "contact", header: t("exc.contact"), priority: "secondary" },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.exceptions")} count={rows.length}>
        {can(user?.role, "manage.exceptions") && <button className="btn btn-brand btn-sm" onClick={() => setFormOpen(true)}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      {loading ? (
        <LoadingState />
      ) : (
        <DataTable
          columns={columns}
          rows={rows}
          emptyMessage={t("exc.empty")}
          emptyAction={can(user?.role, "manage.exceptions") ? <button className="btn btn-brand btn-sm" onClick={() => setFormOpen(true)}><IconPlus />{t("action.addNew")}</button> : undefined}
        />
      )}

      {formOpen && <ExceptionForm routeOptions={routeOptions} onClose={() => setFormOpen(false)} onSaved={load} />}
    </div>
  );
}

function ExceptionForm({ routeOptions, onClose, onSaved }: { routeOptions: Option[]; onClose: () => void; onSaved: () => Promise<void> }) {
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
  const [weekdays, setWeekdays] = useState("");
  const [coords, setCoords] = useState<{ lat?: number; lng?: number }>({});
  const [mapOpen, setMapOpen] = useState(false);
  // "Both" collects a second (get-out / drop-off) point — PRD FR-E3 / RULES §2.5
  const [locTypeOut, setLocTypeOut] = useState<"station" | "free">("station");
  const [stationOut, setStationOut] = useState<string | undefined>();
  const [coordsOut, setCoordsOut] = useState<{ lat?: number; lng?: number }>({});
  const [mapOutOpen, setMapOutOpen] = useState(false);
  const [capOpen, setCapOpen] = useState(false);
  const [errors, setErrors] = useState<Record<string, boolean>>({});

  // Route-scoped dropdowns fetched on route change.
  const [stationOptions, setStationOptions] = useState<Option[]>([]);
  const [passengerOptions, setPassengerOptions] = useState<Option[]>([]);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!route) { setStationOptions([]); setPassengerOptions([]); return; }
    let cancelled = false;
    (async () => {
      try {
        // Stations for the route (get-in / get-out point picker).
        const dirs = await apiGet<DirectionRow[]>("GetTransportationDirection", { RouteId: route });
        if (!cancelled) setStationOptions((dirs.Data ?? []).map((d) => ({ value: String(d.Id), label: d.RouteDirection })));
      } catch (e) { if (!cancelled) toast(errMsg(e, t("exc.empty")), "error"); }
      try {
        // Passenger list = employees assigned to the chosen route (no global list endpoint).
        const emps = await apiGet<RouteEmployeeRow[]>("GetTransportationEmployeeList", { RouteId: route });
        if (!cancelled) setPassengerOptions((emps.Data ?? []).map((m) => ({ value: String(m.HrUserId), label: [m.FirstNane, m.MiddleName, m.LastName].filter(Boolean).join(" ") })));
      } catch (e) { if (!cancelled) toast(errMsg(e, t("exc.empty")), "error"); }
    })();
    return () => { cancelled = true; };
  }, [route, toast, t]);

  const submit = async () => {
    const e: Record<string, boolean> = {};
    if (!passenger) e.passenger = true;
    if (!route) e.route = true;
    if (!reason.trim()) e.reason = true;
    if (!contact.trim()) e.contact = true;
    // "Both" requires the second get-out point to be provided.
    if (period === "both" && (locTypeOut === "station" ? !stationOut : coordsOut.lat == null)) e.getOut = true;
    setErrors(e);
    if (Object.keys(e).length) return;

    const payload: ExceptionInput = {
      transportationVehicleRouteId: route!,
      hrUserId: passenger!,
      Active: true,
      reasonException: reason,
      contactNumber: contact,
      Period: periodToApi(period),
    };
    if (type === "single") payload.ExceptionDate = date;
    else { payload.FromDate = fromDate; payload.ToDate = toDate; payload.DayName = weekdays; }
    if (locType === "station") payload.TransportationVehicleRouteDirectionId = station;
    else { payload.Latitude = coords.lat; payload.Longtitud = coords.lng; }
    // NOTE: the "Both" drop-off point (stationOut/coordsOut) has no field in the
    // AddException body — backend gap; only the get-in location is sent.

    setSaving(true);
    try {
      await addException(payload);
      toast(t("exc.addedToast"));
      onClose();
      await onSaved();
    } catch (err) {
      toast(errMsg(err, t("exc.empty")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <>
      <Dialog
        title={t("exc.addTitle")}
        size="lg"
        onClose={onClose}
        footer={
          <>
            <button className="btn btn-secondary" style={{ marginInlineEnd: "auto" }} disabled={!route || !period} onClick={() => setCapOpen(true)}>{t("exc.routeCapacity")}</button>
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.save")}</button>
          </>
        }
      >
        <div className="two-panel">
          <Field label={t("common.passenger")} required error={errors.passenger ? t("exc.selectPassengerError") : undefined}>
            <Combobox options={passengerOptions} value={passenger} onChange={setPassenger} placeholder={t("exc.selectPassengerPlaceholder")} disabled={!route} disabledReason={t("exc.selectRouteFirst")} />
          </Field>
          <Field label={t("filter.route")} required error={errors.route ? t("exc.selectRouteError") : undefined}>
            <Combobox options={routeOptions} value={route} onChange={(v) => { setRoute(v); setStation(undefined); setStationOut(undefined); setPassenger(undefined); }} placeholder={t("exc.selectRoutePlaceholder")} />
          </Field>
        </div>

        {/* TYPE radio group */}
        <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
          <legend className="label" style={{ padding: 0 }}>{t("common.type")}</legend>
          <div className="row gap-4 wrap">
            <label className="checkbox-row"><input type="radio" name="exc-type" checked={type === "single"} onChange={() => setType("single")} />{t("exc.singleDate")}</label>
            <label className="checkbox-row"><input type="radio" name="exc-type" checked={type === "period"} onChange={() => setType("period")} />{t("exc.period")}</label>
          </div>
          <div className="row gap-3 wrap mt-4">
            {type === "single" ? (
              <input className="input" type="date" aria-label={t("exc.exceptionDateAria")} value={date} onChange={(e) => setDate(e.target.value)} style={{ maxWidth: 200 }} />
            ) : (
              <>
                <input className="input" type="date" aria-label={t("filter.from")} value={fromDate} onChange={(e) => setFromDate(e.target.value)} style={{ maxWidth: 180 }} />
                <input className="input" type="date" aria-label={t("filter.to")} value={toDate} onChange={(e) => setToDate(e.target.value)} style={{ maxWidth: 180 }} />
                {/* The backend matches DayName against DateTime.ToString("dddd") —
                    an invariant English name — so the value is fixed and only the
                    label is localised. A free-text box never matched. */}
                <select className="input" aria-label={t("exc.weekdays")} value={weekdays} onChange={(e) => setWeekdays(e.target.value)} style={{ maxWidth: 180 }}>
                  <option value="">{t("exc.weekdays")}</option>
                  {WEEKDAYS.map((d) => <option key={d} value={d}>{t(`day.${d.toLowerCase()}`)}</option>)}
                </select>
              </>
            )}
          </div>
        </fieldset>

        {/* LOCATION radio group (get-in point) */}
        <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
          <legend className="label" style={{ padding: 0 }}>{period === "both" ? t("exc.pickupLocation") : t("exc.location")}</legend>
          <div className="row gap-4 wrap">
            <label className="checkbox-row"><input type="radio" name="exc-loc" checked={locType === "station"} onChange={() => setLocType("station")} />{t("exc.station")}</label>
            <label className="checkbox-row"><input type="radio" name="exc-loc" checked={locType === "free"} onChange={() => setLocType("free")} />{t("exc.freeLocation")}</label>
          </div>
          <div className="mt-4">
            {locType === "station" ? (
              <Combobox options={stationOptions} value={station} onChange={setStation} placeholder={t("exc.selectStationPlaceholder")} disabled={!route} disabledReason={t("exc.selectRouteFirst")} />
            ) : (
              <button className="btn btn-outline-brand" onClick={() => setMapOpen(true)}>{coords.lat != null ? `${coords.lat}, ${coords.lng}` : t("action.pickLocation")}</button>
            )}
          </div>
        </fieldset>

        {/* PERIOD */}
        <div className="field">
          <span className="label" id="exc-period-label">{t("common.direction")} <span className="req" aria-hidden>*</span></span>
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
          {period === "both" && <p className="field-hint mt-4">{t("exc.bothHint")}</p>}
        </div>

        {/* Second (get-out / drop-off) point — only when period is "Both" */}
        {period === "both" && (
          <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
            <legend className="label" style={{ padding: 0 }}>{t("exc.dropoffLocation")} <span className="req" aria-hidden>*</span></legend>
            <div className="row gap-4 wrap">
              <label className="checkbox-row"><input type="radio" name="exc-loc-out" checked={locTypeOut === "station"} onChange={() => setLocTypeOut("station")} />{t("exc.station")}</label>
              <label className="checkbox-row"><input type="radio" name="exc-loc-out" checked={locTypeOut === "free"} onChange={() => setLocTypeOut("free")} />{t("exc.freeLocation")}</label>
            </div>
            <div className="mt-4">
              {locTypeOut === "station" ? (
                <Combobox options={stationOptions} value={stationOut} onChange={setStationOut} placeholder={t("exc.selectStationPlaceholder")} disabled={!route} disabledReason={t("exc.selectRouteFirst")} />
              ) : (
                <button className="btn btn-outline-brand" onClick={() => setMapOutOpen(true)}>{coordsOut.lat != null ? `${coordsOut.lat}, ${coordsOut.lng}` : t("action.pickLocation")}</button>
              )}
            </div>
            {errors.getOut && <p className="field-hint mt-4" style={{ color: "var(--color-danger)" }}>{t("exc.selectDropoffError")}</p>}
          </fieldset>
        )}

        <div className="two-panel">
          <Field label={t("common.reason")} required error={errors.reason ? t("common_required") : undefined}>
            <Textarea value={reason} onChange={(e) => setReason(e.target.value)} />
          </Field>
          <Field label={t("exc.contact")} required error={errors.contact ? t("common_required") : undefined}>
            <Input inputMode="numeric" value={contact} onChange={(e) => setContact(e.target.value.replace(/\D/g, ""))} />
          </Field>
        </div>
      </Dialog>

      {mapOpen && <MapPickerDialog initial={coords} onPick={(c) => { setCoords(c); setMapOpen(false); }} onClose={() => setMapOpen(false)} />}

      {mapOutOpen && <MapPickerDialog initial={coordsOut} onPick={(c) => { setCoordsOut(c); setMapOutOpen(false); }} onClose={() => setMapOutOpen(false)} />}

      {capOpen && route && <CapacityDialog routeId={route} period={periodToApi(period)} onClose={() => setCapOpen(false)} />}
    </>
  );
}

function CapacityDialog({ routeId, period, onClose }: { routeId: string; period: string; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [cap, setCap] = useState<CapacityNumbers | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      setLoading(true);
      try {
        const c = await getCapacityNumbers(routeId, period);
        if (!cancelled) setCap(c);
      } catch (e) {
        if (!cancelled) toast(errMsg(e, t("exc.empty")), "error");
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => { cancelled = true; };
  }, [routeId, period, toast, t]);

  return (
    <Dialog title={t("exc.routeCapacity")} onClose={onClose}>
      {loading || !cap ? (
        <LoadingState />
      ) : (
        <>
          <div className="stack" style={{ gap: "var(--space-2)" }}>
            <CapRow label={t("exc.fullCapacity")} value={String(cap.fullCapacity)} />
            <CapRow label={t("exc.actualCapacity")} value={String(cap.actualCapacity)} />
            <CapRow label={t("exc.fromOtherLines")} value={`+${cap.fromOtherLines}`} tone="green" />
            <CapRow label={t("exc.toOtherLines")} value={`−${cap.toOtherLines}`} tone="red" />
            <CapRow label={t("exc.capacityWithoutExceptions")} value={String(cap.capacityWithoutExceptions)} strong />
          </div>
          <p className="field-hint mt-4">{t("exc.capacityHint")}</p>
        </>
      )}
    </Dialog>
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
