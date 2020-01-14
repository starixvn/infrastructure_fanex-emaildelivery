using Fanex.MailDelivery.Services.Services.Client;
using Quartz;

namespace Fanex.MailDelivery.MailStationService.Handlers
{
    public class CheckClientOnlineStatusJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {            
            var clientRepository = new ClientDataService();
            clientRepository.UpdateServiceCheckIn();
        }
    }
}