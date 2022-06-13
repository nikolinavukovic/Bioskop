﻿using System;

namespace Bioskop.Models.Dtos
{
    public class KorisnikDto
    {
        public Guid KorisnikID { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string Telefon { get; set; }

        public string Email { get; set; }

        public string Lozinka { get; set; }

        public string KorisnickoIme { get; set; }

        public TipKorisnika TipKorisnika { get; set; }

    }
}
