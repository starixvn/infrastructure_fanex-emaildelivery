using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Client.Handlers
{
    public interface IJobHandler
    {
        void Start();

        void Stop();

        void StartJobs(IList<Schedule> schedules);

        void UpdateSchedule(Schedule schedule);

        void RetryJob(Schedule schedule);

        void RetryAllProcess(Schedule schedule);
    }

    public class JobHandler : IJobHandler
    {
        private IScheduler scheduler;

        public void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            scheduler = schedFact.GetScheduler();
            scheduler.Start();

            StartUpdateActiveTimeJob();

            if (AppSettings.MailCientConfig.HasSchedule())
            {
                StartJobs(MailClientHandler.GetSchedules());
            }
        }

        public void Stop()
        {
            if (scheduler != null)
            {
                scheduler.Shutdown();
            }
        }

        public void StartJobs(IList<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                if (schedule != null && schedule.Active && !schedule.IsScheduleEnd())
                {
                    IJobDetail job = GetJobDetail(schedule);
                    ITrigger trigger = GetTrigger(schedule);

                    // Unschedule all redundant job.
                    scheduler.UnscheduleJob(trigger.Key);

                    // Tell quartz to schedule the job using our trigger.
                    scheduler.ScheduleJob(job, trigger);

                    LogService.LogInfo(string.Format("Cron string: {0}", schedule.RepeatExpression));

                    if (schedule.SiteId != AppSettings.MailCientConfig.SiteId)
                    {
                        LogService.LogInfo(string.Format("Current site id is {0}, but schedule site id is {1}", AppSettings.MailCientConfig.SiteId, schedule.SiteId));
                    }
                }
            }
        }

        public void RetryAllProcess(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryAllProcessTrigger(schedule);

            scheduler.ScheduleJob(job, trigger);
        }

        public void RetryJob(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryTrigger(schedule);

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);
        }

        public void AbortJob(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);

            // Tell quartz to schedule the job using our trigger
            scheduler.Interrupt(job.Key);
        }

        public void UpdateSchedule(Schedule schedule)
        {
            // If the schedule is belong to current Client.
            // We just manage the Schedule belong to us (in appconfig).
            // Console.WriteLine("Update Schedule {0}: {1}", schedule.Id, schedule.RepeatExpression);
            if (AppSettings.HasScheduleId(schedule.Id))
            {
                var triggerKey = new TriggerKey(schedule.Id.ToString(), MailDeliveryConstants.MailCentralizedTriggerGroup);
                var oldTrigger = scheduler.GetTrigger(triggerKey);

                if (oldTrigger != null)
                {
                    //// If schedule still active, then update schedule, opposite case then un-schedule job.
                    if (schedule.Active && !schedule.IsScheduleEnd())
                    {
                        ITrigger newTrigger = GetTrigger(schedule);
                        scheduler.RescheduleJob(triggerKey, newTrigger);
                    }
                    else
                    {
                        scheduler.UnscheduleJob(triggerKey);
                    }
                }
                else
                {
                    if (!schedule.IsScheduleEnd())
                    {
                        IJobDetail job = GetJobDetail(schedule);
                        ITrigger trigger = GetTrigger(schedule);
                        //// Tell quartz to schedule the job using our trigger
                        scheduler.ScheduleJob(job, trigger);
                    }
                }
            }
        }

        private void StartUpdateActiveTimeJob()
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

            scheduler.DeleteJob(job.Key);
            scheduler.ScheduleJob(job, trigger);
        }

        private IJobDetail GetJobDetail(Schedule schedule)
        {
            string jobName = string.Format(MailDeliveryConstants.AutoMailJobNameFormat, schedule.Id);

            // Define the job and tie it to our JobRequest class
            Type jobType = AppSettings.GetJobType(schedule.Id);
            IJobDetail jobDetail = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();
            return jobDetail;
        }

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

        private IJobDetail GetRetryJob(Schedule schedule)
        {
            schedule.ConvertToServerTime(MailClientSection.GetConfig().IsLocalEnvironment());

            string jobName = string.Format(MailDeliveryConstants.AutoMailJobNameFormat, schedule.Id + Guid.NewGuid().ToString());

            // Define the job and tie it to our JobRequest class
            Type jobType = AppSettings.GetJobType(schedule.Id);

            IJobDetail job = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();

            return job;
        }

        private ITrigger GetRetryAllProcessTrigger(Schedule schedule)
        {
            JobDataMap jobDataMap = GetJobDataMap(schedule);

            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartNow()
                .UsingJobData(jobDataMap)
                .Build();

            return retryTrigger;
        }

        private ITrigger GetRetryTrigger(Schedule schedule)
        {
            schedule.ConvertToServerTime(MailClientSection.GetConfig().IsLocalEnvironment());

            JobDataMap jobDataMap = GetJobDataMap(schedule);

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
            schedule.ConvertToServerTime(MailClientSection.GetConfig().IsLocalEnvironment());

            JobDataMap jobData = GetJobDataMap(schedule);

            TriggerBuilder triggerBuilder = TriggerBuilder.Create();

            // Start Schedule
            if (IsScheduleStartNow(schedule))
            {
                triggerBuilder.StartNow();
            }
            else
            {
                triggerBuilder.StartAt(schedule.StartTime);
            }

            // End Schedule
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