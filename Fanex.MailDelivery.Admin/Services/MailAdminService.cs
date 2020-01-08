using Fanex.MailDelivery.Admin.Mapping;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Services.Criteria.Mail;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IMailAdminService
    {
        GetMailListResponse GetMailList(GetMailListRequest request);

        GetMailByIdResponse GetMailById(GetMailByIdRequest request);

        ResultResponse CreateFunctionMail(CreateFunctionMailRequest request);

        ResultResponse CreateAutoMail(CreateAutoMailRequest request);

        ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request);

        ResultResponse UpdateAutoMail(UpdateAutoMailRequest request);

        ResultResponse DeleteMail(DeleteMailRequest request);
    }

    public class MailAdminService : BaseAdminService, IMailAdminService
    {
        public GetMailListResponse GetMailList(GetMailListRequest request)
        {
            IList<MailModel> mailModels = adminDataService.GetListMail(new GetMailListCriteria
            {
                Emails = request.Emails,
                IsActive = request.Status,
                MailGroupId = request.GroupIds,
                MailName = request.MailName,
                SiteId = request.SiteId,
                TriggerType = request.TriggerType
            });

            GetMailListResponse getMailListResponse = new GetMailListResponse
            {
                MailModels = mailModels
            };

            return getMailListResponse;
        }

        public GetMailByIdResponse GetMailById(GetMailByIdRequest request)
        {
            MailModel mailModel = adminDataService.GetMailById(new GetMailByIdCriteria
            {
                MailId = request.Id
            });

            return new GetMailByIdResponse { MailModel = mailModel };
        }

        public ResultResponse CreateFunctionMail(CreateFunctionMailRequest request)
        {
            return adminDataService.CreateMail(new CreateMailCriteria()
            {
                MailGroupId = request.GroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailTo = request.MailTo,
                MailCc = request.MailCc,
                MailBcc = request.MailBcc,

                IsActive = request.IsActive,
                Description = request.Description,

                TriggerType = 0 // Function Mail
            }).ToResultResponse();
        }

        public ResultResponse CreateAutoMail(CreateAutoMailRequest request)
        {
            return adminDataService.CreateMail(new CreateMailCriteria()
            {
                MailGroupId = request.GroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailTo = request.MailTo,
                MailCc = request.MailCc,
                MailBcc = request.MailBcc,

                IsActive = request.IsActive,
                Description = request.Description,

                RepeatType = request.RepeatType,
                CronExpression = request.CronExpression,

                StartOn = request.StartOn,
                EndOn = request.EndOn,

                Attempt = request.Attempt,
                Interval = request.Interval,

                TriggerType = 1 // Auto Mail
            }).ToResultResponse();
        }

        public ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request)
        {
            return adminDataService.UpdateMail(new UpdateMailCriteria()
            {
                MailId = request.MailId,

                MailGroupId = request.GroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailTo = request.MailTo,
                MailCc = request.MailCc,
                MailBcc = request.MailBcc,

                IsActive = request.IsActive,
                Description = request.Description,

                TriggerType = 0 // Function Mail
            }).ToResultResponse();
        }

        public ResultResponse UpdateAutoMail(UpdateAutoMailRequest request)
        {
            return adminDataService.UpdateMail(new UpdateMailCriteria()
            {
                MailId = request.MailId,

                MailGroupId = request.GroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailTo = request.MailTo,
                MailCc = request.MailCc,
                MailBcc = request.MailBcc,

                IsActive = request.IsActive,
                Description = request.Description,

                RepeatType = request.RepeatType,
                CronExpression = request.CronExpression,

                StartOn = request.StartOn,
                EndOn = request.EndOn,

                Attempt = request.Attempt,
                Interval = request.Interval,

                TriggerType = 1 // Auto Mail
            }).ToResultResponse();
        }

        public ResultResponse DeleteMail(DeleteMailRequest request)
        {
            return adminDataService.DeleteMail(new DeleteMailCriteria
            {
                MailId = request.MailId
            }).ToResultResponse();
        }
    }
}