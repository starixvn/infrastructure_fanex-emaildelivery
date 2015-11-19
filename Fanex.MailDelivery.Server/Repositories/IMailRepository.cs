using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Models;
using Fanex.MailDelivery.Services.Criteria;

namespace Fanex.MailDelivery.Server.Repositories
{
    public interface IMailRepository
    {
        Schedule GetScheduleById(GetScheduleByIdCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailId criteria);

        void UpdateMailTask(UpdateMailTaskMSiteMailIdCriteria criteria);

        MailTask GetMailTask(GetMailTaskByIdCriteria criteria);

        void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria);

    }
}
