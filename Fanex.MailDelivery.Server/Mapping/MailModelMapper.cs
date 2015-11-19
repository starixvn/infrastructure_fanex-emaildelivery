using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Services.Models;

namespace Fanex.MailDelivery.Server.Mapping
{
    public static class MailModelMapper
    {
        public static MailDto ToMailDto(this MailModel mailModel)
        {
            return new MailDto
            {
                MailId = mailModel.MailId,
                MailName = mailModel.MailName,

                MailGroupId = mailModel.MailGroupId,
                MailGroupName = mailModel.MailGroupName,

                SiteId = mailModel.SiteId,
                SiteName = mailModel.SiteName,

                TriggerType = mailModel.TriggerType,

                MailTo = mailModel.MailTo,
                MailCc = mailModel.MailCc,
                MailBcc = mailModel.MailBcc,

                LastSent = mailModel.LastSent,
                IsActive = mailModel.IsActive,

                // Auto mail
                RepeatType = mailModel.RepeatType,
                CronExpression = mailModel.CronExpression,
                MailFrom = mailModel.MailFrom,
                StartOn = mailModel.StartOn,
                EndOn = mailModel.EndOn,
                Attempt = mailModel.Attempt,
                Interval = mailModel.Interval,
                Description = mailModel.Description,
                LastActive = mailModel.LastActive
            };
        }
    }
}
