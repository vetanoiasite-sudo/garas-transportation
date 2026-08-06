using System.Globalization;
using ClosedXML.Excel;

namespace GarasTransport.Application.Helpers;

// Small formatting helpers shared across modules.
public static class Fmt
{
    /// <summary>JS Date.toISOString() equivalent: "yyyy-MM-ddTHH:mm:ss.fffZ" (UTC), or "" when null.</summary>
    public static string Iso(DateTime? d) =>
        d.HasValue ? d.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture) : string.Empty;

    /// <summary>ISO string or null (matches the places the NestJS code emits `null` not "").</summary>
    public static string? IsoOrNull(DateTime? d) =>
        d.HasValue ? Iso(d) : null;

    /// <summary>Date-only slice "yyyy-MM-dd" (matches toISOString().slice(0,10)).</summary>
    public static string IsoDate(DateTime? d) =>
        d.HasValue ? d.Value.ToUniversalTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;

    /// <summary>Serialize a ClosedXML workbook to a base64 string (matches the exceljs base64 output).</summary>
    public static string WorkbookBase64(XLWorkbook wb)
    {
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}
