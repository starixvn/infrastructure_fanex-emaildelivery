namespace Fanex.MailDelivery.Admin.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Application Settings.
    /// </summary>
    public class AppSettings
    {
        public static MailAdminSection MailAdminConfig { private set; get; }

        static AppSettings()
        {
            MailAdminConfig = MailAdminSection.GetConfig();
        }
    }
}
