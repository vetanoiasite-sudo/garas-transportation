using GarasTransport.Api.Common;
using GarasTransport.Api.Modules.Transportation.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Repricing;

// Shape of the ModifyPriceOfTransportationLine payload (documented PascalCase keys, §6.3).
public class RepriceBody
{
    public double? IncreaseCost { get; set; }
    public bool? IsPercent { get; set; }
    public bool? ForAllLines { get; set; }
    public bool? ApproximateToFiveFlag { get; set; }
    public List<int>? TransportationLineIds { get; set; } // ARRAY of ROUTE ids
    public string? StartDate { get; set; } // ISO-UTC
}

public class RepricingService
{
    private readonly AppDbContext _db;
    private readonly NotificationsService _notifications;

    public RepricingService(AppDbContext db, NotificationsService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    // Percentage bump multiplies, otherwise adds a flat cost; optionally snapped
    // to the nearest 5 (ApproximateToFiveFlag).
    private static double ComputePriceAfter(double before, double increaseCost, bool isPercent, bool approximateToFive)
    {
        var after = isPercent ? before * (1 + increaseCost / 100) : before + increaseCost;
        return approximateToFive ? Math.Round(after / 5, MidpointRounding.AwayFromZero) * 5 : after;
    }

    /// <summary>POST ModifyPriceOfTransportationLine — create a pending increase request + one line per route.</summary>
    public async Task<object> Modify(RepriceBody body, int userId)
    {
        var increaseCost = body.IncreaseCost ?? 0;
        var isPercent = body.IsPercent ?? false;
        var forAllLines = body.ForAllLines ?? false;
        var approximateToFive = body.ApproximateToFiveFlag ?? false;
        var startDate = ParseDate(body.StartDate) ?? DateTime.UtcNow;

        var routeIds = body.TransportationLineIds?.Where(n => n > 0).ToList() ?? new List<int>();
        if (forAllLines)
            routeIds = await _db.TransportationVehicleRoutes.Where(r => r.Active).Select(r => r.Id).ToListAsync();
        if (routeIds.Count == 0) return Envelope.Fail("Err101", "TransportationLineIds is required");

        var routes = await _db.TransportationVehicleRoutes
            .Where(r => routeIds.Contains(r.Id))
            .Select(r => new { r.Id, r.LineCost })
            .ToListAsync();

        var created = new TransportationLineIncreaseRequest
        {
            IsPercent = isPercent,
            IncreaseCost = increaseCost,
            ForAllLines = forAllLines,
            ApproximateToFiveFlag = approximateToFive,
            StartDate = startDate,
            Approve = null,
            CreationBy = userId,
            Lines = routes.Select(r => new TransportationLineIncreaseRequestLine
            {
                RouteId = r.Id,
                PriceBefore = r.LineCost,
                PriceAfter = ComputePriceAfter(r.LineCost, increaseCost, isPercent, approximateToFive),
            }).ToList(),
        };
        _db.TransportationLineIncreaseRequests.Add(created);
        await _db.SaveChangesAsync();

        await _notifications.NotifySuperAdmins(userId, new NotifyOptions
        {
            Title = "طلب تغيير أسعار الخطوط",
            Description = "بحاجة لاعتماد",
            EntityType = "repricing",
            EntityId = created.Id,
        });
        return Envelope.SuccessWrite(created.Id);
    }

    /// <summary>Resolve a set of User ids → their full names, in one query.</summary>
    private async Task<Dictionary<int, string>> UserNames(IEnumerable<int?> ids)
    {
        var unique = ids.Where(i => i.HasValue).Select(i => i!.Value).Distinct().ToList();
        var map = new Dictionary<int, string>();
        if (unique.Count == 0) return map;
        var users = await _db.Users.Where(u => unique.Contains(u.Id)).ToListAsync();
        foreach (var u in users)
        {
            var name = string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));
            map[u.Id] = string.IsNullOrEmpty(name) ? u.Email : name;
        }
        return map;
    }

    /// <summary>GET getAllModifyPriceOfTransportationLine — paginated ModifiedPriceData with per-route details.</summary>
    public async Task<object> GetAll(int pageNo, int noOfItems, bool? approve)
    {
        var query = _db.TransportationLineIncreaseRequests.AsQueryable();
        // Pending requests are stored as approve=null. approve=false means
        // "not yet approved" (pending null OR rejected false).
        if (approve == true) query = query.Where(r => r.Approve == true);
        else if (approve == false) query = query.Where(r => r.Approve != true);

        var total = await query.CountAsync();
        var rows = await query
            .Include(r => r.Lines).ThenInclude(l => l.Route)
            .OrderByDescending(r => r.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var names = await UserNames(rows.SelectMany(r => new[] { r.CreationBy, r.ApprovedBy }));

        var data = rows.Select(r => new
        {
            Id = r.Id,
            IsPercent = r.IsPercent,
            ApproximateToFiveFlag = r.ApproximateToFiveFlag,
            IncreaseCost = r.IncreaseCost,
            Approve = r.Approve,
            ApprovedBy = r.ApprovedBy,
            ApprovedByName = r.ApprovedBy != null ? names.GetValueOrDefault(r.ApprovedBy.Value, string.Empty) : string.Empty,
            ApprovedDate = Fmt.IsoOrNull(r.ApprovedDate),
            ForAllLines = r.ForAllLines,
            CreationDate = Fmt.Iso(r.CreationDate),
            CreationBy = r.CreationBy ?? 0,
            CreationName = r.CreationBy != null ? names.GetValueOrDefault(r.CreationBy.Value, string.Empty) : string.Empty,
            transportationLineDetails = r.Lines.Select(l => new
            {
                RouteId = l.RouteId,
                RouteName = l.Route?.NameOfRoute ?? string.Empty,
                PriceBefore = l.PriceBefore,
                PriceAfter = l.PriceAfter,
            }),
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>POST UpdatePriceOfTransportationLine — approve: stamp request, apply each route's new lineCost, log exception price.</summary>
    public async Task<object> ApprovePrice(int id, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var request = await _db.TransportationLineIncreaseRequests.Include(r => r.Lines).FirstOrDefaultAsync(r => r.Id == id);
        if (request == null) return Envelope.Fail("Err404", "Repricing request not found");

        request.Approve = true;
        request.ApprovedBy = userId;
        request.ApprovedDate = DateTime.UtcNow;

        var routeIds = request.Lines.Select(l => l.RouteId).ToList();
        var routes = await _db.TransportationVehicleRoutes.Where(r => routeIds.Contains(r.Id)).ToListAsync();
        var routeById = routes.ToDictionary(r => r.Id);
        foreach (var l in request.Lines)
        {
            if (routeById.TryGetValue(l.RouteId, out var route)) route.LineCost = l.PriceAfter;
            _db.TransportionLineExceptionPrices.Add(new TransportionLineExceptionPrice
            {
                RouteId = l.RouteId,
                Price = l.PriceAfter,
                FromDate = request.StartDate,
            });
        }
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    /// <summary>POST RejectUpdatePrice — reject the pending increase request.</summary>
    public async Task<object> RejectPrice(int id, int userId)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var request = await _db.TransportationLineIncreaseRequests.FirstOrDefaultAsync(r => r.Id == id);
        if (request == null) return Envelope.Fail("Err404", "Repricing request not found");
        request.Approve = false;
        request.ApprovedBy = userId;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id);
    }

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : null);
}
