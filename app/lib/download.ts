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
