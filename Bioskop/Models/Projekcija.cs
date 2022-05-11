using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    public class Projekcija
    {
        [Key]
        public Guid ProjekcijaID { get; set; }

        [Required]
        public DateTime Vreme { get; set; }

        [Required]
        public int BrojStampanihKarata { get; set; }

        [Required]
        [ForeignKey("FilmID")]
        public Guid FilmID { get; set; }

        [JsonIgnore]
        public Film Film { get; set; }
    }
}
