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
using Fanex.MailDelivery.Server.Services;
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

        private IAdminServerService _adminServerService;

        public MailServiceMessageBroker()
            : this(null)
        {
        }

        public MailServiceMessageBroker(IMailService mailService)
        {
            _mailService = mailService ?? new MailService.MailService();

            _adminService = new AdminService();

            _adminServerService = new AdminServerService(new AdminService());

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
            _bus.Respond<GetMailListRequest, GetMailListResponse>(request => _adminServerService.GetMailList(request));
        }

        private void ResponseGetMailById()
        {
            _bus.Respond<GetMailByIdRequest, GetMailByIdResponse>(request => _adminServerService.GetMailById(request));
        }

        public void RespondCreateMail()
        {
            _bus.Respond<CreateFunctionMailRequest, ResultResponse>(request => _adminServerService.CreateFunctionMail(request));

            _bus.Respond<CreateAutoMailRequest, ResultResponse>(request => _adminServerService.CreateAutoMail(request));
        }

        public void RespondUpdateMail()
        {
            _bus.Respond<UpdateFunctionMailRequest, ResultResponse>(request => _adminServerService.UpdateFunctionMail(request));

            _bus.Respond<UpdateAutoMailRequest, ResultResponse>(request => _adminServerService.UpdateAutoMail(request));
        }

        public void RespondDeleteMail()
        {
            _bus.Respond<DeleteMailRequest, ResultResponse>(request => _adminServerService.DeleteMail(request));
        }

        #endregion

        #region Recipient

        public void RespondGetRecipientList()
        {
            _bus.Respond<GetRecipientListRequest, GetRecipientListResponse>(request => _adminServerService.GetRecipientList(request));
        }

        private void ResponseGetRecipientById()
        {
            _bus.Respond<GetRecipientByIdRequest, GetRecipientByIdResponse>(request => _adminServerService.GetRecipientById(request));
        }

        public void RespondCreateRecipient()
        {
            _bus.Respond<CreateRecipientRequest, ResultResponse>(request => _adminServerService.CreateRecipient(request));
        }

        public void RespondUpdateRecipient()
        {
            _bus.Respond<UpdateRecipientRequest, ResultResponse>(request => _adminServerService.UpdateRecipient(request));
        }

        public void RespondDeleteRecipient()
        {
            _bus.Respond<DeleteRecipientRequest, ResultResponse>(request => _adminServerService.DeleteRecipient(request));
        }

        #endregion

        #region Group

        private void ResponseGetGroupList()
        {
            _bus.Respond<GetGroupListRequest, GetGroupListResponse>(request => _adminServerService.GetGroupList(request));
        }

        private void ResponseGetGroupById()
        {
            _bus.Respond<GetMailGroupByIdRequest, GetMailGroupByIdResponse>(request => _adminServerService.GetGroupById(request));
        }

        private void ResponseCreateGroup()
        {
            _bus.Respond<CreateGroupRequest, ResultResponse>(request => _adminServerService.CreateGroup(request));
        }

        private void ResponseUpdateGroup()
        {
            _bus.Respond<UpdateGroupRequest, ResultResponse>(request => _adminServerService.UpdateGroup(request));
        }

        private void ResponseDeleteGroup()
        {
            _bus.Respond<DeleteGroupRequest, ResultResponse>(request => _adminServerService.DeleteGroup(request));
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