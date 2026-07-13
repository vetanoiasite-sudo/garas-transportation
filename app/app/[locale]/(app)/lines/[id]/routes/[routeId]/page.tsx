"use client";

import { use, useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { getRoute, getStations, getAssignments, passengers as allPassengers } from "@/lib/data";
import type { Station, PassengerAssignment, Period } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";
import StationForm from "@/components/routes/StationForm";
import { EmptyState } from "@/components/ui/EmptyState";
import {
  IconPlus, IconEdit, IconTrash, IconPin, IconCopy, IconChevronEnd, IconClock, IconUser,
} from "@/components/ui/Icons";

const periodLabel: Record<Period, string> = { go: "period.go", return: "period.return", both: "period.both" };

export default function RouteDetailPage({ params }: { params: Promise<{ id: string; routeId: string; locale: string }> }) {
  const { id, routeId, locale } = use(params);
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const route = getRoute(routeId);
  const canManage = can(user?.role, "crud.entities");
  const p = (path: string) => `/${locale}${path}`;

  const [stations, setStations] = useState<Station[]>(getStations(routeId));
  const [assignments, setAssignments] = useState<PassengerAssignment[]>(getAssignments(routeId));
  const [stationForm, setStationForm] = useState<Station | "new" | null>(null);
  const [delStation, setDelStation] = useState<Station | null>(null);
  const [assignOpen, setAssignOpen] = useState(false);
  const [delAssign, setDelAssign] = useState<PassengerAssignment | null>(null);

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link href={p("/lines")} style={{ color: "var(--color-brand)" }}>{t("nav.lines")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <Link href={p(`/lines/${id}/routes`)} style={{ color: "var(--color-brand)" }}>{route?.lineName}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{route?.name}</span>
      </div>

      <PageHeader title={route?.name ?? "المسار"}>
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => setStationForm("new")}><IconPlus />إضافة محطة</button>}
        {canManage && <button className="btn btn-secondary btn-sm" onClick={() => setAssignOpen(true)}><IconPlus />إضافة راكب</button>}
      </PageHeader>

      {/* route info strip */}
      <div className="card card-pad row wrap gap-4">
        <span className="info-chip"><IconClock style={{ width: 14, height: 14 }} />{route?.fromTime ?? "—"} {route?.toTime ? `→ ${route.toTime}` : ""}</span>
        <span className="info-chip">{t("filter.supplier")}: {route?.supplier}</span>
        <span className="info-chip">{t("filter.driver")}: {route?.driver}</span>
        <span className="info-chip"><IconUser style={{ width: 14, height: 14 }} />{route?.supervisor}</span>
        <span className="info-chip">مسلم: {route?.religionM} · مسيحي: {route?.religionC}</span>
      </div>

      <div className="two-panel">
        {/* Stations panel */}
        <div className="card">
          <div className="rail-head">المحطات ({stations.length})</div>
          {stations.length === 0 ? (
            <EmptyState message="لا توجد محطات بعد." />
          ) : (
            <div className="table-wrap" style={{ border: "none" }}>
              <table className="data">
                <thead><tr><th>#</th><th>المحطة</th><th>الإحداثيات</th>{canManage && <th></th>}</tr></thead>
                <tbody>
                  {stations.map((s, i) => (
                    <tr key={s.id}>
                      <td>{i + 1}</td>
                      <td>
                        <div style={{ color: "var(--text-heading)", fontWeight: 500 }}>{s.name} {!s.active && <span className="badge badge-orange">{t("status.inactive")}</span>}</div>
                        <div className="text-xs muted">{s.description}</div>
                      </td>
                      <td>
                        {s.lat != null ? (
                          <div className="row" style={{ gap: 4 }}>
                            <span className="coords">{s.lat}, {s.lng}</span>
                            <button className="icon-btn" title="نسخ" aria-label="نسخ الإحداثيات" onClick={() => { navigator.clipboard?.writeText(`${s.lat}, ${s.lng}`); toast("تم النسخ", "info"); }}><IconCopy /></button>
                            <span className="icon-btn" title="عرض على الخريطة" role="img" aria-label="الموقع على الخريطة"><IconPin /></span>
                          </div>
                        ) : <span className="muted">—</span>}
                      </td>
                      {canManage && (
                        <td>
                          <div className="cell-actions">
                            <button className="icon-btn brand" aria-label={`${t("action.edit")}: ${s.name}`} onClick={() => setStationForm(s)}><IconEdit /></button>
                            <button className="icon-btn danger" aria-label={`${t("action.delete")}: ${s.name}`} onClick={() => setDelStation(s)}><IconTrash /></button>
                          </div>
                        </td>
                      )}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>

        {/* Passengers panel */}
        <div className="card">
          <div className="rail-head">الركاب ({assignments.length})</div>
          {assignments.length === 0 ? (
            <EmptyState message="لا يوجد ركاب معيّنون بعد." />
          ) : (
            <div className="table-wrap" style={{ border: "none" }}>
              <table className="data">
                <thead><tr><th>#</th><th>الراكب</th><th>المحطة</th><th>الاتجاه</th>{canManage && <th></th>}</tr></thead>
                <tbody>
                  {assignments.map((a, i) => (
                    <tr key={a.id}>
                      <td>{i + 1}</td>
                      <td style={{ color: "var(--text-heading)", fontWeight: 500 }}>{a.passengerName}</td>
                      <td>{a.stationName ?? "—"}</td>
                      <td><span className="badge badge-blue">{t(periodLabel[a.period])}</span></td>
                      {canManage && (
                        <td>
                          <div className="cell-actions">
                            <button className="icon-btn danger" aria-label={`${t("action.delete")}: ${a.passengerName}`} onClick={() => setDelAssign(a)}><IconTrash /></button>
                          </div>
                        </td>
                      )}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {stationForm && (
        <StationForm
          routeId={routeId}
          existing={stationForm === "new" ? undefined : stationForm}
          onClose={() => setStationForm(null)}
          onSaved={(s) => setStations((prev) => (prev.some((x) => x.id === s.id) ? prev.map((x) => (x.id === s.id ? s : x)) : [...prev, s]))}
        />
      )}

      {delStation && (
        <ConfirmDialog
          title="حذف المحطة"
          message={`سيتم حذف المحطة "${delStation.name}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { setStations((s) => s.filter((x) => x.id !== delStation.id)); toast("تم حذف المحطة", "info"); }}
          onClose={() => setDelStation(null)}
        />
      )}

      {assignOpen && (
        <AssignPassengerDialog
          stations={stations}
          onClose={() => setAssignOpen(false)}
          onSaved={(a) => { setAssignments((prev) => [...prev, a]); toast("تم تعيين الراكب"); }}
        />
      )}

      {delAssign && (
        <ConfirmDialog
          title="إزالة الراكب"
          message={`سيتم إزالة "${delAssign.passengerName}" من المسار.`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { setAssignments((a) => a.filter((x) => x.id !== delAssign.id)); toast("تمت الإزالة", "info"); }}
          onClose={() => setDelAssign(null)}
        />
      )}
    </div>
  );
}

function AssignPassengerDialog({
  stations,
  onClose,
  onSaved,
}: {
  stations: Station[];
  onClose: () => void;
  onSaved: (a: PassengerAssignment) => void;
}) {
  const { t } = useLocale();
  const [passenger, setPassenger] = useState<string | undefined>();
  const [station, setStation] = useState<string | undefined>();
  const [period, setPeriod] = useState<Period>("both");
  const [error, setError] = useState(false);

  const submit = () => {
    if (!passenger) { setError(true); return; }
    const pObj = allPassengers.find((x) => x.id === passenger);
    onSaved({
      id: `pa${Date.now()}`,
      passengerId: passenger,
      passengerName: pObj?.name ?? "راكب",
      stationName: stations.find((s) => s.id === station)?.name,
      period,
    });
    onClose();
  };

  return (
    <Dialog
      title="تعيين راكب للمسار"
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
          <button className="btn btn-brand" onClick={submit}>{t("action.submit")}</button>
        </>
      }
    >
      <Field label="الراكب" required error={error ? "يرجى اختيار راكب" : undefined}>
        <Combobox options={allPassengers.map((p) => ({ value: p.id, label: p.name }))} value={passenger} onChange={setPassenger} placeholder="اختر راكبًا" />
      </Field>
      <Field label="المحطة">
        <Combobox options={stations.map((s) => ({ value: s.id, label: s.name }))} value={station} onChange={setStation} placeholder="اختر المحطة" />
      </Field>
      <div className="field">
        <span className="label" id="assign-period-label">الاتجاه <span className="req" aria-hidden>*</span></span>
        <SegmentedControl<Period>
          value={period}
          onChange={setPeriod}
          ariaLabelledby="assign-period-label"
          segments={[
            { value: "both", label: t("period.both") },
            { value: "go", label: t("period.go") },
            { value: "return", label: t("period.return") },
          ]}
        />
      </div>
    </Dialog>
  );
}
