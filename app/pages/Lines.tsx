"use client";

import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can, canAdd, type Line } from "@/lib/types";
import { getLines, addLine, updateLine, deleteLine, approveLine } from "@/lib/services/lines";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import { Field, Input } from "@/components/ui/Field";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import { ApprovalBadge } from "@/components/ui/Badge";
import { IconPlus, IconBus, IconEdit, IconRoute, IconCheck, IconChevronEnd } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function LinesPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [lines, setLines] = useState<Line[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState<Line | "new" | null>(null);
  const [name, setName] = useState("");
  const [confirmDel, setConfirmDel] = useState<Line | null>(null);
  const [search, setSearch] = useState("");
  const [nameFilter, setNameFilter] = useState("");

  const canManage = can(user?.role, "crud.entities");
  const canApprove = can(user?.role, "approve.line");
  const p = (path: string) => `/${locale}${path}`;

  // Debounce the name search (server-side filter on getAllTransportationLine).
  useEffect(() => {
    const id = setTimeout(() => setNameFilter(search.trim()), 300);
    return () => clearTimeout(id);
  }, [search]);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getLines(1, 200, nameFilter || undefined);
      setLines(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [nameFilter, toast, t]);

  useEffect(() => { load(); }, [load]);

  const openEdit = (l: Line | "new") => {
    setEditing(l);
    setName(l === "new" ? "" : l.name);
  };

  const save = async () => {
    if (!name.trim()) return;
    try {
      if (editing === "new") { await addLine(name); toast(t("line.added")); }
      else if (editing) { await updateLine(editing.id, name); toast(t("line.updated")); }
      setEditing(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const approve = async (l: Line) => {
    try {
      await approveLine(l.id);
      toast(t("line.approved"));
      setEditing(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  const del = async (l: Line) => {
    try {
      await deleteLine(l.id);
      toast(t("line.deleted"), "info");
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <div className="stack">
      <PageHeader title={t("nav.lines")} count={lines.length}>
        {canAdd(user?.role) && (
          <button className="btn btn-brand btn-sm" onClick={() => openEdit("new")}><IconPlus />{t("action.addNew")}</button>
        )}
      </PageHeader>

      <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>
        <Field label={t("action.search")} style={{ margin: 0, maxWidth: 360 }}>
          <Input value={search} onChange={(e) => setSearch(e.target.value)} placeholder={t("line.searchByName")} />
        </Field>
      </div>

      {loading ? (
        <LoadingState />
      ) : lines.length === 0 ? (
        <div className="card"><EmptyState message={t("empty.generic")} action={canAdd(user?.role) ? <button className="btn btn-brand btn-sm" onClick={() => openEdit("new")}><IconPlus />{t("action.addNew")}</button> : undefined} /></div>
      ) : (
        <div className="card-grid">
        {lines.map((l) => (
          <div key={l.id} className="card card-pad stack" style={{ gap: "var(--space-3)" }}>
            <div className="row-between">
              <span className="kpi-icon"><IconBus /></span>
              <ApprovalBadge approved={l.approved} />
            </div>
            <div>
              <div className="section-title">{l.name}</div>
              <div className="muted text-sm">
                {l.routesCount > 0 ? `${l.routesCount} ${t("filter.route")}` : t("line.noRoutes")}
              </div>
            </div>
            <div className="row gap-2 wrap">
              <Link to={p(`/lines/${l.id}/routes`)} className="btn btn-secondary btn-sm"><IconRoute />{t("action.viewRoutes")}<IconChevronEnd style={{ width: 14, height: 14 }} /></Link>
              {canManage && <button className="icon-btn brand" onClick={() => openEdit(l)} aria-label={`${t("action.edit")}: ${l.name}`}><IconEdit /></button>}
              {canApprove && !l.approved && <button className="icon-btn" style={{ color: "var(--color-success)" }} onClick={() => approve(l)} aria-label={`${t("action.approve")}: ${l.name}`}><IconCheck /></button>}
            </div>
          </div>
        ))}
        </div>
      )}

      {editing && (
        <Dialog
          title={editing === "new" ? `${t("action.add")} ${t("nav.lines")}` : `${t("action.edit")} ${t("nav.lines")}`}
          onClose={() => setEditing(null)}
          footer={
            <>
              {editing !== "new" && canManage && (
                <button className="btn btn-danger" style={{ marginInlineEnd: "auto" }} onClick={() => { setConfirmDel(editing); }}>{t("action.delete")}</button>
              )}
              {editing !== "new" && canApprove && !editing.approved && (
                <button className="btn btn-success" onClick={() => approve(editing)}>{t("action.approve")}</button>
              )}
              <button className="btn btn-secondary" onClick={() => setEditing(null)}>{t("action.cancel")}</button>
              <button className="btn btn-brand" onClick={save}>{t("action.save")}</button>
            </>
          }
        >
          <div className="field">
            <label className="label">{t("line.name")} <span className="req">*</span></label>
            <input className="input" value={name} onChange={(e) => setName(e.target.value)} autoFocus placeholder="محطة تجمع المعادي" />
          </div>
        </Dialog>
      )}

      {confirmDel && (
        <ConfirmDialog
          title={t("action.delete")}
          message={`${t("line.deletePrefix")} "${confirmDel.name}". ${t("line.deleteIrreversible")}`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { del(confirmDel); setEditing(null); }}
          onClose={() => setConfirmDel(null)}
        />
      )}
    </div>
  );
}
