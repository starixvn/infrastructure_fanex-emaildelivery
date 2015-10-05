namespace Fanex.MailDelivery.Client.Messages.Recipient
{
    public class CreateRecipientMessage
    {
        public string UserName { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public bool IsNAPUser { get; set; }

        public int NAPUserID { get; set; }

        public string Note { get; set; }

        public bool Status { get; set; }
    }
}
