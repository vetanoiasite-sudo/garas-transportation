"use client";

import { useState, useMemo, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { apiGet, apiRaw } from "@/lib/api/client";
import { addRoute, updateRoute, type RouteInput } from "@/lib/services/routes";
import type { RouteItem } from "@/lib/types";
import Dialog from "@/components/ui/Dialog";
import Combobox, { type Option } from "@/components/ui/Combobox";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { Field, Input } from "@/components/ui/Field";
import SegmentedControl from "@/components/ui/SegmentedControl";

type RouteType = "round" | "go" | "return";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Backend row shapes for the dropdown lookups (only the keys we consume).
interface LineRow { Id: number; Name: string }
interface VehicleRow { Id: number; VehicleTypeName?: string; Capacity?: number }
interface SupplierRow { Id: number; Name: string; contacts?: { Id: number; Name: string }[] }
interface UserRow { Id: number; Name: string } // login user (route supervisor candidate)

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
  onSaved?: () => void | Promise<void>;
  onDeleted?: () => void;
  canDelete?: boolean;
}) {
  const { t } = useLocale();
  const { toast } = useToast();

  const [name, setName] = useState(existing?.name ?? "");
  const [line, setLine] = useState<string | undefined>(lineId ?? existing?.lineId);
  // When adding globally, the user may either pick an existing gathering station
  // (محطة تجمع) or leave it off — in which case a new station is auto-created with
  // the route's name. Adding from within a line keeps that line locked in.
  const [chooseLine, setChooseLine] = useState<boolean>(!!lineId || !!existing?.lineId);
  const [cost, setCost] = useState(existing ? String(existing.cost) : "");
  const [supplier, setSupplier] = useState<string | undefined>(existing?.supplierId);
  const [driver, setDriver] = useState<string | undefined>(existing?.contactPersonId);
  const [supervisor, setSupervisor] = useState<string | undefined>(existing?.supervisorId);
  const [vehicle, setVehicle] = useState<string | undefined>(existing?.vehicleId);
  const [routeType, setRouteType] = useState<RouteType>(existing ? (existing.oneWay ? (existing.fromTime ? "go" : "return") : "round") : "round");
  const [fromTime, setFromTime] = useState(existing?.fromTime ?? "");
  const [toTime, setToTime] = useState(existing?.toTime ?? "");
  const [errors, setErrors] = useState<Record<string, boolean>>({});
  const [confirmDel, setConfirmDel] = useState(false);
  const [saving, setSaving] = useState(false);

  // Dropdown options fetched inline from the backend.
  const [lineOptions, setLineOptions] = useState<Option[]>([]);
  const [vehicleOptions, setVehicleOptions] = useState<Option[]>([]);
  const [supplierOptions, setSupplierOptions] = useState<Option[]>([]);
  const [supervisorOptions, setSupervisorOptions] = useState<Option[]>([]);
  const [contactsBySupplier, setContactsBySupplier] = useState<Record<string, Option[]>>({});

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const [linesRes, vehiclesRes, suppliersRes, usersRes] = await Promise.all([
          apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 200 }),
          apiGet<VehicleRow[]>("getAllTransportationVehicle", { PageNo: 1, NoOfItems: 200 }),
          apiGet<SupplierRow[]>("getSuppliers", { PageNo: 1, NoOfItems: 200 }),
          apiRaw<UserRow[]>("GET", "/User/GetAllUsers"),
        ]);
        if (!alive) return;
        setLineOptions((linesRes.Data ?? []).map((l) => ({ value: String(l.Id), label: l.Name })));
        setVehicleOptions((vehiclesRes.Data ?? []).map((v) => ({ value: String(v.Id), label: `${v.VehicleTypeName ?? ""} (${v.Capacity ?? 0})` })));
        setSupplierOptions((suppliersRes.Data ?? []).map((s) => ({ value: String(s.Id), label: s.Name })));
        // Supervisors come from the User table (login users with roles), NOT HrUser/passengers.
        setSupervisorOptions((usersRes.Data ?? []).map((u) => ({ value: String(u.Id), label: u.Name })));
        const contacts: Record<string, Option[]> = {};
        for (const s of suppliersRes.Data ?? []) {
          contacts[String(s.Id)] = (s.contacts ?? []).map((c) => ({ value: String(c.Id), label: c.Name }));
        }
        setContactsBySupplier(contacts);
      } catch (e) {
        if (alive) toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
    return () => { alive = false; };
  }, [toast, t]);

  const driverOptions = useMemo(() => (supplier ? contactsBySupplier[supplier] ?? [] : []), [supplier, contactsBySupplier]);

  const needFrom = routeType === "round" || routeType === "go";
  const needTo = routeType === "round" || routeType === "return";

  const submit = useCallback(async () => {
    const e: Record<string, boolean> = {};
    if (!name.trim()) e.name = true;
    // A gathering station is only required when the user opted to pick one; when
    // unchecked the backend auto-creates one named after the route.
    if (chooseLine && !line) e.line = true;
    if (!cost || isNaN(Number(cost))) e.cost = true;
    if (!supplier) e.supplier = true;
    if (!vehicle) e.vehicle = true;
    if (needFrom && !fromTime) e.fromTime = true;
    if (needTo && !toTime) e.toTime = true;
    setErrors(e);
    if (Object.keys(e).length) return;

    const input: RouteInput = {
      name,
      // No station picked → send none; the backend creates one named after the route.
      lineId: chooseLine ? line : undefined,
      vehicleId: vehicle,
      supplierId: supplier,
      contactPersonId: driver,
      supervisorId: supervisor,
      cost: Number(cost),
      oneWay: routeType !== "round",
      fromTime: needFrom ? fromTime : undefined,
      toTime: needTo ? toTime : undefined,
      active: true,
    };

    setSaving(true);
    try {
      if (existing) await updateRoute(existing.id, input);
      else await addRoute(input);
      toast(existing ? t("route.updated") : t("route.added"));
      await onSaved?.();
      onClose();
    } catch (err) {
      toast(errMsg(err, t("empty.generic")), "error");
    } finally {
      setSaving(false);
    }
  }, [name, line, chooseLine, cost, supplier, vehicle, driver, supervisor, routeType, fromTime, toTime, needFrom, needTo, existing, onSaved, onClose, toast, t]);

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
            <button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.save")}</button>
          </>
        }
      >
        <div className="two-panel" style={{ gridTemplateColumns: "1fr 1fr" }}>
          <div>
            {lineId ? (
              // Adding within a specific line — that station is locked in.
              <Field label={t("filter.line")}>
                <Combobox options={lineOptions} value={line} onChange={setLine} placeholder={t("filter.line")} disabled />
              </Field>
            ) : (
              <div className="field">
                <label className="checkbox-row" style={{ marginBottom: chooseLine ? 8 : 0 }}>
                  <input type="checkbox" checked={chooseLine} onChange={(e) => { setChooseLine(e.target.checked); if (!e.target.checked) setLine(undefined); }} />
                  {t("route.chooseLine")}
                </label>
                {chooseLine && (
                  <Combobox options={lineOptions} value={line} onChange={setLine} placeholder={t("filter.line")} />
                )}
                {chooseLine && errors.line && <div className="field-error">{t("common_required")}</div>}
                {!chooseLine && <div className="muted text-xs" style={{ marginTop: 4 }}>{t("route.autoLineHint")}</div>}
              </div>
            )}
            <Field label={t("route.name")} required error={errors.name ? t("common_required") : undefined}>
              <Input value={name} onChange={(e) => setName(e.target.value)} />
            </Field>
            <Field label={t("route.costPerTrip")} required error={errors.cost ? t("common_required") : undefined}>
              <Input inputMode="decimal" value={cost} onChange={(e) => setCost(e.target.value)} placeholder="850" />
            </Field>
          </div>

          <div>
            <Field label={t("filter.supplier")} required error={errors.supplier ? t("common_required") : undefined}>
              <Combobox options={supplierOptions} value={supplier} onChange={(v) => { setSupplier(v); setDriver(undefined); }} placeholder={t("filter.supplier")} />
            </Field>
            <Field label={t("filter.driver")}>
              <Combobox options={driverOptions} value={driver} onChange={setDriver} placeholder={t("filter.driver")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
            </Field>
            <Field label={t("common.supervisor")}>
              <Combobox options={supervisorOptions} value={supervisor} onChange={setSupervisor} placeholder={t("common.supervisor")} />
            </Field>
            <Field label={t("route.vehicle")} required error={errors.vehicle ? t("common_required") : undefined}>
              <Combobox options={vehicleOptions} value={vehicle} onChange={setVehicle} placeholder={t("route.vehicle")} />
            </Field>
          </div>
        </div>

        <div className="field">
          <span className="label" id="route-type-label">{t("route.tripType")} <span className="req" aria-hidden>*</span></span>
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
            <Field label={t("route.goTime")} required error={errors.fromTime ? t("common_required") : undefined} className="grow" style={{ minWidth: 160 }}>
              <Input type="time" value={fromTime} onChange={(e) => setFromTime(e.target.value)} />
            </Field>
          )}
          {needTo && (
            <Field label={t("route.returnTime")} required error={errors.toTime ? t("common_required") : undefined} className="grow" style={{ minWidth: 160 }}>
              <Input type="time" value={toTime} onChange={(e) => setToTime(e.target.value)} />
            </Field>
          )}
        </div>
      </Dialog>

      {confirmDel && (
        <ConfirmDialog
          title={t("action.delete")}
          message={`${t("route.deleteConfirm")} "${name}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { onDeleted?.(); onClose(); }}
          onClose={() => setConfirmDel(false)}
        />
      )}
    </>
  );
}
