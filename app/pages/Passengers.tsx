"use client";

import { useState, useEffect, useCallback, useRef } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { getPassengers, downloadActivationTemplate, uploadActivationExcel, fileToBase64 } from "@/lib/services/passengers";
import type { Passenger } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import { ActiveBadge } from "@/components/ui/Badge";
import { IconPlus, IconDownload, IconUpload, IconEye } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function PassengersPage() {
  const { t, locale } = useLocale();
  const { toast } = useToast();
  const [rows, setRows] = useState<Passenger[]>([]);
  const [loading, setLoading] = useState(true);
  const p = (path: string) => `/${locale}${path}`;

  const fileRef = useRef<HTMLInputElement>(null);
  const [busy, setBusy] = useState(false);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getPassengers(1, 200);
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("pass.emptyList")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);
  useEffect(() => { load(); }, [load]);

  // Download the ACTIVATION template (ID + نشط) as a file.
  const onDownloadActivationTemplate = useCallback(async () => {
    setBusy(true);
    try {
      const blob = await downloadActivationTemplate();
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = "activation-template.xlsx";
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);
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
      const base64 = await fileToBase64(file);
      const { updated, notFound } = await uploadActivationExcel(base64);
      const suffix = notFound.length ? ` — ${t("pass.notMatched")}: ${notFound.length}` : "";
      toast(`${t("pass.activationDone")} (${updated})${suffix}`, notFound.length ? "info" : "success");
      await load();
    } catch (err) {
      toast(errMsg(err, t("pass.activationDone")), "error");
    } finally {
      setBusy(false);
    }
  }, [toast, t, load]);

  const columns: Column<Passenger>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: t("common.name"), render: (r) => <Link to={p(`/passengers/${r.id}`)} style={{ color: "var(--color-brand)", fontWeight: 600 }}>{r.name}</Link> },
    { key: "identityNumber", header: t("pass.nationalId") },
    { key: "mobile", header: t("common.mobile") },
    { key: "identifier", header: t("pass.otherIdentifier") },
    { key: "routesCount", header: t("pass.routes"), render: (r) => <span className="info-chip">{r.routesCount}</span> },
    { key: "active", header: t("pass.status"), render: (r) => <ActiveBadge active={r.active} /> },
    { key: "actions", header: "", align: "end", render: (r) => <Link to={p(`/passengers/${r.id}`)} className="icon-btn brand" aria-label={`${t("action.view")}: ${r.name}`}><IconEye /></Link> },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.employees")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadActivationTemplate} disabled={busy} title={t("pass.activationHint")}><IconDownload />{t("pass.activationTemplate")}</button>
        <button className="btn btn-secondary btn-sm" onClick={() => fileRef.current?.click()} disabled={busy} title={t("pass.activationHint")}><IconUpload />{t("pass.activationUpload")}</button>
        <input ref={fileRef} type="file" accept=".xlsx,.xls" hidden onChange={onFilePicked} />
        <Link to={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        loading={loading}
        emptyMessage={t("pass.emptyList")}
        emptyAction={<Link to={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>}
      />
    </div>
  );
}
