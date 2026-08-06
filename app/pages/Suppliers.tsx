"use client";

import { useState, useEffect, useCallback } from "react";
import { Link } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { useToast } from "@/contexts/ToastContext";
import { can, type Supplier } from "@/lib/types";
import { getSuppliers } from "@/lib/services/suppliers";
import { formatDate } from "@/lib/datetime";
import PageHeader from "@/components/ui/PageHeader";
import { EmptyState, LoadingState } from "@/components/ui/EmptyState";
import { IconPlus, IconBuilding, IconEye, IconSearch } from "@/components/ui/Icons";

const errMsg = (e: unknown, fallback: string) => (e instanceof Error && e.message ? e.message : fallback);

export default function SuppliersPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const { toast } = useToast();
  const [query, setQuery] = useState("");
  const [rows, setRows] = useState<Supplier[]>([]);
  const [loading, setLoading] = useState(true);
  const p = (path: string) => `/${locale}${path}`;

  const load = useCallback(async () => {
    setLoading(true);
    try {
      // The search box maps to the Name filter header (backend filters by contains).
      const res = await getSuppliers({ pageNo: 1, noOfItems: 100, name: query.trim() || undefined });
      setRows(res.items);
    } catch (e) {
      toast(errMsg(e, t("empty.generic")), "error");
    } finally {
      setLoading(false);
    }
  }, [toast, t, query]);

  useEffect(() => { load(); }, [load]);

  return (
    <div className="stack">
      <PageHeader title={t("nav.suppliers")} count={rows.length}>
        {can(user?.role, "create.supplier") && <Link to={p("/suppliers/new")} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>}
      </PageHeader>

      <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>
        <div className="row gap-2" style={{ maxWidth: 360, border: "1px solid var(--border-base)", borderRadius: "var(--radius-base)", padding: "4px 10px" }}>
          <IconSearch style={{ width: 16, height: 16, color: "var(--gray-400)" }} />
          <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder={t("supp.searchPlaceholder")} style={{ border: "none", outline: "none", width: "100%", fontSize: "var(--text-sm)" }} />
        </div>
      </div>

      {loading ? (
        <LoadingState />
      ) : rows.length === 0 ? (
        <div className="card"><EmptyState message={t("empty.generic")} /></div>
      ) : (
        <div className="card-grid">
          {rows.map((s) => (
            <Link key={s.id} to={p(`/suppliers/${s.id}`)} className="card card-pad stack" style={{ gap: "var(--space-3)" }}>
              <div className="row-between">
                <span className="kpi-icon"><IconBuilding /></span>
                <IconEye style={{ width: 18, height: 18, color: "var(--text-muted)" }} />
              </div>
              <div>
                <div className="section-title">{s.name}</div>
                <div className="muted text-sm">{t("supp.createdOn")} {formatDate(s.createdAt)}</div>
              </div>
              <div className="row gap-2 wrap text-xs">
                <span className="info-chip">{s.activeRoutes} {t("supp.activeRoutesSuffix")}</span>
                <span className="info-chip">{s.contacts.length} {t("supp.driversSuffix")}</span>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
