"use client";

import { useState, useRef, useEffect, useId } from "react";
import { IconChevronDown, IconX, IconSearch } from "./Icons";
import { useLocale } from "@/contexts/LocaleContext";
import { useField } from "./Field";

export interface Option {
  value: string;
  label: string;
}

/** Master searchable dropdown.
 *  States: default / open / loading / empty / disabled-with-reason / selected.
 *  Fully keyboard operable (WCAG 2.1.1 / 4.1.2 · doc §14.4):
 *  arrows move the active option, Enter selects, Esc closes, Home/End jump. */
export default function Combobox({
  options,
  value,
  onChange,
  placeholder,
  disabled,
  disabledReason,
  clearable = true,
  loading = false,
}: {
  options: Option[];
  value?: string;
  onChange: (value: string | undefined) => void;
  placeholder?: string;
  disabled?: boolean;
  disabledReason?: string;
  clearable?: boolean;
  loading?: boolean;
}) {
  const { t } = useLocale();
  const field = useField();
  const [open, setOpen] = useState(false);
  const [query, setQuery] = useState("");
  const [active, setActive] = useState(0);
  const ref = useRef<HTMLDivElement>(null);
  const searchRef = useRef<HTMLInputElement>(null);
  const listRef = useRef<HTMLDivElement>(null);
  const baseId = useId();
  const listId = `${baseId}-list`;
  const optId = (i: number) => `${baseId}-opt-${i}`;

  const selected = options.find((o) => o.value === value);
  const filtered = loading ? [] : options.filter((o) => o.label.toLowerCase().includes(query.toLowerCase()));

  useEffect(() => {
    const onDoc = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener("mousedown", onDoc);
    return () => document.removeEventListener("mousedown", onDoc);
  }, []);

  // when opening, focus the search and reset the active option
  useEffect(() => {
    if (open) {
      setActive(0);
      const id = requestAnimationFrame(() => searchRef.current?.focus());
      return () => cancelAnimationFrame(id);
    }
  }, [open]);

  // keep the active option scrolled into view
  useEffect(() => {
    if (!open || !listRef.current) return;
    const el = listRef.current.querySelector<HTMLElement>(`#${CSS.escape(optId(active))}`);
    el?.scrollIntoView({ block: "nearest" });
  }, [active, open]); // eslint-disable-line react-hooks/exhaustive-deps

  const commit = (o: Option) => {
    onChange(o.value);
    setOpen(false);
    setQuery("");
  };

  const onTriggerKey = (e: React.KeyboardEvent) => {
    if (disabled) return;
    if (e.key === "ArrowDown" || e.key === "Enter" || e.key === " ") {
      e.preventDefault();
      setOpen(true);
    }
  };

  const onSearchKey = (e: React.KeyboardEvent) => {
    if (e.key === "ArrowDown") {
      e.preventDefault();
      setActive((a) => Math.min(a + 1, filtered.length - 1));
    } else if (e.key === "ArrowUp") {
      e.preventDefault();
      setActive((a) => Math.max(a - 1, 0));
    } else if (e.key === "Home") {
      e.preventDefault();
      setActive(0);
    } else if (e.key === "End") {
      e.preventDefault();
      setActive(filtered.length - 1);
    } else if (e.key === "Enter") {
      e.preventDefault();
      if (filtered[active]) commit(filtered[active]);
    } else if (e.key === "Escape") {
      e.preventDefault();
      setOpen(false);
    }
  };

  return (
    <div className="combo" ref={ref}>
      <button
        type="button"
        className={`select${field.invalid ? " input-error" : ""}`}
        style={{ display: "flex", alignItems: "center", justifyContent: "space-between", textAlign: "start" }}
        disabled={disabled}
        onClick={() => setOpen((v) => !v)}
        onKeyDown={onTriggerKey}
        title={disabled ? disabledReason : undefined}
        aria-haspopup="listbox"
        aria-expanded={open}
        aria-controls={open ? listId : undefined}
        id={field.id}
        aria-describedby={field.describedBy}
        aria-invalid={field.invalid || undefined}
        aria-required={field.required || undefined}
      >
        <span style={{ color: selected ? "var(--text-heading)" : "var(--gray-400)", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
          {selected ? selected.label : disabled && disabledReason ? disabledReason : placeholder ?? t("action.search")}
        </span>
        <span className="row" style={{ gap: 4 }}>
          {selected && clearable && !disabled && (
            // role="button" span (not a <button>) so it can live inside the
            // trigger <button> without producing invalid nested-button markup.
            <span
              role="button"
              tabIndex={-1}
              className="icon-btn"
              aria-label={t("action.close")}
              onClick={(e) => {
                e.stopPropagation();
                onChange(undefined);
              }}
              onKeyDown={(e) => {
                if (e.key === "Enter" || e.key === " ") {
                  e.preventDefault();
                  e.stopPropagation();
                  onChange(undefined);
                }
              }}
              style={{ width: 22, height: 22, display: "inline-flex", alignItems: "center", justifyContent: "center" }}
            >
              <IconX style={{ width: 14, height: 14 }} />
            </span>
          )}
          <IconChevronDown style={{ width: 16, height: 16 }} />
        </span>
      </button>

      {open && !disabled && (
        <div className="combo-menu">
          <div className="combo-search">
            <div className="row" style={{ gap: 6, border: "1px solid var(--border-base)", borderRadius: "var(--radius-base)", padding: "4px 8px" }}>
              <IconSearch style={{ width: 15, height: 15, color: "var(--gray-400)" }} />
              <input
                ref={searchRef}
                value={query}
                onChange={(e) => { setQuery(e.target.value); setActive(0); }}
                onKeyDown={onSearchKey}
                placeholder={t("action.search")}
                role="combobox"
                aria-expanded="true"
                aria-controls={listId}
                aria-activedescendant={filtered[active] ? optId(active) : undefined}
                style={{ border: "none", outline: "none", width: "100%", fontSize: "var(--text-sm)", background: "transparent", color: "var(--text-heading)" }}
              />
            </div>
          </div>

          <div role="listbox" id={listId} ref={listRef}>
            {loading ? (
              <div className="combo-empty row" style={{ justifyContent: "center", gap: 8 }}>
                <span className="spinner spinner-brand" style={{ width: 16, height: 16 }} />
                <span>{t("action.search")}…</span>
              </div>
            ) : filtered.length === 0 ? (
              <div className="combo-empty">{t("empty.generic")}</div>
            ) : (
              filtered.map((o, i) => (
                <div
                  key={o.value}
                  id={optId(i)}
                  role="option"
                  aria-selected={o.value === value}
                  className={`combo-opt${o.value === value ? " selected" : ""}${i === active ? " active" : ""}`}
                  onMouseEnter={() => setActive(i)}
                  onClick={() => commit(o)}
                >
                  {o.label}
                </div>
              ))
            )}
          </div>
        </div>
      )}
    </div>
  );
}
