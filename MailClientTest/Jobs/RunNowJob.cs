using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;

namespace MailClientTest.Jobs
{
    using System;
    using System.Threading;
    using Quartz;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class RunNowJob : JobBase
    {
        protected override JobRequest Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Unstable Job: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;
            
            // Get Job init value here
            ////jobContext.MergedJobDataMap

            // Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Unstable Job).";
            mail.Title = "Mail Station Test (Unstable Job).";
            mail.Body = "Email Body";
            Thread.Sleep(5000);

            ////throw new InvalidCastException("Cannot Complete this Job.");
            if (schedule.CurrentRetryTimes < schedule.MaxRetryTimes)
            {
                throw new InvalidCastException("Cannot Complete this Job.");
            }

            return new JobRequest
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
