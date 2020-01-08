using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Job;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IJobService
    {
        GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request);
    }

    public class JobService : BaseAdminService, IJobService
    {
        public GetScheduleByMailIdResponse GetScheduleByIdsRequest(GetScheduleByMailIdRequest request)
        {
            Schedule schedule = adminDataService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
            {
                MailId = request.MailId
            });

            return new GetScheduleByMailIdResponse { Schedule = schedule };
        }
    }
}