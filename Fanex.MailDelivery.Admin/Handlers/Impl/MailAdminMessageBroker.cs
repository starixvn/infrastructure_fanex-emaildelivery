namespace Fanex.MailDelivery.Admin.Handlers.Impl
{
    using System;
    using EasyNetQ;
    using Fanex.MailDelivery.Admin.Configurations;
    using Fanex.MailDelivery.Admin.Handlers.Interface;
    using Fanex.MailDelivery.Client;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Admin Message Broker.
    /// </summary>
    public class MailAdminMessageBroker : IMailAdminMessageBroker
    {
        private IBus _bus;

        public void Start()
        {
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailAdminConfig.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                });
        }

        public void Stop()
        {
            _bus.Dispose();
        }

        public void PublishRetryJobMessage(Schedule schedule)
        {
            string retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            _bus.Publish<Schedule>(schedule, retryJobPublishId);
            // Console.WriteLine("Retry Schedule " + schedule.Id);
        }

        public void PublishScheduleChangeMessage(Schedule schedule)
        {
            string updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

            _bus.Publish<Schedule>(schedule, updateSchedulePublishId);
            // Console.WriteLine("Updated Schedule " + schedule.Id);
        }
    }
}