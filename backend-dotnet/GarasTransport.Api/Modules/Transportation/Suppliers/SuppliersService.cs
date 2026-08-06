using ClosedXML.Excel;
using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Suppliers;

// Shape of the addSupplier payload (documented PascalCase keys).
public class SupplierContactInput
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Mobile { get; set; }
}

public class SupplierBody
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Fax { get; set; }
    public string? Address { get; set; }
    public List<SupplierContactInput>? Contacts { get; set; }
}

// Shape of the AddSupplierPayment payload (documented PascalCase keys, §6.12).
public class SupplierPaymentBody
{
    public int? SupplierId { get; set; }
    public double? Payment { get; set; }
    public string? DatePayment { get; set; }
    public string? StartDate { get; set; }
    public string? TypeOfDebt { get; set; } // Normal | Advance
    public int? NumberOfMonths { get; set; }
}

public class AccountFilters
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? SupplierId { get; set; }
    public int? RouteId { get; set; }
}

// One row of the supplier monthly statement (concrete so the JSON endpoint and
// the Excel export share the exact same aggregation).
public class MonthStatementRow
{
    public string MonthName { get; set; } = string.Empty;
    public int AccountId { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int MonthNum { get; set; }
    public int RoutesNum { get; set; }
    public int CountOfcompleteRounds { get; set; }
    public int CountOfHalfGoRounds { get; set; }
    public int CountOfHalfReturnRounds { get; set; }
    public double TotalDue { get; set; }
    public double TotalDeduct { get; set; }
    public double TotalTaxesDeduct { get; set; }
    public double TotalNormalDeduct { get; set; }
    public double TotalPaidadvance { get; set; }
    public double TotalPaidNormal { get; set; }
    public double TotalDueAfterPaid { get; set; }
    public double TotalDuecompleteRound { get; set; }
    public double TotalDueHalfGoRound { get; set; }
    public double TotalDueHalfReturnRound { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class SuppliersService
{
    private static readonly string[] MonthNames =
        { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private readonly AppDbContext _db;

    public SuppliersService(AppDbContext db) => _db = db;

    /// <summary>Builds a [start, end) date range for the given year and optional month; null when no year.</summary>
    private static (DateTime Gte, DateTime Lt)? MonthRange(int? year, int? month)
    {
        if (!year.HasValue) return null;
        if (month is >= 1 and <= 12)
            return (new DateTime(year.Value, month.Value, 1), new DateTime(year.Value, month.Value, 1).AddMonths(1));
        return (new DateTime(year.Value, 1, 1), new DateTime(year.Value + 1, 1, 1));
    }

    /// <summary>GET getSuppliers — paginated suppliers with their contacts.</summary>
    public async Task<object> GetSuppliers(int pageNo, int noOfItems, string? name, string? phone, string? mobile)
    {
        var query = _db.Suppliers.Where(s => s.Active);
        if (!string.IsNullOrEmpty(name)) query = query.Where(s => s.Name.Contains(name));
        if (!string.IsNullOrEmpty(phone)) query = query.Where(s => s.Phone != null && s.Phone.Contains(phone));
        if (!string.IsNullOrEmpty(mobile)) query = query.Where(s => s.Mobile != null && s.Mobile.Contains(mobile));

        var total = await query.CountAsync();
        var rows = await query
            .Include(s => s.Contacts)
            .OrderByDescending(s => s.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .Select(s => new { S = s, ActiveRoutes = s.Routes.Count(r => r.Active) })
            .ToListAsync();

        var data = rows.Select(x => new
        {
            Id = x.S.Id,
            Name = x.S.Name,
            Email = x.S.Email ?? string.Empty,
            Phone = x.S.Phone ?? string.Empty,
            Mobile = x.S.Mobile ?? string.Empty,
            Fax = x.S.Fax ?? string.Empty,
            Address = x.S.Address ?? string.Empty,
            CreationDate = Fmt.Iso(x.S.CreationDate),
            ActiveRoutes = x.ActiveRoutes,
            contacts = x.S.Contacts.Select(c => new { Id = c.Id, Name = c.Name, Mobile = c.Mobile ?? string.Empty }),
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET getSupplier — a single supplier with its contacts.</summary>
    public async Task<object> GetSupplier(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var s = await _db.Suppliers.Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return Envelope.Fail("Err404", "Supplier not found");
        return Envelope.Success(new
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email ?? string.Empty,
            Phone = s.Phone ?? string.Empty,
            Mobile = s.Mobile ?? string.Empty,
            Fax = s.Fax ?? string.Empty,
            Address = s.Address ?? string.Empty,
            CreationDate = Fmt.Iso(s.CreationDate),
            contacts = s.Contacts.Select(c => new { Id = c.Id, Name = c.Name, Mobile = c.Mobile ?? string.Empty }),
        });
    }

    /// <summary>POST updateSupplier — edit a supplier's fields and sync its contact persons.</summary>
    public async Task<object> UpdateSupplier(int id, SupplierBody body)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        if (string.IsNullOrWhiteSpace(body.Name)) return Envelope.Fail("Err101", "Name is required");
        var s = await _db.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return Envelope.Fail("Err404", "Supplier not found");
        s.Name = body.Name;
        s.Email = body.Email;
        s.Phone = body.Phone;
        s.Mobile = body.Mobile;
        s.Fax = body.Fax;
        s.Address = body.Address;
        await _db.SaveChangesAsync();
        await SyncContacts(id, body.Contacts ?? new());
        return Envelope.SuccessWrite(id);
    }

    /// <summary>Reconcile a supplier's contact persons against the submitted list.</summary>
    private async Task SyncContacts(int supplierId, List<SupplierContactInput> incoming)
    {
        var clean = incoming.Where(c => !string.IsNullOrWhiteSpace(c.Name)).ToList();
        var existing = await _db.SupplierContactPersons.Where(c => c.SupplierId == supplierId).ToListAsync();
        var keptIds = clean.Where(c => c.Id.HasValue).Select(c => c.Id!.Value).ToHashSet();

        // Delete contacts the user removed — unless a route still points at them.
        foreach (var ex in existing)
        {
            if (keptIds.Contains(ex.Id)) continue;
            var refs = await _db.TransportationVehicleRoutes.CountAsync(r => r.SupplierContactPersonId == ex.Id);
            if (refs == 0) _db.SupplierContactPersons.Remove(ex);
        }

        // Update existing, create new.
        foreach (var c in clean)
        {
            var name = c.Name!.Trim();
            var mobile = string.IsNullOrWhiteSpace(c.Mobile) ? null : c.Mobile!.Trim();
            var match = c.Id.HasValue ? existing.FirstOrDefault(e => e.Id == c.Id.Value) : null;
            if (match != null)
            {
                match.Name = name;
                match.Mobile = mobile;
            }
            else
            {
                _db.SupplierContactPersons.Add(new SupplierContactPerson { SupplierId = supplierId, Name = name, Mobile = mobile });
            }
        }
        await _db.SaveChangesAsync();
    }

    /// <summary>POST addSupplier — create a supplier after a duplicate check.</summary>
    public async Task<object> AddSupplier(SupplierBody body)
    {
        if (string.IsNullOrWhiteSpace(body.Name)) return Envelope.Fail("Err101", "Name is required");

        var existing = await _db.Suppliers.Where(s => s.Active && (
            s.Name == body.Name ||
            (body.Email != null && s.Email == body.Email) ||
            (body.Phone != null && s.Phone == body.Phone) ||
            (body.Mobile != null && s.Mobile == body.Mobile) ||
            (body.Fax != null && s.Fax == body.Fax)))
            .FirstOrDefaultAsync();
        if (existing != null) return Envelope.Fail("Err102", "المورد موجود مسبقاً");

        var contacts = (body.Contacts ?? new())
            .Where(c => !string.IsNullOrWhiteSpace(c.Name))
            .Select(c => new SupplierContactPerson { Name = c.Name!.Trim(), Mobile = string.IsNullOrWhiteSpace(c.Mobile) ? null : c.Mobile!.Trim() })
            .ToList();

        var created = new Supplier
        {
            Name = body.Name,
            Email = body.Email,
            Phone = body.Phone,
            Mobile = body.Mobile,
            Fax = body.Fax,
            Address = body.Address,
            Contacts = contacts,
        };
        _db.Suppliers.Add(created);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(created.Id);
    }

    // ── Rounds/pricing helpers (faithful to AccountsAllMonths22 / AccountsAllRounds) ──

    /// <summary>A round "belongs to" (month, year) if any of its date columns falls in it.
    /// CheckInDate compared as-is; CheckOutDate/OneWayDate shifted −16h (legacy shift-boundary correction).</summary>
    private static bool RoundInPeriod(TransportationRoundForRoute r, int? month, int? year)
    {
        if (!month.HasValue && !year.HasValue) return true;
        bool Hit(DateTime? d, int shiftHours)
        {
            if (!d.HasValue) return false;
            var s = d.Value.AddHours(-shiftHours);
            var mOk = !month.HasValue || s.Month == month.Value;
            var yOk = !year.HasValue || s.Year == year.Value;
            return mOk && yOk;
        }
        return Hit(r.CheckInDate, 0) || Hit(r.CheckOutDate, 16) || Hit(r.OneWayDate, 16);
    }

    /// <summary>Per-leg price for a round on a given date: the latest exception price
    /// effective on/before that date, else the route's LineCost — halved for two-way routes.</summary>
    private static double PriceForRoute(int routeId, DateTime? date,
        Dictionary<int, (bool OneWay, double LineCost)> routeMap,
        List<TransportionLineExceptionPrice> exPrices) // sorted fromDate desc
    {
        routeMap.TryGetValue(routeId, out var route);
        var oneWay = route.OneWay;
        var basePrice = route.LineCost;
        if (date.HasValue)
        {
            var ex = exPrices.FirstOrDefault(p => p.RouteId == routeId && p.FromDate <= date.Value);
            if (ex != null) basePrice = ex.Price;
        }
        return oneWay ? basePrice : basePrice / 2;
    }

    /// <summary>Core aggregation shared by the JSON endpoint and the Excel export.</summary>
    private async Task<(List<MonthStatementRow> Rows, int Total)> ComputeMonths(int pageNo, int noOfItems, AccountFilters filters)
    {
        var accQuery = _db.TransportationVehicleRouteAccounts.AsQueryable();
        if (filters.SupplierId.HasValue) accQuery = accQuery.Where(a => a.SupplierId == filters.SupplierId.Value);
        if (filters.RouteId.HasValue) accQuery = accQuery.Where(a => a.RouteId == filters.RouteId.Value);
        var range = MonthRange(filters.Year, filters.Month);
        if (range.HasValue) accQuery = accQuery.Where(a => a.DateOfMonth >= range.Value.Gte && a.DateOfMonth < range.Value.Lt);

        var accounts = await accQuery.Include(a => a.Supplier).OrderByDescending(a => a.DateOfMonth).ToListAsync();

        // Distinct (supplier, year, month) groups; RoutesNum = number of account rows in the group.
        var groups = new Dictionary<string, MonthGroup>();
        foreach (var a in accounts)
        {
            var d = a.DateOfMonth;
            var month = d.Month;
            var year = d.Year;
            var key = $"{a.SupplierId ?? 0}-{year}-{month}";
            if (!groups.TryGetValue(key, out var g))
            {
                g = new MonthGroup { AccountId = a.Id, SupplierId = a.SupplierId ?? 0, SupplierName = a.Supplier?.Name ?? string.Empty, Month = month, Year = year };
                groups[key] = g;
            }
            g.RoutesNum += 1;
        }

        var all = groups.Values.ToList();
        var total = all.Count;
        var page = all.Skip((pageNo - 1) * noOfItems).Take(noOfItems).ToList();
        var supplierIds = page.Select(g => g.SupplierId).Distinct().ToList();

        var rounds = await _db.TransportationRoundForRoutes.Where(r => r.SupplierId != null && supplierIds.Contains(r.SupplierId.Value))
            .Include(r => r.Route).ToListAsync();
        var deductions = await _db.TransportationVehicleRouteDeductions.Where(x => x.SupplierId != null && supplierIds.Contains(x.SupplierId.Value)).ToListAsync();
        var payments = await _db.SupplierPayments.Where(p => supplierIds.Contains(p.SupplierId)).Include(p => p.Distribution).ToListAsync();

        var routeIds = rounds.Select(r => r.RouteId).Distinct().ToList();
        var exPrices = routeIds.Count > 0
            ? await _db.TransportionLineExceptionPrices.Where(p => routeIds.Contains(p.RouteId)).OrderByDescending(p => p.FromDate).ToListAsync()
            : new List<TransportionLineExceptionPrice>();
        var routeMap = new Dictionary<int, (bool OneWay, double LineCost)>();
        foreach (var r in rounds)
            routeMap[r.RouteId] = (r.Route?.OneWay ?? false, r.Route?.LineCost ?? 0);

        var result = page.Select(g =>
        {
            var supRounds = rounds.Where(r => r.SupplierId == g.SupplierId && RoundInPeriod(r, g.Month, g.Year)).ToList();
            var complete = supRounds.Where(r => !(r.Route?.OneWay ?? false)).ToList();
            var halfGo = supRounds.Where(r => (r.Route?.OneWay ?? false) && !string.IsNullOrEmpty(r.Route?.FromTime)).ToList();
            var halfReturn = supRounds.Where(r => (r.Route?.OneWay ?? false) && !string.IsNullOrEmpty(r.Route?.ToTime)).ToList();

            var dueComplete = complete.Sum(r => PriceForRoute(r.RouteId, r.CheckInDate ?? r.CheckOutDate, routeMap, exPrices));
            var dueHalfGo = halfGo.Sum(r => PriceForRoute(r.RouteId, r.OneWayDate, routeMap, exPrices));
            var dueHalfReturn = halfReturn.Sum(r => PriceForRoute(r.RouteId, r.OneWayDate, routeMap, exPrices));
            var totalDue = dueComplete + dueHalfGo + dueHalfReturn;

            var supDeducts = deductions.Where(x => x.SupplierId == g.SupplierId
                && x.DateOfDeduction.Month == g.Month && x.DateOfDeduction.Year == g.Year).ToList();
            var totalDeduct = supDeducts.Sum(x => x.DeductPerRound);
            var totalNormalDeduct = supDeducts.Where(x => (x.TypeOfDedct ?? string.Empty) == "Normal").Sum(x => x.DeductPerRound);
            var totalTaxesDeduct = supDeducts.Where(x => (x.TypeOfDedct ?? string.Empty) == "Taxes").Sum(x => x.DeductPerRound);

            var supPayments = payments.Where(p => p.SupplierId == g.SupplierId).ToList();
            var totalPaidNormal = supPayments
                .Where(p => p.TypeOfDebt == "Normal" && p.DatePayment.Month == g.Month && p.DatePayment.Year == g.Year)
                .Sum(p => p.Payment);
            var advanceDistributions = supPayments.Where(p => p.TypeOfDebt == "Advance").SelectMany(p => p.Distribution).ToList();
            var totalPaidAdvance = advanceDistributions.Where(d => d.MonthNum == g.Month && d.YearNum == g.Year).Sum(d => d.Payment);
            var advanceOtherMonth = advanceDistributions.Any(d => d.MonthNum > g.Month);

            var totalDueAfterPaid = totalDue - (totalDeduct + totalPaidNormal + totalPaidAdvance);

            return new MonthStatementRow
            {
                MonthName = g.Month >= 0 && g.Month < MonthNames.Length ? MonthNames[g.Month] : string.Empty,
                AccountId = g.AccountId,
                SupplierId = g.SupplierId,
                SupplierName = g.SupplierName,
                MonthNum = g.Month,
                RoutesNum = g.RoutesNum,
                CountOfcompleteRounds = complete.Count / 2, // two legs per complete round
                CountOfHalfGoRounds = halfGo.Count,
                CountOfHalfReturnRounds = halfReturn.Count,
                TotalDue = totalDue,
                TotalDeduct = totalDeduct,
                TotalTaxesDeduct = totalTaxesDeduct,
                TotalNormalDeduct = totalNormalDeduct,
                TotalPaidadvance = totalPaidAdvance,
                TotalPaidNormal = totalPaidNormal,
                TotalDueAfterPaid = totalDueAfterPaid,
                TotalDuecompleteRound = dueComplete,
                TotalDueHalfGoRound = dueHalfGo,
                TotalDueHalfReturnRound = dueHalfReturn,
                Note = advanceOtherMonth ? "هناك دفعات مقدمة لهذا المورد في أشهر أخرى، يرجى التحقق من التفاصيل" : string.Empty,
            };
        }).ToList();

        return (result, total);
    }

    /// <summary>GET AccountsAllMonthsForSupplier — supplier account statement grouped by supplier+month.</summary>
    public async Task<object> AccountsAllMonths(int pageNo, int noOfItems, AccountFilters filters)
    {
        var (rows, total) = await ComputeMonths(pageNo, noOfItems, filters);
        return Envelope.SuccessList(rows, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET AccountsAllMonthsForSupplierExcel — the same statement as a base64 (.xlsx) file.</summary>
    public async Task<object> AccountsAllMonthsExcel(AccountFilters filters)
    {
        var (rows, _) = await ComputeMonths(1, 100000, filters);

        using var wb = new XLWorkbook();
        var sheet = wb.AddWorksheet("كشف حساب الموردين");
        sheet.RightToLeft = true;
        string[] headers = { "الشهر", "المورد", "عدد الخطوط", "الجولات الكاملة", "الإجمالي المستحق", "الخصومات", "المتبقّي" };
        for (var c = 0; c < headers.Length; c++) sheet.Cell(1, c + 1).Value = headers[c];
        sheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var d in rows)
        {
            sheet.Cell(row, 1).Value = d.MonthName;
            sheet.Cell(row, 2).Value = d.SupplierName;
            sheet.Cell(row, 3).Value = d.RoutesNum;
            sheet.Cell(row, 4).Value = d.CountOfcompleteRounds;
            sheet.Cell(row, 5).Value = d.TotalDue;
            sheet.Cell(row, 6).Value = d.TotalDeduct;
            sheet.Cell(row, 7).Value = d.TotalDue - d.TotalDeduct;
            row++;
        }

        return Envelope.Success(Fmt.WorkbookBase64(wb));
    }

    /// <summary>GET AccountsAllRoundsForSupplier — per-round detail (exception-aware per-leg price).</summary>
    public async Task<object> AccountsAllRounds(int pageNo, int noOfItems, AccountFilters filters)
    {
        var query = _db.TransportationRoundForRoutes.AsQueryable();
        if (filters.SupplierId.HasValue) query = query.Where(r => r.SupplierId == filters.SupplierId.Value);
        if (filters.RouteId.HasValue) query = query.Where(r => r.RouteId == filters.RouteId.Value);

        // Filter on the round's own date columns (with the −16h shift), in memory.
        var allRows = await query.Include(r => r.Route).OrderByDescending(r => r.Id).ToListAsync();
        var filtered = allRows.Where(r => RoundInPeriod(r, filters.Month, filters.Year)).ToList();
        var total = filtered.Count;
        var rows = filtered.Skip((pageNo - 1) * noOfItems).Take(noOfItems).ToList();

        var routeIds = rows.Select(r => r.RouteId).Distinct().ToList();
        var exPrices = routeIds.Count > 0
            ? await _db.TransportionLineExceptionPrices.Where(p => routeIds.Contains(p.RouteId)).OrderByDescending(p => p.FromDate).ToListAsync()
            : new List<TransportionLineExceptionPrice>();
        var routeMap = new Dictionary<int, (bool OneWay, double LineCost)>();
        foreach (var r in rows) routeMap[r.RouteId] = (r.Route?.OneWay ?? false, r.Route?.LineCost ?? 0);

        var data = rows.Select(r =>
        {
            var oneWay = r.Route?.OneWay ?? false;
            var roundDate = r.CheckInDate ?? r.CheckOutDate ?? r.OneWayDate;
            double roundsNum = 0;
            if (oneWay && r.OneWayDate.HasValue) roundsNum = 1;
            else if (r.CheckInDate.HasValue && r.CheckOutDate.HasValue) roundsNum = 1;
            else if (r.CheckInDate.HasValue || r.CheckOutDate.HasValue) roundsNum = 0.5;

            return new
            {
                NameOfRoute = r.Route?.NameOfRoute ?? string.Empty,
                Serial = !string.IsNullOrEmpty(r.Serial) ? r.Serial : (r.Route?.Serial ?? string.Empty),
                DateOfRound = Fmt.IsoDate(roundDate),
                DateOfCheckIn = Fmt.Iso(r.CheckInDate),
                DateOfCheckOut = Fmt.Iso(r.CheckOutDate),
                DateOfOneWay = Fmt.Iso(r.OneWayDate),
                RoundsNum = roundsNum,
                TotalPriceOfDay = PriceForRoute(r.RouteId, roundDate, routeMap, exPrices),
            };
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>GET getAllSupplierPayment — paginated payments with their monthly distribution.</summary>
    public async Task<object> GetAllSupplierPayment(int pageNo, int noOfItems, string? fromDate, string? toDate, int? supplierId)
    {
        var query = _db.SupplierPayments.AsQueryable();
        if (supplierId.HasValue) query = query.Where(p => p.SupplierId == supplierId.Value);
        var from = ParseDate(fromDate);
        var to = ParseDate(toDate);
        if (from.HasValue) query = query.Where(p => p.DatePayment >= from.Value);
        if (to.HasValue) query = query.Where(p => p.DatePayment <= to.Value);

        var total = await query.CountAsync();
        var rows = await query
            .Include(p => p.Supplier).Include(p => p.Distribution)
            .OrderByDescending(p => p.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var data = rows.Select(p => new
        {
            SupplierName = p.Supplier?.Name ?? string.Empty,
            Payment = p.Payment.ToString(System.Globalization.CultureInfo.InvariantCulture),
            DatePayment = Fmt.Iso(p.DatePayment),
            StartDate = Fmt.Iso(p.StartDate),
            NumberOfMonths = p.NumberOfMonths != null ? p.NumberOfMonths.Value.ToString() : string.Empty,
            TypeOfDebt = p.TypeOfDebt,
            DistributionSupplierPayments = p.Distribution.Select(d => new
            {
                Payment = d.Payment.ToString(System.Globalization.CultureInfo.InvariantCulture),
                MonthNum = d.MonthNum.ToString(),
                YearNum = d.YearNum.ToString(),
            }),
        });

        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    /// <summary>POST AddSupplierPayment — create a payment; Advance debts fan out across NumberOfMonths from StartDate.</summary>
    public async Task<object> AddSupplierPayment(SupplierPaymentBody body)
    {
        var supplierId = body.SupplierId ?? 0;
        if (supplierId == 0) return Envelope.Fail("Err101", "SupplierId is required");
        var payment = body.Payment ?? 0;
        var typeOfDebt = body.TypeOfDebt ?? "Normal";
        var datePayment = ParseDate(body.DatePayment) ?? DateTime.UtcNow;
        var startDate = ParseDate(body.StartDate);
        var numberOfMonths = body.NumberOfMonths ?? 0;

        var distribution = new List<DistributionSupplierPayment>();
        if (typeOfDebt == "Advance" && numberOfMonths > 0 && startDate.HasValue)
        {
            var perMonth = payment / numberOfMonths;
            for (var i = 0; i < numberOfMonths; i++)
            {
                var d = new DateTime(startDate.Value.Year, startDate.Value.Month, 1).AddMonths(i);
                distribution.Add(new DistributionSupplierPayment { Payment = perMonth, MonthNum = d.Month, YearNum = d.Year });
            }
        }

        var created = new SupplierPayment
        {
            SupplierId = supplierId,
            Payment = payment,
            DatePayment = datePayment,
            StartDate = startDate,
            NumberOfMonths = numberOfMonths != 0 ? numberOfMonths : null,
            TypeOfDebt = typeOfDebt,
            Distribution = distribution,
        };
        _db.SupplierPayments.Add(created);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(created.Id);
    }

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var d) ? d : null);

    private class MonthGroup
    {
        public int AccountId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public int RoutesNum { get; set; }
    }
}
