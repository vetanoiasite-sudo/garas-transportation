/* Users service — login accounts (the `User` table + roles via UserRole).
   Real calls to /User (GetUserList / AddUser / UpdateUser / DeleteUser).
   Maps the app's Role strings ↔ the backend's numeric transportation role ids. */
import type { SystemUser, Role } from "@/lib/types";
import { apiRaw, type PaginationHeader } from "@/lib/api/client";

export interface Paginated<T> {
  items: T[];
  pagination: PaginationHeader;
}

// App Role ↔ backend transportation role id (canonical id used for writes).
const ROLE_TO_ID: Record<Role, number> = {
  super_admin: 216,
  transportation_admin: 213,
  hr_admin: 220,
  reader: 221,
  supervisor: 214,
  passenger: 215,
};
const ID_TO_ROLE: Record<number, Role> = {
  216: "super_admin",
  213: "transportation_admin",
  220: "hr_admin",
  221: "reader",
  214: "supervisor",
  215: "passenger",
};
// Highest privilege first — a user with several roles shows its strongest one.
const ROLE_RANK: Role[] = ["super_admin", "transportation_admin", "hr_admin", "supervisor", "reader", "passenger"];

function primaryRole(list: { RoleID: number }[]): Role {
  const roles = list.map((r) => ID_TO_ROLE[r.RoleID]).filter(Boolean) as Role[];
  return ROLE_RANK.find((r) => roles.includes(r)) ?? roles[0] ?? "reader";
}

interface UserRow {
  Id: number;
  Name: string;
  Email: string;
  Mobile: string;
  Active: boolean;
  RoleList: { RoleID: number; RoleName: string }[];
}

function toSystemUser(u: UserRow): SystemUser {
  return {
    id: String(u.Id),
    name: u.Name ?? "",
    email: u.Email ?? "",
    mobile: u.Mobile ?? "",
    role: primaryRole(u.RoleList ?? []),
    active: !!u.Active,
  };
}

/** GET /User/GetUserList — paginated login users (optionally filtered by role). */
export async function getUsers(query: { role?: Role; pageNo?: number; noOfItems?: number } = {}): Promise<Paginated<SystemUser>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 200;
  const res = await apiRaw<UserRow[]>("GET", "/User/GetUserList", {
    headers: {
      PageNo: pageNo,
      NoOfItems: noOfItems,
      RoleId: query.role ? ROLE_TO_ID[query.role] : undefined,
    },
  });
  const items = (res.Data ?? []).map(toSystemUser);
  return {
    items,
    pagination: res.PaginationHeader ?? { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

/** POST /User/AddUser — create a login user with a role. Returns the new id. */
export async function addUser(payload: Omit<SystemUser, "id"> & { password?: string }): Promise<string> {
  const res = await apiRaw<unknown>("POST", "/User/AddUser", {
    body: {
      Name: payload.name,
      Email: payload.email,
      Mobile: payload.mobile,
      Password: payload.password,
      RoleId: ROLE_TO_ID[payload.role],
      Active: payload.active,
    },
  });
  return String((res as { Id?: string | number }).Id ?? "");
}

/** POST /User/UpdateUser — edit a login user (profile + role + status). */
export async function updateUser(payload: SystemUser & { password?: string }): Promise<void> {
  await apiRaw("POST", "/User/UpdateUser", {
    body: {
      Id: Number(payload.id),
      Name: payload.name,
      Email: payload.email,
      Mobile: payload.mobile,
      Password: payload.password || undefined,
      RoleId: ROLE_TO_ID[payload.role],
      Active: payload.active,
    },
  });
}

/** POST /User/DeleteUser — deactivate a login user (Id header). */
export async function deleteUser(id: string): Promise<void> {
  await apiRaw("POST", "/User/DeleteUser", { headers: { Id: id } });
}
