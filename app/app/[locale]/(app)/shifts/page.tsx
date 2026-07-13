"use client";

import { useState } from "react";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";
import { shiftGroups as seed, weekdayNamesAr } from "@/lib/data";
import type { ShiftGroup } from "@/lib/types";
import PageHeader from "@/components/ui/PageHeader";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { IconPlus, IconTrash, IconClock } from "@/components/ui/Icons";

export default function ShiftsPage() {
  const { t } = useLocale();
  const { toast } = useToast();
  const [groups, setGroups] = useState<ShiftGroup[]>(seed);
  const [del, setDel] = useState<ShiftGroup | null>(null);
  const [bulk, setBulk] = useState<{ from: string; to: string }>({ from: "08:00", to: "16:00" });

  const toggleDay = (gid: string, day: number) =>
    setGroups((gs) => gs.map((g) => (g.id === gid ? { ...g, days: g.days.map((d) => (d.day === day ? { ...d, active: !d.active } : d)) } : g)));

  const setTime = (gid: string, day: number, field: "from" | "to", value: string) =>
    setGroups((gs) => gs.map((g) => (g.id === gid ? { ...g, days: g.days.map((d) => (d.day === day ? { ...d, [field]: value } : d)) } : g)));

  const setAll = (gid: string) => {
    setGroups((gs) => gs.map((g) => (g.id === gid ? { ...g, days: g.days.map((d) => ({ ...d, from: bulk.from, to: bulk.to })) } : g)));
    toast("تم تطبيق الوقت على كل الأيام");
  };

  const addGroup = () => {
    const number = groups.length + 1;
    setGroups((gs) => [...gs, { id: `sh${Date.now()}`, number, days: [1, 2, 3, 4, 5, 6, 7].map((d) => ({ day: d, active: false, from: "12:00", to: "18:00" })) }]);
    toast("تمت إضافة وردية");
  };

  return (
    <div className="stack">
      <PageHeader title={t("nav.shifts")} count={groups.length}>
        <button className="btn btn-brand btn-sm" onClick={addGroup}><IconPlus />إضافة وردية</button>
      </PageHeader>

      <div className="stack">
        {groups.map((g) => (
          <div key={g.id} className="card card-pad">
            <div className="row-between mb-4">
              <div className="section-title">وردية {g.number}</div>
              <div className="row gap-3 wrap">
                <div className="row gap-2">
                  <span className="text-sm muted">ضبط للكل:</span>
                  <input className="input" type="time" style={{ width: 120 }} value={bulk.from} onChange={(e) => setBulk((b) => ({ ...b, from: e.target.value }))} />
                  <input className="input" type="time" style={{ width: 120 }} value={bulk.to} onChange={(e) => setBulk((b) => ({ ...b, to: e.target.value }))} />
                  <button className="btn btn-outline-brand btn-sm" onClick={() => setAll(g.id)}><IconClock />تطبيق</button>
                </div>
                <button className="icon-btn danger" aria-label={t("action.delete")} onClick={() => setDel(g)}><IconTrash /></button>
              </div>
            </div>

            <div className="row wrap gap-3">
              {g.days.map((d) => (
                <div key={d.day} className="col" style={{ gap: 6, minWidth: 130, flex: 1 }}>
                  <button
                    className="btn btn-sm day-toggle"
                    aria-pressed={d.active}
                    onClick={() => toggleDay(g.id, d.day)}
                  >
                    {d.active && <span aria-hidden>✓ </span>}{weekdayNamesAr[d.day - 1]}
                  </button>
                  <input className="input" type="time" value={d.from} disabled={!d.active} onChange={(e) => setTime(g.id, d.day, "from", e.target.value)} style={{ padding: "0.3rem 0.4rem", fontSize: "var(--text-xs)" }} />
                  <input className="input" type="time" value={d.to} disabled={!d.active} onChange={(e) => setTime(g.id, d.day, "to", e.target.value)} style={{ padding: "0.3rem 0.4rem", fontSize: "var(--text-xs)" }} />
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>

      {del && (
        <ConfirmDialog
          title="حذف الوردية"
          message={`سيتم تعطيل كل أيام وردية ${del.number}.`}
          confirmLabel={t("action.delete")}
          onConfirm={() => { setGroups((gs) => gs.filter((x) => x.id !== del.id)); toast("تم حذف الوردية", "info"); }}
          onClose={() => setDel(null)}
        />
      )}
    </div>
  );
}
