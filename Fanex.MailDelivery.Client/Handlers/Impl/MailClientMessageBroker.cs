namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System.Collections.Generic;
    using EasyNetQ;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Rabbit MQ Broker for PUB/SUB message.
    /// </summary>
    public class MailClientMessageBroker : IMailClientMessageBroker
    {
        private string _updateScheduleSubscriptionId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private string _retryJobSubscriptionId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
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
            SubscribeScheduleChange();
            SubscribeRetryJob();
        }

        public void Stop()
        {
            _bus.Dispose();
        }

        public void SubscribeScheduleChange()
        {
            _bus.Subscribe<Schedule>(
                _updateScheduleSubscriptionId,
                    schedule =>
                    {
                        _jobHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(_updateScheduleSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            _bus.Subscribe<Schedule>(
                _retryJobSubscriptionId,
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
        public List<Schedule> GetSchedules()
        {
            ScheduleRequestMessage scheduleRequest = new ScheduleRequestMessage();
            var schedules = AppSettings.MailCientConfig.Schedules;

            foreach (ScheduleElement scheduleConfig in schedules)
            {
                scheduleRequest.Ids.Add(scheduleConfig.Id);
            }

            var isconnect = _bus.IsConnected;
            return _bus.Request<ScheduleRequestMessage, List<Schedule>>(scheduleRequest);
        }

        /// <summary>
        /// For the first time Job is running, we need to get back the TaskID of the current running Job.
        /// By insert new record at DB, it get back itself with additional TaskID's value.
        /// </summary>
        /// <param name="jobResult">Job Result.</param>
        /// <returns>
        /// Job Result Message.
        /// </returns>
        public JobResultMessage RequestAddMailTaskForRunningJob(JobResultMessage jobResult)
        {
            return _bus.Request<JobResultMessage, JobResultMessage>(jobResult);
        }

        public void Publish(JobResultMessage jobStatus)
        {
            if (jobStatus.Mail != null && jobStatus.Mail.ApplicationSendMailId == 0)
            {
                jobStatus.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            _bus.Publish(jobStatus, MailDeliveryConstants.JobResultRoutingKey);
        }

        public MailingList GetFunctionMailingList(int functionId)
        {
            var mailingListRequest = new MailingListRequest
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                MailingListId = functionId
            };

            return _bus.Request<MailingListRequest, MailingList>(mailingListRequest);
        }

        public MailingList GetScheduleMailingList(int scheduleId)
        {
            var mailingListRequest = new MailingListRequest
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                MailingListId = scheduleId,
                MailType = MailType.Schedule
            };

            return _bus.Request<MailingListRequest, MailingList>(mailingListRequest);
        }

        public ResultMessage UpdateFunctionMailingList(int functionId, MailingList mailingList)
        {
            mailingList.SiteId = AppSettings.MailCientConfig.SiteId;
            mailingList.Id = functionId;
            mailingList.MailType = MailType.Function;

            return _bus.Request<MailingList, ResultMessage>(mailingList);
        }

        public ResultMessage UpdateScheduleMailingList(int scheduleId, MailingList mailingList)
        {
            mailingList.SiteId = AppSettings.MailCientConfig.SiteId;
            mailingList.Id = scheduleId;
            mailingList.MailType = MailType.Schedule;

            return _bus.Request<MailingList, ResultMessage>(mailingList);
        }
    }
}