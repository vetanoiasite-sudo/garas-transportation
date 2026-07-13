"use client";

import { useState } from "react";
import Link from "next/link";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { passengers as seed } from "@/lib/data";
import type { Passenger } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import DataTable, { type Column } from "@/components/ui/DataTable";
import { ActiveBadge } from "@/components/ui/Badge";
import { IconPlus, IconDownload, IconUpload, IconEye } from "@/components/ui/Icons";

export default function PassengersPage() {
  const { t, locale } = useLocale();
  const { toast } = useToast();
  const [rows] = useState<Passenger[]>(seed);
  const p = (path: string) => `/${locale}${path}`;

  const columns: Column<Passenger>[] = [
    { key: "idx", header: "#", width: "48px", render: (_r, i) => i + 1 },
    { key: "name", header: "الاسم", render: (r) => <Link href={p(`/passengers/${r.id}`)} style={{ color: "var(--color-brand)", fontWeight: 600 }}>{r.name}</Link> },
    { key: "identityNumber", header: "الرقم القومي" },
    { key: "mobile", header: "الجوال" },
    { key: "identifier", header: "معرّف آخر" },
    { key: "routesCount", header: "المسارات", render: (r) => <span className="info-chip">{r.routesCount}</span> },
    { key: "active", header: "الحالة", render: (r) => <ActiveBadge active={r.active} /> },
    { key: "actions", header: "", align: "end", render: (r) => <Link href={p(`/passengers/${r.id}`)} className="icon-btn brand" aria-label={`${t("action.view")}: ${r.name}`}><IconEye /></Link> },
  ];

  return (
    <div className="stack">
      <PageHeader title={t("nav.employees")} count={rows.length}>
        <button className="btn btn-secondary btn-sm" onClick={() => toast("تم تنزيل القالب")}><IconDownload />قالب Excel</button>
        <button className="btn btn-secondary btn-sm" onClick={() => toast("تم رفع الملف بنجاح")}><IconUpload />{t("action.uploadExcel")}</button>
        <Link href={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>
      </PageHeader>

      <DataTable
        columns={columns}
        rows={rows}
        emptyMessage="لا يوجد ركاب مسجّلون بعد."
        emptyAction={<Link href={p(`/passengers/new`)} className="btn btn-brand btn-sm"><IconPlus />{t("action.addNew")}</Link>}
      />
    </div>
  );
}
