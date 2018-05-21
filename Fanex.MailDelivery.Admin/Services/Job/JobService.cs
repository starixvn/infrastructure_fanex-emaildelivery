using System;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public class JobService : AdminService, IJobService
    {
        ~JobService()
        {
            Dispose(false);
        }

        public GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request)
        {
            return Bus.Request<GetScheduleByMailIdRequest, GetScheduleByMailIdResponse>(request);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }
    }
}