import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard } from '../../../common/auth/header-auth.guard';
import { StationsService } from './stations.service';

// Stations (a.k.a. Directions) — §6.5. Same base path & conventions as routes.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class StationsController {
  constructor(private readonly service: StationsService) {}

  @Get('GetTransportationDirection')
  list(@Headers('routeid') routeId?: string) {
    return this.service.list(Number(routeId));
  }

  @Post('AddTransportationDirection')
  add(@Headers('transportationvehiclerouteid') routeId: string, @Body() body: { Data?: StationInput[] }) {
    return this.service.add(Number(routeId), body?.Data ?? []);
  }

  @Post('UpdateTransportationDirection')
  update(@Body() body: StationInput & { Id: number; TransportationVehicleRouteId?: number }) {
    return this.service.update(Number(body?.Id), body);
  }

  @Post('DeleteTransportationDirection')
  remove(@Headers('id') id?: string) {
    return this.service.remove(Number(id));
  }
}

export interface StationInput {
  RouteDirection?: string;
  Description?: string;
  Latitude?: number | string;
  Longtitud?: number | string; // frozen typo key
  Active?: boolean;
}
