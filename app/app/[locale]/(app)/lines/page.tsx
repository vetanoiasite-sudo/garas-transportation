"use client";

import { useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can, canAdd, type Line } from "@/lib/types";
import { lines as seedLines } from "@/lib/data";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { ApprovalBadge } from "@/components/ui/Badge";
import { IconPlus, IconBus, IconEdit, IconRoute, IconCheck, IconChevronEnd } from "@/components/ui/Icons";

export default function LinesPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [lines, setLines] = useState<Line[]>(seedLines);
  const [editing, setEditing] = useState<Line | "new" | null>(null);
  const [name, setName] = useState("");
  const [confirmDel, setConfirmDel] = useState<Line | null>(null);

  const canManage = can(user?.role, "crud.entities");
  const canApprove = can(user?.role, "approve.line");
  const p = (path: string) => `/${locale}${path}`;

  const openEdit = (l: Line | "new") => {
    setEditing(l);
    setName(l === "new" ? "" : l.name);
  };

  const save = () => {
    if (!name.trim()) return;
    if (editing === "new") {
      setLines((ls) => [...ls, { id: `l${Date.now()}`, name, routesCount: 0, approved: false }]);
      toast("تمت إضافة الخط");
    } else if (editing) {
      setLines((ls) => ls.map((l) => (l.id === editing.id ? { ...l, name } : l)));
      toast("تم تحديث الخط");
    }
    setEditing(null);
  };

  const approve = (l: Line) => {
    setLines((ls) => ls.map((x) => (x.id === l.id ? { ...x, approved: true } : x)));
    toast("تم اعتماد الخط");
    setEditing(null);
  };

  const del = (l: Line) => {
    setLines((ls) => ls.filter((x) => x.id !== l.id));
    toast("تم حذف الخط", "info");
  };

  return (
    <div className="stack">
      <PageHeader title={t("nav.lines")} count={lines.length}>
        {canAdd(user?.role) && (
          <button className="btn btn-brand btn-sm" onClick={() => openEdit("new")}><IconPlus />{t("action.addNew")}</button>
        )}
      </PageHeader>

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
                {l.routesCount > 0 ? `${l.routesCount} ${t("filter.route")}` : "لا توجد مسارات — عيّن مسارًا"}
              </div>
            </div>
            <div className="row gap-2 wrap">
              <Link href={p(`/lines/${l.id}/routes`)} className="btn btn-secondary btn-sm"><IconRoute />{t("action.viewRoutes")}<IconChevronEnd style={{ width: 14, height: 14 }} /></Link>
              {canManage && <button className="icon-btn brand" onClick={() => openEdit(l)} aria-label={`${t("action.edit")}: ${l.name}`}><IconEdit /></button>}
              {canApprove && !l.approved && <button className="icon-btn" style={{ color: "var(--color-success)" }} onClick={() => approve(l)} aria-label={`${t("action.approve")}: ${l.name}`}><IconCheck /></button>}
            </div>
          </div>
        ))}
      </div>

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
            <label className="label">اسم الخط <span className="req">*</span></label>
            <input className="input" value={name} onChange={(e) => setName(e.target.value)} autoFocus placeholder="خط المعادي" />
          </div>
        </Dialog>
      )}

      {confirmDel && (
        <ConfirmDialog
          title={t("action.delete")}
          message={`سيتم حذف "${confirmDel.name}". لا يمكن التراجع عن هذا الإجراء.`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { del(confirmDel); setEditing(null); }}
          onClose={() => setConfirmDel(null)}
        />
      )}
    </div>
  );
}
