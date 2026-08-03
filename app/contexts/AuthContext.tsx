"use client";

import { createContext, useContext, useState, useCallback } from "react";
import type { Role } from "@/lib/types";
import { AUTH_COOKIE } from "@/lib/auth";
import { apiRaw } from "@/lib/api/client";

export interface User {
  name: string;
  email: string;
  role: Role;
  branch: string;
  token: string;
  companyName: string;
}

interface AuthCtx {
  user: User | null;
  ready: boolean;
  login: (email: string, password: string, companyName?: string) => Promise<User>;
  logout: () => void;
}

const DEFAULT_COMPANY = import.meta.env.VITE_COMPANY ?? "demo";

// Backend transportation role ids → the app's Role (highest privilege wins).
const ROLE_BY_ID: Record<number, Role> = {
  216: "super_admin",
  213: "transportation_admin",
  220: "hr_admin",
  214: "supervisor",
  215: "passenger",
  221: "reader",
};
const ROLE_RANK: Role[] = ["super_admin", "transportation_admin", "hr_admin", "supervisor", "reader", "passenger"];

interface LoginResponse {
  Data: string; // UserToken
  Name?: string;
  BranchId?: number | null;
  RoleList?: { RoleID: number; RoleName: string }[];
}

function roleFromList(list: { RoleID: number }[]): Role | null {
  const roles = list.map((r) => ROLE_BY_ID[r.RoleID]).filter(Boolean) as Role[];
  if (roles.length === 0) return null;
  return ROLE_RANK.find((r) => roles.includes(r)) ?? roles[0];
}

const Ctx = createContext<AuthCtx | null>(null);

/** Auth is stored in a cookie (not localStorage) so the server can read the
 *  session during SSR and render the correct page directly — no loading
 *  spinner and no client-side redirect flash on first paint.
 *  The cookie name lives in "@/lib/auth" so Server Components can import it. */
const MAX_AGE = 60 * 60 * 24 * 30; // 30 days

/** Read the session straight from the cookie (SPA: no server-seeded initialUser). */
function readUserCookie(): User | null {
  if (typeof document === "undefined") return null;
  const raw = document.cookie.split("; ").find((c) => c.startsWith(`${AUTH_COOKIE}=`));
  if (!raw) return null;
  try {
    return JSON.parse(decodeURIComponent(raw.slice(AUTH_COOKIE.length + 1))) as User;
  } catch {
    return null;
  }
}

function writeCookie(user: User | null) {
  if (typeof document === "undefined") return;
  if (user) {
    const value = encodeURIComponent(JSON.stringify(user));
    document.cookie = `${AUTH_COOKIE}=${value}; path=/; max-age=${MAX_AGE}; samesite=lax`;
  } else {
    document.cookie = `${AUTH_COOKIE}=; path=/; max-age=0; samesite=lax`;
  }
}

export function AuthProvider({
  initialUser = null,
  children,
}: {
  initialUser?: User | null;
  children: React.ReactNode;
}) {
  // Seeded from the cookie by the server, so the first render already knows
  // the auth state — hence `ready` is true immediately (no spinner flash).
  const [user, setUser] = useState<User | null>(() => initialUser ?? readUserCookie());

  const persist = useCallback((u: User | null) => {
    setUser(u);
    writeCookie(u);
  }, []);

  const login = useCallback(
    async (email: string, password: string, companyName: string = DEFAULT_COMPANY) => {
      // The auth cookie must carry companyName+token BEFORE the call so the
      // client can send them — but login itself is unauthenticated (auth:false).
      const resp = await apiRaw<string>("POST", "/User/Login", {
        auth: false,
        body: { Email: email, Password: password, CompanyName: companyName },
      });
      const lr = resp as unknown as LoginResponse;
      const role = roleFromList(lr.RoleList ?? []);
      if (!role) throw new Error("noPerms"); // account has no transportation roles
      const u: User = {
        name: lr.Name?.trim() || email,
        email,
        role,
        branch: lr.BranchId != null ? String(lr.BranchId) : "",
        token: lr.Data,
        companyName,
      };
      persist(u);
      return u;
    },
    [persist]
  );

  const logout = useCallback(() => persist(null), [persist]);

  return (
    <Ctx.Provider value={{ user, ready: true, login, logout }}>
      {children}
    </Ctx.Provider>
  );
}

export function useAuth(): AuthCtx {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
