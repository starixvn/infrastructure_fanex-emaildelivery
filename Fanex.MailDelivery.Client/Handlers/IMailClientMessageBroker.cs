using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Client.Handlers
{
    /// <summary>
    /// Mail Client Message Broker.
    /// </summary>
    public interface IMailClientMessageBroker
    {
        void Start(IJobHandler jobHandler);

        void Stop();

        void Publish(Mail mail);

        IList<Schedule> GetSchedules();

        void PublishClientNotifyMessage();

        void SubscribeScheduleChange();

        void Publish(JobMessage jobStatus);

        JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult);

        MailTaskModel RequestMailTaskById(GetMailTaskByIdRequest taskId);

        void SubscribeRetryJob();
    }
}