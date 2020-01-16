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

namespace Fanex.MailDelivery.Client.Handlers
{
    public interface IMailClientMessageBroker
    {
        void Start(IJobHandler jobHandler);

        void Stop();

        void Publish(Mail mail);

        IList<Schedule> GetSchedules();

        void PublishClientNotifyMessage();

        void SubscribeScheduleChange();

        void Publish(JobMessage jobStatus);

        JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult);

        MailTaskModel RequestMailTaskById(GetMailTaskByIdRequest taskId);

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

        public void Publish(Mail mail)
        {
            if (mail.ApplicationSendMailId == 0)
            {
                mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            mail.SiteId = AppSettings.MailCientConfig.SiteId;

            bus.Publish(mail, MailDeliveryConstants.SendMailRoutingKey);
        }

        public IList<Schedule> GetSchedules()
        {
            ScheduleElementCollection scheduleEles = AppSettings.MailCientConfig.Schedules;

            GetScheduleListRequest request = new GetScheduleListRequest();

            foreach (ScheduleElement scheduleConfig in scheduleEles)
            {
                request.MailIds.Add(scheduleConfig.Id);
            }

            GetScheduleListResponse response = bus.Request<GetScheduleListRequest, GetScheduleListResponse>(request);
            IList<Schedule> schedules = new List<Schedule>();
            response.Schedules.ToList().ForEach(n => schedules.Add(n));

            return schedules;
        }

        public void PublishClientNotifyMessage()
        {
            string mailIds = string.Empty;
            ScheduleElementCollection schedules = AppSettings.MailCientConfig.Schedules;
            foreach (ScheduleElement schedule in schedules)
            {
                mailIds += schedule.Id + ",";
            }

            mailIds = mailIds.Trim(',');

            bus.Publish(new UpdateActiveTimeRequest() { MailIds = mailIds }, MailDeliveryConstants.ClientNotificationTopic);
        }

        /// <summary>
        /// For the first time JobRequest is running, we need to get back the TaskId of the current running JobRequest.
        /// By insert new record at DB, it get back itself with additional TaskId's value.
        /// </summary>
        public JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult)
        {
            return bus.Request<JobMessage, JobMessage>(jobResult);
        }

        public MailTaskModel RequestMailTaskById(GetMailTaskByIdRequest taskId)
        {
            return bus.Request<GetMailTaskByIdRequest, MailTaskModel>(taskId);
        }

        public void Publish(JobMessage jobStatus)
        {
            if (jobStatus.Mail != null && jobStatus.Mail.ApplicationSendMailId == 0)
            {
                jobStatus.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            bus.Publish(jobStatus, MailDeliveryConstants.JobResultRoutingKey);
        }

        public void SubscribeScheduleChange()
        {
            bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(updateScheduleSubscriptionId),
                    schedule =>
                    {
                        jobHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(updateScheduleSubscriptionId));
        }

        public void SubscribeRunNowJob()
        {
            bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(runNowJobSubscriptionId),
                    request =>
                    {
                        jobHandler.RetryAllProcess(request);
                    },
                    config => config.WithTopic(runNowJobSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(retryJobSubscriptionId),
                    schedule =>
                    {
                        jobHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(retryJobSubscriptionId));
        }

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