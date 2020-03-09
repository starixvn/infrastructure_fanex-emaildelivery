using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;

namespace MailStationClientTest.Jobs
{
    using Quartz;
    using System;
    using System.Threading;

    /// <summary>
    /// Hello JobRequest.
    /// </summary>
    public class UnstableJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Unstable JobRequest: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;
            
            // Get JobRequest init value here
            ////jobContext.MergedJobDataMap

            // Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Unstable JobRequest).";
            mail.Title = "Mail Station Test (Unstable JobRequest).";
            mail.Body = "Email Body";
            Thread.Sleep(5000);

            ////throw new InvalidCastException("Cannot Complete this JobRequest.");
            if (schedule.CurrentRetryTimes < schedule.MaxRetryTimes)
            {
                throw new InvalidCastException("Cannot Complete this JobRequest.");
            }

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
