namespace Fanex.MailDelivery.Admin.Messaging.ManageRecipient
{
    public class CreateRecipientRequest
    {
        public string UserName { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public bool IsNapUser { get; set; }

        public int NapUserId { get; set; }

        public string Note { get; set; }

        public bool Status { get; set; }
    }
}