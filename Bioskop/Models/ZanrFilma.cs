using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bioskop.Models
{
    public class ZanrFilma
    {
        [Key]
        public Guid ZanrID { get; set; }

        [Key]
        public Guid FilmID { get; set; }

        [JsonIgnore]
        public Zanr Zanr { get; set; } 
        [JsonIgnore]
        public Film Film { get; set; }

    }
}
