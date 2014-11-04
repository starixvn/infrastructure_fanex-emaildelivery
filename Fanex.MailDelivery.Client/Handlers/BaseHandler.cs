// -----------------------------------------------------------------------
// <copyright file="BaseHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
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
