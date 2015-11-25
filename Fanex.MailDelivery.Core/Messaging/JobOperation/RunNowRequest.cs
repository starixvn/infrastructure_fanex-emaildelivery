using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.MailDelivery.Core.Messaging.JobOperation
{
    public class RunNowRequest
    {
        public int SiteId { get; set; }

        public int MailId { get; set; }

        public int ScheduleId { get; set; }
    }
}
