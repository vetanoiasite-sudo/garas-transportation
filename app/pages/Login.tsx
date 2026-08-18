"use client";

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLocale } from "@/contexts/LocaleContext";
import { useAuth } from "@/contexts/AuthContext";
import { ApiError } from "@/lib/api/client";
import { IconBus } from "@/components/ui/Icons";

export default function LoginPage() {
  const { t, locale } = useLocale();
  const { login } = useAuth();
  const navigate = useNavigate();

  // The backend is multi-tenant: CompanyName picks which company database the
  // login (and every later call) runs against, so the user must supply it.
  // Remembered across sessions since a given user always signs into one company.
  const [company, setCompany] = useState(() => localStorage.getItem("garas.company") ?? "");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [remember, setRemember] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    if (!company || !email || !password) {
      setError(t("login.invalid"));
      return;
    }
    setLoading(true);
    try {
      await login(email, password, company.trim());
      localStorage.setItem("garas.company", company.trim());
      navigate(`/${locale}/dashboard`);
    } catch (err) {
      // Distinguish "can't reach the server" from genuinely bad credentials —
      // both used to surface the same misleading "invalid credentials" message.
      if (err instanceof Error && err.message === "noPerms") {
        setError(t("login.noPerms"));
      } else if (err instanceof ApiError && (err.code === "network" || err.code === "bad-response")) {
        setError(t("login.network"));
      } else {
        setError(t("login.invalid"));
      }
      setLoading(false);
    }
  };

  return (
    <div className="login-wrap">
      <aside className="login-aside">
        <div className="row" style={{ gap: "var(--space-3)" }}>
          <span className="logo" style={{ width: 48, height: 48, borderRadius: "var(--radius-lg)", background: "#fff", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <IconBus style={{ width: 28, height: 28, color: "var(--color-navy)" }} />
          </span>
          <h2 style={{ margin: 0 }}>{t("appName")}</h2>
        </div>
        <p>{t("appTagline")} — {t("login.asideDesc")}</p>
      </aside>

      <div className="login-panel">
        <form className="login-card card card-pad" onSubmit={submit}>
          <h1 className="page-title mb-4">{t("login.title")}</h1>
          <p className="muted mb-4">{t("login.subtitle")}</p>

          {error && (
            <div className="banner-pending" style={{ background: "var(--red-50)", borderColor: "var(--red-500)", color: "var(--red-700)", marginBottom: "var(--space-4)" }}>
              {error}
            </div>
          )}

          <div className="field">
            <label className="label" htmlFor="company">{t("login.company")}</label>
            <input id="company" className="input" value={company} onChange={(e) => setCompany(e.target.value)} autoComplete="organization" placeholder={t("login.companyHint")} />
          </div>

          <div className="field">
            <label className="label" htmlFor="email">{t("login.email")}</label>
            <input id="email" className="input" type="email" value={email} onChange={(e) => setEmail(e.target.value)} autoComplete="username" />
          </div>

          <div className="field">
            <label className="label" htmlFor="password">{t("login.password")}</label>
            <input id="password" className="input" type="password" value={password} onChange={(e) => setPassword(e.target.value)} autoComplete="current-password" />
          </div>

          <label className="checkbox-row mb-4">
            <input type="checkbox" checked={remember} onChange={(e) => setRemember(e.target.checked)} />
            <span>{t("login.remember")}</span>
          </label>

          <button className="btn btn-brand btn-block btn-lg" type="submit" disabled={loading}>
            {loading ? <span className="spinner" /> : t("action.signIn")}
          </button>
        </form>
      </div>
    </div>
  );
}
