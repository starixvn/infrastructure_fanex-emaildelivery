using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Core.Models;

namespace MailClientTest.Installer
{
    using System;
    using System.ServiceProcess;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Handlers;

    /// <summary>
    /// Mail client service for testing.
    /// </summary>
    public partial class MailClientTestService : ServiceBase
    {
        public MailClientTestService()
        {
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) 
        /// or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            MailClientHandler.Start();
            SendFunctionMail();
            Console.WriteLine("Mail Client Service Site: {0} started.", AppSettings.MailCientConfig.SiteId);
            Console.WriteLine("App Name: {0}", System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName);
            try
            {
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

        /// <summary>
        /// Sends the function mail.
        /// </summary>
        private void SendFunctionMail()
        {
            var mail = new Mail
            {
                Subject = "Mail Station Test (Function Mail).",
                Title = "Mail Station Test (Function Mail).",
                Body = "Email Body.",
            };

            // mail.AlternativeViews = new System.Collections.Generic.List<MailLinkedResource>();
            // mail.AlternativeViews.Add(new MailLinkedResource("D:\\image1.jpg", Fanex.MailDelivery.Client.Enumerations.FileExtension.IMG_JPG, "contentId1"));
            mail.MailingListId = AppSettings.GetFunctionId("FuntionA");
            MailClientHandler.SendFunctionMail(mail);
        }
    }
}
