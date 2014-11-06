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

        MailingList GetFunctionMailingList(int functionId);

        MailingList GetScheduleMailingList(int scheduleId);

        ResultMessage UpdateFunctionMailingList(int functionId, MailingList mailingList);

        ResultMessage UpdateScheduleMailingList(int scheduleId, MailingList mailingList);

        List<Schedule> GetSchedules();

        void SubscribeRetryJob();
    }
}