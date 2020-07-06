using System.Web.Mvc;
using Models.Repository;

namespace Enrollment.Controllers
{

    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Home

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LatestEnrolledStudents()
        {
            
            return PartialView(unitOfWork.EnrollmentsRepo.Get());
        }

        public ActionResult Announcements()
        {
            return PartialView(unitOfWork.AnnouncementsRepo.Get());
        }

        public ActionResult CoursesGridViewPartial()
        {
            return PartialView(unitOfWork.CoursesRepo.Get());
        }
        public ActionResult AnnouncementGridViewPartial()
        {
            return PartialView(unitOfWork.AnnouncementsRepo.Get());
        }
    }
}