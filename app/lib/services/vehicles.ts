/* Vehicles + vehicle types service — real calls to the backend
   (/api/Transportation, docs/API.md §6.1). Maps the backend's frozen PascalCase
   envelope keys → the app's Vehicle type. */
import type { Vehicle } from "@/lib/types";
import { apiGet, apiPost, type PaginationHeader } from "@/lib/api/client";

interface VehicleRow {
  Id: number;
  VehicleTypeId: number;
  Capacity: number;
  IsApproved: boolean;
  Active: boolean;
  VehicleTypeName: string;
}

interface VehicleTypeRow {
  Id: number;
  Type: string;
  Active: boolean;
}

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

/* The app's Vehicle carries the type *name* (not its id), but writes need the
   VehicleTypeId. We cache name → id as rows/types stream in so add/update can
   resolve the id the page never sees. */
const typeIdByName = new Map<string, number>();

function toVehicle(v: VehicleRow): Vehicle {
  if (v.VehicleTypeName) typeIdByName.set(v.VehicleTypeName, v.VehicleTypeId);
  return {
    id: String(v.Id),
    type: v.VehicleTypeName ?? "",
    capacity: v.Capacity ?? 0,
    approved: !!v.IsApproved,
    active: !!v.Active,
  };
}

function resolveTypeId(name: string): number {
  const id = typeIdByName.get(name);
  if (!id) throw new Error("نوع المركبة غير معروف");
  return id;
}

/** GET getAllTransportationVehicle — paginated vehicles. */
export async function getVehicles(pageNo = 1, noOfItems = 20): Promise<Paginated<Vehicle>> {
  const res = await apiGet<VehicleRow[]>("getAllTransportationVehicle", { PageNo: pageNo, NoOfItems: noOfItems });
  const items = (res.Data ?? []).map(toVehicle);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** GET getAllVehicleType — vehicle-type lookup mapped to a list of names. */
export async function getVehicleTypes(): Promise<string[]> {
  const res = await apiGet<VehicleTypeRow[]>("getAllVehicleType");
  const rows = res.Data ?? [];
  for (const r of rows) typeIdByName.set(r.Type, r.Id);
  return rows.map((r) => r.Type);
}

/** POST AddTransportationVehicle — create a vehicle (type name resolved → id). */
export async function addVehicle(payload: { type: string; capacity: number; active: boolean }): Promise<void> {
  await apiPost("AddTransportationVehicle", {
    VehicleTypeId: resolveTypeId(payload.type),
    Capacity: payload.capacity,
    Active: payload.active,
  });
}

/** POST UpdateTransportationVehicle — edit a vehicle. */
export async function updateVehicle(payload: Vehicle): Promise<void> {
  await apiPost("UpdateTransportationVehicle", {
    Id: Number(payload.id),
    VehicleTypeId: resolveTypeId(payload.type),
    Capacity: payload.capacity,
    Active: payload.active,
  });
}

/** POST DeleteTransportationVehicle (Id header). */
export async function deleteVehicle(id: string): Promise<void> {
  await apiPost("DeleteTransportationVehicle", {}, { Id: id });
}

/** POST ApproveTransportationVehicle (Id + Approve headers). */
export async function approveVehicle(id: string, approve = true): Promise<void> {
  await apiPost("ApproveTransportationVehicle", {}, { Id: id, Approve: String(approve) });
}

/** POST AddVehicleType — create a vehicle type (created active). */
export async function addVehicleType(name: string): Promise<void> {
  await apiPost("AddVehicleType", { Type: name, Active: true });
}
