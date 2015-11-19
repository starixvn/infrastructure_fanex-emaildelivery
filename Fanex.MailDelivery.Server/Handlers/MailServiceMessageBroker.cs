using System;
using System.Collections.Generic;
using System.Linq;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Logging;
using Fanex.MailDelivery.Server.MailService;
using Fanex.MailDelivery.Server.Mapping;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;
using Fanex.MailDelivery.Services.Services;

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
            _mailService = mailService ?? new MailService.MailService();

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
            ResponseGetMailById();
            RespondCreateMail();
            RespondUpdateMail();
            RespondDeleteMail();

            RespondGetRecipientList();
            ResponseGetRecipientById();
            RespondCreateRecipient();
            RespondUpdateRecipient();
            RespondDeleteRecipient();

            ResponseGetGroupList();
            ResponseGetGroupById();
            ResponseCreateGroup();
            ResponseUpdateGroup();
            ResponseDeleteGroup();

            ResponseGetReportList();
        }

        public void SubscribeAbortJob()
        {
            _bus.Subscribe<Job>(
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
            _bus.Subscribe<Job>(
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
            _bus.Respond<Job, Job>(
                jobResult =>
                {
                    _mailService.AddMailTaskForRunningJob(jobResult);
                    return jobResult;
                });
        }

        public void RespondGetRecipientById()
        {
            _bus.Respond<GetRecipientByIdRequest, RecipientModel>(
                request =>
                {
                    return _adminService.GetRecipientById(new GetRecipientByIdCriteria()
                    {
                        RecipientId = request.Id
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
                    IList<MailModel> mailModels = _adminService.GetListMail(new GetMailListCriteria
                    {
                        Emails = requestMessage.Emails,
                        IsActive = requestMessage.IsActive,
                        MailGroupId = requestMessage.MailGroupId,
                        MailName = requestMessage.MailName,
                        SiteId = requestMessage.SiteId,
                        TriggerType = requestMessage.TriggerType
                    });

                    List<MailDto> getMailListResponseDtos = mailModels.Select(n => n.ToMailDto()).ToList();

                    GetMailListResponse getMailListResponse = new GetMailListResponse
                    {
                        GetMailListResponseDtos = getMailListResponseDtos
                    };

                    return getMailListResponse;
                });
        }

        private void ResponseGetMailById()
        {
            _bus.Respond<GetMailByIdRequest, GetMailByIdResponse>(
                request =>
                {
                    MailModel mailModel = _adminService.GetMailById(new GetMailByIdCriteria
                    {
                        MailId = request.Id
                    });

                    return new GetMailByIdResponse { MailDto = mailModel == null ? null : mailModel.ToMailDto() };
                });
        }

        public void RespondCreateMail()
        {
            _bus.Respond<CreateFunctionMailRequest, ResultResponse>(
                   requestMessage =>
                   {
                       return _adminService.CreateMail(new CreateMailCriteria()
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
                       }).ToResultResponse();
                   });

            _bus.Respond<CreateAutoMailRequest, ResultResponse>(
                    requestMessage =>
                    {
                        return _adminService.CreateMail(new CreateMailCriteria()
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
                        }).ToResultResponse();
                    });

        }

        public void RespondUpdateMail()
        {
            _bus.Respond<UpdateFunctionMailRequest, ResultResponse>(
                requestMessage =>
                {
                    return _adminService.UpdateMail(new UpdateMailCriteria()
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
                    }).ToResultResponse();
                });

            _bus.Respond<UpdateAutoMailRequest, ResultResponse>(
                requestMessage =>
                {
                    return _adminService.UpdateMail(new UpdateMailCriteria()
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
                    }).ToResultResponse();
                });
        }

        public void RespondDeleteMail()
        {
            _bus.Respond<DeleteMailRequest, ResultResponse>(
            requestMessage =>
            {
                return _adminService.DeleteMail(new DeleteMailCriteria
                {
                    MailId = requestMessage.MailId
                }).ToResultResponse();
            });
        }

        #endregion

        #region Recipient

        public void RespondGetRecipientList()
        {
            _bus.Respond<GetRecipientListRequest, GetRecipientListResponse>(
                requestMessage =>
                {
                    IList<RecipientModel> recipientModels = _adminService.GetListRecipient(new GetRecipientListCriteria
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

        private void ResponseGetRecipientById()
        {
            _bus.Respond<GetRecipientByIdRequest, GetRecipientByIdResponse>(
               request =>
               {
                   RecipientModel recipientModel = _adminService.GetRecipientById(new GetRecipientByIdCriteria
                   {
                       RecipientId = request.Id
                   });

                   return new GetRecipientByIdResponse { RecipientDto = recipientModel.ToRecipientDto() };
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
                    }).ToResultResponse();
                });
        }

        public void RespondUpdateRecipient()
        {
            _bus.Respond<UpdateRecipientRequest, ResultResponse>(
                requestMessage =>
                {
                    return _adminService.UpdateRecipient(new UpdateRecipientCriteria()
                    {
                        RecipientId = requestMessage.RecipientId,
                        Username = requestMessage.UserName,

                        EmailID1 = requestMessage.EmailId1,
                        EmailID2 = requestMessage.EmailId2,
                        EmailID3 = requestMessage.EmailId3,
                        Email1 = requestMessage.Email1,
                        Email2 = requestMessage.Email2,
                        Email3 = requestMessage.Email3,

                        Note = requestMessage.Note,
                        Status = requestMessage.Status,
                    }).ToResultResponse();
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
                    }).ToResultResponse();
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

        private void ResponseGetGroupById()
        {
            _bus.Respond<GetMailGroupByIdRequest, GetMailGroupByIdResponse>(
               request =>
               {
                   GroupModel groupModel = _adminService.GetGroupById(new GetGroupByIdCriteria
                   {
                       MailGroupId = request.Id
                   });

                   return new GetMailGroupByIdResponse { GroupDto = groupModel.ToGroupDto() };
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
                  }).ToResultResponse();
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
                   }).ToResultResponse();
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
                  }).ToResultResponse();
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