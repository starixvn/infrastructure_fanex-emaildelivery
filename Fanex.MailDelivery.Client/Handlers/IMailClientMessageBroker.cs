using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers
{
    /// <summary>
    /// Mail Client Message Broker.
    /// </summary>
    public interface IMailClientMessageBroker
    {
        void Start(IJobHandler jobHandler);

        void Stop();

        void SubscribeScheduleChange();

        void Publish(Mail mail);

        void Publish(JobMessage jobStatus);

        IList<Schedule> GetSchedules();

        JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult);

        void SubscribeRetryJob();

        void PublishClientNotifyMessage();
    }
}