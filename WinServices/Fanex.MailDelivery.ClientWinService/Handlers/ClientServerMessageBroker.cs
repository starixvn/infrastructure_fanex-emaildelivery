using System;
using EasyNetQ;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.ClientWinService.Logging;

namespace Fanex.MailDelivery.ClientWinService.Handlers
{
    public class ClientServerMessageBroker : IClientServerMessageBroker
    {
        private IBus _bus;

        public ClientServerMessageBroker()
        {
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
            //_bus.Respond<GetScheduleListRequest, GetScheduleListResponse>(request => _adminServerService.GetScheduleList(request));
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

        /// <summary>
        /// Subscribes client notification message
        /// </summary>
        public void SubscribeClientNotificationMessage()
        {
            //_bus.Subscribe<UpdateActiveTimeRequest>(
            //    MailDeliveryConstants.ClientNotificationTopic,
            //        clientInfoMessage =>
            //        {
            //            _mailService.UpdateClientActiveTime(clientInfoMessage);
            //        },
            //        config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
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
