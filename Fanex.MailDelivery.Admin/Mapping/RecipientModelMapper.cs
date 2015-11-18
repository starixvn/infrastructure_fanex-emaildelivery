using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Models;

namespace Fanex.MailDelivery.Admin.Mapping
{
    public static class RecipientModelMapper
    {
        public static RecipientDto ToRecipientDto(this RecipientModel recipientModel)
        {
            return new RecipientDto
            {
                RecipientId = recipientModel.RecipientId,
                RecipientName = recipientModel.RecipientName,

                EmailId1 = recipientModel.EmailId1,
                EmailId2 = recipientModel.EmailId2,
                EmailId3 = recipientModel.EmailId3,

                Email1 = recipientModel.Email1,
                Email2 = recipientModel.Email2,
                Email3 = recipientModel.Email3,

                IsNapUser = recipientModel.IsNapUser,
                NapUserId = recipientModel.NapUserId,

                IsActive = recipientModel.IsActive,

                MailRelevant = recipientModel.MailRelevant,

                Note = recipientModel.Note
            };
        }
    }
}
