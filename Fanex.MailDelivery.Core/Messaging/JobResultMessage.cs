using System;
using Fanex.MailDelivery.Core.Enumerations;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Core.Messaging
{
    /// <summary>
    /// Job Status Message.
    /// </summary>
    public class JobResultMessage
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public TaskLog TaskLog { get; set; }

        public DateTime ScheduledTime { get; set; }

        public DateTime RunningTime { get; set; }
    }
}