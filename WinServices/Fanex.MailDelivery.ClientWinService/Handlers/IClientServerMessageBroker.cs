namespace Fanex.MailDelivery.ClientWinService.Handlers
{
    public interface IClientServerMessageBroker
    {
        void Dispose();
        void RespondAddMailTaskForRunningJob();
        void Start();
        void SubscribeAbortJob();
        void SubscribeClientNotificationMessage();
        void SubscribeJobResult();
        void SubscribeMail();
    }
}