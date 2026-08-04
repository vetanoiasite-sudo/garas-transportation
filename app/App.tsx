import { createBrowserRouter, createHashRouter, Navigate, Outlet, useParams } from "react-router-dom";
import { isLocale, defaultLocale } from "@/lib/i18n";
import { LocaleProvider } from "@/contexts/LocaleContext";
import { AuthProvider } from "@/contexts/AuthContext";
import { ToastProvider } from "@/contexts/ToastContext";
import DirectionSync from "@/components/DirectionSync";
import AppShell from "@/components/layout/AppShell";

import LoginPage from "@/pages/Login";
import DashboardPage from "@/pages/Dashboard";
import AttendancePage from "@/pages/Attendance";
import LinesPage from "@/pages/Lines";
import LineRoutesPage from "@/pages/LineRoutes";
import RouteDetailPage from "@/pages/RouteDetail";
import AllRoutesPage from "@/pages/AllRoutes";
import VehiclesPage from "@/pages/Vehicles";
import PassengersPage from "@/pages/Passengers";
import PassengerProfilePage from "@/pages/PassengerProfile";
import SuppliersPage from "@/pages/Suppliers";
import SupplierProfilePage from "@/pages/SupplierProfile";
import AccountStatementPage from "@/pages/AccountStatement";
import DeductionsPage from "@/pages/Deductions";
import RepricingPage from "@/pages/Repricing";
import ExceptionsPage from "@/pages/Exceptions";
import UsersPage from "@/pages/Users";
import CostsPage from "@/pages/Costs";
import NotificationsPage from "@/pages/Notifications";

/** Per-locale root: validates the :locale segment and mounts the providers.
 *  Replaces the old Next `app/[locale]/layout.tsx`. */
function LocaleLayout() {
  const { locale } = useParams();
  if (!locale || !isLocale(locale)) {
    return <Navigate to={`/${defaultLocale}/dashboard`} replace />;
  }
  return (
    <LocaleProvider locale={locale}>
      <AuthProvider>
        <ToastProvider>
          <DirectionSync locale={locale} />
          <Outlet />
        </ToastProvider>
      </AuthProvider>
    </LocaleProvider>
  );
}

// The offline/single-file demo uses hash routing so it works at any URL/base
// (e.g. a published static page) with no server rewrites.
const makeRouter = import.meta.env.VITE_DEMO === "true" ? createHashRouter : createBrowserRouter;

export const router = makeRouter([
  { path: "/", element: <Navigate to={`/${defaultLocale}/dashboard`} replace /> },
  {
    path: "/:locale",
    element: <LocaleLayout />,
    children: [
      { index: true, element: <Navigate to="dashboard" replace /> },
      { path: "login", element: <LoginPage /> },
      {
        // Authenticated app shell (sidebar + topbar). Guards auth internally.
        element: <AppShell />,
        children: [
          { path: "dashboard", element: <DashboardPage /> },
          { path: "attendance", element: <AttendancePage /> },
          { path: "lines", element: <LinesPage /> },
          { path: "lines/:id/routes", element: <LineRoutesPage /> },
          { path: "lines/:id/routes/:routeId", element: <RouteDetailPage /> },
          { path: "routes", element: <AllRoutesPage /> },
          { path: "vehicles", element: <VehiclesPage /> },
          { path: "passengers", element: <PassengersPage /> },
          { path: "passengers/:id", element: <PassengerProfilePage /> },
          { path: "suppliers", element: <SuppliersPage /> },
          { path: "suppliers/:id", element: <SupplierProfilePage /> },
          { path: "account-statement", element: <AccountStatementPage /> },
          { path: "deductions", element: <DeductionsPage /> },
          { path: "repricing", element: <RepricingPage /> },
          { path: "exceptions", element: <ExceptionsPage /> },
          { path: "users", element: <UsersPage /> },
          { path: "notifications", element: <NotificationsPage /> },
          { path: "costs", element: <CostsPage /> },
        ],
      },
      { path: "*", element: <Navigate to="dashboard" replace /> },
    ],
  },
]);
