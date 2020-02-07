using EasyNetQ;
using EasyNetQ.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStationService.Logging;
using Fanex.MailDelivery.Services.Services.MailService;
using System;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.MessageBrokers
{
    public interface IMessageBroker : IDisposable
    {
        void Start();
    }

    public class MessageBroker : IMessageBroker
    {
        private IBus bus;
        private readonly IMailService mailService;

        public MessageBroker()
            : this(null)
        {
        }

        public MessageBroker(IMailService mailService)
        {
            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());

            this.mailService = mailService ?? new MailService(AppSettings.SendMailRetryLimit, AppSettings.SendMailRetryIntervalInSeconds);
        }

        public void Start()
        {
            bus = RabbitHutch.CreateBus(AppSettings.RabbitMQConnectionString);

            ResponseGetScheduleList();
            RespondGetMailTaskForById();
            RespondAddMailTaskForRunningJob();

            SubscribeMail();
            SubscribeJobResult();
            SubscribeClientNotificationMessage();
        }

        #region Subscribe
        private void SubscribeMail()
        {
            bus.SubscribeAsync<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                    mail => mailService.SendMail(mail),
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        private void SubscribeClientNotificationMessage()
        {
            bus.SubscribeAsync<UpdateActiveTimeRequest>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        mailService.UpdateClientActiveTime(clientInfoMessage);

                        return Task.CompletedTask;
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        private void SubscribeJobResult()
        {
            bus.SubscribeAsync<JobMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus => mailService.UpdateJobStatus(jobStatus),
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
        }
        #endregion

        #region Response
        private void ResponseGetScheduleList()
        {
            bus.RespondAsync<GetScheduleListRequest, GetScheduleListResponse>(request => Task.FromResult(mailService.GetScheduleList(request)));
        }

        private void RespondAddMailTaskForRunningJob()
        {
            bus.RespondAsync<JobMessage, JobMessage>(
                jobMessage =>
                {
                    mailService.AddMailTaskForRunningJob(jobMessage);

                    return Task.FromResult(jobMessage);
                });
        }

        private void RespondGetMailTaskForById()
        {
            bus.RespondAsync<GetMailTaskByIdRequest, MailTaskModel>(request => Task.FromResult(mailService.GetMailTaskById(request)));
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //  dispose managed state (managed objects).
                }

                bus?.Dispose();

                disposedValue = true;
            }
        }

        ~MessageBroker()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
