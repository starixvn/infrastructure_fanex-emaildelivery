using System;

namespace Fanex.MailDelivery.Services.Messaging.MailManage
{
    public class CreateAutoMailRequest : CreateMailRequestBase
    {
        public int TriggerType { get; set; }

        public int RepeatType { get; set; }

        public string CronExpression { get; set; }

        public DateTime? StartOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int Attempt { get; set; }

        public int Interval { get; set; }
    }
}
