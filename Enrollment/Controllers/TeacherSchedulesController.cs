using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Enrollment.Controllers
{
    public class TeacherSchedulesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string UserId => User.Identity.GetUserId();
        // GET: TeacherSchedules
        public ActionResult Index()
        {
            return View();
        }
        object appointmentContext = null;
        object resourceContext = null;

        public ActionResult SchedulerPartial()
        {
            System.Collections.IEnumerable appointments = null; // Get appointments from the context
            System.Collections.IEnumerable resources = null; // Get resources from the context
            ViewData["Appointments"] = unitOfWork.SchedulesRepo.Get(m =>
                  m.AvailableSubjects.TeacherId == UserId);
            ViewData["Resources"] = resources;

            return PartialView("_SchedulerPartial");
        }
        public ActionResult SchedulerPartialEditAppointment()
        {
            System.Collections.IEnumerable appointments = null; // Get appointments from the context
            System.Collections.IEnumerable resources = null; // Get resources from the context

            try
            {
                SchedulesControllerSchedulerSettings.UpdateEditableDataObject(appointmentContext, resourceContext);
            }
            catch (Exception e)
            {
                ViewData["SchedulerErrorText"] = e.Message;
            }

            ViewData["Appointments"] = appointments;
            ViewData["Resources"] = resources;

            return PartialView("_SchedulerPartial");
        }
    }
}