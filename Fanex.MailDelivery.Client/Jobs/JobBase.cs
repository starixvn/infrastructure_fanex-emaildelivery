using Fanex.MailDelivery.Client.Logging;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Quartz;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Jobs
{
    public abstract class JobBase : IJob
    {
        private StringBuilder logger;

        public async Task Execute(IJobExecutionContext context)
        {
            Schedule schedule = null;
            logger = new StringBuilder($"Client Logs:{Environment.NewLine}");

            try
            {
                schedule = context.MergedJobDataMap[MailDeliveryConstants.ScheduleJobDataKey] as Schedule;

                if (schedule != null && !schedule.Active)
                {
                    LogService.LogInfo($"Scheduled Mail #{schedule.Id} ({schedule.Name}) is disabled.");
                }
                else if (schedule != null && schedule.Active)
                {
                    string message = $"Scheduled Mail #{schedule.Id} ({schedule.Name}) is running...";

                    logger.AppendLine($"> {message}");
                    LogService.LogInfo(message);

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

                        message = $"Added a running task for the mail #{schedule.Id} ({schedule.Name})";

                        logger.AppendLine($"> {message}");
                        LogService.LogInfo(message);
                    }

                    if (schedule.TaskId == 0)
                    {
                        throw new InvalidOperationException($"Cannot add a running mail task for the scheduled mail #{schedule.Id} ({schedule.Name}).");
                    }
                    else
                    {
                        message = $"Running the task #{schedule.TaskId} of the mail #{schedule.Id} ({schedule.Name})";

                        logger.AppendLine($"> {message}");
                        LogService.LogInfo(message);
                    }

                    var mailtask = await MailClientHandler.RequestMailTaskByIdAsync(new GetMailTaskByIdRequest
                    {
                        TaskId = schedule.TaskId
                    });

                    if (mailtask != null && mailtask.Status != (int)JobStatus.Abort)
                    {
                        await HandleJob(context, schedule, jobResult);
                    }
                    else
                    {
                        message = $"Cannot run job to generate mail #{schedule.Id} ({schedule.Name}) because mail task #{schedule.TaskId} is aborted by user";

                        logger.AppendLine($"> {message}");
                        LogService.LogInfo(message);
                    }
                }
                else
                {
                    //Do nothing
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
            string message = $"Running job to generate the mail #{schedule.Id} ({schedule.Name})";

            logger.AppendLine($"> {message}");
            LogService.LogInfo(message);

            await MailClientHandler.ReportJobAsync(jobResult);

            jobResult = RunJob(context, schedule, jobResult);

            jobResult.Schedule = schedule;

            if (jobResult.JobStatus == JobStatus.Success || jobResult.JobStatus == JobStatus.SuccessWithoutSendmail)
            {
                message = $"Generated the mail #{schedule.Id} ({schedule.Name}) successfully";

                logger.AppendLine($"> {message}");
                LogService.LogInfo(message);
            }
            else
            {
                message = $"The task status of the mail #{schedule.Id} ({schedule.Name}) is {jobResult.JobStatus.ToString()}";
                logger.AppendLine($"> {message}");
                LogService.LogInfo(message);

                await RetryJobAsync(jobResult.Schedule);
            }

            if (jobResult.TaskLog != null)
            {
                jobResult.TaskLog.Message = logger.ToString();
            }
            else
            {
                jobResult.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = logger.ToString(),
                    StackTrace = string.Empty
                };
            }

            await MailClientHandler.ReportJobAsync(jobResult);

            if (jobResult.JobStatus == JobStatus.Success || jobResult.JobStatus == JobStatus.SuccessWithoutSendmail)
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
                LogService.LogError(schedule, ex);

                jobResult.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace
                };

                jobResult.JobStatus = JobStatus.JobFailed;
                logger.AppendLine($"> Generating mail failed -> {jobResult.TaskLog.Message}");
            }

            return jobResult;
        }

        protected abstract JobMessage Run(Schedule schedule, IJobExecutionContext jobContext);

        private async Task RetryJobAsync(Schedule schedule)
        {
            schedule.CurrentRetryTimes++;
            string message = string.Empty;

            if (schedule.MaxRetryTimes == 0)
            {
                message = $"The mail #{schedule.Id} ({schedule.Name}) is not allowed to retry in its setting.";

                logger.AppendLine($"> {message}");
                LogService.LogInfo(message);
            }
            else if (schedule.CanRetry())
            {
                message = $"Retry({schedule.CurrentRetryTimes}) to generate the mail #{schedule.Id} ({schedule.Name}) in {schedule.RetryInterval} minutes...";

                logger.AppendLine($"> {message}");
                LogService.LogInfo(message);

                await MailClientHandler.RetryJobAsync(schedule);
            }
            else
            {
                message = $"Exceeded maximum retrying time({schedule.MaxRetryTimes}) for the mail #{schedule.Id} ({schedule.Name})";

                logger.AppendLine($"> {message}");
                LogService.LogInfo(message);
            }
        }
    }
}