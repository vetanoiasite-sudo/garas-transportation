using GarasTransport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GarasTransport.Infrastructure.Persistence;

// EF Core context — faithful to the Prisma schema (SQL Server). All FKs use
// NoAction on delete/update (matches Prisma's onUpdate: NoAction, onDelete:
// NoAction), which also sidesteps SQL Server's multiple-cascade-path errors.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<MaritalStatus> MaritalStatuses => Set<MaritalStatus>();
    public DbSet<HrUser> HrUsers => Set<HrUser>();

    // Suppliers
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplierContactPerson> SupplierContactPersons => Set<SupplierContactPerson>();

    // Lines & vehicles & shifts
    public DbSet<BranchSchedule> BranchSchedules => Set<BranchSchedule>();
    public DbSet<TransportationLine> TransportationLines => Set<TransportationLine>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<TransportationVehicle> TransportationVehicles => Set<TransportationVehicle>();

    // Routes
    public DbSet<TransportationVehicleRoute> TransportationVehicleRoutes => Set<TransportationVehicleRoute>();
    public DbSet<TransportationVehicleRouteDirection> TransportationVehicleRouteDirections => Set<TransportationVehicleRouteDirection>();
    public DbSet<TransportationVehicleRouteEmployee> TransportationVehicleRouteEmployees => Set<TransportationVehicleRouteEmployee>();
    public DbSet<TransportationVehicleRouteEmployeeException> TransportationVehicleRouteEmployeeExceptions => Set<TransportationVehicleRouteEmployeeException>();

    // Financials
    public DbSet<TransportationVehicleRouteDeduction> TransportationVehicleRouteDeductions => Set<TransportationVehicleRouteDeduction>();
    public DbSet<TransportationLineIncreaseRequest> TransportationLineIncreaseRequests => Set<TransportationLineIncreaseRequest>();
    public DbSet<TransportationLineIncreaseRequestLine> TransportationLineIncreaseRequestLines => Set<TransportationLineIncreaseRequestLine>();
    public DbSet<TransportionLineExceptionPrice> TransportionLineExceptionPrices => Set<TransportionLineExceptionPrice>();
    public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();
    public DbSet<DistributionSupplierPayment> DistributionSupplierPayments => Set<DistributionSupplierPayment>();
    public DbSet<TransportationVehicleRouteAccount> TransportationVehicleRouteAccounts => Set<TransportationVehicleRouteAccount>();
    public DbSet<TransportationRoundForRoute> TransportationRoundForRoutes => Set<TransportationRoundForRoute>();

    // Attendance
    public DbSet<UserAttendance> UserAttendances => Set<UserAttendance>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // Role.Id is assigned (not identity) — matches Prisma `@id` with no default.
        b.Entity<Role>().Property(r => r.Id).ValueGeneratedNever();
        // MaritalStatus is a fixed lookup (C / M) seeded with explicit ids.
        b.Entity<MaritalStatus>().Property(m => m.Id).ValueGeneratedNever();

        // Unique constraints
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();
        b.Entity<HrUser>().HasIndex(h => h.UserId).IsUnique();

        // Indexes (mirror Prisma @@index)
        b.Entity<UserSession>().HasIndex(x => x.UserId);
        b.Entity<UserRole>().HasIndex(x => x.UserId);
        b.Entity<UserRole>().HasIndex(x => x.RoleId);
        b.Entity<SupplierContactPerson>().HasIndex(x => x.SupplierId);
        b.Entity<BranchSchedule>().HasIndex(x => x.BranchId);
        b.Entity<TransportationVehicle>().HasIndex(x => x.VehicleTypeId);
        b.Entity<TransportationVehicleRoute>().HasIndex(x => x.TransportationLineId);
        b.Entity<TransportationVehicleRoute>().HasIndex(x => x.SupplierId);
        b.Entity<TransportationVehicleRoute>().HasIndex(x => x.Serial);
        b.Entity<TransportationVehicleRouteDirection>().HasIndex(x => x.RouteId);
        b.Entity<TransportationVehicleRouteEmployee>().HasIndex(x => x.RouteId);
        b.Entity<TransportationVehicleRouteEmployee>().HasIndex(x => x.HrUserId);
        b.Entity<TransportationVehicleRouteEmployeeException>().HasIndex(x => x.HrUserId);
        b.Entity<TransportationVehicleRouteEmployeeException>().HasIndex(x => x.RouteId);
        b.Entity<TransportationVehicleRouteDeduction>().HasIndex(x => x.RouteId);
        b.Entity<TransportationVehicleRouteDeduction>().HasIndex(x => x.SupplierId);
        b.Entity<TransportationLineIncreaseRequestLine>().HasIndex(x => x.RequestId);
        b.Entity<TransportionLineExceptionPrice>().HasIndex(x => x.RouteId);
        b.Entity<SupplierPayment>().HasIndex(x => x.SupplierId);
        b.Entity<DistributionSupplierPayment>().HasIndex(x => x.PaymentId);
        b.Entity<TransportationVehicleRouteAccount>().HasIndex(x => x.RouteId);
        b.Entity<TransportationVehicleRouteAccount>().HasIndex(x => x.SupplierId);
        b.Entity<TransportationRoundForRoute>().HasIndex(x => x.RouteId);
        b.Entity<TransportationRoundForRoute>().HasIndex(x => x.Serial);
        b.Entity<UserAttendance>().HasIndex(x => x.Serial);
        b.Entity<Notification>().HasIndex(x => x.UserId);

        // Relationships — all NoAction (Restrict) on delete.
        b.Entity<UserSession>()
            .HasOne(x => x.User).WithMany(u => u.Sessions)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<UserRole>()
            .HasOne(x => x.User).WithMany(u => u.UserRoles)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<UserRole>()
            .HasOne(x => x.Role).WithMany(r => r.UserRoles)
            .HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<HrUser>()
            .HasOne(x => x.User).WithOne(u => u.HrUser)
            .HasForeignKey<HrUser>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<HrUser>()
            .HasOne(x => x.MaritalStatus).WithMany(m => m.HrUsers)
            .HasForeignKey(x => x.MaritalStatusId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<SupplierContactPerson>()
            .HasOne(x => x.Supplier).WithMany(s => s.Contacts)
            .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<TransportationVehicle>()
            .HasOne(x => x.VehicleType).WithMany(t => t.Vehicles)
            .HasForeignKey(x => x.VehicleTypeId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<TransportationVehicleRoute>(e =>
        {
            e.HasOne(x => x.Line).WithMany(l => l.Routes)
                .HasForeignKey(x => x.TransportationLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Vehicle).WithMany(v => v.Routes)
                .HasForeignKey(x => x.TransportationVehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Supplier).WithMany(s => s.Routes)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ContactPerson).WithMany(c => c.Routes)
                .HasForeignKey(x => x.SupplierContactPersonId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.BranchSchedule).WithMany(b => b.Routes)
                .HasForeignKey(x => x.BranchScheduleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Supervisor).WithMany(u => u.SupervisedRoutes)
                .HasForeignKey(x => x.SupervisorId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportationVehicleRouteDirection>()
            .HasOne(x => x.Route).WithMany(r => r.Directions)
            .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<TransportationVehicleRouteEmployee>(e =>
        {
            e.HasOne(x => x.Route).WithMany(r => r.Employees)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.HrUser).WithMany(h => h.Memberships)
                .HasForeignKey(x => x.HrUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Direction).WithMany(d => d.Employees)
                .HasForeignKey(x => x.DirectionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportationVehicleRouteEmployeeException>(e =>
        {
            e.HasOne(x => x.HrUser).WithMany(h => h.Exceptions)
                .HasForeignKey(x => x.HrUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Route).WithMany(r => r.Exceptions)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportationVehicleRouteDeduction>(e =>
        {
            e.HasOne(x => x.Supplier).WithMany(s => s.Deductions)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Route).WithMany(r => r.Deductions)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportationLineIncreaseRequestLine>(e =>
        {
            e.HasOne(x => x.Request).WithMany(r => r.Lines)
                .HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Route).WithMany(r => r.IncreaseLines)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportionLineExceptionPrice>()
            .HasOne(x => x.Route).WithMany(r => r.ExceptionPrices)
            .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<SupplierPayment>()
            .HasOne(x => x.Supplier).WithMany(s => s.Payments)
            .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<DistributionSupplierPayment>()
            .HasOne(x => x.SupplierPayment).WithMany(p => p.Distribution)
            .HasForeignKey(x => x.PaymentId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<TransportationVehicleRouteAccount>(e =>
        {
            e.HasOne(x => x.Route).WithMany(r => r.Accounts)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Supplier).WithMany(s => s.Accounts)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<TransportationRoundForRoute>(e =>
        {
            e.HasOne(x => x.Route).WithMany(r => r.Rounds)
                .HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Supplier).WithMany(s => s.Rounds)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(b);
    }

    // Mirror Prisma @default(now()) / @updatedAt for the audit columns.
    public override int SaveChanges()
    {
        TouchTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TouchTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void TouchTimestamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Modified) continue;
            foreach (var prop in new[] { "Modified", "ModifiedDate" })
            {
                var p = entry.Metadata.FindProperty(prop);
                if (p != null && p.ClrType == typeof(DateTime))
                    entry.Property(prop).CurrentValue = now;
            }
        }
    }
}
