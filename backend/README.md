# Garas Transportation — Backend (NestJS + Prisma + SQL Server)

A **separate** backend for the Garas Employee Transportation Management System,
replicating the documented CoreApi contract (endpoint names, header-based
filters/pagination, `{ Result, Errors, Data, PaginationHeader }` envelope, and
the `CompanyName` + `UserToken` session auth). Built in **NestJS** to mirror the
old .NET layered design (controllers → services → Prisma data layer).

## Deliberate deviations from the old backend (security only)
- **Passwords are bcrypt-hashed** (not reversibly encrypted).
- **Single-tenant**: the `CompanyName` header is still required and validated
  against `ALLOWED_COMPANIES`, but there is one database (no runtime DB switching).

Everything else — routes, envelope, header conventions, error codes
(`Err-P2`, `Err-P200`, `Err10`, …), frozen JSON key names — stays faithful.

## Architecture
```
src/
  main.ts                      bootstrap (CORS open, global exception filter)
  app.module.ts                wires modules (add one transportation module per phase)
  common/
    response/base-response.ts  envelope + pagination helpers (PascalCase keys)
    filters/                   catch-all → Err10 envelope at HTTP 200
    crypto/token.crypto.ts     AES session-id ↔ UserToken
    auth/header-auth.guard.ts  replicates ValidateHeader (CompanyName + UserToken)
    auth/current-user.decorator.ts
    common.module.ts           global — exports the auth guard
  prisma/                      PrismaService + global module
  modules/
    user/                      /User/Login, /User/Logout, /User/GetUserData
    transportation/            all under /api/Transportation (shifts under /HR/BranchSchedule)
      lines/  routes/  stations/  passengers/  vehicles/  shifts/
      exceptions/  deductions/  repricing/  suppliers/  costs/  dashboard/  excel/
prisma/
  schema.prisma                DB schema (24 models)
  seed.ts                      roles + admin user + sample lines/vehicle-types
```

## Setup
1. Have a **SQL Server** instance available (local, Docker, or Azure SQL).
2. `cd backend && npm install`
3. Copy `.env.example` → `.env` and set `DATABASE_URL`, `TOKEN_SECRET`, `ALLOWED_COMPANIES`.
4. `npm run prisma:generate`
5. `npm run prisma:push`  (or `npm run prisma:migrate` for migration history)
6. `npm run seed`
7. `npm run start:dev`  → `http://localhost:8888`

## Try it
```bash
# Login (no auth headers)
curl -X POST http://localhost:8888/User/Login \
  -H "Content-Type: application/json" \
  -d '{"Email":"admin@garas.co","Password":"demo1234","CompanyName":"demo"}'
# → { "Result": true, "Data": "<UserToken>", "RoleList": [...], ... }

# Protected call (send the token + company as headers)
curl http://localhost:8888/api/Transportation/getAllTransportationLine \
  -H "CompanyName: demo" -H "UserToken: <UserToken>" -H "PageNo: 1" -H "NoOfItems: 20"
```

## Status
- ✅ Foundation: scaffold, envelope, header-auth, exception filter, Prisma (24 models).
- ✅ Auth: `/User/Login`, `/User/Logout`, `/User/GetUserData`.
- ✅ All transportation modules wired — **67 endpoints** mapped across lines,
  routes, stations, passengers, vehicles, shifts, exceptions, deductions,
  repricing, suppliers/payments, costs, dashboard/attendance, Excel.
- `tsc --noEmit` clean · `nest build` green · app boots and maps every route
  (verified up to the DB connect).

### Endpoints intentionally left as stubs (marked `// TODO` in code)
These need the external fingerprint feed and/or an Excel library, so they return
the correct envelope shape but no real payload yet: the Excel import/export
(`InsertUsersWithRoutesExcel`, `AttendanceExcell`, `ReportCostOfLinesExcell`,
template downloads) and the batch attendance sync (`AddUsersAttedance44`). The
deep billing/attendance analytics (round recognition, capacity inflow/outflow,
per-round pricing) return computed-where-possible values with `// TODO` for the
parts that depend on that feed.
