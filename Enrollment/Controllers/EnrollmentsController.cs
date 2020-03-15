using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Helpers;
using Models;
using Models.Repository;

namespace Enrollment.Controllers
{
    public class EnrollmentsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Enrollments
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CboStudentPartial([ModelBinder(typeof(DevExpressEditorsBinder))] string studentId)
        {
            var model = new Enrollments() { StudentId = studentId };
            return PartialView(model);
        }
        public ActionResult TokenAvailableSubjectPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? courseId, [ModelBinder(typeof(DevExpressEditorsBinder))]int? enrollmentId)
        {
            var res = new UnitOfWork().AvailableSubjectsRepo.Fetch();
            res = res.Where(m => m.AvailableCourseId == courseId);//.Where(x => !subjectIds.Contains(x.Id));

            ViewBag.EnrollmentId = enrollmentId;
            ViewBag.CourseId = courseId;
            return PartialView(res.ToList());
        }
        public ActionResult CboAvailableCourse([ModelBinder(typeof(DevExpressEditorsBinder))] int? schoolYearId, [ModelBinder(typeof(DevExpressEditorsBinder))] int? enrollmentId)
        {
            ViewBag.Courses = unitOfWork.AvailableCoursesRepo.Get(m => m.SchoolYearId == schoolYearId);
            ViewBag.SchoolYearId = schoolYearId;

            return PartialView(unitOfWork.EnrollmentsRepo.Find(m => m.Id == enrollmentId));
        }
        public ActionResult AddEditEnrollmentPartial([ModelBinder(typeof(DevExpressEditorsBinder))] int? enrollmentId, [ModelBinder(typeof(DevExpressEditorsBinder))]int? courseId)
        {
            var model = unitOfWork.EnrollmentsRepo.Find(m => m.Id == enrollmentId);
            ViewBag.SchoolYearId = model?.AvailableCourses?.SchoolYears.SchoolYearAndSemester;
            ViewBag.CourseId = courseId;
            return PartialView(model);
        }

        [ValidateInput(false)]
        public ActionResult EnrollmentsGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] string studentId, [ModelBinder(typeof(DevExpressEditorsBinder))]int? courseId)
        {

            var model = unitOfWork.EnrollmentsRepo.Fetch(includeProperties: "AvailableCourses,UserDetails");
          
            ViewBag.StudentId = studentId;
            ViewBag.CourseId = courseId;
            return PartialView("_EnrollmentsGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Enrollments item)
        {
            // var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var i in item.AvailableSubjectIds)
                    {
                        var availableSubjectId = i.ToInt();
                        var enrolled = new EnrolledSubject() { AvailableSubjectId = availableSubjectId };
                        enrolled.Grades.Add(new Grades());
                        item.EnrolledSubject.Add(enrolled);
                    }
                    // Insert here a code to insert the new item in your model
                    unitOfWork.EnrollmentsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = unitOfWork.EnrollmentsRepo.Fetch(includeProperties: "AvailableCourses,UserDetails");
            //if (item.StudentId != null)
            //    model = model.Where(m => m.StudentId == item.StudentId);
            ViewBag.StudentId = item.StudentId;
            return PartialView("_EnrollmentsGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Enrollments item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model

                    unitOfWork.EnrollmentsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.EnrollmentsRepo.Fetch(includeProperties: "AvailableCourses,UserDetails");
            //if (item.StudentId != null)
            //    model = model.Where(m => m.StudentId == item.StudentId);
            ViewBag.StudentId = item.StudentId;
            return PartialView("_EnrollmentsGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EnrollmentsGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]System.Int32 Id, [ModelBinder(typeof(DevExpressEditorsBinder))] string studentId)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.EnrollmentsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.EnrollmentsRepo.Fetch(includeProperties: "AvailableCourses,UserDetails");
            //if (studentId != null)
            //    model = model.Where(m => m.StudentId == studentId);
            ViewBag.StudentId = studentId;
            return PartialView("_EnrollmentsGridViewPartial", model.ToList());
        }


    }
}