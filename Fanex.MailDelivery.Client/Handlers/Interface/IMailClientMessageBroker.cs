namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;

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

        MailingList GetMailingList(int mailId);

        JobResultMessage RequestAddMailTaskForRunningJob(JobResultMessage jobResult);

        void SubscribeRetryJob();

        void PublishClientNotifyMessage();
    }
}