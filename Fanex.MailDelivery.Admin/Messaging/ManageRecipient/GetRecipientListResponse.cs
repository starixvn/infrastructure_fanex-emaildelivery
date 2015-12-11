using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models.Recipient;

namespace Fanex.MailDelivery.Admin.Messaging.ManageRecipient
{
    public class GetRecipientListResponse
    {
        public IList<RecipientModel> RecipientModels { get; set; }
    }
}