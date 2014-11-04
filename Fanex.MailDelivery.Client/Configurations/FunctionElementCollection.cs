namespace Fanex.MailDelivery.Client.Configurations
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ConfigurationCollection(typeof(FunctionElement))]
    public class FunctionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FunctionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FunctionElement)element).Name;
        }
    }
}