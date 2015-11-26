namespace Fanex.MailDelivery.Services.Messaging.MailManage
{
    public class CreateMailRequestBase
    {
        public string MailName { get; set; }

        public int MailGroupId { get; set; }

        public int SiteId { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
