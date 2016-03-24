using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;

namespace Fanex.MailDelivery.Services.Services.MailService
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IMailService
    {
        void UpdateJobStatus(JobMessage jobMessage);

        int SendMail(Mail mail);

        void AddMailTaskForRunningJob(JobMessage jobMessage);

        MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request);
    }
}