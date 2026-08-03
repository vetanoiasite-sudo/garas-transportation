import { Body, Controller, Get, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard } from '../../../common/auth/header-auth.guard';
import { ExcelService } from './excel.service';

// Base path /api/Transportation (faithful to the documented URLs, §6.13); every
// action requires the CompanyName + UserToken headers via the guard.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class ExcelController {
  constructor(private readonly service: ExcelService) {}

  @Get('downloadExcelUserWithRoutesTemplete')
  downloadUserWithRoutesTemplate() {
    return this.service.downloadUserWithRoutesTemplate();
  }

  @Get('downloadExcelUserActiveTemplete')
  downloadUserActiveTemplate() {
    return this.service.downloadUserActiveTemplate();
  }

  /** GET downloadExcelUsersList — export current passengers as .xlsx (base64). */
  @Get('downloadExcelUsersList')
  usersListExcel() {
    return this.service.usersListExcel();
  }

  @Post('InsertUsersWithRoutesExcel')
  insertUsersWithRoutesExcel(@Body() body: { File?: string }) {
    return this.service.insertUsersWithRoutesExcel(body?.File);
  }

  @Post('InsertUserNotActiveExcel')
  insertUserNotActiveExcel(@Body() body: { File?: string }) {
    return this.service.insertUserNotActiveExcel(body?.File);
  }
}
