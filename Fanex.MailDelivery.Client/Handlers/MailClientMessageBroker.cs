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
                    //serviceRegister.Register<IEasyNetQLogger>(_ => new MailClientLogger());
                });

            SubscribeScheduleChange();

            SubscribeRunNowJob();
            SubscribeRetryJob();
        }

        public void Stop()
        {
            _bus.Dispose();
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

        public IList<Schedule> GetSchedules()
        {
            ScheduleElementCollection scheduleEles = AppSettings.MailCientConfig.Schedules;

            GetScheduleListRequest request = new GetScheduleListRequest();

            foreach (ScheduleElement scheduleConfig in scheduleEles)
            {
                request.MailIds.Add(scheduleConfig.Id);
            }

            GetScheduleListResponse response = _bus.Request<GetScheduleListRequest, GetScheduleListResponse>(request);
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

            _bus.Publish(new UpdateActiveTimeRequest() { MailIds = mailIds }, MailDeliveryConstants.ClientNotificationTopic);
        }

        /// <summary>
        /// For the first time JobRequest is running, we need to get back the TaskId of the current running JobRequest.
        /// By insert new record at DB, it get back itself with additional TaskId's value.
        /// </summary>
        public JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult)
        {
            return _bus.Request<JobMessage, JobMessage>(jobResult);
        }

        public MailTaskModel RequestMailTaskById(GetMailTaskByIdRequest taskId)
        {
            return _bus.Request<GetMailTaskByIdRequest, MailTaskModel>(taskId);
        }

        public void Publish(JobMessage jobStatus)
        {
            if (jobStatus.Mail != null && jobStatus.Mail.ApplicationSendMailId == 0)
            {
                jobStatus.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            _bus.Publish(jobStatus, MailDeliveryConstants.JobResultRoutingKey);
        }

        public void SubscribeScheduleChange()
        {
            _bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(_updateScheduleSubscriptionId),
                    schedule =>
                    {
                        _jobHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(_updateScheduleSubscriptionId));
        }

        public void SubscribeRunNowJob()
        {
            _bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(_runNowJobSubscriptionId),
                    request =>
                    {
                        _jobHandler.RetryAllProcess(request);
                    },
                    config => config.WithTopic(_runNowJobSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            _bus.Subscribe<Schedule>(
                GetSubscriptionIdKey(_retryJobSubscriptionId),
                    schedule =>
                    {
                        _jobHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(_retryJobSubscriptionId));
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