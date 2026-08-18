"use client";

import { useState, useEffect, useMemo, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import type { Deduction } from "@/lib/types";
import { getDeductions, downloadDeductionsExcel, addDeduction, updateDeduction, type DeductionInput } from "@/lib/services/deductions";
import { formatDate } from "@/lib/datetime";
import { apiGet } from "@/lib/api/client";
import { getSuppliers } from "@/lib/services/suppliers";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox, { type Option } from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { LoadingState } from "@/components/ui/EmptyState";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus, IconEdit, IconDownload } from "@/components/ui/Icons";

const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";
const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Dropdown row shapes from /api/Transportation list endpoints.
interface SupplierRow { Id: number; Name: string }
interface RouteRow { Id: number; NameOfRoute: string; Serial: string; LineCost: number }

export default function DeductionsPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [rows, setRows] = useState<Deduction[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState<Deduction | "new" | null>(null);
  const [supplierOptions, setSupplierOptions] = useState<Option[]>([]);
  const canManage = can(user?.role, "add.payment");

  // Filters (all applied server-side).
  const [fSupplier, setFSupplier] = useState<string | undefined>();
  const [search, setSearch] = useState("");
  const [nameFilter, setNameFilter] = useState("");
  const [from, setFrom] = useState("");
  const [to, setTo] = useState("");
  const [downloading, setDownloading] = useState(false);

  // Debounce the route-name search.
  useEffect(() => {
    const id = setTimeout(() => setNameFilter(search.trim()), 300);
    return () => clearTimeout(id);
  }, [search]);

  const query = useMemo(
    () => ({ supplierId: fSupplier, name: nameFilter || undefined, from: from || undefined, to: to || undefined }),
    [fSupplier, nameFilter, from, to],
  );

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getDeductions(query);
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("ded.empty")), "error");
    } finally {
      setLoading(false);
    }
  }, [query, toast, t]);

  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const blob = await downloadDeductionsExcel(query);
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = "deductions.xlsx";
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
    } catch (e) {
      toast(errMsg(e, t("ded.empty")), "error");
    } finally {
      setDownloading(false);
    }
  }, [query, toast, t]);

  const loadSuppliers = useCallback(async () => {
    try {
      const res = await getSuppliers({ pageNo: 1, noOfItems: 500 });
      setSupplierOptions(res.items.map((s) => ({ value: s.id, label: s.name })));
    } catch (e) {
      toast(errMsg(e, t("ded.empty")), "error");
    }
  }, [toast, t]);

  useEffect(() => { load(); loadSuppliers(); }, [load, loadSuppliers]);

  const columns: Column<Deduction>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "routeName", header: t("filter.route") },
    { key: "supplier", header: t("filter.supplier") },
    { key: "driver", header: t("filter.driver") },
    { key: "day", header: t("ded.day"), render: (r) => formatDate(r.day) },
    { key: "amount", header: t("common.amount"), render: (r) => <b style={{ color: "var(--color-danger)" }}>{money(r.amount)}</b> },
    { key: "type", header: t("common.type"), render: (r) => (r.type === "tax" ? <span className="badge badge-amber">{t("ded.tax")}</span> : <span className="badge badge-gray">{t("ded.normal")}</span>) },
    { key: "reason", header: t("common.reason") },
    { key: "createdBy", header: t("ded.createdBy") },
    ...(canManage
      ? [{
          key: "update", header: t("ded.update"), align: "end" as const,
          render: (r: Deduction) => (
            <button className="icon-btn brand" aria-label={`${t("action.edit")}: ${r.routeName}`} onClick={() => setEditing(r)}><IconEdit /></button>
          ),
        }]
      : []),
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.deductions")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("action.downloadExcel")}</button>
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => setEditing("new")}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>
        <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
          <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
            <Combobox options={supplierOptions} value={fSupplier} onChange={setFSupplier} placeholder={t("filter.all")} />
          </Field>
          <Field label={t("filter.route")} style={{ margin: 0, minWidth: 180, flex: 1 }}>
            <Input value={search} onChange={(e) => setSearch(e.target.value)} placeholder={t("route.searchByName")} />
          </Field>
          <Field label={t("filter.from")} style={{ margin: 0, minWidth: 150 }}>
            <Input type="date" value={from} onChange={(e) => setFrom(e.target.value)} />
          </Field>
          <Field label={t("filter.to")} style={{ margin: 0, minWidth: 150 }}>
            <Input type="date" value={to} onChange={(e) => setTo(e.target.value)} />
          </Field>
        </div>
      </div>

      {loading ? (
        <LoadingState />
      ) : (
        <DataTable
          columns={columns}
          rows={rows}
          emptyMessage={t("ded.empty")}
          emptyAction={canManage ? <button className="btn btn-brand btn-sm" onClick={() => setEditing("new")}><IconPlus />{t("action.addNew")}</button> : undefined}
        />
      )}

      {editing && (
        <DeductionForm
          existing={editing === "new" ? undefined : editing}
          supplierOptions={supplierOptions}
          onClose={() => setEditing(null)}
          onSaved={load}
        />
      )}
    </div>
  );
}

function DeductionForm({ existing, supplierOptions, onClose, onSaved }: { existing?: Deduction; supplierOptions: Option[]; onClose: () => void; onSaved: () => Promise<void> }) {
  const { t } = useLocale();
  const { toast } = useToast();
  // The Deduction row carries no supplier/route ids, so edit pre-fill matches by name.
  const [supplier, setSupplier] = useState<string | undefined>(existing ? supplierOptions.find((s) => s.label === existing.supplier)?.value : undefined);
  const [route, setRoute] = useState<string | undefined>();
  const [type, setType] = useState<"normal" | "tax">(existing?.type ?? "normal");
  const [percentMode, setPercentMode] = useState(false);
  const [percent, setPercent] = useState("");
  const [perRound, setPerRound] = useState(existing ? String(existing.amount) : "");
  const [date, setDate] = useState(existing?.day ?? "");
  const [reason, setReason] = useState(existing?.reason ?? "");
  const [routeRows, setRouteRows] = useState<RouteRow[]>([]);
  const [saving, setSaving] = useState(false);

  // Routes filtered by the selected supplier.
  useEffect(() => {
    if (!supplier) { setRouteRows([]); return; }
    let cancelled = false;
    (async () => {
      try {
        const res = await apiGet<RouteRow[]>("getAllTransportationRoute", { SupplierId: supplier, PageNo: 1, NoOfItems: 500 });
        if (cancelled) return;
        const list = res.Data ?? [];
        setRouteRows(list);
        // On edit, resolve the route by name once its supplier's routes have loaded.
        if (existing && !route) {
          const match = list.find((r) => r.NameOfRoute === existing.routeName);
          if (match) setRoute(String(match.Id));
        }
      } catch (e) {
        if (!cancelled) toast(errMsg(e, t("ded.empty")), "error");
      }
    })();
    return () => { cancelled = true; };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [supplier]);

  const selectedRoute = routeRows.find((r) => String(r.Id) === route);
  const price = selectedRoute?.LineCost ?? 0;

  // live calc: per-round = price × % ÷ 100
  const computedPerRound = useMemo(() => {
    if (!percentMode || !percent) return perRound;
    return String(((price * Number(percent)) / 100).toFixed(2));
  }, [percentMode, percent, price, perRound]);

  const submit = async () => {
    if (!supplier || !route || !date || !reason.trim() || !computedPerRound) { toast(t("common.completeFields"), "error"); return; }
    const payload: DeductionInput = {
      SupplierId: supplier,
      Serial: selectedRoute?.Serial ?? "",
      TransportationVehicleRouteId: route,
      DeductPerRound: Number(computedPerRound),
      Cause: reason,
      DateOfDeduction: date,
      TypeOfDedct: type === "tax" ? "Taxes" : "Normal",
    };
    setSaving(true);
    try {
      if (existing) { await updateDeduction({ ...payload, id: existing.id }); toast(t("ded.updated")); }
      else { await addDeduction(payload); toast(t("ded.saved")); }
      onClose();
      await onSaved();
    } catch (e) {
      toast(errMsg(e, t("ded.empty")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog
      title={existing ? t("ded.editTitle") : t("ded.addTitle")}
      size="lg"
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.save")}</button></>}
    >
      <div className="two-panel">
        <Field label={t("filter.supplier")} required>
          <Combobox options={supplierOptions} value={supplier} onChange={(v) => { setSupplier(v); setRoute(undefined); }} placeholder={t("filter.supplier")} />
        </Field>
        <Field label={t("filter.route")} required>
          <Combobox options={routeRows.map((r) => ({ value: String(r.Id), label: r.NameOfRoute }))} value={route} onChange={setRoute} placeholder={t("filter.route")} disabled={!supplier} disabledReason={t("filter.selectSupplierFirst")} />
        </Field>
      </div>

      {selectedRoute && (
        <div className="row gap-3 wrap mb-4">
          <span className="info-chip">{t("ded.serial")}: {selectedRoute.Serial}</span>
          <span className="info-chip">{t("ded.routePrice")}: {money(price)}</span>
        </div>
      )}

      <div className="field">
        <span className="label" id="ded-type-label">{t("common.type")} <span className="req" aria-hidden>*</span></span>
        <SegmentedControl<"normal" | "tax">
          value={type}
          onChange={setType}
          ariaLabelledby="ded-type-label"
          segments={[
            { value: "normal", label: t("ded.normal") },
            { value: "tax", label: t("ded.tax") },
          ]}
        />
      </div>

      <label className="checkbox-row mb-4"><input type="checkbox" checked={percentMode} onChange={(e) => setPercentMode(e.target.checked)} />{t("ded.asPercent")}</label>

      <div className="two-panel">
        {percentMode && (
          <Field label={t("ded.percent")}>
            <Input inputMode="decimal" value={percent} onChange={(e) => setPercent(e.target.value)} placeholder="10" />
          </Field>
        )}
        <Field label={t("ded.perRound")} required hint={percentMode ? t("ded.perRoundHint") : undefined}>
          <Input inputMode="decimal" value={computedPerRound} onChange={(e) => setPerRound(e.target.value)} readOnly={percentMode} style={percentMode ? { background: "var(--gray-100)" } : undefined} />
        </Field>
      </div>

      <div className="two-panel">
        <Field label={t("common.date")} required><Input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></Field>
        <Field label={t("common.reason")} required><Input value={reason} onChange={(e) => setReason(e.target.value)} /></Field>
      </div>
    </Dialog>
  );
}
