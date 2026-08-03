import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { DashboardService, AddAttendanceBody } from './dashboard.service';

// Base path /api/Transportation (faithful to the documented URLs, §6.9); every
// action requires the CompanyName + UserToken headers via the guard. Filters and
// pagination ride in HTTP headers (Node lowercases them), bodies are JSON.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class DashboardController {
  constructor(private readonly service: DashboardService) {}

  @Get('DashBoard')
  dashboard(
    @Headers('dateserach') dateSearch?: string,
    @Headers('transportionlineid') lineId?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('routeid') routeId?: string,
  ) {
    return this.service.dashboard(dateSearch, {
      lineId: Number(lineId) || undefined,
      supplierId: Number(supplierId) || undefined,
      serialBus: serialBus || undefined,
      routeId: Number(routeId) || undefined,
    });
  }

  @Get('DashBoardForAttendenceDuration')
  dashboardAttendance(
    @Headers('fromdate') fromDate?: string,
    @Headers('todate') toDate?: string,
    @Headers('transportionlineid') lineId?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('routeid') routeId?: string,
    @Headers('attendaceflag') attendanceFlag?: string,
    @Headers('employeeid') employeeId?: string, // passenger/employee id (fingerprint no.)
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
  ) {
    return this.service.dashboardAttendance(Number(pageNo) || 1, Number(noOfItems) || 20, {
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      lineId: Number(lineId) || undefined,
      supplierId: Number(supplierId) || undefined,
      serialBus: serialBus || undefined,
      routeId: Number(routeId) || undefined,
      attendanceFlag: attendanceFlag || undefined,
      employeeId: employeeId || undefined,
    });
  }

  @Get('AttendanceExcell')
  attendanceExcel(
    @Headers('fromdate') fromDate?: string,
    @Headers('todate') toDate?: string,
    @Headers('transportionlineid') lineId?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('routeid') routeId?: string,
    @Headers('employeeid') employeeId?: string,
  ) {
    return this.service.attendanceExcel({
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      lineId: Number(lineId) || undefined,
      supplierId: Number(supplierId) || undefined,
      serialBus: serialBus || undefined,
      routeId: Number(routeId) || undefined,
      employeeId: employeeId || undefined,
    });
  }

  @Post('AddUsersAttedance')
  addUsersAttendance(@Body() body: AddAttendanceBody, @CurrentUser() v: Validation) {
    return this.service.addUsersAttendance(body, v.userId);
  }

  @Post('AddUsersAttedance44')
  addUsersAttendance44() {
    return this.service.addUsersAttendance44();
  }
}
