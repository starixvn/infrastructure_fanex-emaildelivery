namespace Fanex.MailDelivery.Client.Messages.MailManage
{
    using System;

    public class CreateMailMessage
    {
        public string MailName { get; set; }

        public int MailGroupId { get; set; }

        public int SiteId { get; set; }

        //// public int TriggerType { get; set; }

        //// public int RepeatType { get; set; }

        //// public string CronExpression { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive { get; set; }

        //// public DateTime? StartOn { get; set; }

        //// public DateTime? EndOn { get; set; }

        //// public int Attempt { get; set; }

        //// public int Interval { get; set; }

        public string Description { get; set; }
    }
}
