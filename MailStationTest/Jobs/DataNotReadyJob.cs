using Fanex.MailDelivery.Core.Models;

namespace MailStationTest.Jobs
{
    using Fanex.MailDelivery.Client.Jobs;
    using Quartz;
    using System;
    using System.Threading;

    /// <summary>
    /// Hello JobRequest.
    /// </summary>
    public class DataNotReadyJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Data Not Ready JobRequest : \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Data Not Ready JobRequest).";
            mail.Title = "Mail Station Test (Data Not Ready JobRequest).";
            mail.Body = "Email Body";

            // Do something ...
            Thread.Sleep(5000);

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.DataIsNotReady
            };
        }
    }
}
