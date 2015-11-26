using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging.MailManage;

namespace Fanex.MailDelivery.Client.Mapping
{
    public static class MailModelMapper
    {
        public static GetMailListResponseDto ToGetMailListResponseDto(this MailModel mailModel)
        {
            return new GetMailListResponseDto
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
                IsActive = mailModel.IsActive
            };
        }
    }
}
