"use client";

import { useState, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can, roleLabelKey, PANEL_ROLES, type Role, type SystemUser } from "@/lib/types";
import { getUsers, addUser, updateUser, deleteUser } from "@/lib/services/users";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { ActiveBadge } from "@/components/ui/Badge";
import { Field, Input, Select } from "@/components/ui/Field";
import { IconPlus, IconEdit, IconTrash } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

// Roles a user account can be assigned: the four panel roles + the (mobile)
// supervisor role, which route-supervisor login accounts need.
const roleOptions: Role[] = [...PANEL_ROLES, "supervisor"];

export default function UsersPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const canManage = can(user?.role, "manage.users");

  const [rows, setRows] = useState<SystemUser[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState<SystemUser | "new" | null>(null);
  const [del, setDel] = useState<SystemUser | null>(null);
  const [query, setQuery] = useState("");

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getUsers({ pageNo: 1, noOfItems: 500 });
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("users.empty")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);

  useEffect(() => { load(); }, [load]);

  const filtered = rows.filter((u) => u.name.includes(query) || u.email.includes(query));

  const onDelete = async (u: SystemUser) => {
    try {
      await deleteUser(u.id);
      toast(t("users.deleted"), "info");
      await load();
    } catch (e) {
      toast(errMsg(e, t("users.empty")), "error");
    }
  };

  const columns: Column<SystemUser>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: t("common.name"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.name}</b> },
    { key: "email", header: t("login.email") },
    { key: "mobile", header: t("common.mobile"), priority: "secondary" },
    { key: "role", header: t("users.role"), render: (r) => <span className="badge badge-blue">{t(roleLabelKey[r.role])}</span> },
    { key: "active", header: t("users.statusHeader"), render: (r) => <ActiveBadge active={r.active} /> },
    ...(canManage
      ? [{
          key: "actions", header: "", align: "end" as const,
          render: (r: SystemUser) => (
            <div className="cell-actions">
              <button className="icon-btn brand" aria-label={`${t("action.edit")}: ${r.name}`} onClick={() => setEditing(r)}><IconEdit /></button>
              <button className="icon-btn danger" aria-label={`${t("action.delete")}: ${r.name}`} onClick={() => setDel(r)}><IconTrash /></button>
            </div>
          ),
        }]
      : []),
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.users")} count={filtered.length}>
        {canManage && <button className="btn btn-brand btn-sm" onClick={() => setEditing("new")}><IconPlus />{t("action.addNew")}</button>}
      </PageHeader>

      <DataTable
        columns={columns}
        rows={filtered}
        loading={loading}
        emptyMessage={t("users.empty")}
        emptyAction={canManage ? <button className="btn btn-brand btn-sm" onClick={() => setEditing("new")}><IconPlus />{t("action.addNew")}</button> : undefined}
        toolbar={
          <div className="row gap-2" style={{ maxWidth: 360, border: "1px solid var(--border-base)", borderRadius: "var(--radius-base)", padding: "4px 10px" }}>
            <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder={t("users.searchPlaceholder")} style={{ border: "none", outline: "none", width: "100%", fontSize: "var(--text-sm)" }} />
          </div>
        }
      />

      {editing && (
        <UserForm
          existing={editing === "new" ? undefined : editing}
          onClose={() => setEditing(null)}
          onSaved={load}
        />
      )}

      {del && (
        <ConfirmDialog
          title={t("users.deleteTitle")}
          message={`${t("users.deleteConfirm")} "${del.name}".`}
          confirmLabel={t("action.delete")}
          onConfirm={() => onDelete(del)}
          onClose={() => setDel(null)}
        />
      )}
    </div>
  );
}

function UserForm({ existing, onClose, onSaved }: { existing?: SystemUser; onClose: () => void; onSaved: () => Promise<void> }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [name, setName] = useState(existing?.name ?? "");
  const [email, setEmail] = useState(existing?.email ?? "");
  const [mobile, setMobile] = useState(existing?.mobile ?? "");
  const [role, setRole] = useState<Role | "">(existing?.role ?? "");
  const [active, setActive] = useState(existing?.active ?? true);
  const [password, setPassword] = useState("");
  const [saving, setSaving] = useState(false);

  const submit = async () => {
    if (!name.trim() || !email.trim() || !role) { toast(t("common.completeFields"), "error"); return; }
    if (!existing && !password.trim()) { toast(t("common.completeFields"), "error"); return; }
    setSaving(true);
    try {
      if (existing) {
        await updateUser({ id: existing.id, name, email, mobile, role: role as Role, active, password: password || undefined });
        toast(t("users.updated"));
      } else {
        await addUser({ name, email, mobile, role: role as Role, active, password });
        toast(t("users.added"));
      }
      await onSaved();
      onClose();
    } catch (e) {
      toast(errMsg(e, t("users.empty")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog
      title={existing ? t("users.editTitle") : t("users.addTitle")}
      size="lg"
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
          <button className="btn btn-brand" onClick={submit} disabled={saving}>{t("action.save")}</button>
        </>
      }
    >
      <div className="two-panel">
        <Field label={t("common.name")} required>
          <Input value={name} onChange={(e) => setName(e.target.value)} autoFocus />
        </Field>
        <Field label={t("login.email")} required>
          <Input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        </Field>
      </div>
      <div className="two-panel">
        <Field label={t("common.mobile")}>
          <Input inputMode="numeric" value={mobile} onChange={(e) => setMobile(e.target.value.replace(/\D/g, ""))} />
        </Field>
        <Field label={t("users.role")} required>
          <Select value={role} onChange={(e) => setRole(e.target.value as Role)}>
            <option value="">{t("users.selectRole")}</option>
            {roleOptions.map((r) => <option key={r} value={r}>{t(roleLabelKey[r])}</option>)}
          </Select>
        </Field>
      </div>
      <Field label={t("login.password")} required={!existing} hint={t("users.passwordHint")}>
        <Input type="password" value={password} onChange={(e) => setPassword(e.target.value)} autoComplete="new-password" />
      </Field>
      <label className="checkbox-row"><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} />{active ? t("status.active") : t("status.inactive")}</label>
    </Dialog>
  );
}
