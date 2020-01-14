using Fanex.MailDelivery.Services.Configurations;
using Quartz;
using Quartz.Impl;

namespace Fanex.MailDelivery.MailStationService.Handlers
{
    public class JobHandler
    {
        private IScheduler _scheduler;

        public void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            _scheduler = schedFact.GetScheduler();
            _scheduler.Start();

            IJobDetail job = JobBuilder.Create<CheckClientOnlineStatusJob>()
                    .WithIdentity("CheckClientOnlineStatusJob_CheckIn")
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CheckClientOnlineStatusTriggerName_CheckIn")
                .WithSimpleSchedule(m => m.WithIntervalInSeconds(AppSettings.MailServiceSection.CheckClientOnlineStatusJobInterval).RepeatForever())
                .StartNow()
                .Build();

            _scheduler.DeleteJob(job.Key);
            _scheduler.ScheduleJob(job, trigger);
        }

        public void Stop()
        {
            if (_scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }
    }
}