namespace Fanex.MailDelivery.Client.Jobs
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;

    /// <summary>
    /// Job Base.
    /// </summary>
    public abstract class JobBase : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var schedule = context.MergedJobDataMap[MailDeliveryConstants.ScheduleJobDataKey] as Schedule;
            if (schedule != null)
            {
                var jobResult = Run(schedule, context);

                MailClientHandler.SendAutoMail(jobResult);

                if (jobResult.JobStatus != JobStatus.Success)
                {
                    RetryJob(schedule.Clone(), context.MergedJobDataMap);
                }
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