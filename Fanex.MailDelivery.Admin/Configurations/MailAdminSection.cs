using Fanex.MailDelivery.Core;
using System.Configuration;

namespace Fanex.MailDelivery.Admin.Configurations
{
    public class MailAdminSection : ConfigurationSection
    {
        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"] + MailDeliveryConstants.RabbitMQLoginPassword; }
            set { this["rabbitMQConnectionString"] = value; }
        }

        public static MailAdminSection GetConfig(string sectionName = "Fanex.MailDelivery.Admin")
        {
            return ConfigurationManager.GetSection(sectionName) as MailAdminSection;
        }
    }
}