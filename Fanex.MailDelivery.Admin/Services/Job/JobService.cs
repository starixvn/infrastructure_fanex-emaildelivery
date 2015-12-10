using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.Job
{
    public class JobService : AdminService, IJobService
    {
        public void AbortJob(AbortJobRequest request)
        {
            var jobMessage = new JobMessage
            {
                JobStatus = JobStatus.Abort,
                Schedule = new Schedule { TaskId = request.TaskId }
            };
            Bus.Publish(jobMessage, MailDeliveryConstants.AbortJobRoutingKeyFormat);
        }

        public void RetryJobMessage(ScheduleRequest schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            Bus.Publish(schedule, retryJobPublishId);
        }

        public void PublishScheduleChangeMessage(ScheduleRequest schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);
            Bus.Publish(schedule, updateSchedulePublishId);
        }

        public GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request)
        {
            return Bus.Request<GetScheduleByMailIdRequest, GetScheduleByMailIdResponse>(request);
        }
    }
}