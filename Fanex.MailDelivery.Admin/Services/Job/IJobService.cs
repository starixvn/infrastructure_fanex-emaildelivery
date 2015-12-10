using System;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core.Messaging.Schedule;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public interface IJobService : IDisposable
    {
        void AbortJob(AbortJobRequest request);

        void RetryJobMessage(ScheduleRequest schedule);

        void PublishScheduleChangeMessage(ScheduleRequest schedule);

        GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request);
    }
}