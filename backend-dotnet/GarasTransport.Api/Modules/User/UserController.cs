using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.User;

// Route base is /User (no api/ prefix — matches the documented contract).
[Route("User")]
public class UserController : ApiControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service) => _service = service;

    [HttpPost("Login")]
    public Task<object> Login([FromBody] LoginDto dto) => _service.Login(dto);

    [HttpPost("Logout")]
    public Task<object> Logout() => _service.Logout(Header("UserToken"));

    [HttpGet("GetUserData")]
    [HeaderAuth]
    public Task<object> GetUserData() => _service.GetUserData(CurrentUser.UserId);

    /// <summary>GET /User/GetAllUsers — login users (with roles) for supervisor pickers.</summary>
    [HttpGet("GetAllUsers")]
    [HeaderAuth]
    public Task<object> GetAllUsers() => _service.GetAllUsers();

    /// <summary>GET /User/GetUserList — paginated users for the Users admin screen.</summary>
    [HttpGet("GetUserList")]
    [HeaderAuth]
    public Task<object> GetUserList() =>
        _service.GetUserList(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), HeaderIntOrNull("RoleId"));

    /// <summary>POST /User/AddUser — create a login user with a role.</summary>
    [HttpPost("AddUser")]
    [HeaderAuth]
    public Task<object> AddUser([FromBody] UserAdminDto body) => _service.AddUser(body);

    /// <summary>POST /User/UpdateUser — edit a login user's profile, role and status.</summary>
    [HttpPost("UpdateUser")]
    [HeaderAuth]
    public Task<object> UpdateUser([FromBody] UserAdminDto body) => _service.UpdateUser(body);

    /// <summary>POST /User/DeleteUser — deactivate a login user (Id header).</summary>
    [HttpPost("DeleteUser")]
    [HeaderAuth]
    public Task<object> DeleteUser() => _service.DeleteUser(HeaderInt("Id", 0));
}
