using EasyNetQ;
using EasyNetQ.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.MailStationService.Logging;
using Fanex.MailDelivery.MailStationService.Services;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Services.Client;
using Fanex.MailDelivery.Services.Services.MailService;
using System;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.MessageBrokers
{
    public interface IClientServerMessageBroker : IDisposable
    {
        void Start();

        void RespondAddMailTaskForRunningJob();

        void RespondGetMailTaskForById();

        void SubscribeClientNotificationMessage();

        void SubscribeJobResult();

        void SubscribeMail();
    }

    public class ClientServerMessageBroker : IClientServerMessageBroker
    {
        private IBus bus;
        private readonly IClientServerService clientServerService;

        public ClientServerMessageBroker()
            : this(null)
        {
        }

        public ClientServerMessageBroker(IClientServerService adminServerService)
        {
            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());
            clientServerService = clientServerService ?? new ClientServerService(new ClientDataService(), new MailService());
        }

        public void Start()
        {
            bus = RabbitHutch.CreateBus(AppSettings.MailServiceSection.RabbitMQConnectionString);

            ResponseGetScheduleList();

            SubscribeMail();
            SubscribeJobResult();

            RespondAddMailTaskForRunningJob();
            RespondGetMailTaskForById();
            SubscribeClientNotificationMessage();
        }

        public void SubscribeMail()
        {
            bus.SubscribeAsync<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                    mail =>
                    {
                        clientServerService.SendMail(mail);

                        return Task.CompletedTask;
                    },
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        private void ResponseGetScheduleList()
        {
            bus.RespondAsync<GetScheduleListRequest, GetScheduleListResponse>(request => Task.FromResult(clientServerService.GetScheduleList(request)));
        }

        public void SubscribeClientNotificationMessage()
        {
            bus.SubscribeAsync<UpdateActiveTimeRequest>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        clientServerService.UpdateClientActiveTime(clientInfoMessage);

                        return Task.CompletedTask;
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        public void RespondAddMailTaskForRunningJob()
        {
            bus.RespondAsync<JobMessage, JobMessage>(
                jobMessage =>
                {
                    clientServerService.AddMailTaskForRunningJob(jobMessage);

                    return Task.FromResult(jobMessage);
                });
        }

        public void RespondGetMailTaskForById()
        {
            bus.Respond<GetMailTaskByIdRequest, MailTaskModel>(request => { return clientServerService.GetMailTaskById(request); });
        }

        public void SubscribeJobResult()
        {
            bus.Subscribe<JobMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus =>
                    {
                        clientServerService.UpdateJobStatus(jobStatus);
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
            if (bus != null)
            {
                bus.Dispose();
            }
        }
    }
}
