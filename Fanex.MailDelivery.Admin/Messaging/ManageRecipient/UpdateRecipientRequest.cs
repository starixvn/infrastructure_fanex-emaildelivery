namespace Fanex.MailDelivery.Admin.Messaging.ManageRecipient
{
    public class UpdateRecipientRequest : CreateRecipientRequest
    {
        public int RecipientId { get; set; }

        public int EmailId1 { get; set; }

        public int EmailId2 { get; set; }

        public int EmailId3 { get; set; }
    }
}