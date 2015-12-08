using Fanex.MailDelivery.Core.Models;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.Services.Services.MailService
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        CommandResult UpdateJobStatus(JobMessage jobMessage);

        int SendMail(Mail mail);

        void AddMailTaskForRunningJob(JobMessage jobMessage);
    }
}