using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;

namespace MailClientTest.Jobs
{
    using System;
    using System.Threading;
    using Quartz;

    /// <summary>
    /// Hello JobRequest.
    /// </summary>
    public class MissingJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Missing JobRequest: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}, ScheduledTime: {3}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now,
                schedule.CurrentFireTime);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Missing JobRequest).";
            mail.Title = "Mail Station Test (Missing JobRequest).";
            mail.Body = "Missing JobRequest";
            Thread.Sleep(5000);

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
