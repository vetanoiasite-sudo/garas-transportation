/* API transport scaffold.
 *
 * These service modules mirror the documented Transportation endpoints
 * (see docs/API.md and docs/PROVIDERS.md). They currently return the in-memory
 * mock data from `lib/data.ts` after a small simulated latency.
 *
 * TODO (backend integration): replace `mock(...)` with a real HTTP client
 * (axios/fetch) targeting the CoreApi host. Every request must send the
 * `CompanyName` + `UserToken` headers; list filters/pagination ride in HTTP
 * headers (PageNo/NoOfItems/…), not query strings. Responses use the envelope
 * `{ result, errors, data, paginationHeader }`. Do NOT wire any of this to a
 * live backend as part of the current frontend-only scope.
 */

/** Base path for every Transportation endpoint (docs/API.md §1). */
export const API_BASE = "/api/Transportation";

/** Standard response envelope returned by the CoreApi (docs/API.md §4). */
export interface ApiEnvelope<T> {
  result: boolean;
  errors: { errorCode: string; errorMsg: string }[];
  data: T;
}

/** Pagination header carried by list endpoints (docs/MODELS.md). */
export interface PaginationHeader {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

/** Result of a write (add/edit/delete/approve) — mirrors `PostResponseModel`. */
export interface WriteResult {
  ok: boolean;
  id?: string;
  message?: string;
}

const LATENCY_MS = 150;

/** Resolve with mock data after a small delay to mimic network latency. */
export function mock<T>(data: T): Promise<T> {
  return new Promise((resolve) => setTimeout(() => resolve(data), LATENCY_MS));
}

/** Wrap an array in a mock paginated response. */
export function paginate<T>(items: T[], pageNo = 1, noOfItems = 20): Promise<Paginated<T>> {
  const start = (pageNo - 1) * noOfItems;
  const slice = items.slice(start, start + noOfItems);
  return mock({
    items: slice,
    pagination: {
      currentPage: pageNo,
      itemsPerPage: noOfItems,
      totalItems: items.length,
      totalPages: Math.max(1, Math.ceil(items.length / noOfItems)),
    },
  });
}

/** Mock a successful write response. */
export function ok(id?: string, message?: string): Promise<WriteResult> {
  return mock({ ok: true, id, message });
}

/** Common list query shape (all optional; sent as headers by the real client). */
export interface ListQuery {
  pageNo?: number;
  noOfItems?: number;
  supplierId?: string;
  contactPersonId?: string;
  lineId?: string;
  routeId?: string;
  serial?: string;
  from?: string;
  to?: string;
  date?: string;
}
