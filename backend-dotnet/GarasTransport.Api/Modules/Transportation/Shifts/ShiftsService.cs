using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Api.Modules.Transportation.Shifts;

public class ShiftInput
{
    public int? Id { get; set; }
    public int WeekDayId { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public int ShiftNumber { get; set; }
    public int? BranchId { get; set; }
    public bool? Active { get; set; }
}

public class ShiftsService
{
    private readonly AppDbContext _db;

    public ShiftsService(AppDbContext db) => _db = db;

    /// <summary>GET GetShifts — branch schedule grouped by shiftNumber (opt BranchId header).</summary>
    public async Task<object> GetShifts(int? branchId)
    {
        var query = _db.BranchSchedules.Where(b => b.Active);
        if (branchId.HasValue) query = query.Where(b => b.BranchId == branchId.Value);
        var rows = await query
            .OrderBy(b => b.ShiftNumber).ThenBy(b => b.WeekDayId)
            .ToListAsync();

        var data = rows
            .GroupBy(r => r.ShiftNumber)
            .Select(g => new
            {
                shiftNumber = g.Key,
                shifts = g.Select(MapShift),
            });
        return Envelope.Success(data);
    }

    private static object MapShift(BranchSchedule r) => new
    {
        Id = r.Id,
        WeekDayId = r.WeekDayId,
        From = r.From,
        To = r.To,
        BranchId = r.BranchId,
        ShiftNumber = r.ShiftNumber,
        CreationDate = r.CreationDate,
        CreatedBy = r.CreatedBy,
        ModifiedDate = r.ModifiedDate,
        ModifiedBy = r.ModifiedBy,
    };

    /// <summary>POST AddListOfShifts — create the active days of a shift group.
    /// JSON body { Shifts: [...] } (pragmatic deviation from multipart).</summary>
    public async Task<object> AddListOfShifts(List<ShiftInput>? shifts, int userId)
    {
        if (shifts == null || shifts.Count == 0) return Envelope.Fail("Err101", "Shifts is required");
        var activeDays = shifts.Where(s => s.Active != false).ToList();
        if (activeDays.Count == 0) return Envelope.Fail("Err101", "At least one active shift day is required");

        _db.BranchSchedules.AddRange(activeDays.Select(s => new BranchSchedule
        {
            ShiftNumber = s.ShiftNumber,
            WeekDayId = s.WeekDayId,
            From = s.From,
            To = s.To,
            Active = true,
            BranchId = s.BranchId,
            CreatedBy = userId,
            ModifiedBy = userId,
        }));
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(null, "Shifts created");
    }

    /// <summary>POST UpdateShift — upsert the days of a shift group. Days with an Id
    /// are updated; days without an Id are created.</summary>
    public async Task<object> UpdateShift(List<ShiftInput>? shifts, int userId)
    {
        if (shifts == null || shifts.Count == 0) return Envelope.Fail("Err101", "Shifts is required");

        foreach (var s in shifts)
        {
            if (s.Id.HasValue && s.Id.Value != 0)
            {
                var row = await _db.BranchSchedules.FirstOrDefaultAsync(x => x.Id == s.Id.Value);
                if (row == null) continue;
                row.ShiftNumber = s.ShiftNumber;
                row.WeekDayId = s.WeekDayId;
                row.From = s.From;
                row.To = s.To;
                row.Active = s.Active ?? true;
                row.BranchId = s.BranchId;
                row.ModifiedBy = userId;
            }
            else if (s.Active != false)
            {
                _db.BranchSchedules.Add(new BranchSchedule
                {
                    ShiftNumber = s.ShiftNumber,
                    WeekDayId = s.WeekDayId,
                    From = s.From,
                    To = s.To,
                    Active = true,
                    BranchId = s.BranchId,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                });
            }
        }
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(null, "Shifts updated");
    }
}
