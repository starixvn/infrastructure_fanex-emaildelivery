using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public interface IClientServerService
    {
        int SendMail(Mail mail);

        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);

        void AddMailTaskForRunningJob(JobMessage jobMessage);

        void UpdateJobStatus(JobMessage jobMessage);

        MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request);
    }
}
