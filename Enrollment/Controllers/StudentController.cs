using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Models.Repository;

namespace Enrollment.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        // GET: Student
        public string StudentId => User.Identity.GetUserId();
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Enrollments()
        {
            return View();
        }

        public ActionResult Billings()
        {
            return View();
        }
        public ActionResult Grades()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GradesGridViewPartial()
        {
            var model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubject.Enrollments.StudentId == StudentId);
            return PartialView("_GradesGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult BillingsGridViewPartial()
        {
            var model = unitOfWork.BillingsRepo.Get(m => m.Enrollments.StudentId == StudentId, includeProperties: "Enrollments");
            return PartialView("_BillingsGridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult BillingsDetailGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? enrollmentId)
        {
            var model = unitOfWork.BillingsRepo.Get(m => m.EnrollmentId == enrollmentId, includeProperties: "Enrollments");
            ViewBag.EnrollmentId = enrollmentId;
            return PartialView("_BillingsDetailGridViewPartial", model);
        }

        [ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartial()
        {
            var model = unitOfWork.EnrollmentsRepo.Get(x => x.StudentId == StudentId);
            return PartialView("_EnrollmentGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Enrollments item)
        {
            var model = new object[0];
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
            return PartialView("_EnrollmentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Enrollments item)
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
            return PartialView("_EnrollmentGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentGridViewPartialDelete(System.Int32 Id)
        {
            var model = new object[0];
            if (Id >= 0)
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
            return PartialView("_EnrollmentGridViewPartial", model);
        }
    }
}