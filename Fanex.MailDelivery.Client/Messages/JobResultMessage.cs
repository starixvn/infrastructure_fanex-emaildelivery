namespace Fanex.MailDelivery.Client.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// Job Status Message.
    /// </summary>
    public class JobResultMessage
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public ResultMessage ResultMessage { get; set; }

        public int RunningTime { get; set; }
    }
}
