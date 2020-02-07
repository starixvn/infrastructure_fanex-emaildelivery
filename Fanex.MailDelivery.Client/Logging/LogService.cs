using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.MailDelivery.Core.Models;
using System;

namespace Fanex.MailDelivery.Client.Logging
{
    internal static class LogService
    {
        public static void LogInfo(string logMessage)
        {
            Logger.Log.Info(logMessage);
        }

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

        public static void LogError(Exception ex, Schedule schedule)
        {
            if (schedule != null)
            {
                string message = $@"Schedule Mail #{schedule.Id}
                                <br>\r\nMail Name: {schedule.Name}
                                <br>\r\nMessage: {ex.Message}";

                LogError(message, ex);
            }
            else
            {
                LogError(ex);
            }
        }
    }
}
