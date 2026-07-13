"use client";

import { useState, useMemo } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { suppliers, vehicles, shiftGroups, lines } from "@/lib/data";
import type { RouteItem } from "@/lib/types";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { Field, Input } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";

type RouteType = "round" | "go" | "return";

export default function RouteForm({
  lineId,
  lineName,
  existing,
  onClose,
  onSaved,
  onDeleted,
  canDelete,
}: {
  lineId?: string;
  lineName?: string;
  existing?: RouteItem;
  onClose: () => void;
  onSaved: (r: Partial<RouteItem>) => void;
  onDeleted?: () => void;
  canDelete?: boolean;
}) {
  const { t } = useLocale();
  const { toast } = useToast();

  const [name, setName] = useState(existing?.name ?? "");
  const [line, setLine] = useState<string | undefined>(lineId ?? existing?.lineId);
  const [shift, setShift] = useState<string | undefined>();
  const [cost, setCost] = useState(existing ? String(existing.cost) : "");
  const [supplier, setSupplier] = useState<string | undefined>();
  const [driver, setDriver] = useState<string | undefined>();
  const [supervisor, setSupervisor] = useState<string | undefined>();
  const [vehicle, setVehicle] = useState<string | undefined>();
  const [routeType, setRouteType] = useState<RouteType>(existing ? (existing.oneWay ? (existing.fromTime ? "go" : "return") : "round") : "round");
  const [fromTime, setFromTime] = useState(existing?.fromTime ?? "");
  const [toTime, setToTime] = useState(existing?.toTime ?? "");
  const [errors, setErrors] = useState<Record<string, boolean>>({});
  const [confirmDel, setConfirmDel] = useState(false);

  const driverOptions = useMemo(() => {
    const s = suppliers.find((x) => x.id === supplier);
    return (s?.contacts ?? []).map((c) => ({ value: c.id, label: c.name }));
  }, [supplier]);

  const needFrom = routeType === "round" || routeType === "go";
  const needTo = routeType === "round" || routeType === "return";

  const submit = () => {
    const e: Record<string, boolean> = {};
    if (!name.trim()) e.name = true;
    if (!line) e.line = true;
    if (!shift) e.shift = true;
    if (!cost || isNaN(Number(cost))) e.cost = true;
    if (!supplier) e.supplier = true;
    if (!vehicle) e.vehicle = true;
    if (needFrom && !fromTime) e.fromTime = true;
    if (needTo && !toTime) e.toTime = true;
    setErrors(e);
    if (Object.keys(e).length) return;

    onSaved({
      id: existing?.id,
      name,
      lineId: line,
      lineName: lines.find((l) => l.id === line)?.name ?? lineName,
      cost: Number(cost),
      oneWay: routeType !== "round",
      fromTime: needFrom ? fromTime : undefined,
      toTime: needTo ? toTime : undefined,
    });
    toast(existing ? "تم تحديث المسار" : "تمت إضافة المسار");
    onClose();
  };

  return (
    <>
      <Dialog
        title={existing ? `${t("action.edit")} ${t("filter.route")}` : `${t("action.add")} ${t("filter.route")}`}
        size="lg"
        onClose={onClose}
        footer={
          <>
            {existing && canDelete && (
              <button className="btn btn-danger" style={{ marginInlineEnd: "auto" }} onClick={() => setConfirmDel(true)}>{t("action.delete")}</button>
            )}
            <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
            <button className="btn btn-brand" onClick={submit}>{t("action.save")}</button>
          </>
        }
      >
        <div className="two-panel" style={{ gridTemplateColumns: "1fr 1fr" }}>
          <div>
            <Field label={t("filter.line")} required error={errors.line ? t("common_required") : undefined}>
              <Combobox options={lines.map((l) => ({ value: l.id, label: l.name }))} value={line} onChange={setLine} placeholder={t("filter.line")} />
            </Field>
            <Field label="اسم المسار" required error={errors.name ? t("common_required") : undefined}>
              <Input value={name} onChange={(e) => setName(e.target.value)} />
            </Field>
            <Field label="وردية العمل" required error={errors.shift ? t("common_required") : undefined}>
              <Combobox options={shiftGroups.map((s) => ({ value: s.id, label: `وردية ${s.number}` }))} value={shift} onChange={setShift} placeholder="اختر الوردية" />
            </Field>
            <Field label="التكلفة (لكل جولة)" required error={errors.cost ? t("common_required") : undefined}>
              <Input inputMode="decimal" value={cost} onChange={(e) => setCost(e.target.value)} placeholder="850" />
            </Field>
          </div>

          <div>
            <Field label={t("filter.supplier")} required error={errors.supplier ? t("common_required") : undefined}>
              <Combobox options={suppliers.map((s) => ({ value: s.id, label: s.name }))} value={supplier} onChange={(v) => { setSupplier(v); setDriver(undefined); }} placeholder={t("filter.supplier")} />
            </Field>
            <Field label={t("filter.driver")}>
              <Combobox options={driverOptions} value={driver} onChange={setDriver} placeholder={t("filter.driver")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
            </Field>
            <Field label="المشرف">
              <Combobox options={[{ value: "u1", label: "علي المشرف" }, { value: "u2", label: "منى صابر" }, { value: "u3", label: "خالد فؤاد" }]} value={supervisor} onChange={setSupervisor} placeholder="المشرف" />
            </Field>
            <Field label="المركبة" required error={errors.vehicle ? t("common_required") : undefined}>
              <Combobox options={vehicles.map((v) => ({ value: v.id, label: `${v.type} (${v.capacity})` }))} value={vehicle} onChange={setVehicle} placeholder="المركبة" />
            </Field>
          </div>
        </div>

        <div className="field">
          <span className="label" id="route-type-label">نوع الرحلة <span className="req" aria-hidden>*</span></span>
          <SegmentedControl<RouteType>
            value={routeType}
            onChange={setRouteType}
            ariaLabelledby="route-type-label"
            segments={[
              { value: "round", label: t("period.both") },
              { value: "go", label: t("period.go") },
              { value: "return", label: t("period.return") },
            ]}
          />
        </div>

        <div className="row gap-4 wrap">
          {needFrom && (
            <Field label="وقت الذهاب" required error={errors.fromTime ? t("common_required") : undefined} className="grow" style={{ minWidth: 160 }}>
              <Input type="time" value={fromTime} onChange={(e) => setFromTime(e.target.value)} />
            </Field>
          )}
          {needTo && (
            <Field label="وقت العودة" required error={errors.toTime ? t("common_required") : undefined} className="grow" style={{ minWidth: 160 }}>
              <Input type="time" value={toTime} onChange={(e) => setToTime(e.target.value)} />
            </Field>
          )}
        </div>
      </Dialog>

      {confirmDel && (
        <ConfirmDialog
          title={t("action.delete")}
          message={`سيتم حذف المسار "${name}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { onDeleted?.(); onClose(); }}
          onClose={() => setConfirmDel(false)}
        />
      )}
    </>
  );
}
