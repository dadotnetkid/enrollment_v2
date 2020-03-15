using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Models;
using Models.Repository;

namespace Enrollment.Controllers
{
    public class SchedulesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Schedules
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CboEnrollment([ModelBinder(typeof(DevExpressEditorsBinder))]string studentId)
        {
            var student = unitOfWork.UserDetailsRepo.Find(m => m.Id == studentId);
            return PartialView(student);
        }



        object appointmentContext = null;
        object resourceContext = null;

        public ActionResult SchedulerPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? enrollmentId)
        {
            System.Collections.IEnumerable appointments = null; // Get appointments from the context
            System.Collections.IEnumerable resources = null; // Get resources from the context
            ViewBag.EnrollmentId = enrollmentId;
            ViewData["Appointments"] = unitOfWork.SchedulesRepo.Get(m =>
                  m.AvailableSubjects.EnrolledSubject.Any(x => x.EnrollmentId == enrollmentId));
            ViewData["Resources"] = resources;
            if (User.IsInRole("Student"))
            {
                var list = new List<Schedules>();
                var studentId = User.Identity.GetUserId();
                foreach (var i in unitOfWork.EnrollmentsRepo.Fetch(m => m.StudentId == studentId))
                {
                    list.AddRange(unitOfWork.SchedulesRepo.Get(m =>
                       m.AvailableSubjects.EnrolledSubject.Any(x => x.EnrollmentId == i.Id)));
                }

                ViewData["Appointments"] = list;
            }
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
    public class SchedulesControllerSchedulerSettings
    {
        static DevExpress.Web.Mvc.MVCxAppointmentStorage appointmentStorage;
        public static DevExpress.Web.Mvc.MVCxAppointmentStorage AppointmentStorage
        {
            get
            {
                if (appointmentStorage == null)
                {
                    appointmentStorage = new DevExpress.Web.Mvc.MVCxAppointmentStorage();
                    appointmentStorage.Mappings.AppointmentId = "Id";
                    appointmentStorage.Mappings.Start = "TimeIn";
                    appointmentStorage.Mappings.End = "TimeOut";
                    appointmentStorage.Mappings.Subject = "Subject";
                    appointmentStorage.Mappings.Description = "Description";
                    appointmentStorage.Mappings.Location = "";
                    appointmentStorage.Mappings.AllDay = "";
                    appointmentStorage.Mappings.Type = "";
                    appointmentStorage.Mappings.RecurrenceInfo = "";
                    appointmentStorage.Mappings.ReminderInfo = "";
                    appointmentStorage.Mappings.Label = "Label";
                    appointmentStorage.Mappings.Status = "Status";
                    appointmentStorage.Mappings.ResourceId = "";
                }
                return appointmentStorage;
            }
        }

        static DevExpress.Web.Mvc.MVCxResourceStorage resourceStorage;
        public static DevExpress.Web.Mvc.MVCxResourceStorage ResourceStorage
        {
            get
            {
                if (resourceStorage == null)
                {
                    resourceStorage = new DevExpress.Web.Mvc.MVCxResourceStorage();
                    resourceStorage.Mappings.ResourceId = "";
                    resourceStorage.Mappings.Caption = "";
                }
                return resourceStorage;
            }
        }

        public static void UpdateEditableDataObject(object appointmentContext, object resourceContext)
        {
            InsertAppointments(appointmentContext, resourceContext);
            UpdateAppointments(appointmentContext, resourceContext);
            DeleteAppointments(appointmentContext, resourceContext);
        }

        static void InsertAppointments(object appointmentContext, object resourceContext)
        {
            System.Collections.IEnumerable appointments = null;
            System.Collections.IEnumerable resources = null;

            var newAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToInsert<Models.Schedules>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in newAppointments)
            {
                // Add appointment to your data context
                throw new NotImplementedException();
            }
        }
        static void UpdateAppointments(object appointmentContext, object resourceContext)
        {
            System.Collections.IEnumerable appointments = null;
            System.Collections.IEnumerable resources = null;

            var updAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToUpdate<Models.Schedules>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in updAppointments)
            {
                // Update the appointment in your data context
                throw new NotImplementedException();
            }
        }

        static void DeleteAppointments(object appointmentContext, object resourceContext)
        {
            System.Collections.IEnumerable appointments = null;
            System.Collections.IEnumerable resources = null;

            var delAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToRemove<Models.Schedules>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in delAppointments)
            {
                // Remove the appointment from your data context
                throw new NotImplementedException();
            }
        }
    }

}