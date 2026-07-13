"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { vehicles as seed, vehicleTypes as seedTypes } from "@/lib/data";
import type { Vehicle } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { ApprovalBadge, ActiveBadge } from "@/components/ui/Badge";
import { IconPlus, IconEdit, IconTruck, IconCheck } from "@/components/ui/Icons";

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

  const [vehicles, setVehicles] = useState<Vehicle[]>(seed);
  const [types, setTypes] = useState<string[]>(seedTypes);
  const [editing, setEditing] = useState<Vehicle | "new" | null>(null);
  const [del, setDel] = useState<Vehicle | null>(null);

  // form state
  const [capacity, setCapacity] = useState("");
  const [type, setType] = useState<string | undefined>();
  const [active, setActive] = useState(true);
  const [addTypeOpen, setAddTypeOpen] = useState(false);
  const [newType, setNewType] = useState("");

  const open = (v: Vehicle | "new") => {
    setEditing(v);
    setCapacity(v === "new" ? "" : String(v.capacity));
    setType(v === "new" ? undefined : v.type);
    setActive(v === "new" ? true : v.active);
  };

  const save = () => {
    if (!capacity || !type) { toast("أكمل الحقول المطلوبة", "error"); return; }
    if (editing === "new") {
      setVehicles((vs) => [...vs, { id: `v${Date.now()}`, type, capacity: Number(capacity), approved: false, active }]);
      toast("تمت إضافة المركبة");
    } else if (editing) {
      setVehicles((vs) => vs.map((v) => (v.id === editing.id ? { ...v, type, capacity: Number(capacity), active } : v)));
      toast("تم تحديث المركبة");
    }
    setEditing(null);
  };

  const approve = (v: Vehicle) => { setVehicles((vs) => vs.map((x) => (x.id === v.id ? { ...x, approved: true } : x))); toast("تم اعتماد المركبة"); setEditing(null); };

  return (
    <div className="stack">
      <PageHeader title={t("nav.vehicles")} count={vehicles.length}>
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => open("new")}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

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
              <div className="muted text-sm">{v.capacity} مقعد</div>
            </div>
            <div className="row gap-2 wrap">
              <ApprovalBadge approved={v.approved} />
              <ActiveBadge active={v.active} />
            </div>
          </div>
        ))}
      </div>

      {editing && (
        <Dialog
          title={editing === "new" ? "إضافة مركبة" : "تعديل المركبة"}
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
          <Field label="السعة (عدد المقاعد)" required>
            <Input inputMode="numeric" value={capacity} onChange={(e) => setCapacity(e.target.value.replace(/\D/g, ""))} />
          </Field>
          <Field label="نوع المركبة" required>
            <div className="row gap-2">
              <div className="grow"><Combobox options={types.map((x) => ({ value: x, label: x }))} value={type} onChange={setType} placeholder="اختر النوع" /></div>
              <button className="btn btn-outline-brand btn-sm" onClick={() => setAddTypeOpen(true)}><IconPlus />نوع</button>
            </div>
          </Field>
          <label className="checkbox-row"><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />{t("status.active")}</label>
        </Dialog>
      )}

      {addTypeOpen && (
        <Dialog
          title="إضافة نوع مركبة"
          onClose={() => setAddTypeOpen(false)}
          footer={
            <>
              <button className="btn btn-secondary" onClick={() => setAddTypeOpen(false)}>{t("action.cancel")}</button>
              <button className="btn btn-brand" onClick={() => { if (newType.trim()) { setTypes((ts) => [...ts, newType]); setType(newType); setNewType(""); setAddTypeOpen(false); toast("تمت إضافة النوع"); } }}>{t("action.add")}</button>
            </>
          }
        >
          <Field label="اسم النوع" required>
            <Input value={newType} onChange={(e) => setNewType(e.target.value)} autoFocus />
          </Field>
        </Dialog>
      )}

      {del && (
        <ConfirmDialog
          title="حذف المركبة"
          message={`سيتم حذف "${del.type}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { setVehicles((vs) => vs.filter((x) => x.id !== del.id)); setEditing(null); toast("تم الحذف", "info"); }}
          onClose={() => setDel(null)}
        />
      )}
    </div>
  );
}
