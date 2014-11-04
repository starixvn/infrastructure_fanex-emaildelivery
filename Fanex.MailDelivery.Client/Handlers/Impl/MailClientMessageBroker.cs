namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System.Collections.Generic;
    using EasyNetQ;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailingList;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Rabbit MQ Broker for PUB/SUB message.
    /// </summary>
    public class MailClientMessageBroker : IMailClientMessageBroker
    {
        private string UpdateScheduleSubscriptionId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private string RetryJobSubscriptionId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, AppSettings.MailCientConfig.SiteId);
        private IBus _bus;

        public void Start()
        {
            _bus = RabbitHutch.CreateBus(AppSettings.MailCientConfig.RabbitMQConnectionString,
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
            _bus.Subscribe<Schedule>(UpdateScheduleSubscriptionId,
                    schedule =>
                    {
                        MailClientHandler.UpdateSchedule(schedule);
                    },
                    config => config.WithTopic(UpdateScheduleSubscriptionId));
        }

        public void SubscribeRetryJob()
        {
            _bus.Subscribe<Schedule>(RetryJobSubscriptionId,
                    schedule =>
                    {
                        MailClientHandler.RetryAllProcess(schedule);
                    },
                    config => config.WithTopic(RetryJobSubscriptionId));
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

        public List<Schedule> GetSchedules()
        {
            ScheduleRequestMessage scheduleRequest = new ScheduleRequestMessage();
            var schedules = AppSettings.MailCientConfig.Schedules;

            foreach (ScheduleElement scheduleConfig in schedules)
            {
                scheduleRequest.Ids.Add(scheduleConfig.Id);
            }

            return _bus.Request<ScheduleRequestMessage, List<Schedule>>(scheduleRequest);
        }

        public void Publish(JobResultMessage jobStatus)
        {
            if (jobStatus.Mail.ApplicationSendMailId == 0)
            {
                jobStatus.Mail.ApplicationSendMailId = AppSettings.MailCientConfig.ApplicationSendMailId;
            }

            _bus.Publish(jobStatus, MailDeliveryConstants.JobResultRoutingKey);
        }

        public MailingListMessage GetFunctionMailingList(int functionId)
        {
            var mailingListRequest = new FunctionMailingListRequest
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                FunctionId = functionId
            };

            return _bus.Request<FunctionMailingListRequest, MailingListMessage>(mailingListRequest);
        }

        public MailingListMessage GetScheduleMailingList(int scheduleId)
        {
            var mailingListRequest = new ScheduleMailingListRequest
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                ScheduleId = scheduleId
            };

            return _bus.Request<ScheduleMailingListRequest, MailingListMessage>(mailingListRequest);
        }

        public ResultMessage UpdateFunctionMailingList(int functionId, MailingListMessage mailingList)
        {
            var updateFunctionMailingListMessage = new UpdateFunctionMailingListMessage
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                FunctionId = functionId,
                MailingList = mailingList
            };

            return _bus.Request<UpdateFunctionMailingListMessage, ResultMessage>(updateFunctionMailingListMessage);
        }

        public ResultMessage UpdateScheduleMailingList(int scheduleId, MailingListMessage mailingList)
        {
            var updateScheduleMailingListMessage = new UpdateScheduleMailingListMessage
            {
                SiteId = AppSettings.MailCientConfig.SiteId,
                ScheduleId = scheduleId,
                MailingList = mailingList
            };

            return _bus.Request<UpdateScheduleMailingListMessage, ResultMessage>(updateScheduleMailingListMessage);
        }
    }
}