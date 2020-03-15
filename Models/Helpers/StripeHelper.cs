using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace Helpers
{
    public class StripeHelper
    {
        private string ApiKey => ConfigurationManager.AppSettings["ApiKey"];
        public void Init()
        {
            StripeConfiguration.ApiKey = ApiKey;



            var cRes = new CustomerService().Create(new CustomerCreateOptions()
            {
                Email = "markchristopher.cacal@gmail.com",
                Name = "Mark Christopher",
                

            });



            var paymentMethodCreateOptions = new PaymentMethodCreateOptions
            {

                Type = "card",
                Card = new PaymentMethodCardCreateOptions()
                {
                    Number = "4242424242424242",
                    ExpMonth = 1,
                    ExpYear = 2021,
                    Cvc = "314",

                },
                BillingDetails = new BillingDetailsOptions()
                {
                    Name = "Mark",
                    Address = new AddressOptions()
                    {
                        PostalCode = "3700",
                        City = "Bayombong"
                    },
                    Email = "markchristopher.cacal@gmail.com",
                    Phone = "09067701852"
                },

            };
            var paymentMethodService = new PaymentMethodService();

            var res = paymentMethodService.Create(paymentMethodCreateOptions);
 

            // `source` is obtained with Stripe.js; see https://stripe.com/docs/payments/accept-a-payment-charges#web-create-token
            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = 2000,
                Currency = "usd",
                Source = "tok_visa",
                Description = "Charge for jenny.rosen@example.com",

            };
            var chargeService = new ChargeService();
            var iRes = chargeService.Create(chargeCreateOptions);
        }
    }
}
