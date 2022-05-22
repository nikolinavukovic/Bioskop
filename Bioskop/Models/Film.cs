using System;
using System.ComponentModel.DataAnnotations;

namespace Bioskop.Models
{
    public class Film
    {
        [Key]
        public Guid FilmID { get; set; }

        [Required]
        public string Naziv { get; set; }

        public int Trajanje { get; set; }

        public string Reziser { get; set; }

        public string OriginalniNaziv { get; set; }

        public string Drzava { get; set; }

        public string Opis { get; set; }

        public int Godina { get; set; }

    }
}
