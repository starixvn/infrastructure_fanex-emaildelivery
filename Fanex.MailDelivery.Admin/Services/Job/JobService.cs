using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public class JobService : AdminService, IJobService
    {
        public void AbortJob(AbortJobRequest request)
        {
            Core.Models.Job job = new Core.Models.Job() { JobStatus = JobStatus.Abort, Schedule = new Schedule() { TaskId = request.TaskId } };
            Bus.Publish<Core.Models.Job>(job, MailDeliveryConstants.AbortJobRoutingKeyFormat);
        }

        public void RetryJobMessage(ScheduleRequest schedule)
        {
            string retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish<ScheduleRequest>(schedule, retryJobPublishId);
        }

        public void PublishScheduleChangeMessage(ScheduleRequest schedule)
        {
            string updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);
            Bus.Publish<ScheduleRequest>(schedule, updateSchedulePublishId);
        }
    }
}