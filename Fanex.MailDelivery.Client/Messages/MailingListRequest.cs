namespace Fanex.MailDelivery.Client.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// Function Mailing List Request.
    /// </summary>
    public class MailingListRequest
    {
        public int SiteId { get; set; }

        public int MailingListId { get; set; }

        public MailType MailType { get; set; }
    }
}
