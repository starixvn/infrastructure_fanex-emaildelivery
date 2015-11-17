using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Server.MailService
{
    /// <summary>
    /// Base service for base logging.
    /// </summary>
    public class ServiceBase
    {
        public static void LogError(Exception ex)
        {
            string message = ex.Message +
                "<br>\r\nSOURCE: " + ex.Source +
                "<br>\r\nTARGETSITE: " + ex.TargetSite +
                "<br>\r\nSTACKTRACE: " + ex.StackTrace;
          //  Logger.Log.Error(message);
        }

        public static void LogError(Exception ex, Schedule schedule)
        {
            //// Logger.Log.Error(
            ////    "MailId: " +
            ////    schedule.Id +
            ////    Environment.NewLine +
            ////    "Mail Name: " +
            ////    (schedule.Name ?? string.Empty) +
            ////    Environment.NewLine +
            ////    ex.Message,
            ////    ex);

            string message =
                "MailId: " + schedule.Id +
                "<br>\r\nMail Name: " + (schedule.Name ?? string.Empty) +
                "<br>\r\nMessage: " + ex.Message +
                "<br>\r\nSOURCE: " + ex.Source +
                "<br>\r\nTARGETSITE: " + ex.TargetSite +
                "<br>\r\nSTACKTRACE: " + ex.StackTrace;
         //   Logger.Log.Error(message);
        }

        public static void LogError(Exception ex, int id)
        {
            ////Logger.Log.Error(
            ////    "Function MailId: " +
            ////    id +
            ////    Environment.NewLine +
            ////    ex.Message,
            ////    ex);

            string message =
               "Function MailId: " + id +
               "<br>\r\nMessage: " + ex.Message +
               "<br>\r\nSOURCE: " + ex.Source +
               "<br>\r\nTARGETSITE: " + ex.TargetSite +
               "<br>\r\nSTACKTRACE: " + ex.StackTrace;
         //   Logger.Log.Error(message);
        }

        public static void LogError(Exception ex, List<int> scheduleIds)
        {
            ////Logger.Log.Error(
            ////    "MailId: " +
            ////    string.Join(", ", scheduleIds) +
            ////    Environment.NewLine +
            ////    ex.Message,
            ////    ex);

            string message =
               "MailId: " + string.Join(", ", scheduleIds) +
               "<br>\r\nMessage: " + ex.Message +
               "<br>\r\nSOURCE: " + ex.Source +
               "<br>\r\nTARGETSITE: " + ex.TargetSite +
               "<br>\r\nSTACKTRACE: " + ex.StackTrace;
         //   Logger.Log.Error(message);
        }

        public static void LogInfo(string messsage)
        {
          //  Logger.Log.Info(messsage);
        }
    }
}
