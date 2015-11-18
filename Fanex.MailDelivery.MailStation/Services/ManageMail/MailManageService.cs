using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Admin;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.Schedule;
using Fanex.MailDelivery.Admin.Services.Mail;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    /// <summary>
    /// Mail Manage Service.
    /// </summary>
    public class MailManageService : MailStationBaseService
    {
        IMailService _mailService;

        public MailManageService(IMailService mailService)
        {
            _mailService = mailService;
        }

        public List<MailDto> MailList(GetMailListCriteria criteria)
        {
            GetMailListRequest request = new GetMailListRequest
            {
                MailName = criteria.MailName,
                IsActive = criteria.IsActive,
                MailGroupId = criteria.MailGroupId,
                TriggerType = criteria.TriggerType,
                Emails = criteria.Emails,
                SiteId = criteria.SiteId
            };

            GetMailListResponse response = _mailService.GetMailList(request);

            return response.GetMailListResponseDtos;
        }

        public MailDto MailById(GetMailByIdCriteria criteria)
        {
            GetMailByIdRequest request = new GetMailByIdRequest
            {
                Id = criteria.MailId
            };

            GetMailByIdResponse response = _mailService.GetMailById(request);

            return response.MailDto;
        }

        public CommandResult CreateMail(CreateMailCriteria criteria)
        {
            if (criteria.TriggerType == 0)
            {
                return CreateFunctionMail(criteria);
            }

            return CreateAutoMail(criteria);
        }

        private CommandResult CreateFunctionMail(CreateMailCriteria criteria)
        {
            CreateFunctionMailRequest request = new CreateFunctionMailRequest
            {
                MailGroupId = criteria.MailGroupId,
                MailName = criteria.MailName,
                SiteId = criteria.SiteId,

                // TODO: Remove?
                MailFrom = criteria.MailFrom,

                MailTo = criteria.MailTo,
                MailCc = criteria.MailCc,
                MailBcc = criteria.MailBcc,

                Description = criteria.Description,
                IsActive = criteria.IsActive,
            };

            ResultResponse response = _mailService.CreateFunctionMail(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return commandResult;
        }

        private CommandResult CreateAutoMail(CreateMailCriteria criteria)
        {
            CreateAutoMailRequest request = new CreateAutoMailRequest
            {
                MailGroupId = criteria.MailGroupId,
                MailName = criteria.MailName,
                SiteId = criteria.SiteId,

                // TODO: Remove?
                MailFrom = criteria.MailFrom,

                MailTo = criteria.MailTo,
                MailCc = criteria.MailCc,
                MailBcc = criteria.MailBcc,

                Description = criteria.Description,
                IsActive = criteria.IsActive,

                RepeatType = criteria.RepeatType,
                CronExpression = criteria.CronExpression,
                StartOn = criteria.StartOn,
                EndOn = criteria.EndOn,
                Attempt = criteria.Attempt,
                Interval = criteria.Attempt
            };

            ResultResponse response = _mailService.CreateAutoMail(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return commandResult;
        }

        public CommandResult UpdateMail(UpdateMailCriteria criteria)
        {
            MailAdminService.Start();
            MailAdminService.PublishScheduleChangeMessage(new ScheduleRequest
            {
                Id = criteria.MailId,
                Active = criteria.IsActive,
                EndTime = criteria.EndOn,
                StartTime = criteria.StartOn ?? DateTime.Now,
                MaxRetryTimes = criteria.Attempt,
                Name = criteria.MailName,
                RepeatExpression = criteria.CronExpression,
                RetryInterval = criteria.Interval,
                SiteId = criteria.SiteId
            });

            MailAdminService.Stop();

            if (criteria.TriggerType == 0)
            {
                return UpdateFunctionMail(criteria);
            }

            return UpdateAutoMail(criteria);
        }

        private CommandResult UpdateFunctionMail(UpdateMailCriteria criteria)
        {
            UpdateFunctionMailRequest request = new UpdateFunctionMailRequest
            {
                MailId = criteria.MailId,
                MailName = criteria.MailName,
                MailGroupId = criteria.MailGroupId,
                SiteId = criteria.SiteId,
                MailTo = criteria.MailTo,
                MailCc = criteria.MailCc,
                MailBcc = criteria.MailBcc,
                // TODO: remove?
                MailFrom = criteria.MailFrom,
                Description = criteria.Description,
                IsActive = criteria.IsActive
            };

            ResultResponse response = _mailService.UpdateFunctionMail(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return commandResult;
        }

        private CommandResult UpdateAutoMail(UpdateMailCriteria criteria)
        {
            UpdateAutoMailRequest request = new UpdateAutoMailRequest
            {
                MailId = criteria.MailId,
                MailName = criteria.MailName,
                MailGroupId = criteria.MailGroupId,
                SiteId = criteria.SiteId,
                MailTo = criteria.MailTo,
                MailCc = criteria.MailCc,
                MailBcc = criteria.MailBcc,
                // TODO: remove?
                MailFrom = criteria.MailFrom,
                Description = criteria.Description,
                IsActive = criteria.IsActive,

                RepeatType = criteria.RepeatType,
                CronExpression = criteria.CronExpression,
                StartOn = criteria.StartOn,
                EndOn = criteria.EndOn,
                Attempt = criteria.Attempt,
                Interval = criteria.Interval
            };

            ResultResponse response = _mailService.UpdateAutoMail(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return commandResult;
        }

        public CommandResult DeleteMail(DeleteMailCriteria criteria)
        {
            DeleteMailRequest request = new DeleteMailRequest
            {
                MailId = criteria.MailId
            };

            ResultResponse response = _mailService.DeleteMail(request);

            CommandResult commandResult = new CommandResult
            {
                Code = response.ErrCode,
                Message = response.ErrMsg
            };

            return commandResult;
        }



        public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public void RunJobImmediately(int mailId)
        {
            MailAdminService.Start();
            //TODO: Will be changed
            //Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailId });
            //MailAdminService.PublishRetryJobMessage(schedule);
            MailAdminService.Stop();
        }

        public void Dispose()
        {
            _mailService.Dispose();
        }
    }
}
