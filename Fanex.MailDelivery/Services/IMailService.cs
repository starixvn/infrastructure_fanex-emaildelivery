namespace Fanex.MailDelivery.Services
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        List<Schedule> GetSchedules(List<int> scheduleId);

        CommandResult UpdateJobStatus(Schedule schedule, JobStatus jobStatus);

        CommandResult SendMail(Mail mail);

        MailingList GetMailingList(int mailingListId);

        CommandResult UpdateMailingList(MailingList mailingList);
    }
}