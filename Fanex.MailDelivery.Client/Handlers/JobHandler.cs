using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Handlers
{
    public interface IJobHandler
    {
        Task StartAsync();

        Task StopAsync();

        Task UpdateScheduleAsync(Schedule schedule);

        Task RetryJobAsync(Schedule schedule);

        Task RetryAllProcessAsync(Schedule schedule);

        Task RunAfterUMJobAsync(Schedule schedule, DateTimeOffset afterUMEndTime);

        Task InitializeSchedulerAsync();
    }

    public class JobHandler : IJobHandler
    {
        private IScheduler scheduler;

        public async Task InitializeSchedulerAsync()
        {
            if (AppSettings.MailCientConfig.HasSchedule())
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                scheduler = await schedFact.GetScheduler();
                await scheduler.Shutdown();

                scheduler = await schedFact.GetScheduler();
                await scheduler.Start();
            }
        }

        public async Task StartAsync()
        {
            if (AppSettings.MailCientConfig.HasSchedule())
            {
                await StartUpdateActiveTimeJob();

                var schedules = await MailClientHandler.GetSchedulesAsync();

                await StartJobsAsync(schedules);
            }
        }

        public async Task StopAsync()
        {
            if (scheduler != null)
            {
                await scheduler.Shutdown();
            }
        }

        public async Task RetryAllProcessAsync(Schedule schedule)
        {
            if (AppSettings.HasScheduleId(schedule.Id))
            {
                LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is triggered to start now from management site (mSite)");

                IJobDetail job = GetRetryJob(schedule);
                ITrigger trigger = GetRetryAllProcessTrigger(schedule);

                await scheduler.ScheduleJob(job, trigger);
            }
        }

        public async Task RetryJobAsync(Schedule schedule)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetRetryTrigger(schedule);

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task RunAfterUMJobAsync(Schedule schedule, DateTimeOffset afterUMEndTime)
        {
            IJobDetail job = GetRetryJob(schedule);
            ITrigger trigger = GetAfterUMTrigger(schedule, afterUMEndTime);

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            // If the schedule is belong to current Client.
            // We just manage the Schedule belong to us (in appconfig).
            if (AppSettings.HasScheduleId(schedule.Id))
            {
                var triggerKey = new TriggerKey(schedule.Id.ToString(), MailDeliveryConstants.MailCentralizedTriggerGroup);
                var oldTrigger = await scheduler.GetTrigger(triggerKey);

                if (oldTrigger != null)
                {
                    if (!schedule.Active)
                    {
                        LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is disabled");

                        await scheduler.UnscheduleJob(triggerKey);
                    }
                    else if (schedule.IsScheduleEnd())
                    {
                        LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is expired");

                        await scheduler.UnscheduleJob(triggerKey);
                    }
                    else
                    {
                        LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is updated the new schedule ({schedule.RepeatExpression})");

                        ITrigger newTrigger = GetTrigger(schedule);

                        await scheduler.RescheduleJob(triggerKey, newTrigger);
                    }
                }
                else
                {
                    if (!schedule.IsScheduleEnd())
                    {
                        LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is enabled and triggered at the schedule ({schedule.RepeatExpression})");

                        IJobDetail job = GetJobDetail(schedule);
                        ITrigger trigger = GetTrigger(schedule);

                        await scheduler.ScheduleJob(job, trigger);
                    }
                }
            }
        }

        private async Task StartJobsAsync(IList<Schedule> schedules)
        {
            if (schedules.Count == 0)
            {
                LogService.LogInfo("There is no any schedules to start.");
            }

            foreach (var schedule in schedules)
            {
                if (schedule != null && schedule.Active && !schedule.IsScheduleEnd())
                {
                    if (schedule.SiteId != AppSettings.MailCientConfig.SiteId)
                    {
                        var message = $"Only run for mails belonging the same site #{AppSettings.MailCientConfig.SiteId}. " +
                                        $"But mail #{schedule.Id} ({schedule.Name}) belongs another site #{schedule.SiteId}, it's ignored.";

                        LogService.LogError(message);
                    }
                    else
                    {
                        LogService.LogInfo($"Mail #{schedule.Id} ({schedule.Name}) is triggered by the schedule ({schedule.RepeatExpression})");

                        IJobDetail job = GetJobDetail(schedule);
                        ITrigger trigger = GetTrigger(schedule);

                        // Unschedule all redundant job.
                        await scheduler.UnscheduleJob(trigger.Key);

                        await scheduler.ScheduleJob(job, trigger);
                    }
                }
            }
        }

        private async Task StartUpdateActiveTimeJob()
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

            await scheduler.DeleteJob(job.Key);

            await scheduler.ScheduleJob(job, trigger);
        }

        private static IJobDetail GetJobDetail(Schedule schedule)
        {
            string jobName = string.Format(MailDeliveryConstants.AutoMailJobNameFormat, schedule.Id);

            // Define the job and tie it to our JobRequest class
            Type jobType = AppSettings.GetJobType(schedule.Id);
            IJobDetail jobDetail = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();

            return jobDetail;
        }

        private static JobDataMap GetJobDataMap(Schedule schedule)
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

        private static IJobDetail GetRetryJob(Schedule schedule)
        {
            schedule.ConvertToServerTime(MailClientSection.IsDevEnvironment());

            string jobName = string.Format(MailDeliveryConstants.AutoMailJobNameFormat, schedule.Id + Guid.NewGuid().ToString());

            // Define the job and tie it to our JobRequest class
            Type jobType = AppSettings.GetJobType(schedule.Id);

            IJobDetail job = JobBuilder.Create(jobType)
                .WithIdentity(jobName, MailDeliveryConstants.MailCentralizedTriggerGroup)
                .Build();

            return job;
        }

        private static ITrigger GetRetryAllProcessTrigger(Schedule schedule)
        {
            JobDataMap jobDataMap = GetJobDataMap(schedule);

            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartNow()
                .UsingJobData(jobDataMap)
                .Build();

            return retryTrigger;
        }

        private static ITrigger GetRetryTrigger(Schedule schedule)
        {
            schedule.ConvertToServerTime(MailClientSection.IsDevEnvironment());

            JobDataMap jobDataMap = GetJobDataMap(schedule);

            ITrigger retryTrigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.Now.AddMinutes(schedule.RetryInterval))
                .UsingJobData(jobDataMap)
                .Build();

            return retryTrigger;
        }

        private static ITrigger GetAfterUMTrigger(Schedule schedule, DateTimeOffset afterUMEndTime)
        {
            schedule.ConvertToServerTime(MailClientSection.IsDevEnvironment());

            JobDataMap jobDataMap = GetJobDataMap(schedule);

            ITrigger afterUMTrigger = TriggerBuilder.Create()
                .StartAt(afterUMEndTime)
                .UsingJobData(jobDataMap)
                .Build();

            return afterUMTrigger;
        }

        private static ITrigger GetTrigger(Schedule schedule)
        {
            schedule.ConvertToServerTime(MailClientSection.IsDevEnvironment());

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

        private static bool IsScheduleStartNow(Schedule schedule)
        {
            DateTimeOffset startTimeOffSet = new DateTimeOffset(schedule.StartTime);
            DateTimeOffset currentTimeOffSet = new DateTimeOffset(DateTime.Now);
            TimeSpan timeGap = startTimeOffSet - currentTimeOffSet;

            return timeGap.TotalMilliseconds <= 0;
        }
    }
}