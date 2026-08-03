"use client";

import { createContext, useContext, useCallback } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { type Locale, dir as dirOf, translate, locales } from "@/lib/i18n";

interface LocaleCtx {
  locale: Locale;
  dir: "rtl" | "ltr";
  t: (key: string) => string;
  switchLocale: (next: Locale) => void;
}

const Ctx = createContext<LocaleCtx | null>(null);

export function LocaleProvider({
  locale,
  children,
}: {
  locale: Locale;
  children: React.ReactNode;
}) {
  const navigate = useNavigate();
  const pathname = useLocation().pathname;

  const t = useCallback((key: string) => translate(locale, key), [locale]);

  const switchLocale = useCallback(
    (next: Locale) => {
      if (next === locale) return;
      // pathname is like /ar/dashboard -> swap the first segment
      const parts = pathname.split("/");
      if (locales.includes(parts[1] as Locale)) {
        parts[1] = next;
      } else {
        parts.splice(1, 0, next);
      }
      navigate(parts.join("/") || `/${next}`);
    },
    [locale, pathname, navigate]
  );

  return (
    <Ctx.Provider value={{ locale, dir: dirOf(locale), t, switchLocale }}>
      {children}
    </Ctx.Provider>
  );
}

export function useLocale(): LocaleCtx {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useLocale must be used within LocaleProvider");
  return ctx;
}
