"use client";

import { createContext, useContext, useState, useCallback } from "react";
import type { Role } from "@/lib/types";
import { AUTH_COOKIE } from "@/lib/auth";

export interface User {
  name: string;
  email: string;
  role: Role;
  branch: string;
  token: string;
}

interface AuthCtx {
  user: User | null;
  ready: boolean;
  login: (email: string, password: string) => Promise<User>;
  logout: () => void;
  setRole: (role: Role) => void; // dev role switcher
}

const Ctx = createContext<AuthCtx | null>(null);

/** Auth is stored in a cookie (not localStorage) so the server can read the
 *  session during SSR and render the correct page directly — no loading
 *  spinner and no client-side redirect flash on first paint.
 *  The cookie name lives in "@/lib/auth" so Server Components can import it. */
const MAX_AGE = 60 * 60 * 24 * 30; // 30 days

function writeCookie(user: User | null) {
  if (typeof document === "undefined") return;
  if (user) {
    const value = encodeURIComponent(JSON.stringify(user));
    document.cookie = `${AUTH_COOKIE}=${value}; path=/; max-age=${MAX_AGE}; samesite=lax`;
  } else {
    document.cookie = `${AUTH_COOKIE}=; path=/; max-age=0; samesite=lax`;
  }
}

const nameByRole: Record<Role, string> = {
  system_admin: "أحمد النظام",
  super_admin: "سارة المشرفة",
  line_admin: "محمد الخطوط",
  trans_admin: "خالد النقل",
  supervisor: "علي المشرف",
  reader: "قارئ",
};

export function AuthProvider({
  initialUser = null,
  children,
}: {
  initialUser?: User | null;
  children: React.ReactNode;
}) {
  // Seeded from the cookie by the server, so the first render already knows
  // the auth state — hence `ready` is true immediately (no spinner flash).
  const [user, setUser] = useState<User | null>(initialUser);

  const persist = useCallback((u: User | null) => {
    setUser(u);
    writeCookie(u);
  }, []);

  const login = useCallback(
    async (email: string, _password: string) => {
      // TODO: replace with real API call (POST /auth/login) → { token, roles, branch }
      await new Promise((r) => setTimeout(r, 700));
      const role: Role = "super_admin";
      const u: User = {
        name: nameByRole[role],
        email,
        role,
        branch: "الفرع الرئيسي",
        token: "mock-token",
      };
      persist(u);
      return u;
    },
    [persist]
  );

  const logout = useCallback(() => persist(null), [persist]);

  const setRole = useCallback(
    (role: Role) => {
      setUser((prev) => {
        const next: User = prev
          ? { ...prev, role, name: nameByRole[role] }
          : { name: nameByRole[role], email: "demo@garas.co", role, branch: "الفرع الرئيسي", token: "mock-token" };
        writeCookie(next);
        return next;
      });
    },
    []
  );

  return (
    <Ctx.Provider value={{ user, ready: true, login, logout, setRole }}>
      {children}
    </Ctx.Provider>
  );
}

export function useAuth(): AuthCtx {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
