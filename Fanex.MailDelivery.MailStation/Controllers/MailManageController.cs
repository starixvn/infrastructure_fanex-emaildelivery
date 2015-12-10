// --------------------------------------------------------------------------
// <copyright file="MailManageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Services.Job;
using Fanex.MailDelivery.Admin.Services.JobOperation;
using Fanex.MailDelivery.Admin.Services.Mail;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.MailDelivery.MailStation.Resources;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailManageController : BaseController
    {
        private MailManageService _mailManageService;

        private IJobOperationService _jobOperationService;

        public MailManageController()
        {
            _mailManageService = new MailManageService(new MailService(), new JobService(), new JobOperationService());

            _jobOperationService = new JobOperationService();
        }

        public ActionResult Index(GetMailListCriteria criteria)
        {
            ViewBag.ListMailGroup = _mailManageService.MailGroups();
            ViewBag.ListSite = _mailManageService.Sites();
            ViewBag.ListStatus = _mailManageService.Status();
            ViewBag.ListTriggerType = _mailManageService.Trigger();
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageMail/ManageMail/Index.cshtml", criteria);
        }

        public JsonResult Sites()
        {
            return Json(_mailManageService.Sites());
        }

        public JsonResult MailGroups(bool hasAllElement = true)
        {
            return Json(_mailManageService.MailGroups(hasAllElement));
        }

        public JsonResult AddMailGroup(CreateMailGroupCriteria criteria)
        {
            return Json(_mailManageService.CreateMailGroup(criteria));
        }

        /// <summary>
        /// Creates the mail view.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Create Mail View.</returns>
        public ActionResult CreateMailView(GetMailByIdCriteria criteria)
        {
            ViewBag.ListMailGroup = _mailManageService.MailGroups(false);
            ViewBag.ListSite = _mailManageService.Sites(false);
            ViewBag.ListTriggerType = _mailManageService.Trigger(false);

            MailModel mailModel = _mailManageService.MailById(criteria);

            if (mailModel != null)
            {
                mailModel.MailName += Labels.CopyPrefix;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            CreateMailCriteria model = new CreateMailCriteria(mailModel);
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

            return Json(_mailManageService.CreateMail(criteria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MailList(GetMailListCriteria criteria)
        {
            IList<MailModel> result = _mailManageService.MailList(criteria);
            return Json(result);
        }

        public ActionResult DeleteMail(DeleteMailCriteria criteria)
        {
            CommandResult commandResult = _mailManageService.DeleteMail(criteria);
            return Json(commandResult);
        }

        public ActionResult UpdateMailView(GetMailByIdCriteria criteria)
        {
            ViewBag.ListMailGroup = _mailManageService.MailGroups(false);
            ViewBag.ListSite = _mailManageService.Sites(false);
            ViewBag.ListTriggerType = _mailManageService.Trigger(false);

            MailModel mailModel = _mailManageService.MailById(criteria);

            UpdateMailCriteria model = new UpdateMailCriteria(mailModel);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageMail/UpdateMail/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMail(UpdateMailCriteria criteria)
        {
            CommandResult commandResult = _mailManageService.UpdateMail(criteria);
            return Json(commandResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SuggestMail(SuggestMailCriteria criteria)
        {
            return Json(_mailManageService.SuggestMails(criteria), JsonRequestBehavior.AllowGet);
        }

        public void RunNow(int mailId)
        {
            _mailManageService.RunNow(mailId);
        }
    }
}
