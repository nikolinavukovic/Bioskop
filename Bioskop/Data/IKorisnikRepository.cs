using Bioskop.Models;
using System;
using System.Collections.Generic;

namespace Bioskop.Data
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnikList();
        Korisnik GetKorisnikById(Guid korisnikId);
        Korisnik CreateKorisnik(Korisnik korisnik);
        Korisnik UpdateKorisnik(Korisnik korisnik);
        void DeleteKorisnik(Guid korisnikId);
        bool UserWithCredentialsExists(string korisnickoIme, string lozinka);
        bool SaveChanges();
    }
}
