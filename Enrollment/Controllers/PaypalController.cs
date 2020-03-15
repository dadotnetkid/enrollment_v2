using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using PayPal.Api;

namespace Enrollment.Controllers
{
    public class PaypalController : Controller
    {
        private PaypalHelper paypalHelper = new PaypalHelper();
        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }
        [Route("paypal-payment")]
        public ActionResult PaymentWithPaypal()
        {
            APIContext apiContext = paypalHelper.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {


                    var createdPayment = paypalHelper.CreatePayment("",paypalHelper.BuyCredit(3));
                    var links = createdPayment.links.FirstOrDefault(m => m.rel.ToLower().Contains("approval_url"));
                    //while (links.MoveNext())
                    //{
                    //    Links lnk = links.Current;
                    //    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    //    {
                    //        paypalRedirectUrl = lnk.href;
                    //    }
                    //}

                    return Redirect(links.href);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = paypalHelper.ExecutePayment(payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            //on successful payment, show success page to user.  
            return RedirectToAction("buycredit", "credits");
        }
    }
}