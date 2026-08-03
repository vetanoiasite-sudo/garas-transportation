/* Repricing (with approval workflow) — real calls to /api/Transportation.
   Maps the backend's frozen PascalCase envelope keys → the app's Repricing type
   (see backend repricing.service.ts for the exact keys). */
import type { Repricing } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

interface LineDetailRow {
  RouteId?: number | string;
  RouteName?: string;
  PriceBefore?: number;
  PriceAfter?: number;
}

interface RepricingRow {
  Id: number | string;
  IsPercent?: boolean;
  IncreaseCost?: number;
  ForAllLines?: boolean;
  Approve?: boolean | null;
  ApprovedByName?: string;
  CreationName?: string;
  CreationDate?: string;
  StartDate?: string;
  transportationLineDetails?: LineDetailRow[];
}

const dateOnly = (s?: string): string => (s ? String(s).slice(0, 10) : "");

function toRepricing(r: RepricingRow): Repricing {
  return {
    id: String(r.Id),
    amount: Number(r.IncreaseCost) || 0,
    mode: r.IsPercent ? "percent" : "fixed",
    forAllLines: !!r.ForAllLines,
    createdBy: r.CreationName || "—",
    createdAt: dateOnly(r.CreationDate),
    approved: r.Approve === true,
    approvedBy: r.ApprovedByName || undefined,
    // Backend getAll does not currently return StartDate; fall back to CreationDate.
    startDate: dateOnly(r.StartDate || r.CreationDate),
    lines: (r.transportationLineDetails ?? []).map((l) => ({
      lineName: l.RouteName ?? "",
      before: Number(l.PriceBefore) || 0,
      after: Number(l.PriceAfter) || 0,
    })),
  };
}

/** GET getAllModifyPriceOfTransportationLine — repricing records (optional Approve filter). */
export async function getRepricings(
  query: { approved?: boolean; pageNo?: number; noOfItems?: number } = {},
): Promise<Paginated<Repricing>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<RepricingRow[]>("getAllModifyPriceOfTransportationLine", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    Approve: query.approved === undefined ? undefined : String(query.approved),
  });
  const items = (res.Data ?? []).map(toRepricing);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** POST ModifyPriceOfTransportationLine — create a repricing (unapproved). */
export async function createRepricing(payload: {
  amount: number;
  mode: "percent" | "fixed";
  forAllLines: boolean;
  approximateToFive: boolean;
  startDate: string;
  lineIds: string[]; // ROUTE ids
}): Promise<string> {
  const res = await apiPost<unknown>("ModifyPriceOfTransportationLine", {
    IncreaseCost: payload.amount,
    IsPercent: payload.mode === "percent",
    ForAllLines: payload.forAllLines,
    ApproximateToFiveFlag: payload.approximateToFive,
    TransportationLineIds: payload.lineIds.map((id) => Number(id)).filter((n) => Number.isFinite(n) && n > 0),
    StartDate: payload.startDate || undefined,
  });
  return String((res as { Id?: string | number }).Id ?? "");
}

/** POST UpdatePriceOfTransportationLine — approve/apply a repricing (Id header). */
export async function approveRepricing(id: string): Promise<void> {
  await apiPost("UpdatePriceOfTransportationLine", {}, { Id: id });
}

/** POST RejectUpdatePrice — reject a repricing (Id header; super admin only). */
export async function rejectRepricing(id: string): Promise<void> {
  await apiPost("RejectUpdatePrice", {}, { Id: id });
}
