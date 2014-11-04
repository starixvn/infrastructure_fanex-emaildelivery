namespace Fanex.MailDelivery.Client.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ConfigurationCollection(typeof(JobInputDataElement))]
    public class JobInputDataElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobInputDataElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobInputDataElement)element).Name;
        }
    }
}
