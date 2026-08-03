import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { SupplierBody, SupplierPaymentBody } from './suppliers.controller';

const MONTH_NAMES = [
  '',
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

// Builds a [start, end) date range for the given year and optional month, for
// filtering DateTime columns; returns undefined when no year is supplied.
function monthRange(year?: number, month?: number): { gte: Date; lt: Date } | undefined {
  if (!year) return undefined;
  if (month && month >= 1 && month <= 12) {
    return { gte: new Date(Date.UTC(year, month - 1, 1)), lt: new Date(Date.UTC(year, month, 1)) };
  }
  return { gte: new Date(Date.UTC(year, 0, 1)), lt: new Date(Date.UTC(year + 1, 0, 1)) };
}

@Injectable()
export class SuppliersService {
  constructor(private readonly prisma: PrismaService) {}

  /** GET getSuppliers — paginated suppliers with their contacts (NOTE: not in API.md, needed by suppliers screen). */
  async getSuppliers(pageNo = 1, noOfItems = 20, filters: { name?: string; phone?: string; mobile?: string } = {}) {
    const where: Record<string, unknown> = { active: true };
    if (filters.name) where.name = { contains: filters.name };
    if (filters.phone) where.phone = { contains: filters.phone };
    if (filters.mobile) where.mobile = { contains: filters.mobile };

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.supplier.count({ where }),
      this.prisma.supplier.findMany({
        where,
        include: { contacts: true, _count: { select: { routes: { where: { active: true } } } } },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const data = rows.map((s) => ({
      Id: s.id,
      Name: s.name,
      Email: s.email ?? '',
      Phone: s.phone ?? '',
      Mobile: s.mobile ?? '',
      Fax: s.fax ?? '',
      Address: s.address ?? '',
      CreationDate: s.creationDate.toISOString(),
      ActiveRoutes: s._count.routes, // count of the supplier's active routes
      contacts: s.contacts.map((c) => ({
        Id: c.id,
        Name: c.name,
        Mobile: c.mobile ?? '',
      })),
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET getSupplier — a single supplier with its contacts. */
  async getSupplier(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const s = await this.prisma.supplier.findUnique({ where: { id }, include: { contacts: true } });
    if (!s) return fail('Err404', 'Supplier not found');
    return success({
      Id: s.id,
      Name: s.name,
      Email: s.email ?? '',
      Phone: s.phone ?? '',
      Mobile: s.mobile ?? '',
      Fax: s.fax ?? '',
      Address: s.address ?? '',
      CreationDate: s.creationDate.toISOString(),
      contacts: s.contacts.map((c) => ({ Id: c.id, Name: c.name, Mobile: c.mobile ?? '' })),
    });
  }

  /** POST updateSupplier — edit a supplier's fields and sync its contact persons. */
  async updateSupplier(id: number, body: SupplierBody) {
    if (!id) return fail('Err101', 'Id is required');
    if (!body?.Name?.trim()) return fail('Err101', 'Name is required');
    await this.prisma.supplier.update({
      where: { id },
      data: {
        name: body.Name,
        email: body.Email ?? null,
        phone: body.Phone ?? null,
        mobile: body.Mobile ?? null,
        fax: body.Fax ?? null,
        address: body.Address ?? null,
      },
    });
    await this.syncContacts(id, body.Contacts ?? []);
    return successWrite(id);
  }

  /** Reconcile a supplier's contact persons against the submitted list:
   *  update those with an Id, create the new ones, and delete the removed ones
   *  (skipping any still referenced by a route, which the FK forbids deleting). */
  private async syncContacts(supplierId: number, incoming: NonNullable<SupplierBody['Contacts']>) {
    const clean = incoming.filter((c) => c.Name?.trim());
    const existing = await this.prisma.supplierContactPerson.findMany({ where: { supplierId } });
    const keptIds = new Set(clean.filter((c) => c.Id).map((c) => Number(c.Id)));

    // Delete contacts the user removed — unless a route still points at them.
    for (const ex of existing) {
      if (keptIds.has(ex.id)) continue;
      const refs = await this.prisma.transportationVehicleRoute.count({ where: { supplierContactPersonId: ex.id } });
      if (refs === 0) await this.prisma.supplierContactPerson.delete({ where: { id: ex.id } });
    }

    // Update existing, create new.
    for (const c of clean) {
      const name = c.Name!.trim();
      const mobile = c.Mobile?.trim() || null;
      if (c.Id && existing.some((e) => e.id === Number(c.Id))) {
        await this.prisma.supplierContactPerson.update({ where: { id: Number(c.Id) }, data: { name, mobile } });
      } else {
        await this.prisma.supplierContactPerson.create({ data: { supplierId, name, mobile } });
      }
    }
  }

  /** POST addSupplier — create a supplier after a duplicate check on name/email/phone/mobile/fax. */
  async addSupplier(body: SupplierBody) {
    if (!body?.Name?.trim()) return fail('Err101', 'Name is required');

    const or: Record<string, unknown>[] = [{ name: body.Name }];
    if (body.Email) or.push({ email: body.Email });
    if (body.Phone) or.push({ phone: body.Phone });
    if (body.Mobile) or.push({ mobile: body.Mobile });
    if (body.Fax) or.push({ fax: body.Fax });

    const existing = await this.prisma.supplier.findFirst({ where: { active: true, OR: or } });
    if (existing) return fail('Err102', 'المورد موجود مسبقاً');

    const contacts = (body.Contacts ?? [])
      .filter((c) => c.Name?.trim())
      .map((c) => ({ name: c.Name!.trim(), mobile: c.Mobile?.trim() || null }));

    const created = await this.prisma.supplier.create({
      data: {
        name: body.Name,
        email: body.Email ?? null,
        phone: body.Phone ?? null,
        mobile: body.Mobile ?? null,
        fax: body.Fax ?? null,
        address: body.Address ?? null,
        contacts: contacts.length ? { create: contacts } : undefined,
      },
    });
    return successWrite(created.id);
  }

  // ── Rounds/pricing helpers (faithful to AccountsAllMonths22 / AccountsAllRounds) ──

  /**
   * A round "belongs to" (month, year) if any of its date columns falls in it.
   * CheckInDate is compared as-is; CheckOutDate/OneWayDate are shifted −16h
   * (legacy shift-boundary correction). (old svc:8664-8667, 8761-8764)
   */
  private roundInPeriod(
    r: { checkInDate: Date | null; checkOutDate: Date | null; oneWayDate: Date | null },
    month?: number,
    year?: number,
  ): boolean {
    if (!month && !year) return true;
    const hit = (d: Date | null, shiftHours: number) => {
      if (!d) return false;
      const s = new Date(d.getTime() - shiftHours * 3_600_000);
      const mOk = !month || s.getUTCMonth() + 1 === month;
      const yOk = !year || s.getUTCFullYear() === year;
      return mOk && yOk;
    };
    return hit(r.checkInDate, 0) || hit(r.checkOutDate, 16) || hit(r.oneWayDate, 16);
  }

  /**
   * Per-leg price for a round on a given date: the latest exception price
   * effective on/before that date, else the route's LineCost — halved for
   * two-way routes (a full two-way trip is two legs). (old CalculatePricePerRouteFromExceptionPrice, svc:9174-9203)
   */
  private priceForRoute(
    routeId: number,
    date: Date | null,
    routeMap: Map<number, { oneWay: boolean; lineCost: number }>,
    exPrices: { routeId: number; price: number; fromDate: Date }[], // sorted fromDate desc
  ): number {
    const route = routeMap.get(routeId);
    const oneWay = route?.oneWay ?? false;
    let base = route?.lineCost ?? 0;
    if (date) {
      const ex = exPrices.find((p) => p.routeId === routeId && p.fromDate.getTime() <= date.getTime());
      if (ex) base = ex.price;
    }
    return oneWay ? base : base / 2;
  }

  /**
   * GET AccountsAllMonthsForSupplier — supplier account statement grouped by
   * supplier+month. The account table only drives which (supplier, month)
   * groups exist; every money figure is recomputed from the rounds, deductions
   * and payments tables — faithful to AccountsAllMonths22 (old svc:8609-8735),
   * which likewise ignores the account's stored counters.
   */
  async accountsAllMonths(
    pageNo = 1,
    noOfItems = 20,
    filters: { year?: number; supplierId?: number; routeId?: number; month?: number } = {},
  ) {
    const where: Record<string, unknown> = {};
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.routeId) where.routeId = filters.routeId;
    const range = monthRange(filters.year, filters.month);
    if (range) where.dateOfMonth = range;

    const accounts = await this.prisma.transportationVehicleRouteAccount.findMany({
      where,
      include: { supplier: true },
      orderBy: { dateOfMonth: 'desc' },
    });

    // Distinct (supplier, year, month) groups; RoutesNum = number of account
    // rows in the group (old g.Count()).
    const groups = new Map<
      string,
      { accountId: number; supplierId: number; supplierName: string; month: number; year: number; routesNum: number }
    >();
    for (const a of accounts) {
      const d = a.dateOfMonth;
      const month = d.getUTCMonth() + 1;
      const year = d.getUTCFullYear();
      const key = `${a.supplierId ?? 0}-${year}-${month}`;
      let g = groups.get(key);
      if (!g) {
        g = { accountId: a.id, supplierId: a.supplierId ?? 0, supplierName: a.supplier?.name ?? '', month, year, routesNum: 0 };
        groups.set(key, g);
      }
      g.routesNum += 1;
    }

    const all = Array.from(groups.values());
    const total = all.length;
    const page = all.slice((pageNo - 1) * noOfItems, (pageNo - 1) * noOfItems + noOfItems);
    const supplierIds = [...new Set(page.map((g) => g.supplierId))];

    // Preload the source tables for the page's suppliers.
    const [rounds, deductions, payments] = await Promise.all([
      this.prisma.transportationRoundForRoute.findMany({ where: { supplierId: { in: supplierIds } }, include: { route: true } }),
      this.prisma.transportationVehicleRouteDeduction.findMany({ where: { supplierId: { in: supplierIds } } }),
      this.prisma.supplierPayment.findMany({ where: { supplierId: { in: supplierIds } }, include: { distribution: true } }),
    ]);
    const routeIds = [...new Set(rounds.map((r) => r.routeId))];
    const exPrices = routeIds.length
      ? await this.prisma.transportionLineExceptionPrice.findMany({ where: { routeId: { in: routeIds } }, orderBy: { fromDate: 'desc' } })
      : [];
    const routeMap = new Map(rounds.map((r) => [r.routeId, { oneWay: r.route?.oneWay ?? false, lineCost: r.route?.lineCost ?? 0 }]));

    const data = page.map((g) => {
      const supRounds = rounds.filter((r) => r.supplierId === g.supplierId && this.roundInPeriod(r, g.month, g.year));
      // A complete round = a two-way route leg pair; half-go / half-return =
      // one-way routes that only go (fromTime) or only return (toTime).
      const complete = supRounds.filter((r) => !(r.route?.oneWay ?? false));
      const halfGo = supRounds.filter((r) => (r.route?.oneWay ?? false) && !!r.route?.fromTime);
      const halfReturn = supRounds.filter((r) => (r.route?.oneWay ?? false) && !!r.route?.toTime);

      const dueComplete = complete.reduce((s, r) => s + this.priceForRoute(r.routeId, r.checkInDate ?? r.checkOutDate, routeMap, exPrices), 0);
      const dueHalfGo = halfGo.reduce((s, r) => s + this.priceForRoute(r.routeId, r.oneWayDate, routeMap, exPrices), 0);
      const dueHalfReturn = halfReturn.reduce((s, r) => s + this.priceForRoute(r.routeId, r.oneWayDate, routeMap, exPrices), 0);
      const totalDue = dueComplete + dueHalfGo + dueHalfReturn;

      const supDeducts = deductions.filter(
        (x) => x.supplierId === g.supplierId && x.dateOfDeduction && x.dateOfDeduction.getUTCMonth() + 1 === g.month && x.dateOfDeduction.getUTCFullYear() === g.year,
      );
      const sumDeduct = (list: typeof supDeducts) => list.reduce((s, x) => s + (x.deductPerRound ?? 0), 0);
      const totalDeduct = sumDeduct(supDeducts);
      const totalNormalDeduct = sumDeduct(supDeducts.filter((x) => (x.typeOfDedct ?? '') === 'Normal'));
      const totalTaxesDeduct = sumDeduct(supDeducts.filter((x) => (x.typeOfDedct ?? '') === 'Taxes'));

      const supPayments = payments.filter((p) => p.supplierId === g.supplierId);
      const totalPaidNormal = supPayments
        .filter((p) => p.typeOfDebt === 'Normal' && p.datePayment.getUTCMonth() + 1 === g.month && p.datePayment.getUTCFullYear() === g.year)
        .reduce((s, p) => s + p.payment, 0);
      const advanceDistributions = supPayments.filter((p) => p.typeOfDebt === 'Advance').flatMap((p) => p.distribution);
      const totalPaidAdvance = advanceDistributions
        .filter((d) => d.monthNum === g.month && d.yearNum === g.year)
        .reduce((s, d) => s + d.payment, 0);
      const advanceOtherMonth = advanceDistributions.some((d) => d.monthNum > g.month);

      const totalDueAfterPaid = totalDue - (totalDeduct + totalPaidNormal + totalPaidAdvance);

      return {
        MonthName: MONTH_NAMES[g.month] ?? '',
        AccountId: g.accountId,
        SupplierId: g.supplierId,
        SupplierName: g.supplierName,
        MonthNum: g.month,
        RoutesNum: g.routesNum,
        CountOfcompleteRounds: Math.floor(complete.length / 2), // two legs per complete round (old /2)
        CountOfHalfGoRounds: halfGo.length,
        CountOfHalfReturnRounds: halfReturn.length,
        TotalDue: totalDue,
        TotalDeduct: totalDeduct,
        TotalTaxesDeduct: totalTaxesDeduct,
        TotalNormalDeduct: totalNormalDeduct,
        TotalPaidadvance: totalPaidAdvance,
        TotalPaidNormal: totalPaidNormal,
        TotalDueAfterPaid: totalDueAfterPaid,
        TotalDuecompleteRound: dueComplete,
        TotalDueHalfGoRound: dueHalfGo,
        TotalDueHalfReturnRound: dueHalfReturn,
        Note: advanceOtherMonth ? 'هناك دفعات مقدمة لهذا المورد في أشهر أخرى، يرجى التحقق من التفاصيل' : '',
      };
    });

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET AccountsAllMonthsForSupplierExcel — the supplier account statement as a
   *  base64 (.xlsx) file (RTL, Arabic headers). Reuses the same aggregation. */
  async accountsAllMonthsExcel(filters: { year?: number; supplierId?: number; routeId?: number; month?: number } = {}) {
    const res = await this.accountsAllMonths(1, 100000, filters);
    const rows = (res as { Data?: Record<string, unknown>[] }).Data ?? [];

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('كشف حساب الموردين');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'الشهر', key: 'month', width: 14 },
      { header: 'المورد', key: 'supplier', width: 24 },
      { header: 'عدد الخطوط', key: 'routes', width: 12 },
      { header: 'الجولات الكاملة', key: 'rounds', width: 14 },
      { header: 'الإجمالي المستحق', key: 'due', width: 16 },
      { header: 'الخصومات', key: 'deduct', width: 14 },
      { header: 'المتبقّي', key: 'remaining', width: 16 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const d of rows) {
      const due = Number(d.TotalDue) || 0;
      const deduct = Number(d.TotalDeduct) || 0;
      sheet.addRow({
        month: d.MonthName,
        supplier: d.SupplierName,
        routes: d.RoutesNum,
        rounds: d.CountOfcompleteRounds,
        due,
        deduct,
        remaining: due - deduct,
      });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /**
   * GET AccountsAllRoundsForSupplier — per-round detail. Filtered on the round's
   * OWN date columns (not creationDate), with RoundsNum and the exception-aware
   * per-leg price computed — faithful to AccountsAllRoundsForSupplier (old svc:8752-8798).
   */
  async accountsAllRounds(
    pageNo = 1,
    noOfItems = 20,
    filters: { year?: number; month?: number; supplierId?: number; routeId?: number } = {},
  ) {
    const where: Record<string, unknown> = {};
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.routeId) where.routeId = filters.routeId;

    // Filter on the round's checkIn/checkOut/oneWay dates (with the −16h shift),
    // not creationDate. Prisma can't express the multi-column OR cleanly, so
    // match in memory then paginate the result.
    const allRows = await this.prisma.transportationRoundForRoute.findMany({
      where,
      include: { route: true },
      orderBy: { id: 'desc' },
    });
    const filtered = allRows.filter((r) => this.roundInPeriod(r, filters.month, filters.year));
    const total = filtered.length;
    const rows = filtered.slice((pageNo - 1) * noOfItems, (pageNo - 1) * noOfItems + noOfItems);

    const routeIds = [...new Set(rows.map((r) => r.routeId))];
    const exPrices = routeIds.length
      ? await this.prisma.transportionLineExceptionPrice.findMany({ where: { routeId: { in: routeIds } }, orderBy: { fromDate: 'desc' } })
      : [];
    const routeMap = new Map(rows.map((r) => [r.routeId, { oneWay: r.route?.oneWay ?? false, lineCost: r.route?.lineCost ?? 0 }]));

    const data = rows.map((r) => {
      const oneWay = r.route?.oneWay ?? false;
      const roundDate = r.checkInDate ?? r.checkOutDate ?? r.oneWayDate;
      // 1 for a completed round (one-way with oneWayDate, or both check-in & out);
      // 0.5 for a single leg; 0 otherwise. (old svc:8792-8794)
      let roundsNum = 0;
      if (oneWay && r.oneWayDate) roundsNum = 1;
      else if (r.checkInDate && r.checkOutDate) roundsNum = 1;
      else if (r.checkInDate || r.checkOutDate) roundsNum = 0.5;

      return {
        NameOfRoute: r.route?.nameOfRoute ?? '',
        Serial: r.serial ?? r.route?.serial ?? '',
        DateOfRound: roundDate ? roundDate.toISOString().slice(0, 10) : '',
        DateOfCheckIn: r.checkInDate ? r.checkInDate.toISOString() : '',
        DateOfCheckOut: r.checkOutDate ? r.checkOutDate.toISOString() : '',
        DateOfOneWay: r.oneWayDate ? r.oneWayDate.toISOString() : '',
        RoundsNum: roundsNum,
        TotalPriceOfDay: this.priceForRoute(r.routeId, roundDate, routeMap, exPrices),
      };
    });

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET getAllSupplierPayment — paginated payments with their monthly distribution (AllSupplierPaymentsData). */
  async getAllSupplierPayment(
    pageNo = 1,
    noOfItems = 20,
    filters: { fromDate?: string; toDate?: string; supplierId?: number } = {},
  ) {
    const where: Record<string, unknown> = {};
    if (filters.supplierId) where.supplierId = filters.supplierId;
    const datePayment: Record<string, Date> = {};
    if (filters.fromDate) datePayment.gte = new Date(filters.fromDate);
    if (filters.toDate) datePayment.lte = new Date(filters.toDate);
    if (Object.keys(datePayment).length > 0) where.datePayment = datePayment;

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.supplierPayment.count({ where }),
      this.prisma.supplierPayment.findMany({
        where,
        include: { supplier: true, distribution: true },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const data = rows.map((p) => ({
      SupplierName: p.supplier?.name ?? '',
      Payment: String(p.payment),
      DatePayment: p.datePayment.toISOString(),
      StartDate: p.startDate ? p.startDate.toISOString() : '',
      NumberOfMonths: p.numberOfMonths != null ? String(p.numberOfMonths) : '',
      TypeOfDebt: p.typeOfDebt,
      DistributionSupplierPayments: p.distribution.map((d) => ({
        Payment: String(d.payment),
        MonthNum: String(d.monthNum),
        YearNum: String(d.yearNum),
      })),
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST AddSupplierPayment — create a payment; Advance debts fan out across NumberOfMonths from StartDate. */
  async addSupplierPayment(body: SupplierPaymentBody) {
    const supplierId = Number(body?.SupplierId) || 0;
    if (!supplierId) return fail('Err101', 'SupplierId is required');
    const payment = Number(body?.Payment) || 0;
    const typeOfDebt = body?.TypeOfDebt ?? 'Normal';
    const datePayment = body?.DatePayment ? new Date(body.DatePayment) : new Date();
    const startDate = body?.StartDate ? new Date(body.StartDate) : null;
    const numberOfMonths = Number(body?.NumberOfMonths) || 0;

    // Advance payments spread evenly across N consecutive months starting at StartDate.
    const distribution: { payment: number; monthNum: number; yearNum: number }[] = [];
    if (typeOfDebt === 'Advance' && numberOfMonths > 0 && startDate) {
      const perMonth = payment / numberOfMonths;
      for (let i = 0; i < numberOfMonths; i++) {
        const d = new Date(Date.UTC(startDate.getUTCFullYear(), startDate.getUTCMonth() + i, 1));
        distribution.push({ payment: perMonth, monthNum: d.getUTCMonth() + 1, yearNum: d.getUTCFullYear() });
      }
    }

    const created = await this.prisma.supplierPayment.create({
      data: {
        supplierId,
        payment,
        datePayment,
        startDate,
        numberOfMonths: numberOfMonths || null,
        typeOfDebt,
        distribution: distribution.length > 0 ? { create: distribution } : undefined,
      },
    });

    return successWrite(created.id);
  }
}
