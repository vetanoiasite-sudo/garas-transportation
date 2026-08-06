# Garas Transportation — ASP.NET Core Backend

A **.NET 8 (ASP.NET Core Web API + EF Core + SQL Server)** port of the NestJS
`backend/`, itself a port of the legacy `NewGarasAPI` CoreApi. Same stack as the
original .NET source (net8.0, EF Core 8, SQL Server) and **byte-for-byte faithful
to the HTTP contract** the Flutter/Next clients depend on:

- Response envelope `{ Result, Errors, Data, PaginationHeader }` with **PascalCase**
  keys, including the frozen intentional misspellings (`EmplyeeId`, `FromoDate`,
  `ActualUserAttedance`, `Longtitud`, `FirstNane`, `christianNum`, …).
- Header-based auth (`CompanyName` + `UserToken`), header filters/pagination
  (`PageNo`/`NoOfItems`/…), POST + `Id` header for deletes/approvals.
- Error codes returned at HTTP 200 (`Err-P200`, `Err-P2`, `Err101`, `Err10`, …).
- Session token = AES-256-CBC(sessionId) — **wire-compatible with the NestJS crypto**.

## Requirements

- .NET 8 SDK (builds on the .NET 10 SDK too; the EF CLI needs
  `DOTNET_ROLL_FORWARD=LatestMajor` if only newer runtimes are installed)
- SQL Server (local instance, LocalDB, or a container)

## Configuration

`appsettings.json` (override via env vars):

| Setting                     | Env var             | Default                                   |
|-----------------------------|---------------------|-------------------------------------------|
| `ConnectionStrings:Default` | `DATABASE_URL`      | `Server=localhost;Database=GarasTransport;Trusted_Connection=True;TrustServerCertificate=True;` |
| `AllowedCompanies`          | `ALLOWED_COMPANIES` | `demo`                                    |
| `TokenSecret`               | `TOKEN_SECRET`      | `garas-dev-secret-change-me-please-32b`   |
| `SessionHours`              | —                   | `24`                                      |
| —                           | `PORT`              | `4000`                                    |
| —                           | `SKIP_DB_INIT`      | (unset) — set `true` to skip migrate+seed |

## Run

```bash
cd backend-dotnet
dotnet run
```

On startup the app **applies migrations and seeds** the roles, one login per panel
role, three supervisors, and the C/M lookup (idempotent). It then listens on
`http://0.0.0.0:4000`. Swagger UI is at `/swagger` in Development.

Demo logins (all password `demo1234`, `CompanyName: demo`):
`admin@garas.co` (Super Admin), `transport@garas.co` (Admin), `hr@garas.co`
(HR Admin), `viewer@garas.co` (Reader).

```bash
curl -X POST http://localhost:4000/User/Login \
  -H "Content-Type: application/json" \
  -d '{"Email":"admin@garas.co","Password":"demo1234","CompanyName":"demo"}'
```

## Migrations

```bash
dotnet ef migrations add <Name>
dotnet ef database update
```

## Layout

```
Program.cs              Host, DI (auto-registers *Service), JSON (PascalCase), CORS, migrate+seed
Common/                 Envelope, TokenCrypto, HeaderAuth filter, ApiControllerBase, exception middleware, Fmt (ISO/Excel)
Data/                   AppDbContext (EF Core), DbInitializer (seed), Migrations/
Entities/               24 entities mirroring the Prisma schema
Modules/User/           Login/Logout/GetUserData + Users admin
Modules/Transportation/ 14 modules: Lines, Routes, Stations, Passengers, Vehicles, Shifts,
                        Exceptions, Deductions, Repricing, Suppliers, Costs, Dashboard, Excel, Notifications
```

Excel export/import uses **ClosedXML** (replacing exceljs); passwords use
**BCrypt.Net-Next** (replacing bcryptjs).
