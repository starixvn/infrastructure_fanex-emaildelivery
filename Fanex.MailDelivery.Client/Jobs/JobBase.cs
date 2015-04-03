namespace Fanex.MailDelivery.Client.Jobs
{
    using System;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
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
                Logging.Logger.Log.Info(string.Format("{0} ScheduleID:{1} Cron Exp: {2} is running at {3}",schedule.Name, schedule.Id, schedule.RepeatExpression, DateTime.Now));
                if (schedule != null)
                {
                    int taskID = schedule.TaskId;
                    JobResultMessage jobResult = new JobResultMessage();
                    jobResult.JobStatus = JobStatus.Running;
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
                        jobResult.Mail.MailingListId = schedule.Id;
                    }
                    catch (Exception ex)
                    {
                        jobResult.TaskLog = new TaskLog()
                        {
                            LogTime = DateTime.Now,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                    }

                    jobResult.Schedule.TaskId = taskID;
                    if (jobResult.JobStatus != JobStatus.Success)
                    {
                        RetryJob(jobResult.Schedule, context.MergedJobDataMap);
                        jobResult.JobStatus = jobResult.Schedule.IsRunning() ? JobStatus.Running : JobStatus.DataIsNotReady;
                    }

                    MailClientHandler.ReportJob(jobResult);
                }
            }
            catch (Exception ex)
            {
                BaseHandler.LogError(ex);
            }
        }

        protected abstract JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext);

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