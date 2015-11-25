using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Server.Mapping
{
    public static class ScheduleModelMapper
    {
        public static ScheduleDto ToScheduleDto(this Schedule scheduleModel)
        {
            return new ScheduleDto
            {
                ScheduleId = scheduleModel.Id,
                SiteId = scheduleModel.SiteId,
                MailName = scheduleModel.Name,
                TriggerType = scheduleModel.TriggerType,
                CronExpression = scheduleModel.RepeatExpression,
                StartOn = scheduleModel.StartTime,
                EndOn = scheduleModel.EndTime,
                IsActive = scheduleModel.Active,
                Attempt = scheduleModel.MaxRetryTimes,
                Interval = scheduleModel.RetryInterval
            };
        }
    }
}
