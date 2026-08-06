using System.Text.Json.Serialization;
using GarasTransport.Api.Common;
using GarasTransport.Api.Extensions;
using GarasTransport.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Clean Architecture wiring (doc §13.2): infrastructure (DbContext, UnitOfWork)
// then application services (interface → implementation).
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Transitional: auto-register the not-yet-migrated module services (classes named
// *Service inside the API assembly) as scoped. Shrinks as modules move into
// GarasTransport.Application; delete once the last module migrates.
foreach (var type in typeof(Program).Assembly.GetTypes()
             .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service")))
{
    builder.Services.AddScoped(type);
}

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Keep PascalCase (and the frozen intentional misspellings) byte-for-byte
        // — the Flutter/Next clients string-match these keys.
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Open CORS policy — matches the old backend.
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionEnvelopeMiddleware>();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Best-effort migrate + seed on startup — once PER COMPANY (database-per-company:
// every tenant in the Companies config gets its own DB created/migrated/seeded).
// Wrapped so the app still boots (e.g. to serve Swagger) when SQL Server isn't
// reachable yet; set SKIP_DB_INIT=true to skip.
if (Environment.GetEnvironmentVariable("SKIP_DB_INIT") != "true")
{
    var tenants = app.Services.GetRequiredService<GarasTransport.Infrastructure.Tenancy.TenantRegistry>();
    foreach (var (company, connectionString) in tenants.All)
    {
        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            await using var db = new AppDbContext(options);
            db.Database.Migrate();
            await GarasTransport.Infrastructure.Persistence.Initializers.DbInitializer.SeedAsync(db);
            app.Logger.LogInformation("Company '{Company}': database migrated and seeded.", company);
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "Company '{Company}': database init skipped (SQL Server not reachable?).", company);
        }
    }
}

// Bind all interfaces on the configured port (containers/hosts). Default 4000
// to match the NestJS backend.
var port = Environment.GetEnvironmentVariable("PORT") ?? "4000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
