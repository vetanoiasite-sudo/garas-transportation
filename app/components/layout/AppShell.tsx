"use client";

import { useEffect, useState } from "react";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { useLocale } from "@/contexts/LocaleContext";
import Sidebar from "./Sidebar";
import Topbar from "./Topbar";

export default function AppShell() {
  const { user, ready } = useAuth();
  const { locale } = useLocale();
  const navigate = useNavigate();
  const pathname = useLocation().pathname;
  const [navOpen, setNavOpen] = useState(false);

  useEffect(() => {
    if (ready && !user) navigate(`/${locale}/login`, { replace: true });
  }, [ready, user, navigate, locale]);

  // close the mobile drawer whenever the route changes
  useEffect(() => {
    setNavOpen(false);
  }, [pathname]);

  if (!ready || !user) {
    return (
      <div style={{ minHeight: "100vh", display: "grid", placeItems: "center" }}>
        <span className="spinner spinner-brand" style={{ width: 32, height: 32 }} />
      </div>
    );
  }

  return (
    <div className="shell">
      <Sidebar open={navOpen} onClose={() => setNavOpen(false)} />
      {navOpen && <div className="scrim" onClick={() => setNavOpen(false)} aria-hidden />}
      <div className="main">
        <Topbar onMenu={() => setNavOpen(true)} />
        <div className="content"><Outlet /></div>
      </div>
    </div>
  );
}
