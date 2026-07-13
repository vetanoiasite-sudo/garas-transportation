import { notFound } from "next/navigation";
import { isLocale } from "@/lib/i18n";
import { LocaleProvider } from "@/contexts/LocaleContext";
import { AuthProvider } from "@/contexts/AuthContext";
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

  return (
    <LocaleProvider locale={locale}>
      <AuthProvider>
        <ToastProvider>
          <DirectionSync locale={locale} />
          {children}
        </ToastProvider>
      </AuthProvider>
    </LocaleProvider>
  );
}
