/* Lines service — real calls to the backend (/api/Transportation).
   Maps the backend's frozen PascalCase envelope keys → the app's Line type. */
import type { Line } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

interface LineRow {
  Id: number;
  Name: string;
  RouteNum: number;
  IsApproved: boolean;
}

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

function toLine(l: LineRow): Line {
  return { id: String(l.Id), name: l.Name, routesCount: l.RouteNum ?? 0, approved: !!l.IsApproved };
}

/** GET getAllTransportationLine — paginated lines with an optional server-side name search. */
export async function getLines(pageNo = 1, noOfItems = 20, name?: string): Promise<Paginated<Line>> {
  const res = await apiGet<LineRow[]>("getAllTransportationLine", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it).
    Name: name ? encodeURIComponent(name) : undefined,
  });
  const items = (res.Data ?? []).map(toLine);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** POST AddTransportationLine — returns the new id. */
export async function addLine(name: string): Promise<string> {
  const res = await apiPost<unknown>("AddTransportationLine", { LineName: name });
  return String((res as { Id?: string | number }).Id ?? "");
}

/** POST UpdateTransportationLine. */
export async function updateLine(id: string, name: string): Promise<void> {
  await apiPost("UpdateTransportationLine", { Id: Number(id), LineName: name });
}

/** POST DeleteTransportationLine (Id header). */
export async function deleteLine(id: string): Promise<void> {
  await apiPost("DeleteTransportationLine", {}, { Id: id });
}

/** POST ApproveTransportationLine (Id + Approve headers). */
export async function approveLine(id: string, approve = true): Promise<void> {
  await apiPost("ApproveTransportationLine", {}, { Id: id, Approve: String(approve) });
}
