import { Injectable } from '@nestjs/common';
import * as ExcelJS from 'exceljs';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, success } from '../../../common/response/base-response';

@Injectable()
export class ExcelService {
  constructor(private readonly prisma: PrismaService) {}

  /** Build a header-only (.xlsx) template workbook (RTL) and return it base64. */
  private async headerTemplate(sheetName: string, headers: string[]): Promise<string> {
    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet(sheetName);
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = headers.map((h) => ({ header: h, width: 22 }));
    sheet.getRow(1).font = { bold: true };
    const buffer = await workbook.xlsx.writeBuffer();
    return Buffer.from(buffer).toString('base64');
  }

  /** GET downloadExcelUserWithRoutesTemplete — import template (header-only).
   *  Mirrors the add-passenger form: one Name column (not first/middle/last),
   *  plus ID, mobile and the "other identifier" (opaque C/M code), then the
   *  optional route-assignment columns. */
  async downloadUserWithRoutesTemplate() {
    const base64 = await this.headerTemplate('المستخدمون والخطوط', [
      'الاسم',
      'ID',
      'رقم الموبايل',
      'معرّف آخر',
      'سيريال الخط',
      'الفترة',
      'الاتجاه',
    ]);
    return success(base64);
  }

  /** GET downloadExcelUserActiveTemplete — ACTIVATION sheet PRE-FILLED with every
   *  passenger and their CURRENT active status. The admin just edits the "نشط"
   *  column (نعم/لا) and re-uploads (InsertUserNotActiveExcel), which flips the
   *  active flag on the matching passengers (matched by ID). Column order matches
   *  the import: 1 ID · 2 الاسم · 3 نشط. */
  async downloadUserActiveTemplate() {
    const rows = await this.prisma.hrUser.findMany({ orderBy: { id: 'desc' } });
    const fullName = (u: { firstName?: string | null; middleName?: string | null; lastName?: string | null }) =>
      [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('تفعيل الركاب');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'ID', key: 'id', width: 18 },
      { header: 'الاسم', key: 'name', width: 24 },
      { header: 'نشط (نعم/لا)', key: 'active', width: 14 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const u of rows) {
      sheet.addRow({ id: u.identityNumber ?? '', name: fullName(u), active: u.active ? 'نعم' : 'لا' });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /** GET downloadExcelUsersList — EXPORT of the current passengers (not a template).
   *  Columns mirror the add-upload profile fields, so an exported file can be
   *  edited and re-uploaded to add passengers. */
  async usersListExcel() {
    const rows = await this.prisma.hrUser.findMany({
      include: { maritalStatus: true },
      orderBy: { id: 'desc' },
    });
    const fullName = (u: { firstName?: string | null; middleName?: string | null; lastName?: string | null }) =>
      [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('الركاب');
    sheet.views = [{ rightToLeft: true }];
    sheet.columns = [
      { header: 'الاسم', key: 'name', width: 24 },
      { header: 'ID', key: 'id', width: 18 },
      { header: 'رقم الموبايل', key: 'mobile', width: 16 },
      { header: 'معرّف آخر', key: 'other', width: 12 },
      { header: 'نشط', key: 'active', width: 8 },
    ];
    sheet.getRow(1).font = { bold: true };
    for (const u of rows) {
      sheet.addRow({
        name: fullName(u),
        id: u.identityNumber ?? '',
        mobile: u.mobile ?? '',
        other: u.maritalStatus?.name ?? '',
        active: u.active ? 'نعم' : 'لا',
      });
    }
    const buffer = await workbook.xlsx.writeBuffer();
    return success(Buffer.from(buffer).toString('base64'));
  }

  /** Decode a base64 (or data-URI) payload into a loaded ExcelJS workbook. */
  private async loadWorkbook(fileBase64: string): Promise<ExcelJS.Worksheet> {
    // Tolerate a "data:...;base64," prefix if the client sent a data URI.
    const clean = fileBase64.includes(',') ? fileBase64.slice(fileBase64.indexOf(',') + 1) : fileBase64;
    const buffer = Buffer.from(clean, 'base64');
    const workbook = new ExcelJS.Workbook();
    // Sidestep the @types/node Buffer vs exceljs bundled-Buffer type skew.
    await workbook.xlsx.load(buffer as unknown as Parameters<typeof workbook.xlsx.load>[0]);
    const sheet = workbook.worksheets[0];
    if (!sheet) throw new Error('empty');
    return sheet;
  }

  /** Split a single "name" cell into the stored First/Middle/Last parts. */
  private splitName(name: string): { firstName: string; middleName: string | null; lastName: string | null } {
    const parts = name.trim().split(/\s+/).filter(Boolean);
    const firstName = parts.shift() ?? '';
    const lastName = parts.length ? (parts.pop() as string) : null;
    const middleName = parts.length ? parts.join(' ') : null;
    return { firstName, middleName, lastName };
  }

  /** Map an "other identifier" code (e.g. C / M) to its lookup id, or null. */
  private async otherIdentifierMap(): Promise<Map<string, number>> {
    const rows = await this.prisma.maritalStatus.findMany();
    return new Map(rows.map((r) => [r.name.trim().toUpperCase(), r.id]));
  }

  /** Normalise a period cell (Arabic or English) to the stored enum value. */
  private normalizePeriod(raw: string): string {
    const v = raw.trim().toLowerCase();
    if (/^go$|ذهاب/.test(v)) return 'Go';
    if (/^return$|عود/.test(v)) return 'Return';
    return 'Both';
  }

  /**
   * POST InsertUsersWithRoutesExcel — bulk import passengers (HrUser) using the
   * same fields as the add-passenger form, and, when a route serial is present,
   * assign each to that route. Columns (1-based) match the template:
   *   1 name  2 id  3 mobile  4 otherIdentifier(C/M)  5 serial  6 period  7 direction
   */
  async insertUsersWithRoutesExcel(fileBase64?: string) {
    if (!fileBase64) return fail('Err101', 'File is required');

    let sheet: ExcelJS.Worksheet;
    try {
      sheet = await this.loadWorkbook(fileBase64);
    } catch {
      return fail('Err102', 'ملف Excel غير صالح');
    }

    const idMap = await this.otherIdentifierMap();
    let created = 0;
    let assigned = 0;
    const errors: string[] = [];

    for (let i = 2; i <= sheet.rowCount; i++) {
      const row = sheet.getRow(i);
      const cell = (n: number) => String(row.getCell(n).text ?? '').trim();

      const name = cell(1);
      if (!name) continue; // skip blank rows

      const code = cell(4).toUpperCase();
      const user = await this.prisma.hrUser.create({
        data: {
          ...this.splitName(name),
          mobile: cell(3) || null,
          identityNumber: cell(2) || null,
          maritalStatusId: code ? (idMap.get(code) ?? null) : null,
          active: true,
        },
      });
      created++;

      const serial = cell(5);
      if (!serial) continue;

      const route = await this.prisma.transportationVehicleRoute.findFirst({ where: { serial } });
      if (!route) {
        errors.push(`صف ${i}: لا يوجد خط بسيريال ${serial}`);
        continue;
      }

      const directionName = cell(7);
      let directionId: number | null = null;
      if (directionName) {
        const dir = await this.prisma.transportationVehicleRouteDirection.findFirst({
          where: { routeId: route.id, routeDirection: directionName },
        });
        directionId = dir?.id ?? null;
      }

      await this.prisma.transportationVehicleRouteEmployee.create({
        data: {
          routeId: route.id,
          hrUserId: user.id,
          directionId,
          period: this.normalizePeriod(cell(6)),
          active: true,
        },
      });
      assigned++;
    }

    if (created === 0) return fail('Err103', 'لا توجد صفوف صالحة في الملف');
    return success({ Created: created, Assigned: assigned, Errors: errors });
  }

  /** Parse a "نشط" cell → true (active) / false (inactive) / null (unrecognised). */
  private parseActive(raw: string): boolean | null {
    const v = raw.trim().toLowerCase();
    if (!v) return null;
    if (/^(نعم|yes|true|1|نشط|مفعّل|مفعل|active|y)$/.test(v)) return true;
    if (/^(لا|no|false|0|موقوف|غير نشط|غير مفعّل|inactive|n)$/.test(v)) return false;
    return null;
  }

  /**
   * POST InsertUserNotActiveExcel — ACTIVATION: flip the active flag on EXISTING
   * passengers, matched by their ID. Columns: 1 ID  2 name(ignored)  3 نشط(نعم/لا).
   * Does not create anyone — use InsertUsersWithRoutesExcel to add passengers.
   */
  async insertUserNotActiveExcel(fileBase64?: string) {
    if (!fileBase64) return fail('Err101', 'File is required');

    let sheet: ExcelJS.Worksheet;
    try {
      sheet = await this.loadWorkbook(fileBase64);
    } catch {
      return fail('Err102', 'ملف Excel غير صالح');
    }

    let updated = 0;
    const notFound: string[] = [];
    const errors: string[] = [];

    for (let i = 2; i <= sheet.rowCount; i++) {
      const row = sheet.getRow(i);
      const cell = (n: number) => String(row.getCell(n).text ?? '').trim();
      const id = cell(1);
      if (!id) continue; // skip blank rows

      const active = this.parseActive(cell(3));
      if (active === null) {
        errors.push(`صف ${i}: قيمة "نشط" غير مفهومة (استخدم نعم/لا)`);
        continue;
      }

      const res = await this.prisma.hrUser.updateMany({ where: { identityNumber: id }, data: { active } });
      if (res.count > 0) updated += res.count;
      else notFound.push(id);
    }

    if (updated === 0 && notFound.length === 0 && errors.length === 0) {
      return fail('Err103', 'لا توجد صفوف صالحة في الملف');
    }
    return success({ Updated: updated, NotFound: notFound, Errors: errors });
  }
}
