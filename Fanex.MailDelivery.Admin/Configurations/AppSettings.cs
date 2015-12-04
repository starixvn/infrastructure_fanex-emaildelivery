namespace Fanex.MailDelivery.Admin.Configurations
{
    /// <summary>
    ///     Application Settings.
    /// </summary>
    public class AppSettings
    {
        static AppSettings()
        {
            MailAdminConfig = MailAdminSection.GetConfig();
        }

        public static MailAdminSection MailAdminConfig { get; private set; }
    }
}