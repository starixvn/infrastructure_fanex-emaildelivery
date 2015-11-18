using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Mail;
using Fanex.MailDelivery.Server.Criteria.Recipient;
using Fanex.NAP.Framework.Command;
using CreateMailCriteria = Fanex.MailDelivery.Services.Criteria.ManageMail.CreateMailCriteria;
using DeleteMailCriteria = Fanex.MailDelivery.Services.Criteria.ManageMail.DeleteMailCriteria;
using GetMailListCriteria = Fanex.MailDelivery.Services.Criteria.ManageMail.GetMailListCriteria;
using UpdateMailCriteria = Fanex.MailDelivery.Services.Criteria.ManageMail.UpdateMailCriteria;

namespace Fanex.MailDelivery.Server.MailService
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

        List<RecipientModel> GetListRecipient(GetRecipientListCriteria criteria);



        List<MailModel> GetListMail(GetMailListCriteria criteria);
        ResultResponse CreateMail(CreateMailCriteria criteria);
        ResultResponse UpdateMail(UpdateMailCriteria criteria);
        ResultResponse DeleteMail(DeleteMailCriteria criteria);

        ResultResponse CreateRecipient(CreateRecipientCriteria criteria);
        ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria);
        ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria);

        RecipientModel RecipientById(GetRecipientByIdCriteria criteria);
    }
}