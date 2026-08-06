using System.Globalization;
using GarasTransport.Application.DTOs.Passenger;
using GarasTransport.Application.Helpers;
using GarasTransport.Application.IServices.Passengers;
using GarasTransport.Application.Responses;
using GarasTransport.Domain.Entities;
using GarasTransport.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Application.Services.Passengers;

/// <summary>
/// Passenger business logic on top of IUnitOfWork (doc §8.3). Query shapes and
/// wire keys are byte-for-byte faithful to the legacy module — including the
/// intentional typos (FirstNane, Longtitud). Do not "fix" them.
/// </summary>
public class PassengerService : IPassengerService
{
    private readonly IUnitOfWork _unitOfWork;

    public PassengerService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    private static string FullName(HrUser? u) =>
        u == null ? string.Empty : string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    private static DateTime? ParseDate(string? s) =>
        string.IsNullOrEmpty(s) ? null : (DateTime.TryParse(s, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var d) ? d : null);

    /* ---- Route memberships (route side) ---- */

    public async Task<ServiceResult<IEnumerable<GetRouteEmployeeDto>>> GetEmployeeListAsync(int routeId)
    {
        if (routeId == 0) return ServiceResult<IEnumerable<GetRouteEmployeeDto>>.Fail("Err101", "RouteId is required");
        var rows = await _unitOfWork.RouteEmployees
            .FindAllQueryable(m => m.RouteId == routeId && m.Active)
            .Include(m => m.HrUser).Include(m => m.Direction)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
        var data = rows.Select(m => new GetRouteEmployeeDto
        {
            ID = m.Id,
            HrUserId = m.HrUserId,
            FirstNane = m.HrUser?.FirstName ?? string.Empty, // ⚠ frozen typo
            MiddleName = m.HrUser?.MiddleName ?? string.Empty,
            LastName = m.HrUser?.LastName ?? string.Empty,
            Mobile = m.HrUser?.Mobile ?? string.Empty,
            Email = m.HrUser?.Email ?? string.Empty,
            Photo = m.HrUser?.ImgPath ?? string.Empty,
            DirectionLatitude = m.Direction?.Latitude ?? 0,
            DirectionLongtitud = m.Direction?.Longitude ?? 0, // ⚠ frozen typo
            DirectionId = m.DirectionId ?? 0,
            DirectionName = m.Direction?.RouteDirection ?? string.Empty,
            Period = m.Period ?? string.Empty,
        });
        return ServiceResult<IEnumerable<GetRouteEmployeeDto>>.Success(data);
    }

    public async Task<ServiceResult<IEnumerable<GetRouteEmployeeForMobDto>>> GetEmployeesForMobAsync(int routeId, string? period, string? dateSearch)
    {
        if (routeId == 0) return ServiceResult<IEnumerable<GetRouteEmployeeForMobDto>>.Fail("Err101", "RouteId is required");
        var query = _unitOfWork.RouteEmployees.FindAllQueryable(m => m.RouteId == routeId && m.Active);
        if (!string.IsNullOrEmpty(period))
        {
            var vals = new[] { period, "Both" };
            query = query.Where(m => vals.Contains(m.Period));
        }
        var rows = await query
            .Include(m => m.HrUser).Include(m => m.Direction)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
        var data = rows.Select(m => new GetRouteEmployeeForMobDto
        {
            ID = m.Id,
            HrUserId = m.HrUserId,
            FirstNane = m.HrUser?.FirstName ?? string.Empty, // ⚠ frozen typo
            MiddleName = m.HrUser?.MiddleName ?? string.Empty,
            LastName = m.HrUser?.LastName ?? string.Empty,
            Mobile = m.HrUser?.Mobile ?? string.Empty,
            Email = m.HrUser?.Email ?? string.Empty,
            Photo = m.HrUser?.ImgPath ?? string.Empty,
            DirectionLatitude = m.Direction?.Latitude ?? 0,
            DirectionLongtitud = m.Direction?.Longitude ?? 0, // ⚠ frozen typo
            DirectionId = m.DirectionId ?? 0,
            DirectionName = m.Direction?.RouteDirection ?? string.Empty,
            Latitude = m.HrUser?.Latitude ?? 0,
            Longtitud = m.HrUser?.Longitude ?? 0, // ⚠ frozen typo
            RegisteredInLine = true,
            CheckIn = string.Empty,
            CheckOut = string.Empty,
        });
        return ServiceResult<IEnumerable<GetRouteEmployeeForMobDto>>.Success(data);
    }

    public async Task<ServiceWriteResult> AddEmployeeAsync(int routeId, List<EmployeeInputDto> data)
    {
        if (routeId == 0) return ServiceWriteResult.Fail("Err101", "TransportationVehicleRouteId is required");
        if (data.Count == 0) return ServiceWriteResult.Fail("Err101", "Data is required");
        await _unitOfWork.RouteEmployees.AddRangeAsync(data.Select(d => new TransportationVehicleRouteEmployee
        {
            RouteId = routeId,
            HrUserId = d.hrUserId,
            DirectionId = d.TransportationVehicleRouteDirectionId is > 0 ? d.TransportationVehicleRouteDirectionId : null,
            Period = d.period ?? "Both",
            Active = d.Active ?? true,
        }).ToList());
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(routeId);
    }

    public async Task<ServiceWriteResult> UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        if (dto.Id == 0) return ServiceWriteResult.Fail("Err101", "Id is required");
        var m = await _unitOfWork.RouteEmployees.FindAsync(x => x.Id == dto.Id);
        if (m == null) return ServiceWriteResult.Fail("Err404", "Membership not found");
        m.HrUserId = dto.hrUserId;
        m.RouteId = dto.TransportationVehicleRouteId;
        m.DirectionId = dto.TransportationVehicleRouteDirectionId is > 0 ? dto.TransportationVehicleRouteDirectionId : null;
        m.Active = dto.Active ?? true;
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(dto.Id);
    }

    public async Task<ServiceWriteResult> DeleteEmployeeAsync(int id)
    {
        if (id == 0) return ServiceWriteResult.Fail("Err101", "Id is required");
        var m = await _unitOfWork.RouteEmployees.FindAsync(x => x.Id == id);
        if (m == null) return ServiceWriteResult.Fail("Err404", "Membership not found");
        _unitOfWork.RouteEmployees.Delete(m);
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(id);
    }

    /* ---- Route memberships (passenger side) ---- */

    public async Task<ServiceResult<IEnumerable<GetHrUserRouteDto>>> GetRoutesForHrUserAsync(int hrUserId)
    {
        if (hrUserId == 0) return ServiceResult<IEnumerable<GetHrUserRouteDto>>.Fail("Err101", "HrUserId is required");
        var rows = await _unitOfWork.RouteEmployees
            .FindAllQueryable(m => m.HrUserId == hrUserId)
            .Include(m => m.Route)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
        var data = rows.Select(m => new GetHrUserRouteDto
        {
            Id = m.Id,
            RouteId = m.RouteId,
            TransportationVehicleRouteDirectionId = m.DirectionId ?? 0,
            Serial = m.Route?.Serial ?? string.Empty,
            NameOfRoute = m.Route?.NameOfRoute ?? string.Empty,
            PeriodFrom = Fmt.Iso(m.Route?.PeriodFrom),
            PeriodTo = Fmt.Iso(m.Route?.PeriodTo),
            Active = m.Active,
            CreationDate = Fmt.Iso(m.CreationDate),
            ModifiedDate = Fmt.Iso(m.Route?.ModifiedDate),
            ToDate = Fmt.Iso(m.ToDate),
            FromDate = Fmt.Iso(m.FromDate),
            Period = m.Period ?? string.Empty,
            DurationLatitude = m.DurationLatitude ?? 0,
            DurationLongtitud = m.DurationLongitude ?? 0, // ⚠ frozen typo
        });
        return ServiceResult<IEnumerable<GetHrUserRouteDto>>.Success(data);
    }

    public async Task<ServiceWriteResult> AddRoutesForEmployeeAsync(int hrUserId, List<RouteForEmployeeInputDto> data)
    {
        if (hrUserId == 0) return ServiceWriteResult.Fail("Err101", "hrUserId is required");
        if (data.Count == 0) return ServiceWriteResult.Fail("Err101", "Data is required");
        await _unitOfWork.RouteEmployees.AddRangeAsync(data.Select(d => new TransportationVehicleRouteEmployee
        {
            HrUserId = hrUserId,
            RouteId = d.RouteId,
            DirectionId = d.TransportationVehicleRouteDirectionId is > 0 ? d.TransportationVehicleRouteDirectionId : null,
            Period = d.Period ?? "Both",
            Active = d.Active ?? true,
            FromDate = ParseDate(d.FromDate),
            ToDate = ParseDate(d.ToDate),
            DurationLatitude = d.DurationLatitude,
            DurationLongitude = d.DurationLongtitud,
        }).ToList());
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(hrUserId);
    }

    public async Task<ServiceWriteResult> UpdateRouteEmployeeAsync(UpdateRouteEmployeeDto dto)
    {
        if (dto.Id == 0) return ServiceWriteResult.Fail("Err101", "Id is required");
        var m = await _unitOfWork.RouteEmployees.FindAsync(x => x.Id == dto.Id);
        if (m == null) return ServiceWriteResult.Fail("Err404", "Membership not found");
        m.RouteId = dto.TransportationVehicleRouteId;
        m.HrUserId = dto.HrUserId;
        m.Active = dto.Active ?? true;
        if (dto.TransportationVehicleRouteDirectionId.HasValue)
            m.DirectionId = dto.TransportationVehicleRouteDirectionId.Value > 0 ? dto.TransportationVehicleRouteDirectionId.Value : null;
        if (dto.FromDate != null) m.FromDate = ParseDate(dto.FromDate);
        if (dto.ToDate != null) m.ToDate = ParseDate(dto.ToDate);
        if (dto.Period != null) m.Period = dto.Period;
        if (dto.DurationLatitude.HasValue) m.DurationLatitude = dto.DurationLatitude;
        if (dto.DurationLongtitud.HasValue) m.DurationLongitude = dto.DurationLongtitud;
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(dto.Id);
    }

    public async Task<ServiceWriteResult> DeleteRouteEmployeeAsync(int id)
    {
        if (id == 0) return ServiceWriteResult.Fail("Err101", "Id is required");
        var m = await _unitOfWork.RouteEmployees.FindAsync(x => x.Id == id);
        if (m == null) return ServiceWriteResult.Fail("Err404", "Membership not found");
        _unitOfWork.RouteEmployees.Delete(m);
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(id);
    }

    /* ---- Passenger (HrUser) profiles ---- */

    public async Task<ServiceResultWithHeader<IEnumerable<GetHrUserProfileDto>>> GetAllHrUsersAsync(int pageNo, int noOfItems, string? name, bool? active)
    {
        var query = _unitOfWork.HrUsers.FindAllQueryable();
        if (active.HasValue) query = query.Where(u => u.Active == active.Value);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(u =>
                (u.FirstName != null && u.FirstName.Contains(name)) ||
                (u.MiddleName != null && u.MiddleName.Contains(name)) ||
                (u.LastName != null && u.LastName.Contains(name)) ||
                (u.IdentityNumber != null && u.IdentityNumber.Contains(name)) ||
                (u.Mobile != null && u.Mobile.Contains(name)));

        var total = await query.CountAsync();
        var rows = await query
            .Include(u => u.MaritalStatus)
            .OrderByDescending(u => u.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .Select(u => new { U = u, RoutesCount = u.Memberships.Count })
            .ToListAsync();

        var data = rows.Select(x => Profile(x.U, x.RoutesCount));
        return ServiceResultWithHeader<IEnumerable<GetHrUserProfileDto>>.SuccessList(
            data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    public async Task<ServiceResult<IEnumerable<MaritalStatusDto>>> GetMaritalStatusAsync()
    {
        var rows = await _unitOfWork.MaritalStatuses.FindAllQueryable().OrderBy(m => m.Id).ToListAsync();
        return ServiceResult<IEnumerable<MaritalStatusDto>>.Success(
            rows.Select(m => new MaritalStatusDto { Id = m.Id, Name = m.Name }));
    }

    public async Task<ServiceResult<GetHrUserProfileDto>> GetHrUserAsync(int id)
    {
        if (id == 0) return ServiceResult<GetHrUserProfileDto>.Fail("Err101", "Id is required");
        var u = await _unitOfWork.HrUsers.FindAsync(x => x.Id == id, new[] { "MaritalStatus" });
        if (u == null) return ServiceResult<GetHrUserProfileDto>.Fail("Err404", "Passenger not found");
        var routesCount = await _unitOfWork.RouteEmployees.CountAsync(m => m.HrUserId == id);
        return ServiceResult<GetHrUserProfileDto>.Success(Profile(u, routesCount));
    }

    private static GetHrUserProfileDto Profile(HrUser u, int routesCount) => new()
    {
        Id = u.Id,
        Name = FullName(u),
        FirstName = u.FirstName ?? string.Empty,
        MiddleName = u.MiddleName ?? string.Empty,
        LastName = u.LastName ?? string.Empty,
        Mobile = u.Mobile ?? string.Empty,
        IdentityNumber = u.IdentityNumber ?? string.Empty,
        MaritalStatusId = u.MaritalStatusId ?? 0,
        MaritalStatusName = u.MaritalStatus?.Name ?? string.Empty,
        ImgPath = u.ImgPath ?? string.Empty,
        Latitude = u.Latitude,
        Longitude = u.Longitude,
        Active = u.Active,
        RoutesCount = routesCount,
    };

    public async Task<ServiceWriteResult> AddHrUserAsync(HrUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName)) return ServiceWriteResult.Fail("Err101", "FirstName is required");
        var created = new HrUser
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Mobile = dto.Mobile,
            IdentityNumber = dto.IdentityNumber,
            MaritalStatusId = dto.MaritalStatusId is > 0 ? dto.MaritalStatusId : null,
            ImgPath = dto.ImgPath,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Email = dto.Email,
            Active = dto.Active ?? true,
        };
        await _unitOfWork.HrUsers.AddAsync(created);
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(created.Id);
    }

    public async Task<ServiceWriteResult> UpdateHrUserAsync(int id, HrUserDto dto)
    {
        if (id == 0) return ServiceWriteResult.Fail("Err101", "Id is required");
        var u = await _unitOfWork.HrUsers.FindAsync(x => x.Id == id);
        if (u == null) return ServiceWriteResult.Fail("Err404", "Passenger not found");
        u.FirstName = dto.FirstName ?? string.Empty;
        u.MiddleName = dto.MiddleName;
        u.LastName = dto.LastName;
        u.Mobile = dto.Mobile;
        u.IdentityNumber = dto.IdentityNumber;
        u.MaritalStatusId = dto.MaritalStatusId is > 0 ? dto.MaritalStatusId : null;
        u.ImgPath = dto.ImgPath;
        u.Latitude = dto.Latitude;
        u.Longitude = dto.Longitude;
        u.Active = dto.Active ?? true;
        await _unitOfWork.CompleteAsync();
        return ServiceWriteResult.SuccessWrite(id);
    }
}
