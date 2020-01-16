using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;

namespace Fanex.MailDelivery.Client.Jobs
{
    public abstract class JobBase : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Schedule schedule = context.MergedJobDataMap[MailDeliveryConstants.ScheduleJobDataKey] as Schedule;

                if (schedule != null && schedule.Active)
                {
                    int taskId = schedule.TaskId;

                    JobMessage jobResult = new JobMessage();
                    jobResult.JobStatus = JobStatus.Running;
                    jobResult.ScheduledTime = context.ScheduledFireTimeUtc.GetValueOrDefault().DateTime.ToLocalTime();
                    jobResult.RunningTime = context.FireTimeUtc.GetValueOrDefault().DateTime.ToLocalTime();
                    jobResult.Schedule = schedule.Clone();

                    if (schedule.TaskId == 0)
                    {
                        jobResult = MailClientHandler.RequestAddMailTaskForRunningJob(jobResult);

                        taskId = jobResult.Schedule.TaskId;
                    }
                    else
                    {
                        var mailtask = MailClientHandler.RequestMailTaskById(new GetMailTaskByIdRequest() { TaskId = schedule.TaskId });

                        if (mailtask != null && mailtask.Status == (int)JobStatus.Abort)
                        {
                            // If the job is aborted, then shut down.
                            return;
                        }
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

                    jobResult.Schedule.TaskId = taskId;

                    if (jobResult.JobStatus != JobStatus.Success && jobResult.JobStatus != JobStatus.SuccessWithoutSendmail)
                    {
                        RetryJob(jobResult.Schedule);

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
                LogService.LogError(ex);
            }
        }

        protected abstract JobMessage Run(Schedule schedule, IJobExecutionContext jobContext);

        private void RetryJob(Schedule schedule)
        {
            schedule.CurrentRetryTimes++;

            if (schedule.IsRunning())
            {
                MailClientHandler.RetryJob(schedule);
            }
        }
    }
}