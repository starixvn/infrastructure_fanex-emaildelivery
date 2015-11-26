namespace Fanex.MailDelivery.Services.Messaging.MailManage
{
    public class GetMailListRequest
    {
        public GetMailListRequestDto GetMailListRequestDto { get; set; }
    }

    public class GetMailListRequestDto
    {
        public string MailName { get; set; }

        public int IsActive { get; set; }

        public string MailGroupId { get; set; }

        public int SiteId { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }
    }
}
