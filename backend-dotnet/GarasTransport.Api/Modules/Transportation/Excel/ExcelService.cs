using System.Text.RegularExpressions;
using ClosedXML.Excel;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Excel;

public class ExcelService
{
    private readonly AppDbContext _db;

    public ExcelService(AppDbContext db) => _db = db;

    private static string FullName(HrUser u) =>
        string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    /// <summary>Build a header-only (.xlsx) template workbook (RTL) and return it base64.</summary>
    private static string HeaderTemplate(string sheetName, string[] headers)
    {
        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet(sheetName);
        sheet.RightToLeft = true;
        for (var c = 0; c < headers.Length; c++)
        {
            sheet.Cell(1, c + 1).Value = headers[c];
            sheet.Column(c + 1).Width = 22;
        }
        sheet.Row(1).Style.Font.Bold = true;
        return Fmt.WorkbookBase64(wb);
    }

    /// <summary>GET downloadExcelUserWithRoutesTemplete — import template (header-only).</summary>
    public Task<object> DownloadUserWithRoutesTemplate()
    {
        var b64 = HeaderTemplate("المستخدمون والخطوط", new[]
        {
            "الاسم", "ID", "رقم الموبايل", "معرّف آخر", "سيريال الخط", "الفترة", "الاتجاه",
        });
        return Task.FromResult<object>(Envelope.Success(b64));
    }

    /// <summary>GET downloadExcelUserActiveTemplete — ACTIVATION sheet pre-filled with every passenger.</summary>
    public async Task<object> DownloadUserActiveTemplate()
    {
        var rows = await _db.HrUsers.OrderByDescending(u => u.Id).ToListAsync();

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("تفعيل الركاب");
        sheet.RightToLeft = true;
        sheet.Cell(1, 1).Value = "ID"; sheet.Column(1).Width = 18;
        sheet.Cell(1, 2).Value = "الاسم"; sheet.Column(2).Width = 24;
        sheet.Cell(1, 3).Value = "نشط (نعم/لا)"; sheet.Column(3).Width = 14;
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var u in rows)
        {
            sheet.Cell(row, 1).Value = u.IdentityNumber ?? string.Empty;
            sheet.Cell(row, 2).Value = FullName(u);
            sheet.Cell(row, 3).Value = u.Active ? "نعم" : "لا";
            row++;
        }
        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>GET downloadExcelUsersList — EXPORT of the current passengers.</summary>
    public async Task<object> UsersListExcel()
    {
        var rows = await _db.HrUsers.Include(u => u.MaritalStatus).OrderByDescending(u => u.Id).ToListAsync();

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("الركاب");
        sheet.RightToLeft = true;
        string[] headers = { "الاسم", "ID", "رقم الموبايل", "معرّف آخر", "نشط" };
        int[] widths = { 24, 18, 16, 12, 8 };
        for (var c = 0; c < headers.Length; c++) { sheet.Cell(1, c + 1).Value = headers[c]; sheet.Column(c + 1).Width = widths[c]; }
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var u in rows)
        {
            sheet.Cell(row, 1).Value = FullName(u);
            sheet.Cell(row, 2).Value = u.IdentityNumber ?? string.Empty;
            sheet.Cell(row, 3).Value = u.Mobile ?? string.Empty;
            sheet.Cell(row, 4).Value = u.MaritalStatus?.Name ?? string.Empty;
            sheet.Cell(row, 5).Value = u.Active ? "نعم" : "لا";
            row++;
        }
        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>Decode a base64 (or data-URI) payload into a loaded worksheet.</summary>
    private static IXLWorksheet LoadWorkbook(string fileBase64)
    {
        // Tolerate a "data:...;base64," prefix if the client sent a data URI.
        var clean = fileBase64.Contains(',') ? fileBase64[(fileBase64.IndexOf(',') + 1)..] : fileBase64;
        var bytes = Convert.FromBase64String(clean);
        using var ms = new MemoryStream(bytes);
        var wb = new XLWorkbook(ms);
        var sheet = wb.Worksheets.FirstOrDefault();
        if (sheet == null) throw new Exception("empty");
        return sheet;
    }

    /// <summary>Split a single "name" cell into the stored First/Middle/Last parts.</summary>
    private static (string FirstName, string? MiddleName, string? LastName) SplitName(string name)
    {
        var parts = Regex.Split(name.Trim(), @"\s+").Where(p => p.Length > 0).ToList();
        var firstName = parts.Count > 0 ? parts[0] : string.Empty;
        if (parts.Count > 0) parts.RemoveAt(0);
        string? lastName = parts.Count > 0 ? parts[^1] : null;
        if (parts.Count > 0) parts.RemoveAt(parts.Count - 1);
        string? middleName = parts.Count > 0 ? string.Join(' ', parts) : null;
        return (firstName, middleName, lastName);
    }

    /// <summary>Map an "other identifier" code (e.g. C / M) to its lookup id.</summary>
    private async Task<Dictionary<string, int>> OtherIdentifierMap()
    {
        var rows = await _db.MaritalStatuses.ToListAsync();
        return rows.ToDictionary(r => r.Name.Trim().ToUpperInvariant(), r => r.Id);
    }

    /// <summary>Normalise a period cell (Arabic or English) to the stored enum value.</summary>
    private static string NormalizePeriod(string raw)
    {
        var v = raw.Trim().ToLowerInvariant();
        if (Regex.IsMatch(v, "^go$|ذهاب")) return "Go";
        if (Regex.IsMatch(v, "^return$|عود")) return "Return";
        return "Both";
    }

    /// <summary>POST InsertUsersWithRoutesExcel — bulk import passengers and (optionally) assign to a route.</summary>
    public async Task<object> InsertUsersWithRoutesExcel(string? fileBase64)
    {
        if (string.IsNullOrEmpty(fileBase64)) return Envelope.Fail("Err101", "File is required");

        IXLWorksheet sheet;
        try { sheet = LoadWorkbook(fileBase64); }
        catch { return Envelope.Fail("Err102", "ملف Excel غير صالح"); }

        var idMap = await OtherIdentifierMap();
        var created = 0;
        var updated = 0;
        var assigned = 0;
        var errors = new List<string>();

        var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;
        for (var i = 2; i <= lastRow; i++)
        {
            string Cell(int n) => sheet.Cell(i, n).GetString().Trim();

            var name = Cell(1);
            if (string.IsNullOrEmpty(name)) continue; // skip blank rows

            var code = Cell(4).ToUpperInvariant();
            var (firstName, middleName, lastName) = SplitName(name);
            var identityNumber = string.IsNullOrEmpty(Cell(2)) ? null : Cell(2);
            var mobile = string.IsNullOrEmpty(Cell(3)) ? null : Cell(3);
            var maritalStatusId = !string.IsNullOrEmpty(code) && idMap.TryGetValue(code, out var mid) ? (int?)mid : null;

            // Upsert by ID (رقم الهوية): re-uploading an exported file must NOT
            // duplicate existing passengers — match the ID column and update the
            // existing row instead of inserting a new one. Rows with no ID (or an
            // ID not seen before) are genuinely new and get created.
            HrUser? user = identityNumber != null
                ? await _db.HrUsers.FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber)
                : null;
            if (user == null)
            {
                user = new HrUser
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Mobile = mobile,
                    IdentityNumber = identityNumber,
                    MaritalStatusId = maritalStatusId,
                    Active = true,
                };
                _db.HrUsers.Add(user);
                await _db.SaveChangesAsync();
                created++;
            }
            else
            {
                // Existing passenger — refresh the editable profile fields.
                user.FirstName = firstName;
                user.MiddleName = middleName;
                user.LastName = lastName;
                user.Mobile = mobile;
                if (maritalStatusId != null) user.MaritalStatusId = maritalStatusId;
                await _db.SaveChangesAsync();
                updated++;
            }

            var serial = Cell(5);
            if (string.IsNullOrEmpty(serial)) continue;

            var route = await _db.TransportationVehicleRoutes.FirstOrDefaultAsync(r => r.Serial == serial);
            if (route == null)
            {
                errors.Add($"صف {i}: لا يوجد خط بسيريال {serial}");
                continue;
            }

            var directionName = Cell(7);
            int? directionId = null;
            if (!string.IsNullOrEmpty(directionName))
            {
                var dir = await _db.TransportationVehicleRouteDirections
                    .FirstOrDefaultAsync(d => d.RouteId == route.Id && d.RouteDirection == directionName);
                directionId = dir?.Id;
            }

            // Don't duplicate a membership the passenger already has on this route.
            var alreadyOnRoute = await _db.TransportationVehicleRouteEmployees
                .AnyAsync(m => m.RouteId == route.Id && m.HrUserId == user.Id);
            if (alreadyOnRoute) continue;

            _db.TransportationVehicleRouteEmployees.Add(new TransportationVehicleRouteEmployee
            {
                RouteId = route.Id,
                HrUserId = user.Id,
                DirectionId = directionId,
                Period = NormalizePeriod(Cell(6)),
                Active = true,
            });
            await _db.SaveChangesAsync();
            assigned++;
        }

        if (created == 0 && updated == 0) return Envelope.Fail("Err103", "لا توجد صفوف صالحة في الملف");
        return Envelope.Success(new { Created = created, Updated = updated, Assigned = assigned, Errors = errors });
    }

    /// <summary>Parse a "نشط" cell → true / false / null (unrecognised).</summary>
    private static bool? ParseActive(string raw)
    {
        var v = raw.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(v)) return null;
        if (Regex.IsMatch(v, "^(نعم|yes|true|1|نشط|مفعّل|مفعل|active|y)$")) return true;
        if (Regex.IsMatch(v, "^(لا|no|false|0|موقوف|غير نشط|غير مفعّل|inactive|n)$")) return false;
        return null;
    }

    /// <summary>POST InsertUserNotActiveExcel — ACTIVATION: flip the active flag on existing passengers by ID.</summary>
    public async Task<object> InsertUserNotActiveExcel(string? fileBase64)
    {
        if (string.IsNullOrEmpty(fileBase64)) return Envelope.Fail("Err101", "File is required");

        IXLWorksheet sheet;
        try { sheet = LoadWorkbook(fileBase64); }
        catch { return Envelope.Fail("Err102", "ملف Excel غير صالح"); }

        var updated = 0;
        var notFound = new List<string>();
        var errors = new List<string>();

        var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;
        for (var i = 2; i <= lastRow; i++)
        {
            string Cell(int n) => sheet.Cell(i, n).GetString().Trim();
            var id = Cell(1);
            if (string.IsNullOrEmpty(id)) continue; // skip blank rows

            var active = ParseActive(Cell(3));
            if (active == null)
            {
                errors.Add($"صف {i}: قيمة \"نشط\" غير مفهومة (استخدم نعم/لا)");
                continue;
            }

            var matches = await _db.HrUsers.Where(u => u.IdentityNumber == id).ToListAsync();
            if (matches.Count > 0)
            {
                foreach (var u in matches) u.Active = active.Value;
                await _db.SaveChangesAsync();
                updated += matches.Count;
            }
            else notFound.Add(id);
        }

        if (updated == 0 && notFound.Count == 0 && errors.Count == 0)
            return Envelope.Fail("Err103", "لا توجد صفوف صالحة في الملف");
        return Envelope.Success(new { Updated = updated, NotFound = notFound, Errors = errors });
    }
}
