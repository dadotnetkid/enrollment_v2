using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Microsoft.AspNet.Identity;
using Models.Repository;

namespace Enrollment.Controllers
{
    public class CreditsController : Controller
    {
        private PaypalHelper paypalHelper = new PaypalHelper();

        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Credits

        #region buy-credit
        [Route("buy-credit")]
        public ActionResult BuyCredit()
        {
            return View();
        }


        #endregion

        public ActionResult AddEditBuyCreditPartial([ModelBinder(typeof(DevExpressEditorsBinder))] int? creditId)
        {
            var res = unitOfWork.CreditsRepo.Find(m => m.Id == creditId);
            var state = Request.Params["State"];
            return PartialView(res);
        }


        [ValidateInput(false)]
        public ActionResult BuyCreditsGridViewPartial()
        {
            var model = unitOfWork.CreditsRepo.Get();

            return PartialView("_BuyCreditsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BuyCreditsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Credits item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    item.State = Request.Params["State"];
                    item.UserId = User.Identity.GetUserId();
                    item.DateCreated = DateTime.Now;
                    item.Credit = item.State == "Transfer" ? (-1 * item.Credit) : item.Credit;
                    unitOfWork.CreditsRepo.Insert(item);
                    unitOfWork.Save();
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return Redirect("/");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PayBuyCreditsPartial([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Credits item)
        {
            if (string.IsNullOrEmpty(Request.Params["PayerID"]))
            {
                var credits = unitOfWork.CreditsRepo.Find(m => m.Id == item.Id);
                var guid = Guid.NewGuid().ToString().ToLower().Replace("-", "");
                var createdPayment = paypalHelper.CreatePayment(guid, paypalHelper.BuyCredit(credits?.Credit));
                credits.PayId = createdPayment.id;
                unitOfWork.Save();
                Session.Add(guid, createdPayment.id);
                return Redirect(
                    createdPayment.links.FirstOrDefault(m => m.rel.ToLower().Contains("approval_url"))?.href);
            }
            else
            {
                string paymentId = Session[Request.Params["guid"]].ToString();

                var payment = paypalHelper.ExecutePayment(Request.Params["PayerID"], paymentId);
                var credits = unitOfWork.CreditsRepo.Find(m => m.PayId == paymentId);
                credits.State = "Paid";
                credits.AmountPaid = (0.05M + (credits.Credit * 0.05M));
                credits.PayBy = User.Identity.GetUserId();
                credits.DatePaid = DateTime.Now;
                unitOfWork.Save();
            }

            return RedirectToAction("buycredit");
        }
        [Route("pay-credit")]
        public ActionResult ExecutePaymentPartial()
        {

            string paymentId = Session[Request.Params["guid"]].ToString();

            var payment = paypalHelper.ExecutePayment(Request.Params["PayerID"], paymentId);
            var credits = unitOfWork.CreditsRepo.Find(m => m.PayId == paymentId);
            credits.State = "Paid";
            credits.AmountPaid = (0.05M + (credits.Credit * 0.05M));
            credits.PayBy = User.Identity.GetUserId();
            credits.DatePaid = DateTime.Now;
            unitOfWork.Save();

            return RedirectToAction("buycredit");
        }

        public ActionResult PopupAddEditBuyCreditsPartial()
        {
            var state = Request.Params["State"];

            return PartialView();
        }

        #region comment
        /*[HttpPost, ValidateInput(false)]
    //public ActionResult BuyCreditsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Credits item)
    //{
    //    var model = new object[0];
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            // Insert here a code to update the item in your model
    //        }
    //        catch (Exception e)
    //        {
    //            ViewData["EditError"] = e.Message;
    //        }
    //    }
    //    else
    //        ViewData["EditError"] = "Please, correct all errors.";
    //    return PartialView("_BuyCreditsGridViewPartial", model);
    //}
    //[HttpPost, ValidateInput(false)]
    //public ActionResult BuyCreditsGridViewPartialDelete(System.Int32 Id)
    //{
    //    var model = new object[0];
    //    if (Id >= 0)
    //    {
    //        try
    //        {
    //            // Insert here a code to delete the item from your model
    //        }
    //        catch (Exception e)
    //        {
    //            ViewData["EditError"] = e.Message;
    //        }
    //    }
    //    return PartialView("_BuyCreditsGridViewPartial", model);
    //}*/


        #endregion

    }
}