"use client";

import { Link } from "react-router-dom";
import { IconChevronEnd } from "@/components/ui/Icons";

export default function KPICard({
  icon,
  value,
  label,
  sub,
  href,
  tone = "brand",
}: {
  icon: React.ReactNode;
  value: React.ReactNode;
  label: string;
  sub?: React.ReactNode;
  href?: string;
  tone?: "brand" | "green" | "amber" | "red";
}) {
  const toneStyle: React.CSSProperties =
    tone === "green" ? { background: "var(--green-100)", color: "var(--green-700)" }
    : tone === "amber" ? { background: "var(--amber-100)", color: "var(--amber-700)" }
    : tone === "red" ? { background: "var(--red-100)", color: "var(--red-700)" }
    : { background: "var(--color-brand-softer)", color: "var(--color-brand)" };

  const inner = (
    <>
      <div className="kpi-top">
        <span className="kpi-icon" style={toneStyle}>{icon}</span>
        {href && <span className="kpi-go" aria-hidden><IconChevronEnd /></span>}
      </div>
      <div className="kpi-value">{value}</div>
      <div className="kpi-label">{label}</div>
      {sub && <div className="kpi-sub">{sub}</div>}
    </>
  );

  if (href) return <Link to={href} className="kpi">{inner}</Link>;
  return <div className="kpi">{inner}</div>;
}
