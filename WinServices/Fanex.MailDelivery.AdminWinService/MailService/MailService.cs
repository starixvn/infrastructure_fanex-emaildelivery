using System;
using Fanex.MailDeliver.Business;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDeliver.Core.Infrastructure.Logging;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services.Logging;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.AdminWinService.MailService
{
    /// <summary>
    /// Mail Service.
    /// </summary>
    public class MailService : IMailService
    {
        private IMailRepository _mailRepository;
        private IMailDeliverBusiness _mailDeliverBusiness;

        public MailService()
        {
            this._mailRepository = this._mailRepository ?? new MailRepository();
            this._mailDeliverBusiness = new MailDeliverBusiness();
        }

        /// <summary>
        /// Updates the JobRequest status.
        /// Just incase of status is Running, Retry, DataIsNotReady using this function.
        /// </summary>
        /// <param name="jobRequestJobRequest result message.</param>
        /// <returns>
        /// Command Result.
        /// </returns>
        public CommandResult UpdateJobStatus(JobMessage jobMessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (jobMessage.JobStatus)
                {
                    case JobStatus.Running:
                        this.UpdateRunningJob(jobMessage);
                        break;
                    case JobStatus.DataIsNotReady:
                        this.UpdateDataNotReadyJob(jobMessage);
                        break;
                    case JobStatus.Success:
                        this.UpdateSuccessJob(jobMessage);
                        break;
                    case JobStatus.SuccessWithoutSendmail:
                        this.UpdateSuccessJobWithoutSendMail(jobMessage);
                        break;
                    case JobStatus.JobFailed:
                        this.UpdateFailedJob(jobMessage);
                        break;
                    case JobStatus.Abort:
                        this.UpdateAbortJob(jobMessage);
                        break;
                    default:
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, jobMessage.Schedule);
            }

            return null;
        }

        public void AddMailTaskForRunningJob(JobMessage jobMessage)
        {
            try
            {
                jobMessage.Schedule.TaskId = this._mailRepository.AddMailTask(new AddMailTaskCriteria()
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
                LogService.LogError(ex, jobMessage.Schedule);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mail">The mail need to send out.</param>
        /// <returns>Command Result for sending mail.</returns>
        public int SendMail(Mail mail)
        {
            var mailId = 0;
            try
            {
                var mailService = new MailDeliver.MailClient.API.MailDeliver();
                var mailRecord = this.ConvertFromMailToMailRecord(mail);
                mailId = (int)_mailDeliverBusiness.InsertMailRecord(mailRecord);
                var schedule = this._mailRepository.GetScheduleById(new GetScheduleByMailIdCriteria() { MailId = mail.MailingListId });
                if (schedule.Active)
                {
                    if (mailId > 0)
                    {
                        LogExtension.LogMessage(string.Format("Stored mail {0}", mailId));
                        mailRecord.IsReady = true;
                        mailRecord.MailID = mailId;

                        SendMail(mailRecord);

                        // If is function mail
                        // Insert a MailTask record to database
                        // else
                        // Leave the MailTask record for JobRequest to handle.
                        if (schedule.TriggerTypeEnum == MailType.Function)
                        {
                            _mailRepository.AddMailTask(new AddMailTaskCriteria()
                            {
                                MailId = mail.MailingListId,
                                SiteId = mail.SiteId,
                                ScheduledTime = DateTime.Now,
                                LastSent = DateTime.Now,
                                Status = (int)JobStatus.Success,
                                MSiteMailId = mailId
                            });
                        }
                    }
                    else
                    {
                        LogExtension.LogMessage("Store mail failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, mail.MailingListId);
            }

            return mailId;
        }

        /// <summary>
        /// Send mail.
        /// </summary>
        /// <param name="record">Mail information.</param>
        public void SendMail(MailRecord record)
        {
            record = record.IsReady ? record : _mailDeliverBusiness.GetMailRecord(record.MailID, true);
            if (record != null && record.IsReady)
            {
                string error = string.Empty;
                MailExtension.SendMail(record, out error);
                ////LogExtension.LogMessage(string.Format("Sending mail: {0}. Error message: {1}", record.MailID, error));
            }
        }

        /// <summary>
        /// Updates the running JobRequest.
        /// </summary>
        /// <param name="jobRequestJobRequest result message.</param>
        private void UpdateRunningJob(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Running
            });

            this._mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                RetryCount = jobMessage.Schedule.CurrentRetryTimes
            });

            if (jobMessage.Schedule.CurrentRetryTimes > 0 && jobMessage.TaskLog != null)
            {
                // If is retried then write log.
                this._mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = jobMessage.TaskLog.LogTime,
                    ExceptionMessage = jobMessage.TaskLog.Message,
                    StackTrace = jobMessage.TaskLog.StackTrace,
                    Status = (int)jobMessage.JobStatus,
                    TaskId = jobMessage.Schedule.TaskId
                });
            }
        }

        private void UpdateAbortJob(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Abort
            });
        }

        /// <summary>
        /// Updates for the data not ready JobRequest.
        /// </summary>
        /// <param name="jobRequestJobRequest result message.</param>
        private void UpdateDataNotReadyJob(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus
            });
        }

        /// <summary>
        /// Updates the success JobRequest.
        /// </summary>
        /// <param name="jobRequestJobRequest result message.</param>
        private void UpdateSuccessJob(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the JobRequest cannot be update to success when aborted. We will check Status again to make sure JobRequest is done.
            var mailTask = this._mailRepository.GetMailTask(new GetMailTaskByIdCriteria() { MailTaskId = jobMessage.Schedule.TaskId });
            if (mailTask.Status == (int)jobMessage.JobStatus)
            {
                int mailRecordId = this.SendMail(jobMessage.Mail);
                this._mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = jobMessage.Schedule.TaskId,
                    MSiteMailId = mailRecordId
                });
            }
        }

        /// <summary>
        /// Updates the success JobRequest.
        /// </summary>
        /// <param name="jobRequestJobRequest result message.</param>
        private void UpdateSuccessJobWithoutSendMail(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the JobRequest cannot be update to success when aborted. We will check Status again to make sure JobRequest is done.
            var mailTask = this._mailRepository.GetMailTask(new GetMailTaskByIdCriteria() { MailTaskId = jobMessage.Schedule.TaskId });
            if (mailTask.Status == (int)jobMessage.JobStatus)
            {
                this._mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = jobMessage.Schedule.TaskId,
                    MSiteMailId = 0
                });
            }
        }

        private void UpdateFailedJob(JobMessage jobMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.JobFailed
            });

            if (jobMessage != null && jobMessage.TaskLog != null)
            {
                this._mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = jobMessage.TaskLog.LogTime,
                    ExceptionMessage = jobMessage.TaskLog.Message,
                    StackTrace = jobMessage.TaskLog.StackTrace,
                    Status = (int)jobMessage.JobStatus,
                    TaskId = jobMessage.Schedule.TaskId
                });
            }
        }

        /// <summary>
        /// Converts from mail to mail record.
        /// </summary>
        /// <param name="mail">The mail info.</param>
        /// <returns>Mail Record.</returns>
        private MailRecord ConvertFromMailToMailRecord(Mail mail)
        {
            var mailRecord = new MailRecord();
            mailRecord.AppID = mail.ApplicationSendMailId;
            mailRecord.Config = new MailConfig();
            mailRecord.Config.AppID = mail.ApplicationSendMailId;
            mailRecord.Body = mail.Body;
            mailRecord.Subject = mail.Subject;
            mailRecord.Title = mail.Title;
            if (mail.Attachments != null && mail.Attachments.Count > 0)
            {
                int mailAttachCount = mail.Attachments.Count;
                for (int i = 0; i < mailAttachCount; i++)
                {
                    var attach = mail.Attachments[i];
                    mailRecord.Attachments.Add(new Fanex.MailDeliver.Model.MailAttachment()
                    {
                        AttachmentID = attach.Id,
                        Content = new System.IO.MemoryStream(attach.FileContent),
                        ContentType = (FileExtentions)((int)attach.FileExtension),
                        FileName = attach.FileName,
                        MailID = attach.MailId
                    });
                }
            }

            if (mail.AlternativeViews != null && mail.AlternativeViews.Count > 0)
            {
                int alternativeViewsCount = mail.AlternativeViews.Count;
                for (int i = 0; i < alternativeViewsCount; i++)
                {
                    var alternativeView = mail.AlternativeViews[i];
                    mailRecord.AlternateView.Add(new Fanex.MailDeliver.Model.MailLinkedResource()
                    {
                        ContentCID = alternativeView.ContentId,
                        FileName = alternativeView.FileName,
                        ImageType = (FileExtentions)((int)alternativeView.FileExtension),
                        LinkedID = alternativeView.Id,
                        MailID = alternativeView.MailId,
                        StreamContent = new System.IO.MemoryStream(alternativeView.FileContent)
                    });
                }
            }

            // Get Mailing List from database
            var mailingList = _mailRepository.GetMailingList(new GetMailWithEmailAddressByMailId() { MailId = mail.MailingListId });
            mailRecord.MailTo = string.Join(";", mailingList.TOs);
            mailRecord.MailCc = string.Join(";", mailingList.CCs);
            mailRecord.MailBcc = string.Join(";", mailingList.BCCs);
            return mailRecord;
        }
    }
}