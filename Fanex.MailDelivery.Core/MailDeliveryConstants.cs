namespace Fanex.MailDelivery.Core
{
    public static class MailDeliveryConstants
    {
        public const string SendMailRoutingKey = "RoutingKey.SendMail";
        public const string JobResultRoutingKey = "RoutingKey.JobResult";
        public const string UpdateScheduleRoutingKeyFormat = "RoutingKey.UpdateSchedule_{0}";
        public const string RetryJobRoutingKeyFormat = "RoutingKey.RetryJob_{0}";
        public const string AbortJobRoutingKeyFormat = "RoutingKey.AbortJob";
        public const string RunNowJobRoutingKeyFormat = "RoutingKey.RunNowJob_{0}";

        public const string AutoMailJobNameFormat = "AutoMailJob_{0}";
        public const string MailCentralizedTriggerGroup = "Fanex.MailDelivery.Client.MailCentralizedTriggerGroup";

        public const string ScheduleJobDataKey = "Fanex.MailDelivery.Client.ScheduleJobDataKey";
        public const string RabbitMQLoginPassword = ";password=1q2w3e$R";

        public const string ClientNotificationJobName = "ClientNotificationJobName";
        public const string ClientNotificationTriggerName = "ClientNotificationTriggerName";
        public const string ClientNotificationTopic = "ClientNotificationTopic";

        public const int ClientNotificationIntervalTimeInSecond = 10;

        // Server Time is UTC -4.
        public const string LocalServerTimeZone = "SA Western Standard Time";

        public const string LiveServerTimeZone = "SA Western Standard Time";
    }
}
