namespace Fanex.MailDelivery.Admin.Messaging.ManageMail
{
    public class GetMailListRequest
    {
        public GetMailListRequest()
        {
            Status = -1;
            TriggerType = -1;
            SiteId = -1;
        }

        public string MailName { get; set; }

        public int Status { get; set; }

        public string GroupIds { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }

        public int SiteId { get; set; }
    }
}