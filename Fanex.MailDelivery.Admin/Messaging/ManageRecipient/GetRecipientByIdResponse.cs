#region

using Fanex.MailDelivery.Core.Models.Recipient;

#endregion

namespace Fanex.MailDelivery.Admin.Messaging.ManageRecipient
{
    public class GetRecipientByIdResponse
    {
        public RecipientModel RecipientModel { get; set; }
    }
}