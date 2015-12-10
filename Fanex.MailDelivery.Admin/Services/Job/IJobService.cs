using System;
using Fanex.MailDelivery.Core.Messaging.Job;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public interface IJobService : IDisposable
    {
        void AbortJob(AbortJobRequest request);

        void RetryJobMessage(ScheduleRequest schedule);

        GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request);
    }
}