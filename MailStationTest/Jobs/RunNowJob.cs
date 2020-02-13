using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Quartz;

namespace MailStationTest.Jobs
{
    public class RunNowJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
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
