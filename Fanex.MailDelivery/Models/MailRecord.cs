namespace Fanex.MailDelivery.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Enumerations;

    /// <summary>
    /// Mail Record.
    /// </summary>
    public class MailRecord
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public int SiteId { get; set; }

        public DateTime SentTime { get; set; }

        public MailStatus MailStatus { get; set; }
    }
}
