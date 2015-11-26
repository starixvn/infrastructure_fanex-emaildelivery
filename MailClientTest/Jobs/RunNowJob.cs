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
    public class RunNowJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "RunNow JobRequest: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);

            // Compose mail here.
            var mail = new Mail();
            mail.Subject = "RunNow Test";
            mail.Title = "Title";
            mail.Body = "Email Body";

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
