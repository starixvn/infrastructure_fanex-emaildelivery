namespace Fanex.MailDelivery.Admin.Handlers
{
    using System;
    using EasyNetQ;
    using Fanex.MailDelivery.Admin.Configurations;
    using Fanex.MailDelivery.Admin.Handlers.Impl;
    using Fanex.MailDelivery.Admin.Handlers.Interface;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Admin Handler.
    /// </summary>
    public static class MailAdminHandler
    {
        private static IBus _bus;

        private static IMailAdminMessageBroker _messageBroker;

        internal static IBus Bus
        {
            get
            {
                if (_bus == null)
                {
                    lock (_bus)
                    {
                        if (_bus == null)
                        {
                            _bus = RabbitHutch.CreateBus(
                                AppSettings.MailAdminConfig.RabbitMQConnectionString,
                                serviceRegister =>
                                {
                                    serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                                });
                        }
                    }
                }

                return _bus;
            }
        }

        public static void Start(IMailAdminMessageBroker messageBroker = null)
        {
            _messageBroker = messageBroker ?? new MailAdminMessageBroker();
            _messageBroker.Start();
        }

        public static void Stop()
        {
            _messageBroker.Stop();
        }

        public static void PublishRetryJobMessage(Schedule schedule)
        {
            _messageBroker.PublishRetryJobMessage(schedule);
        }

        public static void PublishScheduleChangeMessage(Schedule schedule)
        {
            _messageBroker.PublishScheduleChangeMessage(schedule);
        }

        public static void PublishAbortJobMessage(int taskID)
        {
            _messageBroker.PublishAbortJobMessage(taskID);
        }

        public static void RequestCreateRecipientMessage()
        {
            throw new NotImplementedException();
        }

        public static void RequestUpdateRecipientMessage()
        {
            throw new NotImplementedException();
        }

        public static void RequestReadRecipientByEmailMessage()
        {
            throw new NotImplementedException();
        }
    }
}