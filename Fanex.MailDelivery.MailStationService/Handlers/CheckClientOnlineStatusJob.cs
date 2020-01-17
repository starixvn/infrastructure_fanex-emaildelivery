using Fanex.MailDelivery.Services.Services.Client;
using Quartz;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.Handlers
{
    public class CheckClientOnlineStatusJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            new ClientDataService().UpdateServiceCheckIn();

            return Task.CompletedTask;
        }
    }
}