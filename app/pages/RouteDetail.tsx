"use client";

import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useNavigate, useParams } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import {
  getRouteDetail,
  getStations,
  getRouteUsers,
  deleteStation,
  addRouteUser,
  deleteRouteUser,
  deleteRoute,
} from "@/lib/services/routes";
import { getPassengers } from "@/lib/services/passengers";
import type { RouteItem, Station, PassengerAssignment, Period } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";
import StationForm from "@/components/routes/StationForm";
import RouteForm from "@/components/routes/RouteForm";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import {
  IconPlus, IconEdit, IconTrash, IconPin, IconCopy, IconChevronEnd, IconClock, IconUser,
} from "@/components/ui/Icons";

const periodLabel: Record<Period, string> = { go: "period.go", return: "period.return", both: "period.both" };
const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function RouteDetailPage() {
  const { id = "", routeId = "", locale = "ar" } = useParams();
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const navigate = useNavigate();
  const canManage = can(user?.role, "crud.entities");
  const p = (path: string) => `/${locale}${path}`;

  const [route, setRoute] = useState<RouteItem | undefined>();
  const [stations, setStations] = useState<Station[]>([]);
  const [assignments, setAssignments] = useState<PassengerAssignment[]>([]);
  const [loading, setLoading] = useState(true);
  const [stationForm, setStationForm] = useState<Station | "new" | null>(null);
  const [delStation, setDelStation] = useState<Station | null>(null);
  const [assignOpen, setAssignOpen] = useState(false);
  const [delAssign, setDelAssign] = useState<PassengerAssignment | null>(null);
  const [editOpen, setEditOpen] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [r, st, us] = await Promise.all([
        getRouteDetail(routeId),
        getStations(routeId),
        getRouteUsers(routeId),
      ]);
      setRoute(r);
      setStations(st);
      setAssignments(us);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [routeId, toast, t]);

  useEffect(() => { load(); }, [load]);

  const removeStation = async (s: Station) => {
    try {
      await deleteStation(s.id);
      toast(t("station.deleted"), "info");
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const assignPassenger = async (input: { passengerId: string; stationId?: string; period: Period }) => {
    await addRouteUser(routeId, { hrUserId: input.passengerId, directionId: input.stationId, period: input.period });
    toast(t("route.passengerAssigned"));
    await load();
  };

  const removeAssignment = async (a: PassengerAssignment) => {
    try {
      await deleteRouteUser(a.id);
      toast(t("route.removed"), "info");
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const removeRoute = async () => {
    try {
      await deleteRoute(routeId);
      toast(t("route.deleted") || t("empty.generic"), "info");
      navigate(p(`/lines/${id}/routes`));
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link to={p("/lines")} style={{ color: "var(--color-brand)" }}>{t("nav.lines")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <Link to={p(`/lines/${id}/routes`)} style={{ color: "var(--color-brand)" }}>{route?.lineName}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{route?.name}</span>
      </div>

      <PageHeader title={route?.name ?? t("filter.route")}>
        {canManage && route && <button className="btn btn-secondary btn-sm" onClick={() => setEditOpen(true)}><IconEdit />{t("action.edit")}</button>}
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => setStationForm("new")}><IconPlus />{t("station.add")}</button>}
        {canManage && <button className="btn btn-secondary btn-sm" onClick={() => setAssignOpen(true)}><IconPlus />{t("route.addPassenger")}</button>}
      </PageHeader>

      {loading ? (
        <LoadingState />
      ) : (
        <>
      {/* route info strip */}
      <div className="card card-pad row wrap gap-4">
        <span className="info-chip"><IconClock style={{ width: 14, height: 14 }} />{route?.fromTime ?? "—"} {route?.toTime ? `→ ${route.toTime}` : ""}</span>
        <span className="info-chip">{t("filter.supplier")}: {route?.supplier}</span>
        <span className="info-chip">{t("filter.driver")}: {route?.driver}</span>
        <span className="info-chip"><IconUser style={{ width: 14, height: 14 }} />{route?.supervisor}</span>
        <span className="info-chip">C: {route?.religionC ?? 0} · M: {route?.religionM ?? 0}</span>
      </div>

      <div className="two-panel">
        {/* Stations panel */}
        <div className="card">
          <div className="rail-head">{t("common.stations")} ({stations.length})</div>
          {stations.length === 0 ? (
            <EmptyState message={t("station.empty")} />
          ) : (
            <div className="table-wrap" style={{ border: "none" }}>
              <table className="data">
                <thead><tr><th>#</th><th>{t("common.station")}</th><th>{t("common.coordinates")}</th>{canManage && <th></th>}</tr></thead>
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
                            <button className="icon-btn" title={t("common.copy")} aria-label={t("station.copyCoords")} onClick={() => { navigator.clipboard?.writeText(`${s.lat}, ${s.lng}`); toast(t("common.copied"), "info"); }}><IconCopy /></button>
                            <span className="icon-btn" title={t("station.viewOnMap")} role="img" aria-label={t("station.locationOnMap")}><IconPin /></span>
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
          <div className="rail-head">{t("common.passengers")} ({assignments.length})</div>
          {assignments.length === 0 ? (
            <EmptyState message={t("route.noPassengers")} />
          ) : (
            <div className="table-wrap" style={{ border: "none" }}>
              <table className="data">
                <thead><tr><th>#</th><th>{t("common.passenger")}</th><th>{t("common.station")}</th><th>{t("common.direction")}</th>{canManage && <th></th>}</tr></thead>
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
        </>
      )}

      {stationForm && (
        <StationForm
          routeId={routeId}
          existing={stationForm === "new" ? undefined : stationForm}
          onClose={() => setStationForm(null)}
          onSaved={load}
        />
      )}

      {delStation && (
        <ConfirmDialog
          title={`${t("action.delete")} ${t("common.station")}`}
          message={`${t("station.deleteConfirm")} "${delStation.name}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { removeStation(delStation); }}
          onClose={() => setDelStation(null)}
        />
      )}

      {editOpen && route && (
        <RouteForm
          existing={route}
          lineName={route.lineName}
          canDelete={canManage}
          onClose={() => setEditOpen(false)}
          onSaved={load}
          onDeleted={removeRoute}
        />
      )}

      {assignOpen && (
        <AssignPassengerDialog
          stations={stations}
          onClose={() => setAssignOpen(false)}
          onAssign={assignPassenger}
        />
      )}

      {delAssign && (
        <ConfirmDialog
          title={t("route.removePassenger")}
          message={`${t("route.removePrefix")} "${delAssign.passengerName}" ${t("route.removeFromRoute")}`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { removeAssignment(delAssign); }}
          onClose={() => setDelAssign(null)}
        />
      )}
    </div>
  );
}

function AssignPassengerDialog({
  stations,
  onClose,
  onAssign,
}: {
  stations: Station[];
  onClose: () => void;
  onAssign: (input: { passengerId: string; stationId?: string; period: Period }) => Promise<void>;
}) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [passenger, setPassenger] = useState<string | undefined>();
  const [station, setStation] = useState<string | undefined>();
  const [period, setPeriod] = useState<Period>("both");
  const [error, setError] = useState(false);
  const [saving, setSaving] = useState(false);
  // Real passengers from the backend — using the mock list here sent bogus ids
  // (e.g. "p1") that the server rejected, which was the "can't add passenger" error.
  const [passengerOptions, setPassengerOptions] = useState<{ value: string; label: string }[]>([]);

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const res = await getPassengers(1, 500, undefined, true); // active passengers only
        if (alive) setPassengerOptions(res.items.map((pp) => ({ value: pp.id, label: pp.name })));
      } catch {
        if (alive) setPassengerOptions([]);
      }
    })();
    return () => { alive = false; };
  }, []);

  const submit = async () => {
    if (!passenger) { setError(true); return; }
    setSaving(true);
    try {
      await onAssign({ passengerId: passenger, stationId: station, period });
      onClose();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog
      title={t("route.assignPassenger")}
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
          <button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.submit")}</button>
        </>
      }
    >
      <Field label={t("common.passenger")} required error={error ? t("route.selectPassengerError") : undefined}>
        <Combobox options={passengerOptions} value={passenger} onChange={setPassenger} placeholder={t("route.selectPassenger")} />
      </Field>
      <Field label={t("common.station")}>
        <Combobox options={stations.map((s) => ({ value: s.id, label: s.name }))} value={station} onChange={setStation} placeholder={t("station.select")} />
      </Field>
      <div className="field">
        <span className="label" id="assign-period-label">{t("common.direction")} <span className="req" aria-hidden>*</span></span>
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
