namespace Fanex.MailDelivery.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Criteria;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        List<Schedule> GetSchedules(List<int> scheduleId);

        CommandResult UpdateJobStatus(JobResultMessage jobResultMessage);

        int SendMail(Mail mail);

        MailingList GetMailingList(int mailingListId);

        void AddMailTaskForRunningJob(JobResultMessage jobResultMessage);

        void UpdateClientActiveTime(ClientInfomationMessage clientInfoMessage);
    }
}