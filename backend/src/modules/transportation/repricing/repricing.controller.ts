import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { RepricingService } from './repricing.service';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (Node lowercases them), bodies are JSON, and
// approvals/rejections are POSTs with an Id header — exactly as documented (§6.3).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class RepricingController {
  constructor(private readonly service: RepricingService) {}

  @Post('ModifyPriceOfTransportationLine')
  modify(@Body() body: RepriceBody, @CurrentUser() v: Validation) {
    return this.service.modify(body, v.userId);
  }

  @Get('getAllModifyPriceOfTransportationLine')
  getAll(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('approve') approve?: string,
  ) {
    const approveFilter =
      approve === undefined || approve === '' ? undefined : String(approve).toLowerCase() === 'true';
    return this.service.getAll(Number(pageNo) || 1, Number(noOfItems) || 20, approveFilter);
  }

  @Post('UpdatePriceOfTransportationLine')
  approvePrice(@Headers('id') id: string, @CurrentUser() v: Validation) {
    return this.service.approvePrice(Number(id), v.userId);
  }

  @Post('RejectUpdatePrice')
  rejectPrice(@Headers('id') id: string, @CurrentUser() v: Validation) {
    return this.service.rejectPrice(Number(id), v.userId);
  }
}

// Shape of the ModifyPriceOfTransportationLine payload (documented PascalCase keys, §6.3).
export interface RepriceBody {
  IncreaseCost?: number;
  IsPercent?: boolean;
  ForAllLines?: boolean;
  ApproximateToFiveFlag?: boolean;
  TransportationLineIds?: number[]; // ARRAY of ROUTE ids
  StartDate?: string; // ISO-UTC
}
