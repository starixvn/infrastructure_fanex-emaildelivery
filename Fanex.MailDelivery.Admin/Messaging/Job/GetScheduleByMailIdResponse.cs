using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Messaging.Job
{
    public class GetScheduleByMailIdResponse
    {
        public Schedule Schedule { get; set; }
    }
}