namespace Fanex.MailDelivery.Client.Messages.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mailing List ErrMsg.
    /// </summary>
    public class MailingListMessage
    {
        public List<string> TOs { get; set; }

        public List<string> CCs { get; set; }

        public List<string> BCCs { get; set; }
    }
}
