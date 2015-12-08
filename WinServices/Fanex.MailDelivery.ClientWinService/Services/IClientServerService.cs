using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public interface IClientServerService
    {
        int SendMail(Mail mail);

        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);
    }
}
