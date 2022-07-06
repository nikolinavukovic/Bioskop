using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bioskop.Models.Stripe
{
    public class StripeSettings
    {
        public string PublicKey { get; set; }
        public string WHSecret { get; set; }
    }
}