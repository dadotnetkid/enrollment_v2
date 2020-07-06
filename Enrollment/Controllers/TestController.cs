using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Repository;
namespace Enrollment.Controllers
{
    public class TestController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult StudentGridViewPartial()
        {
            var model = unitOfWork.UsersRepo.Get();
            return PartialView("_StudentGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Users item)
        {
            var model = new object[0];
            var keys = ListBoxExtension.GetSelectedValues<string>("UserRoles");
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Users item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_StudentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult StudentGridViewPartialDelete(System.String Id)
        {
            var model = new object[0];
            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_StudentGridViewPartial", model);
        }

        public ActionResult AddEditUsers([ModelBinder(typeof(DevExpressEditorsBinder))]string userId)
        {
            var user = unitOfWork.UsersRepo.Find(x => x.Id == userId);
            return PartialView(user);
        }

        public ActionResult ListBoxPartial([ModelBinder(typeof(DevExpressEditorsBinder))]string userId)
        {
            var userRole = unitOfWork.UserRolesRepo.Get(x => x.Users.Any(m => m.Id == userId));
            return PartialView("_ListBoxPartial", userRole);
        }
    }
}