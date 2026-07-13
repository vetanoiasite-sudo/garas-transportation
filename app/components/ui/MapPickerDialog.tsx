"use client";

import { useState } from "react";
import Dialog from "./Dialog";
import { IconPin, IconCopy } from "./Icons";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";

/* Placeholder map picker. Clicking the map converts the click position to
   a lat/lng around a Cairo-area center. TODO: swap for Leaflet.js when a
   maps provider is wired in (see §14.6 of the business doc). */
const CENTER = { lat: 30.0444, lng: 31.2357 }; // Cairo
const SPAN = 0.08;

export default function MapPickerDialog({
  initial,
  onPick,
  onClose,
}: {
  initial?: { lat?: number; lng?: number };
  onPick: (coords: { lat: number; lng: number }) => void;
  onClose: () => void;
}) {
  const { t } = useLocale();
  const { toast } = useToast();
  const [pos, setPos] = useState<{ x: number; y: number }>({ x: 50, y: 50 });
  const [coords, setCoords] = useState<{ lat: number; lng: number }>({
    lat: initial?.lat ?? CENTER.lat,
    lng: initial?.lng ?? CENTER.lng,
  });

  const handleClick = (e: React.MouseEvent<HTMLDivElement>) => {
    const rect = e.currentTarget.getBoundingClientRect();
    const x = ((e.clientX - rect.left) / rect.width) * 100;
    const y = ((e.clientY - rect.top) / rect.height) * 100;
    setPos({ x, y });
    setCoords({
      lat: +(CENTER.lat + (0.5 - y / 100) * SPAN).toFixed(6),
      lng: +(CENTER.lng + (x / 100 - 0.5) * SPAN).toFixed(6),
    });
  };

  const useCurrent = () => {
    if (!navigator.geolocation) {
      toast("خدمة الموقع غير متاحة", "error");
      return;
    }
    navigator.geolocation.getCurrentPosition(
      (p) => setCoords({ lat: +p.coords.latitude.toFixed(6), lng: +p.coords.longitude.toFixed(6) }),
      () => toast("تعذّر الحصول على الموقع", "error")
    );
  };

  return (
    <Dialog
      title={t("action.pickLocation")}
      size="lg"
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>{t("action.cancel")}</button>
          <button className="btn btn-brand" onClick={() => onPick(coords)}>{t("action.confirm")}</button>
        </>
      }
    >
      <div className="stack" style={{ gap: "var(--space-3)" }}>
        <div className="map-box" onClick={handleClick} style={{ cursor: "crosshair" }}>
          <div className="map-pin" style={{ position: "absolute", left: `${pos.x}%`, top: `${pos.y}%`, transform: "translate(-50%, -100%)" }}>
            <IconPin />
          </div>
        </div>
        <div className="row-between wrap">
          <div className="row gap-3">
            <span className="row" style={{ gap: 4 }}>Lat: <span className="coords">{coords.lat}</span></span>
            <span className="row" style={{ gap: 4 }}>Lng: <span className="coords">{coords.lng}</span></span>
            <button
              className="icon-btn"
              onClick={() => {
                navigator.clipboard?.writeText(`${coords.lat}, ${coords.lng}`);
                toast("تم النسخ", "info");
              }}
              aria-label="copy"
            >
              <IconCopy />
            </button>
          </div>
          <button className="btn btn-secondary btn-sm" onClick={useCurrent}>
            <IconPin /> موقعي الحالي
          </button>
        </div>
        <p className="field-hint">انقر على الخريطة لتحديد الموقع، أو استخدم زر موقعي الحالي.</p>
      </div>
    </Dialog>
  );
}
