import { Injectable } from '@nestjs/common';
import * as bcrypt from 'bcryptjs';
import { PrismaService } from '../../prisma/prisma.service';
import { decryptToken, encryptToken } from '../../common/crypto/token.crypto';
import { fail, success, successList, successWrite, paginationHeader } from '../../common/response/base-response';
import { LoginDto } from './dto/login.dto';

/** Payload for creating / editing a login user from the Users admin screen. */
export interface UserAdminDto {
  Id?: number;
  Name?: string;
  Email?: string;
  Mobile?: string;
  Password?: string;
  RoleId?: number;
  Active?: boolean;
}

@Injectable()
export class UserService {
  constructor(private readonly prisma: PrismaService) {}

  private companyAllowed(name?: string): boolean {
    const allowed = (process.env.ALLOWED_COMPANIES ?? 'demo')
      .split(',')
      .map((s) => s.trim().toLowerCase());
    return !!name && allowed.includes(name.toLowerCase());
  }

  private fullName(u: { firstName?: string | null; middleName?: string | null; lastName?: string | null }): string {
    return [u.firstName, u.middleName, u.lastName].filter(Boolean).join(' ');
  }

  /** GET /User/GetAllUsers — active LOGIN users (User table) with their roles,
   *  for pickers such as the route-supervisor dropdown. These are the accounts
   *  that log in and hold roles — distinct from HrUser/passenger records. */
  async getAllUsers() {
    const users = await this.prisma.user.findMany({
      where: { active: true },
      include: { userRoles: { where: { active: true }, include: { role: true } } },
      orderBy: { id: 'asc' },
    });
    return success(
      users.map((u) => ({
        Id: u.id,
        Name: this.fullName(u) || u.email,
        RoleList: u.userRoles.map((r) => ({ RoleID: r.roleId, RoleName: r.role.name })),
      })),
    );
  }

  /** Split a display name into first / last for the User table (single-field UI). */
  private splitName(name?: string): { firstName: string; lastName: string | null } {
    const parts = (name ?? '').trim().split(/\s+/).filter(Boolean);
    if (parts.length === 0) return { firstName: '', lastName: null };
    if (parts.length === 1) return { firstName: parts[0], lastName: null };
    return { firstName: parts[0], lastName: parts.slice(1).join(' ') };
  }

  /** GET /User/GetUserList — paginated login users (optionally filtered by role)
   *  for the Users administration screen. Each row carries its role list. */
  async getUserList(pageNo = 1, noOfItems = 20, roleId?: number) {
    const where: Record<string, unknown> = { active: true };
    if (roleId) where.userRoles = { some: { roleId, active: true } };

    const [total, users] = await this.prisma.$transaction([
      this.prisma.user.count({ where }),
      this.prisma.user.findMany({
        where,
        include: { userRoles: { where: { active: true }, include: { role: true } } },
        orderBy: { id: 'asc' },
        skip: (pageNo - 1) * noOfItems,
        take: noOfItems,
      }),
    ]);

    const data = users.map((u) => ({
      Id: u.id,
      Name: this.fullName(u) || u.email,
      Email: u.email,
      Mobile: u.mobile ?? '',
      Active: u.active,
      RoleList: u.userRoles.map((r) => ({ RoleID: r.roleId, RoleName: r.role.name })),
    }));
    return successList(data, paginationHeader(pageNo, noOfItems, total));
  }

  /** POST /User/AddUser — create a login user and assign a transportation role. */
  async addUser(dto: UserAdminDto) {
    if (!dto?.Email || !dto?.Password) return fail('Err101', 'Email and Password are required');
    if (!dto?.RoleId) return fail('Err101', 'RoleId is required');
    const existing = await this.prisma.user.findFirst({ where: { email: dto.Email } });
    if (existing) return fail('Err102', 'A user with this email already exists');

    const { firstName, lastName } = this.splitName(dto.Name);
    const password = await bcrypt.hash(dto.Password, 10);
    const user = await this.prisma.user.create({
      data: {
        email: dto.Email,
        password,
        firstName,
        lastName,
        mobile: dto.Mobile ?? null,
        active: dto.Active ?? true,
        userRoles: { create: [{ roleId: dto.RoleId, active: true }] },
      },
    });
    return successWrite(user.id, 'User created');
  }

  /** POST /User/UpdateUser — edit a login user's profile, role and status. */
  async updateUser(dto: UserAdminDto) {
    if (!dto?.Id) return fail('Err101', 'Id is required');
    const user = await this.prisma.user.findUnique({ where: { id: dto.Id } });
    if (!user) return fail('Err142', 'User not found');

    // Guard against stealing another account's email.
    if (dto.Email && dto.Email !== user.email) {
      const clash = await this.prisma.user.findFirst({ where: { email: dto.Email, id: { not: dto.Id } } });
      if (clash) return fail('Err102', 'A user with this email already exists');
    }

    const data: Record<string, unknown> = {};
    if (dto.Name !== undefined) {
      const { firstName, lastName } = this.splitName(dto.Name);
      data.firstName = firstName;
      data.middleName = null;
      data.lastName = lastName;
    }
    if (dto.Email !== undefined) data.email = dto.Email;
    if (dto.Mobile !== undefined) data.mobile = dto.Mobile || null;
    if (dto.Active !== undefined) data.active = dto.Active;
    if (dto.Password) data.password = await bcrypt.hash(dto.Password, 10);
    await this.prisma.user.update({ where: { id: dto.Id }, data });

    // Reconcile the role: deactivate any others, (re)activate the chosen one.
    if (dto.RoleId) {
      await this.prisma.userRole.updateMany({
        where: { userId: dto.Id, roleId: { not: dto.RoleId }, active: true },
        data: { active: false },
      });
      const current = await this.prisma.userRole.findFirst({ where: { userId: dto.Id, roleId: dto.RoleId } });
      if (current) {
        if (!current.active) await this.prisma.userRole.update({ where: { id: current.id }, data: { active: true } });
      } else {
        await this.prisma.userRole.create({ data: { userId: dto.Id, roleId: dto.RoleId, active: true } });
      }
    }
    return successWrite(dto.Id, 'User updated');
  }

  /** POST /User/DeleteUser — deactivate a login user and end its sessions. */
  async deleteUser(id: number) {
    if (!id) return fail('Err101', 'Id is required');
    const user = await this.prisma.user.findUnique({ where: { id } });
    if (!user) return fail('Err142', 'User not found');
    await this.prisma.user.update({ where: { id }, data: { active: false } });
    await this.prisma.userSession.updateMany({ where: { userId: id, active: true }, data: { active: false } });
    return successWrite(id, 'User deactivated');
  }

  /** POST /User/Login — validates company + credentials, opens a session,
   *  returns the documented LoginResponse (Data = UserToken, RoleList, …). */
  async login(dto: LoginDto) {
    if (!dto?.Email || (!dto?.Password && dto?.ExternalLoginFrom !== 'office365')) {
      return fail('Err101', 'Email and Password are required');
    }
    if (!this.companyAllowed(dto.CompanyName)) {
      return fail('Err-P200', 'Invalid or unknown CompanyName');
    }

    const user = await this.prisma.user.findFirst({
      where: { email: dto.Email, active: true },
      include: { hrUser: true },
    });
    if (!user) return fail('Err-P6', 'Invalid credentials');

    // ⚠ office365 path skips the password check — faithful to the old docs (insecure).
    if (dto.ExternalLoginFrom !== 'office365') {
      const okPass = await bcrypt.compare(dto.Password ?? '', user.password);
      if (!okPass) return fail('Err-P6', 'Invalid credentials');
    }

    const hours = Number(process.env.SESSION_HOURS ?? 24);
    const session = await this.prisma.userSession.create({
      data: { userId: user.id, active: true, endDate: new Date(Date.now() + hours * 3600_000) },
    });
    const token = encodeURIComponent(encryptToken(String(session.id)));

    const roles = await this.prisma.userRole.findMany({
      where: { userId: user.id, active: true },
      include: { role: true },
    });

    return {
      Result: true,
      Errors: [],
      Data: token, // LoginResponse.Data = UserToken
      UserID: encryptToken(String(user.id)),
      UserIDNO: user.id,
      Name: this.fullName(user),
      PhotoUrl: user.photoUrl ?? '',
      BranchId: user.branchId ?? null,
      EmplyeeId: user.hrUser?.id ?? null, // ⚠ frozen misspelling: linked HrUser id
      RoleList: roles.map((r) => ({ RoleID: r.roleId, RoleName: r.role.name })),
      GroupList: [],
    };
  }

  /** POST /User/Logout — deactivates the session behind the token. */
  async logout(token?: string) {
    if (!token) return fail('Err142', 'Token required');
    try {
      const sessionId = Number(decryptToken(decodeURIComponent(token)));
      await this.prisma.userSession.updateMany({ where: { id: sessionId }, data: { active: false } });
    } catch {
      /* malformed token — treat as already logged out */
    }
    return { Result: true, Errors: [] };
  }

  /** GET /User/GetUserData — rebuild identity for an active session. */
  async getUserData(userId: number) {
    const user = await this.prisma.user.findUnique({ where: { id: userId }, include: { hrUser: true } });
    if (!user) return fail('Err142', 'User not found');
    const roles = await this.prisma.userRole.findMany({
      where: { userId, active: true },
      include: { role: true },
    });
    return success({
      UserIDNO: user.id,
      Name: this.fullName(user),
      PhotoUrl: user.photoUrl ?? '',
      BranchId: user.branchId ?? null,
      EmplyeeId: user.hrUser?.id ?? null,
      RoleList: roles.map((r) => ({ RoleID: r.roleId, RoleName: r.role.name })),
    });
  }
}
