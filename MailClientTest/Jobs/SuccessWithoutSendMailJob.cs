using Fanex.MailDelivery.Core.Enumerations;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace MailClientTest.Jobs
{
    using System;
    using System.Threading;
    using Fanex.MailDelivery.Client.Jobs;
    using Quartz;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class SuccessWithoutSendMailJob : JobBase
    {
        protected override JobRequest Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Success Without Send Mail Job: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Success Without Send Mail Job).";
            mail.Title = "Mail Station Test (Success Without Send Mail Job).";
            mail.Body = "Email Body";
            Thread.Sleep(5000);

            return new JobRequest
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.SuccessWithoutSendmail
            };
        }
    }
}
