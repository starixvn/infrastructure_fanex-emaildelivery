using System;
using Fanex.MailDelivery.Core.Messaging.Job;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public interface IJobService : IDisposable
    {
        GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request);
    }
}