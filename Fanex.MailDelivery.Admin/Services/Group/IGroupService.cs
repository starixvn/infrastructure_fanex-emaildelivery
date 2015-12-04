#region

using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;

#endregion

namespace Fanex.MailDelivery.Admin.Services.Group
{
    public interface IGroupService : IDisposable
    {
        GetGroupListResponse GetGroupList(GetGroupListRequest request);

        GetMailGroupByIdResponse GetMailGroupById(GetMailGroupByIdRequest request);

        ResultResponse CreateGroup(CreateGroupRequest request);

        ResultResponse UpdateGroup(UpdateGroupRequest request);

        ResultResponse DeleteGroup(DeleteGroupRequest request);
    }
}