namespace Fanex.MailDelivery.Admin.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Mail Admin Section.
    /// </summary>
    public class MailAdminSection : ConfigurationSection
    {
        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"]; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        public static MailAdminSection GetConfig(string sectionName = "Fanex.MailDelivery.Admin")
        {
            return ConfigurationManager.GetSection(sectionName) as MailAdminSection;
        }
    }
}