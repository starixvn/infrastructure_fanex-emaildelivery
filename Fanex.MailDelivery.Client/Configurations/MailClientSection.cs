using Fanex.MailDelivery.Core;
using System.Configuration;

namespace Fanex.MailDelivery.Client.Configurations
{
    public class MailClientSection : ConfigurationSection
    {
        [ConfigurationProperty("applicationSendMailId", IsRequired = true)]
        public int ApplicationSendMailId
        {
            get { return (int)this["applicationSendMailId"]; }
            set { this["applicationSendMailId"] = value; }
        }

        [ConfigurationProperty("environment", IsRequired = false)]
        public string Environment
        {
            get { return (string)this["environment"]; }
            set { this["environment"] = value; }
        }

        [ConfigurationProperty("Functions", IsRequired = false)]
        public FunctionElementCollection Functions
        {
            get { return (FunctionElementCollection)this["Functions"]; }
            set { this["Functions"] = value; }
        }

        [ConfigurationProperty("rabbitMQConnectionRetryLimit", IsRequired = false, DefaultValue = 5)]
        public int RabbitMQConnectionRetryLimit
        {
            get
            {
                var retryLimit = (int)this["rabbitMQConnectionRetryLimit"];

                return (retryLimit > 0 ? retryLimit : 1);
            }
            set { this["rabbitMQConnectionRetryLimit"] = value; }
        }

        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"] + MailDeliveryConstants.RabbitMQLoginPassword; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        [ConfigurationProperty("Schedules", IsRequired = false)]
        public ScheduleElementCollection Schedules
        {
            get { return (ScheduleElementCollection)this["Schedules"]; }
            set { this["Schedules"] = value; }
        }

        [ConfigurationProperty("siteId", IsRequired = true)]
        public int SiteId
        {
            get { return (int)this["siteId"]; }
            set { this["siteId"] = value; }
        }

        public static MailClientSection GetConfig(string sectionName = "Fanex.MailDelivery.Client")
        {
            return ConfigurationManager.GetSection(sectionName) as MailClientSection;
        }

        public bool HasSchedule()
        {
            return Schedules.Count > 0;
        }

        public static bool IsLocalEnvironment()
        {
            string environment = MailClientSection.GetConfig().Environment ?? string.Empty;
            bool isLocal = environment.Length == 0 || environment.Trim().Equals("local");

            return isLocal;
        }
    }
}