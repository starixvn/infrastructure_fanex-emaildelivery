using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;

namespace Fanex.MailDelivery.Services.Repositories.Client
{
    public interface IClientRepository
    {
        Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria);

        void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria);
    }
}
