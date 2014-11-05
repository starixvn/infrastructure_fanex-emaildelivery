namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailingList;
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

        MailingListMessage GetFunctionMailingList(int functionId);

        MailingListMessage GetScheduleMailingList(int scheduleId);

        ResultMessage UpdateFunctionMailingList(int functionId, MailingListMessage mailingList);

        ResultMessage UpdateScheduleMailingList(int scheduleId, MailingListMessage mailingList);

        List<Schedule> GetSchedules();

        void SubscribeRetryJob();
    }
}