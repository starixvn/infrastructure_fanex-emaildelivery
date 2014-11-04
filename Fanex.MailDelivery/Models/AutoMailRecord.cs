namespace Fanex.MailDelivery.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Enumerations;

    /// <summary>
    /// Auto Mail Record.
    /// </summary>
    public class AutoMailRecord : MailRecord
    {
        public Schedule Schedule { get; set; }
    }
}
