using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using Models.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Models.Startups;

namespace Enrollment.Controllers
{
    public class MessagesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        ApplicationUserManager userManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private string UserId => User.Identity.GetUserId();

        // GET: Messages
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult MessageGridViewPartial(string mode = "Inbox")
        {
            ViewBag.Mode = mode;
            var _userId = User.Identity.IsAuthenticated ? UserId : Request.Cookies["Email"]?.Value?.ToString();
            var model = unitOfWork.MessagesRepo.Fetch().Where(m => m.MessageTo == _userId);

            if (mode == "Sent")
                model = unitOfWork.MessagesRepo.Fetch().Where(m => m.MessageFrom == _userId);
            return PartialView("_MessageGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MessageGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]
            Models.Messages item)
        {
            var model = unitOfWork.MessagesRepo.Fetch().Where(m => m.MessageTo == UserId);
            model = model.Where(m => m.MessageTo == UserId);
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model

                    if (Session["UploadedFile"] is UploadedFile uploaded)
                    {
                        item.FileName = uploaded.FileName;
                        item.Files = uploaded.FileBytes;
                        item.MimeType = uploaded.ContentType;
                    }

                    item.MessageFrom = User.Identity.IsAuthenticated ? UserId : item.MessageFrom;
                    item.DateCreated = DateTime.Now;
                    item.CreatedBy = User.Identity.IsAuthenticated ? UserId : item.MessageFrom;
                    if (!User.Identity.IsAuthenticated)
                    {
                        foreach (var i in unitOfWork.UsersRepo.Fetch(m =>
                            m.UserRoles.Any(x => x.Name == "Administrator" || x.Name == "Admin")))
                        {
                            item.MessageTo = i.Id;
                            unitOfWork.MessagesRepo.Insert(item);
                        }
                    }
                    else
                    {
                        item.MessageTo = item.MessageFrom;
                        unitOfWork.MessagesRepo.Insert(item);
                    }
                    unitOfWork.Save();
                    Response.Cookies.Add(new HttpCookie("Email", item.MessageFrom){ });
                    if (item.IsMail == true)
                    {
                        userManager.SendEmail(item.MessageTo, item.Subject, item.Body);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("_MessageGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MessageGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))]
            Models.Messages item)
        {
            var model = unitOfWork.MessagesRepo.Fetch().Where(m => m.MessageTo == UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    if (item.IsMail == true)
                    {
                        userManager.SendEmail(item.MessageFrom, item.Subject, item.Body);
                    }


          
                    item.DateCreated = DateTime.Now;
                    item.CreatedBy = User.Identity.IsAuthenticated ? UserId : item.MessageFrom;
                    item.MessageTo = item.MessageFrom;
                    item.MessageFrom = User.Identity.IsAuthenticated ? UserId : item.MessageFrom;
                    unitOfWork.MessagesRepo.Insert(item);

                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("_MessageGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MessageGridViewPartialDelete(System.Int32 Id)
        {
            var model = unitOfWork.MessagesRepo.Fetch().Where(m => m.MessageTo == UserId);

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

            return PartialView("_MessageGridViewPartial", model.ToList());
        }

        public ActionResult AddEditMessagePartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? messageId)
        {
            var model = unitOfWork.MessagesRepo.Find(m => m.Id == messageId);
            if (model != null)
            {
                model.Message =
                    $"<br/><br/><br/><br/><br/>-----------------------------------<br/><br/><br/>From:{model._MessageFrom}<br/><br/>Sent: {model.DateCreated}<br/>To: {model._MessageFrom}<br/> Subject: RE: {model.Subject}" +
                    "<br/>Body:<br/>" + model.Message + "<br/><br/><br/><br/>";


            }

            return PartialView(model);
        }

        public ActionResult MessageNotificationPartial()
        {
            var userId = User.Identity.GetUserId();
            var messages = unitOfWork.MessagesRepo.Fetch(m => m.MessageTo == userId).Where(m => m.Status != true);

            return PartialView(messages);
        }

        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControl",
                MessagesControllerUploadControlSettings.UploadValidationSettings,
                MessagesControllerUploadControlSettings.FileUploadComplete);
            return null;
        }

        public ActionResult PopupMessagePartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? messageId)
        {
            var model = unitOfWork.MessagesRepo.Find(m => m.Id == messageId);
            if (model != null)
            {
                model.Status = true;
                unitOfWork.Save();

            }

            return PartialView(model);
        }

        public ActionResult RichEditPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            Messages messages)
        {

            return PartialView("_RichEditPartial", messages);
        }

        public ActionResult HtmlEditorPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            Messages messages)
        {
            return PartialView("_HtmlEditorPartial", messages);
        }

        public ActionResult HtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor",
                MessagesControllerHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }

        public ActionResult HtmlEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor",
                MessagesControllerHtmlEditorSettings.ImageUploadValidationSettings,
                MessagesControllerHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }

        public JsonResult GetMessageCount()
        {
            var userId =User.Identity.IsAuthenticated?  User.Identity.GetUserId() : Request.Cookies["Email"]?.Value?.ToString();
            var message = unitOfWork.MessagesRepo.Fetch();
            return Json(
                new
                {
                    Inbox = message.Where(m => m.MessageTo == userId).Count(m => m.Status != true),
                    Sent = message.Count(m => m.MessageFrom == userId)
                }, JsonRequestBehavior.AllowGet
            );

        }


    }
    public class MessagesControllerHtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Messages", Action = "HtmlEditorPartial_1ImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}
public class MessagesControllerHtmlEditorSettings
{
    public const string ImageUploadDirectory = "~/Content/UploadImages/";
    public const string ImageSelectorThumbnailDirectory = "~/Content/UploadImages/";

    public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
        MaxFileSize = 4000000
    };

    static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
    public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
    {
        get
        {
            if (imageSelectorSettings == null)
            {
                imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                imageSelectorSettings.Enabled = true;
                imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Messages", Action = "HtmlEditorPartialImageSelectorUpload" };
                imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                imageSelectorSettings.UploadSettings.Enabled = true;
            }
            return imageSelectorSettings;
        }
    }
}

public class MessagesControllerUploadControlSettings
{
    public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { "*.*" },
        MaxFileSize = 4000000
    };
    public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        if (e.UploadedFile.IsValid)
        {
            // Save uploaded file to some location
            HttpContext.Current.Session["UploadedFile"] = e.UploadedFile;
        }
    }
}

