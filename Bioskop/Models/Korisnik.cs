using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    public class Korisnik
    {
        [Key]
        public Guid KorisnikID { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        public string Telefon { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Lozinka { get; set; }

        public string Salt { get; set; }

        public Guid TipKorisnikaID { get; set; }

        [JsonIgnore]
        public TipKorisnika TipKorisnika { get; set; }


    }
}
