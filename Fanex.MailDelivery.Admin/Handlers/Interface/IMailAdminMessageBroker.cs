namespace Fanex.MailDelivery.Admin.Handlers.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Admin ErrMsg Broker interface.
    /// </summary>
    public interface IMailAdminMessageBroker
    {
        void Start();

        void Stop();

        void PublishRetryJobMessage(Schedule schedule);

        void PublishScheduleChangeMessage(Schedule schedule);
    }
}
