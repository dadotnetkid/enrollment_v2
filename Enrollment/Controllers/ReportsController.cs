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
    public class ReportsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Reports
        [OnUserAuthorization("master-list")]
        public ActionResult MasterList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MasterListPartial([ModelBinder(typeof(DevExpressEditorsBinder))]MasterListViewModel item)
        {

            return PartialView(item);
        }
        public ActionResult MasterListPartial()
        {

            return PartialView();
        }


        public ActionResult StudentBillingStatementPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? enrollmentId)
        {
            Rpt_StudentBillingReport rptStudentBillingReport = new Rpt_StudentBillingReport
            {
                DataSource = unitOfWork.EnrollmentsRepo.Get(m => m.Id == enrollmentId,includeProperties: "Billings,Billings.Enrollments,AvailableCourses")
            };
            return PartialView(rptStudentBillingReport);
        }

    }

    public class MasterListViewModel
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private AvailableCourses _AvailableCourses;
        public int? AvailableCourseId { get; set; }
        public AvailableCourses AvailableCourses
        {
            //
            get => _AvailableCourses;
            set
            {
                _AvailableCourses = value ?? unitOfWork.AvailableCoursesRepo.Find(m => m.Id == AvailableCourseId); ;

            }
        }
    }

}
