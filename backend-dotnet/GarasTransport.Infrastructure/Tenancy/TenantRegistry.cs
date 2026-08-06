using Microsoft.Extensions.Configuration;

namespace GarasTransport.Infrastructure.Tenancy;

/// <summary>
/// Multi-tenant registry — database-per-company. The `Companies` config section
/// maps each allowed CompanyName (the value the clients already send in the
/// header/login body) to its own connection string:
///
///   "Companies": {
///     "demo":  "Server=...;Database=GarasTransport;...",
///     "alpha": "Server=...;Database=GarasTransport_Alpha;..."
///   }
///
/// Adding a company = adding one line here (its DB is migrated + seeded on the
/// next startup). Backward compatible: when the section is absent, the legacy
/// single-tenant behaviour applies (`AllowedCompanies` list, every company on
/// `ConnectionStrings:Default`).
/// </summary>
public class TenantRegistry
{
    private readonly Dictionary<string, string> _tenants; // company (lower) → connection string
    private readonly string? _defaultConnectionString;

    public TenantRegistry(IConfiguration config)
    {
        _defaultConnectionString = config.GetConnectionString("Default")
            ?? Environment.GetEnvironmentVariable("DATABASE_URL");

        _tenants = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var section = config.GetSection("Companies");
        if (section.Exists())
        {
            foreach (var child in section.GetChildren())
                if (!string.IsNullOrWhiteSpace(child.Value))
                    _tenants[child.Key] = child.Value!;
        }
        else
        {
            // Legacy fallback: AllowedCompanies (comma-separated) all share the default DB.
            var raw = config["AllowedCompanies"]
                      ?? Environment.GetEnvironmentVariable("ALLOWED_COMPANIES")
                      ?? "demo";
            foreach (var name in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (_defaultConnectionString != null)
                    _tenants[name] = _defaultConnectionString;
        }
    }

    /// <summary>All configured companies with their connection strings (startup migrate/seed loop).</summary>
    public IReadOnlyDictionary<string, string> All => _tenants;

    public bool IsAllowed(string? companyName) =>
        !string.IsNullOrEmpty(companyName) && _tenants.ContainsKey(companyName);

    public bool TryGetConnectionString(string? companyName, out string connectionString)
    {
        connectionString = string.Empty;
        if (string.IsNullOrEmpty(companyName)) return false;
        if (!_tenants.TryGetValue(companyName, out var cs)) return false;
        connectionString = cs;
        return true;
    }

    /// <summary>
    /// Connection string for a request: the company's own DB, else the default
    /// (used before the tenant is known — e.g. design-time tools or the login
    /// request, which switches explicitly once it reads the body).
    /// </summary>
    public string ResolveOrDefault(string? companyName)
    {
        if (companyName != null && _tenants.TryGetValue(companyName, out var cs)) return cs;
        return _defaultConnectionString ?? string.Empty;
    }
}
