"use client";

import { useState, useEffect, type ReactNode } from "react";
import { EmptyState } from "./EmptyState";
import { useLocale } from "@/contexts/LocaleContext";
import { IconChevronDown } from "./Icons";

export interface Column<T> {
  key: string;
  header: string;
  render?: (row: T, index: number) => ReactNode;
  width?: string;
  align?: "start" | "center" | "end";
  /** "secondary" columns collapse into an expandable row on narrow screens (UX4). */
  priority?: "primary" | "secondary";
}

function useMediaQuery(query: string): boolean {
  const [matches, setMatches] = useState(false);
  useEffect(() => {
    const m = window.matchMedia(query);
    const on = () => setMatches(m.matches);
    on();
    m.addEventListener("change", on);
    return () => m.removeEventListener("change", on);
  }, [query]);
  return matches;
}

export default function DataTable<T extends { id: string }>({
  columns,
  rows,
  loading,
  emptyMessage,
  emptyAction,
  toolbar,
  pageSize = 20,
}: {
  columns: Column<T>[];
  rows: T[];
  loading?: boolean;
  emptyMessage: string;
  emptyAction?: ReactNode;
  toolbar?: ReactNode;
  pageSize?: number;
}) {
  const { t } = useLocale();
  const [page, setPage] = useState(1);
  const [size, setSize] = useState(pageSize);
  const [expanded, setExpanded] = useState<string | null>(null);

  // On narrow screens, hide "secondary" columns and reveal them per-row on demand.
  const narrow = useMediaQuery("(max-width: 768px)");
  const secondary = columns.filter((c) => c.priority === "secondary");
  const collapsing = narrow && secondary.length > 0;
  const visibleCols = collapsing ? columns.filter((c) => c.priority !== "secondary") : columns;
  const colCount = visibleCols.length + (collapsing ? 1 : 0);

  const totalPages = Math.max(1, Math.ceil(rows.length / size));
  const current = Math.min(page, totalPages);
  const start = (current - 1) * size;
  const pageRows = rows.slice(start, start + size);

  return (
    <div className="stack" style={{ gap: "var(--space-3)" }}>
      {toolbar && <div className="card card-pad" style={{ padding: "var(--space-3) var(--space-4)" }}>{toolbar}</div>}
      <div className="table-wrap">
        {loading ? (
          <table className="data" aria-busy="true">
            <thead>
              <tr>
                {columns.map((c) => (
                  <th key={c.key} scope="col" style={{ width: c.width, textAlign: c.align ?? "start" }}>
                    {c.header || <span className="sr-only">إجراءات</span>}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {Array.from({ length: Math.min(size, 8) }).map((_, r) => (
                <tr key={r}>
                  {columns.map((c) => (
                    <td key={c.key}><span className="skel" /></td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        ) : rows.length === 0 ? (
          <EmptyState message={emptyMessage} action={emptyAction} />
        ) : (
          <>
            <table className="data">
              <thead>
                <tr>
                  {collapsing && <th scope="col" style={{ width: 40 }}><span className="sr-only">تفاصيل</span></th>}
                  {visibleCols.map((c) => (
                    <th key={c.key} scope="col" style={{ width: c.width, textAlign: c.align ?? "start" }}>
                      {c.header || <span className="sr-only">إجراءات</span>}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {pageRows.map((row, i) => {
                  const idx = start + i;
                  const isOpen = expanded === row.id;
                  return (
                    <FragmentRow key={row.id}>
                      <tr>
                        {collapsing && (
                          <td style={{ width: 40 }}>
                            <button
                              className="row-expand"
                              aria-expanded={isOpen}
                              aria-label={t("action.details")}
                              onClick={() => setExpanded(isOpen ? null : row.id)}
                            >
                              <IconChevronDown style={{ transform: isOpen ? "rotate(180deg)" : "none" }} />
                            </button>
                          </td>
                        )}
                        {visibleCols.map((c) => (
                          <td key={c.key} style={{ textAlign: c.align ?? "start" }}>
                            {c.render ? c.render(row, idx) : (row as Record<string, ReactNode>)[c.key]}
                          </td>
                        ))}
                      </tr>
                      {collapsing && isOpen && (
                        <tr className="row-detail">
                          <td colSpan={colCount}>
                            <dl className="detail-grid">
                              {secondary.map((c) => (
                                <div key={c.key}>
                                  <dt>{c.header}</dt>
                                  <dd>{c.render ? c.render(row, idx) : (row as Record<string, ReactNode>)[c.key]}</dd>
                                </div>
                              ))}
                            </dl>
                          </td>
                        </tr>
                      )}
                    </FragmentRow>
                  );
                })}
              </tbody>
            </table>
            <div className="pagination">
              <div className="row" style={{ gap: "var(--space-2)" }}>
                <span className="muted text-sm">{t("common_perPage")}</span>
                <select
                  className="select"
                  style={{ width: "auto", padding: "0.3rem 0.5rem" }}
                  value={size}
                  onChange={(e) => {
                    setSize(Number(e.target.value));
                    setPage(1);
                  }}
                >
                  {[10, 20, 50].map((n) => (
                    <option key={n} value={n}>{n}</option>
                  ))}
                </select>
                <span className="muted text-sm">
                  {start + 1}–{Math.min(start + size, rows.length)} {t("common_of")} {rows.length}
                </span>
              </div>
              <div className="pager">
                <button disabled={current <= 1} onClick={() => setPage(current - 1)} aria-label="السابق">‹</button>
                {Array.from({ length: totalPages }, (_, i) => i + 1)
                  .filter((p) => Math.abs(p - current) <= 2 || p === 1 || p === totalPages)
                  .map((p, idx, arr) => (
                    <span key={p} className="row">
                      {idx > 0 && arr[idx - 1] !== p - 1 && <span className="muted">…</span>}
                      <button className={p === current ? "active" : ""} aria-current={p === current ? "page" : undefined} onClick={() => setPage(p)}>{p}</button>
                    </span>
                  ))}
                <button disabled={current >= totalPages} onClick={() => setPage(current + 1)} aria-label="التالي">›</button>
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}

/** Renders multiple sibling <tr>s without an extra wrapper element. */
function FragmentRow({ children }: { children: ReactNode }) {
  return <>{children}</>;
}
