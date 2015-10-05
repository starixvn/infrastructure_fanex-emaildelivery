namespace Fanex.MailDelivery.Client.Messages.MailManage
{
    public class GetMailListMessage
    {
        public string MailName { get; set; }

        public int IsActive { get; set; }

        public string MailGroupId { get; set; }

        public int SiteId { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }
    }
}
