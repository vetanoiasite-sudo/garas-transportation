/* Deductions service — real calls to /api/Transportation (docs/API.md §6.10).
   Maps the backend's frozen PascalCase envelope keys → the app's Deduction type. */
import type { Deduction } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

/** One row of the getAllDeduction list (TransportationDeductionData). */
interface DeductionRow {
  Id: number;
  SupplierName: string;
  SupplierId: number;
  TransportationVehicleRouteName: string;
  TransportationVehicleRouteId: number;
  Serial: string;
  DateOfDeduction: string; // RouteDeductionVM's field (there is no `Date`)
  CreationDate: string;
  DeductPerRound: number;
  Cause: string;
  CreationName: string;
  SupplierContentPersonIdName: string; // driver name (frozen typo key)
  typeOfDeduction: string; // Taxes | Normal (lowercase frozen key)
}

function toDeduction(d: DeductionRow): Deduction {
  const iso = (s?: string) => (s ? s.split("T")[0] : "");
  return {
    id: String(d.Id),
    routeName: d.TransportationVehicleRouteName ?? "",
    supplier: d.SupplierName ?? "",
    driver: d.SupplierContentPersonIdName || "—",
    day: iso(d.DateOfDeduction),
    amount: d.DeductPerRound ?? 0,
    createdBy: d.CreationName ?? "",
    createdAt: iso(d.CreationDate),
    reason: d.Cause ?? "",
    type: (d.typeOfDeduction ?? "").toLowerCase() === "taxes" ? "tax" : "normal",
  };
}

export interface DeductionQuery {
  supplierId?: string;
  routeId?: string;
  name?: string; // route name search
  from?: string;
  to?: string;
  pageNo?: number;
  noOfItems?: number;
}

// Filter headers shared by the list read and the Excel export (server-side).
function deductionHeaders(query: DeductionQuery): Record<string, string | number | undefined> {
  return {
    SupplierId: query.supplierId,
    RouteId: query.routeId,
    FromDate: query.from,
    ToDate: query.to,
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
    Name: query.name ? encodeURIComponent(query.name) : undefined,
  };
}

/** GET getAllDeduction — paginated deductions (all filters server-side). */
export async function getDeductions(query: DeductionQuery = {}): Promise<Paginated<Deduction>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<DeductionRow[]>("getAllDeduction", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    ...deductionHeaders(query),
  });
  const items = (res.Data ?? []).map(toDeduction);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET getAllDeductionExcel — the filtered deductions as an .xlsx Blob. */
export async function downloadDeductionsExcel(query: DeductionQuery = {}): Promise<Blob> {
  const res = await apiGet<string>("getAllDeductionExcel", deductionHeaders(query));
  const base64 = res.Data ?? "";
  const bytes = atob(base64);
  const arr = new Uint8Array(bytes.length);
  for (let i = 0; i < bytes.length; i++) arr[i] = bytes.charCodeAt(i);
  return new Blob([arr], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
}

/** Payload accepted by Add/UpdateDeduction (documented PascalCase keys, §6.10). */
export interface DeductionInput {
  SupplierId: number | string;
  Serial: string;
  TransportationVehicleRouteId: number | string;
  DeductPerRound: number;
  Cause: string;
  DateOfDeduction: string; // ISO
  TypeOfDedct: string; // Taxes | Normal
}

function toBody(input: DeductionInput): Record<string, unknown> {
  return {
    SupplierId: Number(input.SupplierId) || 0,
    Serial: input.Serial,
    TransportationVehicleRouteId: Number(input.TransportationVehicleRouteId) || 0,
    DeductPerRound: Number(input.DeductPerRound) || 0,
    Cause: input.Cause,
    DateOfDeduction: input.DateOfDeduction,
    TypeOfDedct: input.TypeOfDedct,
  };
}

/** POST AddDeduction — create a deduction. Returns the new id. */
export async function addDeduction(input: DeductionInput): Promise<string> {
  const res = await apiPost<unknown>("AddDeduction", toBody(input));
  return String((res.Data as { Id?: string | number } | undefined)?.Id ?? "");
}

/** POST UpdateDeduction — edit a deduction (payload + Id). */
export async function updateDeduction(input: DeductionInput & { id: string }): Promise<void> {
  await apiPost("UpdateDeduction", { ...toBody(input), Id: Number(input.id) });
}
