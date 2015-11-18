using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public class JobService : AdminService, IJobService
    {
        public void AbortJob(AbortJobRequest request)
        {
            JobResultMessage jobResultMessage = new JobResultMessage() { JobStatus = Core.Enumerations.JobStatus.Abort, Schedule = new Schedule() { TaskId = request.TaskId } };
            Bus.Publish<JobResultMessage>(jobResultMessage, MailDeliveryConstants.AbortJobRoutingKeyFormat);
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