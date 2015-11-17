// --------------------------------------------------------------------------
// <copyright file="MailGroupManageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Services.Group;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.MailDelivery.MailStation.Services.MailGroup;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailGroupManageController : BaseController
    {
        private MailGroupService _mailGroupService;

        private IGroupService _groupService;

        public MailGroupManageController()
        {
            this._mailGroupService = this._mailGroupService ?? new MailGroupService();

            _groupService = new GroupService();
        }

        public ActionResult Index(GetMailGroupCriteria criteria)
        {
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/ManageMailGroup/Index.cshtml", criteria);
        }

        /// <summary>
        /// Creates the mail view.
        /// </summary>
        /// <returns>Create Mail View.</returns>
        public ActionResult CreateMailGroupView()
        {
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/CreateMailGroup/Index.cshtml");
        }

        /// <summary>
        /// Creates the mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Result message.</returns>
        public JsonResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            CreateGroupRequest request = new CreateGroupRequest
            {
                Name = criteria.MailGroupName,
                Description = criteria.Description
            };

            //ResultResponse response = _groupService.CreateGroup(request);

            //CommandResult commandResult = new CommandResult
            //{
            //    Code = response.ErrCode,
            //    Message = response.ErrMsg
            //};

           // return Json(commandResult);
             return Json(this._mailGroupService.CreateMailGroup(criteria));
        }

        public JsonResult MailGroupList(GetMailGroupCriteria criteria)
        {
            GetGroupListRequest request = new GetGroupListRequest
            {
                Name = criteria.MailGroupName
            };

            GetGroupListResponse response = _groupService.GetGroupList(request);

            return Json(response.GroupDtos);
            // return Json(this._mailGroupService.GetMailGroup(criteria));
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
        public ActionResult UpdateMailGroupView(GetMailGroupByIdCriteria criteria)
        {
            var mailGroup = this._mailGroupService.GetMailGroupByID(criteria);
            var model = new UpdateMailGroupCriteria();
            if (mailGroup != null)
            {
                model.MailGroupId = mailGroup.MailGroupId;
                model.MailGroupName = mailGroup.MailGroupName;
                model.Description = mailGroup.Description;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/UpdateMailGroup/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
        {
            return Json(this._mailGroupService.UpdateMailGroup(criteria));
        }

        public ActionResult ViewRelevantMail(int mailGroupId)
        {
            return RedirectToAction("Index", "MailManage", new { MailGroupId = mailGroupId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _groupService != null)
            {
                _groupService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
