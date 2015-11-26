namespace Fanex.MailDelivery.Core.Messaging.Recipient
{
    public class UpdateRecipientMessage
    {
        public int RecipientId { get; set; }

        public string Username { get; set; }

        public int EmailID1 { get; set; }

        public int EmailID2 { get; set; }

        public int EmailID3 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Note { get; set; }

        public bool Status { get; set; }
    }
}
