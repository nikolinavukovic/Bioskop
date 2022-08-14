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
            Korisnik korisnik = authHelper.AuthenticatePrincipal(principal);
            if (korisnik != null)
            {
                var token = authHelper.GenerateJwt(principal, korisnik.TipKorisnika.Naziv);

                return Ok(token);
            }


            return Unauthorized("Pogrešno korisničko ime ili lozinka.");
        }
    }
}