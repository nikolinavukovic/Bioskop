using AutoMapper;
using Bioskop.Models;
using Bioskop.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Bioskop.Data
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly DatabaseContext Context;
        private readonly IMapper Mapper;
        public List<Korisnik> KorisnikList { get; set; } = new List<Korisnik>();

        public KorisnikRepository(DatabaseContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;

            KorisnikList.AddRange(GetKorisnikList());
        }

        public List<Korisnik> GetKorisnikList(string korisnickoIme = default)
        {
            return Context.Korisnik.Where(e => (korisnickoIme == default || e.KorisnickoIme.Equals(korisnickoIme))).Include(r => r.TipKorisnika).ToList();
        }

        public Korisnik GetKorisnikById(Guid korisnikId)
        {
            return Context.Korisnik.Include(r => r.TipKorisnika).FirstOrDefault(e => e.KorisnikID == korisnikId);
        }

        public Korisnik CreateKorisnik(Korisnik korisnik)
        {
            korisnik.KorisnikID = Guid.NewGuid();

            var sifra = HashPassword(korisnik.Lozinka);

            korisnik.Lozinka = sifra.Item1;
            korisnik.Salt = sifra.Item2;

            Context.Korisnik.Add(korisnik);
            Context.SaveChanges();

            return Mapper.Map<Korisnik>(korisnik);
        }

        public Korisnik UpdateKorisnik(Korisnik korisnik)
        {
            Korisnik k = Context.Korisnik.Include(r => r.TipKorisnika).FirstOrDefault(e => e.KorisnikID == korisnik.KorisnikID);

            if (k == null)
                throw new EntryPointNotFoundException();

            k.KorisnikID = korisnik.KorisnikID;
            k.Ime = korisnik.Ime;
            k.Prezime = korisnik.Prezime;
            k.Telefon = korisnik.Telefon;
            k.Email = korisnik.Email;
            k.KorisnickoIme = korisnik.KorisnickoIme;
            k.Lozinka = korisnik.Lozinka;

            var sifra = HashPassword(korisnik.Lozinka);

            k.Lozinka = sifra.Item1;
            k.Salt = sifra.Item2;

            Context.SaveChanges();

            return Mapper.Map<Korisnik>(k);
        }

        public void DeleteKorisnik(Guid korisnikId)
        {
            Korisnik k = Context.Korisnik.FirstOrDefault(e => e.KorisnikID == korisnikId);

            if (k == null)
                throw new EntryPointNotFoundException();

            Context.Korisnik.Remove(k);
            Context.SaveChanges();
        }

        public bool UserWithEmailExists(string email)
        {
            Korisnik korisnik = Context.Korisnik.FirstOrDefault(k => k.Email == email);

            if (korisnik == null)
            {
                return false;
            }

            return true;
        }

        public Korisnik UserWithCredentialsExists(string korisnickoIme, string lozinka)
        {
            Korisnik korisnik = Context.Korisnik.Include(r => r.TipKorisnika).FirstOrDefault(k => k.KorisnickoIme == korisnickoIme);

            if (korisnik == null)
            {
                return null;
            }

            if (VerifyPassword(lozinka, korisnik.Lozinka, korisnik.Salt))
            {
                return korisnik;
            }

            return null;
        }
        private Tuple<string, string> HashPassword(string lozinka)
        {
            var sBytes = new byte[lozinka.Length];

            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);

            var salt = Convert.ToBase64String(sBytes);
            var derivedBytes = new Rfc2898DeriveBytes(lozinka, sBytes, 100);

            return new Tuple<string, string>(Convert.ToBase64String(derivedBytes.GetBytes(256)), salt);
        }

        private bool VerifyPassword(string lozinka, string savedLozinka, string savedSalt)
        {
            var saltBytes = Convert.FromBase64String(savedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(lozinka, saltBytes, 100);

            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == savedLozinka;
        }

        public bool UserWithUsernameExistst(string username)
        {
            Korisnik korisnik = Context.Korisnik.FirstOrDefault(k => k.KorisnickoIme == username);

            if (korisnik == null)
            {
                return false;
            }

            return true;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
