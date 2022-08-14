using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    //Model objekta kupovine jednog ili vise sedista za jednu projekciju
    public class Kupovina
    {
        [Key]
        public Guid KupovinaID { get; set; }

        public float UkupanIznos { get; set; }

        [Required]
        public bool Placeno { get; set; }

        public DateTime VremeRezervacije { get; set; }

        public DateTime VremePlacanja { get; set; }

        //[ForeignKey("KorisnikID")]
        [AllowNull]
        public Guid KorisnikID { get; set; }

        [JsonIgnore]
        public Korisnik Korisnik { get; set; }

        [JsonIgnore]
        public List<SedisteProjekcije> SedistaProjekcije { get; set; }


    }
}
