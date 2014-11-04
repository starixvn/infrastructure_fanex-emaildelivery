// -----------------------------------------------------------------------
// <copyright file="JobHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Quartz;
    using Quartz.Impl;
    using Fanex.MailDelivery.Configurations;

    /// <summary>
    /// TODO: Update summary.
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
                StartInsertAutoMailRecordJob();
                StartCheckAutoMailStatusJob();
            }
        }

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

        private void StartCheckAutoMailStatusJob()
        {
            IJobDetail job = JobBuilder.Create<CheckAutoMailStatusJob>()
                    .WithIdentity(CheckAutoMailStatusJob.CheckAutoMailStatusJobName)
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(CheckAutoMailStatusJob.CheckAutoMailStatusJobTriggerName)
                .StartNow()
                .WithCronSchedule(AppSettings.MailServiceSection.CheckAutoMailStatusSchedule)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        public void Stop()
        {
            if (AppSettings.MailServiceSection.IsRunAutoMailStatusReport 
                && _scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }
    }
}
