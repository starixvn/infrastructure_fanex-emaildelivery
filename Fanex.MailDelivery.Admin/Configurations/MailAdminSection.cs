namespace Fanex.MailDelivery.Admin.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// Mail Admin Section.
    /// </summary>
    public class MailAdminSection : ConfigurationSection
    {
        public static MailAdminSection GetConfig(string sectionName = "Fanex.MailDelivery.Admin")
        {
            return ConfigurationManager.GetSection(sectionName) as MailAdminSection;
        }

        [ConfigurationProperty("rabbitMQConnectionString", IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)this["rabbitMQConnectionString"]; }
            set { this["rabbitMQConnectionString"] = value; }
        }
    }
}
