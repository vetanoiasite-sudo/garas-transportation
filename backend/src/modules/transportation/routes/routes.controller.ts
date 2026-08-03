import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { RoutesService } from './routes.service';
import { decodeHeader } from '../../../common/http/header.util';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (Node lowercases them), bodies are JSON, and
// deletes/approvals are POSTs with an Id header — exactly as documented (§6.4).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class RoutesController {
  constructor(private readonly service: RoutesService) {}

  @Get('getAllTransportationRoute')
  getAll(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('ptransportionlineid') lineId?: string,
    @Headers('supplierid') supplierId?: string,
    @Headers('serialbus') serialBus?: string,
    @Headers('name') name?: string,
  ) {
    return this.service.getAll(Number(pageNo) || 1, Number(noOfItems) || 20, {
      lineId: Number(lineId) || undefined,
      supplierId: Number(supplierId) || undefined,
      serialBus: serialBus || undefined,
      name: decodeHeader(name),
    });
  }

  @Get('getTransportationRoute')
  getOne(@Headers('routeid') routeId?: string) {
    return this.service.getOne(Number(routeId));
  }

  @Get('getTransportationRouteByHrUser')
  getByHrUser(@Headers('hruserid') hrUserId?: string) {
    return this.service.getByHrUser(Number(hrUserId));
  }

  @Get('RoutesWithUsersExcel')
  routesWithUsersExcel() {
    return this.service.routesWithUsersExcel();
  }

  @Post('AddTransportationRoute')
  add(@Body() body: RouteBody, @CurrentUser() v: Validation) {
    return this.service.add(body, v.userId);
  }

  @Post('UpdateTransportationRoute')
  update(@Body() body: RouteBody & { Id: number }, @CurrentUser() v: Validation) {
    return this.service.update(Number(body?.Id), body, v.userId);
  }

  @Post('DeleteTransportationRoute')
  remove(@Headers('id') id?: string) {
    return this.service.remove(Number(id));
  }

  @Post('ApproveRoute')
  approve(@Headers('id') id: string, @Headers('approve') approve: string, @CurrentUser() v: Validation) {
    return this.service.approve(Number(id), String(approve).toLowerCase() === 'true', v.userId);
  }
}

// Shape of the Add/Update route payload (documented PascalCase keys, §6.4).
export interface RouteBody {
  SupplierId?: number;
  SupplierContactPersonId?: number;
  BranchScheduleId?: number;
  TransportationLineId?: number;
  HrUserId?: number; // → supervisorId
  PeriodFrom?: string; // ISO-UTC
  Active?: boolean;
  FromDate?: string; // go time of day
  ToDate?: string; // return time of day
  NameOfRoute?: string;
  LineCost?: number;
  OneWay?: boolean;
  TransportationVehicleId?: number;
}
