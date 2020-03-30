using EasyNetQ;
using EasyNetQ.Logging;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Handlers
{
    public interface IMailClientMessageBroker
    {
        IJobHandler JobHandler { get; }

        Task StartAsync();

        Task StopAsync();

        Task SendFunctionMailAsync(Mail mail);

        Task<IList<Schedule>> GetSchedulesAsync();

        Task<UMInfoModel> GetUMInfoAsync(int retryCount = 0);

        Task PublishClientNotifyMessageAsync();

        Task ReportJobAsync(JobMessage jobMessage);

        Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobMessage, int retryCount = 0);

        Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest request, int retryCount = 0);
    }

    public class MailClientMessageBroker : IMailClientMessageBroker
    {
        private readonly string updateScheduleSubscriptionId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private readonly string retryJobSubscriptionId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private readonly string runNowJobSubscriptionId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private readonly IBus bus;
        private readonly IJobHandler jobHandler;
        private readonly int waitingForRabbitMQInSeconds = AppSettings.MailCientConfig.RequestRetryIntervalInSeconds;

        public MailClientMessageBroker() : this(null)
        {
        }

        public MailClientMessageBroker(IJobHandler jobHandler)
        {
            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());

            this.jobHandler = jobHandler ?? new JobHandler();
            bus = RabbitHutch.CreateBus(AppSettings.MailCientConfig.RabbitMQConnectionString);
        }

        public IJobHandler JobHandler => jobHandler;

        public async Task StartAsync()
        {
            await jobHandler.InitializeSchedulerAsync();

            while (!bus.IsConnected)
            {
                await Task.Delay(TimeSpan.FromSeconds(waitingForRabbitMQInSeconds));
            }

            if (AppSettings.MailCientConfig.HasSchedule())
            {
                await SubscribeScheduleChangeAsync();
                await SubscribeRunNowJobAsync();
                await SubscribeRetryJobAsync();
            }

            await jobHandler.StartAsync();
        }

        public async Task StopAsync()
        {
            bus.Dispose();

            await jobHandler.StopAsync();
        }

        #region PublishAsync
        public async Task SendFunctionMailAsync(Mail mail)
        {
            if (mail.ApplicationSendMailId == 0)
            {
                mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            mail.SiteId = AppSettings.MailCientConfig.SiteId;

            await PublishMailAsync(mail);
        }

        public async Task PublishClientNotifyMessageAsync()
        {
            ScheduleElementCollection schedules = AppSettings.MailCientConfig.Schedules;
            string mailIds = string.Join(",", schedules.OfType<ScheduleElement>().Select(x => x.Id));

            await bus.PublishAsync(new UpdateActiveTimeRequest { MailIds = mailIds }, MailDeliveryConstants.ClientNotificationTopic);
        }

        public async Task ReportJobAsync(JobMessage jobMessage)
        {
            if (jobMessage.Mail != null && jobMessage.Mail.ApplicationSendMailId == 0)
            {
                jobMessage.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            await PublishJobMessageAsync(jobMessage);
        }

        private async Task PublishMailAsync(Mail mail, int retryCount = 0)
        {
            bool isFailed = false;

            try
            {
                await bus.PublishAsync(mail, MailDeliveryConstants.SendMailRoutingKey);
            }
            catch (Exception ex)
            {
                LogService.LogError($"There is an error while sending a function mail to Mail Station server. Retrying in {waitingForRabbitMQInSeconds} seconds...", $"{mail.MailingListId}", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                await Task.Delay(TimeSpan.FromSeconds(waitingForRabbitMQInSeconds));

                await PublishMailAsync(mail, retryCount);
            }
        }

        private async Task PublishJobMessageAsync(JobMessage jobMessage, int retryCount = 0)
        {
            bool isFailed = false;

            try
            {
                await bus.PublishAsync(jobMessage, MailDeliveryConstants.JobResultRoutingKey);
            }
            catch (Exception ex)
            {
                LogService.LogError($@"There is an error while reporting the status of the scheduled mail to Mail Station server. Retrying in {
                                        waitingForRabbitMQInSeconds
                                        } seconds...", jobMessage.Schedule, ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                await Task.Delay(TimeSpan.FromSeconds(waitingForRabbitMQInSeconds));

                await PublishJobMessageAsync(jobMessage, retryCount);
            }
        }

        #endregion

        #region RequestAsync
        public async Task<IList<Schedule>> GetSchedulesAsync()
        {
            ScheduleElementCollection schedules = AppSettings.MailCientConfig.Schedules;
            var request = new GetScheduleListRequest();

            foreach (ScheduleElement scheduleConfig in schedules)
            {
                request.MailIds.Add(scheduleConfig.Id);
            }

            GetScheduleListResponse response = await RequestMailSchedules(request);

            return response.Schedules;
        }

        public async Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobMessage, int retryCount = 0)
        {
            JobMessage result = null;
            bool isFailed = false;

            try
            {
                result = await bus.RequestAsync<JobMessage, JobMessage>(jobMessage);
            }
            catch (Exception ex)
            {
                LogService.LogError($"There is an error while sending a request to add a running mail task. Retrying...", jobMessage.Schedule, ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                result = await RequestAddMailTaskForRunningJobAsync(jobMessage, retryCount);
            }

            return result;
        }

        public async Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest request, int retryCount = 0)
        {
            MailTaskModel result = null;
            bool isFailed = false;

            try
            {
                result = await bus.RequestAsync<GetMailTaskByIdRequest, MailTaskModel>(request);
            }
            catch (Exception ex)
            {
                LogService.LogError($"Client cannot get the mail task #{request.TaskId} from Mail Station server. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                result = await RequestMailTaskByIdAsync(request, retryCount);
            }

            return result;
        }

        public async Task<UMInfoModel> GetUMInfoAsync(int retryCount = 0)
        {
            UMInfoModel result = null;
            bool isFailed = false;

            try
            {
                result = await bus.RequestAsync<UMInfoModel, UMInfoModel>(new UMInfoModel());
            }
            catch (Exception ex)
            {
                LogService.LogError($"Client cannot get the UM info from Mail Station server. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                result = await GetUMInfoAsync(retryCount);
            }

            return result;
        }

        private async Task<GetScheduleListResponse> RequestMailSchedules(GetScheduleListRequest request, int retryCount = 0)
        {
            GetScheduleListResponse response = null;
            bool isFailed = false;

            try
            {
                response = await bus.RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(request);
            }
            catch (Exception ex)
            {
                LogService.LogError($"Client cannot get schedules from Mail Station server. Retrying...", string.Join(", ", request.MailIds), ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                response = await RequestMailSchedules(request, retryCount);
            }

            return response;
        }
        #endregion

        #region Subscribe
        public async Task SubscribeScheduleChangeAsync(int retryCount = 0)
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<Schedule>(
                    GetSubscriptionIdKey(updateScheduleSubscriptionId),
                    schedule => jobHandler.UpdateScheduleAsync(schedule),
                    config => config.WithTopic(updateScheduleSubscriptionId));

                LogService.LogInfo($"Subcribed the queue with key {updateScheduleSubscriptionId} successfully");
            }
            catch (Exception ex)
            {
                LogService.LogError($"There is an error while subscribing a queue with key {updateScheduleSubscriptionId}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                await SubscribeScheduleChangeAsync(retryCount);
            }
        }

        public async Task SubscribeRunNowJobAsync(int retryCount = 0)
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<Schedule>(
                    GetSubscriptionIdKey(runNowJobSubscriptionId),
                    schedule => jobHandler.RetryAllProcessAsync(schedule),
                    config => config.WithTopic(runNowJobSubscriptionId));

                LogService.LogInfo($"Subcribed the queue with key {runNowJobSubscriptionId} successfully");
            }
            catch (Exception ex)
            {
                LogService.LogError($"There is an error while subscribing a queue with key {runNowJobSubscriptionId}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                await SubscribeRunNowJobAsync(retryCount);
            }
        }

        public async Task SubscribeRetryJobAsync(int retryCount = 0)
        {
            bool isFailed = false;

            try
            {
                bus.SubscribeAsync<Schedule>(
                    GetSubscriptionIdKey(retryJobSubscriptionId),
                    schedule => jobHandler.RetryAllProcessAsync(schedule),
                    config => config.WithTopic(retryJobSubscriptionId));

                LogService.LogInfo($"Subcribed the queue with key {retryJobSubscriptionId} successfully");
            }
            catch (Exception ex)
            {
                LogService.LogError($"There is an error while subscribing a queue with key {retryJobSubscriptionId}. Retrying...", ex);
                isFailed = true;
            }

            if (isFailed && retryCount <= AppSettings.MailCientConfig.RequestRetryLimit)
            {
                retryCount++;

                await SubscribeRetryJobAsync(retryCount);
            }
        }
        #endregion

        private static string GetSubscriptionIdKey(string topic)
        {
            var ip = (
                from ips in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                where ips.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                select ips)
                .FirstOrDefault();

            return string.Format("{0}_{1}_{2}", topic, Process.GetProcessById(Process.GetCurrentProcess().Id).ProcessName, ip);
        }
    }
}