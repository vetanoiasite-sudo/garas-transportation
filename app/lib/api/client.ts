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

export const API_BASE = import.meta.env.VITE_API_URL ?? "http://localhost:4000";

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

function readAuth(): { companyName?: string; token?: string } {
  if (typeof document === "undefined") return {};
  const raw = document.cookie.split("; ").find((c) => c.startsWith(`${AUTH_COOKIE}=`))?.split("=")[1];
  if (!raw) return {};
  try {
    const u = JSON.parse(decodeURIComponent(raw));
    return { companyName: u.companyName, token: u.token };
  } catch {
    return {};
  }
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
/** Call an arbitrary path (e.g. /User/Login, /User/GetAllUsers). */
export function apiRaw<T>(method: "GET" | "POST", path: string, opts: { headers?: HeaderMap; body?: unknown; auth?: boolean } = {}): Promise<Envelope<T>> {
  return request<T>(method, path, opts);
}
