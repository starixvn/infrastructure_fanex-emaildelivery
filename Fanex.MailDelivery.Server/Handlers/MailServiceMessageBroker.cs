using System;
using System.Collections.Generic;
using System.Linq;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Mapping;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Messaging.Recipient;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Server.Logging;
using Fanex.MailDelivery.Server.Services;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria.ManageMail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Handlers.Interface;
using Fanex.MailDelivery.Services.MailService;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.MailDelivery.Services.Messaging.Schedule;
using CreateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.CreateRecipientCriteria;
using DeleteRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.DeleteRecipientCriteria;
using RecipientListCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.RecipientListCriteria;

namespace Fanex.MailDelivery.Server.Handlers
{
    /// <summary>
    /// Mail Service Message Broker.
    /// </summary>
    public class MailServiceMessageBroker : IMessageBroker
    {
        private IBus _bus;
        private IMailService _mailService;

        private IAdminService _adminService;

        public MailServiceMessageBroker()
            : this(null)
        {
        }

        public MailServiceMessageBroker(IMailService mailService)
        {
            _mailService = mailService ?? new MailService();

            _adminService = new AdminService();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            //// Standard practice is to create a single IBus instance for the lifetime of your application. Dispose it when your application closes.
            //// https://github.com/mikehadlow/EasyNetQ/wiki/Connecting-to-RabbitMQ
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailServiceSection.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new RabbitMqLogger());
                });

            SubscribeScheduleRequest();

            SubscribeMail();
            SubscribeJobResult();
            SubscribeAbortJob();
            RespondAddMailTaskForRunningJob();
            SubscribeClientNotificationMessage();
            RespondGetRecipientById();



            RespondGetMailList();
            RespondCreateMail();
            RespondUpdateMail();
            RespondDeleteMail();

            RespondGetRecipientList();
            RespondCreateRecipient();
            RespondUpdateRecipient();
            RespondDeleteRecipient();

            ResponseGetGroupList();
            ResponseCreateGroup();
            ResponseUpdateGroup();
            ResponseDeleteGroup();

            ResponseGetReportList();
        }

        public void SubscribeAbortJob()
        {
            _bus.Subscribe<JobResultMessage>(
                MailDeliveryConstants.AbortJobRoutingKeyFormat,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.AbortJobRoutingKeyFormat));
        }

        /// <summary>
        /// Subscribes the mail.
        /// </summary>
        public void SubscribeMail()
        {
            _bus.Subscribe<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                    mail =>
                    {
                        _mailService.SendMail(mail);
                    },
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        /// <summary>
        /// Subscribes the job result.
        /// </summary>
        public void SubscribeJobResult()
        {
            _bus.Subscribe<JobResultMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
        }

        /// <summary>
        /// Subscribes the job result.
        /// </summary>
        public void SubscribeClientNotificationMessage()
        {
            _bus.Subscribe<ClientInfomationMessage>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        _mailService.UpdateClientActiveTime(clientInfoMessage);
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        /// <summary>
        /// Subscribes the schedule request.
        /// </summary>
        public void SubscribeScheduleRequest()
        {
            _bus.Respond<ScheduleRequest, List<Schedule>>(
                scheduleRequest =>
                {
                    var jobLog = "Subscribe schedule request.";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(jobLog);
                    Console.ForegroundColor = ConsoleColor.White;
                    //  List<Schedule> schedules = _mailService.GetSchedules(scheduleRequest.Ids);

                    return null;
                });
        }

        public void RespondAddMailTaskForRunningJob()
        {
            _bus.Respond<JobResultMessage, JobResultMessage>(
                jobResult =>
                {
                    _mailService.AddMailTaskForRunningJob(jobResult);
                    return jobResult;
                });
        }

        public void RespondGetRecipientById()
        {
            _bus.Respond<GetRecipientByIdMessage, RecipientModel>(
                requestMessage =>
                {
                    return _mailService.RecipientById(new RecipientByIdCriteria()
                    {
                        RecipientId = requestMessage.RecipientId
                    });
                });
        }

        public void Dispose()
        {
            _bus.Dispose();
            GC.SuppressFinalize(this);
        }

        #region Mail

        public void RespondGetMailList()
        {
            _bus.Respond<GetMailListRequest, GetMailListResponse>(
                requestMessage =>
                {
                    List<MailModel> mailModels = _mailService.GetListMail(new GetMailListCriteria
                    {
                        Emails = requestMessage.Emails,
                        IsActive = requestMessage.IsActive,
                        MailGroupId = requestMessage.MailGroupId,
                        MailName = requestMessage.MailName,
                        SiteId = requestMessage.SiteId,
                        TriggerType = requestMessage.TriggerType
                    });

                    List<GetMailListResponseDto> getMailListResponseDtos = mailModels.Select(n => n.ToGetMailListResponseDto()).ToList();

                    GetMailListResponse getMailListResponse = new GetMailListResponse
                    {
                        GetMailListResponseDtos = getMailListResponseDtos
                    };

                    return getMailListResponse;
                });
        }

        public void RespondCreateMail()
        {
            _bus.Respond<CreateFunctionMailRequest, ResultResponse>(
                   requestMessage =>
                   {
                       return _mailService.CreateMail(new CreateMailCriteria()
                       {
                           MailGroupId = requestMessage.MailGroupId,
                           MailName = requestMessage.MailName,
                           SiteId = requestMessage.SiteId,

                           MailFrom = requestMessage.MailFrom,
                           MailTo = requestMessage.MailTo,
                           MailCc = requestMessage.MailCc,
                           MailBcc = requestMessage.MailBcc,

                           IsActive = requestMessage.IsActive,
                           Description = requestMessage.Description,

                           TriggerType = 0 // Function Mail
                       });
                   });

            _bus.Respond<CreateAutoMailRequest, ResultResponse>(
                    requestMessage =>
                    {
                        return _mailService.CreateMail(new CreateMailCriteria()
                        {
                            MailGroupId = requestMessage.MailGroupId,
                            MailName = requestMessage.MailName,
                            SiteId = requestMessage.SiteId,

                            MailFrom = requestMessage.MailFrom,
                            MailTo = requestMessage.MailTo,
                            MailCc = requestMessage.MailCc,
                            MailBcc = requestMessage.MailBcc,

                            IsActive = requestMessage.IsActive,
                            Description = requestMessage.Description,

                            RepeatType = requestMessage.RepeatType,
                            CronExpression = requestMessage.CronExpression,

                            StartOn = requestMessage.StartOn,
                            EndOn = requestMessage.EndOn,

                            Attempt = requestMessage.Attempt,
                            Interval = requestMessage.Interval,

                            TriggerType = 1 // Auto Mail
                        });
                    });

        }

        public void RespondUpdateMail()
        {
            _bus.Respond<UpdateFunctionMailRequest, ResultResponse>(
                requestMessage =>
                {
                    return _mailService.UpdateMail(new UpdateMailCriteria()
                    {
                        MailId = requestMessage.MailId,

                        MailGroupId = requestMessage.MailGroupId,
                        MailName = requestMessage.MailName,
                        SiteId = requestMessage.SiteId,

                        MailFrom = requestMessage.MailFrom,
                        MailTo = requestMessage.MailTo,
                        MailCc = requestMessage.MailCc,
                        MailBcc = requestMessage.MailBcc,

                        IsActive = requestMessage.IsActive,
                        Description = requestMessage.Description,

                        TriggerType = 0 // Function Mail
                    });
                });

            _bus.Respond<UpdateAutoMailRequest, ResultResponse>(
                requestMessage =>
                {
                    return _mailService.UpdateMail(new UpdateMailCriteria()
                    {
                        MailId = requestMessage.MailId,

                        MailGroupId = requestMessage.MailGroupId,
                        MailName = requestMessage.MailName,
                        SiteId = requestMessage.SiteId,

                        MailFrom = requestMessage.MailFrom,
                        MailTo = requestMessage.MailTo,
                        MailCc = requestMessage.MailCc,
                        MailBcc = requestMessage.MailBcc,

                        IsActive = requestMessage.IsActive,
                        Description = requestMessage.Description,

                        RepeatType = requestMessage.RepeatType,
                        CronExpression = requestMessage.CronExpression,

                        StartOn = requestMessage.StartOn,
                        EndOn = requestMessage.EndOn,

                        Attempt = requestMessage.Attempt,
                        Interval = requestMessage.Interval,

                        TriggerType = 1 // Auto Mail
                    });
                });
        }

        public void RespondDeleteMail()
        {
            _bus.Respond<DeleteMailRequest, ResultResponse>(
            requestMessage =>
            {
                return _mailService.DeleteMail(new DeleteMailCriteria
                {
                    MailId = requestMessage.MailId
                });
            });
        }

        #endregion

        #region Recipient

        public void RespondGetRecipientList()
        {
            _bus.Respond<GetRecipientListRequest, GetRecipientListResponse>(
                requestMessage =>
                {
                    IList<RecipientModel> recipientModels = _adminService.GetListRecipient(new RecipientListCriteria
                    {
                        Emails = requestMessage.Emails,
                        Status = requestMessage.Status,
                        UserName = requestMessage.UserName
                    });

                    IList<RecipientDto> recipientDtos = recipientModels.Select(n => n.ToRecipientDto()).ToList();

                    GetRecipientListResponse getMailListResponse = new GetRecipientListResponse
                    {
                        RecipientDtos = recipientDtos
                    };

                    return getMailListResponse;
                });
        }

        public void RespondCreateRecipient()
        {
            _bus.Respond<CreateRecipientRequest, ResultResponse>(
                requestMessage =>
                {
                    return _adminService.CreateRecipient(new CreateRecipientCriteria()
                    {
                        Email1 = requestMessage.Email1,
                        Email2 = requestMessage.Email2,
                        Email3 = requestMessage.Email3,
                        IsNAPUser = requestMessage.IsNapUser,
                        NAPUserID = requestMessage.NapUserId,
                        Note = requestMessage.Note,
                        Status = requestMessage.Status,
                        UserName = requestMessage.UserName
                    });
                });
        }

        public void RespondUpdateRecipient()
        {
            _bus.Respond<UpdateRecipientRequest, ResultResponse>(
                requestMessage =>
                {
                    return _mailService.UpdateRecipient(new UpdateRecipientCriteria()
                    {
                        Email1 = requestMessage.Email1,
                        Email2 = requestMessage.Email2,
                        Email3 = requestMessage.Email3,
                        Note = requestMessage.Note,
                        Status = requestMessage.Status,
                        Username = requestMessage.UserName
                    });
                });
        }

        public void RespondDeleteRecipient()
        {
            _bus.Respond<DeleteRecipientRequest, ResultResponse>(
                requestMessage =>
                {
                    return _adminService.DeleteRecipient(new DeleteRecipientCriteria
                    {
                        RecipientId = requestMessage.RecipientId
                    });
                });
        }

        #endregion

        #region Group

        private void ResponseGetGroupList()
        {
            _bus.Respond<GetGroupListRequest, GetGroupListResponse>(
               requestMessage =>
               {
                   IList<GroupModel> groupModels = _adminService.GetGroupList(new GetGroupListCriteria
                   {
                       MailGroupName = requestMessage.Name
                   });

                   IList<GroupDto> groupDtos = groupModels.Select(n => n.ToGroupDto()).ToList();

                   return new GetGroupListResponse { GroupDtos = groupDtos };
               });
        }

        private void ResponseCreateGroup()
        {
            _bus.Respond<CreateGroupRequest, ResultResponse>(
              request =>
              {
                  return _adminService.CreateGroup(new CreateGroupCriteria
                  {
                      MailGroupName = request.Name,
                      Description = request.Description
                  });
              });
        }

        private void ResponseUpdateGroup()
        {
            _bus.Respond<UpdateGroupRequest, ResultResponse>(
               request =>
               {
                   return _adminService.UpdateGroup(new UpdateGroupCriteria
                   {
                       MailGroupId = request.Id,
                       MailGroupName = request.Name,
                       Description = request.Description
                   });
               });
        }

        private void ResponseDeleteGroup()
        {
            _bus.Respond<DeleteGroupRequest, ResultResponse>(
              request =>
              {
                  return _adminService.DeleteGroup(new DeleteGroupCriteria
                  {
                      MailGroupId = request.Id
                  });
              });
        }

        #endregion

        #region Report

        private void ResponseGetReportList()
        {
            _bus.Respond<GetReportListRequest, GetReportListResponse>(
              request =>
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
              });
        }

        #endregion
    }
}