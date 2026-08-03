/* Suppliers, statement & payments service — real calls to /api/Transportation.
   Maps the backend's frozen PascalCase envelope keys → the app's Supplier /
   StatementRow / SupplierPayment types (see backend suppliers.service.ts). */
import type { Supplier, StatementRow, SupplierPayment, SupplierPaymentDistribution } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";
import { base64ToBlob } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

const dateOnly = (s?: string): string => (s ? String(s).slice(0, 10) : "");

/* ---- Suppliers ---- */

interface ContactRow {
  Id: number | string;
  Name?: string;
  Mobile?: string;
}
interface SupplierRow {
  Id: number | string;
  Name?: string;
  Email?: string;
  Phone?: string;
  Mobile?: string;
  Fax?: string;
  Address?: string;
  CreationDate?: string;
  ActiveRoutes?: number;
  contacts?: ContactRow[];
}

function toSupplier(s: SupplierRow): Supplier {
  return {
    id: String(s.Id),
    name: s.Name ?? "",
    createdAt: dateOnly(s.CreationDate),
    phone: s.Phone || undefined,
    mobile: s.Mobile || undefined,
    email: s.Email || undefined,
    fax: s.Fax || undefined,
    address: s.Address || undefined,
    activeRoutes: s.ActiveRoutes ?? 0,
    contacts: (s.contacts ?? []).map((c) => ({ id: String(c.Id), name: c.Name ?? "", mobile: c.Mobile ?? "" })),
  };
}

/** GET getSuppliers — paginated suppliers (name/phone/mobile filters ride in headers). */
export async function getSuppliers(
  query: { pageNo?: number; noOfItems?: number; name?: string; phone?: string; mobile?: string } = {},
): Promise<Paginated<Supplier>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<SupplierRow[]>("getSuppliers", {
    PageNo: pageNo,
    NoOfItems: noOfItems,
    // Percent-encode so Arabic survives the HTTP header hop (backend decodes it) —
    // a raw non-ASCII header value throws a ByteString error in fetch.
    Name: query.name ? encodeURIComponent(query.name) : undefined,
    Phone: query.phone,
    Mobile: query.mobile,
  });
  const items = (res.Data ?? []).map(toSupplier);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** A contact person as edited in the form (new ones have a temporary "c…" id). */
export interface SupplierContactInput {
  id?: string;
  name: string;
  mobile?: string;
}

// Only real (numeric) ids identify an existing contact; temp "c…" ids are new.
function toContactPayload(contacts: SupplierContactInput[] = []) {
  return contacts
    .filter((c) => c.name.trim())
    .map((c) => ({
      Id: c.id && /^\d+$/.test(c.id) ? Number(c.id) : undefined,
      Name: c.name.trim(),
      Mobile: c.mobile?.trim() || "",
    }));
}

/** POST addSupplier — create a supplier (with contact persons); returns the new id. */
export async function addSupplier(payload: {
  name: string;
  email?: string;
  phone?: string;
  mobile?: string;
  fax?: string;
  address?: string;
  contacts?: SupplierContactInput[];
}): Promise<string> {
  const res = await apiPost<unknown>("addSupplier", {
    Name: payload.name,
    Email: payload.email ?? "",
    Phone: payload.phone ?? "",
    Mobile: payload.mobile ?? "",
    Fax: payload.fax ?? "",
    Address: payload.address ?? "",
    Contacts: toContactPayload(payload.contacts),
  });
  return String((res as { Id?: string | number }).Id ?? "");
}

/** GET getSupplier — a single supplier + contacts (Id header). */
export async function getSupplier(id: string): Promise<Supplier | undefined> {
  const res = await apiGet<SupplierRow | null>("getSupplier", { Id: id });
  return res.Data ? toSupplier(res.Data) : undefined;
}

/** POST updateSupplier — edit a supplier (and sync its contact persons). */
export async function updateSupplier(id: string, payload: {
  name: string; email?: string; phone?: string; mobile?: string; fax?: string; address?: string;
  contacts?: SupplierContactInput[];
}): Promise<void> {
  await apiPost("updateSupplier", {
    Id: Number(id),
    Name: payload.name,
    Email: payload.email ?? "",
    Phone: payload.phone ?? "",
    Mobile: payload.mobile ?? "",
    Fax: payload.fax ?? "",
    Address: payload.address ?? "",
    Contacts: toContactPayload(payload.contacts),
  });
}

/* ---- Monthly account statement ---- */

interface StatementRowData {
  AccountId?: number | string;
  SupplierId?: number | string;
  SupplierName?: string;
  MonthNum?: number;
  RoutesNum?: number;
  CountOfcompleteRounds?: number;
  CountOfHalfGoRounds?: number;
  CountOfHalfReturnRounds?: number;
  TotalDue?: number;
  TotalDeduct?: number;
  TotalPaidNormal?: number;
  TotalPaidadvance?: number;
}

/** GET AccountsAllMonthsForSupplier — monthly account statement rows. */
export async function getMonthlyStatement(
  query: { supplierId?: string; routeId?: string; month?: number; year?: number; pageNo?: number; noOfItems?: number } = {},
): Promise<Paginated<StatementRow>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = await apiGet<StatementRowData[]>("AccountsAllMonthsForSupplier", {
    Year: query.year,
    Month: query.month,
    SupplierId: query.supplierId,
    RouteId: query.routeId,
    PageNo: pageNo,
    NoOfItems: noOfItems,
  });
  const items = (res.Data ?? []).map(
    (r, i): StatementRow => ({
      id: r.AccountId != null ? String(r.AccountId) : `stmt${i}`,
      month: Number(r.MonthNum) || query.month || 0,
      // Backend does not return the year on each row; fall back to the query year.
      year: query.year ?? 0,
      supplierId: r.SupplierId != null ? String(r.SupplierId) : "",
      supplier: r.SupplierName ?? "",
      routesCount: Number(r.RoutesNum) || 0,
      roundsFull: Number(r.CountOfcompleteRounds) || 0,
      roundsHalfGo: Number(r.CountOfHalfGoRounds) || 0,
      roundsHalfReturn: Number(r.CountOfHalfReturnRounds) || 0,
      totalDue: Number(r.TotalDue) || 0,
      totalDeductions: Number(r.TotalDeduct) || 0,
      normalPayments: Number(r.TotalPaidNormal) || 0,
      advancePayments: Number(r.TotalPaidadvance) || 0,
    }),
  );
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** One daily round in the supplier rounds-detail view (TransportationDaysInMonthDate). */
export interface SupplierRound {
  routeName: string;
  serial: string;
  dateOfRound: string;
  checkIn: string;
  checkOut: string;
  oneWay: string;
  price: number;
}

interface SupplierRoundData {
  NameOfRoute?: string;
  Serial?: string | number;
  DateOfRound?: string;
  DateOfCheckIn?: string;
  DateOfCheckOut?: string;
  DateOfOneWay?: string;
  TotalPriceOfDay?: number;
}

/** GET AccountsAllRoundsForSupplier — the per-day round detail for a supplier/month. */
export async function getSupplierRounds(
  query: { supplierId?: string; routeId?: string; month?: number; year?: number; pageNo?: number; noOfItems?: number } = {},
): Promise<Paginated<SupplierRound>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 200;
  const iso = (s?: string) => (s ? s.split("T")[0] : "");
  const res = await apiGet<SupplierRoundData[]>("AccountsAllRoundsForSupplier", {
    Year: query.year,
    Month: query.month,
    SupplierId: query.supplierId,
    RouteId: query.routeId,
    PageNo: pageNo,
    NoOfItems: noOfItems,
  });
  const items = (res.Data ?? []).map((r): SupplierRound => ({
    routeName: r.NameOfRoute ?? "",
    serial: r.Serial != null ? String(r.Serial) : "",
    dateOfRound: iso(r.DateOfRound),
    checkIn: iso(r.DateOfCheckIn),
    checkOut: iso(r.DateOfCheckOut),
    oneWay: iso(r.DateOfOneWay),
    price: Number(r.TotalPriceOfDay) || 0,
  }));
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET AccountsAllMonthsForSupplierExcel — the filtered account statement as an .xlsx Blob. */
export async function downloadStatementExcel(
  query: { supplierId?: string; routeId?: string; month?: number; year?: number } = {},
): Promise<Blob> {
  const res = await apiGet<string>("AccountsAllMonthsForSupplierExcel", {
    Year: query.year,
    Month: query.month,
    SupplierId: query.supplierId,
    RouteId: query.routeId,
  });
  return base64ToBlob(res.Data ?? "");
}

/* ---- Supplier payments ---- */

interface DistributionRow {
  Payment?: string;
  MonthNum?: string;
  YearNum?: string;
}
interface PaymentRow {
  Id?: number | string;
  SupplierId?: number | string;
  SupplierName?: string;
  Payment?: string;
  DatePayment?: string;
  StartDate?: string;
  NumberOfMonths?: string;
  TypeOfDebt?: string;
  DistributionSupplierPayments?: DistributionRow[];
}

/** GET getAllSupplierPayment — payments (with advance distribution) for a supplier. */
export async function getPayments(supplierId?: string): Promise<SupplierPayment[]> {
  const res = await apiGet<PaymentRow[]>("getAllSupplierPayment", { SupplierId: supplierId });
  return (res.Data ?? []).map((p, i): SupplierPayment => {
    const distribution: SupplierPaymentDistribution[] = (p.DistributionSupplierPayments ?? []).map((d) => ({
      month: Number(d.MonthNum) || 0,
      year: Number(d.YearNum) || 0,
      amount: Number(d.Payment) || 0,
    }));
    return {
      // Backend getAllSupplierPayment does not return a payment Id; synthesize one.
      id: p.Id != null ? String(p.Id) : `pay${i}`,
      supplierId: p.SupplierId != null ? String(p.SupplierId) : supplierId ?? "",
      supplier: p.SupplierName ?? "",
      amount: Number(p.Payment) || 0,
      paymentDate: dateOnly(p.DatePayment),
      startDate: p.StartDate ? dateOnly(p.StartDate) : undefined,
      months: p.NumberOfMonths ? Number(p.NumberOfMonths) : undefined,
      type: String(p.TypeOfDebt).toLowerCase() === "advance" ? "advance" : "normal",
      distribution: distribution.length > 0 ? distribution : undefined,
    };
  });
}

/** POST AddSupplierPayment — add a payment (Advance → per-month distribution). */
export async function addPayment(payload: {
  supplierId: string;
  amount: number;
  paymentDate: string;
  type: "advance" | "normal";
  months?: number;
  startDate?: string;
}): Promise<string> {
  const res = await apiPost<unknown>("AddSupplierPayment", {
    SupplierId: Number(payload.supplierId),
    Payment: payload.amount,
    DatePayment: payload.paymentDate || undefined,
    StartDate: payload.startDate || undefined,
    TypeOfDebt: payload.type === "advance" ? "Advance" : "Normal",
    NumberOfMonths: payload.type === "advance" ? payload.months : undefined,
  });
  return String((res as { Id?: string | number }).Id ?? "");
}
