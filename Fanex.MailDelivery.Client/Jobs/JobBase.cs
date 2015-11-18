using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Jobs
{
    using System;
    using Fanex.MailDelivery.Client.Handlers;
    using Quartz;

    /// <summary>
    /// Job Base abstract.
    /// </summary>
    public abstract class JobBase : IJob
    {
        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>
        /// The implementation may wish to set a  result object on the
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to
        /// <see cref="T:Quartz.IJobListener" />s or
        /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        /// execution.
        /// </remarks>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var schedule = context.MergedJobDataMap[MailDeliveryConstants.ScheduleJobDataKey] as Schedule;
                if (schedule != null && schedule.Active == true)
                {
                    int taskID = schedule.TaskId;
                    JobRequest jobResult = new JobRequest();
                    jobResult.JobStatus = JobStatus.Running;
                    jobResult.ScheduledTime = context.ScheduledFireTimeUtc.GetValueOrDefault().DateTime.ToLocalTime();
                    jobResult.RunningTime = context.FireTimeUtc.GetValueOrDefault().DateTime.ToLocalTime();
                    jobResult.Schedule = schedule.Clone();
                    if (schedule != null && schedule.TaskId == 0)
                    {
                        jobResult = MailClientHandler.RequestAddMailTaskForRunningJob(jobResult);
                        taskID = jobResult.Schedule.TaskId;
                    }

                    MailClientHandler.ReportJob(jobResult);
                    try
                    {
                        jobResult = Run(schedule, context);
                        if (jobResult != null && jobResult.Mail != null)
                        {
                            jobResult.Mail.MailingListId = schedule.Id;
                        }
                    }
                    catch (Exception ex)
                    {
                        jobResult.TaskLog = new TaskLog()
                        {
                            LogTime = DateTime.Now,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        jobResult.JobStatus = JobStatus.JobFailed;
                    }

                    jobResult.Schedule.TaskId = taskID;
                    if (jobResult.JobStatus != JobStatus.Success && jobResult.JobStatus != JobStatus.SuccessWithoutSendmail)
                    {
                        RetryJob(jobResult.Schedule, context.MergedJobDataMap);
                        jobResult.JobStatus = jobResult.Schedule.IsRunning() ? JobStatus.Running : jobResult.JobStatus;
                    }

                    MailClientHandler.ReportJob(jobResult);
                    if (jobResult.JobStatus != JobStatus.Running)
                    {
                        schedule.TaskId = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                BaseHandler.LogError(ex);
            }
        }

        protected abstract JobRequest Run(Schedule schedule, IJobExecutionContext jobContext);

        private void RetryJob(Schedule schedule, JobDataMap jobDataMap)
        {
            schedule.CurrentRetryTimes++;

            if (schedule.IsRunning())
            {
                MailClientHandler.RetryJob(schedule);
            }
        }
    }
}