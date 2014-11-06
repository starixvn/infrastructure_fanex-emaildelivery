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
                if (schedule != null)
                {
                    JobResultMessage jobResult = null;
                    try
                    {
                        jobResult = Run(schedule, context);
                    }
                    catch (Exception ex)
                    {
                        BaseHandler.LogError(ex);
                        jobResult = new JobResultMessage
                        {
                            Schedule = schedule,
                            JobStatus = JobStatus.Failed,
                            Result = new ResultMessage
                            {
                                ErrCode = MailDeliveryConstants.JobFailedErrorCode,
                                ErrMsg = ex.ToString()
                            }
                        };
                    }

                    MailClientHandler.ReportJob(jobResult);

                    if (jobResult.JobStatus != JobStatus.Success)
                    {
                        RetryJob(schedule.Clone(), context.MergedJobDataMap);
                    }
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