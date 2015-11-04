namespace Fanex.MailDelivery.Repositories
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Criteria;
    using Fanex.MailDelivery.Criteria.Mail;
    using Fanex.MailDelivery.Criteria.Recipient;
    using Fanex.MailDelivery.Models;

    public interface IMailRepository
    {
        Schedule GetScheduleByID(GetScheduleByIdCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailId criteria);

        void UpdateMailTask(UpdateMailTaskMSiteMailIdCriteria criteria);

        MailTask GetMailTask(GetMailTaskByIdCriteria criteria);

        void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria);

        List<RecipientModel> GetListRecipient(RecipientListCriteria criteria);

        ResultMessage CreateRecipient(CreateRecipientCriteria criteria);

        ResultMessage UpdateRecipient(UpdateRecipientCriteria criteria);

        List<MailModel> GetListMail(GetMailListCriteria criteria);

        ResultMessage CreateMail(CreateMailCriteria criteria);

        ResultMessage UpdateMail(UpdateMailCriteria criteria);

        RecipientModel RecipientById(RecipientByIdCriteria criteria);
    }
}
