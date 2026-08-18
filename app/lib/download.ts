/* Shared helpers for turning a backend base64 payload into a downloaded file.
   The Transportation Excel endpoints return the .xlsx as a base64 string inside
   the envelope Data; these helpers decode it and trigger a browser download. */

const XLSX_MIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

/** Decode a base64 string into a Blob (defaults to the .xlsx MIME type). */
export function base64ToBlob(base64: string, type = XLSX_MIME): Blob {
  const bytes = atob(base64 ?? "");
  const arr = new Uint8Array(bytes.length);
  for (let i = 0; i < bytes.length; i++) arr[i] = bytes.charCodeAt(i);
  return new Blob([arr], { type });
}

/** Trigger a browser download for a Blob under the given file name. */
export function saveBlob(blob: Blob, filename: string): void {
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  a.download = filename;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}

/** Decode a base64 .xlsx payload and download it in one step. */
export function saveBase64Xlsx(base64: string, filename: string): void {
  saveBlob(base64ToBlob(base64), filename);
}

/* The CoreApi's Excel endpoints don't return base64 — they write the workbook
   under wwwroot and put its URL in Data, e.g.
     "https://localhost:7178//Attachments\\garastest\\Excel\\file.xlsx".
   The URL is normalised here (backslashes, doubled slashes, and the reference
   config's hardcoded host, which is not where this API actually runs). */

/** Turn the backend's file path/URL into one the browser can fetch. */
export function fileUrl(raw?: string | null): string {
  if (!raw) return "";
  const cleaned = String(raw).replace(/\\/g, "/");
  const apiBase = (import.meta.env.VITE_API_URL ?? "").replace(/\/$/, "");
  // Absolute URL from the server: keep only the path and re-root it on the API
  // we're actually talking to.
  const m = cleaned.match(/^https?:\/\/[^/]+\/(.*)$/i);
  const path = m ? m[1] : cleaned.replace(/^\/+/, "");
  return `${apiBase}/${path.replace(/\/{2,}/g, "/")}`;
}

/** Open a backend-generated file (Excel report) in a new tab / download it. */
export function openFileUrl(raw?: string | null, filename?: string): void {
  const url = fileUrl(raw);
  if (!url) return;
  const a = document.createElement("a");
  a.href = url;
  a.target = "_blank";
  a.rel = "noopener";
  if (filename) a.download = filename;
  document.body.appendChild(a);
  a.click();
  a.remove();
}
