using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;
using System.Threading;

namespace MailStationClient.AutoJobDemo
{
    public class ErrorJob : JobBase
    {
        private const int WaitingTime = 5000;

        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var mail = new Mail();
            mail.Subject = "Mail Station Test (Error JobRequest).";
            mail.Title = "Mail Station Test (Error JobRequest).";
            mail.Body = "Email Body.";

            Thread.Sleep(WaitingTime);

            throw new InvalidCastException("Cannot Complete this JobRequest.");
        }
    }
}
