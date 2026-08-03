import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { DeductionsService } from './deductions.service';
import { decodeHeader } from '../../../common/http/header.util';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (Node lowercases them) and bodies are JSON — exactly as
// documented (§6.10).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class DeductionsController {
  constructor(private readonly service: DeductionsService) {}

  @Get('getAllDeduction')
  getAll(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('routeid') routeId?: string,
    @Headers('fromdate') fromDate?: string,
    @Headers('todate') toDate?: string,
    @Headers('name') name?: string,
  ) {
    return this.service.getAll(Number(pageNo) || 1, Number(noOfItems) || 20, {
      supplierId: Number(supplierId) || undefined,
      routeId: Number(routeId) || undefined,
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      name: decodeHeader(name),
    });
  }

  /** GET getAllDeductionExcel — the filtered deductions as a base64 .xlsx. */
  @Get('getAllDeductionExcel')
  excel(
    @Headers('supplierid') supplierId?: string,
    @Headers('routeid') routeId?: string,
    @Headers('fromdate') fromDate?: string,
    @Headers('todate') toDate?: string,
    @Headers('name') name?: string,
  ) {
    return this.service.excel({
      supplierId: Number(supplierId) || undefined,
      routeId: Number(routeId) || undefined,
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      name: decodeHeader(name),
    });
  }

  @Post('AddDeduction')
  add(@Body() body: DeductionBody, @CurrentUser() v: Validation) {
    return this.service.add(body, v.userId);
  }

  @Post('UpdateDeduction')
  update(@Body() body: DeductionBody & { Id: number }) {
    return this.service.update(Number(body?.Id), body);
  }
}

// Shape of the Add/Update deduction payload (documented PascalCase keys, §6.10).
export interface DeductionBody {
  SupplierId?: number;
  Serial?: string;
  TransportationVehicleRouteId?: number;
  DeductPerRound?: number;
  Cause?: string;
  DateOfDeduction?: string; // ISO
  TypeOfDedct?: string; // Taxes | Normal
}
