namespace Fanex.MailDelivery.Client.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Mail Client Section.
    /// </summary>
    public class MailClientSection : ConfigurationSection
    {
        public static MailClientSection GetConfig(string sectionName = "Fanex.MailDelivery.Client")
        {
            return ConfigurationManager.GetSection(sectionName) as MailClientSection;
        }

        [ConfigurationProperty("Functions", IsRequired=false)]
        public FunctionElementCollection Functions
        {
            get { return (FunctionElementCollection)this["Functions"]; }
            set { this["Functions"] = value; }
        }

        [ConfigurationProperty("Schedules", IsRequired= false)]
        public ScheduleElementCollection Schedules
        {
            get { return (ScheduleElementCollection)this["Schedules"]; }
            set { this["Schedules"] = value; }
        }

        public bool HasSchedule()
        {
            return Schedules.Count > 0;
        }

        [ConfigurationProperty("applicationSendMailId", IsRequired = true)]
        public int ApplicationSendMailId
        {
            get { return (int)this["applicationSendMailId"]; }
            set { this["applicationSendMailId"] = value; }
        }

        [ConfigurationProperty("siteId", IsRequired = true)]
        public int SiteId
        {
            get { return (int)this["siteId"]; }
            set { this["siteId"] = value; }
        }

        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"]; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        [ConfigurationProperty("messageBrokerLogCategory", IsRequired = true)]
        public string MessageBrokerLogCategory
        {
            get { return (string)this["messageBrokerLogCategory"]; }
            set { this["messageBrokerLogCategory"] = value; }
        }
    }
}