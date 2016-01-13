using System.Configuration;
using Fanex.MailDelivery.Core;

namespace Fanex.MailDelivery.Admin.Configurations
{
    /// <summary>
    ///     Mail Admin Section.
    /// </summary>
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