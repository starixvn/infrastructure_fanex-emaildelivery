namespace Fanex.MailDelivery.Admin.Handlers
{
    using Fanex.MailDelivery.Admin.Handlers.Impl;
    using Fanex.MailDelivery.Admin.Handlers.Interface;
    using Fanex.MailDelivery.Client.Models;
    using System;

    /// <summary>
    /// Mail Admin Handler.
    /// </summary>
    public class MailAdminHandler
    {
        private static IMailAdminMessageBroker _messageBroker;

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