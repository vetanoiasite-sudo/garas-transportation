/* HTTP client for the Garas transportation backend.
 *
 * Faithful to the documented contract: filters/pagination ride in HTTP HEADERS
 * (not query strings), auth is CompanyName + UserToken headers, and every
 * response is the { Result, Errors, Data, PaginationHeader } envelope.
 *
 * Base URL comes from NEXT_PUBLIC_API_URL (default http://localhost:4000).
 * This module runs client-side (pages fetch in effects), so it reads the auth
 * cookie via document.cookie. */

import { AUTH_COOKIE } from "@/lib/auth";
import { demoResponse } from "@/lib/api/demo";

export const API_BASE = import.meta.env.VITE_API_URL ?? "http://192.168.1.151:8888";
// Offline demo build: serve canned data, never touch a backend.
const DEMO = import.meta.env.VITE_DEMO === "true";

export interface ErrorItem { ErrorCode: string; ErrorMSG: string }
export interface PaginationHeader { CurrentPage: number; ItemsPerPage: number; TotalItems: number; TotalPages: number }
export interface Envelope<T> {
  Result: boolean;
  Errors: ErrorItem[];
  Data: T;
  PaginationHeader?: PaginationHeader;
  // Write/ack responses (successWrite) return the new/affected row id here and an
  // optional message — not in Data.
  Id?: string | number;
  Message?: string;
}

/** Thrown when the envelope reports Result:false (or the network fails). */
export class ApiError extends Error {
  constructor(public code: string, message: string) {
    super(message);
    this.name = "ApiError";
  }
}

function readAuth(): { companyName?: string; token?: string; userId?: string } {
  if (typeof document === "undefined") return {};
  const raw = document.cookie.split("; ").find((c) => c.startsWith(`${AUTH_COOKIE}=`))?.split("=")[1];
  if (!raw) return {};
  try {
    const u = JSON.parse(decodeURIComponent(raw));
    return { companyName: u.companyName, token: u.token, userId: u.userId };
  } catch {
    return {};
  }
}

/** The signed-in user's AES-encrypted id — some write endpoints want it in the
 *  body (`CreatedBy`) and decrypt it server-side. */
export function currentUserId(): string | undefined {
  return readAuth().userId;
}

/** Session-expiry codes (see backend §3.2). Matching Err-P2 also catches Err-P200. */
function isSessionExpired(errors: ErrorItem[]): boolean {
  return errors.some((e) => e.ErrorCode?.includes("Err-P2"));
}

// Header values are always strings; drop undefined/empty ones (faithful: only
// send a filter header when it has a value).
type HeaderMap = Record<string, string | number | boolean | undefined | null>;
function toHeaders(map: HeaderMap): Record<string, string> {
  const out: Record<string, string> = {};
  for (const [k, v] of Object.entries(map)) {
    if (v === undefined || v === null || v === "") continue;
    out[k] = String(v);
  }
  return out;
}

async function request<T>(
  method: "GET" | "POST",
  path: string,
  opts: { headers?: HeaderMap; body?: unknown; auth?: boolean } = {},
): Promise<Envelope<T>> {
  // Demo build: resolve from the captured dataset instead of the network.
  if (DEMO) {
    const json = demoResponse(method, path, opts.headers as Record<string, unknown>) as Envelope<T>;
    if (!json.Result) {
      const first = (json.Errors ?? [])[0];
      throw new ApiError(first?.ErrorCode ?? "error", first?.ErrorMSG ?? "حدث خطأ");
    }
    return json;
  }

  const { companyName, token } = readAuth();
  const headers: Record<string, string> = {
    Accept: "application/json",
    "Content-Type": "application/json",
    ...toHeaders(opts.headers ?? {}),
  };
  if (opts.auth !== false) {
    if (companyName) headers.CompanyName = companyName;
    if (token) headers.UserToken = token;
  }

  let res: Response;
  try {
    res = await fetch(`${API_BASE}${path}`, {
      method,
      headers,
      body: opts.body !== undefined ? JSON.stringify(opts.body) : undefined,
    });
  } catch {
    throw new ApiError("network", "تعذّر الاتصال بالخادم");
  }

  const json = (await res.json().catch(() => null)) as Envelope<T> | null;
  if (!json || typeof json.Result !== "boolean") {
    throw new ApiError("bad-response", `استجابة غير متوقعة (${res.status})`);
  }
  if (!json.Result) {
    const errs = json.Errors ?? [];
    if (isSessionExpired(errs) && typeof window !== "undefined") {
      // clear the session and bounce to login
      document.cookie = `${AUTH_COOKIE}=; path=/; max-age=0; samesite=lax`;
      window.location.href = "/ar/login";
    }
    const first = errs[0];
    throw new ApiError(first?.ErrorCode ?? "error", first?.ErrorMSG ?? "حدث خطأ");
  }
  return json;
}

/** GET a Transportation action; filters/pagination go in `headers`. */
export function apiGet<T>(action: string, headers?: HeaderMap): Promise<Envelope<T>> {
  return request<T>("GET", `/api/Transportation/${action}`, { headers });
}
/** POST a Transportation action (writes/deletes/approvals). */
export function apiPost<T>(action: string, body?: unknown, headers?: HeaderMap): Promise<Envelope<T>> {
  return request<T>("POST", `/api/Transportation/${action}`, { body, headers });
}
/** Call an arbitrary path (e.g. /User/Login, /Supplier/GetSuppliersCards). */
export function apiRaw<T>(method: "GET" | "POST", path: string, opts: { headers?: HeaderMap; body?: unknown; auth?: boolean } = {}): Promise<Envelope<T>> {
  return request<T>(method, path, opts);
}

/** Flatten a nested object into ASP.NET model-binding form keys (`a.b`, `a[0].c`). */
function appendForm(form: FormData, value: unknown, prefix = ""): void {
  if (value === undefined || value === null) return;
  if (value instanceof File || value instanceof Blob) {
    form.append(prefix, value);
  } else if (Array.isArray(value)) {
    value.forEach((v, i) => appendForm(form, v, `${prefix}[${i}]`));
  } else if (typeof value === "object") {
    for (const [k, v] of Object.entries(value as Record<string, unknown>)) {
      appendForm(form, v, prefix ? `${prefix}.${k}` : k);
    }
  } else {
    form.append(prefix, String(value));
  }
}

/** POST a multipart/form-data action — the CoreApi's [FromForm] endpoints
 *  (CreateHrUser, CreateHrUserWithAllRoutes, EditHrEmployee …). */
export async function apiForm<T>(path: string, payload: Record<string, unknown>): Promise<Envelope<T>> {
  const form = new FormData();
  appendForm(form, payload);
  const { companyName, token } = readAuth();
  const headers: Record<string, string> = { Accept: "application/json" }; // no Content-Type: the browser sets the boundary
  if (companyName) headers.CompanyName = companyName;
  if (token) headers.UserToken = token;

  let res: Response;
  try {
    res = await fetch(`${API_BASE}${path.startsWith("/") ? path : `/api/Transportation/${path}`}`, { method: "POST", headers, body: form });
  } catch {
    throw new ApiError("network", "تعذّر الاتصال بالخادم");
  }
  const json = (await res.json().catch(() => null)) as Envelope<T> | null;
  if (!json || typeof json.Result !== "boolean") throw new ApiError("bad-response", `استجابة غير متوقعة (${res.status})`);
  if (!json.Result) {
    const first = (json.Errors ?? [])[0];
    throw new ApiError(first?.ErrorCode ?? "error", first?.ErrorMSG ?? "حدث خطأ");
  }
  return json;
}
