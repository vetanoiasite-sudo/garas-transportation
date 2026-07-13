"use client";

import { useEffect } from "react";
import { IconAlert } from "@/components/ui/Icons";

// Route-level error boundary for the [locale] subtree. Provider-independent
// (uses static Arabic copy) so it renders even if an error occurred before
// context mounted. Graceful recovery via reset() — no hard crash (§4.1).
export default function LocaleError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    // TODO: forward to error monitoring (Sentry, etc.) when wired up
    console.error(error);
  }, [error]);

  return (
    <div style={{ minHeight: "70vh", display: "grid", placeItems: "center", padding: 24 }} dir="rtl">
      <div className="card card-pad" style={{ maxWidth: 440, width: "100%", textAlign: "center" }}>
        <span className="kpi-icon" style={{ margin: "0 auto", background: "var(--red-100)", color: "var(--color-danger)" }}>
          <IconAlert />
        </span>
        <h1 className="section-title" style={{ marginTop: 16 }}>حدث خطأ غير متوقع</h1>
        <p className="muted" style={{ marginTop: 4 }}>تعذّر عرض هذه الصفحة. يمكنك إعادة المحاولة.</p>
        <button className="btn btn-brand" style={{ marginTop: 20 }} onClick={reset}>
          إعادة المحاولة
        </button>
      </div>
    </div>
  );
}
