﻿// --------------------------------------------------------------------------
// <copyright file="MailRecipientController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.MailDelivery.MailStation.Services.ManageRecipient;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Recipient Controller.
    /// </summary>
    public class MailRecipientController : BaseController
    {
        private ManageRecipientService _manageRecipientService;

        public MailRecipientController()
        {
            _manageRecipientService = _manageRecipientService ?? new ManageRecipientService();
        }

        public ActionResult Index()
        {
            ViewBag.ListStatus = _manageRecipientService.Status(true);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageRecipient/ManageRecipient/Index.cshtml");
        }

        public ActionResult CreateRecipientView()
        {
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageRecipient/CreateRecipient/Index.cshtml");
        }

        public ActionResult RecipientList(RecipientListCriteria criteria)
        {
            return Json(_manageRecipientService.RecipientList(criteria));
        }

        public ActionResult UpdateRecipientView(RecipientByIdCriteria criteria)
        {
            var recipient = _manageRecipientService.RecipientById(criteria);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageRecipient/UpdateRecipient/Index.cshtml", recipient);
        }

        [HttpPost]
        public JsonResult UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            return Json(_manageRecipientService.EditRecipient(criteria));
        }

        [HttpPost]
        public JsonResult DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            return Json(_manageRecipientService.DeleteRecipient(criteria));
        }

        [HttpPost]
        public JsonResult CreateRecipient(CreateRecipientCriteria criteria)
        {
            return Json(_manageRecipientService.InsertRecipient(criteria));
        }

        [HttpPost]
        public JsonResult CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return Json(_manageRecipientService.CheckRecipient(criteria));
        }

        public ActionResult ViewRelevantMail(RecipientByIdCriteria criteria)
        {
            var recipient = _manageRecipientService.RecipientById(criteria);
            var emails = new string[] { recipient.Email1, recipient.Email2, recipient.Email3 };

            return RedirectToAction("Index", "MailManage", new { Emails = string.Join(",", emails).Trim(',') });
        }
    }
}