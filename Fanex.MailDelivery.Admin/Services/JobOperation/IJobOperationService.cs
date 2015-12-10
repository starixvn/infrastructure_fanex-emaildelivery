using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Admin.Services.JobOperation
{
    public interface IJobOperationService
    {
        void PublishScheduleChange(Schedule request);

        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);

        void Abort(JobMessage jobMessage);
    }
}