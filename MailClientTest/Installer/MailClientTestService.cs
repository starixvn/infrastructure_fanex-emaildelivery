namespace MailClientTest.Installer
{
    using System.ServiceProcess;
    using Fanex.MailDelivery.Client.Handlers;
    using System;
    using Fanex.MailDelivery.Client.Configurations;

    /// <summary>
    /// Mail client service for testing.
    /// </summary>
    public partial class MailClientTestService : ServiceBase
    {
        public MailClientTestService()
        {
        }

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
    }
}
