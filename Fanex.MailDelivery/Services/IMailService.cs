namespace Fanex.MailDelivery.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Criteria.Mail;
    using Fanex.MailDelivery.Criteria.Recipient;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        List<Schedule> GetSchedules(List<int> scheduleId);

        CommandResult UpdateJobStatus(JobResultMessage jobResultMessage);

        int SendMail(Mail mail);

        void AddMailTaskForRunningJob(JobResultMessage jobResultMessage);

        void UpdateClientActiveTime(ClientInfomationMessage clientInfoMessage);

        List<RecipientModel> GetListRecipient(RecipientListCriteria criteria);

        ResultMessage CreateRecipient(CreateRecipientCriteria criteria);

        ResultMessage UpdateRecipient(UpdateRecipientCriteria criteria);

        List<MailModel> GetListMail(GetMailListCriteria criteria);

        ResultMessage CreateMail(CreateMailCriteria criteria);

        ResultMessage UpdateMail(UpdateMailCriteria criteria);

        RecipientModel RecipientById(RecipientByIdCriteria criteria);
    }
}