using System;

namespace Fanex.MailDelivery.ClientWinService.MessageBrokers
{
    public interface IClientServerMessageBroker : IDisposable
    {
        void Start();

        void RespondAddMailTaskForRunningJob();

        void SubscribeClientNotificationMessage();

        void SubscribeJobResult();

        void SubscribeMail();
    }
}