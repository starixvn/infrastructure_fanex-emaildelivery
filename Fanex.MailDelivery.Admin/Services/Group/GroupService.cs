using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Services.Group
{
    public class GroupService : AdminService, IGroupService
    {
        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            return Bus.Request<GetGroupListRequest, GetGroupListResponse>(request);
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return Bus.Request<CreateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return Bus.Request<UpdateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return Bus.Request<DeleteGroupRequest, ResultResponse>(request);
        }
    }
}