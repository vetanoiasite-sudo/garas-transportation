"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import Dialog from "./Dialog";
import { IconPin, IconCopy } from "./Icons";
import { useLocale } from "@/contexts/LocaleContext";
import { useToast } from "@/contexts/ToastContext";

/* Real slippy map picker backed by OpenStreetMap raster tiles — no external map
   library. Click to drop the pin, drag to pan, +/- to zoom. Coordinates use the
   standard Web Mercator projection so the pin maps to a true lat/lng. */

const CENTER = { lat: 30.0444, lng: 31.2357 }; // Cairo
const TILE = 256;
const MIN_Z = 3;
const MAX_Z = 18;
const DRAG_THRESHOLD = 5; // px moved before a press counts as a pan, not a click

// lat/lng → fractional world-tile coordinates at zoom z.
function project(lat: number, lng: number, z: number): { x: number; y: number } {
  const n = 2 ** z;
  const latRad = (lat * Math.PI) / 180;
  const x = ((lng + 180) / 360) * n;
  const y = ((1 - Math.log(Math.tan(latRad) + 1 / Math.cos(latRad)) / Math.PI) / 2) * n;
  return { x, y };
}
// fractional world-tile coordinates → lat/lng at zoom z.
function unproject(x: number, y: number, z: number): { lat: number; lng: number } {
  const n = 2 ** z;
  const lng = (x / n) * 360 - 180;
  const latRad = Math.atan(Math.sinh(Math.PI * (1 - (2 * y) / n)));
  return { lat: (latRad * 180) / Math.PI, lng };
}
const clamp = (v: number, lo: number, hi: number) => Math.max(lo, Math.min(hi, v));

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

  const start = { lat: initial?.lat ?? CENTER.lat, lng: initial?.lng ?? CENTER.lng };
  const [coords, setCoords] = useState(start); // the picked point (the pin)
  const [center, setCenter] = useState(start); // map viewport center
  const [zoom, setZoom] = useState(13);
  const [size, setSize] = useState({ w: 0, h: 360 });

  const boxRef = useRef<HTMLDivElement>(null);
  // Drag bookkeeping (refs so pointer handlers don't re-bind every render).
  const drag = useRef<{ active: boolean; moved: boolean; sx: number; sy: number; startCenter: { lat: number; lng: number } } | null>(null);

  // Measure the map box so the tile grid and pixel↔latlng math match the DOM.
  useEffect(() => {
    const el = boxRef.current;
    if (!el) return;
    const measure = () => setSize({ w: el.clientWidth, h: el.clientHeight });
    measure();
    const ro = new ResizeObserver(measure);
    ro.observe(el);
    return () => ro.disconnect();
  }, []);

  const centerWorldPx = { x: project(center.lat, center.lng, zoom).x * TILE, y: project(center.lat, center.lng, zoom).y * TILE };

  // Screen pixel (relative to the box) → lat/lng.
  const pxToLatLng = useCallback(
    (px: number, py: number) => {
      const worldX = centerWorldPx.x + (px - size.w / 2);
      const worldY = centerWorldPx.y + (py - size.h / 2);
      return unproject(worldX / TILE, worldY / TILE, zoom);
    },
    [centerWorldPx.x, centerWorldPx.y, size.w, size.h, zoom],
  );

  // lat/lng → screen pixel (relative to the box).
  const latLngToPx = useCallback(
    (lat: number, lng: number) => {
      const p = project(lat, lng, zoom);
      return { x: p.x * TILE - centerWorldPx.x + size.w / 2, y: p.y * TILE - centerWorldPx.y + size.h / 2 };
    },
    [centerWorldPx.x, centerWorldPx.y, size.w, size.h, zoom],
  );

  const onPointerDown = (e: React.PointerEvent<HTMLDivElement>) => {
    e.currentTarget.setPointerCapture(e.pointerId);
    drag.current = { active: true, moved: false, sx: e.clientX, sy: e.clientY, startCenter: center };
  };
  const onPointerMove = (e: React.PointerEvent<HTMLDivElement>) => {
    const d = drag.current;
    if (!d?.active) return;
    const dx = e.clientX - d.sx;
    const dy = e.clientY - d.sy;
    if (!d.moved && Math.hypot(dx, dy) < DRAG_THRESHOLD) return;
    d.moved = true;
    // Pan: content follows the cursor, so the center moves opposite to the drag.
    const startPx = { x: project(d.startCenter.lat, d.startCenter.lng, zoom).x * TILE, y: project(d.startCenter.lat, d.startCenter.lng, zoom).y * TILE };
    const next = unproject((startPx.x - dx) / TILE, (startPx.y - dy) / TILE, zoom);
    setCenter({ lat: clamp(next.lat, -85, 85), lng: next.lng });
  };
  const onPointerUp = (e: React.PointerEvent<HTMLDivElement>) => {
    const d = drag.current;
    drag.current = null;
    if (!d) return;
    if (!d.moved) {
      // A click (not a pan): drop the pin at the cursor.
      const rect = e.currentTarget.getBoundingClientRect();
      const picked = pxToLatLng(e.clientX - rect.left, e.clientY - rect.top);
      setCoords({ lat: +picked.lat.toFixed(6), lng: +picked.lng.toFixed(6) });
    }
  };

  const zoomBy = (delta: number) => setZoom((z) => clamp(z + delta, MIN_Z, MAX_Z));

  const useCurrent = () => {
    if (!navigator.geolocation) {
      toast(t("map.unavailable"), "error");
      return;
    }
    navigator.geolocation.getCurrentPosition(
      (p) => {
        const c = { lat: +p.coords.latitude.toFixed(6), lng: +p.coords.longitude.toFixed(6) };
        setCoords(c);
        setCenter(c);
        setZoom((z) => Math.max(z, 15));
      },
      () => toast(t("map.failed"), "error"),
    );
  };

  // Build the visible tile grid: every tile overlapping the viewport.
  const tiles: { key: string; src: string; left: number; top: number }[] = [];
  if (size.w > 0) {
    const n = 2 ** zoom;
    const topLeftWorldX = centerWorldPx.x - size.w / 2;
    const topLeftWorldY = centerWorldPx.y - size.h / 2;
    const x0 = Math.floor(topLeftWorldX / TILE);
    const y0 = Math.floor(topLeftWorldY / TILE);
    const x1 = Math.floor((topLeftWorldX + size.w) / TILE);
    const y1 = Math.floor((topLeftWorldY + size.h) / TILE);
    for (let tx = x0; tx <= x1; tx++) {
      for (let ty = y0; ty <= y1; ty++) {
        if (ty < 0 || ty >= n) continue;
        const wrappedX = ((tx % n) + n) % n;
        tiles.push({
          key: `${zoom}/${tx}/${ty}`,
          src: `https://tile.openstreetmap.org/${zoom}/${wrappedX}/${ty}.png`,
          left: tx * TILE - centerWorldPx.x + size.w / 2,
          top: ty * TILE - centerWorldPx.y + size.h / 2,
        });
      }
    }
  }

  const pin = latLngToPx(coords.lat, coords.lng);

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
        <div
          ref={boxRef}
          className="map-box"
          onPointerDown={onPointerDown}
          onPointerMove={onPointerMove}
          onPointerUp={onPointerUp}
          style={{ cursor: "grab", touchAction: "none" }}
        >
          {tiles.map((tl) => (
            <img
              key={tl.key}
              src={tl.src}
              alt=""
              draggable={false}
              width={TILE}
              height={TILE}
              style={{ position: "absolute", left: tl.left, top: tl.top, width: TILE, height: TILE, userSelect: "none", pointerEvents: "none" }}
            />
          ))}
          <div className="map-pin" style={{ position: "absolute", left: pin.x, top: pin.y, transform: "translate(-50%, -100%)", pointerEvents: "none" }}>
            <IconPin />
          </div>
          <div className="map-zoom" style={{ position: "absolute", insetInlineEnd: 8, top: 8, display: "flex", flexDirection: "column", gap: 4 }}>
            <button type="button" className="btn btn-secondary btn-sm" aria-label="zoom in" onClick={() => zoomBy(1)}>+</button>
            <button type="button" className="btn btn-secondary btn-sm" aria-label="zoom out" onClick={() => zoomBy(-1)}>−</button>
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
                toast(t("common.copied"), "info");
              }}
              aria-label="copy"
            >
              <IconCopy />
            </button>
          </div>
          <button className="btn btn-secondary btn-sm" onClick={useCurrent}>
            <IconPin /> {t("map.myLocation")}
          </button>
        </div>
        <p className="field-hint">{t("map.hint")}</p>
      </div>
    </Dialog>
  );
}
