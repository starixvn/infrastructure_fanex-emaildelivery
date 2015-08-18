namespace MailClientTest.Jobs
{
    using System;
    using System.Threading;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class DataNotReadyJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Data Not Ready Job : \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Data Not Ready Job).";
            mail.Title = "Mail Station Test (Data Not Ready Job).";
            mail.Body = "Email Body";

            // Do something ...
            Thread.Sleep(5000);

            return new JobResultMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.DataIsNotReady
            };
        }
    }
}
