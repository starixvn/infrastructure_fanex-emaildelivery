using Fanex.MailDelivery.Configurations;
using Quartz;
using Quartz.Impl;

namespace Fanex.MailDelivery.Services.Handlers.Impl
{
    /// <summary>
    /// Job Handler.
    /// </summary>
    public class JobHandler
    {
        private IScheduler _scheduler;

        public void Start()
        {
            if (AppSettings.MailServiceSection.IsRunAutoMailStatusReport)
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _scheduler = schedFact.GetScheduler();
                _scheduler.Start();
                StartCheckAutoMailStatusJob();
            }
        }

        public void Stop()
        {
            if (AppSettings.MailServiceSection.IsRunAutoMailStatusReport
                && _scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }

        /// <summary>
        /// Starts the check automatic mail service status.
        /// </summary>
        private void StartCheckAutoMailStatusJob()
        {
            IJobDetail job = JobBuilder.Create<CheckClientOnlineStatusJob>()
                    .WithIdentity(CheckClientOnlineStatusJob.CheckClientOnlineStatusJobName)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(CheckClientOnlineStatusJob.CheckClientOnlineStatusTriggerName)
                .WithSimpleSchedule(m => m.WithIntervalInSeconds(AppSettings.MailServiceSection.CheckClientOnlineStatusJobInterval).RepeatForever())
                .StartNow()
                .Build();

            _scheduler.DeleteJob(job.Key);
            _scheduler.ScheduleJob(job, trigger);
        }
    }
}