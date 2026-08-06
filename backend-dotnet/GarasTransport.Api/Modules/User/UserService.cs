using GarasTransport.Api.Common;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;
using UserEntity = GarasTransport.Domain.Entities.User;

namespace GarasTransport.Api.Modules.User;

public class UserService
{
    private readonly AppDbContext _db;
    private readonly TokenCrypto _crypto;
    private readonly IConfiguration _config;
    private readonly GarasTransport.Infrastructure.Tenancy.TenantRegistry _tenants;

    public UserService(AppDbContext db, TokenCrypto crypto, IConfiguration config, GarasTransport.Infrastructure.Tenancy.TenantRegistry tenants)
    {
        _db = db;
        _crypto = crypto;
        _config = config;
        _tenants = tenants;
    }

    private static string FullName(UserEntity u) =>
        string.Join(' ', new[] { u.FirstName, u.MiddleName, u.LastName }.Where(s => !string.IsNullOrEmpty(s)));

    /// <summary>Split a display name into first / last for the User table (single-field UI).</summary>
    private static (string FirstName, string? LastName) SplitName(string? name)
    {
        var parts = (name ?? string.Empty).Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) return (string.Empty, null);
        if (parts.Length == 1) return (parts[0], null);
        return (parts[0], string.Join(' ', parts.Skip(1)));
    }

    /// <summary>GET /User/GetAllUsers — active LOGIN users with their roles, for pickers.</summary>
    public async Task<object> GetAllUsers()
    {
        var users = await _db.Users
            .Where(u => u.Active)
            .Include(u => u.UserRoles.Where(r => r.Active)).ThenInclude(r => r.Role)
            .OrderBy(u => u.Id)
            .ToListAsync();

        return Envelope.Success(users.Select(u => new
        {
            Id = u.Id,
            Name = string.IsNullOrEmpty(FullName(u)) ? u.Email : FullName(u),
            RoleList = u.UserRoles.Select(r => new { RoleID = r.RoleId, RoleName = r.Role!.Name }),
        }));
    }

    /// <summary>GET /User/GetUserList — paginated login users (optionally by role).</summary>
    public async Task<object> GetUserList(int pageNo = 1, int noOfItems = 20, int? roleId = null)
    {
        var query = _db.Users.Where(u => u.Active);
        if (roleId.HasValue)
            query = query.Where(u => u.UserRoles.Any(r => r.RoleId == roleId.Value && r.Active));

        var total = await query.CountAsync();
        var users = await query
            .Include(u => u.UserRoles.Where(r => r.Active)).ThenInclude(r => r.Role)
            .OrderBy(u => u.Id)
            .Skip((pageNo - 1) * noOfItems)
            .Take(noOfItems)
            .ToListAsync();

        var data = users.Select(u => new
        {
            Id = u.Id,
            Name = string.IsNullOrEmpty(FullName(u)) ? u.Email : FullName(u),
            Email = u.Email,
            Mobile = u.Mobile ?? string.Empty,
            Active = u.Active,
            RoleList = u.UserRoles.Select(r => new { RoleID = r.RoleId, RoleName = r.Role!.Name }),
        });
        return Envelope.SuccessList(data, Envelope.Pagination(pageNo, noOfItems, total));
    }

    // Requested roles: RoleIds list wins; single RoleId is the legacy fallback.
    private static List<int> RequestedRoles(UserAdminDto dto) =>
        (dto.RoleIds is { Count: > 0 } ? dto.RoleIds : dto.RoleId.HasValue ? new List<int> { dto.RoleId.Value } : new List<int>())
            .Distinct().ToList();

    /// <summary>POST /User/AddUser — create a login user and assign one or more roles.</summary>
    public async Task<object> AddUser(UserAdminDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            return Envelope.Fail("Err101", "Email and Password are required");
        var roleIds = RequestedRoles(dto);
        if (roleIds.Count == 0)
            return Envelope.Fail("Err101", "RoleId is required");
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return Envelope.Fail("Err102", "A user with this email already exists");

        var (firstName, lastName) = SplitName(dto.Name);
        var user = new UserEntity
        {
            Email = dto.Email!,
            Password = BCryptNet.HashPassword(dto.Password, 10),
            FirstName = firstName,
            LastName = lastName,
            Mobile = dto.Mobile,
            Active = dto.Active ?? true,
            UserRoles = roleIds.Select(id => new GarasTransport.Domain.Entities.UserRole { RoleId = id, Active = true }).ToList(),
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(user.Id, "User created");
    }

    /// <summary>POST /User/UpdateUser — edit a login user's profile, role and status.</summary>
    public async Task<object> UpdateUser(UserAdminDto dto)
    {
        if (!dto.Id.HasValue) return Envelope.Fail("Err101", "Id is required");
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.Id.Value);
        if (user == null) return Envelope.Fail("Err142", "User not found");

        // Guard against stealing another account's email.
        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
        {
            var clash = await _db.Users.AnyAsync(u => u.Email == dto.Email && u.Id != dto.Id.Value);
            if (clash) return Envelope.Fail("Err102", "A user with this email already exists");
        }

        if (dto.Name != null)
        {
            var (firstName, lastName) = SplitName(dto.Name);
            user.FirstName = firstName;
            user.MiddleName = null;
            user.LastName = lastName;
        }
        if (dto.Email != null) user.Email = dto.Email;
        if (dto.Mobile != null) user.Mobile = string.IsNullOrEmpty(dto.Mobile) ? null : dto.Mobile;
        if (dto.Active.HasValue) user.Active = dto.Active.Value;
        if (!string.IsNullOrEmpty(dto.Password)) user.Password = BCryptNet.HashPassword(dto.Password, 10);
        await _db.SaveChangesAsync();

        // Reconcile roles against the requested set: deactivate the ones no
        // longer wanted, (re)activate/add every requested one. Omitting both
        // RoleIds and RoleId leaves the roles untouched.
        var roleIds = RequestedRoles(dto);
        if (roleIds.Count > 0)
        {
            var existing = await _db.UserRoles
                .Where(r => r.UserId == dto.Id.Value)
                .ToListAsync();
            foreach (var o in existing.Where(r => r.Active && !roleIds.Contains(r.RoleId)))
                o.Active = false;
            foreach (var id in roleIds)
            {
                var current = existing.FirstOrDefault(r => r.RoleId == id);
                if (current != null) current.Active = true;
                else _db.UserRoles.Add(new GarasTransport.Domain.Entities.UserRole { UserId = dto.Id.Value, RoleId = id, Active = true });
            }
            await _db.SaveChangesAsync();
        }
        return Envelope.SuccessWrite(dto.Id.Value, "User updated");
    }

    /// <summary>POST /User/DeleteUser — deactivate a login user and end its sessions.</summary>
    public async Task<object> DeleteUser(int id)
    {
        if (id == 0) return Envelope.Fail("Err101", "Id is required");
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return Envelope.Fail("Err142", "User not found");
        user.Active = false;
        var sessions = await _db.UserSessions.Where(s => s.UserId == id && s.Active).ToListAsync();
        foreach (var s in sessions) s.Active = false;
        await _db.SaveChangesAsync();
        return Envelope.SuccessWrite(id, "User deactivated");
    }

    /// <summary>POST /User/Login — validates company + credentials, opens a session.</summary>
    public async Task<object> Login(LoginDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email) || (string.IsNullOrEmpty(dto.Password) && dto.ExternalLoginFrom != "office365"))
            return Envelope.Fail("Err101", "Email and Password are required");
        // Login carries CompanyName in the BODY (no header yet), so the injected
        // DbContext started on the default connection. Re-target it to the
        // company's own database BEFORE the first query — database-per-company.
        if (!_tenants.TryGetConnectionString(dto.CompanyName, out var tenantCs))
            return Envelope.Fail("Err-P200", "Invalid or unknown CompanyName");
        _db.Database.SetConnectionString(tenantCs);

        var user = await _db.Users
            .Include(u => u.HrUser)
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Active);
        if (user == null) return Envelope.Fail("Err-P6", "Invalid credentials");

        // ⚠ office365 path skips the password check — faithful to the old docs (insecure).
        if (dto.ExternalLoginFrom != "office365")
        {
            var okPass = BCryptNet.Verify(dto.Password ?? string.Empty, user.Password);
            if (!okPass) return Envelope.Fail("Err-P6", "Invalid credentials");
        }

        var hours = int.TryParse(_config["SessionHours"], out var h) ? h : 24;
        var session = new GarasTransport.Domain.Entities.UserSession
        {
            UserId = user.Id,
            Active = true,
            EndDate = DateTime.UtcNow.AddHours(hours),
        };
        _db.UserSessions.Add(session);
        await _db.SaveChangesAsync();
        // Token carries "sessionId|company" so a token can never authenticate
        // against another company's database (ids repeat across tenant DBs).
        var token = Uri.EscapeDataString(_crypto.EncryptSession(session.Id, dto.CompanyName));

        var roles = await _db.UserRoles
            .Include(r => r.Role)
            .Where(r => r.UserId == user.Id && r.Active)
            .ToListAsync();

        return new
        {
            Result = true,
            Errors = Array.Empty<ErrorItem>(),
            Data = token, // LoginResponse.Data = UserToken
            UserID = _crypto.Encrypt(user.Id.ToString()),
            UserIDNO = user.Id,
            Name = FullName(user),
            PhotoUrl = user.PhotoUrl ?? string.Empty,
            BranchId = (int?)user.BranchId,
            EmplyeeId = (int?)user.HrUser?.Id, // ⚠ frozen misspelling: linked HrUser id
            RoleList = roles.Select(r => new { RoleID = r.RoleId, RoleName = r.Role!.Name }),
            GroupList = Array.Empty<object>(),
        };
    }

    /// <summary>POST /User/Logout — deactivates the session behind the token.</summary>
    public async Task<object> Logout(string? token)
    {
        if (string.IsNullOrEmpty(token)) return Envelope.Fail("Err142", "Token required");
        try
        {
            if (_crypto.TryDecryptSession(Uri.UnescapeDataString(token), out var sessionId, out var company) && sessionId != 0)
            {
                // End the session in the company's own database (token is tenant-bound).
                if (_tenants.TryGetConnectionString(company, out var cs))
                    _db.Database.SetConnectionString(cs);
                var sessions = await _db.UserSessions.Where(s => s.Id == sessionId).ToListAsync();
                foreach (var s in sessions) s.Active = false;
                await _db.SaveChangesAsync();
            }
        }
        catch
        {
            /* malformed token — treat as already logged out */
        }
        return new BaseResponse { Result = true, Errors = new() };
    }

    /// <summary>GET /User/GetUserData — rebuild identity for an active session.</summary>
    public async Task<object> GetUserData(int userId)
    {
        var user = await _db.Users.Include(u => u.HrUser).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return Envelope.Fail("Err142", "User not found");
        var roles = await _db.UserRoles
            .Include(r => r.Role)
            .Where(r => r.UserId == userId && r.Active)
            .ToListAsync();
        return Envelope.Success(new
        {
            UserIDNO = user.Id,
            Name = FullName(user),
            PhotoUrl = user.PhotoUrl ?? string.Empty,
            BranchId = (int?)user.BranchId,
            EmplyeeId = (int?)user.HrUser?.Id,
            RoleList = roles.Select(r => new { RoleID = r.RoleId, RoleName = r.Role!.Name }),
        });
    }
}
