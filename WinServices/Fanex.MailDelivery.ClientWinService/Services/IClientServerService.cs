using Fanex.MailDelivery.Core.Messaging.Schedule;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public interface IClientServerService
    {
        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);
    }
}
