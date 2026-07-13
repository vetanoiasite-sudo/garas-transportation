# 🚌 Garas Employee Transportation Management System
## Implementation Plan

### Overview
A full-featured **Arabic-first (RTL) Employee Transportation Management System** built as a **Next.js 14 App Router + TypeScript** web application, using the **Flowbite Arabic Design System** tokens from the Figma file. All screens will be wired together with placeholder API hooks for real backend integration later.

---

## Stack

| Layer | Choice |
|---|---|
| Framework | **Next.js 14** (App Router) |
| Language | **TypeScript** |
| Styling | **Vanilla CSS** (CSS custom properties from Figma design tokens) |
| State | **React Context + Zustand** (auth, role, filters) |
| Icons | **Heroicons / Lucide React** |
| Fonts | **Rubik** (Arabic + Latin) from Google Fonts |
| Maps | **Leaflet.js** (for station picker & map view) |
| Charts | **Recharts** (for KPI dashboard) |
| API Layer | **Axios** with typed service modules (placeholder hooks) |
| i18n | **next-intl** (Arabic RTL primary, English LTR toggle) |

---

## Design Tokens (from Figma)

| Token | Value |
|---|---|
| `--color-brand` | `#1C64F2` (Flowbite Blue) |
| `--color-brand-dark` | `#1A56DB` |
| `--color-navy` | `#1E3A5F` (primary nav) |
| `--color-success` | `#057A55` |
| `--color-danger` | `#E02424` |
| `--color-warning` | `#E3A008` |
| `--color-secondary` | `#6B7280` |
| `--color-surface` | `#F9FAFB` |
| `--color-border` | `#E5E7EB` |
| Arabic font | **Cairo** (wght 400–700) |
| Border radius | `0.5rem` (base), `0.375rem` (sm), `1rem` (lg) |

---

## Information Architecture

```
/                       → redirect → /ar/dashboard  (or /en/dashboard)
/[locale]/login         → Login screen
/[locale]/dashboard     → Dashboard (KPIs + route rail)
/[locale]/lines         → Lines list (card grid)
/[locale]/lines/[id]/routes             → Routes of a line
/[locale]/lines/[id]/routes/[routeId]   → Route Detail (stations + passengers)
/[locale]/passengers    → Passengers list + profile
/[locale]/passengers/[id]               → Passenger profile
/[locale]/vehicles      → Vehicles list
/[locale]/shifts        → Working Days & Hours
/[locale]/suppliers     → Suppliers list
/[locale]/suppliers/[id]                → Supplier profile
/[locale]/account-statement             → Financial statement
/[locale]/repricing     → Repricing list + create
/[locale]/exceptions    → Exceptions list
/[locale]/deductions    → Deductions list
/[locale]/attendance    → Attendance report
```

---

## Proposed Changes

### Foundation & Design System

#### [NEW] `src/styles/tokens.css`
CSS custom properties for all Figma design tokens: colors, typography scale, spacing, shadows, border-radius, z-index.

#### [NEW] `src/styles/globals.css`
Global styles: RTL/LTR body direction, font imports (Cairo + Inter), reset, and utility classes mirroring the Flowbite system.

#### [NEW] `src/styles/components.css`
Component-level styles: buttons (Brand/Secondary/Success/Danger/Warning/Ghost/Dark × 5 sizes × 5 states), badges, alerts, cards, tables, forms.

---

### Layout & Navigation

#### [NEW] `src/components/layout/AppShell.tsx`
Root layout wrapper: sidebar + topbar + main content area. Handles RTL direction from locale context.

#### [NEW] `src/components/layout/Sidebar.tsx`
Permission-aware collapsible sidebar with the exact navigation tree from the doc:
- Dashboard, Deductions, Suppliers Statement, Employees, Users, Exceptions, Repricing, Transportation Management (collapsible).

#### [NEW] `src/components/layout/Topbar.tsx`
Top header: page title, user avatar, role badge, language switcher (AR/EN), branch indicator.

#### [NEW] `src/components/layout/RoleSwitcher.tsx`
Dev-mode role switcher (visible in the header) to test all 6 roles during prototype phase.

---

### Authentication

#### [NEW] `src/app/[locale]/login/page.tsx`
Login screen: email/password form, "Remember me", loading state on submit, error states (invalid creds, no permissions, server unreachable toast).

#### [NEW] `src/contexts/AuthContext.tsx`
Auth context with role, branch, token. Drives permission-aware rendering everywhere.

---

### Dashboard Module

#### [NEW] `src/app/[locale]/dashboard/page.tsx`
- Left route rail (15% width, scrollable, click to filter KPIs)
- Filter bar: Supplier → Driver (disabled until supplier) → Bus Serial → Date
- 10 KPI cards (clickable → navigate to relevant list)
- Header actions: Line Cost Report, View Attendance, Electronic Touch Attendance (role-gated)

#### [NEW] `src/components/dashboard/KPICard.tsx`
Reusable card with icon, value, label, optional subtitle, navigation link.

#### [NEW] `src/app/[locale]/attendance/page.tsx`
Attendance report: multi-filter table, expandable rows, passenger history dialog, Excel download.

---

### Lines, Routes & Stations Module

#### [NEW] `src/app/[locale]/lines/page.tsx`
Card grid, role-gated add button, Add/Edit Line dialog.

#### [NEW] `src/app/[locale]/lines/[id]/routes/page.tsx`
Routes card grid for a specific line.

#### [NEW] `src/app/[locale]/lines/[id]/routes/[routeId]/page.tsx`
Route detail: two-panel layout (Stations + Passengers), all CRUD dialogs.

#### [NEW] `src/components/routes/RouteForm.tsx`
Add/Edit Route dialog with business logic: segmented route-type control, cascading dropdowns, conditional time fields.

#### [NEW] `src/components/routes/StationForm.tsx`
Add/Edit Station dialog with map picker.

#### [NEW] `src/components/routes/MapPickerDialog.tsx`
Leaflet.js map dialog with draggable pin, current-location button, returns lat/lng.

---

### Passengers Module

#### [NEW] `src/app/[locale]/passengers/page.tsx`
Passenger list with bulk upload/download.

#### [NEW] `src/app/[locale]/passengers/[id]/page.tsx`
Passenger profile: personal info form, photo upload, home location map, route assignments.

#### [NEW] `src/components/passengers/AssignRoutesDialog.tsx`
Multi-row assignment dialog with edit-in-place, change detection, per-row save.

---

### Vehicles Module

#### [NEW] `src/app/[locale]/vehicles/page.tsx`
Card grid, color-coded by approval+active status (green/orange/red).

---

### Shifts Module

#### [NEW] `src/app/[locale]/shifts/page.tsx`
Horizontal band per shift group, day toggle buttons (orange=active), time pickers, "Set time for all" bulk action.

---

### Exceptions Module

#### [NEW] `src/app/[locale]/exceptions/page.tsx`
Table + Add/Edit dialog with mutually exclusive Type/Location radio groups, capacity popup.

---

### Suppliers & Financials Module

#### [NEW] `src/app/[locale]/suppliers/page.tsx`
Card grid, filter drawer.

#### [NEW] `src/app/[locale]/suppliers/[id]/page.tsx`
Supplier profile with live duplicate checking, contact persons, logo upload.

#### [NEW] `src/app/[locale]/account-statement/page.tsx`
Financial statement table with all row actions: Payment, Deduction, View Payments, Print Invoice (Arabic PDF print preview).

---

### Repricing Module

#### [NEW] `src/app/[locale]/repricing/page.tsx`
List table + Create screen + Review popup (Before → After price table, Approve/Reject).

---

### Deductions Module

#### [NEW] `src/app/[locale]/deductions/page.tsx`
Deductions list + Add/Edit dialog with live calculation.

---

### Shared UI Components

#### [NEW] `src/components/ui/Button.tsx`
Brand/Secondary/Success/Danger/Warning/Ghost/Dark × xs/sm/base/l/xl × all states.

#### [NEW] `src/components/ui/Badge.tsx`
Status badges matching the color-semantic table.

#### [NEW] `src/components/ui/DataTable.tsx`
Reusable table: sticky header, pagination, Excel export button, role-gated actions column, empty/loading states.

#### [NEW] `src/components/ui/Combobox.tsx`
Master searchable dropdown: default/open/loading/empty/disabled-with-reason/selected states.

#### [NEW] `src/components/ui/Dialog.tsx`
Modal wrapper: focus trap, backdrop, success/error toasts.

#### [NEW] `src/components/ui/ConfirmDialog.tsx`
Destructive action confirmation (delete, approve, reject).

#### [NEW] `src/components/ui/Toast.tsx`
Success/error/info toast system.

---

### API Layer

#### [NEW] `src/services/api.ts`
Axios instance with base URL (env var), auth token interceptor, 401 → login redirect.

#### [NEW] `src/services/lines.ts`, `routes.ts`, `passengers.ts`, `vehicles.ts`, `shifts.ts`, `suppliers.ts`, `payments.ts`, `deductions.ts`, `repricing.ts`, `exceptions.ts`, `attendance.ts`
Typed service modules with placeholder implementations (return mock data). Each function has a `TODO: replace with real API call` comment and the expected endpoint documented.

---

## Verification Plan

### Automated
- `npm run build` — TypeScript compile check, no type errors
- `npm run lint` — ESLint clean

### Manual
- Login screen → form validation, loading state, role-based redirect
- Dashboard → KPI cards display, route rail filters, role-based button visibility
- Lines → add line dialog, card grid, navigate to routes
- Route detail → add station (with map picker), add passenger, delete with confirm
- Passenger → profile form, multi-row route assignment
- Repricing → create, pending state, approve/reject by role
- Role switcher → verify menu items and action buttons change per role
- RTL layout → verify Arabic text direction, mirrored icons, correct alignment
- LTR toggle → verify English layout mirrors correctly

---

> [!IMPORTANT]
> **Start-up order:** Design tokens CSS → Layout shell → Auth → Dashboard → then modules in order.
> The project directory `d:\Francois\11.Garas Employee Transportation Management System` already exists and contains only the documentation file.
