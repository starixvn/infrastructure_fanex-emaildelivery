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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Services.ManageMail;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailManageController : Controller
    {
        private MailManageService mailManageService;

        public ActionResult Index(GetMailListCriteria criteria)
        {
            mailManageService = new MailManageService();
            ViewBag.ListMailGroup = mailManageService.MailGroups();
            ViewBag.ListSite = mailManageService.Sites();
            ViewBag.ListStatus = mailManageService.Status();
            ViewBag.ListTriggerType = mailManageService.Trigger();
            return View("~/Areas/MailStation/ManageMail/Index.cshtml");
        }

        public JsonResult Sites()
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.Sites());
        }

        public JsonResult MailGroups()
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.MailGroups());
        }

        public JsonResult AddMailGroup(CreateMailGroupCriteria criteria)
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.CreateMailGroup(criteria));
        }

        public ActionResult CreateMailView(ReadMailByIDCriteria criteria)
        {
            mailManageService = new MailManageService();
            ViewBag.ListMailGroup = mailManageService.MailGroups(false);
            ViewBag.ListSite = mailManageService.Sites(false);
            ViewBag.ListTriggerType = mailManageService.Trigger(false);
            MailModel mail = mailManageService.MailByID(criteria);
            CreateMailCriteria model = new CreateMailCriteria(mail);
            return View("~/Areas/MailStation/ManageMail/CreateMail/Index.cshtml", model);
        }

        public JsonResult CreateMail(CreateMailCriteria criteria)
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.CreateMail(criteria));
        }

        public ActionResult MailList(GetMailListCriteria criteria)
        {
            mailManageService = new MailManageService();

            return Json(mailManageService.MailList(criteria));
        }

        public ActionResult DeleteMail()
        {
            return Json(new List<int>() { 3 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateMailView(ReadMailByIDCriteria criteria)
        {
            mailManageService = new MailManageService();
            ViewBag.ListMailGroup = mailManageService.MailGroups(false);
            ViewBag.ListSite = mailManageService.Sites(false);
            ViewBag.ListTriggerType = mailManageService.Trigger(false);
            MailModel mail = mailManageService.MailByID(criteria);
            UpdateMailCriteria model = new UpdateMailCriteria(mail);

            return View("~/Areas/MailStation/ManageMail/UpdateMail/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMail(UpdateMailCriteria criteria)
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.UpdateMail(criteria));
        }

        // [HttpPost]
        public JsonResult SuggestMail(SuggestMailCriteria criteria)
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.SuggestMails(criteria), JsonRequestBehavior.AllowGet);
        }
    }
}
