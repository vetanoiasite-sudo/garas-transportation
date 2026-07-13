"use client";

import { useLocale } from "@/contexts/LocaleContext";

type Tone = "green" | "red" | "amber" | "orange" | "blue" | "gray";

export function Badge({ tone = "gray", children }: { tone?: Tone; children: React.ReactNode }) {
  return <span className={`badge badge-${tone}`}>{children}</span>;
}

/** Approval state badge (independent of active). */
export function ApprovalBadge({ approved }: { approved: boolean }) {
  const { t } = useLocale();
  return approved ? (
    <span className="badge badge-green"><span className="dot dot-green" />{t("status.approved")}</span>
  ) : (
    <span className="badge badge-red"><span className="dot dot-red" />{t("status.notApproved")}</span>
  );
}

/** Active state badge (independent of approval). */
export function ActiveBadge({ active }: { active: boolean }) {
  const { t } = useLocale();
  return active ? (
    <span className="badge badge-green">{t("status.active")}</span>
  ) : (
    <span className="badge badge-orange">{t("status.inactive")}</span>
  );
}

export function AttendanceText({ attended, time }: { attended: boolean; time?: string }) {
  const { t } = useLocale();
  // Pair colour with a non-colour glyph + SR label so status never relies on
  // colour alone (WCAG 1.4.1). Doc §5.3 colour semantics are preserved.
  return (
    <span className="row" style={{ gap: 4, color: attended ? "var(--text-heading)" : "var(--color-danger)", fontWeight: 500 }}>
      <span aria-hidden style={{ fontWeight: 700 }}>{attended ? "✓" : "✕"}</span>
      <span className="sr-only">{attended ? t("status.attended") : t("status.absent")}: </span>
      <span>{time ? time : attended ? t("status.attended") : t("status.absent")}</span>
    </span>
  );
}
