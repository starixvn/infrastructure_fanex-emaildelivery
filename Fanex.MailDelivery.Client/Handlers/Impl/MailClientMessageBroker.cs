namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using EasyNetQ;
    using Fanex.MailDelivery.Client.Configurations;
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
        private string _runJobImmediatelySubscriptionId = string.Format(MailDeliveryConstants.RunJobImmediatelyRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
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
            SubscribeRunJobImmediately();
        }

        public void Stop()
        {
            _bus.Dispose();
        }

        public void SubscribeScheduleChange()
        {
            _bus.Subscribe<Schedule>(
                this.GetRandomString(),
                    schedule =>
                    {
                        _jobHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(_updateScheduleSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            _bus.Subscribe<Schedule>(
                this.GetRandomString(),
                    schedule =>
                    {
                        _jobHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(_retryJobSubscriptionId));
        }

        public void SubscribeRunJobImmediately()
        {
            _bus.Subscribe<Schedule>(
                this.GetRandomString(),
                    schedule =>
                    {
                        _jobHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(_runJobImmediatelySubscriptionId));
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

        public MailingList GetMailingList(int mailId)
        {
            return _bus.Request<MailingListRequest, MailingList>(new MailingListRequest() { MailingListId = mailId });
        }

        /// <summary>
        /// For the first time Job is running, we need to get back the TaskId of the current running Job.
        /// By insert new record at DB, it get back itself with additional TaskId's value.
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

        /// <summary>
        /// Gets the random string.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        /// <returns>Random string.</returns>
        private string GetRandomString(int maxSize = 8)
        {
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }
    }
}