using Bioskop.Data;
using Bioskop.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bioskop.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration configuration;
        private readonly IKorisnikRepository korisnikRepository;

        public AuthHelper(IConfiguration configuration, IKorisnikRepository korisnikRepository)
        {
            this.configuration = configuration;
            this.korisnikRepository = korisnikRepository;
        }

        public Korisnik AuthenticatePrincipal(Principal principal)
        {
            Korisnik korisnik = korisnikRepository.UserWithCredentialsExists(principal.KorisnickoIme, principal.Lozinka);

            return korisnik;
        }

        public string GenerateJwt(Principal principal, string tipKorisnika)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, principal.KorisnickoIme),
                    new Claim(ClaimTypes.Role, tipKorisnika)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
