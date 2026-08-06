using GarasTransport.Domain.Entities;

namespace GarasTransport.Domain.IRepository;

/// <summary>
/// Unit of Work contract (doc §9.3): one repository property per aggregate plus
/// explicit transaction control. Services depend on this, never on the DbContext.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Identity
    IBaseRepository<User, int> Users { get; }
    IBaseRepository<UserSession, int> UserSessions { get; }
    IBaseRepository<Role, int> Roles { get; }
    IBaseRepository<UserRole, int> UserRoles { get; }
    IBaseRepository<MaritalStatus, int> MaritalStatuses { get; }
    IBaseRepository<HrUser, int> HrUsers { get; }

    // Suppliers
    IBaseRepository<Supplier, int> Suppliers { get; }
    IBaseRepository<SupplierContactPerson, int> SupplierContactPersons { get; }

    // Lines & vehicles
    IBaseRepository<BranchSchedule, int> BranchSchedules { get; }
    IBaseRepository<TransportationLine, int> TransportationLines { get; }
    IBaseRepository<VehicleType, int> VehicleTypes { get; }
    IBaseRepository<TransportationVehicle, int> TransportationVehicles { get; }

    // Routes
    IBaseRepository<TransportationVehicleRoute, int> Routes { get; }
    IBaseRepository<TransportationVehicleRouteDirection, int> RouteDirections { get; }
    IBaseRepository<TransportationVehicleRouteEmployee, int> RouteEmployees { get; }
    IBaseRepository<TransportationVehicleRouteEmployeeException, int> RouteEmployeeExceptions { get; }

    // Financials
    IBaseRepository<TransportationVehicleRouteDeduction, int> Deductions { get; }
    IBaseRepository<TransportationLineIncreaseRequest, int> IncreaseRequests { get; }
    IBaseRepository<TransportationLineIncreaseRequestLine, int> IncreaseRequestLines { get; }
    IBaseRepository<TransportionLineExceptionPrice, int> ExceptionPrices { get; }
    IBaseRepository<SupplierPayment, int> SupplierPayments { get; }
    IBaseRepository<DistributionSupplierPayment, int> DistributionSupplierPayments { get; }
    IBaseRepository<TransportationVehicleRouteAccount, int> RouteAccounts { get; }
    IBaseRepository<TransportationRoundForRoute, int> Rounds { get; }

    // Attendance & notifications
    IBaseRepository<UserAttendance, int> UserAttendances { get; }
    IBaseRepository<Notification, int> Notifications { get; }

    int Complete();
    Task<int> CompleteAsync();

    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
