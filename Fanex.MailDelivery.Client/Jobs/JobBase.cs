using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Jobs
{
    public abstract class JobBase : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Schedule schedule = null;

            try
            {
                schedule = context.MergedJobDataMap[MailDeliveryConstants.ScheduleJobDataKey] as Schedule;

                if (schedule != null && schedule.Active)
                {
                    JobMessage jobResult = new JobMessage
                    {
                        JobStatus = JobStatus.Running,
                        ScheduledTime = context.ScheduledFireTimeUtc.GetValueOrDefault().DateTime.ToLocalTime(),
                        RunningTime = context.FireTimeUtc.DateTime.ToLocalTime(),
                        Schedule = schedule.Clone()
                    };

                    if (schedule.TaskId == 0)
                    {
                        jobResult = await MailClientHandler.RequestAddMailTaskForRunningJobAsync(jobResult);

                        schedule.TaskId = jobResult.Schedule.TaskId;
                    }

                    if (schedule.TaskId == 0)
                    {
                        throw new InvalidOperationException($"Cannot add a new mail task for the scheduled mail.");
                    }

                    var mailtask = await MailClientHandler.RequestMailTaskByIdAsync(new GetMailTaskByIdRequest
                    {
                        TaskId = schedule.TaskId
                    });

                    if (mailtask != null && mailtask.Status != (int)JobStatus.Abort)
                    {
                        await HandleJob(context, schedule, jobResult);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(schedule, ex);
            }
        }

        private async Task HandleJob(IJobExecutionContext context, Schedule schedule, JobMessage jobMessage)
        {
            JobMessage jobResult = jobMessage;

            await MailClientHandler.ReportJobAsync(jobResult);

            jobResult = RunJob(context, schedule, jobResult);

            jobResult.Schedule = schedule;

            if (jobResult.JobStatus != JobStatus.Success && jobResult.JobStatus != JobStatus.SuccessWithoutSendmail)
            {
                jobResult.JobStatus = jobResult.Schedule.IsRunning() ? JobStatus.Running : jobResult.JobStatus;

                await RetryJobAsync(jobResult.Schedule);
            }

            await MailClientHandler.ReportJobAsync(jobResult);

            if (jobResult.JobStatus != JobStatus.Running)
            {
                schedule.TaskId = 0;
            }
        }

        private JobMessage RunJob(IJobExecutionContext context, Schedule schedule, JobMessage jobMessage)
        {
            JobMessage jobResult = jobMessage;

            try
            {
                var mailJobResult = Run(schedule, context);

                if (mailJobResult == null || mailJobResult.Mail == null)
                {
                    throw new InvalidOperationException($"Mail information cannot be null");
                }
                else
                {
                    jobResult = mailJobResult;
                    jobResult.Mail.MailingListId = schedule.Id;
                }
            }
            catch (Exception ex)
            {
                jobResult.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };

                jobResult.JobStatus = JobStatus.JobFailed;
            }

            return jobResult;
        }

        protected abstract JobMessage Run(Schedule schedule, IJobExecutionContext jobContext);

        private static async Task RetryJobAsync(Schedule schedule)
        {
            schedule.CurrentRetryTimes++;

            if (schedule.IsRunning())
            {
                await MailClientHandler.RetryJobAsync(schedule);
            }
        }
    }
}