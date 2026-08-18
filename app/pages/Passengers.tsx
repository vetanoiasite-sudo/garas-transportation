"use client";

import { useState, useEffect, useCallback, useRef } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getPassengers, downloadActivationTemplate, uploadActivationExcel } from "@/lib/services/passengers";
import { openFileUrl } from "@/lib/download";
import type { Passenger } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { IconPlus, IconDownload, IconUpload, IconEye, IconX } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function PassengersPage() {
  const { t, locale } = useLocale();
  const { toast } = useToast();
  const [rows, setRows] = useState<Passenger[]>([]);
  const [loading, setLoading] = useState(true);
  const p = (path: string) => `/${locale}${path}`;

  const fileRef = useRef<HTMLInputElement>(null);
  const [busy, setBusy] = useState(false);

  // Scope filters handed off from the dashboard (?supplierId=&routeId=&serial=&driverId=).
  const [params, setParams] = useSearchParams();
  const scope = {
    supplierId: params.get("supplierId") ?? undefined,
    routeId: params.get("routeId") ?? undefined,
    serial: params.get("serial") ?? undefined,
    driverId: params.get("driverId") ?? undefined,
  };
  const hasScope = !!(scope.supplierId || scope.routeId || scope.serial || scope.driverId);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getPassengers(1, 200, undefined, undefined, scope);
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("pass.emptyList")), "error");
    } finally {
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [toast, t, scope.supplierId, scope.routeId, scope.serial, scope.driverId]);
  useEffect(() => { load(); }, [load]);

  // Download the ACTIVATION template (ID + نشط) as a file.
  const onDownloadActivationTemplate = useCallback(async () => {
    setBusy(true);
    try {
      openFileUrl(await downloadActivationTemplate(), "activation-template.xlsx");
      toast(t("pass.activationTemplateDownloaded"));
    } catch (e) {
      toast(errMsg(e, t("pass.activationTemplateDownloaded")), "error");
    } finally {
      setBusy(false);
    }
  }, [toast, t]);

  // Upload an activation file → flip active/inactive on EXISTING passengers by ID.
  const onFilePicked = useCallback(async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    e.target.value = ""; // allow re-picking the same file
    if (!file) return;
    setBusy(true);
    try {
      // The API answers with a URL to an error log when some rows failed.
      const { errorLogUrl } = await uploadActivationExcel(file);
      toast(t("pass.activationDone"), errorLogUrl ? "info" : "success");
      if (errorLogUrl) openFileUrl(errorLogUrl);
      await load();
    } catch (err) {
      toast(errMsg(err, t("pass.activationDone")), "error");
    } finally {
      setBusy(false);
    }
  }, [toast, t, load]);

  // Name ↔ ID filters, two-way linked (like the dashboard's route ↔ serial pair):
  // picking a name fills the ID field; typing a full ID selects the name.
  const [nameFilter, setNameFilter] = useState<string | undefined>(); // passenger id
  const [idFilter, setIdFilter] = useState("");
  const filteredRows = rows.filter(
    (r) =>
      (!nameFilter || r.id === nameFilter) &&
      (!idFilter || r.identityNumber.includes(idFilter))
  );

  const columns: Column<Passenger>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: t("common.name"), render: (r) => <Link to={p(`/passengers/${r.id}`)} style={{ color: "var(--color-brand)", fontWeight: 600 }}>{r.name}</Link> },
    { key: "identityNumber", header: t("pass.nationalId") },
    { key: "mobile", header: t("common.mobile") },
    // "معرّف آخر" (C/M), the routes count and the active flag are NOT on
    // HrUserCardDto — only GetHrUser (the profile) carries them, so showing them
    // here would print a blank cell and a permanently-"active" badge for everyone.
    // They live on the passenger profile and in the attendance report instead.
    { key: "actions", header: "", align: "end", render: (r) => <Link to={p(`/passengers/${r.id}`)} className="icon-btn brand" aria-label={`${t("action.view")}: ${r.name}`}><IconEye /></Link> },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.employees")} count={filteredRows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadActivationTemplate} disabled={busy} title={t("pass.activationHint")}><IconDownload />{t("pass.activationTemplate")}</button>
        <button className="btn btn-secondary btn-sm" onClick={() => fileRef.current?.click()} disabled={busy} title={t("pass.activationHint")}><IconUpload />{t("pass.activationUpload")}</button>
        <input ref={fileRef} type="file" accept=".xlsx,.xls" hidden onChange={onFilePicked} />
        <Link to={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>
      </PageHeader>

      {hasScope && (
        <div className="row" style={{ gap: 8, alignItems: "center" }}>
          <span className="info-chip">{t("pass.dashFiltered")}{scope.serial ? ` · ${t("filter.serial")}: ${scope.serial}` : ""}</span>
          <button className="btn btn-secondary btn-sm" onClick={() => setParams({}, { replace: true })}><IconX />{t("action.clearFilter")}</button>
        </div>
      )}

      <DataTable
        columns={columns}
        rows={filteredRows}
        loading={loading}
        emptyMessage={t("pass.emptyList")}
        emptyAction={<Link to={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>}
        toolbar={
          <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
            <Field label={t("common.name")} style={{ margin: 0, minWidth: 200, flex: 1 }}>
              <Combobox
                options={rows.map((r) => ({ value: r.id, label: r.name }))}
                value={nameFilter}
                onChange={(v) => {
                  setNameFilter(v);
                  const match = rows.find((r) => r.id === v);
                  setIdFilter(match?.identityNumber ?? "");
                }}
                placeholder={t("common.name")}
              />
            </Field>
            <Field label={t("att.identityNumber")} style={{ margin: 0, minWidth: 180 }}>
              <Input
                inputMode="numeric"
                value={idFilter}
                onChange={(e) => {
                  const v = e.target.value;
                  setIdFilter(v);
                  const match = rows.find((r) => r.identityNumber === v);
                  setNameFilter(match ? match.id : undefined);
                }}
                placeholder={t("att.identityNumber")}
              />
            </Field>
          </div>
        }
      />
    </div>
  );
}
