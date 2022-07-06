using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bioskop.Models.Stripe
{
    public class CreateCheckoutSessionRequest
    {

        [Required]
        public int Cena { get; set; }


        [Required]
        public Guid ProjekcijaId { get; set; }
        [Required]
        public Guid KorisnikId { get; set; }
        //[Required]
        //public SedisteProjekcije[] SedistaProjekcije { get; set; }
        public string[] SedistaId { get; set; }

        public Guid KupovinaId { get; set; }


        [Required]
        public string SuccessUrl { get; set; }
        [Required]
        public string FailureUrl { get; set; }

    }
}
