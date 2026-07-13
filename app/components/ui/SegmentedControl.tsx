"use client";

import type { ReactNode } from "react";

export interface Segment<T extends string> {
  value: T;
  label: ReactNode;
}

/* Single source of truth for mutually-exclusive segmented choices
   (route type, period, deduction type, repricing scope/mode). Carries
   group semantics (role="group" + aria-pressed) so it's accessible by
   default. Pair with a visible caption via `ariaLabelledby`. */
export default function SegmentedControl<T extends string>({
  value,
  onChange,
  segments,
  ariaLabelledby,
  ariaLabel,
}: {
  value: T;
  onChange: (value: T) => void;
  segments: Segment<T>[];
  ariaLabelledby?: string;
  ariaLabel?: string;
}) {
  return (
    <div className="segmented" role="group" aria-labelledby={ariaLabelledby} aria-label={ariaLabel}>
      {segments.map((s) => (
        <button
          key={s.value}
          type="button"
          aria-pressed={value === s.value}
          className={value === s.value ? "active" : ""}
          onClick={() => onChange(s.value)}
        >
          {s.label}
        </button>
      ))}
    </div>
  );
}
