namespace Fanex.MailDelivery.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.Framework.Command;
using Fanex.MailDelivery.Client.Messages;

    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        List<Schedule> GetSchedules(List<int> scheduleId);

        CommandResult UpdateJobStatus(JobResultMessage jobResultMessage);

        CommandResult SendMail(Mail mail);

        MailingList GetMailingList(int mailingListId);

        CommandResult UpdateMailingList(MailingList mailingList);

        void AddMailTaskForRunningJob(JobResultMessage jobResultMessage);
    }
}