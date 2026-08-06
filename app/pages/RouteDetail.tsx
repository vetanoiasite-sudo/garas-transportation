"use client";

import { useState, useEffect, useCallback, useRef } from "react";
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
  addRouteUsers,
  deleteRouteUser,
  deleteRoute,
} from "@/lib/services/routes";
import { getPassengers } from "@/lib/services/passengers";
import { formatTime } from "@/lib/datetime";
import type { RouteItem, Station, PassengerAssignment, Period } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Select } from "@/components/ui/Field";
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

  const assignPassengers = async (inputs: { passengerId: string; stationId?: string; period: Period }[]) => {
    await addRouteUsers(
      routeId,
      inputs.map((i) => ({ hrUserId: i.passengerId, directionId: i.stationId, period: i.period })),
    );
    toast(inputs.length > 1 ? t("route.passengersAssigned") : t("route.passengerAssigned"));
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
        <span className="info-chip"><IconClock style={{ width: 14, height: 14 }} />{route?.fromTime ? formatTime(route.fromTime) : "—"} {route?.toTime ? `→ ${formatTime(route.toTime)}` : ""}</span>
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
          onAssign={assignPassengers}
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

/** One row of the multi-add form: a passenger plus its own station + direction. */
interface AssignRow {
  key: string;
  passengerId?: string;
  station?: string;
  period: Period;
}

function AssignPassengerDialog({
  stations,
  onClose,
  onAssign,
}: {
  stations: Station[];
  onClose: () => void;
  onAssign: (inputs: { passengerId: string; stationId?: string; period: Period }[]) => Promise<void>;
}) {
  const { t } = useLocale();
  const { toast } = useToast();
  const rowSeq = useRef(1);
  const [rows, setRows] = useState<AssignRow[]>([{ key: "r0", period: "both" }]);
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

  const stationOptions = stations.map((s) => ({ value: s.id, label: s.name }));

  const addRow = () =>
    setRows((rs) => [...rs, { key: `r${rowSeq.current++}`, period: "both" }]);
  const patchRow = (key: string, p: Partial<AssignRow>) =>
    setRows((rs) => rs.map((r) => (r.key === key ? { ...r, ...p } : r)));
  const removeRow = (key: string) =>
    setRows((rs) => (rs.length > 1 ? rs.filter((r) => r.key !== key) : rs));

  // Passengers already chosen in *other* rows can't be picked again.
  const optionsFor = (row: AssignRow) =>
    passengerOptions.filter(
      (o) => o.value === row.passengerId || !rows.some((r) => r.key !== row.key && r.passengerId === o.value),
    );

  const submit = async () => {
    const chosen = rows.filter((r) => r.passengerId);
    if (chosen.length === 0) { setError(true); return; }
    setSaving(true);
    try {
      await onAssign(chosen.map((r) => ({ passengerId: r.passengerId!, stationId: r.station, period: r.period })));
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
      size="lg"
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
          <button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.submit")}</button>
        </>
      }
    >
      <div className="stack" style={{ gap: "var(--space-3)" }}>
        {rows.map((r) => (
          <div key={r.key} className="card card-pad">
            <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
              <Field label={t("common.passenger")} required className="grow" style={{ margin: 0, minWidth: 180 }}>
                <Combobox
                  options={optionsFor(r)}
                  value={r.passengerId}
                  onChange={(v) => { patchRow(r.key, { passengerId: v }); if (v) setError(false); }}
                  placeholder={t("route.selectPassenger")}
                />
              </Field>
              <Field label={t("common.station")} style={{ margin: 0, minWidth: 150 }}>
                <Combobox
                  options={stationOptions}
                  value={r.station}
                  onChange={(v) => patchRow(r.key, { station: v })}
                  placeholder={t("station.select")}
                />
              </Field>
              <Field label={t("common.direction")} required style={{ margin: 0, minWidth: 130 }}>
                <Select value={r.period} onChange={(e) => patchRow(r.key, { period: e.target.value as Period })}>
                  <option value="both">{t("period.both")}</option>
                  <option value="go">{t("period.go")}</option>
                  <option value="return">{t("period.return")}</option>
                </Select>
              </Field>
              <button
                type="button"
                className="icon-btn danger"
                aria-label={t("action.delete")}
                disabled={rows.length === 1}
                onClick={() => removeRow(r.key)}
              >
                <IconTrash />
              </button>
            </div>
          </div>
        ))}
      </div>

      {error && <div className="text-sm" style={{ color: "var(--color-danger)", marginTop: "var(--space-2)" }}>{t("route.selectAtLeastOne")}</div>}

      <div style={{ marginTop: "var(--space-3)" }}>
        <button className="btn btn-outline-brand btn-sm" onClick={addRow}><IconPlus />{t("route.addAnother")}</button>
      </div>
    </Dialog>
  );
}
