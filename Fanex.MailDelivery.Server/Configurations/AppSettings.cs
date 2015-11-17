// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Server.Configurations
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class AppSettings
    {
        static AppSettings()
        {
            MailServiceSection = MailServiceSection.GetConfig();
        }

        public static MailServiceSection MailServiceSection { get; set; }
    }
}
