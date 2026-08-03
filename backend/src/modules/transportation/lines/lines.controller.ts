import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { LinesService } from './lines.service';
import { decodeHeader } from '../../../common/http/header.util';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (PageNo/NoOfItems), and deletes/approvals are POSTs with
// an Id header — exactly as documented.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class LinesController {
  constructor(private readonly service: LinesService) {}

  @Get('getAllTransportationLine')
  getAll(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('name') name?: string,
  ) {
    return this.service.getAll(Number(pageNo) || 1, Number(noOfItems) || 20, decodeHeader(name));
  }

  @Post('AddTransportationLine')
  add(@Body() body: { LineName: string }, @CurrentUser() v: Validation) {
    return this.service.add(body?.LineName, v.userId);
  }

  @Post('UpdateTransportationLine')
  update(@Body() body: { Id: number; LineName: string }, @CurrentUser() v: Validation) {
    return this.service.update(Number(body?.Id), body?.LineName, v.userId);
  }

  @Post('DeleteTransportationLine')
  remove(@Headers('id') id?: string) {
    return this.service.remove(Number(id));
  }

  @Post('ApproveTransportationLine')
  approve(@Headers('id') id: string, @Headers('approve') approve: string, @CurrentUser() v: Validation) {
    return this.service.approve(Number(id), String(approve).toLowerCase() === 'true', v.userId);
  }
}
