using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Client
{
    public static class MailClientHandler
    {
        private static IJobHandler _jobHandler;
        private static IMailClientMessageBroker _messageBroker;

        public static void Start(IJobHandler jobHandler = null, IMailClientMessageBroker messageBroker = null)
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

        public static IList<Schedule> GetSchedules()
        {
            return _messageBroker.GetSchedules();
        }

        public static void PublishClientNotifyMessage()
        {
            _messageBroker.PublishClientNotifyMessage();
        }

        public static JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult)
        {
            return _messageBroker.RequestAddMailTaskForRunningJob(jobResult);
        }

        internal static MailTaskModel RequestMailTaskById(GetMailTaskByIdRequest taskID)
        {
            return _messageBroker.RequestMailTaskById(taskID);
        }

        public static void ReportJob(JobMessage jobResult)
        {
            _messageBroker.Publish(jobResult);
        }

        public static void RetryJob(Schedule schedule)
        {
            _jobHandler.RetryJob(schedule);
        }
    }
}