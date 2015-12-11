using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models.Group;

namespace Fanex.MailDelivery.Admin.Messaging.ManageGroup
{
    public class GetGroupListResponse
    {
        public IList<GroupModel> GroupModels { get; set; }
    }
}