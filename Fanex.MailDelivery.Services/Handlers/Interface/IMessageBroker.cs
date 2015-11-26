// -----------------------------------------------------------------------
// <copyright file="IMessageBroker.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Fanex.MailDelivery.Services.Handlers.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMessageBroker : IDisposable
    {
        void Start();

        void SubscribeMail();

        void SubscribeJobResult();

        void SubscribeScheduleRequest();
    }
}
