import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { paginationHeader, success, successList } from '../../../common/response/base-response';

// Filters accepted by ReportCostOfLines (documented header filters, §6.11).
interface CostFilters {
  supplierId?: number;
  lineId?: number;
  serialBus?: string;
  dateFrom?: string;
  dateTo?: string;
}

export interface CostRow {
  LineName: string;
  Serial: string;
  NameOfRoute: string;
  SupplierName: string;
  LineCost: number;
  OneWay: boolean;
  CountOfround: number; // frozen odd-casing key
  DeductPerRound: number;
  DeductRoundNum: number;
  NetCost: number;
}

@Injectable()
export class CostsService {
  constructor(private readonly prisma: PrismaService) {}

  /** Build an inclusive [dateFrom, dateTo] day-range Prisma filter, or `undefined` (no bound). */
  private roundDateFilter(dateFrom?: string, dateTo?: string): Record<string, Date> | undefined {
    const bounds: Record<string, Date> = {};
    if (dateFrom) {
      const f = new Date(dateFrom);
      if (!isNaN(f.getTime())) bounds.gte = new Date(f.getFullYear(), f.getMonth(), f.getDate());
    }
    if (dateTo) {
      const t = new Date(dateTo);
      if (!isNaN(t.getTime())) {
        const lt = new Date(t.getFullYear(), t.getMonth(), t.getDate());
        lt.setDate(lt.getDate() + 1); // inclusive of the To calendar day
        bounds.lt = lt;
      }
    }
    return Object.keys(bounds).length > 0 ? bounds : undefined;
  }

  /**
   * Compute one TransportationReportCostData row for a route (§7.5 Cost report):
   *   two-way: rounds = checkIns + checkOuts ; NetCost = (LineCost/2)*rounds - deductions
   *   one-way: rounds = checkIns             ; NetCost =  LineCost   *rounds - deductions
   */
  private async costRowForRoute(
    r: { id: number; serial: string; nameOfRoute: string; lineCost: number; oneWay: boolean | null; line?: { lineName: string } | null; supplier?: { name: string } | null },
    dateCond: Record<string, Date> | undefined,
  ): Promise<CostRow> {
    const oneWay = r.oneWay ?? false;
    // Nullable round-date columns: with no date bound, still require the column present.
    const checkInCond = dateCond ?? { not: null };
    const checkOutCond = dateCond ?? { not: null };

    const [checkIns, checkOuts, deductions] = await Promise.all([
      this.prisma.transportationRoundForRoute.count({ where: { routeId: r.id, checkInDate: checkInCond } }),
      oneWay
        ? Promise.resolve(0)
        : this.prisma.transportationRoundForRoute.count({ where: { routeId: r.id, checkOutDate: checkOutCond } }),
      this.prisma.transportationVehicleRouteDeduction.findMany({
        where: { routeId: r.id, dateOfDeduction: dateCond },
        orderBy: { dateOfDeduction: 'desc' },
        select: { deductPerRound: true },
      }),
    ]);

    const countOfRound = oneWay ? checkIns : checkIns + checkOuts;
    const deductRoundNum = deductions.length;
    // Representative per-round deduction = the most recent row's value.
    const deductPerRound = deductRoundNum > 0 ? deductions[0].deductPerRound : 0;
    // Total actually deducted = sum of each round's deduction.
    const deductionsTotal = deductions.reduce((s, d) => s + d.deductPerRound, 0);
    const netCost = oneWay
      ? r.lineCost * countOfRound - deductionsTotal
      : (r.lineCost / 2) * countOfRound - deductionsTotal;

    return {
      LineName: r.line?.lineName ?? '',
      Serial: r.serial ?? '',
      NameOfRoute: r.nameOfRoute ?? '',
      SupplierName: r.supplier?.name ?? '',
      LineCost: r.lineCost,
      OneWay: oneWay,
      CountOfround: countOfRound,
      DeductPerRound: deductPerRound,
      DeductRoundNum: deductRoundNum,
      NetCost: netCost,
    };
  }

  /**
   * GET ReportCostOfLines — paginated TransportationReportCostData rows built
   * from routes (joined to line & supplier), with real round/deduction math (§7.5).
   */
  async reportCostOfLines(pageNo = 1, noOfItems = 20, filters: CostFilters = {}) {
    const where: Record<string, unknown> = { active: true };
    if (filters.lineId) where.transportationLineId = filters.lineId;
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.serialBus) where.serial = filters.serialBus;

    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicleRoute.count({ where }),
      this.prisma.transportationVehicleRoute.findMany({
        where,
        include: { line: true, supplier: true },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const dateCond = this.roundDateFilter(filters.dateFrom, filters.dateTo);
    const data = await Promise.all(rows.map((r) => this.costRowForRoute(r, dateCond)));

    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET ReportCostOfLinesExcell — base64 (.xlsx) costs export (RTL, Arabic headers). */
  async reportCostOfLinesExcel(filters: CostFilters = {}) {
    const where: Record<string, unknown> = { active: true };
    if (filters.lineId) where.transportationLineId = filters.lineId;
    if (filters.supplierId) where.supplierId = filters.supplierId;
    if (filters.serialBus) where.serial = filters.serialBus;

    const rows = await this.prisma.transportationVehicleRoute.findMany({
      where,
      include: { line: true, supplier: true },
      orderBy: { id: 'desc' },
    });
    const dateCond = this.roundDateFilter(filters.dateFrom, filters.dateTo);
    const data = await Promise.all(rows.map((r) => this.costRowForRoute(r, dateCond)));

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('تكلفة الخطوط');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'اسم الخط', key: 'line', width: 22 },
      { header: 'الرقم المسلسل', key: 'serial', width: 14 },
      { header: 'خط السير', key: 'route', width: 22 },
      { header: 'المورد', key: 'supplier', width: 22 },
      { header: 'تكلفة الخط', key: 'cost', width: 14 },
      { header: 'اتجاه واحد', key: 'oneWay', width: 12 },
      { header: 'عدد الجولات', key: 'rounds', width: 14 },
      { header: 'الخصم لكل جولة', key: 'deductPer', width: 16 },
      { header: 'عدد جولات الخصم', key: 'deductNum', width: 16 },
      { header: 'صافي التكلفة', key: 'net', width: 16 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const d of data) {
      sheet.addRow({
        line: d.LineName,
        serial: d.Serial,
        route: d.NameOfRoute,
        supplier: d.SupplierName,
        cost: d.LineCost,
        oneWay: d.OneWay ? 'نعم' : 'لا',
        rounds: d.CountOfround,
        deductPer: d.DeductPerRound,
        deductNum: d.DeductRoundNum,
        net: d.NetCost,
      });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }
}
