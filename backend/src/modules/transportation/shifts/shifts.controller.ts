import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { ShiftsService } from './shifts.service';

// Base path /HR/BranchSchedule (§6.7 — shifts live under the HR prefix, NOT
// /api/Transportation). Every action requires the CompanyName + UserToken
// headers via the guard.
@Controller('HR/BranchSchedule')
@UseGuards(HeaderAuthGuard)
export class ShiftsController {
  constructor(private readonly service: ShiftsService) {}

  @Get('GetShifts')
  getShifts(@Headers('branchid') branchId?: string) {
    return this.service.getShifts(branchId ? Number(branchId) : undefined);
  }

  // NOTE: old API used multipart shiftDtos[i].* fields; we accept JSON { Shifts: [...] }.
  @Post('AddListOfShifts')
  addListOfShifts(
    @Body()
    body: {
      Shifts: { WeekDayId: number; From: string; To: string; ShiftNumber: number; BranchId?: number; Active?: boolean }[];
    },
    @CurrentUser() v: Validation,
  ) {
    return this.service.addListOfShifts(body?.Shifts, v.userId);
  }

  // NOTE: old API used multipart (leading Id + shiftDtos[i].*); we accept JSON { Id?, Shifts: [...] }.
  @Post('UpdateShift')
  updateShift(
    @Body()
    body: {
      Id?: number;
      Shifts: {
        Id?: number;
        WeekDayId: number;
        From: string;
        To: string;
        ShiftNumber: number;
        BranchId?: number;
        Active?: boolean;
      }[];
    },
    @CurrentUser() v: Validation,
  ) {
    return this.service.updateShift(body?.Shifts, v.userId);
  }
}
