using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Services.MailService;
using System.Collections.Generic;

namespace Fanex.MailDelivery.MailStationService.Services
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

    public class ClientServerService : IClientServerService
    {
        private readonly IMailService mailService;

        public ClientServerService(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public int SendMail(Mail mail)
        {
            return mailService.SendMail(mail);
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            IList<Schedule> schedules = new List<Schedule>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = mailService.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                schedules.Add(schedule);
            }

            return new GetScheduleListResponse { Schedules = schedules };
        }

        public void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest)
        {
            mailService.UpdateClientActiveTime(updateActiveTimeRequest);
        }

        public void AddMailTaskForRunningJob(JobMessage jobMessage)
        {
            mailService.AddMailTaskForRunningJob(jobMessage);
        }

        public MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request)
        {
            return mailService.GetMailTaskById(request);
        }

        public void UpdateJobStatus(JobMessage jobMessage)
        {
            mailService.UpdateJobStatus(jobMessage);
        }
    }
}