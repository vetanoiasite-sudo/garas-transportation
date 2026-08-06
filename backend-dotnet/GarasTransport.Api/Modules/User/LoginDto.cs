namespace GarasTransport.Api.Modules.User;

// POST /User/Login body (UserLogin). Field names match the documented contract.
public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? ExternalLoginFrom { get; set; }
}

/// <summary>Payload for creating / editing a login user from the Users admin screen.</summary>
public class UserAdminDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Password { get; set; }
    public int? RoleId { get; set; }
    // Multi-role: full list of role ids. Takes precedence over RoleId when
    // non-empty (RoleId kept for backward compatibility with older clients).
    public List<int>? RoleIds { get; set; }
    public bool? Active { get; set; }
}
