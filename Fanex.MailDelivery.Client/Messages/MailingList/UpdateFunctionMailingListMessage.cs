// -----------------------------------------------------------------------
// <copyright file="UpdateFunctionMailingListMessage.cs" company="">
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
    public class UpdateFunctionMailingListMessage
    {
        public int SiteId { get; set; }

        public int FunctionId { get; set; }

        public MailingListMessage MailingList { get; set; }
    }
}
