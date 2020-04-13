using Fanex.MailDelivery.Core;
using System;
using System.Configuration;

namespace Fanex.MailDelivery.MailStationService
{
    public static class AppSettings
    {
        public static string RabbitMQConnectionString
            => $"{ ConfigurationManager.AppSettings.Get("MailStation:RabbitMQConnectionString")}{MailDeliveryConstants.RabbitMQLoginPassword}";

        public static int SendMailRetryLimit => Convert.ToInt32(ConfigurationManager.AppSettings.Get("MailStation:SendMailRetryLimit"));

        public static int SendMailRetryIntervalInMinutes => Convert.ToInt32(ConfigurationManager.AppSettings.Get("MailStation:SendMailRetryIntervalInMinutes"));

        public static string ONEbookUMServiceUri => ConfigurationManager.AppSettings.Get("ONEbook.UM:ServiceUri");

        public static int UpdateFailedForLongRunningTaskIntervalInMinutes => Convert.ToInt32(ConfigurationManager.AppSettings.Get("MailStation:UpdateFailedForLongRunningTaskIntervalInMinutes"));

        public static string LogCategory => ConfigurationManager.AppSettings.Get("MailStation:LogCategory");

        public static string SentryDsn => ConfigurationManager.AppSettings.Get("MailStation:SentryDsn");
    }
}