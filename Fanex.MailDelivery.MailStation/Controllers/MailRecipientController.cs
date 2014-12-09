// --------------------------------------------------------------------------
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
    using Fanex.MailDelivery.MailStation.Services.ManageRecipient;

    /// <summary>
    /// Mail Recipient Controller.
    /// </summary>
    public class MailRecipientController : Controller
    {
        private IManageRecipientService _manageRecipientService;

        public MailRecipientController()
            : this(null)
        {
        }

        public MailRecipientController(IManageRecipientService manageRecipientService)
        {
            _manageRecipientService = manageRecipientService ?? new ManageRecipientService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/MailStation/ManageRecipient/ManageRecipient/Index.cshtml");
        }

        public ActionResult InsertRecipientView()
        {
            return View("~/Areas/MailStation/ManageRecipient/InsertRecipient/Index.cshtml");
        }

        public ActionResult RecipientList(RecipientListCriteria criteria)
        {
            return Json(_manageRecipientService.RecipientList(criteria));
        }

        public ActionResult EditRecipientView(int recipientId)
        {
            var recipient = _manageRecipientService.RecipientById(new RecipientByIdCriteria
            {
                RecipientId = recipientId
            });

            return View("~/Areas/MailStation/ManageRecipient/EditRecipient/Index.cshtml", recipient);
        }

        [HttpPost]
        public JsonResult EditRecipient(UpdateRecipientCriteria criteria)
        {
            return Json(_manageRecipientService.EditRecipient(criteria));
        }

        [HttpPost]
        public JsonResult InsertRecipient(InsertRecipientCriteria criteria)
        {
            return Json(_manageRecipientService.InsertRecipient(criteria));
        }

        [HttpPost]
        public JsonResult CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return Json(_manageRecipientService.CheckRecipient(criteria));
        }

        public ActionResult ViewRelevantMail(int recipientId)
        {
            var recipient = _manageRecipientService.RecipientById(new RecipientByIdCriteria
            {
                RecipientId = recipientId
            });

            return View("~/Areas/MailStation/ManageMail/Index.cshtml", recipient);
        }
    }
}
