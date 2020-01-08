using Fanex.MailDelivery.Admin.Mapping;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Services.Criteria.Group;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IGroupService
    {
        GetGroupListResponse GetGroupList(GetGroupListRequest request);

        GetMailGroupByIdResponse GetMailGroupById(GetMailGroupByIdRequest request);

        ResultResponse CreateGroup(CreateGroupRequest request);

        ResultResponse UpdateGroup(UpdateGroupRequest request);

        ResultResponse DeleteGroup(DeleteGroupRequest request);
    }

    public class GroupService : BaseAdminService, IGroupService
    {
        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            IList<GroupModel> groupModels = adminDataService.GetGroupList(new GetGroupListCriteria
            {
                MailGroupName = request.Name
            });

            return new GetGroupListResponse { GroupModels = groupModels };
        }

        public GetMailGroupByIdResponse GetMailGroupById(GetMailGroupByIdRequest request)
        {
            GroupModel groupModel = adminDataService.GetGroupById(new GetGroupByIdCriteria
            {
                MailGroupId = request.Id
            });

            return new GetMailGroupByIdResponse { GroupModel = groupModel };
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return adminDataService.CreateGroup(new CreateGroupCriteria
            {
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return adminDataService.UpdateGroup(new UpdateGroupCriteria
            {
                MailGroupId = request.Id,
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return adminDataService.DeleteGroup(new DeleteGroupCriteria
            {
                MailGroupId = request.Id
            }).ToResultResponse();
        }
    }
}