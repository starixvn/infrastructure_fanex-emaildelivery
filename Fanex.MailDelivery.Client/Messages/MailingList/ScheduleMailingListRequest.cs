namespace Fanex.MailDelivery.Client.Messages.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Schedule Mailing List Request.
    /// </summary>
    public class ScheduleMailingListRequest
    {
        public int SiteId { get; set; }

        public int ScheduleId { get; set; }
    }
}
