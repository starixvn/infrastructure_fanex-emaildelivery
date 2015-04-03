// --------------------------------------------------------------------------
// <copyright file="MailGroupManageController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.MailDelivery.MailStation.Services.MailGroup;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailGroupManageController : BaseController
    {
        private MailGroupService _mailGroupService;

        public MailGroupManageController()
        {
            this._mailGroupService = this._mailGroupService ?? new MailGroupService();
        }

        public ActionResult Index(GetMailListCriteria criteria)
        {
            return View("~/Areas/MailStation/MailGroup/ManageMailGroup/Index.cshtml");
        }

        /// <summary>
        /// Creates the mail view.
        /// </summary>
        /// <returns>Create Mail View.</returns>
        public ActionResult CreateMailGroupView()
        {
            return View("~/Areas/MailStation/MailGroup/CreateMailGroup/Index.cshtml");
        }

        /// <summary>
        /// Creates the mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Result message.</returns>
        public JsonResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            return Json(this._mailGroupService.CreateMailGroup(criteria));
        }

        public JsonResult MailGroupList(GetMailGroupCriteria criteria)
        {
            return Json(this._mailGroupService.GetMailGroup(criteria));
        }

        public ActionResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
        {
            return Json(this._mailGroupService.DeleteMailGroup(criteria));
        }

        /// <summary>
        /// Updates mail group.
        /// </summary>
        /// <param name="criteria">Mail group by id.</param>
        /// <returns>Action Result.</returns>
        public ActionResult UpdateMailGroupView(GetMailGroupByIDCriteria criteria)
        {
            var mailGroup = this._mailGroupService.GetMailGroupByID(criteria);
            var model = new UpdateMailGroupCriteria();
            if (mailGroup != null)
            {
                model.MailGroupID = mailGroup.MailGroupID;
                model.MailGroupName = mailGroup.MailGroupName;
                model.Description = mailGroup.Description;
            }

            return View("~/Areas/MailStation/MailGroup/UpdateMailGroup/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
        {
            return Json(this._mailGroupService.UpdateMailGroup(criteria));
        }

        public ActionResult ViewRelevantMail(int mailGroupID)
        {
            return RedirectToAction("Index", "MailManage", new { MailGroupID = mailGroupID });
        }
    }
}
