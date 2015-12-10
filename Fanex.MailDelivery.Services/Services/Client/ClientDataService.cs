using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
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
    }
}