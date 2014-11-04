namespace Fanex.MailDelivery.Client.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Logging;

    /// <summary>
    /// Base Handler.
    /// </summary>
    public class BaseHandler
    {
        public static void LogError(Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }

        public static void LogInfo(string message)
        {
            Logger.Log.Info(message);
        }
    }
}
