// --------------------------------------------------------------------------
// <copyright file="MailRecipientController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Services.Recipient;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Recipient Controller.
    /// </summary>
    public class MailRecipientController : BaseController
    {
        private ManageRecipientService _manageRecipientService;

        private IRecipientService _recipientService;

        public MailRecipientController()
        {
            _manageRecipientService = _manageRecipientService ?? new ManageRecipientService();

            _recipientService = new RecipientService();
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
            GetRecipientListRequest request = new GetRecipientListRequest
            {
                UserName = criteria.UserName,
                Emails = criteria.Emails,
                Status = criteria.Status
            };

            GetRecipientListResponse response = _recipientService.GetRecipientList(request);

            return Json(response.RecipientModels);
        }

        public ActionResult UpdateRecipientView(RecipientByIdCriteria criteria)
        {
            GetRecipientByIdRequest request = new GetRecipientByIdRequest
            {
                Id = criteria.RecipientId
            };

            GetRecipientByIdResponse response = _recipientService.GetRecipientById(request);

            RecipientModel recipientModel = new RecipientModel
            {
                RecipientId = response.RecipientModel.RecipientId,
                RecipientName = response.RecipientModel.RecipientName,

                EmailId1 = response.RecipientModel.EmailId1,
                EmailId2 = response.RecipientModel.EmailId2,
                EmailId3 = response.RecipientModel.EmailId3,
                Email1 = response.RecipientModel.Email1,
                Email2 = response.RecipientModel.Email2,
                Email3 = response.RecipientModel.Email3,

                IsNapUser = response.RecipientModel.IsNapUser,
                NapUserId = response.RecipientModel.NapUserId,

                IsActive = response.RecipientModel.IsActive,
                MailRelevant = response.RecipientModel.MailRelevant,
                Note = response.RecipientModel.Note
            };

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/ManageRecipient/UpdateRecipient/Index.cshtml", recipientModel);
        }

        [HttpPost]
        public JsonResult UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            UpdateRecipientRequest request = new UpdateRecipientRequest
            {
                RecipientId = criteria.RecipientId,
                UserName = criteria.Username,

                EmailId1 = criteria.EmailID1,
                EmailId2 = criteria.EmailID2,
                EmailId3 = criteria.EmailID3,

                Email1 = criteria.Email1,
                Email2 = criteria.Email2,
                Email3 = criteria.Email3,

                Note = criteria.Note,
                Status = criteria.Status
            };

            ResultResponse response = _recipientService.UpdateRecipient(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(commandResult);
        }

        [HttpPost]
        public JsonResult DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            DeleteRecipientRequest request = new DeleteRecipientRequest
            {
                RecipientId = criteria.RecipientId
            };

            ResultResponse response = _recipientService.DeleteRecipient(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(response);
        }

        [HttpPost]
        public JsonResult CreateRecipient(CreateRecipientCriteria criteria)
        {
            CreateRecipientRequest request = new CreateRecipientRequest
            {
                UserName = criteria.UserName,
                Email1 = criteria.Email1,
                Email2 = criteria.Email2,
                Email3 = criteria.Email3,
                IsNapUser = criteria.IsNAPUser,
                NapUserId = criteria.NAPUserID,
                Note = criteria.Note,
                Status = criteria.Status
            };

            ResultResponse response = _recipientService.CreateRecipient(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(commandResult);
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

        protected override void Dispose(bool disposing)
        {
            _recipientService.Dispose();

            base.Dispose(disposing);
        }
    }
}