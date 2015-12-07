using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Services;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public class ClientServerService : IClientServerService
    {
        private readonly IMailStationService _mailStationService;

        public ClientServerService(IMailStationService mailStationService)
        {
            _mailStationService = mailStationService;
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            IList<Schedule> schedules = new List<Schedule>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = _mailStationService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                schedules.Add(schedule);
            }

            return new GetScheduleListResponse { Schedules = schedules };
        }
    }
}