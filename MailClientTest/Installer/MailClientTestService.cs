namespace MailClientTest.Installer
{
    using System;
    using System.ServiceProcess;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;

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
            Console.WriteLine("Mail Client Service Site: {0} started.", AppSettings.MailCientConfig.SiteId);
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
                        schedule.ConvertToServerTime();
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
        /// Gets the mail list.
        /// </summary>
        /// <param name="mailID">The mail identifier.</param>
        private void GetMaillingList(int mailID)
        {
            var mail = MailClientHandler.GetMailingList(mailID);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("MailID: " + mail.Id.ToString());
            Console.WriteLine("Mail Type: " + mail.MailType.ToString());
            Console.WriteLine("Mail Name: " + mail.Name.ToString());
            Console.WriteLine("Mail Group: " + mail.Group.ToString());
            Console.WriteLine("Is Active: " + mail.Active.ToString());
            Console.WriteLine("Site id: " + mail.SiteId.ToString());
            Console.WriteLine("Mail to: " + mail.TOs.ToString());
            Console.WriteLine("Mail CC: " + mail.CCs.ToString());
            Console.WriteLine("Mail BCC: " + mail.BCCs.ToString());
            Console.ForegroundColor = ConsoleColor.White;
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

            mail.AlternativeViews = new System.Collections.Generic.List<MailLinkedResource>();
            mail.AlternativeViews.Add(new MailLinkedResource("D:\\image1.jpg", Fanex.MailDelivery.Client.Enumerations.FileExtension.IMG_JPG, "contentId1"));
            mail.MailingListId = AppSettings.GetFunctionId("FuntionA");
            MailClientHandler.SendMail(mail);
        }
    }
}
