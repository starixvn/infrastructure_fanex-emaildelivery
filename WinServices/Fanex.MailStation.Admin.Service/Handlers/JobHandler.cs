using Fanex.MailDelivery.Services.Configurations;
using Quartz;
using Quartz.Impl;

namespace Fanex.MailStation.Admin.Service.Handlers
{
    /// <summary>
    /// JobRequest Handler.
    /// </summary>
    public class JobHandler
    {
        private IScheduler _scheduler;

        public void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            _scheduler = schedFact.GetScheduler();
            _scheduler.Start();
            StartCheckAutoMailStatusJob();
        }

        public void Stop()
        {
            if (_scheduler != null)
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