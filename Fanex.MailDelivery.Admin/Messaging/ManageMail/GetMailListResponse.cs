using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models.Mails;

namespace Fanex.MailDelivery.Admin.Messaging.ManageMail
{
    public class GetMailListResponse
    {
        public IList<MailModel> MailModels { get; set; }
    }
}