using Fanex.MailDelivery.Core.Messaging.JobOperation;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public interface IJobOperationService
    {
        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);

        void Abort();
    }
}
