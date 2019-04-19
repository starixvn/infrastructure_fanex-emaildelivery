using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;

namespace Fanex.MailDelivery.Services.Services.Client
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IClientDataService
    {
        Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria);

        void UpdateClientActiveTime(UpdateActiveTimeRequest clientInfoMessage);

        void UpdateServiceCheckIn();
    }
}