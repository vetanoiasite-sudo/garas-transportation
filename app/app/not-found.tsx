import Link from "next/link";
import { defaultLocale } from "@/lib/i18n";

// Root 404 — also catches invalid /[locale] segments. Renders inside the root
// layout (Cairo font, dir="rtl"); provider-independent by design.
export default function NotFound() {
  return (
    <div style={{ minHeight: "100vh", display: "grid", placeItems: "center", padding: 24 }}>
      <div className="card card-pad" style={{ maxWidth: 440, width: "100%", textAlign: "center" }}>
        <div style={{ fontSize: 56, fontWeight: 800, color: "var(--color-brand)", lineHeight: 1 }}>404</div>
        <h1 className="section-title" style={{ marginTop: 12 }}>الصفحة غير موجودة</h1>
        <p className="muted" style={{ marginTop: 4 }}>الرابط الذي طلبته غير صحيح أو تم نقله.</p>
        <Link href={`/${defaultLocale}/dashboard`} className="btn btn-brand" style={{ marginTop: 20 }}>
          العودة إلى لوحة المعلومات
        </Link>
      </div>
    </div>
  );
}
