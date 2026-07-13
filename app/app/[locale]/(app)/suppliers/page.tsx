"use client";

import { useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { can } from "@/lib/types";
import { suppliers as seed } from "@/lib/data";
import PageHeader from "@/components/ui/PageHeader";
import { IconPlus, IconBuilding, IconEye, IconSearch } from "@/components/ui/Icons";

export default function SuppliersPage() {
  const { t, locale } = useLocale();
  const { user } = useAuth();
  const [query, setQuery] = useState("");
  const p = (path: string) => `/${locale}${path}`;

  const rows = seed.filter((s) => s.name.includes(query) || (s.mobile ?? "").includes(query) || (s.phone ?? "").includes(query));

  return (
    <div className="stack">
      <PageHeader title={t("nav.suppliers")} count={rows.length}>
        {can(user?.role, "create.supplier") && <Link href={p("/suppliers/new")} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>}
      </PageHeader>

      <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>
        <div className="row gap-2" style={{ maxWidth: 360, border: "1px solid var(--border-base)", borderRadius: "var(--radius-base)", padding: "4px 10px" }}>
          <IconSearch style={{ width: 16, height: 16, color: "var(--gray-400)" }} />
          <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder="بحث بالاسم أو الهاتف أو الجوال" style={{ border: "none", outline: "none", width: "100%", fontSize: "var(--text-sm)" }} />
        </div>
      </div>

      <div className="card-grid">
        {rows.map((s) => (
          <Link key={s.id} href={p(`/suppliers/${s.id}`)} className="card card-pad stack" style={{ gap: "var(--space-3)" }}>
            <div className="row-between">
              <span className="kpi-icon"><IconBuilding /></span>
              <IconEye style={{ width: 18, height: 18, color: "var(--text-muted)" }} />
            </div>
            <div>
              <div className="section-title">{s.name}</div>
              <div className="muted text-sm">أُنشئ في {s.createdAt}</div>
            </div>
            <div className="row gap-2 wrap text-xs">
              <span className="info-chip">{s.activeRoutes} مسار فعّال</span>
              <span className="info-chip">{s.contacts.length} سائق</span>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}
