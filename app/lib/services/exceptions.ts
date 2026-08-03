/* Exceptions + capacity service — real calls to /api/Transportation (docs/API.md §6.8).
   Maps the backend's frozen PascalCase envelope keys → the app's Exception type. */
import type { Exception, Period } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

/** Backend Period strings (Go | Return | Both) ↔ the app's lowercase Period. */
export function periodToApi(p: Period): string {
  return p === "return" ? "Return" : p === "both" ? "Both" : "Go";
}
function periodFromApi(p?: string): Period {
  const v = (p ?? "").toLowerCase();
  return v === "return" ? "return" : v === "both" ? "both" : "go";
}

/** One row of the GetEmployeeException list (TransportationExceptionData). */
interface ExceptionRow {
  Id: string;
  HrUserId: string;
  TransportationVehicleRouteId: number;
  TransportationLineName: string;
  TransportationVehicleRouteDirectionName: string;
  FromDate: string;
  ToDate: string;
  Period: string;
  DayName: string;
  ExceptionDate: string;
  Latitude: number;
  Longtitud: number; // frozen typo key
  HrUserName: string;
  ReasonException: string;
  ContactNumber: string;
}

function toException(r: ExceptionRow): Exception {
  const iso = (s?: string) => (s ? s.split("T")[0] : "");
  return {
    id: String(r.Id),
    routeName: r.TransportationLineName ?? "",
    passenger: r.HrUserName ?? "",
    station: r.TransportationVehicleRouteDirectionName ?? "",
    lat: r.Latitude || undefined,
    lng: r.Longtitud || undefined,
    date: iso(r.ExceptionDate),
    fromDate: iso(r.FromDate) || undefined,
    toDate: iso(r.ToDate) || undefined,
    period: periodFromApi(r.Period),
    weekdays: r.DayName || undefined,
    reason: r.ReasonException ?? "",
    contact: r.ContactNumber ?? "",
  };
}

/** GET GetEmployeeException — paginated exceptions (optional HrUserId). */
export async function getExceptions(
  query: { hrUserId?: string; pageNo?: number; noOfItems?: number } = {},
): Promise<Paginated<Exception>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<ExceptionRow[]>("GetEmployeeException", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    HrUserId: query.hrUserId,
  });
  const items = (res.Data ?? []).map(toException);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** Payload accepted by Add/UpdateException (documented keys, §6.8). Period-mode
 *  carries FromDate/ToDate/DayName or ExceptionDate; location-mode carries
 *  Latitude/Longtitud or TransportationVehicleRouteDirectionId. */
export interface ExceptionInput {
  transportationVehicleRouteId: number | string;
  hrUserId: number | string;
  Active?: boolean;
  reasonException: string;
  contactNumber: string;
  Period: string; // Go | Return | Both
  FromDate?: string;
  ToDate?: string;
  DayName?: string;
  ExceptionDate?: string;
  Latitude?: number;
  Longtitud?: number; // frozen typo key
  TransportationVehicleRouteDirectionId?: number | string;
}

function toBody(input: ExceptionInput): Record<string, unknown> {
  return {
    transportationVehicleRouteId: Number(input.transportationVehicleRouteId) || 0,
    hrUserId: Number(input.hrUserId) || 0,
    Active: input.Active ?? true,
    reasonException: input.reasonException,
    contactNumber: input.contactNumber,
    Period: input.Period,
    FromDate: input.FromDate,
    ToDate: input.ToDate,
    DayName: input.DayName,
    ExceptionDate: input.ExceptionDate,
    Latitude: input.Latitude,
    Longtitud: input.Longtitud,
    TransportationVehicleRouteDirectionId:
      input.TransportationVehicleRouteDirectionId != null && input.TransportationVehicleRouteDirectionId !== ""
        ? Number(input.TransportationVehicleRouteDirectionId)
        : undefined,
  };
}

/** POST AddException — add an employee exception. Returns the new id. */
export async function addException(input: ExceptionInput): Promise<string> {
  const res = await apiPost<unknown>("AddException", toBody(input));
  return String((res.Data as { Id?: string | number } | undefined)?.Id ?? "");
}

/** POST UpdateException — edit an exception (payload + id). */
export async function updateException(input: ExceptionInput & { id: string }): Promise<void> {
  await apiPost("UpdateException", { ...toBody(input), id: Number(input.id) });
}

/** GET GetCapacityNumbers — capacity figures for a route + period. */
export interface CapacityNumbers {
  fullCapacity: number;
  actualCapacity: number;
  capacityWithoutExceptions: number;
  fromOtherLines: number;
  toOtherLines: number;
}
interface CapacityRow {
  FullCapacity: number;
  ActualCapacity: number;
  CapacityWithoutExpection: number; // frozen typo key
  ExpectionNumFromOtherLines: number; // frozen typo key
  RouteEmployeesToOtherLines: number;
}
export async function getCapacityNumbers(routeId: string, period: string): Promise<CapacityNumbers> {
  const res = await apiGet<CapacityRow>("GetCapacityNumbers", { RouteID: routeId, Period: period });
  const d = res.Data ?? ({} as CapacityRow);
  return {
    fullCapacity: d.FullCapacity ?? 0,
    actualCapacity: d.ActualCapacity ?? 0,
    capacityWithoutExceptions: d.CapacityWithoutExpection ?? 0,
    fromOtherLines: d.ExpectionNumFromOtherLines ?? 0,
    toOtherLines: d.RouteEmployeesToOtherLines ?? 0,
  };
}
