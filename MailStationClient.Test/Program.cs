using Fanex.Logging;
using Fanex.Logging.Sentry;
using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Core.Models;
using Sentry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailStationClient.NetCoreTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RegisterLogging();

            await StartMailSchedules();
        }

        private static async Task StartMailSchedules()
        {
            try
            {
                await MailClientHandler.StartAsync("appsettings.json");

                Console.WriteLine("Mail Client Service Site: {0} started.", MailClientHandler.MailStationSetting.SiteId);
                Console.WriteLine("App Name: {0}", System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName);

                foreach (var schedule in MailClientHandler.MailStationSetting.Schedules)
                {
                    Console.WriteLine("Schedule list: {0}", schedule.Id);
                }

                var schedules = await MailClientHandler.GetSchedulesAsync();

                if (schedules == null || schedules.Count == 0)
                {
                    Console.WriteLine("Schedule list is null or empty");
                }
                else
                {
                    Console.WriteLine("Get Schedules successfully.");

                    foreach (var schedule in schedules)
                    {
                        schedule.ConvertToServerTime(MailClientHandler.MailStationSetting.IsDevEnvironment());

                        Console.WriteLine("Schedule: {0}; Cron: {1}", schedule.Id, schedule.RepeatExpression);
                    }
                }

                await SendFunctionMailAsync();

                Console.ReadLine();
            }
            finally
            {
                await MailClientHandler.StopAsync();
            }
        }

        private static async Task SendFunctionMailAsync()
        {
            int mailingListId = MailClientHandler.MailStationSetting.GetFunctionId("FuntionA");

            if (mailingListId > 0)
            {
                var mail = new Mail
                {
                    Subject = "Mail Station Test (Function Mail).",
                    Title = "Mail Station Test (Function Mail).",
                    Body = "Email Body.",
                    MailingListId = mailingListId
                };

                await MailClientHandler.SendFunctionMailAsync(mail);

                Console.WriteLine("Function Mail has been sent.");
            }
        }

        private static void RegisterLogging()
        {
            LogManager
                .SetDefaultLogCategory("")
                .Use(new List<ILogEngine>
                {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn("https://3714717bde1f487795ca0bdeb6c6da9f@sentry.nexdev.net/98"),
                        Environment = "DEV"
                     })
                });
        }
    }
}
