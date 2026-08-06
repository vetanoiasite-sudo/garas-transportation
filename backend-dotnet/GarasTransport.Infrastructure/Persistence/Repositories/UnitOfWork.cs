using GarasTransport.Domain.Entities;
using GarasTransport.Domain.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace GarasTransport.Infrastructure.Persistence.Repositories;

/// <summary>Unit of Work (doc §9.3) — owns the DbContext, exposes lazily-created repositories.</summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context) => _context = context;

    private readonly Dictionary<Type, object> _repos = new();

    private IBaseRepository<TEntity, int> Repo<TEntity>() where TEntity : class
    {
        if (_repos.TryGetValue(typeof(TEntity), out var r)) return (IBaseRepository<TEntity, int>)r;
        var created = new BaseRepository<TEntity, int>(_context);
        _repos[typeof(TEntity)] = created;
        return created;
    }

    // Identity
    public IBaseRepository<User, int> Users => Repo<User>();
    public IBaseRepository<UserSession, int> UserSessions => Repo<UserSession>();
    public IBaseRepository<Role, int> Roles => Repo<Role>();
    public IBaseRepository<UserRole, int> UserRoles => Repo<UserRole>();
    public IBaseRepository<MaritalStatus, int> MaritalStatuses => Repo<MaritalStatus>();
    public IBaseRepository<HrUser, int> HrUsers => Repo<HrUser>();

    // Suppliers
    public IBaseRepository<Supplier, int> Suppliers => Repo<Supplier>();
    public IBaseRepository<SupplierContactPerson, int> SupplierContactPersons => Repo<SupplierContactPerson>();

    // Lines & vehicles
    public IBaseRepository<BranchSchedule, int> BranchSchedules => Repo<BranchSchedule>();
    public IBaseRepository<TransportationLine, int> TransportationLines => Repo<TransportationLine>();
    public IBaseRepository<VehicleType, int> VehicleTypes => Repo<VehicleType>();
    public IBaseRepository<TransportationVehicle, int> TransportationVehicles => Repo<TransportationVehicle>();

    // Routes
    public IBaseRepository<TransportationVehicleRoute, int> Routes => Repo<TransportationVehicleRoute>();
    public IBaseRepository<TransportationVehicleRouteDirection, int> RouteDirections => Repo<TransportationVehicleRouteDirection>();
    public IBaseRepository<TransportationVehicleRouteEmployee, int> RouteEmployees => Repo<TransportationVehicleRouteEmployee>();
    public IBaseRepository<TransportationVehicleRouteEmployeeException, int> RouteEmployeeExceptions => Repo<TransportationVehicleRouteEmployeeException>();

    // Financials
    public IBaseRepository<TransportationVehicleRouteDeduction, int> Deductions => Repo<TransportationVehicleRouteDeduction>();
    public IBaseRepository<TransportationLineIncreaseRequest, int> IncreaseRequests => Repo<TransportationLineIncreaseRequest>();
    public IBaseRepository<TransportationLineIncreaseRequestLine, int> IncreaseRequestLines => Repo<TransportationLineIncreaseRequestLine>();
    public IBaseRepository<TransportionLineExceptionPrice, int> ExceptionPrices => Repo<TransportionLineExceptionPrice>();
    public IBaseRepository<SupplierPayment, int> SupplierPayments => Repo<SupplierPayment>();
    public IBaseRepository<DistributionSupplierPayment, int> DistributionSupplierPayments => Repo<DistributionSupplierPayment>();
    public IBaseRepository<TransportationVehicleRouteAccount, int> RouteAccounts => Repo<TransportationVehicleRouteAccount>();
    public IBaseRepository<TransportationRoundForRoute, int> Rounds => Repo<TransportationRoundForRoute>();

    // Attendance & notifications
    public IBaseRepository<UserAttendance, int> UserAttendances => Repo<UserAttendance>();
    public IBaseRepository<Notification, int> Notifications => Repo<Notification>();

    public int Complete() => _context.SaveChanges();
    public Task<int> CompleteAsync() => _context.SaveChangesAsync();

    public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
