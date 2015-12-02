using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Server.Mapping;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;
using Fanex.MailDelivery.Services.Services;

namespace Fanex.MailDelivery.Server.Services
{
    public class AdminServerService : IAdminServerService
    {
        private readonly IAdminService _adminService;

        public AdminServerService(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public GetMailListResponse GetMailList(GetMailListRequest request)
        {
            IList<MailModel> mailModels = _adminService.GetListMail(new GetMailListCriteria
            {
                Emails = request.Emails,
                IsActive = request.IsActive,
                MailGroupId = request.MailGroupId,
                MailName = request.MailName,
                SiteId = request.SiteId,
                TriggerType = request.TriggerType
            });

            //List<MailModel> getMailListResponseDtos = mailModels.Select(n => n.ToMailDto()).ToList();

            GetMailListResponse getMailListResponse = new GetMailListResponse
            {
                MailModels = mailModels
            };

            return getMailListResponse;
        }

        public GetMailByIdResponse GetMailById(GetMailByIdRequest request)
        {
            MailModel mailModel = _adminService.GetMailById(new GetMailByIdCriteria
            {
                MailId = request.Id
            });

            return new GetMailByIdResponse { MailModel = mailModel == null ? null : mailModel };
        }

        public ResultResponse CreateFunctionMail(CreateFunctionMailRequest request)
        {
            return _adminService.CreateMail(new CreateMailCriteria()
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
            return _adminService.CreateMail(new CreateMailCriteria()
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
            return _adminService.UpdateMail(new UpdateMailCriteria()
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
            return _adminService.UpdateMail(new UpdateMailCriteria()
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
            return _adminService.DeleteMail(new DeleteMailCriteria
            {
                MailId = request.MailId
            }).ToResultResponse();
        }

        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            IList<RecipientModel> recipientModels = _adminService.GetListRecipient(new GetRecipientListCriteria
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
            RecipientModel recipientModel = _adminService.GetRecipientById(new GetRecipientByIdCriteria
            {
                RecipientId = request.Id
            });

            return new GetRecipientByIdResponse { RecipientModel = recipientModel };
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return _adminService.CreateRecipient(new CreateRecipientCriteria()
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
            return _adminService.UpdateRecipient(new UpdateRecipientCriteria()
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
            return _adminService.DeleteRecipient(new DeleteRecipientCriteria
            {
                RecipientId = request.RecipientId
            }).ToResultResponse();
        }

        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            IList<GroupModel> groupModels = _adminService.GetGroupList(new GetGroupListCriteria
            {
                MailGroupName = request.Name
            });

            IList<GroupDto> groupDtos = groupModels.Select(n => n.ToGroupDto()).ToList();

            return new GetGroupListResponse { GroupDtos = groupDtos };
        }

        public GetMailGroupByIdResponse GetGroupById(GetMailGroupByIdRequest request)
        {
            GroupModel groupModel = _adminService.GetGroupById(new GetGroupByIdCriteria
            {
                MailGroupId = request.Id
            });

            return new GetMailGroupByIdResponse { GroupDto = groupModel.ToGroupDto() };
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return _adminService.CreateGroup(new CreateGroupCriteria
            {
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return _adminService.UpdateGroup(new UpdateGroupCriteria
            {
                MailGroupId = request.Id,
                MailGroupName = request.Name,
                Description = request.Description
            }).ToResultResponse();
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return _adminService.DeleteGroup(new DeleteGroupCriteria
            {
                MailGroupId = request.Id
            }).ToResultResponse();
        }

        public GetReportListResponse GetReportList(GetReportListRequest request)
        {
            IList<MailTaskModel> mailTaskModels = _adminService.GetReportList(new GetReportListCriteria
            {
                MailName = request.MailName,
                SiteId = request.SiteId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status
            });

            IList<MailTaskDto> mailTaskDtos = mailTaskModels.Select(n => n.ToMailTaskDto()).ToList();

            return new GetReportListResponse { MailTaskDtos = mailTaskDtos };
        }

        public GetScheduleByMailIdResponse GetScheduleByMailId(GetScheduleByMailIdRequest request)
        {
            Schedule schedule = _adminService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
            {
                MailId = request.MailId
            });

            return new GetScheduleByMailIdResponse { ScheduleDto = schedule.ToScheduleDto() };
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            IList<ScheduleDto> scheduleDtos = new List<ScheduleDto>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = _adminService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                scheduleDtos.Add(schedule.ToScheduleDto());
            }

            return new GetScheduleListResponse { ScheduleDtos = scheduleDtos };
        }
    }
}