using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailStationClient.NetCoreDemo
{
    public class NormalJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobLog = string.Format("Run Normal JobRequest: \n\tSite Id: {0}, Schedule: {1}. \n\tSent at {2}", schedule.SiteId, schedule.Id, DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(jobLog);
            Console.ForegroundColor = ConsoleColor.White;

            //// Compose mail here.
            var mail = new Mail
            {
                Subject = "Mail Station Test (Normal JobRequest).",
                Title = "Mail Station Test (Normal JobRequest).",
                Body = "Email Body"
            };

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
