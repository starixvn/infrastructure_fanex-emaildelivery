using Fanex.MailDeliver.Business;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Services.Services.MailService
{
    public interface IMailService
    {
        Task UpdateJobStatus(JobMessage jobMessage);

        Task SendMail(Mail mail, int mailTaskId = 0);

        void AddMailTaskForRunningJob(JobMessage jobMessage);

        MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request);

        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);
    }

    public class MailService : IMailService
    {
        private readonly IMailRepository mailRepository;
        private readonly IMailDeliverBusiness mailDeliverBusiness;
        private readonly LogService logService;
        private readonly int sendMailRetryLimit;
        private readonly int sendMailRetryIntervalInSeconds;

        public MailService(int sendMailRetryLimit, int sendMailRetryIntervalInSeconds) : this(null, sendMailRetryLimit, sendMailRetryIntervalInSeconds)
        {
        }

        public MailService(LogService logService, int sendMailRetryLimit, int sendMailRetryIntervalInSeconds)
        {
            this.logService = logService ?? new LogServiceImpl();
            mailRepository = new MailRepository();
            mailDeliverBusiness = new MailDeliverBusiness();
            this.sendMailRetryIntervalInSeconds = sendMailRetryIntervalInSeconds;
            this.sendMailRetryLimit = sendMailRetryLimit;
        }

        public GetScheduleListResponse GetScheduleList(GetScheduleListRequest request)
        {
            var schedules = new List<Schedule>();

            foreach (int mail in request.MailIds)
            {
                Schedule schedule = mailRepository.GetScheduleByMailId(new GetScheduleByMailIdCriteria
                {
                    MailId = mail
                });

                if (schedule != null)
                {
                    schedules.Add(schedule);
                }
            }

            return new GetScheduleListResponse { Schedules = schedules };
        }

        public void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest)
        {
            try
            {
                var criteria = new UpdateClientActiveTimeCriteria
                {
                    MailIds = updateActiveTimeRequest.MailIds
                };

                mailRepository.UpdateClientActiveTime(criteria);
            }
            catch (Exception ex)
            {
                logService.LogError(ex);
            }
        }

        public async Task UpdateJobStatus(JobMessage jobMessage)
        {
            try
            {
                if (jobMessage.TaskLog != null)
                {
                    mailRepository.AddMailLog(new AddMailLogCriteria
                    {
                        LogTime = jobMessage.TaskLog.LogTime,
                        ExceptionMessage = jobMessage.TaskLog.Message,
                        StackTrace = jobMessage.TaskLog.StackTrace,
                        Status = (int)jobMessage.JobStatus,
                        TaskId = jobMessage.Schedule.TaskId
                    });
                }

                switch (jobMessage.JobStatus)
                {
                    case JobStatus.Running:
                        UpdateRunningJob(jobMessage);
                        break;

                    case JobStatus.DataIsNotReady:
                        UpdateDataNotReadyJob(jobMessage);
                        break;

                    case JobStatus.Success:
                        await SendMailForSuccessfulJob(jobMessage);
                        break;

                    case JobStatus.SuccessWithoutSendmail:
                        UpdateSuccessJobWithoutSendMail(jobMessage);
                        break;

                    case JobStatus.JobFailed:
                        UpdateFailedJob(jobMessage);
                        break;

                    case JobStatus.Abort:
                        UpdateAbortJob(jobMessage);
                        break;

                    default:
                        // Do nothing
                        break;
                }
            }
            catch (Exception ex)
            {
                logService.LogError(ex, jobMessage.Schedule);
            }
        }

        public void AddMailTaskForRunningJob(JobMessage jobMessage)
        {
            try
            {
                jobMessage.Schedule.TaskId = mailRepository.AddMailTask(new AddMailTaskCriteria
                {
                    MailId = jobMessage.Schedule.Id,
                    SiteId = jobMessage.Schedule.SiteId,
                    ScheduledTime = jobMessage.ScheduledTime,
                    Status = (int)JobStatus.Running,
                    Attempt = jobMessage.Schedule.CurrentRetryTimes
                });
            }
            catch (Exception ex)
            {
                logService.LogError(ex, jobMessage.Schedule);
            }
        }

        public MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request)
        {
            MailTaskModel result = null;

            try
            {
                result = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria
                {
                    MailTaskId = request.TaskId
                });
            }
            catch (Exception ex)
            {
                logService.LogError(ex, request.TaskId);

                result = new MailTaskModel();
            }

            return result;
        }

        public async Task SendMail(Mail mail, int mailTaskId = 0)
        {
            var mailRecordId = 0;
            Schedule schedule = null;
            int taskId = mailTaskId;
            var logger = new StringBuilder($"Server Logs:{Environment.NewLine}");
            string message = string.Empty;

            try
            {
                schedule = mailRepository.GetScheduleByMailId(new GetScheduleByMailIdCriteria { MailId = mail.MailingListId });

                if (schedule == null)
                {
                    throw new InvalidOperationException($"Mail #{mail.MailingListId} is not found.");
                }
                else if (schedule.Active)
                {
                    var mailRecord = ConvertFromMailToMailRecord(mail);

                    ValidateMail(mailRecord);

                    message = $"Inserting a mail record for the mail #{mail.MailingListId}";
                    logger.AppendLine($"> {message}");
                    logService.LogInfo(message);

                    mailRecordId = (int)mailDeliverBusiness.InsertMailRecord(mailRecord);

                    if (schedule.TriggerTypeEnum == MailType.Function)
                    {
                        message = $"Inserting the task for the function mail #{mail.MailingListId}";
                        logger.AppendLine($"> {message}");
                        logService.LogInfo(message);

                        taskId = InsertMailTaskForFunctionMail(mail, mailRecordId);
                    }

                    if (mailRecordId > 0)
                    {
                        message = $"Sending out the mail record #{mailRecordId} (mail #{mail.MailingListId})";
                        logger.AppendLine($"> {message}");
                        logService.LogInfo(message);

                        mailRecord.IsReady = true;
                        mailRecord.MailID = mailRecordId;
                        mailRecord.ResentCount = sendMailRetryLimit;

                        bool isSuccessful = await SendMailRecord(mailRecord, mail.MailingListId, logger);

                        var status = isSuccessful ? JobStatus.Success : JobStatus.JobFailed;
                        var taskLog = new TaskLog
                        {
                            LogTime = DateTime.Now,
                            Message = logger.ToString()
                        };

                        InsertOrUpdateMailTaskAndLog(taskId, mail, mailRecordId, status, taskLog);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Failed to store a mail record for the mail #{mail.MailingListId}.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Mail #{mail.MailingListId} is disabled.");
                }
            }
            catch (Exception ex)
            {
                logService.LogError(ex, schedule, mail.MailingListId);

                InsertOrUpdateMailTaskAndLog(
                    taskId,
                    mail,
                    mailRecordId,
                    JobStatus.JobFailed,
                    new TaskLog
                    {
                        LogTime = DateTime.Now,
                        Message = $"{logger}{Environment.NewLine}There is an error -> {ex.InnerException?.Message ?? ex.Message}",
                        StackTrace = ex.StackTrace
                    });
            }
        }

        private int InsertMailTaskForFunctionMail(Mail mail, int mailRecordId)
        {
            int taskId = mailRepository.AddMailTask(new AddMailTaskCriteria
            {
                MailId = mail.MailingListId,
                SiteId = mail.SiteId,
                ScheduledTime = DateTime.Now,
                Status = (int)JobStatus.Running,
                MSiteMailId = mailRecordId
            });

            return taskId;
        }

        private void InsertOrUpdateMailTaskAndLog(
            int mailTaskId,
            Mail mail,
            int mailRecordId,
            JobStatus status,
            TaskLog taskLog = null)
        {
            try
            {
                int taskId = mailTaskId;

                if (taskId == 0)
                {
                    taskId = InsertMailTaskForFunctionMail(mail, mailRecordId);
                }

                if (taskId > 0)
                {
                    var updateTaskCriteria = new UpdateMailTaskStatusCriteria
                    {
                        TaskId = taskId,
                        Status = (int)status
                    };

                    if (status == JobStatus.Success)
                    {
                        updateTaskCriteria.LastSent = DateTime.Now;
                    }

                    mailRepository.UpdateMailTaskStatus(updateTaskCriteria);

                    if (mailRecordId > 0)
                    {
                        mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria
                        {
                            TaskId = taskId,
                            MSiteMailId = mailRecordId
                        });
                    }

                    if (taskLog != null)
                    {
                        mailRepository.AddMailLog(new AddMailLogCriteria
                        {
                            LogTime = taskLog.LogTime,
                            ExceptionMessage = taskLog.Message,
                            StackTrace = taskLog.StackTrace,
                            Status = (int)status,
                            TaskId = taskId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogError(ex);
            }
        }

        private async Task<bool> SendMailRecord(MailRecord mailRecord, int mailId, StringBuilder logger)
        {
            bool isSuccessful = true;
            string error = string.Empty;
            string message = string.Empty;

            if (mailRecord != null)
            {
                MailExtension.SendMail(mailRecord, out error);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    isSuccessful = false;
                    message = $"Failed to send the mail record #{mailRecord.MailID} (mail #{mailId}) -> {error}";
                    logger.AppendLine($"> {message}");
                    logService.LogError(message);

                    if (mailRecord.ResentCount > 0)
                    {
                        message = $"Retry to send the mail record #{mailRecord.MailID} (mail #{mailId}) in {sendMailRetryIntervalInSeconds} seconds...";
                        logger.AppendLine($"> {message}");
                        logService.LogInfo(message);

                        await Task.Delay(TimeSpan.FromSeconds(sendMailRetryIntervalInSeconds));

                        mailRecord.ResentCount--;

                        isSuccessful = await SendMailRecord(mailRecord, mailId, logger);
                    }
                    else
                    {
                        message = $"Sending the mail record #{mailRecord.MailID} (mail #{mailId}) exceeded maximum retrying time({sendMailRetryLimit})";

                        logger.AppendLine($"> {message}");
                        logService.LogError(message);
                    }
                }
                else
                {
                    isSuccessful = true;

                    message = $"Succeeded to send the mail record #{mailRecord.MailID} (mail #{mailId})";
                    logger.AppendLine($"> {message}");
                    logService.LogError(message);
                }
            }

            return isSuccessful;
        }

        private static void ValidateMail(MailRecord mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Subject))
            {
                throw new ArgumentNullException("mail.Subject", "Mail subject cannot be null or empty.");
            }
        }

        private void UpdateRunningJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Running
            });

            mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                RetryCount = jobMessage.Schedule.CurrentRetryTimes
            });
        }

        private void UpdateAbortJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Abort
            });
        }

        private void UpdateDataNotReadyJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus
            });
        }

        private async Task SendMailForSuccessfulJob(JobMessage jobMessage)
        {
            var mailTask = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria { MailTaskId = jobMessage.Schedule.TaskId });

            if (mailTask.Status != (int)JobStatus.Abort)
            {
                await SendMail(jobMessage.Mail, jobMessage.Schedule.TaskId);
            }
            else
            {
                string message = $"Not allow to send out the mail #{jobMessage.Schedule.Id} ({jobMessage.Schedule.Name}) because it is aborted by user";

                logService.LogInfo(message);
            }
        }

        private void UpdateSuccessJobWithoutSendMail(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the JobRequest cannot be update to success when aborted. We will check Status again to make sure JobRequest is done.
            var mailTask = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria { MailTaskId = jobMessage.Schedule.TaskId });

            if (mailTask.Status == (int)jobMessage.JobStatus)
            {
                mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria
                {
                    TaskId = jobMessage.Schedule.TaskId,
                    MSiteMailId = 0
                });
            }
        }

        private void UpdateFailedJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.JobFailed
            });
        }

        private MailRecord ConvertFromMailToMailRecord(Mail mail)
        {
            var mailingList = mailRepository.GetMailingList(new GetMailWithEmailAddressByMailId { MailId = mail.MailingListId });

            var mailRecord = new MailRecord
            {
                AppID = mail.ApplicationSendMailId,
                Config = new MailConfig
                {
                    AppID = mail.ApplicationSendMailId
                },
                Body = mail.Body,
                Subject = mail.Subject,
                Title = mail.Title,
                MailTo = string.Join(";", mailingList.TOs),
                MailCc = string.Join(";", mailingList.CCs),
                MailBcc = string.Join(";", mailingList.BCCs)
            };

            TransferAttachmentsToMailRecord(mail, mailRecord);

            TransferAlternateViewToMailRecord(mail, mailRecord);

            return mailRecord;
        }

        private static void TransferAlternateViewToMailRecord(Mail sourceMail, MailRecord destinationMailRecord)
        {
            if (sourceMail.AlternativeViews != null && sourceMail.AlternativeViews.Count > 0)
            {
                int alternativeViewsCount = sourceMail.AlternativeViews.Count;

                for (int i = 0; i < alternativeViewsCount; i++)
                {
                    var alternativeView = sourceMail.AlternativeViews[i];

                    destinationMailRecord.AlternateView.Add(new MailDeliver.Model.MailLinkedResource
                    {
                        ContentCID = alternativeView.ContentId,
                        FileName = alternativeView.FileName,
                        ImageType = (FileExtentions)((int)alternativeView.FileExtension),
                        LinkedID = alternativeView.Id,
                        MailID = alternativeView.MailId,
                        StreamContent = new MemoryStream(alternativeView.FileContent)
                    });
                }
            }
        }

        private static void TransferAttachmentsToMailRecord(Mail sourceMail, MailRecord destinationMailRecord)
        {
            if (sourceMail.Attachments != null && sourceMail.Attachments.Count > 0)
            {
                int mailAttachCount = sourceMail.Attachments.Count;

                for (int i = 0; i < mailAttachCount; i++)
                {
                    var attach = sourceMail.Attachments[i];

                    destinationMailRecord.Attachments.Add(new MailDeliver.Model.MailAttachment
                    {
                        AttachmentID = attach.Id,
                        Content = new MemoryStream(attach.FileContent),
                        ContentType = (FileExtentions)((int)attach.FileExtension),
                        FileName = attach.FileName,
                        MailID = attach.MailId
                    });
                }
            }
        }
    }
}