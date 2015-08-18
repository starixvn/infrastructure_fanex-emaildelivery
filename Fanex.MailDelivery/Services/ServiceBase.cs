namespace Fanex.MailDelivery.Services
{
    using System;
    using Fanex.Logging;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Base service for base logging.
    /// </summary>
    public class ServiceBase
    {
        public static void LogError(Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }

        public static void LogError(Exception ex, Schedule schedule)
        {
            Logger.Log.Error(
                "MailId: " +
                schedule.Id +
                Environment.NewLine +
                "Mail Name: " +
                (schedule.Name ?? string.Empty) +
                Environment.NewLine +
                ex.Message,
                ex);
        }

        public static void LogError(Exception ex, int id)
        {
            Logger.Log.Error(
                "Function MailId: " +
                id +
                Environment.NewLine +
                ex.Message,
                ex);
        }

        public static void LogInfo(string messsage)
        {
            Logger.Log.Info(messsage);
        }
    }
}
