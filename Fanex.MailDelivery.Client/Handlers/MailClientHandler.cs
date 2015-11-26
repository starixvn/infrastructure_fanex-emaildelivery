using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Handlers.Impl;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Client Handler.
    /// </summary>
    public static class MailClientHandler
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

        public static void SendMail(Mail mail)
        {
            _messageBroker.Publish(mail);
        }

        public static void ReportJob(JobResultMessage jobResult)
        {
            _messageBroker.Publish(jobResult);
        }

        public static List<Schedule> GetSchedules()
        {
            return _messageBroker.GetSchedules();
        }

        public static JobResultMessage RequestAddMailTaskForRunningJob(JobResultMessage jobResult)
        {
            return _messageBroker.RequestAddMailTaskForRunningJob(jobResult);
        }

        public static void RetryJob(Schedule schedule)
        {
            _jobHandler.RetryJob(schedule);
        }

        public static void PublishClientNotifyMessage()
        {
            _messageBroker.PublishClientNotifyMessage();
        }
    }
}