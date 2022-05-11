using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    public class Kupovina
    {
        [Key]
        public Guid KupovinaID { get; set; }

        public float UkupanIznos { get; set; }

        [Required]
        public bool Placeno { get; set; }

        public DateTime VremeRezervacije { get; set; }

        public DateTime VremePlacanja { get; set; }

        [Required]
        [ForeignKey("KorisnikID")]
        public Guid KorisnikID { get; set; }

        [JsonIgnore]
        public Korisnik Korisnik { get; set; }


    }
}
