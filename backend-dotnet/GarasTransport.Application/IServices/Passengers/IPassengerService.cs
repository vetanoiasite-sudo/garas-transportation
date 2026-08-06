using GarasTransport.Application.DTOs.Passenger;
using GarasTransport.Application.Responses;

namespace GarasTransport.Application.IServices.Passengers;

/// <summary>
/// Passenger (HrUser) + route-membership business logic (doc §8.1). Every method
/// returns a ServiceResult — no raw nulls, no business-failure exceptions.
/// </summary>
public interface IPassengerService
{
    // Route memberships (route side)
    Task<ServiceResult<IEnumerable<GetRouteEmployeeDto>>> GetEmployeeListAsync(int routeId);
    Task<ServiceResult<IEnumerable<GetRouteEmployeeForMobDto>>> GetEmployeesForMobAsync(int routeId, string? period, string? dateSearch);
    Task<ServiceWriteResult> AddEmployeeAsync(int routeId, List<EmployeeInputDto> data);
    Task<ServiceWriteResult> UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task<ServiceWriteResult> DeleteEmployeeAsync(int id);

    // Route memberships (passenger side)
    Task<ServiceResult<IEnumerable<GetHrUserRouteDto>>> GetRoutesForHrUserAsync(int hrUserId);
    Task<ServiceWriteResult> AddRoutesForEmployeeAsync(int hrUserId, List<RouteForEmployeeInputDto> data);
    Task<ServiceWriteResult> UpdateRouteEmployeeAsync(UpdateRouteEmployeeDto dto);
    Task<ServiceWriteResult> DeleteRouteEmployeeAsync(int id);

    // Passenger profiles
    Task<ServiceResultWithHeader<IEnumerable<GetHrUserProfileDto>>> GetAllHrUsersAsync(int pageNo, int noOfItems, string? name, bool? active);
    Task<ServiceResult<GetHrUserProfileDto>> GetHrUserAsync(int id);
    Task<ServiceResult<IEnumerable<MaritalStatusDto>>> GetMaritalStatusAsync();
    Task<ServiceWriteResult> AddHrUserAsync(HrUserDto dto);
    Task<ServiceWriteResult> UpdateHrUserAsync(int id, HrUserDto dto);
}
