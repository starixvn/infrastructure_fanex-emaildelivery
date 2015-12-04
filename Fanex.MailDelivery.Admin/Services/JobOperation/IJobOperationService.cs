#region

using Fanex.MailDelivery.Core.Models;

#endregion

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public interface IJobOperationService
    {
        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);

        void Abort();
    }
}