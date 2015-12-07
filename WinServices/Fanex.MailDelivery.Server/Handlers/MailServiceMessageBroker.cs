using System;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.AdminWinService.Logging;
using Fanex.MailDelivery.AdminWinService.MailService;
using Fanex.MailDelivery.AdminWinService.Services;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Services;

namespace Fanex.MailDelivery.AdminWinService.Handlers
{
    /// <summary>
    /// Mail Service Message Broker.
    /// </summary>
    public class MailServiceMessageBroker : IMessageBroker
    {
        private IBus _bus;
        private IMailService _mailService;

        private IAdminServerService _adminServerService;

        public MailServiceMessageBroker()
            : this(null)
        {
        }

        public MailServiceMessageBroker(IMailService mailService)
        {
            _mailService = mailService ?? new MailService.MailService();

            _adminServerService = new AdminServerService(new AdminService());
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            //// Standard practice is to create a single IBus instance for the lifetime of your application. Dispose it when your application closes.
            //// https://github.com/mikehadlow/EasyNetQ/wiki/Connecting-to-RabbitMQ
            // TODO => Singleton
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailServiceSection.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new RabbitMqLogger());
                });

            // Register admin functions
            ResponseMailFunctions();
            ResponseRecipientFunctions();
            ResponseGroupFunctions();
            ResponseReportFunctions();
            ResponseJobFunctions();

            ResponseGetScheduleList();

            SubscribeMail();
            SubscribeJobResult();
            SubscribeAbortJob();

            RespondAddMailTaskForRunningJob();

            SubscribeClientNotificationMessage();
        }

        private void ResponseRecipientFunctions()
        {
            _bus.Respond<GetRecipientListRequest, GetRecipientListResponse>(request => _adminServerService.GetRecipientList(request));
            _bus.Respond<GetRecipientByIdRequest, GetRecipientByIdResponse>(request => _adminServerService.GetRecipientById(request));

            _bus.Respond<CreateRecipientRequest, ResultResponse>(request => _adminServerService.CreateRecipient(request));
            _bus.Respond<UpdateRecipientRequest, ResultResponse>(request => _adminServerService.UpdateRecipient(request));
            _bus.Respond<DeleteRecipientRequest, ResultResponse>(request => _adminServerService.DeleteRecipient(request));
        }

        private void ResponseMailFunctions()
        {
            _bus.Respond<GetMailListRequest, GetMailListResponse>(request => _adminServerService.GetMailList(request));

            _bus.Respond<GetMailByIdRequest, GetMailByIdResponse>(request => _adminServerService.GetMailById(request));
            _bus.Respond<CreateFunctionMailRequest, ResultResponse>(request => _adminServerService.CreateFunctionMail(request));

            _bus.Respond<CreateAutoMailRequest, ResultResponse>(request => _adminServerService.CreateAutoMail(request));
            _bus.Respond<UpdateFunctionMailRequest, ResultResponse>(request => _adminServerService.UpdateFunctionMail(request));

            _bus.Respond<UpdateAutoMailRequest, ResultResponse>(request => _adminServerService.UpdateAutoMail(request));
            _bus.Respond<DeleteMailRequest, ResultResponse>(request => _adminServerService.DeleteMail(request));
        }

        private void ResponseGroupFunctions()
        {
            _bus.Respond<GetGroupListRequest, GetGroupListResponse>(request => _adminServerService.GetGroupList(request));
            _bus.Respond<GetMailGroupByIdRequest, GetMailGroupByIdResponse>(request => _adminServerService.GetGroupById(request));

            _bus.Respond<CreateGroupRequest, ResultResponse>(request => _adminServerService.CreateGroup(request));
            _bus.Respond<UpdateGroupRequest, ResultResponse>(request => _adminServerService.UpdateGroup(request));
            _bus.Respond<DeleteGroupRequest, ResultResponse>(request => _adminServerService.DeleteGroup(request));
        }

        private void ResponseReportFunctions()
        {
            _bus.Respond<GetReportListRequest, GetReportListResponse>(request => _adminServerService.GetReportList(request));
        }

        private void ResponseJobFunctions()
        {
            _bus.Respond<GetScheduleByMailIdRequest, GetScheduleByMailIdResponse>(request => _adminServerService.GetScheduleByMailId(request));
        }

        private void ResponseGetScheduleList()
        {
            _bus.Respond<GetScheduleListRequest, GetScheduleListResponse>(request => _adminServerService.GetScheduleList(request));
        }

        public void SubscribeAbortJob()
        {
            _bus.Subscribe<JobMessage>(
                MailDeliveryConstants.AbortJobRoutingKeyFormat,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.AbortJobRoutingKeyFormat));
        }

        /// <summary>
        /// Subscribes client notification message
        /// </summary>
        public void SubscribeClientNotificationMessage()
        {
            _bus.Subscribe<UpdateActiveTimeRequest>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        _mailService.UpdateClientActiveTime(clientInfoMessage);
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

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

        public void SubscribeJobResult()
        {
            _bus.Subscribe<JobMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
        }

        public void RespondAddMailTaskForRunningJob()
        {
            _bus.Respond<JobMessage, JobMessage>(
                jobMessage =>
                {
                    _mailService.AddMailTaskForRunningJob(jobMessage);
                    return jobMessage;
                });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
            }

            // Free native resources if there are any.
            _bus.Dispose();
        }
    }
}