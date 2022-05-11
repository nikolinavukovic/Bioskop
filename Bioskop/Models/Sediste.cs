using System;
using System.ComponentModel.DataAnnotations;

namespace Bioskop.Models
{
    public class Sediste
    {
        [Key]
        public Guid SedisteID { get; set; }

        [Required]
        public int BrojReda { get; set; }

        [Required]
        public int BrojSedista { get; set; }

    }
}
