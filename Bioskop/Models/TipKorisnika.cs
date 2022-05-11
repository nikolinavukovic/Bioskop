using System;
using System.ComponentModel.DataAnnotations;

namespace Bioskop.Models
{
    public class TipKorisnika
    {
        [Key]
        public Guid TipKorisnikaID { get; set; }
        [Required]
        public string Naziv { get; set; }

    }
}
