namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;
    using Fanex.MailDelivery.Client.Jobs;

    /// <summary>
    /// Handling job executing.
    /// </summary>
    public class JobHandler : BaseHandler, IJobHandler
    {
        private IScheduler _scheduler;

        public void RetryAllProcess(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryAllProcessTrigger(schedule);

            // Tell quartz to schedule the job using our trigger
            _scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Retries the job.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        public void RetryJob(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryTrigger(schedule);

            // Tell quartz to schedule the job using our trigger
            _scheduler.ScheduleJob(job, trigger);
        }

        public void AbortJob(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);

            // Tell quartz to schedule the job using our trigger
            _scheduler.Interrupt(job.Key);
        }

        public void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            _scheduler = schedFact.GetScheduler();
            _scheduler.Start();
            StartClientNotificationJob();
            if (AppSettings.MailCientConfig.HasSchedule())
            {
                StartJobs(MailClientHandler.GetSchedules());
            }
        }

        /// <summary>
        /// Starts the jobs.
        /// </summary>
        /// <param name="schedules">The schedules.</param>
        public void StartJobs(List<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                if (schedule != null && schedule.Active && !schedule.IsScheduleEnd())
                {
                    IJobDetail job = GetJob(schedule);
                    ITrigger trigger = GetTrigger(schedule);
                    //// Unschedule all redundant job.
                    _scheduler.UnscheduleJob(trigger.Key);
                    //// Tell quartz to schedule the job using our trigger.
                    _scheduler.ScheduleJob(job, trigger);
                    LogInfo(string.Format("Cron string: {0}", schedule.RepeatExpression));
                    if (schedule.SiteId != AppSettings.MailCientConfig.SiteId)
                    {
                        LogInfo(
                            string.Format(
                                "Current site id is {0}, but schedule site id is {1}",
                                AppSettings.MailCientConfig.SiteId,
                                schedule.SiteId));
                    }
                }
            }
        }

        public void Stop()
        {
            if (_scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }

        /// <summary>
        /// Updates the schedule.
        /// </summary>
        /// <param name="schedule">The schedule contains information need be updated.</param>
        public void UpdateSchedule(Schedule schedule)
        {
            // If the schedule is belong to current Client.
            // We just manage the Schedule belong to us (in appconfig).
            // Console.WriteLine("Update Schedule {0}: {1}", schedule.Id, schedule.RepeatExpression);
            if (AppSettings.HasScheduleId(schedule.Id))
            {
                var triggerKey = new TriggerKey(schedule.Id.ToString(), MailDeliveryConstants.MailCentralizedTriggerGroup);
                var oldTrigger = _scheduler.GetTrigger(triggerKey);

                if (oldTrigger != null)
                {
                    //// If schedule still active, then update schedule, opposite case then un-schedule job.
                    if (schedule.Active && !schedule.IsScheduleEnd())
                    {
                        ITrigger newTrigger = GetTrigger(schedule);
                        _scheduler.RescheduleJob(triggerKey, newTrigger);
                    }
                    else
                    {
                        _scheduler.UnscheduleJob(triggerKey);
                    }
                }
                else
                {
                    if (!schedule.IsScheduleEnd())
                    {
                        IJobDetail job = GetJob(schedule);
                        ITrigger trigger = GetTrigger(schedule);
                        //// Tell quartz to schedule the job using our trigger
                        _scheduler.ScheduleJob(job, trigger);
                    }
                }
            }
        }

        public void StartClientNotificationJob()
        {
            IJobDetail job = JobBuilder.Create<ClientNotificationJob>()
                .WithIdentity(MailDeliveryConstants.ClientNotificationJobName)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(MailDeliveryConstants.ClientNotificationTriggerName)
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(MailDeliveryConstants.ClientNotificationIntervalTimeInSecond).RepeatForever())
                .ForJob(MailDeliveryConstants.ClientNotificationJobName)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        private IJobDetail GetJob(Schedule schedule)
        {
            string jobName = GetJobName(schedule.Id);
            //// Define the job and tie it to our Job class
            var jobType = AppSettings.GetJobType(schedule.Id);
            IJobDetail job = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();
            return job;
        }

        /// <summary>
        /// Gets the data map with supply for running job.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns>Job data map.</returns>
        private JobDataMap GetJobDataMap(Schedule schedule)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap.Add(MailDeliveryConstants.ScheduleJobDataKey, schedule);

            var schedules = AppSettings.MailCientConfig.Schedules;

            foreach (ScheduleElement scheduleConfig in schedules)
            {
                if (schedule.Id == scheduleConfig.Id)
                {
                    foreach (JobInputDataElement jobData in scheduleConfig.JobInputData)
                    {
                        jobDataMap.Add(jobData.Name, jobData.Value);
                    }
                }
            }

            return jobDataMap;
        }

        private string GetJobName(int scheduleId)
        {
            return string.Format(MailDeliveryConstants.AutoMailJobNameFormat, scheduleId);
        }

        /// <summary>
        /// Gets the retry all process trigger.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns>Trigger details.</returns>
        private ITrigger GetRetryAllProcessTrigger(Schedule schedule)
        {
            var jobDataMap = GetJobDataMap(schedule);
            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartNow()
                .UsingJobData(jobDataMap)
                .Build();

            return retryTrigger;
        }

        /// <summary>
        /// Gets the retry job.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns>Job details.</returns>
        private IJobDetail GetRetryJob(Schedule schedule)
        {
            schedule.ConvertToServerTime();
            string jobName = string.Format(
                MailDeliveryConstants.AutoMailJobNameFormat,
                schedule.Id.ToString() + Guid.NewGuid().ToString());
            //// define the job and tie it to our Job class
            var jobType = AppSettings.GetJobType(schedule.Id);
            IJobDetail job = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();
            return job;
        }

        /// <summary>
        /// Gets the retry trigger.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns>Trigger details.</returns>
        private ITrigger GetRetryTrigger(Schedule schedule)
        {
            schedule.ConvertToServerTime();
            var jobDataMap = GetJobDataMap(schedule);
            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.Now.AddSeconds(schedule.RetryInterval * 60))
                .UsingJobData(jobDataMap)
                .Build();

            return retryTrigger;
        }

        /// <summary>
        /// Gets the trigger from schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns>Trigger starts job.</returns>
        private ITrigger GetTrigger(Schedule schedule)
        {
            schedule.ConvertToServerTime();
            var jobData = GetJobDataMap(schedule);

            TriggerBuilder triggerBuilder = TriggerBuilder.Create();

            //// Start Schedule
            if (IsScheduleStartNow(schedule))
            {
                triggerBuilder.StartNow();
            }
            else
            {
                triggerBuilder.StartAt(schedule.StartTime);
            }

            //// End Schedule
            if (schedule.EndTime != null)
            {
                triggerBuilder.EndAt(new DateTimeOffset((DateTime)schedule.EndTime));
            }

            ITrigger trigger = triggerBuilder
                .WithIdentity(schedule.Id.ToString(), MailDeliveryConstants.MailCentralizedTriggerGroup)
                .WithCronSchedule(schedule.RepeatExpression)
                .UsingJobData(jobData)
                .Build();
            return trigger;
        }

        /// <summary>
        /// Determines whether [is schedule start now or start in the future] [the specified schedule].
        /// </summary>
        /// <param name="schedule">The schedule for job.</param>
        /// <returns>Whether schedule should start now or not.</returns>
        private bool IsScheduleStartNow(Schedule schedule)
        {
            DateTimeOffset startTimeOffSet = new DateTimeOffset(schedule.StartTime);
            DateTimeOffset currentTimeOffSet = new DateTimeOffset(DateTime.Now);
            TimeSpan timeGap = startTimeOffSet - currentTimeOffSet;

            return timeGap.TotalMilliseconds <= 0;
        }
    }
}