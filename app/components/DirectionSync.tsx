"use client";

import { useEffect } from "react";
import type { Locale } from "@/lib/i18n";
import { dir } from "@/lib/i18n";

/** Keeps <html lang/dir> in sync with the active locale. */
export default function DirectionSync({ locale }: { locale: Locale }) {
  useEffect(() => {
    document.documentElement.lang = locale;
    document.documentElement.dir = dir(locale);
  }, [locale]);
  return null;
}
