using GarasTransport.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

namespace GarasTransport.Infrastructure.Persistence.Initializers;

// Seed — roles + one login per panel role + supervisors + the "other identifier"
// lookup (C / M). Faithful to the NestJS prisma/seed.ts identity slice, so the
// app can be logged into out of the box. Idempotent.
public static class DbInitializer
{
    // The system has exactly four panel roles + two mobile-only roles (supervisor,
    // passenger). Ids are the backend transportation role ids the frontend maps.
    private static readonly (int Id, string Name)[] Roles =
    {
        (216, "Transportation Super Admin"), // مسؤول عام
        (213, "Transportation Admin"),       // مسئول المواصلات
        (220, "Transportation HR Admin"),    // مسئول الموظفين
        (221, "Transportation Reader"),      // مشاهد
        (214, "Transportation Supervisor"),  // mobile only
        (215, "Transportation Passenger"),   // mobile only
    };

    // Legacy roles that were consolidated away — dropped from the DB on seed.
    private static readonly int[] RemovedRoleIds = { 137, 210, 30 };

    // One login account per panel role (all share the demo password).
    private static readonly (string Email, string First, string Last, int RoleId)[] RoleAccounts =
    {
        ("admin@garas.co", "أحمد", "النظام", 216),
        ("transport@garas.co", "محمد", "المواصلات", 213),
        ("hr@garas.co", "خالد", "الموظفين", 220),
        ("viewer@garas.co", "سلمى", "المشاهدة", 221),
    };

    private static readonly (string Email, string First, string Last)[] Supervisors =
    {
        ("ali.supervisor@garas.co", "علي", "المشرف"),
        ("mona.supervisor@garas.co", "منى", "صابر"),
        ("khaled.supervisor@garas.co", "خالد", "فؤاد"),
    };

    private static readonly (int Id, string Name)[] MaritalStatuses = { (1, "C"), (2, "M") };

    public static async Task SeedAsync(AppDbContext db)
    {
        // Roles.
        foreach (var (id, name) in Roles)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) db.Roles.Add(new Role { Id = id, Name = name });
            else role.Name = name;
        }
        await db.SaveChangesAsync();

        // Purge the consolidated-away roles and any links to them.
        var deadLinks = await db.UserRoles.Where(ur => RemovedRoleIds.Contains(ur.RoleId)).ToListAsync();
        db.UserRoles.RemoveRange(deadLinks);
        var deadRoles = await db.Roles.Where(r => RemovedRoleIds.Contains(r.Id)).ToListAsync();
        db.Roles.RemoveRange(deadRoles);
        await db.SaveChangesAsync();

        var password = BCryptNet.HashPassword("demo1234", 10);

        // One login per panel role, each carrying ONLY its designated role.
        foreach (var (email, first, last, roleId) in RoleAccounts)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User { Email = email, Password = password, FirstName = first, LastName = last, Active = true, BranchId = 1 };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            else
            {
                user.FirstName = first;
                user.LastName = last;
                user.Active = true;
                await db.SaveChangesAsync();
            }
            var wrongRoles = await db.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId != roleId).ToListAsync();
            db.UserRoles.RemoveRange(wrongRoles);
            if (!await db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == roleId))
                db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId, Active = true });
            await db.SaveChangesAsync();
        }

        // Supervisors (login users with role 214).
        foreach (var (email, first, last) in Supervisors)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User { Email = email, Password = password, FirstName = first, LastName = last, Active = true, BranchId = 1 };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            else
            {
                user.FirstName = first;
                user.LastName = last;
                user.Active = true;
                await db.SaveChangesAsync();
            }
            if (!await db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == 214))
                db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = 214, Active = true });
            await db.SaveChangesAsync();
        }

        // "Other identifier" lookup — opaque codes C / M (fixed ids, idempotent).
        foreach (var (id, name) in MaritalStatuses)
        {
            var ms = await db.MaritalStatuses.FirstOrDefaultAsync(m => m.Id == id);
            if (ms == null) db.MaritalStatuses.Add(new MaritalStatus { Id = id, Name = name });
            else ms.Name = name;
        }
        await db.SaveChangesAsync();
    }
}
