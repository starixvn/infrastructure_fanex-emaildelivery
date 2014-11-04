namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailingList;

    /// <summary>
    /// I Message Broker.
    /// </summary>
    public interface IMailClientMessageBroker
    {
        void Start();

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
