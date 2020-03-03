using EasyNetQ;
using EasyNetQ.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStationService.Logging;
using Fanex.MailDelivery.Services.Services.Logging;
using Fanex.MailDelivery.Services.Services.MailService;
using System;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.MessageBrokers
{
    public interface IMessageBroker : IDisposable
    {
        Task StartAsync();
    }

    public class MessageBroker : IMessageBroker
    {
        private IBus bus;
        private readonly IMailService mailService;
        private const int waitingForRabbitMQInSeconds = 30;
        private readonly LogService logService;

        public MessageBroker(LogService logService)
            : this(null, logService)
        {
        }

        public MessageBroker(IMailService mailService, LogService logService)
        {
            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());

            this.logService = logService;
            this.mailService = mailService ?? new MailService(AppSettings.SendMailRetryLimit, AppSettings.SendMailRetryIntervalInSeconds);
        }

        public async Task StartAsync()
        {
            bus = RabbitHutch.CreateBus(AppSettings.RabbitMQConnectionString);

            while (!bus.IsConnected)
            {
                await Task.Delay(TimeSpan.FromSeconds(waitingForRabbitMQInSeconds));
            }

            await SubscribeMailAsync();
            await SubscribeJobResultAsync();
            await SubscribeClientNotificationMessageAsync();

            ResponseGetScheduleList();
            RespondGetMailTaskForById();
            RespondAddMailTaskForRunningJob();
        }

        #region Subscribe
        private async Task SubscribeMailAsync()
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<Mail>(
                    MailDeliveryConstants.SendMailRoutingKey,
                    mail => mailService.SendMail(mail),
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));

                logService.LogInfo($"Subcribed the queue with key {MailDeliveryConstants.SendMailRoutingKey} successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while subscribing the queue with key {MailDeliveryConstants.SendMailRoutingKey}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                await SubscribeMailAsync();
            }
        }

        private async Task SubscribeClientNotificationMessageAsync()
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<UpdateActiveTimeRequest>(
                    MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        mailService.UpdateClientActiveTime(clientInfoMessage);

                        return Task.CompletedTask;
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));

                logService.LogInfo($"Subcribed the queue with key {MailDeliveryConstants.ClientNotificationTopic} successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while subscribing the queue with key {MailDeliveryConstants.ClientNotificationTopic}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                await SubscribeClientNotificationMessageAsync();
            }
        }

        private async Task SubscribeJobResultAsync()
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<JobMessage>(
                    MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus => mailService.UpdateJobStatus(jobStatus),
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));

                logService.LogInfo($"Subcribed the queue with key {MailDeliveryConstants.JobResultRoutingKey} successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while subscribing the queue with key {MailDeliveryConstants.JobResultRoutingKey}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                await SubscribeJobResultAsync();
            }
        }
        #endregion

        #region Response
        private void ResponseGetScheduleList()
        {
            bool isFailed = false;

            try
            {
                bus.RespondAsync<GetScheduleListRequest, GetScheduleListResponse>(request => Task.FromResult(mailService.GetScheduleList(request)));

                logService.LogInfo($"Registered to response schedules to clients successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while registering to response schedules to clients. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                ResponseGetScheduleList();
            }
        }

        private void RespondAddMailTaskForRunningJob()
        {
            bool isFailed = false;

            try
            {
                bus.RespondAsync<JobMessage, JobMessage>(
                    jobMessage =>
                    {
                        mailService.AddMailTaskForRunningJob(jobMessage);

                        return Task.FromResult(jobMessage);
                    });

                logService.LogInfo($"Registered to add mail tasks for running jobs successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while registering to add mail tasks for running jobs. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                RespondAddMailTaskForRunningJob();
            }
        }

        private void RespondGetMailTaskForById()
        {
            bool isFailed = false;

            try
            {
                bus.RespondAsync<GetMailTaskByIdRequest, MailTaskModel>(request => Task.FromResult(mailService.GetMailTaskById(request)));

                logService.LogInfo($"Registered to response mail tasks to clients successfully");
            }
            catch (Exception ex)
            {
                logService.LogError($"There is an error while registering to response mail tasks to clients. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed)
            {
                RespondGetMailTaskForById();
            }
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
