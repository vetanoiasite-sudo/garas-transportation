/* Suppliers, statement & payments service.
   Supplier CRUD lives on the CoreApi's own /Supplier controller (AddNewSupplier,
   GetSuppliersCards, GetSupplierDataResponse, GetSupplierContactPersonsResponse);
   the money side (statement, rounds, payments) is on /api/Transportation.
   Maps the backend's frozen PascalCase keys → the app's Supplier /
   StatementRow / SupplierPayment types. */
import type { Supplier, StatementRow, SupplierPayment, SupplierPaymentDistribution } from "@/lib/types";
import { apiGet, apiPost, apiRaw, currentUserId, type PaginationHeader, type Envelope } from "@/lib/api/client";
import { fileUrl } from "@/lib/download";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

const dateOnly = (s?: string): string => (s ? String(s).slice(0, 10) : "");

/* ---- Suppliers ---- */

/** GetSupplierContactPerson — the CoreApi's contact-person row. */
interface ContactPersonRow {
  ID?: number | string;
  Name?: string;
  Mobile?: string;
  Email?: string;
  Title?: string;
  Active?: boolean;
}
/** SupplierCardData — the list row returned by GetSuppliersCards. */
interface SupplierCardRow {
  Id?: number | string;
  Name?: string;
  Logo?: string;
  CreationDate?: string;
}
/** SupplierData — the full record inside GetSupplierDataResponse. */
interface SupplierDataRow {
  Id?: number | string;
  Name?: string;
  Email?: string;
  Website?: string;
  RegistrationDate?: string;
  Type?: string;
  SupplierSerialCounter?: number;
  Note?: string;
  Rate?: number;
  RegistrationNumber?: string;
  CommercialRecord?: string;
  TaxCard?: string;
  FirstContractDate?: string;
}

// The /Supplier controller returns its own response shapes (SuppliersList /
// SuppliersData / SupplierContactPersonData) instead of the Data envelope.
interface SuppliersCardsResponse { SuppliersList?: SupplierCardRow[]; PaginationHeader?: PaginationHeader }
interface GetSupplierDataResponse {
  SuppliersData?: SupplierDataRow;
  SupplierContactPersonData?: ContactPersonRow[];
  SupplierMobileData?: { ID?: number; Mobile?: string }[];
  SupplierLandLineData?: { ID?: number; LandLine?: string }[];
  SupplierFaxData?: { ID?: number; Fax?: string }[];
  SupplierAddressData?: { Address?: string }[];
}
interface ContactPersonsResponse { SupplierContactPersonData?: ContactPersonRow[] }

const toContact = (c: ContactPersonRow) => ({ id: String(c.ID ?? ""), name: c.Name ?? "", mobile: c.Mobile ?? "" });

function cardToSupplier(s: SupplierCardRow): Supplier {
  return {
    id: String(s.Id ?? ""),
    name: s.Name ?? "",
    createdAt: dateOnly(s.CreationDate),
    activeRoutes: 0,
    contacts: [],
  };
}

/** GET /Supplier/GetSuppliersCards — paginated supplier cards (filters ride in headers). */
export async function getSuppliers(
  query: { pageNo?: number; noOfItems?: number; name?: string; phone?: string; mobile?: string } = {},
): Promise<Paginated<Supplier>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 100;
  const res = (await apiRaw<unknown>("GET", "/Supplier/GetSuppliersCards", {
    headers: {
      CurrentPage: pageNo,
      NumberOfItemsPerPage: noOfItems,
      // Percent-encode so Arabic survives the HTTP header hop (backend decodes it) —
      // a raw non-ASCII header value throws a ByteString error in fetch.
      SupplierName: query.name ? encodeURIComponent(query.name) : undefined,
      Phone: query.phone,
      Mobile: query.mobile,
    },
  })) as Envelope<unknown> & SuppliersCardsResponse;
  const items = (res.SuppliersList ?? []).map(cardToSupplier);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET /Supplier/GetSupplierContactPersonsResponse — a supplier's drivers/contacts. */
export async function getSupplierContacts(supplierId: string): Promise<{ id: string; name: string; mobile: string }[]> {
  const res = (await apiRaw<unknown>("GET", "/Supplier/GetSupplierContactPersonsResponse", {
    headers: { SupplierId: supplierId },
  })) as Envelope<unknown> & ContactPersonsResponse;
  return (res.SupplierContactPersonData ?? []).map(toContact);
}

/** A contact person as edited in the form (new ones have a temporary "c…" id). */
export interface SupplierContactInput {
  id?: string;
  name: string;
  mobile?: string;
}

interface SupplierFormPayload {
  name: string;
  email?: string;
  phone?: string;
  mobile?: string;
  fax?: string;
  address?: string;
  contacts?: SupplierContactInput[];
}

/** Build the CoreApi SupplierData body. `Id` set → AddNewSupplier updates in place.
 *
 *  AddNewSupplier validates more than the form collects:
 *   - `Type` and `CreatedBy` are mandatory; CreatedBy is the AES-ENCRYPTED user id
 *     (the service does Decrypt(CreatedBy) → long), which login returns as UserID.
 *   - `SupplierSpecialities` must be present: the land-line branch reads
 *     `Request.SupplierSpecialities.Count` without a null check → NullReference.
 *   - Address rows need CountryID/GovernorateID/AreaID, so a bare address is not
 *     sent at all (the field is optional in this UI).
 *   - Nested phone rows must echo their `ID` on edit, otherwise the duplicate
 *     pre-check rejects the supplier's own number ("Line: 1") and, past that,
 *     inserts a second row.
 *   - `prev` carries the columns the update path overwrites unconditionally, so
 *     editing from this screen doesn't erase notes/rating/serial/etc. */
function supplierBody(payload: SupplierFormPayload, id?: string, prev?: SupplierExtras) {
  return {
    Id: id ? Number(id) : undefined,
    Name: payload.name,
    Email: payload.email ?? "",
    Type: prev?.type || "Company",
    CreatedBy: currentUserId(),
    SupplierSpecialities: [],
    SupplierMobiles: payload.mobile ? [{ ID: prev?.mobileId, Mobile: payload.mobile }] : [],
    SupplierLandLines: payload.phone ? [{ ID: prev?.landLineId, LandLine: payload.phone }] : [],
    SupplierFaxes: payload.fax ? [{ ID: prev?.faxId, Fax: payload.fax }] : [],
    // Preserved so the unconditional update block doesn't null them out.
    SupplierSerialCounter: prev?.serialCounter,
    Note: prev?.note,
    Rate: prev?.rate,
    Website: prev?.website,
    RegistrationNumber: prev?.registrationNumber,
    CommercialRecord: prev?.commercialRecord,
    TaxCard: prev?.taxCard,
    FirstContractDate: prev?.firstContractDate,
  };
}

/** Fields the edit path must echo back — see supplierBody. */
interface SupplierExtras {
  type?: string;
  serialCounter?: number;
  note?: string;
  rate?: number;
  website?: string;
  registrationNumber?: string;
  commercialRecord?: string;
  taxCard?: string;
  firstContractDate?: string;
  mobileId?: number;
  landLineId?: number;
  faxId?: number;
}

// Only real (numeric) ids identify an existing contact; temp "c…" ids are new.
// Title and Mobile are both mandatory server-side ("Line: 0, Title Is Mandatory"),
// so rows without a mobile are skipped rather than failing the whole save.
function contactPersonBody(supplierId: string, contacts: SupplierContactInput[] = []) {
  return {
    SupplierId: Number(supplierId),
    SupplierContactPersons: contacts
      .filter((c) => c.name.trim() && c.mobile?.trim())
      .map((c) => ({
        ID: c.id && /^\d+$/.test(c.id) ? Number(c.id) : undefined,
        Name: c.name.trim(),
        Title: "Driver",
        Mobile: c.mobile!.trim(),
        Active: true,
      })),
  };
}

/** POST /Supplier/AddNewSupplier — create a supplier (+ its contact persons). */
export async function addSupplier(payload: SupplierFormPayload): Promise<string> {
  const res = await apiRaw<unknown>("POST", "/Supplier/AddNewSupplier", { body: supplierBody(payload) });
  const id = String((res as { ID?: string | number; Id?: string | number }).ID ?? (res as { Id?: string | number }).Id ?? "");
  if (id && payload.contacts?.length) {
    await apiRaw("POST", "/Supplier/AddNewSupplierContactPerson", { body: contactPersonBody(id, payload.contacts) });
  }
  return id;
}

/** GET /Supplier/GetSupplierDataResponse — one supplier + contacts/phones. */
export async function getSupplier(id: string): Promise<Supplier | undefined> {
  const res = (await apiRaw<unknown>("GET", "/Supplier/GetSupplierDataResponse", {
    headers: { SupplierId: id },
  })) as Envelope<unknown> & GetSupplierDataResponse;
  const d = res.SuppliersData;
  if (!d) return undefined;
  return {
    id: String(d.Id ?? id),
    name: d.Name ?? "",
    createdAt: dateOnly(d.RegistrationDate),
    email: d.Email || undefined,
    mobile: res.SupplierMobileData?.[0]?.Mobile || undefined,
    phone: res.SupplierLandLineData?.[0]?.LandLine || undefined,
    fax: res.SupplierFaxData?.[0]?.Fax || undefined,
    address: res.SupplierAddressData?.[0]?.Address || undefined,
    activeRoutes: 0,
    contacts: (res.SupplierContactPersonData ?? []).map(toContact),
  };
}

/** POST /Supplier/AddNewSupplier with an Id — edits in place, then syncs contacts.
 *  The update path overwrites every column and re-keys the phone rows, so the
 *  current record is read first and its untouched fields are echoed back. */
export async function updateSupplier(id: string, payload: SupplierFormPayload): Promise<void> {
  const prev = await getSupplierExtras(id);
  await apiRaw("POST", "/Supplier/AddNewSupplier", { body: supplierBody(payload, id, prev) });
  if (payload.contacts?.length) {
    await apiRaw("POST", "/Supplier/AddNewSupplierContactPerson", { body: contactPersonBody(id, payload.contacts) });
  }
}

/** Read the fields the update path would otherwise wipe (see supplierBody). */
async function getSupplierExtras(id: string): Promise<SupplierExtras> {
  try {
    const res = (await apiRaw<unknown>("GET", "/Supplier/GetSupplierDataResponse", {
      headers: { SupplierId: id },
    })) as Envelope<unknown> & GetSupplierDataResponse;
    const d = (res.SuppliersData ?? {}) as SupplierDataRow;
    return {
      type: d.Type,
      serialCounter: d.SupplierSerialCounter,
      note: d.Note,
      rate: d.Rate,
      website: d.Website,
      registrationNumber: d.RegistrationNumber,
      commercialRecord: d.CommercialRecord,
      taxCard: d.TaxCard,
      firstContractDate: d.FirstContractDate,
      mobileId: res.SupplierMobileData?.[0]?.ID,
      landLineId: res.SupplierLandLineData?.[0]?.ID,
      faxId: res.SupplierFaxData?.[0]?.ID,
    };
  } catch {
    return {};
  }
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
      // AccountId is never assigned server-side (always 0), so a composite
      // key is used instead — otherwise every row shares the same React key.
      id: `${r.SupplierId ?? "s"}-${r.MonthNum ?? i}`,
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
): Promise<string> {
  const res = await apiGet<string>("AccountsAllMonthsForSupplierExcel", {
    Year: query.year,
    Month: query.month,
    SupplierId: query.supplierId,
    RouteId: query.routeId,
  });
  return fileUrl(res.Data);
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
  // This endpoint pages server-side: with no PageNo/NoOfItems the page size is 0,
  // so it answers TotalItems: N but Data: [] — the payment you just added is
  // stored, it simply never comes back.
  const res = await apiGet<PaymentRow[]>("getAllSupplierPayment", {
    SupplierId: supplierId,
    PageNo: 1,
    NoOfItems: 500,
  });
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
