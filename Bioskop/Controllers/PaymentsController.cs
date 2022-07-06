using Bioskop.Data;
using Bioskop.Models;
using Bioskop.Models.Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bioskop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
	{
        private readonly StripeSettings _stripeSettings;
        private readonly IKorisnikRepository korisnikRepository;
        private readonly IKupovinaRepository kupovinaRepository;
        private readonly ISedisteProjekcijeRepository sedisteProjekcijeRepository;

        private readonly DatabaseContext context;

        public PaymentsController(IOptions<StripeSettings> stripeSettings, IKorisnikRepository korisnikRepository, DatabaseContext context, IKupovinaRepository kupovinaRepository, ISedisteProjekcijeRepository sedisteProjekcijeRepository)
        {
			_stripeSettings = stripeSettings.Value;
            this.korisnikRepository = korisnikRepository;
            this.kupovinaRepository = kupovinaRepository;
            this.sedisteProjekcijeRepository = sedisteProjekcijeRepository;
            this.context = context;
        }

		[HttpPost("create-checkout-session")]
		public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest price)
		{
            if (!price.KorisnikId.Equals(Guid.Empty) && !price.ProjekcijaId.Equals(Guid.Empty))
           {

           
                Transakcija t = new Transakcija();

                t.KorisnikId = price.KorisnikId;
                t.ProjekcijaId = price.ProjekcijaId;
            t.SedistaId = "";
            //t.SedistaProjekcije = price.SedistaProjekcije.ToList<SedisteProjekcije>(); //sa null radi ali posle pravi problem jer ga ne zna
            foreach (var item in price.SedistaId)
            {
                t.SedistaId += item + ',';     
            }
            t.SedistaId = t.SedistaId[0..^1];

                context.Transakcija.Add(t);
                context.SaveChanges();
            }




            var optionsCreate = new PriceCreateOptions
			{
				UnitAmount = price.Cena * 100, 
				Currency = "rsd",
				Product = "prod_LxBNMwDY7eBOYa",
			};

            
			var serviceCreate = new PriceService();
			var newPriceId = serviceCreate.Create(optionsCreate).Id;

			var options = new SessionCreateOptions
			{
				SuccessUrl = price.SuccessUrl, //promenila 
				CancelUrl = price.FailureUrl,
				Mode = "payment",
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						Price = newPriceId,
						Quantity = 1,
					},
				},
			};

			var service = new SessionService();
			service.Create(options);
			try
			{
				var session = await service.CreateAsync(options);
				return Ok(new CreateCheckoutSessionResponse
				{
					SessionId = session.Id,
					PublicKey = _stripeSettings.PublicKey
				});
			}
			catch (StripeException e)
			{
				Console.WriteLine(e.StripeError.Message);
				return BadRequest(new ErrorResponse
				{
					ErrorMessage = new ErrorMessage
					{
						Message = e.StripeError.Message,
					}
				});
			}
		}

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();


            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSettings.WHSecret
                    );

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine(paymentIntent);
                    // Then define and call a method to handle the successful payment intent.
                    handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.CustomerCreated)
                {
                    var customer = stripeEvent.Data.Object as Customer;
                    //Do Stuff
                    await addCustomerIdToUser(customer);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest();
            }
        }

        private async void handlePaymentIntentSucceeded(PaymentIntent paymentIntent)
        {
            Transakcija t = new Transakcija();

            t = context.Transakcija.OrderByDescending(p => p.Id).FirstOrDefault();


            Kupovina kupovina = new Kupovina
            {
                KupovinaID = new Guid(),
                Placeno = true,
                UkupanIznos = paymentIntent.Amount / 100,
                KorisnikID = t.KorisnikId,
                //Sedis = t.SedistaProjekcije,
                VremePlacanja = DateTime.Now,
                VremeRezervacije = DateTime.Now
            };

            context.Kupovina.Add(kupovina);
            context.SaveChanges();

            context.Transakcija.OrderByDescending(p => p.Id).FirstOrDefault().KupovinaId = kupovina.KupovinaID;
            context.SaveChanges();

            string[] niz = t.SedistaId.Split(',');

            foreach (var s in niz)
            {
                
                context.SedisteProjekcije.Where(a => a.SedisteID.ToString().Equals(s) &&
                        a.ProjekcijaID.Equals(t.ProjekcijaId)).FirstOrDefault().KupovinaID = t.KupovinaId;
            }

            context.SaveChanges();

        }

        private async Task addCustomerIdToUser(Customer customer)
        {
            try
            {
                var userFromDb = korisnikRepository.GetKorisnikByEmail(customer.Email);

                if (userFromDb != null)
                {
                    userFromDb.CustomerId = customer.Id;
                    korisnikRepository.UpdateKorisnik(userFromDb);
                    Console.WriteLine("Customer Id added to user ");
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Unable to add customer id to user");
                Console.WriteLine(ex);
            }
        }



    }


}