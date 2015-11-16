using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Messaging.ManageGroup
{
    public class GetGroupListResponse
    {
        public IList<GroupDto> GroupDtos { get; set; }
    }

    public class GroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RelevantMailCount { get; set; }
    }
}
