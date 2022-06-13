using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bioskop.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    FilmID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    Reziser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalniNaziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Drzava = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Godina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.FilmID);
                });

            migrationBuilder.CreateTable(
                name: "Sediste",
                columns: table => new
                {
                    SedisteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojReda = table.Column<int>(type: "int", nullable: false),
                    BrojSedista = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sediste", x => x.SedisteID);
                });

            migrationBuilder.CreateTable(
                name: "TipKorisnika",
                columns: table => new
                {
                    TipKorisnikaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipKorisnika", x => x.TipKorisnikaID);
                });

            migrationBuilder.CreateTable(
                name: "Zanr",
                columns: table => new
                {
                    ZanrID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zanr", x => x.ZanrID);
                });

            migrationBuilder.CreateTable(
                name: "Projekcija",
                columns: table => new
                {
                    ProjekcijaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vreme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrojStampanihKarata = table.Column<int>(type: "int", nullable: false),
                    FilmID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekcija", x => x.ProjekcijaID);
                    table.ForeignKey(
                        name: "FK_Projekcija_Film_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Film",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipKorisnikaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.KorisnikID);
                    table.ForeignKey(
                        name: "FK_Korisnik_TipKorisnika_TipKorisnikaID",
                        column: x => x.TipKorisnikaID,
                        principalTable: "TipKorisnika",
                        principalColumn: "TipKorisnikaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZanrFilma",
                columns: table => new
                {
                    ZanrID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilmID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZanrFilma", x => new { x.ZanrID, x.FilmID });
                    table.ForeignKey(
                        name: "FK_ZanrFilma_Film_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Film",
                        principalColumn: "FilmID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZanrFilma_Zanr_ZanrID",
                        column: x => x.ZanrID,
                        principalTable: "Zanr",
                        principalColumn: "ZanrID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kupovina",
                columns: table => new
                {
                    KupovinaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UkupanIznos = table.Column<float>(type: "real", nullable: false),
                    Placeno = table.Column<bool>(type: "bit", nullable: false),
                    VremeRezervacije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VremePlacanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupovina", x => x.KupovinaID);
                    table.ForeignKey(
                        name: "FK_Kupovina_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SedisteProjekcije",
                columns: table => new
                {
                    SedisteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjekcijaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cena = table.Column<float>(type: "real", nullable: false),
                    KupovinaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SedisteProjekcije", x => new { x.SedisteID, x.ProjekcijaID });
                    table.ForeignKey(
                        name: "FK_SedisteProjekcije_Kupovina_KupovinaID",
                        column: x => x.KupovinaID,
                        principalTable: "Kupovina",
                        principalColumn: "KupovinaID",
                        onDelete: ReferentialAction.SetNull
                    );
                    table.ForeignKey(
                        name: "FK_SedisteProjekcije_Projekcija_ProjekcijaID",
                        column: x => x.ProjekcijaID,
                        principalTable: "Projekcija",
                        principalColumn: "ProjekcijaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SedisteProjekcije_Sediste_SedisteID",
                        column: x => x.SedisteID,
                        principalTable: "Sediste",
                        principalColumn: "SedisteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Film",
                columns: new[] { "FilmID", "Drzava", "Godina", "Naziv", "Opis", "OriginalniNaziv", "Reziser", "Trajanje" },
                values: new object[,]
                {
                    { new Guid("94e9fb20-1834-433c-b588-4a6e4eb32150"), "USA", 2015, "Ratatui", "Opis", "Ratatuille", "Brad Bird", 122 },
                    { new Guid("1ae8137b-1674-4c91-a4b5-87a133f5dd87"), "USA", 2021, "Kraljica od velveta", null, "The velvet queen", "Marie Amiguet", 121 },
                    { new Guid("3032e0d3-a212-4c56-b36a-be5604dd7646"), "USA", 2021, "Belfast", null, "Belfast", "Kenneth Branagh", 144 }
                });

            migrationBuilder.InsertData(
                table: "Sediste",
                columns: new[] { "SedisteID", "BrojReda", "BrojSedista" },
                values: new object[,]
                {
                    { new Guid("4a1f75a1-b2d9-41e5-af55-2c118ea20423"), 1, 1 },
                    { new Guid("58ec047e-00fc-4e84-a4ce-5aaca90638c2"), 1, 2 },
                    { new Guid("9fb310e0-927f-4d4d-96a2-c6ee8dcaf9a9"), 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "TipKorisnika",
                columns: new[] { "TipKorisnikaID", "Naziv" },
                values: new object[,]
                {
                    { new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb"), "Registrovani korisnik" },
                    { new Guid("0475c5d6-8db1-461e-84f4-81a22451a834"), "Zaposleni" },
                    { new Guid("d7a80343-d802-43d6-b128-79ba8554acd2"), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Zanr",
                columns: new[] { "ZanrID", "Naziv" },
                values: new object[,]
                {
                    { new Guid("f876fbcc-a7d0-49f8-b6ef-9b5a59c44fa0"), "Dokumentarni" },
                    { new Guid("cb5b3279-811c-4ca4-abaa-69016ba157b6"), "Animirani" },
                    { new Guid("955f059b-94d9-442f-b7df-4b42538b7e07"), "Drama" }
                });

            migrationBuilder.InsertData(
                table: "Korisnik",
                columns: new[] { "KorisnikID", "Email", "Ime", "KorisnickoIme", "Lozinka", "Prezime", "Salt", "Telefon", "TipKorisnikaID" },
                values: new object[,]
                {
                    { new Guid("955f059b-94d9-442f-b7df-4b42538b7e07"), "nikolina.kika23@gmail.com", "Nikolina", "kikakika", "lozzzzinka", "Vukovic", null, "063595223", new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb") },
                    { new Guid("94e9fb20-1834-433c-b588-4a6e4eb32150"), "petrapetra@gmail.com", "Petra", "petra1", "1233342", "Vukovic", null, "063593423", new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb") },
                    { new Guid("167a01c0-2e68-46a8-b201-3a23e3a20bff"), "mile@gmail.com", "Milenko", "mile123", "lozinkalozinka", "Milovac", null, "062593423", new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb") }
                });

            migrationBuilder.InsertData(
                table: "Projekcija",
                columns: new[] { "ProjekcijaID", "BrojStampanihKarata", "FilmID", "Vreme" },
                values: new object[,]
                {
                    { new Guid("955f059b-94d9-442f-b7df-4b42538b7e07"), 150, new Guid("94e9fb20-1834-433c-b588-4a6e4eb32150"), new DateTime(2022, 6, 11, 14, 11, 48, 335, DateTimeKind.Local).AddTicks(8759) },
                    { new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb"), 150, new Guid("1ae8137b-1674-4c91-a4b5-87a133f5dd87"), new DateTime(2022, 6, 11, 14, 11, 48, 360, DateTimeKind.Local).AddTicks(4036) },
                    { new Guid("167a01c0-2e68-46a8-b201-3a23e3a20bff"), 150, new Guid("1ae8137b-1674-4c91-a4b5-87a133f5dd87"), new DateTime(2022, 6, 11, 14, 11, 48, 360, DateTimeKind.Local).AddTicks(4101) }
                });

            migrationBuilder.InsertData(
                table: "ZanrFilma",
                columns: new[] { "FilmID", "ZanrID" },
                values: new object[,]
                {
                    { new Guid("1ae8137b-1674-4c91-a4b5-87a133f5dd87"), new Guid("f876fbcc-a7d0-49f8-b6ef-9b5a59c44fa0") },
                    { new Guid("94e9fb20-1834-433c-b588-4a6e4eb32150"), new Guid("cb5b3279-811c-4ca4-abaa-69016ba157b6") },
                    { new Guid("3032e0d3-a212-4c56-b36a-be5604dd7646"), new Guid("955f059b-94d9-442f-b7df-4b42538b7e07") },
                    { new Guid("1ae8137b-1674-4c91-a4b5-87a133f5dd87"), new Guid("955f059b-94d9-442f-b7df-4b42538b7e07") }
                });

            migrationBuilder.InsertData(
                table: "Kupovina",
                columns: new[] { "KupovinaID", "KorisnikID", "Placeno", "UkupanIznos", "VremePlacanja", "VremeRezervacije" },
                values: new object[] { new Guid("0ff62e96-3ace-4de3-a515-849a3e901afe"), new Guid("955f059b-94d9-442f-b7df-4b42538b7e07"), true, 0f, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Kupovina",
                columns: new[] { "KupovinaID", "KorisnikID", "Placeno", "UkupanIznos", "VremePlacanja", "VremeRezervacije" },
                values: new object[] { new Guid("d655239b-4ca7-4bd4-a154-9e4551976e38"), new Guid("94e9fb20-1834-433c-b588-4a6e4eb32150"), true, 0f, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Kupovina",
                columns: new[] { "KupovinaID", "KorisnikID", "Placeno", "UkupanIznos", "VremePlacanja", "VremeRezervacije" },
                values: new object[] { new Guid("2e026a0e-58b5-4c7a-8a8c-7d92bbc006c4"), new Guid("167a01c0-2e68-46a8-b201-3a23e3a20bff"), false, 0f, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SedisteProjekcije",
                columns: new[] { "ProjekcijaID", "SedisteID", "Cena", "KupovinaID" },
                values: new object[] { new Guid("955f059b-94d9-442f-b7df-4b42538b7e07"), new Guid("4a1f75a1-b2d9-41e5-af55-2c118ea20423"), 350f, new Guid("0ff62e96-3ace-4de3-a515-849a3e901afe") });

            migrationBuilder.InsertData(
                table: "SedisteProjekcije",
                columns: new[] { "ProjekcijaID", "SedisteID", "Cena", "KupovinaID" },
                values: new object[] { new Guid("bc679089-e19f-43e4-946f-651ffbdb2afb"), new Guid("58ec047e-00fc-4e84-a4ce-5aaca90638c2"), 350f, new Guid("d655239b-4ca7-4bd4-a154-9e4551976e38") });

            migrationBuilder.InsertData(
                table: "SedisteProjekcije",
                columns: new[] { "ProjekcijaID", "SedisteID", "Cena", "KupovinaID" },
                values: new object[] { new Guid("167a01c0-2e68-46a8-b201-3a23e3a20bff"), new Guid("9fb310e0-927f-4d4d-96a2-c6ee8dcaf9a9"), 350f, new Guid("2e026a0e-58b5-4c7a-8a8c-7d92bbc006c4") });

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_TipKorisnikaID",
                table: "Korisnik",
                column: "TipKorisnikaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kupovina_KorisnikID",
                table: "Kupovina",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_FilmID",
                table: "Projekcija",
                column: "FilmID");

            migrationBuilder.CreateIndex(
                name: "IX_SedisteProjekcije_KupovinaID",
                table: "SedisteProjekcije",
                column: "KupovinaID");

            migrationBuilder.CreateIndex(
                name: "IX_SedisteProjekcije_ProjekcijaID",
                table: "SedisteProjekcije",
                column: "ProjekcijaID");

            migrationBuilder.CreateIndex(
                name: "IX_ZanrFilma_FilmID",
                table: "ZanrFilma",
                column: "FilmID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SedisteProjekcije");

            migrationBuilder.DropTable(
                name: "ZanrFilma");

            migrationBuilder.DropTable(
                name: "Kupovina");

            migrationBuilder.DropTable(
                name: "Projekcija");

            migrationBuilder.DropTable(
                name: "Sediste");

            migrationBuilder.DropTable(
                name: "Zanr");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "TipKorisnika");
        }
    }
}
