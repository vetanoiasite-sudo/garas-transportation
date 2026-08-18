/* Line cost report service — real calls to /api/Transportation (ReportCostOfLines).
   Maps the backend's frozen TransportationReportCostData keys → CostReportRow.

   NOTE: round-based figures (CountOfround / DeductPerRound / DeductRoundNum) come
   back as 0 until the accounts feed is wired (see backend costs.service.ts); the
   table and the .xlsx export populate once those figures are filled in. */
import type { CostReportRow } from "@/lib/types";
import { apiGet, type PaginationHeader } from "@/lib/api/client";
import { fileUrl } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

// TransportationReportCostData row (frozen PascalCase / odd-casing keys).
interface CostRow {
  LineName?: string;
  NameOfRoute?: string;
  SupplierName?: string;
  Serial?: string; // may be absent
  SupplierContactPersonName?: string; // driver — may be absent
  LineCost?: number;
  OneWay?: boolean;
  CountOfround?: number; // frozen odd-casing key
  DeductPerRound?: number;
  DeductRoundNum?: number;
  NetCost?: number;
}

const num = (v: unknown): number => {
  const x = Number(v);
  return Number.isFinite(x) ? x : 0;
};

function toCostRow(r: CostRow, i: number): CostReportRow {
  const deductionRounds = num(r.DeductRoundNum);
  const deductPerRound = num(r.DeductPerRound);
  return {
    id: String(i), // backend cost rows carry no Id — index is stable within a page
    lineName: r.LineName ?? "",
    routeName: r.NameOfRoute ?? "",
    serial: r.Serial ?? "",
    supplier: r.SupplierName ?? "",
    driver: r.SupplierContactPersonName ?? "",
    cost: num(r.LineCost),
    oneWay: !!r.OneWay,
    rounds: num(r.CountOfround),
    deductionRounds,
    // DeductPerRound is already Sum(DeductPerRound) server-side — NOT a unit
    // rate, and NetCost subtracts it once. Multiplying inflated it by the row count.
    deductionCost: deductPerRound,
    netCost: num(r.NetCost),
  };
}

export interface CostQuery {
  pageNo?: number;
  noOfItems?: number;
  supplierId?: string;
  driverId?: string;
  lineId?: string;
  serial?: string;
  from?: string;
  to?: string;
}

/** GET ReportCostOfLines — paginated line-cost report (filters ride in headers). */
export async function getCostReport(query: CostQuery = {}): Promise<Paginated<CostReportRow>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<CostRow[]>("ReportCostOfLines", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    SupplierId: query.supplierId,
    supplierContactPersonId: query.driverId,
    TransportionlineId: query.lineId, // frozen typo header
    serialBus: query.serial,
    DateFrom: query.from,
    DateTo: query.to,
  });
  const items = (res.Data ?? []).map(toCostRow);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET ReportCostOfLinesExcell — the filtered line-cost report as an .xlsx Blob. */
export async function downloadCostReportExcel(query: CostQuery = {}): Promise<string> {
  const res = await apiGet<string>("ReportCostOfLinesExcell", {
    SupplierId: query.supplierId,
    supplierContactPersonId: query.driverId,
    TransportionlineId: query.lineId, // frozen typo header
    serialBus: query.serial,
    DateFrom: query.from,
    DateTo: query.to,
  });
  return fileUrl(res.Data);
}
