using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using EasyNetQ;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Mapping;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers
{
    /// <summary>
    /// Rabbit MQ Broker for PUB/SUB message.
    /// </summary>
    public class MailClientMessageBroker : IMailClientMessageBroker
    {
        private string _updateScheduleSubscriptionId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private string _retryJobSubscriptionId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private string _runNowJobSubscriptionId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private IBus _bus;
        private IJobHandler _jobHandler;

        /// <summary>
        /// Starts the specified job handler.
        /// </summary>
        /// <param name="jobHandler">The job handler.</param>
        public void Start(IJobHandler jobHandler)
        {
            _jobHandler = jobHandler;
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailCientConfig.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new MailClientLogger());
                });

            SubscribeRunNowJob();

            SubscribeScheduleChange();
            SubscribeRetryJob();
        }

        public void Stop()
        {
            _bus.Dispose();
        }


        public void SubscribeRunNowJob()
        {
            _bus.Subscribe<Schedule>(
                this.GetSubscriptionIdKey(_runNowJobSubscriptionId),
                    request =>
                    {
                        _jobHandler.RetryAllProcess(request);
                    },
                    config => config.WithTopic(_runNowJobSubscriptionId));
        }

        public void SubscribeScheduleChange()
        {
            _bus.Subscribe<Schedule>(
                this.GetSubscriptionIdKey(_updateScheduleSubscriptionId),
                    schedule =>
                    {
                        _jobHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(_updateScheduleSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            _bus.Subscribe<Schedule>(
                this.GetSubscriptionIdKey(_retryJobSubscriptionId),
                    schedule =>
                    {
                        _jobHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(_retryJobSubscriptionId));
        }



        public void Publish(Mail mail)
        {
            if (mail.ApplicationSendMailId == 0)
            {
                mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            mail.SiteId = AppSettings.MailCientConfig.SiteId;
            _bus.Publish(mail, MailDeliveryConstants.SendMailRoutingKey);
        }

        /// <summary>
        /// Gets the schedules.
        /// </summary>
        /// <returns>Schedule list.</returns>
        public IList<Schedule> GetSchedules()
        {
            Schedule scheduleRequest = new Schedule();
            ScheduleElementCollection scheduleEles = AppSettings.MailCientConfig.Schedules;

            GetScheduleListRequest request = new GetScheduleListRequest();

            foreach (ScheduleElement scheduleConfig in scheduleEles)
            {
                request.MailIds.Add(scheduleConfig.Id);
            }

            GetScheduleListResponse response = _bus.Request<GetScheduleListRequest, GetScheduleListResponse>(request);
            IList<Schedule> schedules = new List<Schedule>();
            response.ScheduleDtos.ToList().ForEach(n => schedules.Add(n.ToScheduleModel()));

            return schedules;
        }

        /// <summary>
        /// For the first time JobRequest is running, we need to get back the TaskId of the current running JobRequest.
        /// By insert new record at DB, it get back itself with additional TaskId's value.
        /// </summary>
        /// <param name="jobResult">JobRequest Result.</param>
        /// <returns>
        /// JobRequest Result Message.
        /// </returns>
        public JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult)
        {
            return _bus.Request<JobMessage, JobMessage>(jobResult);
        }

        public void Publish(JobMessage jobStatus)
        {
            if (jobStatus.Mail != null && jobStatus.Mail.ApplicationSendMailId == 0)
            {
                jobStatus.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            _bus.Publish(jobStatus, MailDeliveryConstants.JobResultRoutingKey);
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
            _bus.Publish(new UpdateActiveTimeRequest() { MailIds = mailIds }, MailDeliveryConstants.ClientNotificationTopic);
        }

        /// <summary>
        /// Gets the random string.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns>Random string.</returns>
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