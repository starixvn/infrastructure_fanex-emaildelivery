namespace Fanex.MailDelivery.Handlers.Impl
{
    using Fanex.MailDelivery.Configurations;
    using Quartz;
    using Quartz.Impl;

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
        /// Starts the check automatic mail status job.
        /// </summary>
        private void StartCheckAutoMailStatusJob()
        {
            IJobDetail job = JobBuilder.Create<CheckMailStatusJob>()
                    .WithIdentity(CheckMailStatusJob.CheckAutoMailStatusJobName)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(CheckMailStatusJob.CheckAutoMailStatusJobTriggerName)
                .StartNow()
                .WithCronSchedule(AppSettings.MailServiceSection.CheckAutoMailStatusSchedule)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Starts the insert automatic mail record job.
        /// </summary>
        private void StartInsertAutoMailRecordJob()
        {
            IJobDetail job = JobBuilder.Create<InsertAutoMailRecordJob>()
                    .WithIdentity(InsertAutoMailRecordJob.InsertAutoMailRecordJobName)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(InsertAutoMailRecordJob.InsertAutoMailRecordJobTriggerName)
                .StartNow()
                .WithCronSchedule(AppSettings.MailServiceSection.InsertMailStatusRecordSchedule)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}