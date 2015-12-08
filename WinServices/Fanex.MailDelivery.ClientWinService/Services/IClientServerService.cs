using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public interface IClientServerService
    {
        int SendMail(Mail mail);

        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);

        void AddMailTaskForRunningJob(JobMessage jobMessage);

        CommandResult UpdateJobStatus(JobMessage jobMessage);
    }
}
