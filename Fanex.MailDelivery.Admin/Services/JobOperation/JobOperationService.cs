#region

using System;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;

#endregion

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public class JobOperationService : AdminService, IJobOperationService
    {
        public void RunNow(Schedule schedule)
        {
            var runnowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish(schedule, runnowJobPublishId);
        }

        public void Retry(Schedule schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish(schedule, retryJobPublishId);
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }
    }
}