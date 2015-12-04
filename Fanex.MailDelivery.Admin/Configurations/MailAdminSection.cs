#region

using System.Configuration;
using Fanex.MailDelivery.Core;

#endregion

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

        [ConfigurationProperty("clientNotifyTimeOutInSecond", DefaultValue = "10", IsRequired = false)]
        public int ClientNotifyTimeOutInSecond
        {
            get { return (int)this["clientNotifyTimeOutInSecond"]; }
            set { this["clientNotifyTimeOutInSecond"] = value; }
        }

        public static MailAdminSection GetConfig(string sectionName = "Fanex.MailDelivery.Admin")
        {
            return ConfigurationManager.GetSection(sectionName) as MailAdminSection;
        }
    }
}