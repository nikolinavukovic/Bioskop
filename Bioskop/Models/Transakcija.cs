using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bioskop.Models
{
    //Model objekta koji se unosi u bazu prilikom stripe payment-a i koji se kasnije iscitava
    public class Transakcija
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SedistaId { get; set; }
        public Guid ProjekcijaId { get; set; }
        public Guid KupovinaId { get; set; }
        public Guid KorisnikId { get; set; }
    }
}
