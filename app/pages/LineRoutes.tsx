"use client";

import { useParams } from "react-router-dom";
import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { canAdd } from "@/lib/types";
import { apiGet } from "@/lib/api/client";
import { getRoutes } from "@/lib/services/routes";
import type { RouteItem } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import RouteForm from "@/components/routes/RouteForm";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import { IconPlus, IconRoute, IconChevronEnd, IconEye, IconClock, IconUser } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

interface LineRow { Id: number; Name: string }

export default function LineRoutesPage() {
  const { id = "", locale = "ar" } = useParams();
  const { t } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const p = (path: string) => `/${locale}${path}`;

  const [routes, setRoutes] = useState<RouteItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [lineName, setLineName] = useState<string>("");
  const [adding, setAdding] = useState(false);
  const [details, setDetails] = useState<RouteItem | null>(null);

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const res = await getRoutes({ lineId: id });
      setRoutes(res.items);
      if (res.items[0]?.lineName) setLineName(res.items[0].lineName);
    } catch (e) {
      toast(errMsg(e, t("empty.routes")), "error");
    } finally {
      setLoading(false);
    }
  }, [id, toast, t]);

  useEffect(() => { load(); }, [load]);

  // Resolve the line's name for the header/breadcrumb (best-effort).
  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const res = await apiGet<LineRow[]>("getAllTransportationLine", { PageNo: 1, NoOfItems: 200 });
        if (!alive) return;
        const match = (res.Data ?? []).find((l) => String(l.Id) === id);
        if (match) setLineName(match.Name);
      } catch {
        /* fall back to name derived from routes */
      }
    })();
    return () => { alive = false; };
  }, [id]);

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link to={p("/lines")} style={{ color: "var(--color-brand)" }}>{t("nav.lines")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{lineName || id}</span>
      </div>

      <PageHeader title={`${t("filter.route")} — ${lineName}`} count={routes.length}>
        {canAdd(user?.role) && <button className="btn btn-brand btn-sm" onClick={() => setAdding(true)}><IconPlus />{t("action.addRoute")}</button>}
      </PageHeader>

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
                <span className="info-chip">{t("common.bus")} {r.serial}</span>
              </div>
              <div>
                <div className="section-title">{r.name}</div>
                <div className="muted text-sm row" style={{ gap: 6 }}><IconUser style={{ width: 14, height: 14 }} />{r.supervisor}</div>
              </div>
              <div className="row gap-3 wrap text-xs">
                <span className="info-chip">{t("route.capacity")}: {r.usersInRoute}/{r.fullCapacity}</span>
                <span className="info-chip">{t("route.attendance")}: {r.attended}/{r.actualCapacity}</span>
                <span className="info-chip">{r.stationCount} {t("common.stations")}</span>
              </div>
              <div className="row gap-2">
                <Link to={p(`/lines/${id}/routes/${r.id}`)} className="btn btn-secondary btn-sm grow" style={{ justifyContent: "center" }}>{t("action.details")}</Link>
                <button className="icon-btn" onClick={() => setDetails(r)} aria-label={t("action.details")}><IconEye /></button>
              </div>
            </div>
          ))}
        </div>
      )}

      {adding && (
        <RouteForm
          lineId={id}
          lineName={lineName}
          onClose={() => setAdding(false)}
          onSaved={load}
        />
      )}

      {details && (
        <Dialog title={details.name} onClose={() => setDetails(null)}>
          <div className="stack" style={{ gap: "var(--space-2)" }}>
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
