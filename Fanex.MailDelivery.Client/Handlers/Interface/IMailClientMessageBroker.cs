using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System.Collections.Generic;

    /// <summary>
    /// Mail Client Message Broker.
    /// </summary>
    public interface IMailClientMessageBroker
    {
        void Start(IJobHandler jobHandler);

        void Stop();

        void SubscribeScheduleChange();

        void Publish(Mail mail);

        void Publish(JobResultMessage jobStatus);

        List<Schedule> GetSchedules();

        JobResultMessage RequestAddMailTaskForRunningJob(JobResultMessage jobResult);

        void SubscribeRetryJob();

        void PublishClientNotifyMessage();
    }
}