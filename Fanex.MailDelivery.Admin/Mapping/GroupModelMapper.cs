using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Models;

namespace Fanex.MailDelivery.Admin.Mapping
{
    public static class GroupModelMapper
    {
        public static GroupDto ToGroupDto(this GroupModel groupModel)
        {
            return new GroupDto
            {
                Id = groupModel.MailGroupId,
                Name = groupModel.MailGroupName,
                Description = groupModel.Description,
                RelevantMailCount = groupModel.MailRelevantCount
            };
        }
    }
}
