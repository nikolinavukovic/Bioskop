using Bioskop.Models;

namespace Bioskop.Helpers
{
    public interface IAuthHelper
    {
        Korisnik AuthenticatePrincipal(Principal principal);
        string GenerateJwt(Principal principal, string NazivTipaKorisnika);
    }
}
