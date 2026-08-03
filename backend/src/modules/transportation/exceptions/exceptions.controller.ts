import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { ExceptionsService } from './exceptions.service';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (Node lowercases them) and bodies are JSON — exactly as
// documented (§6.8).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class ExceptionsController {
  constructor(private readonly service: ExceptionsService) {}

  @Get('GetEmployeeException')
  getAll(
    @Headers('hruserid') hrUserId?: string,
    @Headers('id') id?: string,
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
  ) {
    return this.service.getAll(Number(pageNo) || 1, Number(noOfItems) || 20, {
      hrUserId: Number(hrUserId) || undefined,
      id: Number(id) || undefined,
    });
  }

  @Post('AddException')
  add(@Body() body: ExceptionBody, @CurrentUser() v: Validation) {
    return this.service.add(body, v.userId);
  }

  @Post('UpdateException')
  update(@Body() body: ExceptionBody & { id: number }, @CurrentUser() v: Validation) {
    return this.service.update(Number(body?.id), body, v.userId);
  }

  @Get('GetCapacityNumbers')
  getCapacity(
    @Headers('routeid') routeId?: string,
    @Headers('period') period?: string,
    @Headers('dateserach') dateSearch?: string,
  ) {
    return this.service.getCapacity(Number(routeId), period || undefined, dateSearch || undefined);
  }
}

// Shape of the Add/Update exception payload (documented keys, §6.8). Period-mode
// carries FromDate/ToDate/DayName or ExceptionDate; location-mode carries
// Latitude/Longtitud or TransportationVehicleRouteDirectionId.
export interface ExceptionBody {
  transportationVehicleRouteId?: number;
  hrUserId?: number;
  Active?: boolean;
  reasonException?: string;
  contactNumber?: string;
  Period?: string; // Go | Return | Both
  FromDate?: string; // ISO
  ToDate?: string; // ISO
  DayName?: string;
  ExceptionDate?: string; // ISO
  Latitude?: number;
  Longtitud?: number; // frozen typo key
  TransportationVehicleRouteDirectionId?: number;
}
