namespace Fanex.MailDelivery.Admin.Messaging.ManageMail
{
    public class GetMailListRequest
    {
        public string MailName { get; set; }

        public int IsActive { get; set; }

        public string MailGroupId { get; set; }

        public int SiteId { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }
    }
}