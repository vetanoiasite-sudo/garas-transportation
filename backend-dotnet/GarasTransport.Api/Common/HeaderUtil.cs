namespace GarasTransport.Api.Common;

public static class HeaderUtil
{
    /// <summary>
    /// Decode a percent-encoded HTTP header value. Filters ride in HTTP headers
    /// (faithful to the documented contract), but header values must be ASCII —
    /// non-ASCII (e.g. Arabic search text) gets mangled in transit. Clients
    /// percent-encode such values; this reverses it. Falls back to the raw value
    /// if it was not encoded (or is malformed), so ASCII stays untouched.
    /// </summary>
    public static string? DecodeHeader(string? value)
    {
        if (string.IsNullOrEmpty(value)) return null;
        try
        {
            return Uri.UnescapeDataString(value);
        }
        catch
        {
            return value;
        }
    }

    /// <summary>Read a header value from the request (case-insensitive), or null.</summary>
    public static string? Get(HttpRequest req, string name)
    {
        if (req.Headers.TryGetValue(name, out var v)) return v.ToString();
        return null;
    }

    public static int GetInt(HttpRequest req, string name, int fallback)
    {
        var raw = Get(req, name);
        return int.TryParse(raw, out var n) ? n : fallback;
    }

    public static int? GetIntOrNull(HttpRequest req, string name)
    {
        var raw = Get(req, name);
        return int.TryParse(raw, out var n) ? n : null;
    }
}
