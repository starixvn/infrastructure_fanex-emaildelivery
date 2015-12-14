using System;

namespace Fanex.MailDelivery.AdminWinService.MessageBrokers
{
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