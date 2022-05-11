using Bioskop.Data;
using Bioskop.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        public bool AuthenticatePrincipal(Principal principal)
        {
            if (korisnikRepository.UserWithCredentialsExists(principal.KorisnickoIme, principal.Lozinka))
            {
                return true;
            }

            return false;
        }

        public string GenerateJwt(Principal principal)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
