"use client";

import { useEffect, useRef } from "react";
import { IconX } from "./Icons";
import { useLocale } from "@/contexts/LocaleContext";

const FOCUSABLE =
  'a[href], button:not([disabled]), textarea:not([disabled]), input:not([disabled]), select:not([disabled]), [tabindex]:not([tabindex="-1"])';

// Ref-counted body scroll lock so a nested dialog (e.g. map picker inside a
// form) closing doesn't restore page scroll while the outer dialog is open.
let lockCount = 0;
function lockScroll() {
  if (lockCount === 0) document.body.style.overflow = "hidden";
  lockCount++;
}
function unlockScroll() {
  lockCount = Math.max(0, lockCount - 1);
  if (lockCount === 0) document.body.style.overflow = "";
}

export default function Dialog({
  title,
  onClose,
  children,
  footer,
  size = "base",
}: {
  title: string;
  onClose: () => void;
  children: React.ReactNode;
  footer?: React.ReactNode;
  size?: "base" | "lg" | "xl";
}) {
  const { t } = useLocale();
  const panelRef = useRef<HTMLDivElement>(null);
  // Keep the latest onClose without re-running the mount effect. Callers pass an
  // inline `() => setX(null)` that changes identity on every parent render; if the
  // effect below depended on it, it would tear down and re-run on each keystroke,
  // yanking focus out of the field being typed in.
  const onCloseRef = useRef(onClose);
  onCloseRef.current = onClose;

  useEffect(() => {
    const opener = document.activeElement as HTMLElement | null;
    lockScroll();

    // Move focus into the dialog — prefer the first form field (so typing starts
    // there), then any focusable element, then the panel itself.
    const panel = panelRef.current;
    const firstField = panel?.querySelector<HTMLElement>("input:not([disabled]), textarea:not([disabled]), select:not([disabled])");
    const first = firstField ?? panel?.querySelector<HTMLElement>(FOCUSABLE);
    (first ?? panel)?.focus();

    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        onCloseRef.current();
        return;
      }
      if (e.key !== "Tab" || !panel) return;
      // focus trap
      const items = Array.from(panel.querySelectorAll<HTMLElement>(FOCUSABLE)).filter(
        (el) => el.offsetParent !== null
      );
      if (items.length === 0) {
        e.preventDefault();
        panel.focus();
        return;
      }
      const firstEl = items[0];
      const lastEl = items[items.length - 1];
      if (e.shiftKey && document.activeElement === firstEl) {
        e.preventDefault();
        lastEl.focus();
      } else if (!e.shiftKey && document.activeElement === lastEl) {
        e.preventDefault();
        firstEl.focus();
      }
    };

    document.addEventListener("keydown", onKey);
    return () => {
      document.removeEventListener("keydown", onKey);
      unlockScroll();
      opener?.focus?.(); // restore focus to the trigger
    };
    // Mount-once: set up focus trap + scroll lock when the dialog opens and tear
    // them down when it closes. onClose is read via onCloseRef (see above), so it
    // must NOT be a dependency — otherwise the effect re-runs on every keystroke.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div
      className="dialog-backdrop"
      onMouseDown={(e) => {
        if (e.target === e.currentTarget) onClose();
      }}
    >
      <div
        ref={panelRef}
        className={`dialog${size === "lg" ? " dialog-lg" : size === "xl" ? " dialog-xl" : ""}`}
        role="dialog"
        aria-modal="true"
        aria-label={title}
        tabIndex={-1}
      >
        <div className="dialog-head">
          <span className="dialog-title">{title}</span>
          <button className="icon-btn" onClick={onClose} aria-label={t("action.close")}>
            <IconX />
          </button>
        </div>
        <div className="dialog-body">{children}</div>
        {footer && <div className="dialog-foot">{footer}</div>}
      </div>
    </div>
  );
}
