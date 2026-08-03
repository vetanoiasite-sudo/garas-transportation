import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, success, successWrite } from '../../../common/response/base-response';
import { StationInput } from './stations.controller';

function num(v: unknown): number | null {
  if (v === null || v === undefined || v === '') return null;
  const n = Number(v);
  return Number.isFinite(n) ? n : null;
}

@Injectable()
export class StationsService {
  constructor(private readonly prisma: PrismaService) {}

  /** GET GetTransportationDirection — stations for a route (TransportationDirectionsData). */
  async list(routeId: number) {
    if (!routeId) return fail('Err101', 'routeId is required');
    const rows = await this.prisma.transportationVehicleRouteDirection.findMany({
      where: { routeId },
      orderBy: { id: 'asc' },
    });
    const data = rows.map((d) => ({
      Id: String(d.id),
      RouteDirection: d.routeDirection,
      Description: d.description ?? '',
      Latitude: d.latitude != null ? String(d.latitude) : '',
      Longtitud: d.longitude != null ? String(d.longitude) : '', // frozen typo key
      Active: d.active,
    }));
    return success(data);
  }

  /** POST AddTransportationDirection — bulk insert stations under a route. */
  async add(routeId: number, items: StationInput[]) {
    if (!routeId) return fail('Err101', 'TransportationVehicleRouteId is required');
    if (!items.length) return fail('Err101', 'Data is required');
    await this.prisma.transportationVehicleRouteDirection.createMany({
      data: items.map((i) => ({
        routeId,
        routeDirection: i.RouteDirection ?? '',
        description: i.Description ?? null,
        latitude: num(i.Latitude),
        longitude: num(i.Longtitud),
        active: i.Active ?? true,
      })),
    });
    return successWrite(routeId);
  }

  /** POST UpdateTransportationDirection — edit a station. */
  async update(id: number, body: StationInput & { TransportationVehicleRouteId?: number }) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRouteDirection.update({
      where: { id },
      data: {
        routeDirection: body.RouteDirection ?? '',
        description: body.Description ?? null,
        latitude: num(body.Latitude),
        longitude: num(body.Longtitud),
        active: body.Active ?? true,
        ...(body.TransportationVehicleRouteId ? { routeId: Number(body.TransportationVehicleRouteId) } : {}),
      },
    });
    return successWrite(id);
  }

  /** POST DeleteTransportationDirection — delete a station. Blocks with a
   *  friendly message when a passenger is still assigned to it (FK guard). */
  async remove(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const assignedCount = await this.prisma.transportationVehicleRouteEmployee.count({
      where: { directionId: id },
    });
    if (assignedCount > 0) {
      return fail(
        'Err102',
        'لا يمكن حذف المحطة لارتباطها بركاب. انقل الركاب إلى محطة أخرى أولاً.',
      );
    }
    await this.prisma.transportationVehicleRouteDirection.delete({ where: { id } });
    return successWrite(id);
  }
}
