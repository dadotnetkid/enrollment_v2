using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Enrollment.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        [Route("transaction-error")]
        public ActionResult TransactionsError([ModelBinder(typeof(DevExpressEditorsBinder))]
            string error)
        {
            ViewBag.Error = error;
            return PartialView();
        }
        [Route("access-denied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}