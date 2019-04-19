using System;
using System.Net;
using Fanex.Logging;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories.Client;

namespace Fanex.MailDelivery.Services.Services.Client
{
    /// <summary>
    /// Admin Service.
    /// </summary>
    public class ClientDataService : IClientDataService
    {
        private readonly IClientRepository _clientRepository;

        public ClientDataService()
        {
            _clientRepository = _clientRepository ?? new ClientRepository();
        }

        public Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria)
        {
            return _clientRepository.GetScheduleByMailId(criteria);
        }

        public void UpdateClientActiveTime(UpdateActiveTimeRequest clientInfoMessage)
        {
            _clientRepository.UpdateClientActiveTime(new UpdateClientActiveTimeCriteria() { MailIds = clientInfoMessage.MailIds });
        }

        public void UpdateServiceCheckIn()
        {
            var criteria = new ServiceMonitoringCheckInCriteria
            {
                Interval = AppSettings.MailServiceSection.CheckClientOnlineStatusJobInterval,
                IPServer = GetLocalIP(),
                OrderBy = 1,
                ServiceName = AppSettings.MailServiceSection.ServiceName
            };

            var msg = $"ServiceName: {AppSettings.MailServiceSection.ServiceName}, IP: {GetLocalIP()}, Interval: {AppSettings.MailServiceSection.CheckClientOnlineStatusJobInterval}, Time: {DateTime.Now}";
            LogService.Info($"{msg}");

            _clientRepository.UpdateServiceCheckIn(criteria);
        }

        private string GetLocalIP()
        {
            var ip = string.Empty;
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ip = addr.ToString();
                }
            }

            return ip;
        }
    }
}