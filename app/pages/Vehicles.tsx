"use client";

import { useState, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import type { Vehicle } from "@/lib/types";
import { getVehicles, getVehicleTypes, addVehicle, updateVehicle, deleteVehicle, approveVehicle, addVehicleType } from "@/lib/services/vehicles";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import { ApprovalBadge, ActiveBadge } from "@/components/ui/Badge";
import { IconPlus, IconEdit, IconTruck, IconCheck } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

function statusColor(v: Vehicle) {
  if (!v.approved) return "var(--red-500)";
  return v.active ? "var(--green-500)" : "var(--amber-500)";
}

export default function VehiclesPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const canManage = can(user?.role, "manage.vehicles");
  const canApprove = can(user?.role, "approve.vehicle");

  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [types, setTypes] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState<Vehicle | "new" | null>(null);
  const [del, setDel] = useState<Vehicle | null>(null);

  // form state
  const [capacity, setCapacity] = useState("");
  const [type, setType] = useState<string | undefined>();
  const [active, setActive] = useState(true);
  const [addTypeOpen, setAddTypeOpen] = useState(false);
  const [newType, setNewType] = useState("");

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const [res, ts] = await Promise.all([getVehicles(1, 100), getVehicleTypes()]);
      setVehicles(res.items);
      setTypes(ts);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);

  useEffect(() => { load(); }, [load]);

  const open = (v: Vehicle | "new") => {
    setEditing(v);
    setCapacity(v === "new" ? "" : String(v.capacity));
    setType(v === "new" ? undefined : v.type);
    setActive(v === "new" ? true : v.active);
  };

  const save = async () => {
    if (!capacity || !type) { toast(t("common.completeFields"), "error"); return; }
    try {
      if (editing === "new") {
        await addVehicle({ type, capacity: Number(capacity), active });
        toast(t("veh.added"));
      } else if (editing) {
        await updateVehicle({ ...editing, type, capacity: Number(capacity), active });
        toast(t("veh.updated"));
      }
      setEditing(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const approve = async (v: Vehicle) => {
    try {
      await approveVehicle(v.id);
      toast(t("veh.approved"));
      setEditing(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const remove = async (v: Vehicle) => {
    try {
      await deleteVehicle(v.id);
      toast(t("veh.deleted"), "info");
      setEditing(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const createType = async () => {
    if (!newType.trim()) return;
    const name = newType.trim();
    try {
      await addVehicleType(name);
      setNewType("");
      setAddTypeOpen(false);
      const ts = await getVehicleTypes();
      setTypes(ts);
      setType(name);
      toast(t("veh.typeAdded"));
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <div className="stack">
      <PageHeader title={t("nav.vehicles")} count={vehicles.length}>
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => open("new")}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      {loading ? (
        <LoadingState />
      ) : vehicles.length === 0 ? (
        <div className="card"><EmptyState message={t("empty.generic")} action={canManage ? <button className="btn btn-brand btn-sm" onClick={() => open("new")}><IconPlus />{t("action.addNew")}</button> : undefined} /></div>
      ) : (
      <div className="card-grid">
        {vehicles.map((v) => (
          <div key={v.id} className="card card-pad stack" style={{ gap: "var(--space-3)", borderInlineStartWidth: 4, borderInlineStartColor: statusColor(v) }}>
            <div className="row-between">
              <span className="kpi-icon"><IconTruck /></span>
              {canManage && (
                <div className="row gap-2">
                  {canApprove && !v.approved && <button className="icon-btn" style={{ color: "var(--color-success)" }} aria-label={t("action.approve")} onClick={() => approve(v)}><IconCheck /></button>}
                  <button className="icon-btn brand" aria-label={t("action.edit")} onClick={() => open(v)}><IconEdit /></button>
                </div>
              )}
            </div>
            <div>
              <div className="section-title">{v.type}</div>
              <div className="muted text-sm">{v.capacity} {t("common.seats")}</div>
            </div>
            <div className="row gap-2 wrap">
              <ApprovalBadge approved={v.approved} />
              <ActiveBadge active={v.active} />
            </div>
          </div>
        ))}
      </div>
      )}

      {editing && (
        <Dialog
          title={editing === "new" ? t("veh.addTitle") : t("veh.editTitle")}
          onClose={() => setEditing(null)}
          footer={
            <>
              {editing !== "new" && canManage && <button className="btn btn-danger" style={{ marginInlineEnd: "auto" }} onClick={() => setDel(editing)}>{t("action.delete")}</button>}
              {editing !== "new" && canApprove && !editing.approved && <button className="btn btn-success" onClick={() => approve(editing)}>{t("action.approve")}</button>}
              <button className="btn btn-secondary" onClick={() => setEditing(null)}>{t("action.cancel")}</button>
              <button className="btn btn-brand" onClick={save}>{t("action.save")}</button>
            </>
          }
        >
          <Field label={t("veh.capacity")} required>
            <Input inputMode="numeric" value={capacity} onChange={(e) => setCapacity(e.target.value.replace(/\D/g, ""))} />
          </Field>
          <Field label={t("veh.vehicleType")} required>
            <div className="row gap-2">
              <div className="grow"><Combobox options={types.map((x) => ({ value: x, label: x }))} value={type} onChange={setType} placeholder={t("veh.selectType")} /></div>
              <button className="btn btn-outline-brand btn-sm" onClick={() => setAddTypeOpen(true)}><IconPlus />{t("veh.typeShort")}</button>
            </div>
          </Field>
          <label className="checkbox-row"><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />{t("status.active")}</label>
        </Dialog>
      )}

      {addTypeOpen && (
        <Dialog
          title={t("veh.addTypeTitle")}
          onClose={() => setAddTypeOpen(false)}
          footer={
            <>
              <button className="btn btn-secondary" onClick={() => setAddTypeOpen(false)}>{t("action.cancel")}</button>
              <button className="btn btn-brand" onClick={createType}>{t("action.add")}</button>
            </>
          }
        >
          <Field label={t("veh.typeName")} required>
            <Input value={newType} onChange={(e) => setNewType(e.target.value)} autoFocus />
          </Field>
        </Dialog>
      )}

      {del && (
        <ConfirmDialog
          title={t("veh.deleteTitle")}
          message={`${t("veh.deleteConfirm")} "${del.type}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { remove(del); }}
          onClose={() => setDel(null)}
        />
      )}
    </div>
  );
}
