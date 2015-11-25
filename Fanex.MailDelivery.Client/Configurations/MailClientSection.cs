using Fanex.MailDelivery.Core;

namespace Fanex.MailDelivery.Client.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Mail Client Section.
    /// </summary>
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

        [ConfigurationProperty("messageBrokerLogCategory", IsRequired = true)]
        public string MessageBrokerLogCategory
        {
            get { return (string)this["messageBrokerLogCategory"]; }
            set { this["messageBrokerLogCategory"] = value; }
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
    }
}