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

        void Publish(Mail mail);

        IList<Schedule> GetSchedules();

        void PublishClientNotifyMessage();

        void SubscribeScheduleChange();

        void Publish(JobMessage jobStatus);

        JobMessage RequestAddMailTaskForRunningJob(JobMessage jobResult);

        void SubscribeRetryJob();
    }
}