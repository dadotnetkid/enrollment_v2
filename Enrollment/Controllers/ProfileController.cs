using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.Repository;
using Models.Startups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Enrollment.Controllers
{
    public class ProfileController : Controller
    {
        string UserId => User.Identity.GetUserId();
        UnitOfWork unitOfWork = new UnitOfWork();
        public ApplicationUserManager UserManager =>
           HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GradesGridViewPartial()
        {
            var model = unitOfWork.GradesRepo.Get(m => m.EnrolledSubject.Enrollments.StudentId == UserId);
            return PartialView("_GradesGridViewPartial", model);
        }
        [ValidateInput(false)]
        public ActionResult BillingsGridViewPartial()
        {
            var model = unitOfWork.BillingsRepo.Get(m => m.Enrollments.StudentId == UserId, includeProperties: "Enrollments");
            return PartialView("_BillingsGridViewPartial", model);
        }
        public ActionResult StudentProfileTabPartials(int? courseId)
        {
            ViewBag.CourseId = courseId;
            ViewBag.StudentId = UserId;

            return PartialView();
        }
        public ActionResult AddEditStudentPartial()
        {
            var model = unitOfWork.UserDetailsRepo.Find(m => m.Id == UserId);
            return PartialView(model);
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> StudentsGridViewPartialUpdate([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Models.UserDetails item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model



                    var user = unitOfWork.UsersRepo.Find(m => m.Id == item.Id, includeProperties: "UserDetails,UserRoles");
                    user.Email = item.Users.Email;
                    user.UserName = item.Users.Email?.Split('@')[0];
                    user.UserDetails.FirstName = item.FirstName;
                    user.UserDetails.MiddleName = item.MiddleName;
                    user.UserDetails.LastName = item.LastName;
                    if (item.Users.Password != null)
                        await UserManager.ChangePasswordAsync(user, item.Users.Password);
                    user.UserRoles.Clear();
                    user.UserRoles.Add(unitOfWork.UserRolesRepo.Find(m => m.Name == "Student") ?? new UserRoles()
                    {
                        Name = "Student",
                        Id = Guid.NewGuid().ToString(),
                        Description = "Student"
                    });
                    unitOfWork.Save();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UserDetailsRepo.Fetch(m => m.Users.UserRoles.Any(x => x.Name == "Student"));
        
            return PartialView("_StudentsGridViewPartial", model.ToList());
        }
    }
}