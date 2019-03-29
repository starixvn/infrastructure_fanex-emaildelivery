using System.Diagnostics.CodeAnalysis;
using Fanex.MailDelivery.Services.Services.Client;
using Quartz;

namespace Fanex.MailStation.Admin.Service.Handlers
{
    /// <summary>
    /// For every 5 mins, load all schedules into cache and identify online status.
    /// Send mail to admin if there is any change on online status (On/Off).
    /// </summary>
    public class CheckClientOnlineStatusJob : IJob
    {
        public const string CheckClientOnlineStatusJobName = "CheckClientOnlineStatusJobName";
        public const string CheckClientOnlineStatusTriggerName = "CheckClientOnlineStatusTriggerName";
        private const string keyScheduleMailListCache = "Fanex.MailStation.Admin.Service.Handlers";

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void Execute(IJobExecutionContext context)
        {
            var clientRepository = new ClientDataService();
            clientRepository.UpdateServiceCheckIn();
        }
    }
}