using Bioskop.Models;
using Bioskop.Models.Dtos;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnikList(string korisnickoIme);
        Korisnik GetKorisnikById(Guid korisnikId);
        Korisnik CreateKorisnik(Korisnik korisnik);
        Korisnik UpdateKorisnik(Korisnik korisnik);
        void DeleteKorisnik(Guid korisnikId);
        Korisnik UserWithCredentialsExists(string korisnickoIme, string lozinka);
        bool UserWithEmailExists(string email);
        bool UserWithUsernameExistst(string username);
        bool SaveChanges();
    }
}
