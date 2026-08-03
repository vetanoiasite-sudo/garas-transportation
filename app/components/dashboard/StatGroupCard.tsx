"use client";

import { Fragment } from "react";
import { Link } from "react-router-dom";
import { IconChevronEnd } from "@/components/ui/Icons";

export interface Stat {
  label: string;
  value: React.ReactNode;
}

/** A KPI card that shows a title + a row of sub-stats (e.g. vehicle attendance
 *  broken into check-in / check-out / one-way). Matches the .kpi card chrome. */
export default function StatGroupCard({
  icon,
  title,
  stats,
  href,
  tone = "brand",
}: {
  icon: React.ReactNode;
  title: string;
  stats: Stat[];
  href?: string;
  tone?: "brand" | "green" | "amber" | "red";
}) {
  const iconStyle: React.CSSProperties =
    tone === "green" ? { background: "var(--green-100)", color: "var(--green-700)" }
    : tone === "amber" ? { background: "var(--amber-100)", color: "var(--amber-700)" }
    : tone === "red" ? { background: "var(--red-100)", color: "var(--red-700)" }
    : { background: "var(--color-brand-softer)", color: "var(--color-brand)" };

  const valueColor =
    tone === "green" ? "var(--green-700)"
    : tone === "amber" ? "var(--amber-700)"
    : tone === "red" ? "var(--red-700)"
    : "var(--color-brand)";

  const inner = (
    <>
      <div className="kpi-top">
        <span className="kpi-icon" style={iconStyle}>{icon}</span>
        {href && <span className="kpi-go" aria-hidden><IconChevronEnd /></span>}
      </div>
      <div className="kpi-label" style={{ fontWeight: 600, color: "var(--text-heading)" }}>{title}</div>
      <div className="kpi-stats">
        {stats.map((s, i) => (
          <Fragment key={i}>
            {i > 0 && <span className="kpi-divider" />}
            <span className="kpi-stat">
              <span className="l">{s.label}</span>
              <span className="v" style={{ color: valueColor }}>{s.value}</span>
            </span>
          </Fragment>
        ))}
      </div>
    </>
  );

  if (href) return <Link to={href} className="kpi">{inner}</Link>;
  return <div className="kpi">{inner}</div>;
}
