using System;
using System.ComponentModel.DataAnnotations;

namespace Bioskop.Models
{
    public class Zanr
    {
        [Key]
        public Guid ZanrID { get; set; }
        [Required]
        public string Naziv { get; set; }
    }
}
