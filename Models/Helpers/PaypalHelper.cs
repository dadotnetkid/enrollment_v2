using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DevExpress.DataProcessing;
using PayPal.Api;

namespace Helpers
{
    public class PaypalHelper
    {
        public string RedirectUrl { get; set; }
        private string ClientId;
        private string ClientSecret;
        //Constructor  
        public PaypalHelper()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
            RedirectUrl = HttpContext.Current.Request.Url.Scheme + "://" +
                HttpContext.Current.Request.Url.Authority + "/pay-credit?guid=";
        }
        public PaypalHelper(string redirectUrl)
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
            this.RedirectUrl = redirectUrl;
        }
        // getti
        // getting properties from the web.config  
        Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
        private PayPal.Api.Payment payment;
        public Payment ExecutePayment(string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(this.GetAPIContext(), paymentExecution);
        }


        public Payment CreatePayment(string guid, List<Transaction> transactionList)
        {

            this.payment = new Payment()
            {
                intent = "sale",
                payer = new Payer()
                {
                    payment_method = "paypal",
                    payer_info = new PayerInfo()
                    {

                    }
                },
                transactions = transactionList,
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = RedirectUrl + guid + "&Cancel=true",
                    return_url = RedirectUrl + guid
                }
            };
            // Create a payment using a APIContext  
            return this.payment.Create(this.GetAPIContext());
        }












        public List<Transaction> BuyCredit(int? quantity)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Buy Credit",
                currency = "USD",
                price = "",
                quantity = quantity?.ToString(),
                sku = "sku"
            });

            var amount = new Amount()
            {
                currency = "USD",
                total = (.05 + (quantity * .05))?.ToString(),
                details = new Details()
                {
                    tax = ".05",
                    shipping = "0",
                    subtotal = (quantity * .05)?.ToString(),
                }
            };
            return new List<Transaction>()
            {
                new Transaction()
                {
                    description = "Buy Credit",
                    invoice_number = "inv-"+Convert.ToString((new Random()).Next(1000000)), //Generate an Invoice No  
                    amount = amount,
                    item_list = itemList
                }
            };
        }


    }
}
