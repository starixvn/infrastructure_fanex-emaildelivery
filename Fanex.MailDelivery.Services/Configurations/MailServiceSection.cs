using System.Configuration;
using Fanex.MailDelivery.Core;

namespace Fanex.MailDelivery.Services.Configurations
{
    /// <summary>
    /// Mail Service Section.
    /// </summary>
    public class MailServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("isRunAutoMailStatusReport", IsRequired = true)]
        public bool IsRunAutoMailStatusReport
        {
            get { return (bool)this["isRunAutoMailStatusReport"]; }
            set { this["isRunAutoMailStatusReport"] = value; }
        }

        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"] + MailDeliveryConstants.RabbitMQLoginPassword; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        [ConfigurationProperty("clientNotifyTimeOutInSecond", DefaultValue = "10", IsRequired = false)]
        public int ClientNotifyTimeOutInSecond
        {
            get { return (int)this["clientNotifyTimeOutInSecond"]; }
            set { this["clientNotifyTimeOutInSecond"] = value; }
        }

        [ConfigurationProperty("checkClientOnlineStatusJobInterval", DefaultValue = "300", IsRequired = false)]
        public int CheckClientOnlineStatusJobInterval
        {
            get { return (int)this["checkClientOnlineStatusJobInterval"]; }
            set { this["checkClientOnlineStatusJobInterval"] = value; }
        }

        [ConfigurationProperty("adminMail", IsRequired = true)]
        public string AdminMail
        {
            get { return (string)this["adminMail"]; }
            set { this["adminMail"] = value; }
        }

        [ConfigurationProperty("appSendMailToAdminId", IsRequired = true)]
        public int AppSendMailToAdminId
        {
            get { return (int)this["appSendMailToAdminId"]; }
            set { this["appSendMailToAdminId"] = value; }
        }

        public static MailServiceSection GetConfig(string sectionName = "Fanex.MailDelivery.Server")
        {
            return ConfigurationManager.GetSection(sectionName) as MailServiceSection;
        }

        [ConfigurationProperty("maxRetrySendMailServiceCounter", DefaultValue = "3", IsRequired = false)]
        public int MaxRetrySendMailServiceCounter
        {
            get { return (int)this["maxRetrySendMailServiceCounter"]; }
            set { this["maxRetrySendMailServiceCounter"] = value; }
        }

        [ConfigurationProperty("maxRetryStartUpService", DefaultValue = "3", IsRequired = false)]
        public int MaxRetryStartUpService
        {
            get { return (int)this["maxRetryStartUpService"]; }
            set { this["maxRetryStartUpService"] = value; }
        }

        [ConfigurationProperty("maxIntervalStartUpServiceInSecond", DefaultValue = "300", IsRequired = false)]
        public int MaxIntervalStartUpServiceInSecond
        {
            get { return (int)this["maxIntervalStartUpServiceInSecond"]; }
            set { this["maxIntervalStartUpServiceInSecond"] = value; }
        }
    }
}