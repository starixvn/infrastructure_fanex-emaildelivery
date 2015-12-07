using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.AdminWinService.Mapping;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Services;

namespace Fanex.MailDelivery.AdminWinService.Services
{
    public class AdminServerService : IAdminServerService
    {
        private readonly IMailStationService _mailStationService;

        public AdminServerService(IMailStationService mailStationService)
        {
            _mailStationService = mailStationService;
        }

        public GetMailListResponse GetMailList(GetMailListRequest request)
        {
            IList<MailModel> mailModels = _mailStationService.GetListMail(new GetMailListCriteria
            {
                Emails = request.Emails,
                IsActive = request.IsActive,
                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,
                TriggerType = request.TriggerType
            });

            // List<MailModel> getMailListResponseDtos = mailModels.Select(n => n.ToMailDto()).ToList();
            GetMailListResponse getMailListResponse = new GetMailListResponse
            {
                MailModels = mailModels
            };

            return getMailListResponse;
        }

        public GetMailByIdResponse GetMailById(GetMailByIdRequest request)
        {
            MailModel mailModel = _mailStationService.GetMailById(new GetMailByIdCriteria
            {
                MailId = request.Id
            });

            return new GetMailByIdResponse { MailModel = mailModel == null ? null : mailModel };
        }

        public ResultResponse CreateFunctionMail(CreateFunctionMailRequest request)
        {
            return _mailStationService.CreateMail(new CreateMailCriteria()
            {
                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailFrom = request.MailFrom,
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
            return _mailStationService.CreateMail(new CreateMailCriteria()
            {
                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailFrom = request.MailFrom,
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
            return _mailStationService.UpdateMail(new UpdateMailCriteria()
            {
                MailId = request.MailId,

                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailFrom = request.MailFrom,
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
            return _mailStationService.UpdateMail(new UpdateMailCriteria()
            {
                MailId = request.MailId,

                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,

                MailFrom = request.MailFrom,
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
            return _mailStationService.DeleteMail(new DeleteMailCriteria
            {
                MailId = request.MailId
            }).ToResultResponse();
        }

        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            IList<RecipientModel> recipientModels = _mailStationService.GetListRecipient(new GetRecipientListCriteria
            {
                Emails = request.Emails,
                Status = request.Status,
                UserName = request.UserName
            });

            GetRecipientListResponse getMailListResponse = new GetRecipientListResponse
            {
                RecipientModels = recipientModels
            };

            return getMailListResponse;
        }

        public GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request)
        {
            RecipientModel recipientModel = _mailStationService.GetRecipientById(new GetRecipientByIdCriteria
            {
                RecipientId = request.Id
            });

            return new GetRecipientByIdResponse { RecipientModel = recipientModel };
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return _mailStationService.CreateRecipient(new CreateRecipientCriteria()
            {
                Email1 = request.Email1,
                Email2 = request.Email2,
                Email3 = request.Email3,
                IsNAPUser = request.IsNapUser,
                NAPUserID = request.NapUserId,
                Note = request.Note,
                Status = request.Status,
                UserName = request.UserName
            }).ToResultResponse();
        }

        public ResultResponse UpdateRecipient(UpdateRecipientRequest request)
        {
            return _mailStationService.UpdateRecipient(new UpdateRecipientCriteria()
            {
                RecipientId = request.RecipientId,
                Username = request.UserName,

                EmailID1 = request.EmailId1,
                EmailID2 = request.EmailId2,
                EmailID3 = request.EmailId3,
                Email1 = request.Email1,
                Email2 = request.Email2,
                Email3 = request.Email3,

                Note = request.Note,
                Status = request.Status,
            }).ToResultResponse();
        }

        public ResultResponse DeleteRecipient(DeleteRecipientRequest request)
        {
            return _mailStationService.DeleteRecipient(new DeleteRecipientCriteria
            {
                RecipientId = request.RecipientId
            }).ToResultResponse();
        }

        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            IList<GroupModel> groupModels = _mailStationService.GetGroupList(new GetGroupListCriteria
            {
                MailGroupName = request.Name
            });

            return new GetGroupListResponse { GroupModels = groupModels };
        }

        public GetMailGroupByIdResponse GetGroupById(GetMailGroupByIdRequest request)
        {
            GroupModel groupModel = _mailStationService.GetGroupById(new GetGroupByIdCriteria
            {
                MailGroupId = request.Id
            });

            return new GetMailGroupByIdResponse { GroupModel = groupModel };
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return _mailStationService.CreateGroup(new CreateGroupCriteria
            {
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return _mailStationService.UpdateGroup(new UpdateGroupCriteria
            {
                MailGroupId = request.Id,
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return _mailStationService.DeleteGroup(new DeleteGroupCriteria
            {
                MailGroupId = request.Id
            }).ToResultResponse();
        }

        public GetReportListResponse GetReportList(GetReportListRequest request)
        {
            IList<MailTaskModel> mailTaskModels = _mailStationService.GetReportList(new GetReportListCriteria
            {
                MailName = request.MailName,
                SiteId = request.SiteId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status
            });

            return new GetReportListResponse { MailTaskModels = mailTaskModels };
        }

        public GetScheduleByMailIdResponse GetScheduleByMailId(GetScheduleByMailIdRequest request)
        {
            Schedule schedule = _mailStationService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
            {
                MailId = request.MailId
            });

            return new GetScheduleByMailIdResponse { Schedule = schedule };
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            IList<Schedule> scheduleDtos = new List<Schedule>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = _mailStationService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                scheduleDtos.Add(schedule);
            }

            return new GetScheduleListResponse { Schedules = scheduleDtos };
        }
    }
}