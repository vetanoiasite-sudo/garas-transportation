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
    // ApprovedByName is projected from CreationByNavigation (the requester), so
    // it is only meaningful once the request has actually been approved.
    approvedBy: r.Approve === true ? r.ApprovedByName || undefined : undefined,
    // Backend getAll does not currently return StartDate; fall back to CreationDate.
    startDate: dateOnly(r.StartDate || r.CreationDate),
    // For percentage requests the API puts the INCREASE in PriceAfter, not the
    // resulting price (approval then applies before + increase) — so the new
    // price is reconstructed here to match what approving will actually save.
    lines: (r.transportationLineDetails ?? []).map((l) => {
      const before = Number(l.PriceBefore) || 0;
      const raw = Number(l.PriceAfter) || 0;
      return { lineName: l.RouteName ?? "", before, after: r.IsPercent ? before + raw : raw };
    }),
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
    // The write DTO spells it IsPrecent (frozen typo) while the read model
    // returns IsPercent — sending IsPercent here silently saved "fixed amount".
    IsPrecent: payload.mode === "percent",
    ForAllLines: payload.forAllLines,
    ApproximateToFiveFlag: payload.approximateToFive,
    TransportationLineIds: payload.lineIds.map((id) => Number(id)).filter((n) => Number.isFinite(n) && n > 0),
    // Non-nullable DateTime → omitting it binds 01/01/0001, which the
    // datetime column rejects with a raw SQL error. Default to today.
    StartDate: payload.startDate || new Date().toISOString().slice(0, 10),
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
