using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Core.Models;
using System;
using System.ServiceProcess;
using System.Threading;

namespace MailClientTest.Installer
{
    public partial class MailClientTestService : ServiceBase
    {
        private int counter;
        private int maxRetryStartUpService = 5;
        private int maxIntervalStartUpServiceInSecond = 60;

        protected override void OnStart(string[] args)
        {
            try
            {
                bool isStarted = false;

                while (!isStarted)
                {
                    isStarted = Execute();

                    if (counter >= maxRetryStartUpService)
                    {
                        throw new Exception("The service can't be started, maybe it's related to RabbitMQ or something else.");
                    }

                    Thread.Sleep(1000 * maxIntervalStartUpServiceInSecond);
                }

                Console.WriteLine("Mail Client Service Site: {0} started.", AppSettings.MailCientConfig.SiteId);
                Console.WriteLine("App Name: {0}", System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName);
                var schedules = AppSettings.MailCientConfig.Schedules;
                foreach (ScheduleElement schedule in schedules)
                {
                    Console.WriteLine("Schedule list: {0}", schedule.Id);
                }

                var listschedule = MailClientHandler.GetSchedules();
                if (listschedule == null || listschedule.Count == 0)
                {
                    Console.WriteLine("List Schedule is null or empty");
                }
                else
                {
                    Console.WriteLine("Get Schedules success.");
                    foreach (var schedule in listschedule)
                    {
                        Console.WriteLine("Schedule: {0}; Cron: {1}", schedule.Id, schedule.RepeatExpression);
                        schedule.ConvertToServerTime(MailClientSection.GetConfig().IsLocalEnvironment());
                    }
                }

                SendFunctionMail();
            }
            catch (Exception ex)
            {
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
            MailClientHandler.Stop();
        }

        private void SendFunctionMail()
        {
            var mail = new Mail
            {
                Subject = "Mail Station Test (Function Mail).",
                Title = "Mail Station Test (Function Mail).",
                Body = "Email Body.",
            };

            mail.MailingListId = AppSettings.GetFunctionId("FuntionA");
            MailClientHandler.SendFunctionMail(mail);
            Console.WriteLine("Function Mail has been sent.");
        }

        private bool Execute()
        {
            var result = false;

            try
            {
                counter++;
                MailClientHandler.Start();
                result = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Service starts fail at {DateTime.Now} - {exception.Message}");
            }

            return result;
        }
    }
}
