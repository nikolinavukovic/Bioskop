using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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

        //[Required]
        [AllowNull]
        [ForeignKey("KupovinaID")]
        public Guid KupovinaID { get; set; }

        [JsonIgnore]
        public Projekcija Projekcija { get; set; }

        [JsonIgnore]
        public Sediste Sediste { get; set; }

        [AllowNull]
        [JsonIgnore]
        public Kupovina Kupovina { get; set; }

    }
}
