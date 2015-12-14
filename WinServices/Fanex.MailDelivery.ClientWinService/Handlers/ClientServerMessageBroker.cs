using System;
using EasyNetQ;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.ClientWinService.Logging;
using Fanex.MailDelivery.ClientWinService.Services;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Services.Client;
using Fanex.MailDelivery.Services.Services.MailService;

namespace Fanex.MailDelivery.ClientWinService.Handlers
{
    public class ClientServerMessageBroker : IClientServerMessageBroker
    {
        private IBus _bus;

        private IClientServerService _clientServerService;

        public ClientServerMessageBroker()
            : this(null)
        {
        }

        public ClientServerMessageBroker(IClientServerService adminServerService)
        {
            _clientServerService = _clientServerService ?? new ClientServerService(new ClientDataService(), new MailService());
        }

        public void Start()
        {
            //// Standard practice is to create a single IBus instance for the lifetime of your application. Dispose it when your application closes.
            //// https://github.com/mikehadlow/EasyNetQ/wiki/Connecting-to-RabbitMQ
            // TODO => Singleton
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailCientConfig.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new ServerClientLogger());
                });

            ResponseGetScheduleList();

            SubscribeMail();
            SubscribeJobResult();

            RespondAddMailTaskForRunningJob();

            SubscribeClientNotificationMessage();
        }

        public void SubscribeMail()
        {
            _bus.Subscribe<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                    mail =>
                    {
                        _clientServerService.SendMail(mail);
                    },
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        private void ResponseGetScheduleList()
        {
            _bus.Respond<GetScheduleListRequest, GetScheduleListResponse>(request => _clientServerService.GetScheduleList(request));
        }

        public void SubscribeClientNotificationMessage()
        {
            _bus.Subscribe<UpdateActiveTimeRequest>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        _clientServerService.UpdateClientActiveTime(clientInfoMessage);
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        public void RespondAddMailTaskForRunningJob()
        {
            _bus.Respond<JobMessage, JobMessage>(
                jobMessage =>
                {
                    _clientServerService.AddMailTaskForRunningJob(jobMessage);
                    return jobMessage;
                });
        }

        public void SubscribeJobResult()
        {
            _bus.Subscribe<JobMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus =>
                    {
                        _clientServerService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
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
