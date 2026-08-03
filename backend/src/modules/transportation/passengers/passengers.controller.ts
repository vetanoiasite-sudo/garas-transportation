import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { PassengersService } from './passengers.service';
import { decodeHeader } from '../../../common/http/header.util';

// Base path /api/Transportation (faithful to the documented URLs); every action
// requires the CompanyName + UserToken headers via the guard. Filters ride in
// HTTP headers (all lowercase) and deletes are POSTs with an Id header — exactly
// as documented in API.md §6.6 (Passengers / route employees).
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class PassengersController {
  constructor(private readonly service: PassengersService) {}

  /** GET getAllHrUsers — paginated passenger (HrUser) profiles. Closes the
   *  frontend's mock fallback for the passengers list. Not in API.md; added. */
  @Get('getAllHrUsers')
  getAllHrUsers(
    @Headers('pageno') pageNo?: string,
    @Headers('noofitems') noOfItems?: string,
    @Headers('name') name?: string,
    @Headers('active') active?: string,
  ) {
    // active header: "true"/"false" filters; absent → all (active + inactive).
    const activeFilter = active === undefined || active === '' ? undefined : active.toLowerCase() === 'true';
    return this.service.getAllHrUsers(Number(pageNo) || 1, Number(noOfItems) || 20, decodeHeader(name), activeFilter);
  }

  /** GET getHrUser — a single passenger profile (Id header). */
  @Get('getHrUser')
  getHrUser(@Headers('id') id?: string) {
    return this.service.getHrUser(Number(id));
  }

  /** GET GetMaritalStatus — lookup options for the "Other Identifier" dropdown. */
  @Get('GetMaritalStatus')
  getMaritalStatus() {
    return this.service.getMaritalStatus();
  }

  /** POST AddHrUser — create a passenger profile. */
  @Post('AddHrUser')
  addHrUser(@Body() body: HrUserBody, @CurrentUser() _v: Validation) {
    return this.service.addHrUser(body);
  }

  /** POST UpdateHrUser — edit a passenger profile. */
  @Post('UpdateHrUser')
  updateHrUser(@Body() body: HrUserBody & { Id: number }) {
    return this.service.updateHrUser(Number(body?.Id), body);
  }

  /** GET GetTransportationEmployeeList — passengers assigned to a route (RouteUsersModel). */
  @Get('GetTransportationEmployeeList')
  getEmployeeList(@Headers('routeid') routeId?: string) {
    return this.service.getEmployeeList(Number(routeId));
  }

  /** GET GetTransportationEmployees — "who rides today" for the mobile app (RouteUsersForMobModel). */
  @Get('GetTransportationEmployees')
  getEmployeesForMob(
    @Headers('routeid') routeId?: string,
    @Headers('period') period?: string,
    @Headers('dateserach') dateSearch?: string,
  ) {
    return this.service.getEmployeesForMob(Number(routeId), period, dateSearch);
  }

  /** POST AddTransportationEmployee — bulk insert memberships (TransportationVehicleRouteId header). */
  @Post('AddTransportationEmployee')
  addEmployee(
    @Headers('transportationvehiclerouteid') routeId: string,
    @Body()
    body: {
      Data?: Array<{
        hrUserId: number;
        TransportationVehicleRouteDirectionId?: number;
        Active?: boolean;
        period?: string;
      }>;
    },
  ) {
    return this.service.addEmployee(Number(routeId), body?.Data ?? []);
  }

  /** POST UpdateTransportationEmployee — update a single membership. */
  @Post('UpdateTransportationEmployee')
  updateEmployee(
    @Body()
    body: {
      Id: number;
      hrUserId: number;
      TransportationVehicleRouteId: number;
      TransportationVehicleRouteDirectionId?: number;
      Active?: boolean;
    },
  ) {
    return this.service.updateEmployee(body);
  }

  /** POST DeleteTransportationEmployee — delete a membership (Id header). */
  @Post('DeleteTransportationEmployee')
  deleteEmployee(@Headers('id') id?: string) {
    return this.service.deleteEmployee(Number(id));
  }

  /** GET getTransportationRoutesForHrUser — all route memberships for one hr user. */
  @Get('getTransportationRoutesForHrUser')
  getRoutesForHrUser(@Headers('hruserid') hrUserId?: string) {
    return this.service.getRoutesForHrUser(Number(hrUserId));
  }

  /** POST AddRoutesForEmployee — bulk create memberships for one hr user (hrUserId header). */
  @Post('AddRoutesForEmployee')
  addRoutesForEmployee(
    @Headers('hruserid') hrUserId: string,
    @Body()
    body: {
      Data?: Array<{
        RouteId: number;
        Active?: boolean;
        FromDate?: string;
        ToDate?: string;
        Period?: string;
        DurationLatitude?: number;
        DurationLongtitud?: number;
        TransportationVehicleRouteDirectionId?: number;
      }>;
    },
  ) {
    return this.service.addRoutesForEmployee(Number(hrUserId), body?.Data ?? []);
  }

  /** POST UpdateRouteEmployee — update a single membership from the passenger side. */
  @Post('UpdateRouteEmployee')
  updateRouteEmployee(
    @Body()
    body: {
      Id: number;
      TransportationVehicleRouteId: number;
      HrUserId: number;
      Active?: boolean;
      TransportationVehicleRouteDirectionId?: number;
      FromDate?: string;
      ToDate?: string;
      Period?: string;
      DurationLatitude?: number;
      DurationLongtitud?: number;
    },
  ) {
    return this.service.updateRouteEmployee(body);
  }

  /** POST DeleteRouteEmployee — delete a membership (Id header). */
  @Post('DeleteRouteEmployee')
  deleteRouteEmployee(@Headers('id') id?: string) {
    return this.service.deleteRouteEmployee(Number(id));
  }
}

// Passenger (HrUser) profile payload.
export interface HrUserBody {
  FirstName?: string;
  MiddleName?: string;
  LastName?: string;
  Mobile?: string;
  IdentityNumber?: string;
  MaritalStatusId?: number;
  ImgPath?: string;
  Latitude?: number;
  Longitude?: number;
  Active?: boolean;
  Email?: string;
}
