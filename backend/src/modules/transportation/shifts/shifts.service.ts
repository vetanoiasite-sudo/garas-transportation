import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, success, successWrite } from '../../../common/response/base-response';

interface ShiftInput {
  Id?: number;
  WeekDayId: number;
  From: string;
  To: string;
  ShiftNumber: number;
  BranchId?: number;
  Active?: boolean;
}

@Injectable()
export class ShiftsService {
  constructor(private readonly prisma: PrismaService) {}

  /** GET GetShifts — branch schedule grouped by shiftNumber (opt BranchId header). */
  async getShifts(branchId?: number) {
    const where = branchId ? { active: true, branchId } : { active: true };
    const rows = await this.prisma.branchSchedule.findMany({
      where,
      orderBy: [{ shiftNumber: 'asc' }, { weekDayId: 'asc' }],
    });

    const groups = new Map<number, ReturnType<typeof this.mapShift>[]>();
    for (const r of rows) {
      const list = groups.get(r.shiftNumber) ?? [];
      list.push(this.mapShift(r));
      groups.set(r.shiftNumber, list);
    }

    const data = Array.from(groups.entries()).map(([shiftNumber, shifts]) => ({
      shiftNumber,
      shifts,
    }));
    return success(data);
  }

  private mapShift(r: {
    id: number;
    weekDayId: number;
    from: string;
    to: string;
    branchId: number | null;
    shiftNumber: number;
    creationDate: Date;
    createdBy: number | null;
    modifiedDate: Date;
    modifiedBy: number | null;
  }) {
    return {
      Id: r.id,
      WeekDayId: r.weekDayId,
      From: r.from,
      To: r.to,
      BranchId: r.branchId,
      ShiftNumber: r.shiftNumber,
      CreationDate: r.creationDate,
      CreatedBy: r.createdBy,
      ModifiedDate: r.modifiedDate,
      ModifiedBy: r.modifiedBy,
    };
  }

  /**
   * POST AddListOfShifts — create the active days of a shift group.
   * NOTE: the old API accepted multipart shiftDtos[i].* fields; we accept a JSON
   * body { Shifts: [...] } instead (pragmatic deviation — same data, cleaner wire).
   */
  async addListOfShifts(shifts: ShiftInput[], userId: number) {
    if (!Array.isArray(shifts) || shifts.length === 0) return fail('Err101', 'Shifts is required');
    const activeDays = shifts.filter((s) => s?.Active !== false);
    if (activeDays.length === 0) return fail('Err101', 'At least one active shift day is required');

    await this.prisma.branchSchedule.createMany({
      data: activeDays.map((s) => ({
        shiftNumber: Number(s.ShiftNumber),
        weekDayId: Number(s.WeekDayId),
        from: s.From,
        to: s.To,
        active: true,
        branchId: s.BranchId != null ? Number(s.BranchId) : null,
        createdBy: userId,
        modifiedBy: userId,
      })),
    });
    return successWrite(undefined, 'Shifts created');
  }

  /**
   * POST UpdateShift — upsert the days of a shift group.
   * NOTE: JSON body { Id?, Shifts: [...] } (pragmatic deviation from multipart);
   * each day with an Id is updated, days without an Id are created.
   */
  async updateShift(shifts: ShiftInput[], userId: number) {
    if (!Array.isArray(shifts) || shifts.length === 0) return fail('Err101', 'Shifts is required');

    for (const s of shifts) {
      if (s?.Id) {
        await this.prisma.branchSchedule.update({
          where: { id: Number(s.Id) },
          data: {
            shiftNumber: Number(s.ShiftNumber),
            weekDayId: Number(s.WeekDayId),
            from: s.From,
            to: s.To,
            active: s.Active ?? true,
            branchId: s.BranchId != null ? Number(s.BranchId) : null,
            modifiedBy: userId,
          },
        });
      } else if (s?.Active !== false) {
        await this.prisma.branchSchedule.create({
          data: {
            shiftNumber: Number(s.ShiftNumber),
            weekDayId: Number(s.WeekDayId),
            from: s.From,
            to: s.To,
            active: true,
            branchId: s.BranchId != null ? Number(s.BranchId) : null,
            createdBy: userId,
            modifiedBy: userId,
          },
        });
      }
    }
    return successWrite(undefined, 'Shifts updated');
  }
}
