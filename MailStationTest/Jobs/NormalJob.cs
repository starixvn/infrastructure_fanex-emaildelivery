using Fanex.MailDelivery.Core.Models;

namespace MailStationClientTest.Jobs
{
    using Fanex.MailDelivery.Client.Jobs;
    using Quartz;
    using System;
    using System.Threading;

    /// <summary>
    /// Hello JobRequest.
    /// </summary>
    public class NormalJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Normal JobRequest: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Normal JobRequest).";
            mail.Title = "Mail Station Test (Normal JobRequest).";
            mail.Body = "Email Body";
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
