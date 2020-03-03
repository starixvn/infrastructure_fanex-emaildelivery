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

        public static int SendMailRetryIntervalInSeconds => Convert.ToInt32(ConfigurationManager.AppSettings.Get("MailStation:SendMailRetryIntervalInSeconds"));
    }
}