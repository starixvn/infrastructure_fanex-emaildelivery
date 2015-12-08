using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Services.Client;
using Fanex.MailDelivery.Services.Services.MailService;

namespace Fanex.MailDelivery.ClientWinService.Services
{
    public class ClientServerService : IClientServerService
    {
        private readonly IClientDataService _clientDataService;
        private readonly IMailService _mailService;


        public ClientServerService(IClientDataService clientDataService, IMailService mailService)
        {
            _clientDataService = clientDataService;
            _mailService = mailService;
        }

        public int SendMail(Mail mail)
        {
            return _mailService.SendMail(mail);
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            IList<Schedule> schedules = new List<Schedule>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = _clientDataService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                schedules.Add(schedule);
            }

            return new GetScheduleListResponse { Schedules = schedules };
        }

        public void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest)
        {
            _clientDataService.UpdateClientActiveTime(updateActiveTimeRequest);
        }



    }
}