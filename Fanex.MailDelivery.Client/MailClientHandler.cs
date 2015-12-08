using System.Collections.Generic;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client
{
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

        public static void SendFunctionMail(Mail mail)
        {
            _messageBroker.Publish(mail);
        }

        public static void ReportJob(JobMessage jobResult)
        {
            _messageBroker.Publish(jobResult);
        }

        public static IList<Schedule> GetSchedules()
        {
            return _messageBroker.GetSchedules();
        }

        public static JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult)
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