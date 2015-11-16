using Fanex.MailDelivery.Core.Enumerations;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace MailClientTest.Jobs
{
    using System;
    using System.Threading;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class NormalJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Normal Job but not send mail: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Normal Job).";
            mail.Title = "Mail Station Test (Normal Job).";
            mail.Body = "Email Body";
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
