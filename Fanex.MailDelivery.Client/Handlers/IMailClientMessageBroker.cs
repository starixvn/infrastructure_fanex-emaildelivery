using System.Collections.Generic;
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

        void Publish(JobRequest jobStatus);

        List<Schedule> GetSchedules();

        JobRequest RequestAddMailTaskForRunningJob(JobRequest jobResult);

        void SubscribeRetryJob();

        void PublishClientNotifyMessage();
    }
}