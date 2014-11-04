namespace Fanex.MailDelivery.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoMailRecord
    {
        public Schedule Schedule { get; set; }

        public DateTime SendTime { get; set; }

        public MailStatus MailStatus { get; set; }
    }
}
