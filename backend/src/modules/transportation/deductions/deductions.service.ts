import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { DeductionBody } from './deductions.controller';

// Rich include so list rows can be flattened to the frozen JSON keys.
const deductionInclude = {
  supplier: true,
  route: { include: { contactPerson: true } },
} as const;

export interface DeductionFilters {
  supplierId?: number;
  routeId?: number;
  fromDate?: string;
  toDate?: string;
  name?: string; // route name search
}

@Injectable()
export class DeductionsService {
  constructor(private readonly prisma: PrismaService) {}

  /** Resolve a set of creator (User) ids → their full names, in one query. */
  private async creatorNames(ids: (number | null | undefined)[]): Promise<Map<number, string>> {
    const unique = [...new Set(ids.filter((id): id is number => !!id))];
    const map = new Map<number, string>();
    if (unique.length === 0) return map;
    const users = await this.prisma.user.findMany({
      where: { id: { in: unique } },
      select: { id: true, firstName: true, middleName: true, lastName: true, email: true },
    });
    for (const u of users) {
      const name = [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ') || u.email;
      map.set(u.id, name);
    }
    return map;
  }

  /** Shared WHERE builder for the list + Excel export (same filters). */
  private buildWhere(filters: DeductionFilters): Record<string, unknown> {
    const where: Record<string, unknown> = {};
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.routeId) where.routeId = filters.routeId;
    if (filters.name) where.route = { is: { nameOfRoute: { contains: filters.name } } };
    if (filters.fromDate || filters.toDate) {
      const range: Record<string, Date> = {};
      if (filters.fromDate) range.gte = new Date(filters.fromDate);
      if (filters.toDate) range.lte = new Date(filters.toDate);
      where.dateOfDeduction = range;
    }
    return where;
  }

  /** GET getAllDeduction — paginated deductions (TransportationDeductionData rows). */
  async getAll(pageNo = 1, noOfItems = 20, filters: DeductionFilters = {}) {
    const where = this.buildWhere(filters);

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRouteDeduction.count({ where }),
      this.prisma.transportationVehicleRouteDeduction.findMany({
        where,
        include: deductionInclude,
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const creators = await this.creatorNames(rows.map((d) => d.creationBy));

    const data = rows.map((d) => ({
      Id: d.id,
      SupplierName: d.supplier?.name ?? '',
      SupplierId: d.supplierId ?? 0,
      TransportationVehicleRouteName: d.route?.nameOfRoute ?? '',
      TransportationVehicleRouteId: d.routeId,
      Serial: d.serial ?? '0',
      Date: d.dateOfDeduction ? d.dateOfDeduction.toISOString() : '',
      CreationDate: d.creationDate ? d.creationDate.toISOString() : '',
      DeductPerRound: d.deductPerRound ?? 0,
      Cause: d.cause ?? '',
      CreationName: creators.get(d.creationBy ?? -1) ?? '', // creator (User) full name
      CreationBy: d.creationBy ?? 0,
      SupplierContentPersonIdName: d.route?.contactPerson?.name ?? '', // driver name (frozen typo key: Content vs Contact)
      FromDate: d.fromDate ? d.fromDate.toISOString() : '',
      ToDate: d.toDate ? d.toDate.toISOString() : '',
      typeOfDeduction: d.typeOfDedct ?? '', // lowercase frozen key
      // Faithful to the old getAllDeduction: routePrice is the route's CURRENT
      // LineCost (dynamic), not a stored deduction column. (old svc:2781)
      routePrice: d.route?.lineCost ?? d.routePrice ?? 0, // lowercase frozen key
    }));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET getAllDeductionExcel — the filtered deductions as a base64 (.xlsx) file. */
  async excel(filters: DeductionFilters = {}) {
    const rows = await this.prisma.transportationVehicleRouteDeduction.findMany({
      where: this.buildWhere(filters),
      include: deductionInclude,
      orderBy: { id: 'desc' },
    });

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('الخصومات');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'الخط', key: 'route', width: 26 },
      { header: 'المورد', key: 'supplier', width: 22 },
      { header: 'السيريال', key: 'serial', width: 14 },
      { header: 'التاريخ', key: 'date', width: 14 },
      { header: 'قيمة الخصم', key: 'amount', width: 14 },
      { header: 'النوع', key: 'type', width: 12 },
      { header: 'السبب', key: 'reason', width: 30 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const d of rows) {
      sheet.addRow({
        route: d.route?.nameOfRoute ?? '',
        supplier: d.supplier?.name ?? '',
        serial: d.serial ?? '',
        date: d.dateOfDeduction ? d.dateOfDeduction.toISOString().slice(0, 10) : '',
        amount: d.deductPerRound ?? 0,
        type: (d.typeOfDedct ?? '').toLowerCase() === 'taxes' ? 'ضرائب' : 'عادي',
        reason: d.cause ?? '',
      });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /** POST AddDeduction — create a deduction. creationBy = caller. */
  async add(body: DeductionBody, userId: number) {
    const routeId = Number(body?.TransportationVehicleRouteId) || 0;
    if (!routeId) return fail('Err101', 'TransportationVehicleRouteId is required');

    const created = await this.prisma.transportationVehicleRouteDeduction.create({
      data: { ...this.toData(body, routeId), creationBy: userId },
    });
    return successWrite(created.id);
  }

  /** POST UpdateDeduction — update a deduction. Preserves the original creator
   *  (old UpdateDeduction never touches CreationBy — svc:7277-7283). */
  async update(id: number, body: DeductionBody) {
    if (!id) return fail('Err101', 'Id is required');
    const routeId = Number(body?.TransportationVehicleRouteId) || 0;
    await this.prisma.transportationVehicleRouteDeduction.update({
      where: { id },
      data: this.toData(body, routeId),
    });
    return successWrite(id);
  }

  /** Maps the documented payload to the deduction model columns (no creator —
   *  the caller adds creationBy only on create). */
  private toData(body: DeductionBody, routeId: number) {
    return {
      supplierId: body.SupplierId ? Number(body.SupplierId) : null,
      routeId,
      serial: body.Serial ?? '',
      dateOfDeduction: body.DateOfDeduction ? new Date(body.DateOfDeduction) : new Date(),
      deductPerRound: Number(body.DeductPerRound) || 0,
      cause: body.Cause ?? null,
      typeOfDedct: body.TypeOfDedct ?? 'Normal',
    };
  }
}
