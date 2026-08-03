"use client";

import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { canAdd } from "@/lib/types";
import { apiGet, type PaginationHeader } from "@/lib/api/client";
import { getRoutes, getRouteUsers, downloadRoutesWithUsersExcel } from "@/lib/services/routes";
import { saveBlob } from "@/lib/download";
import type { RouteItem, PassengerAssignment, Period } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import Combobox, { type Option } from "@/components/ui/Combobox";
import RouteForm from "@/components/routes/RouteForm";
import { Field, Input } from "@/components/ui/Field";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import { IconPlus, IconRoute, IconEye, IconClock, IconUser, IconUsers, IconDownload } from "@/components/ui/Icons";

const periodLabel: Record<Period, string> = { go: "period.go", return: "period.return", both: "period.both" };

const PAGE_SIZE = 12;

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

interface LineRow { Id: number; Name: string }
interface SupplierRow { Id: number; Name: string }

// Top-level list of ALL routes (خطوط) across every line — the target of the
// dashboard "Routes" KPI card (PRD FR-DB2).
export default function AllRoutesPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const p = (path: string) => `/${locale}${path}`;

  const [routes, setRoutes] = useState<RouteItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [line, setLine] = useState<string | undefined>();
  const [supplier, setSupplier] = useState<string | undefined>();
  const [search, setSearch] = useState("");
  const [pageNo, setPageNo] = useState(1);
  const [pagination, setPagination] = useState<PaginationHeader | null>(null);
  const [adding, setAdding] = useState(false);
  const [details, setDetails] = useState<RouteItem | null>(null);
  const [passengersFor, setPassengersFor] = useState<RouteItem | null>(null);
  const [lineOptions, setLineOptions] = useState<Option[]>([]);
  const [supplierOptions, setSupplierOptions] = useState<Option[]>([]);
  const [downloading, setDownloading] = useState(false);

  const onDownloadExcel = useCallback(async () => {
    setDownloading(true);
    try {
      const blob = await downloadRoutesWithUsersExcel();
      saveBlob(blob, "routes-with-passengers.xlsx");
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setDownloading(false);
    }
  }, [toast, t]);

  // Debounce the name search so each keystroke doesn't fire a request.
  const [nameFilter, setNameFilter] = useState("");
  useEffect(() => {
    const id = setTimeout(() => setNameFilter(search.trim()), 300);
    return () => clearTimeout(id);
  }, [search]);

  // Any filter change resets to the first page.
  useEffect(() => { setPageNo(1); }, [line, supplier, nameFilter]);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      // All filters + pagination are server-side (headers on getAllTransportationRoute).
      const res = await getRoutes({ lineId: line, supplierId: supplier, name: nameFilter || undefined, pageNo, noOfItems: PAGE_SIZE });
      setRoutes(res.items);
      setPagination(res.pagination);
    } catch (e) {
      toast(errMsg(e, t("empty.routes")), "error");
    } finally {
      setLoading(false);
    }
  }, [line, supplier, nameFilter, pageNo, toast, t]);

  useEffect(() => { load(); }, [load]);

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const [linesRes, suppliersRes] = await Promise.all([
          apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 200 }),
          apiGet<SupplierRow[]>("getSuppliers", { PageNo: 1, NoOfItems: 200 }),
        ]);
        if (!alive) return;
        setLineOptions((linesRes.Data ?? []).map((l) => ({ value: String(l.Id), label: l.Name })));
        setSupplierOptions((suppliersRes.Data ?? []).map((s) => ({ value: String(s.Id), label: s.Name })));
      } catch {
        /* filter dropdowns are best-effort; the list still loads */
      }
    })();
    return () => { alive = false; };
  }, []);

  return (
    <div className="stack">
      <PageHeader title={t("nav.routes")} count={pagination?.TotalItems ?? routes.length}>
        <button className="btn btn-secondary btn-sm" onClick={onDownloadExcel} disabled={downloading || loading}><IconDownload />{t("route.downloadWithPassengers")}</button>
        {canAdd(user?.role) && <button className="btn btn-brand btn-sm" onClick={() => setAdding(true)}><IconPlus />{t("action.addRoute")}</button>}
      </PageHeader>

      <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>
        <div className="row wrap gap-3" style={{ alignItems: "flex-end" }}>
          <Field label={t("action.search")} style={{ margin: 0, minWidth: 220, flex: 1 }}>
            <Input value={search} onChange={(e) => setSearch(e.target.value)} placeholder={t("route.searchByName")} />
          </Field>
          <Field label={t("filter.line")} style={{ margin: 0, minWidth: 200, flex: 1 }}>
            <Combobox options={lineOptions} value={line} onChange={setLine} placeholder={t("filter.all")} />
          </Field>
          <Field label={t("filter.supplier")} style={{ margin: 0, minWidth: 200, flex: 1 }}>
            <Combobox options={supplierOptions} value={supplier} onChange={setSupplier} placeholder={t("filter.all")} />
          </Field>
        </div>
      </div>

      {loading ? (
        <LoadingState />
      ) : routes.length === 0 ? (
        <div className="card"><EmptyState message={t("empty.routes")} action={canAdd(user?.role) ? <button className="btn btn-brand btn-sm" onClick={() => setAdding(true)}><IconPlus />{t("action.addRoute")}</button> : undefined} /></div>
      ) : (
        <div className="card-grid">
          {routes.map((r) => (
            <div key={r.id} className="card card-pad stack" style={{ gap: "var(--space-3)" }}>
              <div className="row-between">
                <span className="kpi-icon"><IconRoute /></span>
                <span className="info-chip" title={t("filter.serial")}>{t("filter.serial")}: <b style={{ fontWeight: 700 }}>{r.serial || "—"}</b></span>
              </div>
              <div>
                <div className="section-title">{r.name}</div>
                <div className="muted text-sm">{r.lineName}</div>
                <div className="muted text-sm row" style={{ gap: 6 }}><IconUser style={{ width: 14, height: 14 }} />{r.supervisor}</div>
              </div>
              <div className="row gap-3 wrap text-xs">
                <span className="info-chip">{t("route.capacity")}: {r.usersInRoute}/{r.fullCapacity}</span>
                <span className="info-chip">{t("route.attendance")}: {r.attended}/{r.actualCapacity}</span>
                <span className="info-chip">{r.stationCount} {t("common.stations")}</span>
              </div>
              <div className="row gap-2">
                <Link to={p(`/lines/${r.lineId}/routes/${r.id}`)} className="btn btn-secondary btn-sm grow" style={{ justifyContent: "center" }}>{t("action.details")}</Link>
                <button className="icon-btn brand" onClick={() => setPassengersFor(r)} aria-label={`${t("common.passengers")}: ${r.name}`} title={t("common.passengers")}><IconUsers /></button>
                <button className="icon-btn" onClick={() => setDetails(r)} aria-label={t("action.details")}><IconEye /></button>
              </div>
            </div>
          ))}
        </div>
      )}

      {!loading && pagination && pagination.TotalPages > 1 && (
        <div className="pagination">
          <span className="muted text-sm">
            {(pagination.CurrentPage - 1) * PAGE_SIZE + 1}–{Math.min(pagination.CurrentPage * PAGE_SIZE, pagination.TotalItems)} {t("common_of")} {pagination.TotalItems}
          </span>
          <div className="pager">
            <button disabled={pageNo <= 1} onClick={() => setPageNo((n) => n - 1)} aria-label={t("common.prev")}>‹</button>
            {Array.from({ length: pagination.TotalPages }, (_, i) => i + 1)
              .filter((pg) => Math.abs(pg - pageNo) <= 2 || pg === 1 || pg === pagination.TotalPages)
              .map((pg, idx, arr) => (
                <span key={pg} className="row">
                  {idx > 0 && arr[idx - 1] !== pg - 1 && <span className="muted">…</span>}
                  <button className={pg === pageNo ? "active" : ""} aria-current={pg === pageNo ? "page" : undefined} onClick={() => setPageNo(pg)}>{pg}</button>
                </span>
              ))}
            <button disabled={pageNo >= pagination.TotalPages} onClick={() => setPageNo((n) => n + 1)} aria-label={t("common.next")}>›</button>
          </div>
        </div>
      )}

      {passengersFor && (
        <RoutePassengersDialog route={passengersFor} onClose={() => setPassengersFor(null)} />
      )}

      {adding && (
        <RouteForm
          onClose={() => setAdding(false)}
          onSaved={load}
        />
      )}

      {details && (
        <Dialog title={details.name} onClose={() => setDetails(null)}>
          <div className="stack" style={{ gap: "var(--space-2)" }}>
            <DetailRow label={t("filter.serial")} value={details.serial || "—"} />
            <DetailRow label={t("filter.line")} value={details.lineName} />
            <DetailRow label={t("filter.supplier")} value={details.supplier} />
            <DetailRow label={t("filter.driver")} value={details.driver} />
            <DetailRow label={t("common.supervisor")} value={details.supervisor} />
            <DetailRow label={t("route.fullCapacityPassengers")} value={`${details.fullCapacity} / ${details.usersInRoute}`} />
            <DetailRow label={t("route.actualCapacityAttendance")} value={`${details.actualCapacity} / ${details.attended}`} />
            <DetailRow label={t("route.stationsCount")} value={String(details.stationCount)} />
            <DetailRow label={t("route.times")} value={<span className="row" style={{ gap: 8 }}><IconClock style={{ width: 14, height: 14 }} />{details.fromTime ?? "—"} {details.toTime ? `→ ${details.toTime}` : ""}</span>} />
            <DetailRow label={t("route.otherIdentifier")} value={`C: ${details.religionC} · M: ${details.religionM}`} />
          </div>
        </Dialog>
      )}
    </div>
  );
}

function DetailRow({ label, value }: { label: string; value: React.ReactNode }) {
  return (
    <div className="row-between" style={{ padding: "6px 0", borderBottom: "1px solid var(--gray-100)" }}>
      <span className="muted text-sm">{label}</span>
      <span style={{ color: "var(--text-heading)", fontWeight: 500 }}>{value}</span>
    </div>
  );
}

// Popup listing every passenger registered on a route (GetTransportationEmployeeList).
function RoutePassengersDialog({ route, onClose }: { route: RouteItem; onClose: () => void }) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [rows, setRows] = useState<PassengerAssignment[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const list = await getRouteUsers(route.id);
        if (alive) setRows(list);
      } catch (e) {
        if (alive) toast(errMsg(e, t("route.noPassengers")), "error");
      } finally {
        if (alive) setLoading(false);
      }
    })();
    return () => { alive = false; };
  }, [route.id, toast, t]);

  return (
    <Dialog title={`${t("common.passengers")} — ${route.name}`} size="lg" onClose={onClose}>
      {loading ? (
        <LoadingState />
      ) : rows.length === 0 ? (
        <EmptyState message={t("route.noPassengers")} />
      ) : (
        <div className="table-wrap">
          <table className="data">
            <thead>
              <tr>
                <th>#</th>
                <th>{t("common.passenger")}</th>
                <th>{t("common.station")}</th>
                <th>{t("common.direction")}</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((a, i) => (
                <tr key={a.id}>
                  <td>{i + 1}</td>
                  <td style={{ color: "var(--text-heading)", fontWeight: 500 }}>{a.passengerName}</td>
                  <td>{a.stationName ?? "—"}</td>
                  <td><span className="badge badge-blue">{t(periodLabel[a.period])}</span></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Dialog>
  );
}
