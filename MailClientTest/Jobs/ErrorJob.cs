namespace MailClientTest.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Client.Jobs;
    using Quartz;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using System.Threading;

    /// <summary>
    /// Hello Job.
    /// </summary>
    public class ErrorJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format(
                "Run Error Job: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}",
                schedule.SiteId,
                schedule.Id,
                DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;
            //// Get Job init value here
            //jobContext.MergedJobDataMap

            //// Compose mail here.
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Error Job).";
            mail.Title = "Mail Station Test (Error Job).";
            mail.Body = "Email Body.";
            mail.MailingListId = 2;
            Thread.Sleep(5000);
            throw new InvalidCastException("Cannot Complete this Job.");
        }
    }
}
