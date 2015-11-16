using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Mapping
{
    public static class RecipientModelMapper
    {
        public static RecipientDto ToRecipientDto(this RecipientModel recipientModel)
        {
            return new RecipientDto
            {
                RecipientId = recipientModel.RecipientId,
                RecipientName =  recipientModel.RecipientName,

                IsNapUser =  recipientModel.IsNapUser,

                Email1 =  recipientModel.Email1,
                Email2 =  recipientModel.Email2,
                Email3 = recipientModel.Email3,
                
                MailRelevant = recipientModel.MailRelevant,

                IsActive = recipientModel.IsActive,
            };
        }
    }
}
