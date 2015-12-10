using System;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public class JobOperationService : AdminService, IJobOperationService, IDisposable
    {
        public void PublishScheduleChange(Schedule schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);
            Bus.Publish(schedule, updateSchedulePublishId);
        }

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

        public void Abort(JobMessage jobMessage)
        {
            throw new NotImplementedException();
        }
    }
}