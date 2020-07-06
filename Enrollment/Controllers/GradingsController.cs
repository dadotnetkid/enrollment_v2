using DevExpress.Web.Mvc;
using Models;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Enrollment.Controllers
{
    public class GradingsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Gradings
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GradesGridViewPartialBatchUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]string subjectCode,
            [ModelBinder(typeof(DevExpressEditorsBinder))]    MVCxGridViewBatchUpdateValues<Grades, int> updateValues)
        {
            foreach (var i in updateValues.Update)
            {
                var grades = unitOfWork.GradesRepo.Find(m => m.Id == i.Id);
                grades.Prelim = i.Prelim;
                grades.MidTerm = i.MidTerm;
                grades.FinalTerm = i.FinalTerm;
                unitOfWork.Save();
            }
            var model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubject.AvailableSubjects.SubjectCode == subjectCode).ToList();
            ViewBag.SubjectCode = subjectCode;
            if (subjectCode != null)
                model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubject.AvailableSubjects.SubjectCode == subjectCode || (m.EnrolledSubject.Enrollments.UserDetails.FirstName.Contains(subjectCode) || m.EnrolledSubject.Enrollments.UserDetails.LastName.Contains(subjectCode))).ToList();
            return PartialView("_GradingsGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult GradingsGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] string subjectCode)
        {
            ViewBag.SubjectCode = subjectCode;
            var model = new List<Grades>();

            if (subjectCode != null)
            {
                var name = subjectCode.Split(' ');

                if (unitOfWork.GradesRepo.Fetch(m => m.EnrolledSubject.AvailableSubjects.SubjectCode == subjectCode)
                    .Any())
                {
                    model = unitOfWork.GradesRepo.Get(m =>
                        m.EnrolledSubject.AvailableSubjects.SubjectCode == subjectCode).ToList();
                }
                else if (unitOfWork.GradesRepo.Fetch(m =>
                    (m.EnrolledSubject.Enrollments.UserDetails.LastName + " " +
                     m.EnrolledSubject.Enrollments.UserDetails.FirstName).Contains(subjectCode)).Any())
                {
                    model = unitOfWork.GradesRepo.Get(m =>
                        (m.EnrolledSubject.Enrollments.UserDetails.LastName + " " +
                         m.EnrolledSubject.Enrollments.UserDetails.FirstName).Contains(subjectCode)).ToList();
                }
                else if (unitOfWork.GradesRepo.Fetch(m => name.Contains(m.EnrolledSubject.Enrollments.UserDetails.LastName)).Any())
                {
                    model = unitOfWork.GradesRepo.Get(m =>
                        name.Contains(m.EnrolledSubject.Enrollments.UserDetails.LastName)).ToList();
                }
                else if (unitOfWork.GradesRepo.Fetch(m => name.Contains(m.EnrolledSubject.Enrollments.UserDetails.FirstName)).Any())
                {
                    model = unitOfWork.GradesRepo.Get(m =>
                        name.Contains(m.EnrolledSubject.Enrollments.UserDetails.LastName)).ToList();
                }

                else if (unitOfWork.GradesRepo.Fetch(m => m.EnrolledSubject.Enrollments.UserDetails.SchoolId == subjectCode).Any())
                {
                    model = unitOfWork.GradesRepo.Get(m =>
                        name.Contains(m.EnrolledSubject.Enrollments.UserDetails.LastName)).ToList();
                }




            }

            if (User.IsInRole("Teacher"))
            {
                var teacherId = User.Identity.GetUserId();
                foreach (var i in unitOfWork.EnrolledSubjectRepo.Fetch(m => m.AvailableSubjects.TeacherId == teacherId))
                {
                    model.AddRange(unitOfWork.GradesRepo.Get(m => m.EnrolledSubject.Id == i.Id));
                }
            }
            return PartialView("_GradingsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GradingsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Grades item)
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
            return PartialView("_GradingsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GradingsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Grades item)
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
            return PartialView("_GradingsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GradingsGridViewPartialDelete(System.Int32 Id)
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
            return PartialView("_GradingsGridViewPartial", model);
        }
    }
}