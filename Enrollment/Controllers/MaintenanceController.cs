using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Helpers;
using Models;
using Models.ControllerHelpers;
using Models.Repository;

namespace Enrollment.Controllers
{

    public class MaintenanceController : IdentityController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Maintenance
        public ActionResult Logs()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Users

        [OnUserAuthorization(ActionName = "Users")]
        public ActionResult Users()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult UsersGridViewPartial()
        {
            var model = unitOfWork.UsersRepo.Get();
            return PartialView("_UsersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Add Users")]
        public async Task<ActionResult> UsersGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Users item, [ModelBinder(typeof(DevExpressEditorsBinder))] List<string> roles)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    item.Id = Guid.NewGuid().ToString();
                    item.UserName = item.Email.Split('@')[0];
                    
                    var userResult = await UserManager.CreateAsync(item, item.Password);
                    if (userResult.Succeeded)
                    {
                        foreach (var i in roles)
                            await UserManager.AddToRoleAsync(item.Id, i);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["Model"] = item; ViewData["EditError"] = "Please, correct all errors.";
            }

            var model = unitOfWork.UsersRepo.Get();

            return PartialView("_UsersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Edit Users")]
        public async Task<ActionResult> UsersGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Users item, [ModelBinder(typeof(DevExpressEditorsBinder))] List<string> roles)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = unitOfWork.UsersRepo.Find(m => m.Id == item.Id, includeProperties: "UserRoles");

                    user.Email = item.Email;
                    user.UserName = item.Email.Split('@')[0];
                    user.UserDetails = item.UserDetails;
                    if (item.Password != null)
                    {
                        var res = await UserManager.ChangePasswordAsync(user, item.Password);
                        if (res.Succeeded)
                            Debug.Write(res.Succeeded);
                    }


                    user.UserRoles.Clear();
                    foreach (var i in roles)
                        user.UserRoles.Add(unitOfWork.UserRolesRepo.Find(m => m.Name == i));
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UsersRepo.Get();

            return PartialView("_UsersGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Delete Users")]
        public async Task<ActionResult> UsersGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]string Id)
        {
            if (Id != null)
            {
                try
                {
                    await UserManager.DeleteAsync(await UserManager.FindByIdAsync(Id));
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.UsersRepo.Get();
            return PartialView("_UsersGridViewPartial", model);
        }

        public PartialViewResult AddEditUserPartial([ModelBinder(typeof(DevExpressEditorsBinder))]string userId)
        {
            var model = unitOfWork.UsersRepo.Find(m => m.Id == userId);
            return PartialView(model);
        }

        #endregion

        #region UsersInController





        public PartialViewResult CboControllersPartial([ModelBinder(typeof(DevExpressEditorsBinder))]string controllerName)
        {
            ViewBag.ControllerName = controllerName;
            var model = new ControllerActionHelper().ControllerNames().ToList();
            return PartialView(model);
        }
        public PartialViewResult CboActionsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]string actionName, [ModelBinder(typeof(DevExpressEditorsBinder))]string controllerName)
        {
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            var model = new ControllerActionHelper().ActionNames();
            return PartialView(model);
        }

        public PartialViewResult CboUsersPartial([ModelBinder(typeof(DevExpressEditorsBinder))]string userId)
        {
            ViewBag.UserId = userId;
            var model = unitOfWork.UsersRepo.Get().Select(x => x.FullName);
            return PartialView(model);
        }


        public PartialViewResult TokenBoxActionsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]IEnumerable<UserRolesInActions> userRolesInActions)
        {
            var model = new ControllerActionHelper().ActionNames();
            ViewBag.ActionNames = userRolesInActions;
            return PartialView(model);
        }

        public ActionResult AddEditControllersActionsPartial([ModelBinder(typeof(DevExpressEditorsBinder))]int? controllersActionsId)
        {
            var model = new object(); //unitOfWork.ControllersActionsRepo.Find(m => m.Id == controllersActionsId);
            return PartialView(model);
        }
        public ActionResult Controllers()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ControllersGridView()
        {
            var model = unitOfWork.ControllersActionsRepo.Get();
            return PartialView("_ControllersGridView", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ControllersGridViewAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.ControllersActions item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model

                    foreach (var i in item.UserId)
                    {
                        //   item.Users.Add(unitOfWork.UsersRepo.Find(m => m.Id == i));
                    }
                    unitOfWork.ControllersActionsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ControllersActionsRepo.Get();
            return PartialView("_ControllersGridView", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ControllersGridViewUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.ControllersActions item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //    var res = unitOfWork.ControllersActionsRepo.Find(m => m.Id == item.Id);
                    //foreach (var i in res.Users)
                    //    res.Users.Remove(i);
                    //foreach (var i in res.UserId)
                    //    res.Users.Add(unitOfWork.UsersRepo.Find(m => m.Id == i));
                    //res.Controller = item.Controller;
                    //res.Action = item.Action;
                    unitOfWork.Save();



                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ControllersActionsRepo.Get();
            return PartialView("_ControllersGridView", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ControllersGridViewDelete([ModelBinder(typeof(DevExpressEditorsBinder))]int? Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // unitOfWork.ControllersActionsRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.ControllersActionsRepo.Get();
            return PartialView("_ControllersGridView", model);
        }
        #endregion


        #region User Roles

        public ActionResult AddEditUserRolesPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            string userRoleId, [ModelBinder(typeof(DevExpressEditorsBinder))]IEnumerable<UserRolesInActions> userRolesInActions)
        {
            var model = unitOfWork.UserRolesRepo.Find(m => m.Id == userRoleId);

            return PartialView(model);
        }
        [Route("maintenance/user-roles")]
        [OnUserAuthorization(ActionName = "User Roles")]
        public ActionResult UserRoles()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult UserRolesGridViewPartial()
        {
            var model = unitOfWork.UserRolesRepo.Get(includeProperties: "UserRolesInActions");
            return PartialView("_UserRolesGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Add User Roles")]
        public ActionResult UserRolesGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.UserRoles item, [ModelBinder(typeof(DevExpressEditorsBinder))]List<string> actionName)
        {
            //var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    foreach (var i in actionName)
                    {
                        item.UserRolesInActions.Add(new UserRolesInActions() { RoleId = item.Id, Action = i });
                    }

                    unitOfWork.UserRolesRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UserRolesRepo.Get(includeProperties: "UserRolesInActions");
            return PartialView("_UserRolesGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Update User Roles")]
        public ActionResult UserRolesGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.UserRoles item, [ModelBinder(typeof(DevExpressEditorsBinder))]List<string> actionName)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.UserRolesInActionsRepo.DeleteRange(m => m.RoleId == item.Id);
                    unitOfWork.Save();

                    var userRole = unitOfWork.UserRolesRepo.Find(m => m.Id == item.Id, includeProperties: "UserRolesInActions");
                    userRole.Name = item.Name;
                    userRole.Description = item.Description;

                    foreach (var i in actionName)
                    {
                        userRole.UserRolesInActions.Add(new UserRolesInActions() { RoleId = item.Id, Action = i, UserRoles = userRole });
                    }

                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.UserRolesRepo.Get(includeProperties: "UserRolesInActions");
            return PartialView("_UserRolesGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        [OnUserAuthorization(ActionName = "Delete User Roles")]
        public ActionResult UserRolesGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]string Id)
        {

            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.UserRolesRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.UserRolesRepo.Get(includeProperties: "UserRolesInActions");
            return PartialView("_UserRolesGridViewPartial", model);
        }

        #endregion



        #region Logs

        [ValidateInput(false)]
        public ActionResult LogsGridViewPartial()
        {
            var model = new object[0];
            return PartialView("_LogsGridViewPartial", model);
        }
        #endregion
        public ActionResult Company()
        {
            return View();
        }

    }
}