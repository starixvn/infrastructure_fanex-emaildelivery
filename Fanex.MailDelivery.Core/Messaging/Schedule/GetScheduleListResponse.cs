using System.Collections.Generic;

namespace Fanex.MailDelivery.Core.Messaging.Schedule
{
    public class GetScheduleListResponse
    {
        public IList<ScheduleDto> ScheduleDtos { get; set; }
    }
}
