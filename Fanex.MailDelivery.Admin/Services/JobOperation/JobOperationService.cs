using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public class JobOperationService : AdminService, IJobOperationService
    {
        public void RunNow(Schedule schedule)
        {
            string runnowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish<Schedule>(schedule, runnowJobPublishId);
        }

        public void Retry(Schedule schedule)
        {
            string retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish<Schedule>(schedule, retryJobPublishId);
        }

        public void Abort()
        {
            throw new System.NotImplementedException();
        }
    }
}