"use client";

import { useState, useEffect, useCallback } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can } from "@/lib/types";
import { monthNamesAr } from "@/lib/data";
import type { StatementRow, SupplierPayment, Supplier, RouteItem } from "@/lib/types";
import { getMonthlyStatement, getPayments, addPayment, getSuppliers, downloadStatementExcel, getSupplierRounds, type SupplierRound } from "@/lib/services/suppliers";
import { getRoutes } from "@/lib/services/routes";
import { addDeduction } from "@/lib/services/deductions";
import { saveBlob } from "@/lib/download";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import Dialog from "@/components/ui/Dialog";
import Combobox from "@/components/ui/Combobox";
import { Field, Input, Select, Textarea } from "@/components/ui/Field";
import { LoadingState, EmptyState } from "@/components/ui/EmptyState";
import { IconMoney, IconReceipt, IconEye, IconPrint, IconDownload, IconRoute } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);
// Remaining = total due minus deductions and payments.
const remaining = (r: StatementRow) => r.totalDue - r.totalDeductions - r.normalPayments - r.advancePayments;
const money = (n: number) => n.toLocaleString("ar-EG") + " ج.م";

export default function StatementPage() {
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const canPay = can(user?.role, "add.payment");
  const canPrint = can(user?.role, "print.invoice");

  const [rows, setRows] = useState<StatementRow[]>([]);
  const [loading, setLoading] = useState(true);
  const [suppliersList, setSuppliersList] = useState<Supplier[]>([]);
  const [routeOpts, setRouteOpts] = useState<{ value: string; label: string }[]>([]);
  const [pay, setPay] = useState<StatementRow | null>(null);
  const [deduct, setDeduct] = useState<StatementRow | null>(null);
  const [rounds, setRounds] = useState<StatementRow | null>(null);
  const [invoice, setInvoice] = useState<StatementRow | null>(null);
  const [viewPayments, setViewPayments] = useState<StatementRow | null>(null);
  const [supplier, setSupplier] = useState<string | undefined>();
  const [route, setRoute] = useState<string | undefined>();
  // Defaults follow the current calendar month/year (RULES §4).
  const now = new Date();
  const [month, setMonth] = useState(now.getMonth() + 1);
  const [year, setYear] = useState(now.getFullYear());
  const years = Array.from({ length: 5 }, (_, i) => now.getFullYear() - i);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getMonthlyStatement({ supplierId: supplier, routeId: route, month, year, pageNo: 1, noOfItems: 100 });
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [supplier, route, month, year, toast, t]);

  useEffect(() => { load(); }, [load]);

  const [downloading, setDownloading] = useState(false);
  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const blob = await downloadStatementExcel({ supplierId: supplier, routeId: route, month, year });
      saveBlob(blob, "supplier-statement.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [supplier, route, month, year, toast, t]);

  // Supplier dropdown.
  useEffect(() => {
    (async () => {
      try {
        const res = await getSuppliers({ pageNo: 1, noOfItems: 200 });
        setSuppliersList(res.items);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [toast, t]);

  // Route dropdown, scoped to the selected supplier.
  useEffect(() => {
    (async () => {
      try {
        const res = await getRoutes({ supplierId: supplier, pageNo: 1, noOfItems: 500 });
        setRouteOpts(res.items.map((r) => ({ value: r.id, label: r.name })));
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [supplier, toast, t]);

  const columns: Column<StatementRow>[] = [
    { key: "month", header: t("filter.month"), render: (r) => monthNamesAr[r.month - 1] },
    { key: "supplier", header: t("filter.supplier"), render: (r) => <b style={{ color: "var(--text-heading)" }}>{r.supplier}</b> },
    { key: "routesCount", header: t("stmt.routes"), align: "center", priority: "secondary" },
    { key: "rounds", header: t("stmt.roundsHeader"), priority: "secondary", render: (r) => `${r.roundsFull} / ${r.roundsHalfGo} / ${r.roundsHalfReturn}` },
    { key: "totalDue", header: t("stmt.totalDue"), render: (r) => money(r.totalDue) },
    { key: "totalDeductions", header: t("stmt.deductions"), priority: "secondary", render: (r) => <span style={{ color: "var(--color-danger)" }}>{money(r.totalDeductions)}</span> },
    { key: "normalPayments", header: t("stmt.normalPayments"), priority: "secondary", render: (r) => <span style={{ color: "var(--color-success)" }}>{money(r.normalPayments)}</span> },
    { key: "advancePayments", header: t("stmt.advancePayments"), priority: "secondary", render: (r) => money(r.advancePayments) },
    { key: "remaining", header: t("stmt.remaining"), render: (r) => <b style={{ color: remaining(r) > 0 ? "var(--color-danger)" : "var(--color-success)" }}>{money(remaining(r))}</b> },
    {
      key: "actions", header: "", align: "end",
      render: (r) => (
        <div className="cell-actions">
          {canPay && <button className="icon-btn" style={{ color: "var(--color-success)" }} title={t("stmt.addPayment")} onClick={() => setPay(r)}><IconMoney /></button>}
          {canPay && <button className="icon-btn danger" title={t("stmt.addDeduction")} onClick={() => setDeduct(r)}><IconReceipt /></button>}
          <button className="icon-btn" title={t("stmt.viewRounds")} aria-label={t("stmt.viewRounds")} onClick={() => setRounds(r)}><IconRoute /></button>
          <button className="icon-btn brand" title={t("stmt.viewPayments")} onClick={() => setViewPayments(r)}><IconEye /></button>
          {canPrint && <button className="icon-btn" title={t("action.print")} onClick={() => setInvoice(r)}><IconPrint /></button>}
        </div>
      ),
    },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.statement")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("action.downloadExcel")}</button>
      </PageHeader>

      {loading ? (
        <LoadingState />
      ) : (
        <DataTable
          columns={columns}
          rows={rows}
          emptyMessage={t("empty.statement")}
          toolbar={
            <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
              <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 200 }}>
                <Combobox options={suppliersList.map((s) => ({ value: s.id, label: s.name }))} value={supplier} onChange={(v) => { setSupplier(v); setRoute(undefined); }} placeholder={t("filter.all")} />
              </Field>
              <Field label={t("filter.route")} style={{ margin: 0, minWidth: 200 }}>
                <Combobox options={routeOpts} value={route} onChange={setRoute} placeholder={t("filter.all")} />
              </Field>
              <Field label={t("filter.month")} style={{ margin: 0, minWidth: 150 }}>
                <Select value={month} onChange={(e) => setMonth(Number(e.target.value))}>
                  {monthNamesAr.map((m, i) => <option key={i} value={i + 1}>{m}</option>)}
                </Select>
              </Field>
              <Field label={t("filter.year")} style={{ margin: 0, minWidth: 120 }}>
                <Select value={year} onChange={(e) => setYear(Number(e.target.value))}>
                  {years.map((y) => <option key={y} value={y}>{y}</option>)}
                </Select>
              </Field>
            </div>
          }
        />
      )}

      {pay && <AddPaymentDialog row={pay} onClose={() => setPay(null)} onSaved={load} />}
      {deduct && <AddDeductionDialog row={deduct} onClose={() => setDeduct(null)} onSaved={load} />}
      {invoice && <InvoiceDialog row={invoice} onClose={() => setInvoice(null)} />}
      {viewPayments && <PaymentsListDialog row={viewPayments} onClose={() => setViewPayments(null)} />}
      {rounds && <RoundsDialog row={rounds} month={month} year={year} onClose={() => setRounds(null)} />}
    </div>
  );
}

function AddPaymentDialog({ row, onClose, onSaved }: { row: StatementRow; onClose: () => void; onSaved: () => Promise<void> }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [amount, setAmount] = useState("");
  const [date, setDate] = useState(new Date().toISOString().slice(0, 10));
  const [type, setType] = useState<"advance" | "normal">("normal");
  const [months, setMonths] = useState("3");
  const [startDate, setStartDate] = useState("");
  const [saving, setSaving] = useState(false);

  const submit = async () => {
    if (!amount) { toast(t("stmt.enterAmount"), "error"); return; }
    if (type === "advance" && (Number(months) < 1 || Number(months) > 12)) { toast(t("stmt.monthsRange"), "error"); return; }
    setSaving(true);
    try {
      await addPayment({
        supplierId: row.supplierId,
        amount: Number(amount),
        paymentDate: date,
        type,
        months: type === "advance" ? Number(months) : undefined,
        startDate: type === "advance" ? startDate || undefined : undefined,
      });
      toast(t("stmt.paymentSaved"));
      await onSaved();
      onClose();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog
      title={`💰 ${t("stmt.payment")} — ${row.supplier}`}
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-success" onClick={submit} disabled={saving}>{t("stmt.pay")}</button></>}
    >
      <Field label={t("common.amount")} required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} /></Field>
      <Field label={t("stmt.paymentDate")}><Input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></Field>
      <fieldset className="field" style={{ border: 0, padding: 0, margin: "0 0 var(--space-4)", minInlineSize: 0 }}>
        <legend className="label" style={{ padding: 0 }}>{t("common.type")}</legend>
        <div className="row gap-4">
          <label className="checkbox-row"><input type="radio" name="pay-type" checked={type === "advance"} onChange={() => setType("advance")} />{t("stmt.advanceInstallments")}</label>
          <label className="checkbox-row"><input type="radio" name="pay-type" checked={type === "normal"} onChange={() => setType("normal")} />{t("stmt.monthlyPayment")}</label>
        </div>
      </fieldset>
      {type === "advance" && (
        <div className="row gap-3 wrap">
          <Field label={t("stmt.monthsCount")} className="grow"><Input inputMode="numeric" value={months} onChange={(e) => setMonths(e.target.value.replace(/\D/g, ""))} /></Field>
          <Field label={t("stmt.startDate")} className="grow"><Input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} /></Field>
        </div>
      )}
    </Dialog>
  );
}

function AddDeductionDialog({ row, onClose, onSaved }: { row: StatementRow; onClose: () => void; onSaved: () => Promise<void> }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [amount, setAmount] = useState("");
  const [reason, setReason] = useState("");
  const [date, setDate] = useState(new Date().toISOString().slice(0, 10));
  const [type, setType] = useState<"Normal" | "Taxes">("Normal");
  const [routeId, setRouteId] = useState<string | undefined>();
  const [routes, setRoutes] = useState<RouteItem[]>([]);
  const [saving, setSaving] = useState(false);

  // A deduction is booked against a specific route (serial) of this supplier.
  useEffect(() => {
    (async () => {
      try {
        const res = await getRoutes({ supplierId: row.supplierId, pageNo: 1, noOfItems: 500 });
        setRoutes(res.items);
        if (res.items.length === 1) setRouteId(res.items[0].id);
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      }
    })();
  }, [row.supplierId, toast, t]);

  const submit = async () => {
    if (!routeId) { toast(t("exc.selectRouteError"), "error"); return; }
    if (!amount) { toast(t("stmt.enterAmount"), "error"); return; }
    const route = routes.find((r) => r.id === routeId);
    setSaving(true);
    try {
      await addDeduction({
        SupplierId: row.supplierId,
        Serial: route?.serial ?? "",
        TransportationVehicleRouteId: routeId,
        DeductPerRound: Number(amount),
        Cause: reason,
        DateOfDeduction: date,
        TypeOfDedct: type,
      });
      toast(t("stmt.deductionSaved"));
      await onSaved();
      onClose();
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog
      title={`${t("stmt.deduction")} — ${row.supplier}`}
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button><button className="btn btn-danger" onClick={submit} disabled={saving}>{t("stmt.saveDeduction")}</button></>}
    >
      <Field label={t("filter.route")} required>
        <Combobox options={routes.map((r) => ({ value: r.id, label: r.serial ? `${r.name} (${r.serial})` : r.name }))} value={routeId} onChange={setRouteId} placeholder={t("exc.selectRoutePlaceholder")} />
      </Field>
      <div className="two-panel">
        <Field label={t("common.amount")} required><Input inputMode="decimal" value={amount} onChange={(e) => setAmount(e.target.value)} /></Field>
        <Field label={t("stmt.paymentDate")}><Input type="date" value={date} onChange={(e) => setDate(e.target.value)} /></Field>
      </div>
      <Field label={t("common.type")}>
        <Select value={type} onChange={(e) => setType(e.target.value as "Normal" | "Taxes")}>
          <option value="Normal">{t("ded.normal")}</option>
          <option value="Taxes">{t("ded.tax")}</option>
        </Select>
      </Field>
      <Field label={t("common.reason")}><Textarea value={reason} onChange={(e) => setReason(e.target.value)} /></Field>
    </Dialog>
  );
}

function InvoiceDialog({ row, onClose }: { row: StatementRow; onClose: () => void }) {
  const { t } = useLocale();
  const rem = remaining(row);
  return (
    <Dialog
      title={t("stmt.invoicePreview")}
      size="lg"
      onClose={onClose}
      footer={<><button className="btn btn-secondary" onClick={onClose}>{t("action.close")}</button><button className="btn btn-brand" onClick={() => window.print()}><IconPrint />{t("action.print")}</button></>}
    >
      <div className="invoice-print card card-pad">
        <div className="row-between mb-4" style={{ borderBottom: "2px solid var(--color-navy)", paddingBottom: "var(--space-3)" }}>
          <div><div className="page-title">{t("stmt.companyName")}</div><div className="muted text-sm">{t("stmt.invoiceSubtitle")}</div></div>
          <div className="text-sm" style={{ textAlign: "end" }}><div>{t("filter.month")}: {monthNamesAr[row.month - 1]} {row.year}</div><div>{t("filter.supplier")}: <b>{row.supplier}</b></div></div>
        </div>
        <table className="data" style={{ marginBottom: "var(--space-4)" }}>
          <thead><tr><th>{t("stmt.item")}</th><th>{t("stmt.count")}</th></tr></thead>
          <tbody>
            <tr><td>{t("stmt.roundsFull")}</td><td>{row.roundsFull}</td></tr>
            <tr><td>{t("stmt.halfGo")}</td><td>{row.roundsHalfGo}</td></tr>
            <tr><td>{t("stmt.halfReturn")}</td><td>{row.roundsHalfReturn}</td></tr>
            <tr><td>{t("stmt.routesCount")}</td><td>{row.routesCount}</td></tr>
          </tbody>
        </table>
        <div className="stack" style={{ gap: 6, maxWidth: 340, marginInlineStart: "auto" }}>
          <div className="row-between"><span>{t("stmt.totalDue")}</span><b>{money(row.totalDue)}</b></div>
          <div className="row-between"><span>{t("stmt.totalDeductions")}</span><span style={{ color: "var(--color-danger)" }}>{money(row.totalDeductions)}</span></div>
          <div className="row-between"><span>{t("stmt.invoiceNormalPayments")}</span><span style={{ color: "var(--color-success)" }}>{money(row.normalPayments)}</span></div>
          <div className="row-between"><span>{t("stmt.invoiceAdvancePayments")}</span><span>{money(row.advancePayments)}</span></div>
          <div className="row-between" style={{ borderTop: "1px solid var(--border-base)", paddingTop: 6 }}><b>{t("stmt.remaining")}</b><b style={{ color: rem > 0 ? "var(--color-danger)" : "var(--color-success)" }}>{money(rem)}</b></div>
        </div>
        <div className="row-between mt-4" style={{ marginTop: "var(--space-12)" }}>
          <div className="col" style={{ gap: 24 }}><span className="muted">{t("stmt.supplierSignature")}</span><span>_______________</span></div>
          <div className="col" style={{ gap: 24, textAlign: "end" }}><span className="muted">{t("stmt.companySignature")}</span><span>_______________</span></div>
        </div>
      </div>
    </Dialog>
  );
}

function PaymentsListDialog({ row, onClose }: { row: StatementRow; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [payments, setPayments] = useState<SupplierPayment[]>([]);
  const [loading, setLoading] = useState(true);
  const [dist, setDist] = useState<SupplierPayment | null>(null);

  useEffect(() => {
    (async () => {
      setLoading(true);
      try {
        setPayments(await getPayments(row.supplierId));
      } catch (e) {
        toast(errMsg(e, t("empty.generic")), "error");
      } finally {
        setLoading(false);
      }
    })();
  }, [row.supplierId, toast, t]);

  return (
    <>
      <Dialog
        title={`${t("stmt.payments")} — ${row.supplier}`}
        size="lg"
        onClose={onClose}
        footer={<button className="btn btn-secondary" onClick={onClose}>{t("action.close")}</button>}
      >
        {loading ? (
          <LoadingState />
        ) : payments.length === 0 ? (
          <p className="muted">{t("stmt.noPayments")}</p>
        ) : (
          <div className="table-wrap">
            <table className="data">
              <thead><tr><th>{t("common.amount")}</th><th>{t("stmt.paymentDate")}</th><th>{t("stmt.startDate")}</th><th>{t("stmt.months")}</th><th>{t("common.type")}</th><th></th></tr></thead>
              <tbody>
                {payments.map((p) => (
                  <tr key={p.id}>
                    <td><b>{money(p.amount)}</b></td>
                    <td>{p.paymentDate}</td>
                    <td>{p.startDate ?? "—"}</td>
                    <td>{p.months ?? "—"}</td>
                    <td>{p.type === "advance" ? <span className="badge badge-blue">{t("stmt.advanceBadge")}</span> : <span className="badge badge-gray">{t("stmt.monthlyBadge")}</span>}</td>
                    <td style={{ textAlign: "end" }}>
                      {p.type === "advance" && p.distribution && p.distribution.length > 0 && (
                        <button className="icon-btn brand" title={t("stmt.viewDistribution")} aria-label={t("stmt.viewDistribution")} onClick={() => setDist(p)}><IconEye /></button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Dialog>

      {dist && <DistributionDialog payment={dist} onClose={() => setDist(null)} />}
    </>
  );
}

function DistributionDialog({ payment, onClose }: { payment: SupplierPayment; onClose: () => void }) {
  const { t } = useLocale();
  return (
    <Dialog
      title={t("stmt.distributionTitle")}
      onClose={onClose}
      footer={<button className="btn btn-secondary" onClick={onClose}>{t("action.close")}</button>}
    >
      <div className="table-wrap">
        <table className="data">
          <thead><tr><th>{t("filter.month")}</th><th>{t("filter.year")}</th><th>{t("common.amount")}</th></tr></thead>
          <tbody>
            {(payment.distribution ?? []).map((d, i) => (
              <tr key={i}><td>{monthNamesAr[d.month - 1]}</td><td>{d.year}</td><td>{money(d.amount)}</td></tr>
            ))}
          </tbody>
        </table>
      </div>
    </Dialog>
  );
}

// Per-day round detail for a supplier's month — the "days in month" view, with
// the route Serial column (mirrors the base app's supplier rounds report).
function RoundsDialog({ row, month, year, onClose }: { row: StatementRow; month: number; year: number; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [list, setList] = useState<SupplierRound[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const res = await getSupplierRounds({ supplierId: row.supplierId, month, year, pageNo: 1, noOfItems: 500 });
        if (alive) setList(res.items);
      } catch (e) {
        if (alive) toast(errMsg(e, t("empty.generic")), "error");
      } finally {
        if (alive) setLoading(false);
      }
    })();
    return () => { alive = false; };
  }, [row.supplierId, month, year, toast, t]);

  return (
    <Dialog title={`${t("stmt.viewRounds")} — ${row.supplier}`} size="lg" onClose={onClose}>
      {loading ? (
        <LoadingState />
      ) : list.length === 0 ? (
        <EmptyState message={t("stmt.noRounds")} />
      ) : (
        <div className="table-wrap">
          <table className="data">
            <thead>
              <tr>
                <th>#</th>
                <th>{t("filter.route")}</th>
                <th>{t("filter.serial")}</th>
                <th>{t("common.date")}</th>
                <th>{t("att.checkIn")}</th>
                <th>{t("att.checkOut")}</th>
                <th>{t("common.amount")}</th>
              </tr>
            </thead>
            <tbody>
              {list.map((r, i) => (
                <tr key={i}>
                  <td>{i + 1}</td>
                  <td>{r.routeName || "—"}</td>
                  <td>{r.serial || "—"}</td>
                  <td>{r.dateOfRound || "—"}</td>
                  <td>{r.checkIn || "—"}</td>
                  <td>{r.checkOut || "—"}</td>
                  <td>{money(r.price)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Dialog>
  );
}
