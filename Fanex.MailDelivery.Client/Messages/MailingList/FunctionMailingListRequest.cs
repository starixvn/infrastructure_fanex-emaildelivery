namespace Fanex.MailDelivery.Client.Messages.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Function Mailing List Request.
    /// </summary>
    public class FunctionMailingListRequest
    {
        public int SiteId { get; set; }

        public int FunctionId { get; set; }
    }
}
