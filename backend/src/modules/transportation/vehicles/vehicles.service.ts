import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { NotificationsService } from '../notifications/notifications.service';

@Injectable()
export class VehiclesService {
  constructor(
    private readonly prisma: PrismaService,
    private readonly notifications: NotificationsService,
  ) {}

  /** GET getAllVehicleType — vehicle-type lookup ({ Id, Type, Active, TransportationVehicles: [] }). */
  async getAllVehicleType() {
    const rows = await this.prisma.vehicleType.findMany({ orderBy: { id: 'desc' } });
    const data = rows.map((t) => ({
      Id: t.id,
      Type: t.type,
      Active: t.active,
      TransportationVehicles: [] as unknown[],
    }));
    return success(data);
  }

  /** POST AddVehicleType — create a vehicle type. */
  async addVehicleType(type: string, active?: boolean) {
    if (!type?.trim()) return fail('Err101', 'Type is required');
    const created = await this.prisma.vehicleType.create({
      data: { type, active: active ?? true },
    });
    return successWrite(created.id);
  }

  /** POST UpdateVehicleType — rename / toggle a vehicle type. */
  async updateVehicleType(id: number, type: string, active?: boolean) {
    if (!id) return fail('Err101', 'Id is required');
    if (!type?.trim()) return fail('Err101', 'Type is required');
    await this.prisma.vehicleType.update({
      where: { id },
      data: { type, active: active ?? true },
    });
    return successWrite(id);
  }

  /** GET getAllTransportationVehicle — paginated vehicles with joined type name. */
  async getAllVehicles(pageNo = 1, noOfItems = 20) {
    const where = { active: true };
    const [total, rows] = await this.prisma.$transaction([
      this.prisma.transportationVehicle.count({ where }),
      this.prisma.transportationVehicle.findMany({
        where,
        include: { vehicleType: true },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);
    const data = rows.map((v) => ({
      Id: v.id,
      VehicleTypeId: v.vehicleTypeId,
      Capacity: v.capacity,
      IsApproved: v.isApproved,
      ApprovedBy: v.approvedBy ?? '',
      Active: v.active,
      CreationDate: v.creationDate,
      VehicleTypeName: v.vehicleType?.type ?? '',
      TransportationLines: [] as unknown[],
    }));
    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST AddTransportationVehicle — create (IsApproved defaults false). */
  async addVehicle(vehicleTypeId: number, capacity: number, active: boolean | undefined, userId: number) {
    if (!vehicleTypeId) return fail('Err101', 'VehicleTypeId is required');
    const created = await this.prisma.transportationVehicle.create({
      data: {
        vehicleTypeId,
        capacity: capacity ?? 0,
        active: active ?? true,
        isApproved: false,
        creationBy: userId,
        modifiedBy: userId,
      },
    });
    await this.notifications.notifySuperAdmins(userId, {
      title: 'إضافة مركبة جديدة',
      description: 'بحاجة لاعتماد',
      entityType: 'vehicle',
      entityId: created.id,
    });
    return successWrite(created.id);
  }

  /** POST UpdateTransportationVehicle — update type/capacity/active. */
  async updateVehicle(
    id: number,
    vehicleTypeId: number,
    capacity: number,
    active: boolean | undefined,
    userId: number,
  ) {
    if (!id) return fail('Err101', 'Id is required');
    if (!vehicleTypeId) return fail('Err101', 'VehicleTypeId is required');
    await this.prisma.transportationVehicle.update({
      where: { id },
      data: {
        vehicleTypeId,
        capacity: capacity ?? 0,
        active: active ?? true,
        modifiedBy: userId,
      },
    });
    return successWrite(id);
  }

  /** POST DeleteTransportationVehicle — delete (Id header). */
  async removeVehicle(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicle.delete({ where: { id } });
    return successWrite(id);
  }

  /** POST ApproveTransportationVehicle — approve/unapprove (Id + Approve headers). */
  async approveVehicle(id: number, approve: boolean, userId: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicle.update({
      where: { id },
      data: { isApproved: approve, approvedBy: String(userId) },
    });
    return successWrite(id);
  }
}
