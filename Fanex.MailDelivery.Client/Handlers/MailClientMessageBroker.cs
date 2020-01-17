using EasyNetQ;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Handlers
{
    public interface IMailClientMessageBroker
    {
        void Start(IJobHandler jobHandler);

        void Stop();

        Task PublishMailAsync(Mail mail);

        Task<IList<Schedule>> GetSchedulesAsync();

        Task PublishClientNotifyMessageAsync();

        void SubscribeScheduleChange();

        Task PublishJobMessageAsync(JobMessage jobMessage);

        Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobMessage);

        Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest request);

        void SubscribeRetryJob();
    }

    public class MailClientMessageBroker : IMailClientMessageBroker
    {
        private readonly string updateScheduleSubscriptionId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private readonly string retryJobSubscriptionId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private readonly string runNowJobSubscriptionId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private IBus bus;
        private IJobHandler jobHandler;

        public void Start(IJobHandler jobHandler)
        {
            this.jobHandler = jobHandler;
            bus = RabbitHutch.CreateBus(AppSettings.MailCientConfig.RabbitMQConnectionString);

            SubscribeScheduleChange();
            SubscribeRunNowJob();
            SubscribeRetryJob();
        }

        public void Stop()
        {
            bus.Dispose();
        }

        #region PublishAsync
        public async Task PublishMailAsync(Mail mail)
        {
            if (mail.ApplicationSendMailId == 0)
            {
                mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            mail.SiteId = AppSettings.MailCientConfig.SiteId;

            await bus.PublishAsync(mail, MailDeliveryConstants.SendMailRoutingKey);
        }

        public async Task PublishClientNotifyMessageAsync()
        {
            ScheduleElementCollection schedules = AppSettings.MailCientConfig.Schedules;
            string mailIds = string.Join(",", schedules.OfType<ScheduleElement>().Select(x => x.Id));

            await bus.PublishAsync(new UpdateActiveTimeRequest() { MailIds = mailIds }, MailDeliveryConstants.ClientNotificationTopic);
        }

        public async Task PublishJobMessageAsync(JobMessage jobMessage)
        {
            if (jobMessage.Mail != null && jobMessage.Mail.ApplicationSendMailId == 0)
            {
                jobMessage.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            await bus.PublishAsync(jobMessage, MailDeliveryConstants.JobResultRoutingKey);
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

            GetScheduleListResponse response = await bus.RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(request);

            return response.Schedules;
        }

        public async Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobMessage)
        {
            return await bus.RequestAsync<JobMessage, JobMessage>(jobMessage);
        }

        public async Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest request)
        {
            return await bus.RequestAsync<GetMailTaskByIdRequest, MailTaskModel>(request);
        }
        #endregion

        #region Subscribe
        public void SubscribeScheduleChange()
        {
            bus.SubscribeAsync<Schedule>(
                GetSubscriptionIdKey(updateScheduleSubscriptionId),
                schedule => jobHandler.UpdateSchedule(schedule),
                config => config.WithTopic(updateScheduleSubscriptionId));
        }

        public void SubscribeRunNowJob()
        {
            bus.SubscribeAsync<Schedule>(
                GetSubscriptionIdKey(runNowJobSubscriptionId),
                request => jobHandler.RetryAllProcess(request),
                config => config.WithTopic(runNowJobSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            bus.SubscribeAsync<Schedule>(
                GetSubscriptionIdKey(retryJobSubscriptionId),
                schedule => jobHandler.RetryAllProcess(schedule),
                config => config.WithTopic(retryJobSubscriptionId));
        }
        #endregion

        private string GetSubscriptionIdKey(string topic)
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