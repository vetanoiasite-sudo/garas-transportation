"use client";

import { useState, useRef, useEffect } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { useLocale } from "@/contexts/LocaleContext";
import { type Role, roleLabelKey } from "@/lib/types";
import { IconChevronDown } from "@/components/ui/Icons";

const roles: Role[] = ["system_admin", "super_admin", "line_admin", "trans_admin", "supervisor", "reader"];

/** Dev-only role switcher to preview permission-aware UI across all 6 roles. */
export default function RoleSwitcher() {
  const { user, setRole } = useAuth();
  const { t } = useLocale();
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const onDoc = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener("mousedown", onDoc);
    return () => document.removeEventListener("mousedown", onDoc);
  }, []);

  if (!user) return null;

  return (
    <div className="combo" ref={ref} style={{ minWidth: 150 }}>
      <button className="btn btn-secondary btn-sm" onClick={() => setOpen((v) => !v)} style={{ width: "100%", justifyContent: "space-between" }}>
        <span className="row" style={{ gap: 6 }}>
          <span className="dot dot-green" />
          {t(roleLabelKey[user.role])}
        </span>
        <IconChevronDown style={{ width: 15, height: 15 }} />
      </button>
      {open && (
        <div className="combo-menu">
          <div style={{ padding: "6px 12px", fontSize: "var(--text-xs)", color: "var(--text-muted)", borderBottom: "1px solid var(--border-base)" }}>
            وضع المطوّر — تبديل الدور
          </div>
          {roles.map((r) => (
            <div
              key={r}
              className={`combo-opt${r === user.role ? " selected" : ""}`}
              onClick={() => {
                setRole(r);
                setOpen(false);
              }}
            >
              {t(roleLabelKey[r])}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
