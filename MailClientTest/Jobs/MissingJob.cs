namespace MailClientTest.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Client.Jobs;
    using Quartz;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using System.Threading;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class MissingJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Missing Job: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}, ScheduledTime: {3}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now,
                schedule.CurrentFireTime);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Missing Job).";
            mail.Title = "Mail Station Test (Missing Job).";
            mail.Body = "Missing Job";
            //mail.MailingListId = 2;
            Thread.Sleep(5000);

            return new JobResultMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
