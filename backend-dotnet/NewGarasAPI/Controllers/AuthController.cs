using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewGarasAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code not found.");
            }

            // Log or process the 'code'
            // Exchange it for an access token (you can implement the logic below)
            return Ok(new { Message = "Authorization successful!", Code = code, State = state });
        }
    }
}
