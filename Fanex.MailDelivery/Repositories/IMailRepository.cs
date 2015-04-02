namespace Fanex.MailDelivery.Repositories
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Criteria;

    public interface IMailRepository
    {
        Schedule GetScheduleByID(GetScheduleByIDCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailID criteria);

        MailType GetMailingListType(GetMailingListTypeCriteria criteria);

        void UpdateMailTask(UpdateMailTaskMSiteMailIDCriteria criteria);
    }
}
