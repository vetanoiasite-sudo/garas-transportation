"use client";

import { useCallback, useEffect, useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import type { Period } from "@/lib/types";
import {
  getPassengerRoutes,
  getRouteOptions,
  addRoutesForPassenger,
  updateRouteForPassenger,
  deleteRouteForPassenger,
  type PassengerRoute,
  type PassengerRouteInput,
  type RouteOption,
} from "@/lib/services/passengers";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import MapPickerDialog from "@/components/ui/MapPickerDialog";
import { Field, Input, Select } from "@/components/ui/Field";
import { LoadingState } from "@/components/ui/EmptyState";
import { IconPlus, IconEdit, IconTrash, IconPin } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

interface Row {
  id: string; // local React key
  membershipId?: string; // backend membership Id (saved rows) — Update/Delete target
  routeId?: string;
  station?: string;
  period: Period;
  fromDate?: string;
  toDate?: string;
  lat?: number;
  lng?: number;
  saved: boolean;
  editing: boolean;
  dirty: boolean;
}

const toRow = (r: PassengerRoute): Row => ({
  id: `m${r.id}`,
  membershipId: r.id,
  routeId: r.routeId,
  period: r.period,
  fromDate: r.fromDate,
  toDate: r.toDate,
  lat: r.lat,
  lng: r.lng,
  saved: true,
  editing: false,
  dirty: false,
});

const toInput = (r: Row): PassengerRouteInput => ({
  routeId: r.routeId ?? "",
  period: r.period,
  fromDate: r.fromDate,
  toDate: r.toDate,
  lat: r.lat,
  lng: r.lng,
});

/* Multi-row assignment: saved rows are read-only until Edit; per-row Submit
   enables only when something changed (change detection); only new rows are
   posted on the dialog-level Submit. (§7.2) */
export default function AssignRoutesDialog({ passengerId, onClose }: { passengerId: string; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [rows, setRows] = useState<Row[]>([]);
  const [options, setOptions] = useState<RouteOption[]>([]);
  const [loading, setLoading] = useState(true);
  const [mapFor, setMapFor] = useState<string | null>(null);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [memberships, opts] = await Promise.all([getPassengerRoutes(passengerId), getRouteOptions()]);
      setRows(memberships.map(toRow));
      setOptions(opts);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [passengerId, toast, t]);

  useEffect(() => {
    load();
  }, [load]);

  const addRow = () =>
    setRows((rs) => [...rs, { id: `row${Date.now()}`, period: "both", saved: false, editing: true, dirty: false }]);

  const patch = (id: string, p: Partial<Row>) =>
    setRows((rs) => rs.map((r) => (r.id === id ? { ...r, ...p, dirty: true } : r)));

  const removeRow = async (id: string) => {
    const row = rows.find((r) => r.id === id);
    if (row?.saved && row.membershipId) {
      try {
        await deleteRouteForPassenger(row.membershipId);
        toast(t("assign.deleted"), "info");
        await load();
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
      return;
    }
    setRows((rs) => rs.filter((r) => r.id !== id));
  };

  const saveRow = async (id: string) => {
    const row = rows.find((r) => r.id === id);
    if (!row || !row.routeId) return;
    try {
      if (row.membershipId) await updateRouteForPassenger(passengerId, row.membershipId, toInput(row));
      else await addRoutesForPassenger(passengerId, [toInput(row)]);
      toast(t("assign.saved"));
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const submitAll = async () => {
    const newRows = rows.filter((r) => !r.saved && r.routeId);
    try {
      if (newRows.length) {
        await addRoutesForPassenger(passengerId, newRows.map(toInput));
        toast(`${t("assign.savedNewPrefix")} ${newRows.length} ${t("assign.savedNewSuffix")}`);
      }
      onClose();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <>
      <Dialog
        title={t("assign.title")}
        size="lg"
        onClose={onClose}
        footer={
          <>
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submitAll}>{t("action.submit")}</button>
          </>
        }
      >
        <div className="mb-4">
          <button className="btn btn-outline-brand btn-sm" onClick={addRow}><IconPlus />{t("action.addRoute")}</button>
        </div>

        {loading ? (
          <LoadingState />
        ) : (
        <div className="stack" style={{ gap: "var(--space-3)" }}>
          {rows.map((r) => {
            const readOnly = r.saved && !r.editing;
            return (
              <div key={r.id} className="card card-pad" style={{ background: readOnly ? "var(--gray-50)" : "#fff" }}>
                <div className="row gap-3 wrap" style={{ alignItems: "flex-end" }}>
                  <Field label={t("filter.route")} required className="grow" style={{ margin: 0, minWidth: 160 }}>
                    <Combobox options={options} value={r.routeId} onChange={(v) => patch(r.id, { routeId: v })} placeholder={t("assign.chooseRoute")} disabled={readOnly} />
                  </Field>
                  <Field label={t("common.station")} style={{ margin: 0, minWidth: 130 }}>
                    <Input value={r.station ?? ""} onChange={(e) => patch(r.id, { station: e.target.value })} disabled={readOnly} placeholder="—" />
                  </Field>
                  <Field label={t("common.direction")} required style={{ margin: 0, minWidth: 150 }}>
                    <Select value={r.period} onChange={(e) => patch(r.id, { period: e.target.value as Period })} disabled={readOnly}>
                      <option value="both">{t("period.both")}</option>
                      <option value="go">{t("period.go")}</option>
                      <option value="return">{t("period.return")}</option>
                    </Select>
                  </Field>
                </div>
                <div className="row gap-3 wrap mt-4" style={{ alignItems: "flex-end" }}>
                  <Field label={t("filter.from")} style={{ margin: 0, minWidth: 130 }}>
                    <Input type="date" value={r.fromDate ?? ""} onChange={(e) => patch(r.id, { fromDate: e.target.value })} disabled={readOnly} />
                  </Field>
                  <Field label={t("filter.to")} style={{ margin: 0, minWidth: 130 }}>
                    <Input type="date" value={r.toDate ?? ""} onChange={(e) => patch(r.id, { toDate: e.target.value })} disabled={readOnly} />
                  </Field>
                  <div className="field" style={{ margin: 0 }}>
                    <span className="label">{t("common.coordinates")}</span>
                    <button className="btn btn-secondary btn-sm" onClick={() => setMapFor(r.id)} disabled={readOnly}>
                      <IconPin />{r.lat != null ? `${r.lat}, ${r.lng}` : t("action.pickLocation")}
                    </button>
                  </div>
                  <div className="row gap-2" style={{ marginInlineStart: "auto" }}>
                    {readOnly ? (
                      <button className="icon-btn brand" aria-label={t("action.edit")} onClick={() => patch(r.id, { editing: true, dirty: false })}><IconEdit /></button>
                    ) : (
                      <button className="btn btn-success btn-sm" disabled={!r.dirty && r.saved} onClick={() => saveRow(r.id)}>{t("action.save")}</button>
                    )}
                    <button className="icon-btn danger" aria-label={t("action.delete")} onClick={() => removeRow(r.id)}><IconTrash /></button>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
        )}
      </Dialog>

      {mapFor && (
        <MapPickerDialog
          onPick={(c) => { patch(mapFor, { lat: c.lat, lng: c.lng }); setMapFor(null); }}
          onClose={() => setMapFor(null)}
        />
      )}
    </>
  );
}
