// -----------------------------------------------------------------------
// <copyright file="IMessageBroker.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Handlers.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMessageBroker : IDisposable
    {
        void Start();

        void SubscribeMail();

        void SubscribeJobResult();

        void SubscribeScheduleRequest();

        void SubscribeMailingListRequest();
    }
}
