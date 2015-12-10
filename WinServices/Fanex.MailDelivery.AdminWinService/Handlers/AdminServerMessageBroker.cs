using System;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.AdminWinService.Logging;
using Fanex.MailDelivery.AdminWinService.MailService;
using Fanex.MailDelivery.AdminWinService.Services;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Services.Admin;

namespace Fanex.MailDelivery.AdminWinService.Handlers
{
    /// <summary>
    /// Mail Service Message Broker.
    /// </summary>
    public class AdminServerMessageBroker : IAdminServerMessageBroker
    {
        private IBus _bus;
        private IMailService _mailService;

        private IAdminServerService _adminServerService;

        public AdminServerMessageBroker()
            : this(null)
        {
        }

        public AdminServerMessageBroker(IMailService mailService)
        {
            _mailService = mailService ?? new MailService.MailService();

            _adminServerService = new AdminServerService(new AdminDataService());
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

            SubscribeAbortJob();
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