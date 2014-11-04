// -----------------------------------------------------------------------
// <copyright file="UpdateScheduleMailingList.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Messages.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateScheduleMailingListMessage
    {
        public int SiteId { get; set; }

        public int ScheduleId { get; set; }

        public MailingListMessage MailingList { get; set; }
    }
}
