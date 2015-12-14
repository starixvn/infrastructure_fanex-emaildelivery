using System;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public interface IJobOperationService : IDisposable
    {
        void PublishScheduleChange(Schedule request);

        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);

        void AbortJob(AbortJobRequest request);
    }
}