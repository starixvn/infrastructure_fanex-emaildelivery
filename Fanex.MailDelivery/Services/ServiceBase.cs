namespace Fanex.MailDelivery.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Logging;

    /// <summary>
    /// Base service for base logging.
    /// </summary>
    public class ServiceBase
    {
        public static void LogError(Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }

        public static void LogInfo(string messsage)
        {
            Logger.Log.Info(messsage);
        }
    }
}
