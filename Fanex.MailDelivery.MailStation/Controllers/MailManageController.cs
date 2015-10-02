// --------------------------------------------------------------------------
// <copyright file="MailManageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Resources;
    using Fanex.MailDelivery.MailStation.Services.ManageMail;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailManageController : BaseController
    {
        private MailManageService _mailManageService;

        public ActionResult Index(GetMailListCriteria criteria)
        {
            _mailManageService = new MailManageService();
            ViewBag.ListMailGroup = _mailManageService.MailGroups();
            ViewBag.ListSite = _mailManageService.Sites();
            ViewBag.ListStatus = _mailManageService.Status();
            ViewBag.ListTriggerType = _mailManageService.Trigger();
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageMail/ManageMail/Index.cshtml", criteria);
        }

        public JsonResult Sites()
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.Sites());
        }

        public JsonResult MailGroups(bool hasAllElement = true)
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.MailGroups(hasAllElement));
        }

        public JsonResult AddMailGroup(CreateMailGroupCriteria criteria)
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.CreateMailGroup(criteria));
        }

        /// <summary>
        /// Creates the mail view.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Create Mail View.</returns>
        public ActionResult CreateMailView(ReadMailByIdCriteria criteria)
        {
            _mailManageService = new MailManageService();
            ViewBag.ListMailGroup = _mailManageService.MailGroups(false);
            ViewBag.ListSite = _mailManageService.Sites(false);
            ViewBag.ListTriggerType = _mailManageService.Trigger(false);
            MailModel mail = _mailManageService.MailByID(criteria);
            if (mail != null)
            {
                mail.MailName += Labels.CopyPrefix;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            CreateMailCriteria model = new CreateMailCriteria(mail);
            return View("~/Areas/MailStation/ManageMail/CreateMail/Index.cshtml", model);
        }

        /// <summary>
        /// Creates the mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Result message.</returns>
        public JsonResult CreateMail(CreateMailCriteria criteria)
        {
            if (criteria.StartOn != null)
            {
                criteria.StartOn = criteria.StartOn.Value.ToUniversalTime();
            }

            if (criteria.EndOn != null)
            {
                criteria.EndOn = criteria.EndOn.Value.ToUniversalTime();
            }

            _mailManageService = new MailManageService();
            return Json(_mailManageService.CreateMail(criteria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MailList(GetMailListCriteria criteria)
        {
            _mailManageService = new MailManageService();
            var result = _mailManageService.MailList(criteria);
            return Json(result);
        }

        public ActionResult DeleteMail(DeleteMailCriteria criteria)
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.DeleteMail(criteria));
        }

        public ActionResult UpdateMailView(ReadMailByIdCriteria criteria)
        {
            _mailManageService = new MailManageService();
            ViewBag.ListMailGroup = _mailManageService.MailGroups(false);
            ViewBag.ListSite = _mailManageService.Sites(false);
            ViewBag.ListTriggerType = _mailManageService.Trigger(false);
            MailModel mail = _mailManageService.MailByID(criteria);
            UpdateMailCriteria model = new UpdateMailCriteria(mail);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageMail/UpdateMail/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMail(UpdateMailCriteria criteria)
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.UpdateMail(criteria), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuggestMail(SuggestMailCriteria criteria)
        {
            _mailManageService = new MailManageService();
            return Json(_mailManageService.SuggestMails(criteria), JsonRequestBehavior.AllowGet);
        }

        public void RunNow(int mailId)
        {
            _mailManageService = new MailManageService();
            _mailManageService.RunJobImmediately(mailId);
        }
    }
}
