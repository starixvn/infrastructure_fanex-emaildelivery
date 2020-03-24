using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;

namespace MailStationClient.AutoJobDemo
{
    public class SendMailJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            // DO NOT wrap your code in try catch block because it's handled in the base class to retry running if there is any error.

            // Generate mail content
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attachment.txt");
            var mail = new Mail
            {
                Subject = "Mail Station Client - Auto job demo",
                Title = "Auto job demo",
                Body = "Auto job Body",
                Attachments = new List<MailAttachment> {
                    new MailAttachment(filePath, FileExtension.TEXT)
                }
            };

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
