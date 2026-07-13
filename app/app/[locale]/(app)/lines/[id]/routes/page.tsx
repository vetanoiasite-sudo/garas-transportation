"use client";

import { use, useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { canAdd } from "@/lib/types";
import { getLine, getRoutesForLine } from "@/lib/data";
import type { RouteItem } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import Dialog from "@/components/ui/Dialog";
import RouteForm from "@/components/routes/RouteForm";
import { EmptyState } from "@/components/ui/EmptyState";
import { IconPlus, IconRoute, IconChevronEnd, IconEye, IconClock, IconUser } from "@/components/ui/Icons";

export default function LineRoutesPage({ params }: { params: Promise<{ id: string; locale: string }> }) {
  const { id, locale } = use(params);
  const { t } = useLocale();
  const { user } = useAuth();
  const line = getLine(id);
  const [routes, setRoutes] = useState<RouteItem[]>(getRoutesForLine(id));
  const [adding, setAdding] = useState(false);
  const [details, setDetails] = useState<RouteItem | null>(null);
  const p = (path: string) => `/${locale}${path}`;

  return (
    <div className="stack">
      <div className="row text-sm muted" style={{ gap: 6 }}>
        <Link href={p("/lines")} style={{ color: "var(--color-brand)" }}>{t("nav.lines")}</Link>
        <IconChevronEnd style={{ width: 14, height: 14 }} />
        <span>{line?.name ?? id}</span>
      </div>

      <PageHeader title={`${t("filter.route")} — ${line?.name ?? ""}`} count={routes.length}>
        {canAdd(user?.role) && <button className="btn btn-brand btn-sm" onClick={() => setAdding(true)}><IconPlus />{t("action.addRoute")}</button>}
      </PageHeader>

      {routes.length === 0 ? (
        <div className="card"><EmptyState message={t("empty.routes")} action={canAdd(user?.role) ? <button className="btn btn-brand btn-sm" onClick={() => setAdding(true)}><IconPlus />{t("action.addRoute")}</button> : undefined} /></div>
      ) : (
        <div className="card-grid">
          {routes.map((r) => (
            <div key={r.id} className="card card-pad stack" style={{ gap: "var(--space-3)" }}>
              <div className="row-between">
                <span className="kpi-icon"><IconRoute /></span>
                <span className="info-chip">حافلة {r.serial}</span>
              </div>
              <div>
                <div className="section-title">{r.name}</div>
                <div className="muted text-sm row" style={{ gap: 6 }}><IconUser style={{ width: 14, height: 14 }} />{r.supervisor}</div>
              </div>
              <div className="row gap-3 wrap text-xs">
                <span className="info-chip">السعة: {r.usersInRoute}/{r.fullCapacity}</span>
                <span className="info-chip">حضور: {r.attended}/{r.actualCapacity}</span>
                <span className="info-chip">{r.stationCount} محطة</span>
              </div>
              <div className="row gap-2">
                <Link href={p(`/lines/${id}/routes/${r.id}`)} className="btn btn-secondary btn-sm grow" style={{ justifyContent: "center" }}>{t("action.details")}</Link>
                <button className="icon-btn" onClick={() => setDetails(r)} aria-label={t("action.details")}><IconEye /></button>
              </div>
            </div>
          ))}
        </div>
      )}

      {adding && (
        <RouteForm
          lineId={id}
          lineName={line?.name}
          onClose={() => setAdding(false)}
          onSaved={(r) => setRoutes((rs) => [...rs, buildRoute(r, id, line?.name ?? "")])}
        />
      )}

      {details && (
        <Dialog title={details.name} onClose={() => setDetails(null)}>
          <div className="stack" style={{ gap: "var(--space-2)" }}>
            <DetailRow label={t("filter.supplier")} value={details.supplier} />
            <DetailRow label={t("filter.driver")} value={details.driver} />
            <DetailRow label="المشرف" value={details.supervisor} />
            <DetailRow label="السعة الكاملة / الركاب" value={`${details.fullCapacity} / ${details.usersInRoute}`} />
            <DetailRow label="السعة الفعلية / الحضور" value={`${details.actualCapacity} / ${details.attended}`} />
            <DetailRow label="عدد المحطات" value={String(details.stationCount)} />
            <DetailRow label="المواعيد" value={<span className="row" style={{ gap: 8 }}><IconClock style={{ width: 14, height: 14 }} />{details.fromTime ?? "—"} {details.toTime ? `→ ${details.toTime}` : ""}</span>} />
            <DetailRow label="الديانة" value={`مسلم: ${details.religionM} · مسيحي: ${details.religionC}`} />
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

function buildRoute(r: Partial<RouteItem>, lineId: string, lineName: string): RouteItem {
  return {
    id: r.id ?? `r${Date.now()}`,
    lineId,
    lineName,
    name: r.name ?? "مسار جديد",
    serial: String(Math.floor(1000 + Math.random() * 9000)),
    supervisor: "—",
    supplier: "—",
    driver: "—",
    fullCapacity: 0, usersInRoute: 0, actualCapacity: 0, attended: 0, stationCount: 0,
    oneWay: r.oneWay ?? false,
    fromTime: r.fromTime, toTime: r.toTime,
    cost: r.cost ?? 0, religionM: 0, religionC: 0,
  };
}
