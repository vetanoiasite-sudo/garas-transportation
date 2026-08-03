import { Body, Controller, Get, Headers, Post, UseGuards } from '@nestjs/common';
import { HeaderAuthGuard, Validation } from '../../../common/auth/header-auth.guard';
import { CurrentUser } from '../../../common/auth/current-user.decorator';
import { VehiclesService } from './vehicles.service';

// Base path /api/Transportation (§6.1 — vehicles & vehicle types). Every action
// requires the CompanyName + UserToken headers via the guard. Filters/pagination
// ride in HTTP headers (PageNo/NoOfItems); deletes/approvals are POSTs with an
// Id header — exactly as documented.
@Controller('api/Transportation')
@UseGuards(HeaderAuthGuard)
export class VehiclesController {
  constructor(private readonly service: VehiclesService) {}

  @Get('getAllVehicleType')
  getAllVehicleType() {
    return this.service.getAllVehicleType();
  }

  @Post('AddVehicleType')
  addVehicleType(@Body() body: { Type: string; Active?: boolean }) {
    return this.service.addVehicleType(body?.Type, body?.Active);
  }

  @Post('UpdateVehicleType')
  updateVehicleType(@Body() body: { Id: number; Type: string; Active?: boolean }) {
    return this.service.updateVehicleType(Number(body?.Id), body?.Type, body?.Active);
  }

  @Get('getAllTransportationVehicle')
  getAllVehicles(@Headers('pageno') pageNo?: string, @Headers('noofitems') noOfItems?: string) {
    return this.service.getAllVehicles(Number(pageNo) || 1, Number(noOfItems) || 20);
  }

  @Post('AddTransportationVehicle')
  addVehicle(
    @Body() body: { VehicleTypeId: number; Capacity: number; Active?: boolean },
    @CurrentUser() v: Validation,
  ) {
    return this.service.addVehicle(Number(body?.VehicleTypeId), Number(body?.Capacity), body?.Active, v.userId);
  }

  @Post('UpdateTransportationVehicle')
  updateVehicle(
    @Body() body: { Id: number; VehicleTypeId: number; Capacity: number; Active?: boolean },
    @CurrentUser() v: Validation,
  ) {
    return this.service.updateVehicle(
      Number(body?.Id),
      Number(body?.VehicleTypeId),
      Number(body?.Capacity),
      body?.Active,
      v.userId,
    );
  }

  @Post('DeleteTransportationVehicle')
  removeVehicle(@Headers('id') id?: string) {
    return this.service.removeVehicle(Number(id));
  }

  @Post('ApproveTransportationVehicle')
  approveVehicle(@Headers('id') id: string, @Headers('approve') approve: string, @CurrentUser() v: Validation) {
    return this.service.approveVehicle(Number(id), String(approve).toLowerCase() === 'true', v.userId);
  }
}
