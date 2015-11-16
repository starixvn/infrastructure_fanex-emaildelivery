using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.ManageMail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.Services.MailService
{
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



        List<MailModel> GetListMail(GetMailListCriteria criteria);

        ResultResponse CreateMail(CreateMailCriteria criteria);
        ResultResponse UpdateMail(UpdateMailCriteria criteria);
        ResultResponse DeleteMail(DeleteMailCriteria criteria);

        ResultResponse CreateRecipient(CreateRecipientCriteria criteria);
        ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria);
        ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria);

        RecipientModel RecipientById(RecipientByIdCriteria criteria);
    }
}