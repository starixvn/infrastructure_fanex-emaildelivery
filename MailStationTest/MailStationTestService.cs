using Fanex.Logging;
using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Core.Models;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MailStationTest
{
    public partial class MailStationTestService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            try
            {
                MailClientHandler.StartAsync()
                                .GetAwaiter()
                                .GetResult();

                Console.WriteLine("Mail Client Service Site: {0} started.", AppSettings.MailCientConfig.SiteId);
                Console.WriteLine("App Name: {0}", System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName);

                foreach (ScheduleElement schedule in AppSettings.MailCientConfig.Schedules)
                {
                    Console.WriteLine("Schedule list: {0}", schedule.Id);
                }

                var schedules = MailClientHandler.GetSchedulesAsync()
                                                    .GetAwaiter()
                                                    .GetResult();

                if (schedules == null || schedules.Count == 0)
                {
                    Console.WriteLine("Schedule list is null or empty");
                }
                else
                {
                    Console.WriteLine("Get Schedules successfully.");

                    foreach (var schedule in schedules)
                    {
                        schedule.ConvertToServerTime(MailClientSection.IsLocalEnvironment());

                        Console.WriteLine("Schedule: {0}; Cron: {1}", schedule.Id, schedule.RepeatExpression);
                    }
                }

                SendFunctionMailAsync()
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.InnerException?.Message ?? ex.Message, ex);

                Console.WriteLine(ex.Message);
                var innerException = ex.InnerException;

                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            Console.ReadLine();
        }

        protected override void OnStop()
        {
            MailClientHandler.StopAsync()
                            .GetAwaiter()
                            .GetResult();
        }

        private async Task SendFunctionMailAsync()
        {
            int mailingListId = AppSettings.GetFunctionId("FuntionA");

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
    }
}
