using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bioskop.Models
{
    public class Transakcija
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[JsonIgnore]
        //public List<SedisteProjekcije> SedistaProjekcije { get; set; }
        //[NotMapped]
        public string SedistaId { get; set; }
        public Guid ProjekcijaId { get; set; }
        public Guid KupovinaId { get; set; }
        public Guid KorisnikId { get; set; }
    }
}
