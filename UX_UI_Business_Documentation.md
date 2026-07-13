# 🚌 Employee Transportation Management System
## UX/UI Business Documentation
### For Design Team — New Design Refactor Reference

> **Document Purpose:** This document is the single source of truth for the UX/UI team to understand the Transportation system's business logic, user flows, and screen-by-screen requirements before designing or redesigning any interface component.

---

## Table of Contents

1. [System Overview](#1-system-overview)
2. [User Roles & Permissions](#2-user-roles--permissions)
3. [Navigation & Information Architecture](#3-navigation--information-architecture)
4. [Authentication Module](#4-authentication-module)
5. [Dashboard Module](#5-dashboard-module)
6. [Lines, Routes & Stations Module](#6-lines-routes--stations-module)
7. [Passengers Module](#7-passengers-module)
8. [Mobile Supervisor App — Attendance](#8-mobile-supervisor-app--attendance)
9. [Vehicles Module](#9-vehicles-module)
10. [Shifts (Working Days & Hours) Module](#10-shifts-working-days--hours-module)
11. [Exceptions Module](#11-exceptions-module)
12. [Suppliers & Financials Module](#12-suppliers--financials-module)
13. [Costs, Repricing & Deductions](#13-costs-repricing--deductions)
14. [Global UX Patterns & Components](#14-global-ux-patterns--components)
15. [Business Rules Quick Reference](#15-business-rules-quick-reference)
16. [Known UX Issues to Fix in the Redesign](#16-known-ux-issues-to-fix-in-the-redesign)

---

## 1. System Overview

### What is this system?

An **Employee (Staff) Transportation Management System** inside a larger ERP/HR platform. It helps an organization that buses its employees to/from work using **contracted external suppliers**:

- Plan **transportation lines → routes → pickup stations**
- Assign **employees (passengers)** to routes and stations
- Capture **daily GPS-backed attendance** (bus + passenger check-in/check-out) via a mobile app
- Settle money with **transportation suppliers** — rounds driven, deductions, taxes, payments, Arabic invoices
- Give management a **KPI dashboard** (attendance %, utilization, costs)

### Who uses it?

| User Type | Where They Work | Primary Goal |
|---|---|---|
| System Admin | Office (web) | Full control; approves lines, vehicles, repricing |
| Transportation Super Admin | Office (web) | Runs the whole transportation operation; approves & rejects |
| Transportation Line Admin | Office (web) | Day-to-day setup: lines, routes, stations, passengers, payments |
| Transportation Admin | Office (web) | Creates suppliers, lines, routes |
| Bus Supervisor | On the bus (mobile) | Takes daily bus + passenger attendance with GPS |
| Reader / Passenger role | Web | Read-only views |

> [!IMPORTANT]
> **Two very different surfaces:**
> - **Web** = the full administrative back office (11 sub-modules, data-dense tables and forms).
> - **Mobile** = ONE focused workflow: the bus supervisor sees *their* routes and takes attendance. Nothing else.
>
> Passengers are **employees**, not customers — there is no rider-facing app, no tickets, no fares. Drivers are **supplier contact persons** (data records, not logins).

> [!IMPORTANT]
> The system is **Arabic-first (RTL)**. Every screen must be designed RTL-first with an LTR variant. Printed invoices are Arabic documents with company logo and signature lines.

---

## 2. User Roles & Permissions

Role-Based Access Control. Menus, buttons, and actions are shown/hidden by role — a permissions-aware sidebar and per-action visibility are required.

### Role Matrix

| Feature / Action | System Admin | Super Admin | Line Admin | Trans. Admin | Supervisor | Reader |
|---|:---:|:---:|:---:|:---:|:---:|:---:|
| View dashboard, statements, reports | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Add/Edit/Delete line, route, station, passenger assignment | ✅ | ✅ | ✅ | ➕ add only | ❌ | ❌ |
| **Approve line** | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| Add/Edit vehicles & types | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Approve vehicle** | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| Manage shifts (working hours) | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| Add payment / deduction | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| Create repricing | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Approve repricing** | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Reject repricing** | ❌ | ✅ | ❌ | ❌ | ❌ | ❌ |
| Create supplier | ✅ | ✅* | ❌ | ✅ | ❌ | ❌ |
| Print supplier invoice (PDF) | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| Take Electronic Touch Attendance | ✅ | ✅ | ❌ | ✅ | ❌ | ❌ |
| Mobile attendance (own routes only) | ❌ | ❌ | ❌ | ❌ | ✅ | ❌ |

\* via a separate "Add Supplier" permission flag.

### UX Design Implications

> [!IMPORTANT]
> - **Reject is rarer than Approve**: only the Super Admin can reject a repricing. Design Approve/Reject as clearly distinct actions with different visual weight.
> - **Approval ≠ Active**: vehicles and lines have BOTH an approval state (workflow) and an active state (usability). These are independent and must both be visible at a glance.
> - The Supervisor role sees **only the mobile attendance flow**, never the admin panel.

---

## 3. Navigation & Information Architecture

### Web Sidebar Menu Structure

```
🚌 Transportation
   ├── 📊 Dashboard (KPIs + route rail)
   ├── 💸 Deductions
   ├── 🧾 Suppliers Account Statement
   ├── 👥 Employees (passengers)          [admin roles only]
   ├── 👤 Users                            [admin roles only]
   ├── 🔁 Exceptions
   ├── 🏷️ Repricing
   └── ⚙️ Transportation Management        [admin roles only, collapsible]
        ├── Shifts (Working Days & Hours)
        └── Supervisor & Admin (user management)
```

### Screen Flow Summary (Web)

```
[Login] ──► [Dashboard]
              ├──► [Attendance Report] ──► [Passenger History (dialog)]
              ├──► [Line Cost Report]
              ├──► KPI cards ──► [Lines] / [Routes] / [Vehicles] / [Employees] / [Suppliers]
[Lines] ──► [Routes of a line] ──► [Route Detail: Stations + Passengers]
              ├──► [Add/Edit Route (dialog)]
              ├──► [Add/Edit Station (dialog + map picker)]
              └──► [Assign Passenger (dialog)]
[Passengers] ──► [Passenger Profile] ──► [Assign Routes (dialog, multi-row)]
[Suppliers] ──► [Supplier Profile]
[Account Statement] ──► [Month Details] / [Add Payment] / [Add Deduction]
              └──► [All Payments] ──► [Advance Distribution]
[Repricing] ──► [Create Repricing] ──► [Review & Approve/Reject (dialog)]
[Exceptions] ──► [Exception Form (dialog)] ──► [Capacity Popup]
[Vehicles] / [Shifts] / [Deductions]: list ──► add/edit dialog
```

### Screen Flow Summary (Mobile — Supervisor)

```
[Login] ──► [My Routes] ──► [Route Attendance]
                                ├──► Period toggle (Go / Return)
                                ├──► [Bus Attendance (dialog)]
                                ├──► Passenger Check-In / Check-Out (GPS)
                                └──► [Map View of stations]
```

### Domain Model (for the designer's mental map)

```
Line ──< Route >── Supplier ──< Contact Person ("Driver")
          ├── Vehicle (Vehicle Type, capacity)
          ├── Supervisor (an HR user)
          ├──< Station (pickup point, lat/lng)
          └──< Passenger assignment (employee @ station, period, validity dates)
Route ──< Daily Round ── Attendance events (GPS check-in/out)
Supplier ──< Monthly Account ── Payments / Deductions / Repricing
Exception = temporary per-employee change of route/station/period/location
```

**Vocabulary the UI must use consistently:**

| Term | Meaning |
|---|---|
| **Line** | Top-level grouping of routes |
| **Route** | One contracted bus trip on a line (has a bus **Serial**) |
| **Station / Direction** | A pickup/drop point on a route |
| **Period** | Trip direction: **Go / Return / Both** |
| **Round** | One billed trip: full / half-go / half-return |
| **Exception** | Temporary per-employee route or station change |
| **Advance vs Normal** | Prepaid installment payment vs single monthly payment |

---

## 4. Authentication Module

### 4.1 Login Screen

**Purpose:** single entry point for all staff (web and mobile).

**Business Logic:**
- User signs in with credentials; the server returns a session **token** plus the user's **roles** and **branch**
- All permissions (Section 2) are computed from the role list at login — the UI adapts immediately
- If the session expires, ANY action can bounce the user back to the login screen — design a graceful "session expired, please sign in again" state, not a hard crash
- "Remember me" exists in the current app

**UX Design Notes:**
- On success: web → Dashboard; mobile supervisor → My Routes
- Loading state on the Sign In button
- Error states: invalid credentials (below form), account has no transportation permissions (explanatory message), server unreachable (toast)

> [!NOTE]
> The platform is **multi-branch**. The user's branch scopes some data (notably Shifts). If branch switching is designed, it must be visible in the app chrome.

---

## 5. Dashboard Module

### 5.1 Overview Screen (Web)

**Purpose:** landing page for admins — operational snapshot of the whole transportation operation.

**Layout:** left **route rail** (~15% width, scrollable list of all routes; clicking one filters the KPIs) + main **KPI grid**.

**Filter Bar (cascading):**

| Filter | Behavior |
|---|---|
| Supplier | Selecting clears the Driver filter |
| Driver | **Disabled until a Supplier is chosen** (currently shows an error snackbar — redesign as a disabled state with helper text) |
| Bus Serial | Digits only |
| Date | Single date picker |

### 5.2 KPI Cards

| Card | Value | Navigates to |
|---|---|---|
| 🚏 Transportation Lines | count of lines | Lines list |
| 🚌 Routes / Vehicles | count | Routes list |
| 🚐 Vehicle Types | count | Vehicles list |
| 🧑‍💼 Employees | riders / all employees | Employees list |
| 🤝 Suppliers | active / total | Suppliers list |
| ✅ Vehicle Attendance | check-in, check-out, one-way counts | — |
| 📈 Vehicle Attendance % | 3 percentages (1 decimal) | — |
| 🙋 Users Attendance | count | — |
| 📊 Users Attendance % | percentage | — |
| ↔️ Routes | round-trip vs one-way counts | — |

Header actions: **Line Cost Report**, **View Attendance**, **Take Electronic Touch Attendance** (admin-gated).

### 5.3 Attendance Report Screen

High-frequency table used by admins to check who actually rode the bus.

**Filters:** line, supplier, driver (needs supplier), route (auto-fills serial), serial, from/to dates, plus two checkboxes: **Attended** / **Absent**.

**Columns:** # · Name · ID Code · Another Identifier · Line · Route · Supervisor · Supplier · Driver · **Attendance**

**Attendance cell logic:**
- Shows latest check-in/check-out times, **black text = attended**, **red = absent**
- If the passenger has multiple attendance records in the range → expandable → opens the **Passenger History dialog** (table: date, check-in, check-out, attended/absent badge)

**Actions:** "Download Excel" — exports the filtered result (server-generated file, opens in new tab).

> [!TIP]
> Design this table for scanning speed: sticky header, zebra rows, clear attended/absent color semantics (align with the status badge system in §14.3).

---

## 6. Lines, Routes & Stations Module

### 6.1 Lines List

**Purpose:** manage the top-level groupings of routes.

- Card grid; each card: line name, bus icon, **routes count** (or "Assign to Route" if none)
- Card actions: Update (role-gated), View Routes, Add Route
- Add-new-line card visible only to admin roles

#### Add / Edit Line (dialog)

| Field | Type | Required | Notes |
|---|---|---|---|
| Line Name | Text | ✅ | The only editable field |

- Edit mode adds: **Delete** (confirmation dialog) and **Approve** (Super/System admin, visible only when not yet approved)

### 6.2 Routes List

- Card grid, optionally scoped to a line (title shows the line name)
- Card content: route name, supervisor, **full capacity / users in route**, **actual capacity / attended**, station count
- Card actions: **Actions** (opens route detail; role-gated) · **Route Details** (info popup)

#### Route Details Popup (read-only)

Shows: supplier, driver, creation date, capacities, arrival/departure times, religion counts (M:/C: chips), **exceptions from/to other lines**, station count, supervisor.

### 6.3 Add / Edit Route Form (dialog)

**Business logic — route type drives the time fields:**

```
[✓] Round trip (ذهاب وعودة)  → oneWay = false, BOTH From & To times required
[✓] Go only (ذهاب)           → oneWay = true, clears To time, From required
[✓] Return only (عودة)       → oneWay = true, clears From time, To required
(The three checkboxes are mutually exclusive — design as a segmented control)
```

| Field | Type | Required | Notes |
|---|---|---|---|
| Transportation Line | Dropdown OR free text | ✅ | A checkbox toggles "choose gathering station" mode |
| Route Name | Text | ✅ | |
| Shift | Dropdown | ✅ | From Shifts module |
| Cost | Number (decimal) | ✅ | The contracted price per round |
| Supplier | Dropdown | ✅ | |
| Driver (contact person) | Dropdown | ❌ | **Disabled until Supplier chosen** |
| Supervisor | Dropdown (HR users) | ❌ | The mobile-app attendance taker |
| Vehicle | Dropdown | ✅ | Shows type + capacity |
| Route type | 3 exclusive checkboxes | ✅ | See above |
| From / To Time | Time picker (12h) | Conditional | Per route type |

Edit mode adds **Delete** with confirmation.

### 6.4 Route Detail Screen

The workhorse screen for setting up a route. Header: line name, arrival/departure times, supplier, driver, supervisor, religion counts. Two side-by-side panels:

**Stations panel:**
| Column | Notes |
|---|---|
| # · Station Name · Description/Time | |
| Lat/Lng | 📍 view-on-map button + 📋 copy-to-clipboard + tooltip |
| ✏️ Edit · 🗑 Delete | Delete confirms first |

**Passengers panel:**
| Column | Notes |
|---|---|
| # · Passenger Name · Station | |
| ✏️ Edit · 🗑 Delete | Delete confirms first |

Header actions: **Add Station**, **Add Passenger**, **Update Route**.

#### Add / Edit Station (dialog)

| Field | Type | Required | Notes |
|---|---|---|---|
| Station Name | Text | ✅ | |
| Description / Time | Text | ❌ | Often used for pickup time notes |
| Active | Checkbox | ✅ | |
| Latitude / Longitude | Decimal inputs | ❌ | OR use **Pick Location** map dialog |

**UX Design Note:** the map picker is central to this flow — a full-size map dialog with a draggable pin, returning lat/lng into read-only fields.

#### Assign Passenger to Route (dialog)

| Field | Type | Required |
|---|---|---|
| Passenger | Dropdown (HR employees, searchable) | ✅ |
| Station | Dropdown (route's stations) | ❌ |
| Period | Dropdown: Go / Return / Both | ✅ |

---

## 7. Passengers Module

### 7.1 Passenger Profile (Create / View / Edit)

**Purpose:** register an employee as a transportation passenger.

**Section A — Personal Information**
| Field | Type | Required | Notes |
|---|---|---|---|
| Name | Text | ✅ | |
| Identity Number | Text | ✅ | |
| Mobile | Tel (digits only) | ✅ | |
| Another Identifier | Dropdown | ✅ | (marital-status style lookup) |
| Photo | Image upload | ❌ | Owner can edit their own photo |
| Home Location | Map picker → read-only Lat/Lng | ❌ | Where the employee lives |

**Header actions:**
- Active/Inactive toggle
- 📥 **Download Excel template** / 📤 **Upload Excel** — bulk load passengers WITH their route assignments (success toast on completion)

### 7.2 Assign Routes to Passenger (dialog, multi-row)

The passenger-side of assignment — an employee can ride **multiple routes** with validity windows.

```
┌──────────────────────────────────────────────────────────┐
│  Assign Routes to Passenger                        [✕]  │
├──────────────────────────────────────────────────────────┤
│  [+ Add Route]                                           │
│  ┌────────────────────────────────────────────────────┐  │
│  │ Route* ▾   Station ▾   Period* ▾                  │  │
│  │ From Date 📅   To Date 📅                          │  │
│  │ Lat [____] Lng [____] [📍 Pick]   [Edit] [Delete] │  │
│  └────────────────────────────────────────────────────┘  │
│  ┌── existing saved row (read-only until "Edit") ─────┐  │
│  └────────────────────────────────────────────────────┘  │
│                                          [Submit]        │
└──────────────────────────────────────────────────────────┘
```

**Business Logic:**
- **Saved rows are read-only** until the user clicks *Edit*; the Submit button on a row enables **only when something actually changed** (change detection)
- Only **new rows** are posted on the dialog-level Submit; per-row edits save individually
- Deleting a saved row calls the server; deleting an unsaved row just removes it
- Custom pickup coordinates per assignment override the station location

**Validation:** Route required ("please select route"), Period required ("please select period"); dates and coordinates optional.

---

## 8. Mobile Supervisor App — Attendance

> [!IMPORTANT]
> This is the **single most important operational flow** in the system — the equivalent of the "New Loan" flow in a library. It happens on a moving bus, possibly in sunlight, with one hand. Design for **large touch targets, minimal steps, and offline-tolerant feedback**.

### 8.1 My Routes Screen

- Lists ONLY the routes this supervisor supervises
- Card: line name, supplier, supervisor, driver, bus image
- Empty state: *"This user has no routes to supervise"*

### 8.2 Route Attendance Screen

```
┌────────────────────────────────────┐
│ ← Back      [Line Name]   🚌  🗺  │
│  Period: [ Go ▾ ]                  │
├────────────────────────────────────┤
│  👤 Ahmed Mohamed                  │
│     a.mohamed@co.com               │
│     0100 123 4567                  │
│              [ ✅ Check In ]       │
├────────────────────────────────────┤
│  👤 Sara Ali                       │
│     checked in 07:42 AM            │
├────────────────────────────────────┤
│  ...                               │
└────────────────────────────────────┘
```

**Business Logic (step by step):**

```
1. Screen opens with Period = "Go" (mobile NEVER shows "Both")
2. GO period → each passenger row shows:
   - [Check In] button if not yet checked in
   - the recorded time (no button) if already checked in  ← prevents duplicates
3. RETURN period → row shows check-in status + [Check Out] button or recorded time
4. Tapping Check In/Out:
   a. Requests device GPS (permission flow)
   b. Records the event with coordinates (0.0 if GPS unavailable)
   c. Row instantly switches from button → recorded time
5. 🚌 Bus attendance button → confirmation dialog → records the BUS's own
   check-in/out against the bus serial (same GPS capture)
6. 🗺 Map button → full-screen map plotting all passengers' station pins
```

**Error/edge states to design:**

| Condition | UI |
|---|---|
| GPS permission denied | Explain why location is needed; retry affordance |
| GPS unavailable | Event still records (coords 0.0) — show a subtle "no location" indicator |
| Already checked in | Time shown instead of button (no error needed) |
| No network on the bus | Currently fails silently after retries — design an explicit failure + retry state |

> [!WARNING]
> In the current app, the bus-attendance dialog's buttons are **swapped**: "Attended" triggers check-OUT and "Leave" triggers check-IN. The redesign must use unambiguous labels: **"Bus Check-In (start of trip)"** and **"Bus Check-Out (end of trip)"**.

---

## 9. Vehicles Module

### 9.1 Vehicles List

Card grid. **Card color IS the status system:**

| Color | Meaning |
|---|---|
| 🟢 Green | Approved + Active |
| 🟠 Orange | Approved + Inactive |
| 🔴 Red | **Not approved** |

Card shows: vehicle type, capacity (seats), approval icon + text, active icon + text.

### 9.2 Add / Edit Vehicle (dialog)

| Field | Type | Required |
|---|---|---|
| Capacity (seats) | Number | ✅ |
| Vehicle Type | Dropdown | ✅ |
| Active | Checkbox | ❌ |

- Inline "+ Add Vehicle Type" opens a mini-dialog (type name only; created active)
- Edit mode: **Delete** (confirm) and **Approve** (Super/System admin, only when unapproved)

> [!NOTE]
> There is **no plate number field** — vehicles are identified by type + capacity, and routes carry the bus **serial**.

---

## 10. Shifts (Working Days & Hours) Module

**Purpose:** define per-branch weekly time windows that routes reference.

**Business Logic:**
- A **shift group** = 7 weekday slots (Sunday=1 … Saturday=7)
- Default window **12:00–18:00**, all days inactive by default
- **Only active days are saved**; "deleting" a group deactivates all its days
- Groups are auto-numbered (Shift 1, Shift 2, …); branch-scoped

### Screen: Working Days (list)

Horizontal band per shift group: shift number + 7 day columns (day name button — **orange = active, gray = inactive** — with From/To times below in 12h format), delete button per group.

### Add / Edit Shift (dialog)

- Tap a day to toggle active
- Tap From/To to open a time picker per day
- **"Set time for all"** row applies one window across the whole group — keep this bulk affordance prominent
- Cancel / Submit

---

## 11. Exceptions Module

**Purpose:** temporary, per-employee deviations — e.g., "this week Ahmed rides Route B from a different pickup point."

### 11.1 Exceptions List

Table: route, passenger, exception station, lat/lng, exception date, from/to dates, period, weekdays, reason, contact number. Add/Edit gated to admin roles.

### 11.2 Exception Form (dialog)

**Business Logic — two mutually exclusive TYPE choices and two mutually exclusive LOCATION choices:**

```
TYPE:      (•) Single Exception Date [📅]
           ( ) Period → From 📅  To 📅  + Weekdays ▾

LOCATION:  (•) Station → pick from the target route's stations
           ( ) Free Location → map picker → lat/lng

PERIOD:    Go / Return / Both
           "Both" reveals a SECOND location block (get-out point)
```

| Field | Required | Validation |
|---|---|---|
| Passenger | ✅ | "please select passenger" |
| Route | ✅ | "please select route"; station dropdown disabled until chosen |
| Period | ✅ | "please select period" |
| Reason | ✅ | Multiline |
| Contact Number | ✅ | Digits only |

**Route Details / Capacity button** (enabled once route + period chosen) opens the Capacity popup:

```
┌──────────── Route Capacity ────────────┐
│  Full Capacity: 40   Actual: 36        │
│  From other lines: +3   To others: −1  │
│  Capacity without exceptions: 34       │
└────────────────────────────────────────┘
```

> [!NOTE]
> Capacity is **informational only** — the system does NOT block over-capacity assignments. Design it as guidance (e.g., a gauge), not a gate.

---

## 12. Suppliers & Financials Module

### 12.1 Suppliers List

Card grid (name, creation date, logo) + filter drawer (name / phone / mobile). "Add new supplier" gated by permission.

### 12.2 Supplier Profile (Add / View)

- **Live duplicate checking** on name, email, phone, mobile, fax — each field has a check indicator (🔴 duplicate exists / 🔵 unique / ⚪ unchecked) and a dialog listing conflicting suppliers
- Mobile numbers get a country-code picker (default +20)
- Contact persons = the **drivers** shown on routes — make this relationship explicit in the UI ("Contact persons appear as Drivers on routes")
- Logo upload (square)
- Two-phase save in the current app (supplier, then contacts) — redesign as one perceived action with a single loading state

### 12.3 Suppliers Account Statement (the money screen)

**Purpose:** one row per supplier per month — what is owed, what was deducted, what was paid, what remains.

**Filters:** Supplier ▾ · Route ▾ · Month (1–12) · Year (year picker)

**Columns:**
| Column | Meaning |
|---|---|
| Month | |
| Supplier | |
| Routes # | routes billed that month |
| Rounds | full / half-go / half-return counts |
| Total Due | Σ rounds × prices |
| Total Deductions | taxes + normal |
| Normal Payments | monthly payments received |
| Advance Payments | installments applied to this month |
| **Remaining** | due − deductions − payments |

**Row actions:** 📋 Details · ➕ Add Payment* · ➖ Add Deduction* · 👁 View Payments · 🖨 **Print Invoice*** (*admin-gated)

**Print Invoice** generates an **Arabic PDF**: company logo, trips table, financial summary (Total Due / Total Deductions / Taxes / Normal Deduction / Advance Payment / Normal Payment / Remaining), notes, supplier + company signature lines. The redesign should include a print-preview state.

### 12.4 Month Details (dialog)

Daily rounds for a supplier-month: date, route, check-in time, check-out time, one-way time, rounds count, total price of that day. Route filter chips across the top.

### 12.5 Add Payment (dialog)

```
┌────────────────────────────────────────┐
│  💰 Payment — [Supplier Name]    [✕]  │
├────────────────────────────────────────┤
│  Amount:        [________]             │
│  Payment Date:  [📅 2026-07-05]        │
│                                        │
│  (•) Advance Payment (installments)    │
│  ( ) Monthly Payment                   │
│                                        │
│  ── if Advance ──                      │
│  Number of Months: [__]  (max 12)      │
│  Start Date:       [📅]                │
│                                        │
│                      [ Pay ]           │
└────────────────────────────────────────┘
```

**Business Logic:**
- **Advance** = prepayment spread over N months (**N ≤ 12**) from a start month → the server produces a per-month distribution
- **Normal** = a single monthly payment
- After saving, the statement refreshes and the month's Remaining drops

### 12.6 All Payments (dialog)

Table: amount, payment date, start date, months, type (Advance/Normal). **"Show Details"** appears only on Advance rows → opens the **Distribution dialog** (month, year, payment amount per month).

---

## 13. Costs, Repricing & Deductions

### 13.1 Line Cost Report

**Filters:** line, supplier, driver (needs supplier), route (auto-fills serial), serial, from/to dates — **defaults to the current calendar month**.

**Columns:** line · route · supplier · cost · one-way · rounds # · deducted rounds # · deduction total · **Net Cost**

`Net Cost = line cost − (deducted rounds × deduction per round)` — server-computed.

**Action:** Download Excel.

### 13.2 Repricing (approval workflow)

**Purpose:** bulk price changes to contracted route costs — with a maker/checker approval flow.

#### Create Repricing screen

| Control | Options |
|---|---|
| Scope | **All lines** vs **Selected lines** (picker adds rows to a table) |
| Mode | **Percent %** vs **Fixed amount $** |
| Rounding | Round result to nearest 5: on/off |
| Amount | Number input (icon reflects current mode) |
| Start Date | Date picker — when new prices take effect |

- Preview table shows affected lines (all lines paginated, or the selected set with remove buttons)
- Save → **confirmation dialog** (this changes contract prices!) → record created **unapproved**
- Error if "Selected lines" mode with an empty selection

#### View Repricing list

Table: increase amount, created by, creation date, **Approved** (green/red), approved by (or "pending"), for-all-lines flag. Filter checkboxes: approved-only / not-approved.

#### Review popup (the approval moment)

- All-lines: message "all transportation lines will be increased by {amount}"
- Selected lines: table of **Before (red) → After (green)** prices per line
- If unapproved AND the viewer can approve: **[Approve]** (applies prices, stamps approver+date) and **[Reject]** (Super Admin only, discards)

> [!IMPORTANT]
> **Prices never change without approval.** The design must make the pending state impossible to miss (e.g., amber banner on unapproved records).

### 13.3 Deductions

**Purpose:** money subtracted from a supplier's monthly dues (penalties, taxes).

#### Deductions list
Table: route, supplier, driver, deduction day, amount, created by, creation date, reason. Add/Edit admin-gated.

#### Add / Edit Deduction (dialog)

| Field | Type | Required | Notes |
|---|---|---|---|
| Supplier | Dropdown | ✅ | Choose first |
| Route | Dropdown | ✅ | **Auto-fills Serial + Route Price (read-only)** |
| Type | 2 exclusive checkboxes | ✅ | **Normal deduction** (خصم عادي) vs **Tax** (ضريبة) |
| Percentage mode | Checkbox | ❌ | Reveals Percent field |
| Percent | Number | Conditional | **Live-computes:** deduction per round = price × % ÷ 100 |
| Deduction per (half) round | Number | ✅ | Read-only in percentage mode |
| Date | Date picker | ✅ | |
| Reason | Text | ✅ | |

On save: deductions list AND the supplier's month statement both refresh — the deduction immediately reduces Remaining.

---

## 14. Global UX Patterns & Components

### 14.1 Design Direction

- **RTL-first.** Arabic is the primary language; every layout must mirror correctly. Use a font pairing with excellent Arabic support.
- **Navy-centric light UI** (matches the existing brand): navy for primary actions/navigation, white/off-white surfaces, green reserved for success/approve/money-in, red for danger/absent/money-out, orange/amber for pending/warning states.
- **Data-dense but calm:** this is a back-office tool used all day. Prefer generous row height toggles, sticky headers, and column alignment over decoration.
- **Two design kits:** Web admin (desktop, mouse) and Mobile supervisor (one-handed, outdoor, large targets).

### 14.2 Data Tables (Reusable)

```
┌─────────────────────────────────────────────────────────┐
│  [Screen Title]  (count)                  [+ Add New]   │
├─────────────────────────────────────────────────────────┤
│  Filter ▾  Filter ▾  [serial]  [📅 from] [📅 to]  [⬇ Excel] │
├────┬──────────────┬────────────┬──────────┬─────────────┤
│ #  │ Main Column  │ ...        │ Status   │ Actions     │
├────┼──────────────┼────────────┼──────────┼─────────────┤
│ 1  │ ...          │ ...        │ 🟢       │ 👁 ✏️ 🗑    │
└────┴──────────────┴────────────┴──────────┴─────────────┘
│  Items per page [20 ▾]      [< 1 2 3 … n >]             │
└─────────────────────────────────────────────────────────┘
```

**Standard features:** pagination (default 20/page) · horizontal scroll for wide tables · Excel export where applicable · empty state · loading state · role-gated action column.

**Cascading filters pattern (used everywhere):** Supplier → Driver → Serial/Route. Changing a parent clears its children. Design dependent dropdowns as visibly disabled with helper text ("Select a supplier first") instead of error snackbars.

### 14.3 Status & Color Semantics (one system, everywhere)

| Status | Color | Use case |
|---|---|---|
| Active / Attended / Available / Approved | 🟢 Green | |
| Inactive (but approved) | 🟠 Orange | Vehicles |
| Not approved / Pending approval | 🔴 Red or 🟡 Amber banner | Vehicles, lines, repricing |
| Absent / Overdue-style | 🔴 Red | Attendance |
| Price before → after | 🔴 → 🟢 | Repricing review |
| Money in (payment) / Money out (deduction) | 🟢 / 🔴 | Statements |

### 14.4 Dropdowns (Reusable)

All lookups share one pattern: search field on top + scrollable list + keyboard navigation (arrows + Enter) + infinite scroll for long lists. Selected value shows a **clear (✕)** affordance in the field. Design one master combobox component with: default, open, loading, empty ("no results"), disabled-with-reason, and selected states.

### 14.5 Forms & Dialogs

- Nearly all CRUD happens in **modal dialogs** over list screens (route form, station form, payment, deduction, exception, shift, vehicle, line)
- Required fields marked `*`; inline validation under the field
- Mutually exclusive checkbox groups (route type, deduction type, exception type/location) should become **segmented controls / radio groups** in the redesign
- **Destructive actions always confirm** (delete route/station/passenger/vehicle/deduction, shift group)
- **Approve/Reject confirmation** must restate what will happen ("Prices of 12 lines will increase by 10% starting 2026-08-01")
- Success/error feedback via toast; failures keep the dialog open with values intact

### 14.6 Maps (Reusable)

Two components:
1. **Map Picker** — dialog with draggable pin; returns lat/lng into read-only fields; used by stations, passenger home/pickup locations, exceptions
2. **Map View** — read-only map plotting station/passenger pins; used from route detail (web) and the supervisor app (mobile)

Include: current-location button, coordinate copy, and a fallback for missing/0.0 coordinates.

### 14.7 Empty States

| Screen | Message |
|---|---|
| Routes list | "No routes yet. Add the first route." |
| Supervisor routes (mobile) | "This user has no routes to supervise" |
| Attendance report | "No attendance records for the selected filters." |
| Statement | "No account activity for this month." |
| Repricing | "No repricing requests." |

### 14.8 Exports & Documents

| Output | Format | Trigger |
|---|---|---|
| Attendance report | Excel (server file, new tab) | Dashboard attendance |
| Line cost report | Excel | Costs screen |
| Passenger bulk template | Excel download → fill → upload | Passenger screen |
| Supplier invoice | **Arabic PDF** (client-rendered) | Statement row |

---

## 15. Business Rules Quick Reference

### Route & Assignment Rules
| Rule | Impact on UI |
|---|---|
| Route type is exclusive: round-trip / go / return | Segmented control; switching clears the irrelevant time field |
| Go = check-in trips; Return = check-out trips; "Both" = both points | Periods drive which buttons appear on mobile |
| **Mobile never offers "Both"** | Mobile period toggle has 2 options only |
| Driver list depends on chosen supplier | Dependent dropdown, disabled until supplier picked |
| Capacity math is server-computed and advisory | Show as info/gauge; never block on it |
| One passenger can be on many routes with validity windows | Multi-row assignment dialog (edit-in-place rows) |

### Attendance Rules
| Rule | Impact on UI |
|---|---|
| An existing check-in shows a time instead of a button | Prevents duplicate capture by design |
| GPS captured per event; 0.0 if unavailable | Subtle "no location" indicator, never a blocker |
| Bus attendance is recorded against the bus **serial** | Separate, clearly-labeled bus check-in/out action |
| Supervisors see only their own routes | No search/browse of other routes on mobile |

### Financial Rules
| Rule | Impact on UI |
|---|---|
| Billing = full rounds + half-go + half-return, each priced | Statement shows the 3 counts separately |
| Advance payment ≤ **12 months**, needs start date | Month input capped; distribution viewable per month |
| Deduction types: Normal vs Taxes (tracked separately) | Exclusive toggle; statement shows both totals |
| Percent deduction = route price × % ÷ 100, per (half) round | Live calculation shown read-only |
| Remaining = due − deductions − payments (server-computed) | Never editable |
| **Repricing requires approval before prices change** | Pending banner; Approve (super/system) vs Reject (super only) |
| Rounding to nearest 5 is optional in repricing | Toggle with example preview |

### Approval Rules
| Rule | Impact on UI |
|---|---|
| Lines & vehicles start unapproved | Red/amber state until approved |
| Approval and Active are independent | Two distinct indicators (never merge them) |
| Approve button appears only when unapproved AND role allows | Role-aware action rendering |

### Validation Rules (client-side)
| Form | Required |
|---|---|
| Line | name |
| Route | name, shift, numeric cost, supplier, vehicle, times per type |
| Station | name |
| Passenger profile | name, identity number, mobile (digits), identifier |
| Assignment | route, period |
| Exception | passenger, route, reason, contact number (digits) |
| Deduction | supplier, route, type, date, reason, amount/percent |
| Payment | amount, date (+ months ≤ 12 and start date if Advance) |
| Supplier | duplicate-checked live on name/email/phone/mobile/fax |

---

## 16. Known UX Issues to Fix in the Redesign

These are confirmed problems in the current implementation — the redesign should explicitly solve them:

1. **Swapped bus-attendance labels (mobile):** "Attended" currently performs check-OUT and "Leave" performs check-IN. Use unambiguous trip-start / trip-end language.
2. **Religion count chips bug:** route detail shows the same value for both "M:" and "C:" chips. Also reconsider whether demographic chips belong at this prominence at all.
3. **Error-snackbar-as-guidance:** dependent dropdowns (Driver before Supplier) throw error snackbars; replace with disabled states + helper text.
4. **Dashboard "Total Capacity" card** binds to the vehicle-type count, not seat capacity — the metric is misleading and needs a real data source or removal.
5. **Deep-link fragility:** several screens depend on hidden state (which route/line/exception was clicked) and break when opened in a new tab; the redesign's navigation should assume every screen is directly addressable (entity ID in the URL).
6. **Mixed CRUD surfaces:** the exception form exists as both a full screen and a dialog with identical content — pick one pattern.
7. **Dense 10-column tables with horizontal scroll** (attendance, statement, exceptions): consider column priority, row expansion, or card-per-row patterns for the redesign.
8. **No loading/failure states on the bus:** mobile attendance fails silently on bad connectivity; design explicit pending/retry affordances.
9. **Legacy disabled fields** (line cost/currency/one-way on the Line entity) still exist in data but not UI — do not design for them.
10. **Inconsistent period vocabulary:** "Get In / Get Out / Both" vs "Go / Return / Both" appear in different screens; standardize the terminology system-wide.

---

*Document created for the UX/UI design team.*
*Based on: docs/PRD.md, docs/SCREENS.md, docs/RULES.md, docs/MODELS.md, docs/PROVIDERS.md, docs/API.md, docs/second-pass/* (as-built reverse-engineering of the Flutter implementation).*
