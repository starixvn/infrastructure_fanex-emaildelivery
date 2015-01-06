namespace Fanex.MailDelivery.Client.Messages
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using System;

    /// <summary>
    /// Job Status Message.
    /// </summary>
    public class JobResultMessage
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public ResultMessage Result { get; set; }

        public TaskLog TaskLog { get; set; }

        public DateTime RunningTime { get; set; }
    }
}