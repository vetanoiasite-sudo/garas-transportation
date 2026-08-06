"use client";

import { useState, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import type { Repricing } from "@/lib/types";
import { getRepricings, createRepricing, approveRepricing, rejectRepricing } from "@/lib/services/repricing";
import { formatDate } from "@/lib/datetime";
import { apiGet } from "@/lib/api/client";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input } from "@/components/ui/Field";
import { LoadingState } from "@/components/ui/EmptyState";
import SegmentedControl from "@/components/ui/SegmentedControl";
import { IconPlus, IconEye, IconAlert, IconPercent, IconMoney, IconTrash } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function RepricingPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const canCreate = can(user?.role, "create.repricing");
  const canApprove = can(user?.role, "approve.repricing");
  const canReject = can(user?.role, "reject.repricing");

  const [rows, setRows] = useState<Repricing[]>([]);
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);
  const [review, setReview] = useState<Repricing | null>(null);
  const [confirmApprove, setConfirmApprove] = useState<Repricing | null>(null);

  const pendingCount = rows.filter((r) => !r.approved).length;

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getRepricings({ pageNo: 1, noOfItems: 100 });
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t]);

  useEffect(() => { load(); }, [load]);

  const columns: Column<Repricing>[] = [
    { key: "amount", header: t("rep.increaseAmount"), render: (r) => <b>{r.amount}{r.mode === "percent" ? "%" : " ج.م"}</b> },
    { key: "createdBy", header: t("rep.createdBy") },
    { key: "createdAt", header: t("rep.createdAt"), render: (r) => formatDate(r.createdAt) },
    { key: "forAllLines", header: t("rep.allLines"), render: (r) => (r.forAllLines ? <span className="badge badge-blue">{t("common.yes")}</span> : <span className="badge badge-gray">{t("common.selected")}</span>) },
    { key: "approved", header: t("rep.approval"), render: (r) => (r.approved ? <span className="badge badge-green">{t("status.approved")}</span> : <span className="badge badge-amber">{t("status.pending")}</span>) },
    { key: "approvedBy", header: t("rep.approvedBy"), render: (r) => r.approvedBy ?? <span className="muted">—</span> },
    { key: "actions", header: "", align: "end", render: (r) => <button className="icon-btn brand" aria-label={t("action.view")} onClick={() => setReview(r)}><IconEye /></button> },
  ];

  const approve = async (r: Repricing) => {
    try {
      await approveRepricing(r.id);
      toast(t("rep.approvedToast"));
      setReview(null);
      setConfirmApprove(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };
  const reject = async (r: Repricing) => {
    try {
      await rejectRepricing(r.id);
      toast(t("rep.rejectedToast"), "info");
      setReview(null);
      await load();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <div className="stack">
      <PageHeader title={t("nav.repricing")} count={rows.length}>
        {canCreate && <button className="btn btn-brand btn-sm" onClick={() => setCreating(true)}><IconPlus />{t("rep.create")}</button>}
      </PageHeader>

      {pendingCount > 0 && (
        <div className="banner-pending"><IconAlert style={{ width: 18, height: 18 }} />{t("rep.pendingBannerBefore")} {pendingCount} {t("rep.pendingBannerAfter")}</div>
      )}

      {loading ? <LoadingState /> : <DataTable columns={columns} rows={rows} emptyMessage={t("empty.repricing")} />}

      {creating && <CreateRepricing onClose={() => setCreating(false)} onCreated={load} />}

      {review && (
        <Dialog
          title={t("rep.review")}
          size="lg"
          onClose={() => setReview(null)}
          footer={
            !review.approved && (canApprove || canReject) ? (
              <>
                {canReject && <button className="btn btn-danger" style={{ marginInlineEnd: "auto" }} onClick={() => reject(review)}>{t("action.reject")}</button>}
                <button className="btn btn-secondary" onClick={() => setReview(null)}>{t("action.close")}</button>
                {canApprove && <button className="btn btn-success" onClick={() => setConfirmApprove(review)}>{t("action.approve")}</button>}
              </>
            ) : (
              <button className="btn btn-secondary" onClick={() => setReview(null)}>{t("action.close")}</button>
            )
          }
        >
          {!review.approved && <div className="banner-pending mb-4"><IconAlert style={{ width: 18, height: 18 }} />{t("rep.reviewPendingBanner")}</div>}
          {review.forAllLines ? (
            <p style={{ lineHeight: "var(--leading-6)" }}>{t("rep.willIncreasePrefix")} <b>{t("rep.allTransportLines")}</b> {t("rep.byAmount")} <b style={{ color: "var(--color-brand)" }}>{review.amount}{review.mode === "percent" ? "%" : " ج.م"}</b> {t("rep.effectiveFrom")} {review.startDate}.</p>
          ) : (
            <div className="table-wrap">
              <table className="data">
                <thead><tr><th>{t("rep.line")}</th><th>{t("rep.before")}</th><th>{t("rep.after")}</th></tr></thead>
                <tbody>
                  {review.lines.map((l, i) => (
                    <tr key={i}><td>{l.lineName}</td><td style={{ color: "var(--color-danger)" }}>{l.before} ج.م</td><td style={{ color: "var(--color-success)" }}>{l.after} ج.م</td></tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </Dialog>
      )}

      {confirmApprove && (
        <ConfirmDialog
          title={t("rep.confirmApproveTitle")}
          tone="success"
          message={confirmApprove.forAllLines
            ? `${t("rep.confirmAllPrefix")} ${confirmApprove.amount}${confirmApprove.mode === "percent" ? "%" : " ج.م"} ${t("rep.effectiveFrom")} ${confirmApprove.startDate}.`
            : `${t("rep.confirmSelectedPrefix")} ${confirmApprove.lines.length} ${t("rep.linesUnit")} ${t("rep.effectiveFrom")} ${confirmApprove.startDate}.`}
          confirmLabel={t("action.approve")}
          onConfirm={() => approve(confirmApprove)}
          onClose={() => setConfirmApprove(null)}
        />
      )}
    </div>
  );
}

const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";

interface PickLine { id: string; name: string }
interface PickRoute { id: string; name: string; lineId: string; cost: number }
interface LineRow { Id: number | string; Name?: string }
interface RouteRow { Id: number | string; NameOfRoute?: string; LineId?: number | string; LineCost?: number | string }

function CreateRepricing({ onClose, onCreated }: { onClose: () => void; onCreated: () => Promise<void> }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [scope, setScope] = useState<"all" | "selected">("selected");
  const [mode, setMode] = useState<"percent" | "fixed">("percent");
  const [round5, setRound5] = useState(false);
  const [amount, setAmount] = useState("");
  const [startDate, setStartDate] = useState("");
  const [lineFilter, setLineFilter] = useState<string | undefined>(); // محطة التجمع → narrows the route picker
  const [selected, setSelected] = useState<string[]>([]); // chosen route ids
  const [picker, setPicker] = useState<string | undefined>();
  const [confirm, setConfirm] = useState(false);
  const [lines, setLines] = useState<PickLine[]>([]); // محطات التجمع
  const [routes, setRoutes] = useState<PickRoute[]>([]); // خطوط

  useEffect(() => {
    (async () => {
      try {
        const [lineRes, routeRes] = await Promise.all([
          apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 200 }),
          apiGet<RouteRow[]>("getAllTransportationRoute", { PageNo: 1, NoOfItems: 500 }),
        ]);
        setLines((lineRes.Data ?? []).map((l) => ({ id: String(l.Id), name: l.Name ?? "" })));
        setRoutes((routeRes.Data ?? []).map((r) => ({
          id: String(r.Id),
          name: r.NameOfRoute ?? "",
          lineId: r.LineId != null ? String(r.LineId) : "",
          cost: Number(r.LineCost) || 0,
        })));
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  const preview = (before: number) => {
    let after = mode === "percent" ? before * (1 + Number(amount) / 100) : before + Number(amount);
    if (round5) after = Math.round(after / 5) * 5;
    return Math.round(after);
  };

  // Dropdown of routes (خطوط), optionally scoped to a محطة تجمع, excluding already-picked.
  const routeOptions = routes
    .filter((r) => !lineFilter || r.lineId === lineFilter)
    .filter((r) => !selected.includes(r.id))
    .map((r) => ({ value: r.id, label: r.name }));

  const addRoute = (id?: string) => { if (id && !selected.includes(id)) setSelected((s) => [...s, id]); setPicker(undefined); };
  const removeRoute = (id: string) => setSelected((s) => s.filter((x) => x !== id));

  const create = async () => {
    if (!amount) { toast(t("rep.enterAmount"), "error"); return; }
    if (scope === "selected" && selected.length === 0) { toast(t("rep.selectAtLeastOneLine"), "error"); return; }
    try {
      await createRepricing({
        amount: Number(amount),
        mode,
        forAllLines: scope === "all",
        approximateToFive: round5,
        startDate,
        lineIds: scope === "all" ? [] : selected,
      });
      toast(t("rep.createdToast"));
      setConfirm(false);
      await onCreated();
      onClose();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    }
  };

  return (
    <>
      <Dialog
        title={t("rep.create")}
        size="xl"
        onClose={onClose}
        footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-brand" onClick={() => setConfirm(true)}>{t("action.save")}</button></>}
      >
        <div className="two-panel">
          <div className="field">
            <span className="label" id="rp-scope-label">{t("rep.scope")}</span>
            <SegmentedControl<"all" | "selected">
              value={scope}
              onChange={setScope}
              ariaLabelledby="rp-scope-label"
              segments={[
                { value: "selected", label: t("rep.selectedLines") },
                { value: "all", label: t("rep.allLines") },
              ]}
            />
          </div>
          <div className="field">
            <span className="label" id="rp-mode-label">{t("rep.method")}</span>
            <SegmentedControl<"percent" | "fixed">
              value={mode}
              onChange={setMode}
              ariaLabelledby="rp-mode-label"
              segments={[
                { value: "percent", label: <><IconPercent style={{ width: 14, height: 14 }} /> {t("rep.percent")}</> },
                { value: "fixed", label: <><IconMoney style={{ width: 14, height: 14 }} /> {t("rep.fixedAmount")}</> },
              ]}
            />
          </div>
        </div>
        <div className="two-panel">
          <Field label={t("rep.amount")} required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} placeholder={mode === "percent" ? "10" : "50"} /></Field>
          <Field label={t("rep.applyDate")}><Input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} /></Field>
        </div>
        <label className="checkbox-row mb-4"><input type="checkbox" checked={round5} onChange={(e) => setRound5(e.target.checked)} />{t("rep.round5Label")} {preview(823)})</label>

        {scope === "selected" ? (
          <div className="card card-pad">
            <div className="section-title mb-4">{t("rep.selectLines")}</div>
            {/* Pick a محطة تجمع to narrow the routes, then choose routes from the dropdown */}
            <div className="two-panel">
              <Field label={t("filter.line")}>
                <Combobox options={lines.map((l) => ({ value: l.id, label: l.name }))} value={lineFilter} onChange={(v) => setLineFilter(v)} placeholder={t("filter.all")} />
              </Field>
              <Field label={t("filter.route")}>
                <Combobox options={routeOptions} value={picker} onChange={(v) => addRoute(v)} placeholder={t("rep.selectRouteToAdd")} />
              </Field>
            </div>

            {selected.length === 0 ? (
              <p className="muted text-sm">{t("rep.noRoutesSelected")}</p>
            ) : (
              <div className="table-wrap">
                <table className="data">
                  <thead><tr><th>#</th><th>{t("rep.line")}</th><th>{t("costs.cost")}</th><th>{t("rep.before")}</th><th>{t("rep.after")}</th><th></th></tr></thead>
                  <tbody>
                    {selected.map((id, i) => {
                      const r = routes.find((x) => x.id === id);
                      if (!r) return null;
                      return (
                        <tr key={id}>
                          <td>{i + 1}</td>
                          <td style={{ color: "var(--text-heading)", fontWeight: 500 }}>{r.name}</td>
                          <td>{money(r.cost)}</td>
                          <td style={{ color: "var(--color-danger)" }}>{money(r.cost)}</td>
                          <td style={{ color: "var(--color-success)", fontWeight: 600 }}>{amount ? money(preview(r.cost)) : "—"}</td>
                          <td><button className="icon-btn danger" aria-label={t("action.delete")} onClick={() => removeRoute(id)}><IconTrash /></button></td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        ) : (
          <div className="banner-pending"><IconAlert style={{ width: 18, height: 18 }} />{t("rep.willIncreasePrefix")} {t("rep.allTransportLines")} ({routes.length}).</div>
        )}
      </Dialog>

      {confirm && (
        <ConfirmDialog
          title={t("rep.confirmCreateTitle")}
          tone="brand"
          message={t("rep.confirmCreateMsg")}
          confirmLabel={t("action.confirm")}
          onConfirm={create}
          onClose={() => setConfirm(false)}
        />
      )}
    </>
  );
}
