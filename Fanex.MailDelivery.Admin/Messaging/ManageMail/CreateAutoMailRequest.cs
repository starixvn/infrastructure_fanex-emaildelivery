using System;

namespace Fanex.MailDelivery.Admin.Messaging.ManageMail
{
    public class CreateAutoMailRequest : MailRequestBase
    {
        public int RepeatType { get; set; }

        public string CronExpression { get; set; }

        public DateTime? StartOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int Attempt { get; set; }

        public int Interval { get; set; }
    }
}
