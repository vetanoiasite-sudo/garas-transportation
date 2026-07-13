"use client";

import { createContext, useContext, useState, useCallback, useEffect, useRef } from "react";
import { useLocale } from "./LocaleContext";
import { IconX } from "@/components/ui/Icons";

type ToastKind = "success" | "error" | "info";
interface Toast {
  id: number;
  kind: ToastKind;
  message: string;
}

interface ToastCtx {
  toast: (message: string, kind?: ToastKind) => void;
}

const Ctx = createContext<ToastCtx | null>(null);
let seq = 1;
const MAX_VISIBLE = 4;

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const remove = useCallback((id: number) => {
    setToasts((t) => t.filter((x) => x.id !== id));
  }, []);

  const toast = useCallback((message: string, kind: ToastKind = "success") => {
    const id = seq++;
    setToasts((t) => {
      const next = [...t, { id, kind, message }];
      return next.length > MAX_VISIBLE ? next.slice(next.length - MAX_VISIBLE) : next;
    });
  }, []);

  return (
    <Ctx.Provider value={{ toast }}>
      {children}
      {/* single polite live region for the whole stack */}
      <div className="toast-stack" aria-live="polite" aria-atomic="false">
        {toasts.map((t) => (
          <ToastItem key={t.id} toast={t} onClose={() => remove(t.id)} />
        ))}
      </div>
    </Ctx.Provider>
  );
}

function ToastItem({ toast, onClose }: { toast: Toast; onClose: () => void }) {
  const { t } = useLocale();
  const [leaving, setLeaving] = useState(false);
  const timer = useRef<number | undefined>(undefined);
  // errors linger longer so they can be read; others auto-dismiss
  const duration = toast.kind === "error" ? 6000 : 3500;

  const dismiss = useCallback(() => {
    if (timer.current) window.clearTimeout(timer.current);
    setLeaving(true);
    window.setTimeout(onClose, 200);
  }, [onClose]);

  const start = useCallback(() => {
    timer.current = window.setTimeout(dismiss, duration);
  }, [dismiss, duration]);

  const stop = useCallback(() => {
    if (timer.current) window.clearTimeout(timer.current);
  }, []);

  useEffect(() => {
    start();
    return stop;
  }, [start, stop]);

  return (
    <div
      className={`toast toast-${toast.kind}${leaving ? " leaving" : ""}`}
      role={toast.kind === "error" ? "alert" : "status"}
      onMouseEnter={stop}
      onMouseLeave={start}
    >
      <span className="toast-icon" aria-hidden>
        {toast.kind === "success" ? "✓" : toast.kind === "error" ? "!" : "i"}
      </span>
      <span className="grow">{toast.message}</span>
      <button className="toast-close" aria-label={t("action.close")} onClick={dismiss}>
        <IconX />
      </button>
    </div>
  );
}

export function useToast(): ToastCtx {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useToast must be used within ToastProvider");
  return ctx;
}
