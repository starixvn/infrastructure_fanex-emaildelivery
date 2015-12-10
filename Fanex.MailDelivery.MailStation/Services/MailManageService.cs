using System;
using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Services.Job;
using Fanex.MailDelivery.Admin.Services.Mail;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Services
{
    public class MailManageService : MailStationBaseService
    {
        private readonly IMailService _mailService;

        private readonly IJobService _jobService;

        public MailManageService(IMailService mailService, IJobService jobService)
        {
            _mailService = mailService;
            _jobService = jobService;
        }

        public IList<MailModel> MailList(GetMailListCriteria criteria)
        {
            GetMailListRequest request = new GetMailListRequest
            {
                MailName = criteria.MailName,
                Status = criteria.IsActive,
                GroupIds = criteria.MailGroupId,
                TriggerType = criteria.TriggerType,
                Emails = criteria.Emails,
                SiteId = criteria.SiteId
            };

            GetMailListResponse response = _mailService.GetMailList(request);

            return response.MailModels;
        }

        public MailModel MailById(GetMailByIdCriteria criteria)
        {
            GetMailByIdRequest request = new GetMailByIdRequest
            {
                Id = criteria.MailId
            };

            GetMailByIdResponse response = _mailService.GetMailById(request);

            return response.MailModel;
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
                GroupId = criteria.MailGroupId,
                MailName = criteria.MailName,
                SiteId = criteria.SiteId,

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
                GroupId = criteria.MailGroupId,
                MailName = criteria.MailName,
                SiteId = criteria.SiteId,

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
            // TODO: No need publish for Function Mail ?
            _jobService.PublishScheduleChange(new ScheduleChangeRequest
            {
                Schedule = new Schedule
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
                }
            });

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
                GroupId = criteria.MailGroupId,
                SiteId = criteria.SiteId,
                MailTo = criteria.MailTo,
                MailCc = criteria.MailCc,
                MailBcc = criteria.MailBcc,

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
                GroupId = criteria.MailGroupId,
                SiteId = criteria.SiteId,
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
            // TODO: Will be changed
            // Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailId });
            // MailAdminService.PublishRetryJobMessage(schedule);
        }

        public void Dispose()
        {
            _mailService.Dispose();
            _jobService.Dispose();
        }
    }
}
