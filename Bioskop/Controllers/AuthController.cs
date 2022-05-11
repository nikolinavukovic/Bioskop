using Bioskop.Helpers;
using Bioskop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bioskop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthHelper authHelper;

        public AuthController(IAuthHelper authHelper)
        {
            this.authHelper = authHelper;
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] Principal principal)
        {
            if (authHelper.AuthenticatePrincipal(principal))
            {
                var tokenString = authHelper.GenerateJwt(principal);

                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }
    }
}