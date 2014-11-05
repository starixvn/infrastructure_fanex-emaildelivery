namespace Fanex.MailDelivery.Client.Handlers
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Handlers.Impl;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailingList;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Client Handler.
    /// </summary>
    public class MailClientHandler
    {
        private static IJobHandler _jobHandler;
        private static IMailClientMessageBroker _messageBroker;

        public static void Start(
            IJobHandler jobHandler = null,
            IMailClientMessageBroker messageBroker = null)
        {
            _jobHandler = jobHandler ?? new JobHandler();
            _messageBroker = messageBroker ?? new MailClientMessageBroker();
            _messageBroker.Start(_jobHandler);
            _jobHandler.Start();
        }

        public static void Stop()
        {
            _messageBroker.Stop();
            _jobHandler.Stop();
        }

        public static void SendFunctionMail(Mail mail)
        {
            _messageBroker.Publish(mail);
        }

        public static void SendAutoMail(JobResultMessage jobResult)
        {
            _messageBroker.Publish(jobResult);
        }

        public static List<Schedule> GetSchedules()
        {
            return _messageBroker.GetSchedules();
        }

        public static MailingListMessage GetFunctionMailingList(int functionId)
        {
            return _messageBroker.GetFunctionMailingList(functionId);
        }

        public static MailingListMessage GetScheduleMailingList(int scheduleId)
        {
            return _messageBroker.GetScheduleMailingList(scheduleId);
        }

        public static ResultMessage UpdateFunctionMailingList(int functionId, MailingListMessage mailingList)
        {
            return _messageBroker.UpdateFunctionMailingList(functionId, mailingList);
        }

        public static ResultMessage UpdateScheduleMailingList(int scheduleId, MailingListMessage mailingList)
        {
            return _messageBroker.UpdateScheduleMailingList(scheduleId, mailingList);
        }

        public static void RetryJob(Schedule schedule)
        {
            _jobHandler.RetryJob(schedule);
        }
    }
}