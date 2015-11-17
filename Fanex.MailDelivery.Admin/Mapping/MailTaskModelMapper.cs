using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Admin.Models;

namespace Fanex.MailDelivery.Admin.Mapping
{
    public static class MailTaskModelMapper
    {
        public static MailTaskDto ToMailTaskDto(this MailTaskModel mailTaskModel)
        {
            return new MailTaskDto
            {
                MailTaskId = mailTaskModel.MailTaskId,
                MailId = mailTaskModel.MailId,
                MailName = mailTaskModel.MailName,
                TriggerType = mailTaskModel.TriggerType,
                SiteId = mailTaskModel.SiteId,
                SiteName = mailTaskModel.SiteName,
                ScheduledTime = mailTaskModel.ScheduledTime,

                LastSent = mailTaskModel.LastSent,
                Status = mailTaskModel.Status,
                Attempt = mailTaskModel.Attempt,

                MSiteMailId = mailTaskModel.MSiteMailId,
                IsUnableToRetry = mailTaskModel.IsUnableToRetry
            };
        }
    }
}
