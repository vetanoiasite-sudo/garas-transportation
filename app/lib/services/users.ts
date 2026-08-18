/* Users service — login accounts (the `User` table + roles via UserRole).
   Real calls to /User (GetUserList / AddUser / UpdateUser / DeleteUser).
   Maps the app's Role strings ↔ the backend's numeric transportation role ids. */
import type { SystemUser, Role } from "@/lib/types";
import { apiRaw, ApiError, type PaginationHeader } from "@/lib/api/client";

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

// All mapped roles, ordered strongest-first (rank order, no duplicates).
function allRoles(list: { RoleID: number }[]): Role[] {
  const roles = new Set(list.map((r) => ID_TO_ROLE[r.RoleID]).filter(Boolean) as Role[]);
  return ROLE_RANK.filter((r) => roles.has(r));
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
  const roles = allRoles(u.RoleList ?? []);
  return {
    id: String(u.Id),
    name: u.Name ?? "",
    email: u.Email ?? "",
    mobile: u.Mobile ?? "",
    role: roles[0] ?? "reader",
    roles: roles.length ? roles : ["reader"],
    active: !!u.Active,
  };
}

/** UserDDL — the row /User/GetUserList returns (under DDLList, not Data). */
interface UserDDLRow {
  ID?: number | string;
  Name?: string;
  Email?: string;
  JobTitleName?: string;
  Department?: string;
  BranchName?: string;
}

/** GET /User/GetUserList — the login users of the current company.
 *  Returns a DDL list (no pagination, no role filter) — role/status filtering
 *  is done client-side by the callers. */
export async function getUsers(query: { role?: Role; pageNo?: number; noOfItems?: number } = {}): Promise<Paginated<SystemUser>> {
  const pageNo = query.pageNo ?? 1;
  const noOfItems = query.noOfItems ?? 200;
  const res = (await apiRaw<unknown>("GET", "/User/GetUserList")) as { DDLList?: UserDDLRow[] };
  const items: SystemUser[] = (res.DDLList ?? []).map((u) => ({
    id: String(u.ID ?? ""),
    name: u.Name ?? "",
    email: u.Email ?? "",
    mobile: "",
    role: "reader",
    roles: ["reader"],
    active: true,
  }));
  void query;
  return {
    items,
    pagination: { CurrentPage: pageNo, ItemsPerPage: noOfItems, TotalItems: items.length, TotalPages: 1 },
  };
}

// Multi-role payload: RoleIds carries the full set; RoleId (strongest) rides
// along for backward compatibility.
const roleIdsBody = (roles: Role[]) => {
  const ids = [...new Set(roles.map((r) => ROLE_TO_ID[r]))];
  return { RoleIds: ids, RoleId: ids[0] };
};

/* The CoreApi has no AddUser/UpdateUser/DeleteUser. A login account is created
   by granting an EXISTING HR employee access:
     HrUser/CreateHrUser → HrUser/AddHrEmployeeToUser (email + password)
   and roles are assigned through the HR module's EditEmployeeRole. Only the
   first step is exposed here; the rest needs the HR module wired up. */

/** POST /HrUser/AddHrEmployeeToUser — grant an existing HR employee a login. */
export async function grantLogin(hrUserId: string, email: string, password: string): Promise<void> {
  await apiRaw("POST", "/HrUser/AddHrEmployeeToUser", {
    body: { HrUserId: Number(hrUserId), Email: email, Password: password, ConfirmPass: password },
  });
}

const notSupported = () =>
  Promise.reject(new ApiError("not-supported", "غير مدعوم في الـ API الحالي: أنشئ الموظف ثم امنحه صلاحية الدخول."));

/** Not available on the CoreApi — see grantLogin above. */
export function addUser(_payload: Omit<SystemUser, "id"> & { password?: string }): Promise<string> {
  void _payload;
  return notSupported() as Promise<string>;
}

/** Not available on the CoreApi — roles are managed by the HR module. */
export function updateUser(_payload: SystemUser & { password?: string }): Promise<void> {
  void _payload;
  return notSupported() as Promise<void>;
}

/** Not available on the CoreApi. */
export function deleteUser(_id: string): Promise<void> {
  void _id;
  return notSupported() as Promise<void>;
}
