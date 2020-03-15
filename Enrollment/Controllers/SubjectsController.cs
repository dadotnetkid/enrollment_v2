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
    public class SubjectsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult CopyAvailableSubjects()
        {
            return PartialView();
        }

        #region available subject


        public ActionResult AddEditAvailableSubjectsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? availableSubjectId)
        {
            var model = unitOfWork.AvailableSubjectsRepo.Find(m => m.Id == availableSubjectId);
            return PartialView(model);
        }


        public ActionResult AvailableSubjects()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult AvailableSubjectsGridView()
        {
            var model = unitOfWork.AvailableSubjectsRepo.Get(includeProperties: "AvailableCourses,Subjects,UserDetails");
            return PartialView("_AvailableSubjectsGridView", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Add Available Subject")]
        public ActionResult AvailableSubjectsGridViewAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.AvailableSubjects item)
        {
            if (unitOfWork.AvailableSubjectsRepo
                .Fetch(m => m.SubjectId == item.SubjectId && m.AvailableCourseId == item.AvailableCourseId).Any())
                ModelState.AddModelError("SubjectId", "Already Added Subject to this course and school year");
            // var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    var course = unitOfWork.AvailableCoursesRepo.Find(m => m.Id == item.AvailableCourseId);
                    item.SubjectCode = new IdHelpers().GenerateSubjectCode(course.Id);

                    var schedulesHelper = new SchedulesHelper();
                    schedulesHelper.GenerateSchedule(item);
                    unitOfWork.AvailableSubjectsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.AvailableSubjectsRepo.Get(includeProperties: "AvailableCourses,Subjects,UserDetails");
            return PartialView("_AvailableSubjectsGridView", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Update Available Subject")]
        public ActionResult AvailableSubjectsGridViewUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.AvailableSubjects item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.SubjectCode))
                    {
                        item.SubjectCode = new IdHelpers().GenerateSubjectCode(item.AvailableCourseId ?? 0);
                    }
                    var schedulesHelper = new SchedulesHelper();
                    schedulesHelper.UpdateSchedule(item);
                    // Insert here a code to update the item in your model
                    unitOfWork.AvailableSubjectsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.AvailableSubjectsRepo.Get(includeProperties: "AvailableCourses,Subjects,UserDetails");
            return PartialView("_AvailableSubjectsGridView", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Delete Available Subject")]
        public ActionResult AvailableSubjectsGridViewDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.Int32 Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.AvailableSubjectsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.AvailableSubjectsRepo.Get(includeProperties: "AvailableCourses,Subjects,UserDetails");
            return PartialView("_AvailableSubjectsGridView", model);
        }

        #endregion
        [OnUserAuthorization("Subjects")]
        public ActionResult Subjects()
        {
            return PartialView();
        }
        #region subject

        public ActionResult AddEditSubjectPartial([ModelBinder(typeof(DevExpressEditorsBinder))] int? subjectId)
        {

            return PartialView(unitOfWork.SubjectsRepo.Find(m => m.Id == subjectId));
        }

        [ValidateInput(false)]
        public ActionResult SubjectsGridViewPartial()
        {
            var model = unitOfWork.SubjectsRepo.Get();
            return PartialView("_SubjectsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Add Subjects")]
        public ActionResult SubjectsGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Subjects item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    unitOfWork.SubjectsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SubjectsRepo.Get();
            return PartialView("_SubjectsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Edit Subjects")]
        public ActionResult SubjectsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Subjects item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.SubjectsRepo.Update(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.SubjectsRepo.Get();
            return PartialView("_SubjectsGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization("Delete Subjects")]
        public ActionResult SubjectsGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.Int32 Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.SubjectsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.SubjectsRepo.Get();
            return PartialView("_SubjectsGridViewPartial", model);
        }


        #endregion
        public ActionResult MasterGridCoursesPartial()
        {
            var model = unitOfWork.SchoolYearsRepo.Get();
            return PartialView(model);
        }
        public ActionResult CboSubjectsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? availableCourseId)
        {
            var subjects = unitOfWork.AvailableSubjectsRepo.Get(m => m.AvailableCourseId == availableCourseId)
                .Select(x => x.SubjectId).ToList();
            ViewBag.Subjects = unitOfWork.SubjectsRepo.Get(m => !subjects.Contains(m.Id));
            return PartialView();
        }

        public ActionResult CopyAvailableSubjectsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]AvailableCourses item)
        {
            if (item == null)
                return PartialView(item);
            var availableCourses = unitOfWork.AvailableCoursesRepo.Fetch(m => m.SchoolYearId == item.SchoolYearId && m.AvailableSubjects.Any()).ToList();
            ViewBag.AvailableCourses = availableCourses;
            return PartialView(item);
        }


    }
}