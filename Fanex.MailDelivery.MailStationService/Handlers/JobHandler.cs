using Fanex.MailDelivery.Services.Configurations;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService.Handlers
{
    public class JobHandler
    {
        private IScheduler scheduler;

        public async Task Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            scheduler = await schedFact.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CheckClientOnlineStatusJob>()
                    .WithIdentity("CheckClientOnlineStatusJob_CheckIn")
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CheckClientOnlineStatusTriggerName_CheckIn")
                .WithSimpleSchedule(m => m.WithIntervalInSeconds(AppSettings.MailServiceSection.CheckClientOnlineStatusJobInterval).RepeatForever())
                .StartNow()
                .Build();

            await scheduler.DeleteJob(job.Key);

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task Stop()
        {
            if (scheduler != null)
            {
                await scheduler.Shutdown();
            }
        }
    }
}