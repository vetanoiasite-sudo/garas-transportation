import { cookies } from "next/headers";
import { notFound } from "next/navigation";
import { isLocale } from "@/lib/i18n";
import { LocaleProvider } from "@/contexts/LocaleContext";
import { AuthProvider, type User } from "@/contexts/AuthContext";
import { AUTH_COOKIE } from "@/lib/auth";
import { ToastProvider } from "@/contexts/ToastContext";
import DirectionSync from "@/components/DirectionSync";

export function generateStaticParams() {
  return [{ locale: "ar" }, { locale: "en" }];
}

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  if (!isLocale(locale)) notFound();

  // Read the session from the cookie so the tree renders with the correct
  // auth state on the server — avoids the client-side loading/redirect flash.
  const raw = (await cookies()).get(AUTH_COOKIE)?.value;
  let initialUser: User | null = null;
  if (raw) {
    try {
      initialUser = JSON.parse(decodeURIComponent(raw)) as User;
    } catch {
      /* malformed cookie — treat as logged out */
    }
  }

  return (
    <LocaleProvider locale={locale}>
      <AuthProvider initialUser={initialUser}>
        <ToastProvider>
          <DirectionSync locale={locale} />
          {children}
        </ToastProvider>
      </AuthProvider>
    </LocaleProvider>
  );
}
