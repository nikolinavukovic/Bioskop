using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    public class SedisteProjekcije
    {
        [Key]
        [ForeignKey("SedisteID")]
        public Guid SedisteID { get; set; }

        [Key]
        [ForeignKey("ProjekcijaID")]
        public Guid ProjekcijaID { get; set; }

        [Required]
        public float Cena { get; set; }

        [Required]
        [ForeignKey("KupovinaID")]
        public Guid KupovinaID { get; set; }

        [JsonIgnore]
        public Projekcija Projekcija { get; set; }

        [JsonIgnore]
        public Sediste Sediste { get; set; }

        [JsonIgnore]
        public Kupovina Kupovina { get; set; }

    }
}
