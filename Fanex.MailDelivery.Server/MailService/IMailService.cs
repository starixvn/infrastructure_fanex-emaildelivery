using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Recipient;
using Fanex.NAP.Framework.Command;

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


    }
}