using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace Bioskop.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<TipKorisnika> TipKorisnika { get; set; }
        public DbSet<Zanr> Zanr { get; set; }
        public DbSet<Film> Film { get; set; }
        public DbSet<Projekcija> Projekcija { get; set; }
        public DbSet<Kupovina> Kupovina { get; set; }
        public DbSet<Sediste> Sediste { get; set; }
        public DbSet<SedisteProjekcije> SedisteProjekcije { get; set; }
        public DbSet<ZanrFilma> ZanrFilma { get; set; }

        private Tuple<string, string> HashPassword(string lozinka)
        {
            var sBytes = new byte[lozinka.Length];

            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);

            var salt = Convert.ToBase64String(sBytes);
            var derivedBytes = new Rfc2898DeriveBytes(lozinka, sBytes, 100);

            return new Tuple<string, string>(Convert.ToBase64String(derivedBytes.GetBytes(256)), salt);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ZanrFilma>().HasKey("ZanrID", "FilmID");
            modelBuilder.Entity<SedisteProjekcije>().HasKey("SedisteID", "ProjekcijaID");

            modelBuilder.Entity<TipKorisnika>()
                .HasData(
                new TipKorisnika
                {
                    TipKorisnikaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb"),
                    Naziv = "Registrovani korisnik"
                },
                new TipKorisnika
                {
                    TipKorisnikaID = Guid.Parse("d7a80343-d802-43d6-b128-79ba8554acd2"),
                    Naziv = "Admin"
                });

            modelBuilder.Entity<Korisnik>()
                .HasData(
                new Korisnik
                {
                    KorisnikID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07"),
                    Ime = "Nikolina",
                    Prezime = "Vukovic",
                    Telefon = "063595223",
                    Email = "nikolina.kika23@gmail.com",
                    KorisnickoIme = "kikakika",
                    Lozinka = "lozzzzinka",
                    TipKorisnikaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb")
                },
                new Korisnik
                {
                    KorisnikID = Guid.Parse("94e9fb20-1834-433c-b588-4a6e4eb32150"),
                    Ime = "Petra",
                    Prezime = "Vukovic",
                    Telefon = "063593423",
                    Email = "petrapetra@gmail.com",
                    KorisnickoIme = "petra1",
                    Lozinka = "1233342",
                    TipKorisnikaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb")

                },
                new Korisnik
                {
                    KorisnikID = Guid.Parse("167a01c0-2e68-46a8-b201-3a23e3a20bff"),
                    Ime = "Milenko",
                    Prezime = "Milovac",
                    Telefon = "062593423",
                    Email = "mile@gmail.com",
                    KorisnickoIme = "mile123",
                    Lozinka = "lozinkalozinka",
                    TipKorisnikaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb")

                }
                );

            modelBuilder.Entity<Zanr>()
                .HasData(
                new Zanr
                {
                    ZanrID = Guid.Parse("f876fbcc-a7d0-49f8-b6ef-9b5a59c44fa0"),
                    Naziv = "Dokumentarni"
                },
                new Zanr
                {
                    ZanrID = Guid.Parse("cb5b3279-811c-4ca4-abaa-69016ba157b6"),
                    Naziv = "Animirani"
                },
                new Zanr
                {
                    ZanrID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07"),
                    Naziv = "Drama"
                }
                );

            modelBuilder.Entity<Film>()
                .HasData(
                new Film
                {
                    FilmID = Guid.Parse("94e9fb20-1834-433c-b588-4a6e4eb32150"),
                    Naziv = "Ratatui",
                    Trajanje = 122,
                    Reziser = "Brad Bird",
                    OriginalniNaziv = "Ratatuille",
                    Drzava = "USA",
                    Opis = "Opis",
                    Godina = 2015
                },
                new Film
                {
                    FilmID = Guid.Parse("1ae8137b-1674-4c91-a4b5-87a133f5dd87"),
                    Naziv = "Kraljica od velveta",
                    Trajanje = 121,
                    Reziser = "Marie Amiguet",
                    OriginalniNaziv = "The velvet queen",
                    Drzava = "USA",
                    Godina = 2021
                },
                new Film
                {
                    FilmID = Guid.Parse("3032e0d3-a212-4c56-b36a-be5604dd7646"),
                    Naziv = "Belfast",
                    Trajanje = 144,
                    Reziser = "Kenneth Branagh",
                    OriginalniNaziv = "Belfast",
                    Drzava = "USA",
                    Godina = 2021
                }
                );

            modelBuilder.Entity<ZanrFilma>()
                .HasData(
                new ZanrFilma
                {
                    FilmID = Guid.Parse("94e9fb20-1834-433c-b588-4a6e4eb32150"),
                    ZanrID = Guid.Parse("cb5b3279-811c-4ca4-abaa-69016ba157b6")
                },
                new ZanrFilma
                {
                    FilmID = Guid.Parse("1ae8137b-1674-4c91-a4b5-87a133f5dd87"),
                    ZanrID = Guid.Parse("f876fbcc-a7d0-49f8-b6ef-9b5a59c44fa0")
                },
                new ZanrFilma
                {
                    FilmID = Guid.Parse("3032e0d3-a212-4c56-b36a-be5604dd7646"),
                    ZanrID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07")
                },
                new ZanrFilma
                {
                    FilmID = Guid.Parse("1ae8137b-1674-4c91-a4b5-87a133f5dd87"),
                    ZanrID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07")
                }
                );

            modelBuilder.Entity<Projekcija>()
                .HasData(
                new Projekcija
                {
                    ProjekcijaID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07"),
                    Vreme = DateTime.Now,
                    BrojStampanihKarata = 150,
                    FilmID = Guid.Parse("94e9fb20-1834-433c-b588-4a6e4eb32150")
                },
                new Projekcija
                {
                    ProjekcijaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb"),
                    Vreme = DateTime.Now,
                    BrojStampanihKarata = 150,
                    FilmID = Guid.Parse("1ae8137b-1674-4c91-a4b5-87a133f5dd87")
                },
                new Projekcija
                {
                    ProjekcijaID = Guid.Parse("167a01c0-2e68-46a8-b201-3a23e3a20bff"),
                    Vreme = DateTime.Now,
                    BrojStampanihKarata = 150,
                    FilmID = Guid.Parse("1ae8137b-1674-4c91-a4b5-87a133f5dd87")
                }
                );

            modelBuilder.Entity<Sediste>()
                .HasData(
                new Sediste
                {
                    SedisteID = Guid.Parse("4a1f75a1-b2d9-41e5-af55-2c118ea20423"),
                    BrojReda = 1,
                    BrojSedista = 1
                },
                new Sediste
                {
                    SedisteID = Guid.Parse("58ec047e-00fc-4e84-a4ce-5aaca90638c2"),
                    BrojReda = 1,
                    BrojSedista = 2
                },
                new Sediste
                {
                    SedisteID = Guid.Parse("9fb310e0-927f-4d4d-96a2-c6ee8dcaf9a9"),
                    BrojReda = 1,
                    BrojSedista = 3
                }
                );

            modelBuilder.Entity<SedisteProjekcije>()
                .HasData(
                new SedisteProjekcije
                {
                    SedisteID = Guid.Parse("4a1f75a1-b2d9-41e5-af55-2c118ea20423"),
                    ProjekcijaID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07"),
                    Cena = 350,
                    KupovinaID = Guid.Parse("0ff62e96-3ace-4de3-a515-849a3e901afe")
                },
                new SedisteProjekcije
                {
                    SedisteID = Guid.Parse("58ec047e-00fc-4e84-a4ce-5aaca90638c2"),
                    ProjekcijaID = Guid.Parse("bc679089-e19f-43e4-946f-651ffbdb2afb"),
                    Cena = 350,
                    KupovinaID = Guid.Parse("d655239b-4ca7-4bd4-a154-9e4551976e38")
                },
                new SedisteProjekcije
                {
                    SedisteID = Guid.Parse("9fb310e0-927f-4d4d-96a2-c6ee8dcaf9a9"),
                    ProjekcijaID = Guid.Parse("167a01c0-2e68-46a8-b201-3a23e3a20bff"),
                    Cena = 350,
                    KupovinaID = Guid.Parse("2e026a0e-58b5-4c7a-8a8c-7d92bbc006c4")
                }
                );

            modelBuilder.Entity<Kupovina>()
                .HasData(
                new Kupovina
                {
                    KupovinaID = Guid.Parse("0ff62e96-3ace-4de3-a515-849a3e901afe"),
                    Placeno = true,
                    KorisnikID = Guid.Parse("955f059b-94d9-442f-b7df-4b42538b7e07")
                },
                new Kupovina
                {
                    KupovinaID = Guid.Parse("d655239b-4ca7-4bd4-a154-9e4551976e38"),
                    Placeno = true,
                    KorisnikID = Guid.Parse("94e9fb20-1834-433c-b588-4a6e4eb32150"),
                },
                new Kupovina
                {
                    KupovinaID = Guid.Parse("2e026a0e-58b5-4c7a-8a8c-7d92bbc006c4"),
                    Placeno = false,
                    KorisnikID = Guid.Parse("167a01c0-2e68-46a8-b201-3a23e3a20bff"),
                }
                );

        }
    }
}
