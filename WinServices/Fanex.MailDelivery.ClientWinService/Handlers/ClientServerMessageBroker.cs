using System;
using EasyNetQ;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.ClientWinService.Logging;
using Fanex.MailDelivery.ClientWinService.Services;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Services.Services;
using Fanex.MailDelivery.Services.Services.Admin;
using Fanex.MailDelivery.Services.Services.Client;

namespace Fanex.MailDelivery.ClientWinService.Handlers
{
    public class ClientServerMessageBroker : IClientServerMessageBroker
    {
        private IBus _bus;

        private IClientServerService _adminServerService;

        public ClientServerMessageBroker()
            : this(null)
        {
        }

        public ClientServerMessageBroker(IClientServerService adminServerService)
        {
            _adminServerService = _adminServerService ?? new ClientServerService(new ClientDataService());
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
            SubscribeAbortJob();

            RespondAddMailTaskForRunningJob();

            SubscribeClientNotificationMessage();
        }

        private void ResponseGetScheduleList()
        {
            _bus.Respond<GetScheduleListRequest, GetScheduleListResponse>(request => _adminServerService.GetScheduleList(request));
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
                        _adminServerService.UpdateClientActiveTime(clientInfoMessage);
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        public void SubscribeAbortJob()
        {
            //_bus.Subscribe<JobMessage>(
            //    MailDeliveryConstants.AbortJobRoutingKeyFormat,
            //        jobStatus =>
            //        {
            //            _mailService.UpdateJobStatus(jobStatus);
            //        },
            //        config => config.WithTopic(MailDeliveryConstants.AbortJobRoutingKeyFormat));
        }



        public void SubscribeMail()
        {
            //_bus.Subscribe<Mail>(
            //    MailDeliveryConstants.SendMailRoutingKey,
            //        mail =>
            //        {
            //            _mailService.SendMail(mail);
            //        },
            //        config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        public void SubscribeJobResult()
        {
            //_bus.Subscribe<JobMessage>(
            //    MailDeliveryConstants.JobResultRoutingKey,
            //        jobStatus =>
            //        {
            //            _mailService.UpdateJobStatus(jobStatus);
            //        },
            //        config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
        }

        public void RespondAddMailTaskForRunningJob()
        {
            //_bus.Respond<JobMessage, JobMessage>(
            //    jobMessage =>
            //    {
            //        _mailService.AddMailTaskForRunningJob(jobMessage);
            //        return jobMessage;
            //    });
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
