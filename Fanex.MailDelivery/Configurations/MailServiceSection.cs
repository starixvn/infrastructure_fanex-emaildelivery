namespace Fanex.MailDelivery.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Mail Service Section.
    /// </summary>
    public class MailServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("checkAutoMailStatusSchedule", IsRequired = true)]
        public string CheckAutoMailStatusSchedule
        {
            get { return (string)this["checkAutoMailStatusSchedule"]; }
            set { this["checkAutoMailStatusSchedule"] = value; }
        }

        [ConfigurationProperty("insertMailStatusRecordSchedule", IsRequired = true)]
        public string InsertMailStatusRecordSchedule
        {
            get { return (string)this["insertMailStatusRecordSchedule"]; }
            set { this["insertMailStatusRecordSchedule"] = value; }
        }

        [ConfigurationProperty("isRunAutoMailStatusReport", IsRequired = true)]
        public bool IsRunAutoMailStatusReport
        {
            get { return (bool)this["isRunAutoMailStatusReport"]; }
            set { this["isRunAutoMailStatusReport"] = value; }
        }

        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"] + Fanex.MailDelivery.Client.MailDeliveryConstants.RabbitMQLoginPassword; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        public static MailServiceSection GetConfig(string sectionName = "Fanex.MailDelivery.Server")
        {
            return ConfigurationManager.GetSection(sectionName) as MailServiceSection;
        }
    }
}