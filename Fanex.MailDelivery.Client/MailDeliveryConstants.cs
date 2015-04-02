namespace Fanex.MailDelivery.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Route key for deliver message.
    /// </summary>
    public class MailDeliveryConstants
    {
        public const string SendMailRoutingKey = "RoutingKey.SendMail";
        public const string JobResultRoutingKey = "RoutingKey.JobResult";
        public const string UpdateScheduleRoutingKeyFormat = "RoutingKey.UpdateSchedule_{0}";
        public const string RetryJobRoutingKeyFormat = "RoutingKey.RetryJob_{0}";

        public const string AutoMailJobNameFormat = "AutoMailJob_{0}";
        public const string MailCentralizedJobGroup = "Fanex.MailDelivery.Client.MailCentralizedJobGroup";
        public const string MailCentralizedTriggerGroup = "Fanex.MailDelivery.Client.MailCentralizedTriggerGroup";
        public const string JobRetryNameFormat = "JobRetry_{0}";
        public const string TriggerRetryNameFormat = "TriggerRetry_{0}";

        public const string ScheduleJobDataKey = "Fanex.MailDelivery.Client.ScheduleJobDataKey";
        public const string RabbitMQLoginPassword = ";password=1q2w3e$R";

        public const int JobFailedErrorCode = 1000;
        public const int RetryDelayInSecond = 5;

        // Server Time is UTC -4.
        public const string LocalServerTimeZone = "Atlantic Standard Time";

        public const string LiveServerTimeZone = "SA Western Standard Time";
    }
}
