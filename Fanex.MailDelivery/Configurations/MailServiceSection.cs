namespace Fanex.MailDelivery.Configurations
{
    using System.Configuration;

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
            get { return (string)this["rabbitMQConnectionString"] + Fanex.MailDelivery.Client.MailDeliveryConstants.RabbitMQLoginPassword; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        public static MailServiceSection GetConfig(string sectionName = "Fanex.MailDelivery.Server")
        {
            return ConfigurationManager.GetSection(sectionName) as MailServiceSection;
        }
    }
}