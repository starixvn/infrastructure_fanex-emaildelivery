using System;
using Fanex.MailDelivery.Core.Messaging;

namespace Fanex.MailDelivery.Core.Models
{
    /// <summary>
    /// Job Status Message.
    /// </summary>
    public class Job
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public TaskLog TaskLog { get; set; }

        public DateTime ScheduledTime { get; set; }

        public DateTime RunningTime { get; set; }
    }
}