using Fanex.Logging;
using Fanex.Logging.Extensions;
using System;

namespace Fanex.MailDelivery.Client.Logging
{
    internal static class LogService
    {
        public static void LogError(Exception exception)
        {
            LogError(exception.Message, exception);
        }

        public static void LogError(string message, Exception exception)
        {
            var logMessage = new LogMessage(message)
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();

            Logger.Log.Error(logMessage);
        }

        public static void LogInfo(string logMessage)
        {
            Logger.Log.Info(logMessage);
        }
    }
}
