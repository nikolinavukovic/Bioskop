﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bioskop.Models.Stripe
{
    public class CreateCheckoutSessionResponse
    {
        public string SessionId { get; set; }

        public string PublicKey { get; set; }
    }
}