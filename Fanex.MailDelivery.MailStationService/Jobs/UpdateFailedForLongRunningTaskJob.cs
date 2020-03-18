using Fanex.MailDelivery.Services.Services;
using Quartz;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.Jobs
{
    public class UpdateFailedForLongRunningTaskJob : IJob
    {
        private readonly MailService mailService;

        public UpdateFailedForLongRunningTaskJob()
        {
            mailService = new MailServiceImpl(AppSettings.SendMailRetryLimit, AppSettings.SendMailRetryIntervalInMinutes);
        }

        public Task Execute(IJobExecutionContext context)
        {
            mailService.UpdateFailedForLongRunningMailTask();

            return Task.CompletedTask;
        }
    }
}
