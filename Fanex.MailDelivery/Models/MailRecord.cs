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
        public DateTime SendTime { get; set; }

        public MailStatus MailStatus { get; set; }
    }
}
