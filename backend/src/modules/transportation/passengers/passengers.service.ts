import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../../prisma/prisma.service';
import { fail, paginationHeader, success, successList, successWrite } from '../../../common/response/base-response';
import { HrUserBody } from './passengers.controller';

function fullName(u: { firstName?: string | null; middleName?: string | null; lastName?: string | null }): string {
  return [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');
}

// Maps the transportationVehicleRouteEmployee membership rows (joined with hrUser
// and the route direction) onto the FROZEN JSON keys the Flutter/Next clients
// depend on byte-for-byte (see MODELS.md — RouteUser, RouteUserForMob,
// TransportationRoutesListForPassengerItem). Some keys are intentional typos
// (FirstNane, Longtitud) — do not "fix" them.
@Injectable()
export class PassengersService {
  constructor(private readonly prisma: PrismaService) {}

  /** GET GetTransportationEmployeeList — passengers assigned to a route (shape: RouteUser). */
  async getEmployeeList(routeId: number) {
    if (!routeId) return fail('Err101', 'RouteId is required');
    const rows = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: { routeId, active: true },
      include: { hrUser: true, direction: true },
      orderBy: { id: 'desc' },
    });
    const data = rows.map((m) => ({
      ID: m.id,
      HrUserId: m.hrUserId,
      FirstNane: m.hrUser?.firstName ?? '', // ⚠ frozen typo
      MiddleName: m.hrUser?.middleName ?? '',
      LastName: m.hrUser?.lastName ?? '',
      Mobile: m.hrUser?.mobile ?? '',
      Email: m.hrUser?.email ?? '',
      Photo: m.hrUser?.imgPath ?? '',
      DirectionLatitude: m.direction?.latitude ?? 0,
      DirectionLongtitud: m.direction?.longitude ?? 0, // ⚠ frozen typo
      DirectionId: m.directionId ?? 0,
      DirectionName: m.direction?.routeDirection ?? '',
      Period: m.period ?? '', // Go | Return | Both
    }));
    return success(data);
  }

  /**
   * GET GetTransportationEmployees — "who rides today" for the mobile app
   * (shape: RouteUserForMob). Active memberships whose period matches the
   * requested Period, or 'Both'.
   */
  async getEmployeesForMob(routeId: number, period?: string, dateSearch?: string) {
    if (!routeId) return fail('Err101', 'RouteId is required');
    // TODO: dateSearch filtering + exception (opt-in/opt-out) logic once the
    // TransportationVehicleRouteEmployeeException flow is wired in.
    const rows = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: {
        routeId,
        active: true,
        ...(period ? { period: { in: [period, 'Both'] } } : {}),
      },
      include: { hrUser: true, direction: true },
      orderBy: { id: 'desc' },
    });
    const data = rows.map((m) => ({
      ID: m.id,
      HrUserId: m.hrUserId,
      FirstNane: m.hrUser?.firstName ?? '', // ⚠ frozen typo
      MiddleName: m.hrUser?.middleName ?? '',
      LastName: m.hrUser?.lastName ?? '',
      Mobile: m.hrUser?.mobile ?? '',
      Email: m.hrUser?.email ?? '',
      Photo: m.hrUser?.imgPath ?? '',
      DirectionLatitude: m.direction?.latitude ?? 0,
      DirectionLongtitud: m.direction?.longitude ?? 0, // ⚠ frozen typo
      DirectionId: m.directionId ?? 0,
      DirectionName: m.direction?.routeDirection ?? '',
      Latitude: m.hrUser?.latitude ?? 0, // live/passenger latitude
      Longtitud: m.hrUser?.longitude ?? 0, // ⚠ frozen typo
      RegisteredInLine: true,
      CheckIn: '', // TODO: exception check-in time
      CheckOut: '', // TODO: exception check-out time
    }));
    return success(data);
  }

  /** POST AddTransportationEmployee — bulk insert memberships for one route. */
  async addEmployee(
    routeId: number,
    data: Array<{
      hrUserId: number;
      TransportationVehicleRouteDirectionId?: number;
      Active?: boolean;
      period?: string;
    }>,
  ) {
    if (!routeId) return fail('Err101', 'TransportationVehicleRouteId is required');
    if (!data.length) return fail('Err101', 'Data is required');
    await this.prisma.transportationVehicleRouteEmployee.createMany({
      data: data.map((d) => ({
        routeId,
        hrUserId: Number(d.hrUserId),
        directionId: d.TransportationVehicleRouteDirectionId
          ? Number(d.TransportationVehicleRouteDirectionId)
          : null,
        period: d.period ?? 'Both',
        active: d.Active ?? true,
      })),
    });
    return successWrite(routeId);
  }

  /** POST UpdateTransportationEmployee — update a single membership. */
  async updateEmployee(body: {
    Id: number;
    hrUserId: number;
    TransportationVehicleRouteId: number;
    TransportationVehicleRouteDirectionId?: number;
    Active?: boolean;
  }) {
    const id = Number(body?.Id);
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRouteEmployee.update({
      where: { id },
      data: {
        hrUserId: Number(body.hrUserId),
        routeId: Number(body.TransportationVehicleRouteId),
        directionId: body.TransportationVehicleRouteDirectionId
          ? Number(body.TransportationVehicleRouteDirectionId)
          : null,
        active: body.Active ?? true,
      },
    });
    return successWrite(id);
  }

  /** POST DeleteTransportationEmployee — delete a membership (Id header). */
  async deleteEmployee(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRouteEmployee.delete({ where: { id } });
    return successWrite(id);
  }

  /**
   * GET getTransportationRoutesForHrUser — all route memberships for one hr user
   * (shape: TransportationRoutesListForPassengerItem).
   */
  async getRoutesForHrUser(hrUserId: number) {
    if (!hrUserId) return fail('Err101', 'HrUserId is required');
    const rows = await this.prisma.transportationVehicleRouteEmployee.findMany({
      where: { hrUserId },
      include: { route: true },
      orderBy: { id: 'desc' },
    });
    const data = rows.map((m) => ({
      Id: m.id,
      RouteId: m.routeId,
      TransportationVehicleRouteDirectionId: m.directionId ?? 0,
      Serial: m.route?.serial ?? '',
      NameOfRoute: m.route?.nameOfRoute ?? '',
      PeriodFrom: m.route?.periodFrom ?? '',
      PeriodTo: m.route?.periodTo ?? '',
      Active: m.active,
      CreationDate: m.creationDate ?? '',
      ModifiedDate: m.route?.modifiedDate ?? '',
      ToDate: m.toDate ?? '',
      FromDate: m.fromDate ?? '',
      Period: m.period ?? '',
      DurationLatitude: m.durationLatitude ?? 0,
      DurationLongtitud: m.durationLongitude ?? 0, // ⚠ frozen typo
    }));
    return success(data);
  }

  /** POST AddRoutesForEmployee — bulk create memberships for one hr user. */
  async addRoutesForEmployee(
    hrUserId: number,
    data: Array<{
      RouteId: number;
      Active?: boolean;
      FromDate?: string;
      ToDate?: string;
      Period?: string;
      DurationLatitude?: number;
      DurationLongtitud?: number;
      TransportationVehicleRouteDirectionId?: number;
    }>,
  ) {
    if (!hrUserId) return fail('Err101', 'hrUserId is required');
    if (!data.length) return fail('Err101', 'Data is required');
    await this.prisma.transportationVehicleRouteEmployee.createMany({
      data: data.map((d) => ({
        hrUserId,
        routeId: Number(d.RouteId),
        directionId: d.TransportationVehicleRouteDirectionId
          ? Number(d.TransportationVehicleRouteDirectionId)
          : null,
        period: d.Period ?? 'Both',
        active: d.Active ?? true,
        fromDate: d.FromDate ? new Date(d.FromDate) : null,
        toDate: d.ToDate ? new Date(d.ToDate) : null,
        durationLatitude: d.DurationLatitude ?? null,
        durationLongitude: d.DurationLongtitud ?? null, // frozen key → schema field
      })),
    });
    return successWrite(hrUserId);
  }

  /** POST UpdateRouteEmployee — update a single membership from the passenger side. */
  async updateRouteEmployee(body: {
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
  }) {
    const id = Number(body?.Id);
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRouteEmployee.update({
      where: { id },
      data: {
        routeId: Number(body.TransportationVehicleRouteId),
        hrUserId: Number(body.HrUserId),
        active: body.Active ?? true,
        ...(body.TransportationVehicleRouteDirectionId !== undefined
          ? {
              directionId: body.TransportationVehicleRouteDirectionId
                ? Number(body.TransportationVehicleRouteDirectionId)
                : null,
            }
          : {}),
        ...(body.FromDate !== undefined
          ? { fromDate: body.FromDate ? new Date(body.FromDate) : null }
          : {}),
        ...(body.ToDate !== undefined
          ? { toDate: body.ToDate ? new Date(body.ToDate) : null }
          : {}),
        ...(body.Period !== undefined ? { period: body.Period } : {}),
        ...(body.DurationLatitude !== undefined
          ? { durationLatitude: body.DurationLatitude }
          : {}),
        ...(body.DurationLongtitud !== undefined
          ? { durationLongitude: body.DurationLongtitud }
          : {}),
      },
    });
    return successWrite(id);
  }

  /** POST DeleteRouteEmployee — delete a membership (Id header). */
  async deleteRouteEmployee(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.transportationVehicleRouteEmployee.delete({ where: { id } });
    return successWrite(id);
  }

  /* ---- Passenger (HrUser) profiles ---- */

  /** GET getAllHrUsers — paginated passenger profiles. By default returns ALL
   *  passengers (active + inactive) so deactivated ones stay visible with their
   *  status; pass active=true/false to filter (e.g. route-assign wants active only). */
  async getAllHrUsers(pageNo = 1, noOfItems = 20, name?: string, active?: boolean) {
    const where: Record<string, unknown> = {};
    if (active !== undefined) where.active = active;
    if (name)
      where.OR = [
        { firstName: { contains: name } },
        { middleName: { contains: name } },
        { lastName: { contains: name } },
        { identityNumber: { contains: name } },
        { mobile: { contains: name } },
      ];
    const [total, rows] = await this.prisma.$transaction([
      this.prisma.hrUser.count({ where }),
      this.prisma.hrUser.findMany({
        where,
        include: { maritalStatus: true, _count: { select: { memberships: true } } },
        orderBy: { id: 'desc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);
    const data = rows.map((u) => ({
      Id: u.id,
      Name: fullName(u),
      FirstName: u.firstName ?? '',
      MiddleName: u.middleName ?? '',
      LastName: u.lastName ?? '',
      Mobile: u.mobile ?? '',
      IdentityNumber: u.identityNumber ?? '',
      MaritalStatusId: u.maritalStatusId ?? 0,
      MaritalStatusName: u.maritalStatus?.name ?? '',
      ImgPath: u.imgPath ?? '',
      Latitude: u.latitude ?? null,
      Longitude: u.longitude ?? null,
      Active: u.active,
      RoutesCount: u._count?.memberships ?? 0,
    }));
    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** GET GetMaritalStatus — marital-status lookup (Id + Name) for the profile dropdown. */
  async getMaritalStatus() {
    const rows = await this.prisma.maritalStatus.findMany({ orderBy: { id: 'asc' } });
    return success(rows.map((m) => ({ Id: m.id, Name: m.name })));
  }

  /** GET getHrUser — a single passenger profile. */
  async getHrUser(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const u = await this.prisma.hrUser.findUnique({ where: { id }, include: { maritalStatus: true, _count: { select: { memberships: true } } } });
    if (!u) return fail('Err404', 'Passenger not found');
    return success({
      Id: u.id,
      Name: fullName(u),
      FirstName: u.firstName ?? '',
      MiddleName: u.middleName ?? '',
      LastName: u.lastName ?? '',
      Mobile: u.mobile ?? '',
      IdentityNumber: u.identityNumber ?? '',
      MaritalStatusId: u.maritalStatusId ?? 0,
      MaritalStatusName: u.maritalStatus?.name ?? '',
      ImgPath: u.imgPath ?? '',
      Latitude: u.latitude ?? null,
      Longitude: u.longitude ?? null,
      Active: u.active,
      RoutesCount: u._count?.memberships ?? 0,
    });
  }

  /** POST AddHrUser — create a passenger profile. */
  async addHrUser(body: HrUserBody) {
    if (!body?.FirstName?.trim()) return fail('Err101', 'FirstName is required');
    const created = await this.prisma.hrUser.create({
      data: {
        firstName: body.FirstName,
        middleName: body.MiddleName ?? null,
        lastName: body.LastName ?? null,
        mobile: body.Mobile ?? null,
        identityNumber: body.IdentityNumber ?? null,
        maritalStatusId: body.MaritalStatusId ? Number(body.MaritalStatusId) : null,
        imgPath: body.ImgPath ?? null,
        latitude: body.Latitude ?? null,
        longitude: body.Longitude ?? null,
        email: body.Email ?? null,
        active: body.Active ?? true,
      },
    });
    return successWrite(created.id);
  }

  /** POST UpdateHrUser — edit a passenger profile. */
  async updateHrUser(id: number, body: HrUserBody) {
    if (!id) return fail('Err101', 'Id is required');
    await this.prisma.hrUser.update({
      where: { id },
      data: {
        firstName: body.FirstName ?? '',
        middleName: body.MiddleName ?? null,
        lastName: body.LastName ?? null,
        mobile: body.Mobile ?? null,
        identityNumber: body.IdentityNumber ?? null,
        maritalStatusId: body.MaritalStatusId ? Number(body.MaritalStatusId) : null,
        imgPath: body.ImgPath ?? null,
        latitude: body.Latitude ?? null,
        longitude: body.Longitude ?? null,
        active: body.Active ?? true,
      },
    });
    return successWrite(id);
  }
}
