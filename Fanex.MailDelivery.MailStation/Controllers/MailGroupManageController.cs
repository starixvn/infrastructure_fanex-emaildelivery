// --------------------------------------------------------------------------
// <copyright file="MailGroupManageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Services.Group;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Criteria.MailGroup;
    using Extensions;
    using Fanex.Maintenance.Controller;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailGroupManageController : BaseController
    {
        private readonly IGroupService _groupService;

        public MailGroupManageController()
        {
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

            ResultResponse response = _groupService.CreateGroup(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(commandResult);
        }

        public JsonResult MailGroupList(GetMailGroupCriteria criteria)
        {
            GetGroupListRequest request = new GetGroupListRequest
            {
                Name = criteria.MailGroupName
            };

            GetGroupListResponse response = _groupService.GetGroupList(request);

            return Json(response.GroupModels);
        }

        public ActionResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
        {
            DeleteGroupRequest request = new DeleteGroupRequest
            {
                Id = criteria.MailGroupId,
            };

            ResultResponse response = _groupService.DeleteGroup(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(commandResult);
        }

        /// <summary>
        /// Updates mail group.
        /// </summary>
        /// <param name="criteria">Mail group by id.</param>
        /// <returns>Action Result.</returns>
        public ActionResult UpdateMailGroupView(GetMailGroupByIdCriteria criteria)
        {
            GetMailGroupByIdRequest request = new GetMailGroupByIdRequest
            {
                Id = criteria.MailGroupId
            };

            GetMailGroupByIdResponse response = _groupService.GetMailGroupById(request);

            var model = new UpdateMailGroupCriteria();
            if (response != null)
            {
                model.MailGroupId = response.GroupModel.MailGroupId;
                model.MailGroupName = response.GroupModel.MailGroupName;
                model.Description = response.GroupModel.Description;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/UpdateMailGroup/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
        {
            UpdateGroupRequest request = new UpdateGroupRequest
            {
                Id = criteria.MailGroupId,
                Name = criteria.MailGroupName,
                Description = criteria.Description
            };

            ResultResponse response = _groupService.UpdateGroup(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return Json(commandResult);
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
