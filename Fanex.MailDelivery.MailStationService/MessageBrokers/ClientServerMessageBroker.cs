using EasyNetQ;
using EasyNetQ.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.MailStationService.Logging;
using Fanex.MailDelivery.Services.Services.MailService;
using System;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.MessageBrokers
{
    public interface IClientServerMessageBroker : IDisposable
    {
        void Start();
    }

    public class ClientServerMessageBroker : IClientServerMessageBroker
    {
        private IBus bus;
        private readonly IMailService mailService;

        public ClientServerMessageBroker()
            : this(null)
        {
        }

        public ClientServerMessageBroker(IMailService mailService)
        {
            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());
            this.mailService = mailService ?? new MailService(AppSettings.SendMailRetryLimit);
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
                    mail =>
                    {
                        mailService.SendMail(mail);

                        return Task.CompletedTask;
                    },
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
                    jobStatus =>
                    {
                        mailService.UpdateJobStatus(jobStatus);

                        return Task.CompletedTask;
                    },
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
            bus.Respond<GetMailTaskByIdRequest, MailTaskModel>(request => { return mailService.GetMailTaskById(request); });
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

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

        ~ClientServerMessageBroker()
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
