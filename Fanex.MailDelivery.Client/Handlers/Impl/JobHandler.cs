namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Configurations;
    using Fanex.MailDelivery.Client.Handlers.Interface;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using Quartz.Impl;

    /// <summary>
    /// Handling job executing.
    /// </summary>
    public class JobHandler : IJobHandler
    {
        private IScheduler _scheduler;

        public void RetryAllProcess(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryAllProcessTrigger(schedule);

            // Tell quartz to schedule the job using our trigger
            _scheduler.ScheduleJob(job, trigger);
            Console.WriteLine(string.Format("Retry Schedule {0}", schedule.Id));
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

        public void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            _scheduler = schedFact.GetScheduler();
            _scheduler.Start();
            
            if (AppSettings.MailCientConfig.HasSchedule())
            {
                StartJobs(MailClientHandler.GetSchedules());
            }
        }

        public void StartJobs(List<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                IJobDetail job = GetJob(schedule);
                ITrigger trigger = GetTrigger(schedule);
                //// Tell quartz to schedule the job using our trigger
                _scheduler.ScheduleJob(job, trigger);
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
            var triggerKey = new TriggerKey(schedule.Name, MailDeliveryConstants.MailCentralizedTriggerGroup);
            var oldTrigger = _scheduler.GetTrigger(triggerKey);

            if (oldTrigger != null)
            {
                //// If schedule still active, then update schedule, opposite case then un-schedule job.
                if (schedule.Active)
                {
                    ITrigger newTrigger = GetTrigger(schedule);
                    _scheduler.RescheduleJob(triggerKey, newTrigger);
                    Console.WriteLine(string.Format("Update Schedule {0}", schedule.Id));
                }
                else
                {
                    _scheduler.UnscheduleJob(triggerKey);
                    Console.WriteLine(string.Format("Unschedule {0}", schedule.Id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Schedule {0} is null", schedule.Id));
            }
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
            var jobDataMap = GetJobDataMap(schedule);
            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.Now.AddSeconds(schedule.RetryIntervalInSeconds))
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
                .WithIdentity(schedule.Name, MailDeliveryConstants.MailCentralizedTriggerGroup)
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