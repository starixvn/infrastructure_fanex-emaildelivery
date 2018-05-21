using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;

namespace Fanex.MailDelivery.Admin.Services.Group
{
    public class GroupService : AdminService, IGroupService
    {
        ~GroupService()
        {
            Dispose(false);
        }

        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            return Bus.Request<GetGroupListRequest, GetGroupListResponse>(request);
        }

        public GetMailGroupByIdResponse GetMailGroupById(GetMailGroupByIdRequest request)
        {
            return Bus.Request<GetMailGroupByIdRequest, GetMailGroupByIdResponse>(request);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }
    }
}