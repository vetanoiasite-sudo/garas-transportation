/* Shared date/time display formatting.
 *
 * House format (used everywhere dates/times are shown to the user):
 *   date       →  "2026 - 8 - 5"            (year - month - day, no leading zeros)
 *   time       →  "2:08 pm"                 (12-hour, lowercase am/pm)
 *   date+time  →  "2026 - 8 - 5 2:08 pm"
 *
 * These are for DISPLAY only. Never feed the output back to <input type="date">
 * or to the backend — those keep the raw ISO value (YYYY-MM-DD / full ISO).
 *
 * Parsing is regex-first (not `new Date(...)`) for plain "YYYY-MM-DD" and
 * "HH:MM" strings so a timezone offset can never shift the day or the hour.
 * Unparseable input is returned unchanged (matches the old "fall back to raw"
 * behaviour). */

interface Parts {
  y?: number;
  mo?: number;
  d?: number;
  h?: number;
  mi?: number;
  hasDate: boolean;
  hasTime: boolean;
}

function parse(value?: string | number | Date | null): Parts | null {
  if (value === undefined || value === null || value === "") return null;
  if (value instanceof Date) {
    if (isNaN(value.getTime())) return null;
    return { y: value.getFullYear(), mo: value.getMonth() + 1, d: value.getDate(), h: value.getHours(), mi: value.getMinutes(), hasDate: true, hasTime: true };
  }
  const s = String(value).trim();
  if (!s) return null;

  // Time-only: "HH:MM" or "HH:MM:SS".
  const timeOnly = /^(\d{1,2}):(\d{2})(?::\d{2})?$/.exec(s);
  if (timeOnly) return { h: +timeOnly[1], mi: +timeOnly[2], hasDate: false, hasTime: true };

  // ISO-ish: "YYYY-MM-DD" with an optional "THH:MM" / " HH:MM" time.
  const iso = /^(\d{4})-(\d{1,2})-(\d{1,2})(?:[T ](\d{1,2}):(\d{2}))?/.exec(s);
  if (iso) {
    const hasTime = iso[4] !== undefined;
    return { y: +iso[1], mo: +iso[2], d: +iso[3], h: hasTime ? +iso[4] : 0, mi: hasTime ? +iso[5] : 0, hasDate: true, hasTime };
  }

  // Anything else: let the engine try (e.g. "8/5/2026", RFC strings).
  const dt = new Date(s);
  if (!isNaN(dt.getTime())) return { y: dt.getFullYear(), mo: dt.getMonth() + 1, d: dt.getDate(), h: dt.getHours(), mi: dt.getMinutes(), hasDate: true, hasTime: true };
  return null;
}

function to12h(h: number, mi: number): string {
  const ampm = h < 12 ? "am" : "pm";
  const hr = h % 12 === 0 ? 12 : h % 12;
  return `${hr}:${String(mi).padStart(2, "0")} ${ampm}`;
}

const raw = (value?: string | number | Date | null): string => (value === undefined || value === null ? "" : String(value));

/** "2026 - 8 - 5" (falls back to the raw value if it has no parseable date). */
export function formatDate(value?: string | number | Date | null): string {
  const p = parse(value);
  if (!p || !p.hasDate) return raw(value);
  return `${p.y} - ${p.mo} - ${p.d}`;
}

/** "2:08 pm" (falls back to the raw value if it has no parseable time). */
export function formatTime(value?: string | number | Date | null): string {
  const p = parse(value);
  if (!p || !p.hasTime) return raw(value);
  return to12h(p.h!, p.mi!);
}

/** "2026 - 8 - 5 2:08 pm" — omits the time when the value carries only a date. */
export function formatDateTime(value?: string | number | Date | null): string {
  const p = parse(value);
  if (!p) return raw(value);
  if (!p.hasDate) return p.hasTime ? to12h(p.h!, p.mi!) : raw(value);
  const date = `${p.y} - ${p.mo} - ${p.d}`;
  return p.hasTime ? `${date} ${to12h(p.h!, p.mi!)}` : date;
}
