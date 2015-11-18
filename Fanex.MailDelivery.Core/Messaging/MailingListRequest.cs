using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Core.Messaging
{
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
