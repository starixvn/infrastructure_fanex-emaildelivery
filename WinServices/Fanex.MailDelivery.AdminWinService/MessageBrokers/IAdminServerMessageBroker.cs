namespace Fanex.MailDelivery.AdminWinService.MessageBrokers
{
    using System;

    public interface IAdminServerMessageBroker : IDisposable
    {
        void Start();

        void ResponseMailFunctions();

        void ResponseRecipientFunctions();

        void ResponseGroupFunctions();

        void ResponseReportFunctions();

        void ResponseJobFunctions();

        void SubscribeAbortJob();
    }
}