"use client";

import { createContext, useContext, useEffect, useState, useCallback } from "react";
import type { Role } from "@/lib/types";

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
const STORAGE_KEY = "garas.auth";

const nameByRole: Record<Role, string> = {
  system_admin: "أحمد النظام",
  super_admin: "سارة المشرفة",
  line_admin: "محمد الخطوط",
  trans_admin: "خالد النقل",
  supervisor: "علي المشرف",
  reader: "قارئ",
};

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [ready, setReady] = useState(false);

  useEffect(() => {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (raw) setUser(JSON.parse(raw));
    } catch {
      /* ignore */
    }
    setReady(true);
  }, []);

  const persist = useCallback((u: User | null) => {
    setUser(u);
    try {
      if (u) localStorage.setItem(STORAGE_KEY, JSON.stringify(u));
      else localStorage.removeItem(STORAGE_KEY);
    } catch {
      /* ignore */
    }
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
        try {
          localStorage.setItem(STORAGE_KEY, JSON.stringify(next));
        } catch {
          /* ignore */
        }
        return next;
      });
    },
    []
  );

  return (
    <Ctx.Provider value={{ user, ready, login, logout, setRole }}>
      {children}
    </Ctx.Provider>
  );
}

export function useAuth(): AuthCtx {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
