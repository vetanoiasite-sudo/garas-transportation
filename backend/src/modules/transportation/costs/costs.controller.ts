import { Controller, Get, Headers, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard } from '../../../common/auth/header-auth.guard';
import { CostsService } from './costs.service';

// Base path /api/Transportation (faithful to the documented URLs, §6.11); every
// action requires the CompanyName + UserToken headers via the guard. Filters and
// pagination ride in HTTP headers (Node lowercases them) — exactly as documented.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class CostsController {
  constructor(private readonly service: CostsService) {}

  @Get('ReportCostOfLines')
  reportCostOfLines(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('transportionlineid') lineId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('datefrom') dateFrom?: string,
    @Headers('dateto') dateTo?: string,
  ) {
    return this.service.reportCostOfLines(Number(pageNo) || 1, Number(noOfItems) || 20, {
      supplierId: Number(supplierId) || undefined,
      lineId: Number(lineId) || undefined,
      serialBus: serialBus || undefined,
      dateFrom: dateFrom || undefined,
      dateTo: dateTo || undefined,
    });
  }

  @Get('ReportCostOfLinesExcell')
  reportCostOfLinesExcel(
    @Headers('supplierid') supplierId?: string,
    @Headers('transportionlineid') lineId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('datefrom') dateFrom?: string,
    @Headers('dateto') dateTo?: string,
  ) {
    return this.service.reportCostOfLinesExcel({
      supplierId: Number(supplierId) || undefined,
      lineId: Number(lineId) || undefined,
      serialBus: serialBus || undefined,
      dateFrom: dateFrom || undefined,
      dateTo: dateTo || undefined,
    });
  }
}
