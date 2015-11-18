using System;
using System.Collections.Generic;
using Fanex.MailDeliver.Business;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDeliver.Core.Infrastructure.Logging;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Repositories;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.Server.MailService
{
    /// <summary>
    /// Mail Service.
    /// </summary>
    public class MailService : ServiceBase, IMailService
    {
        private IMailRepository _mailRepository;
        private IMailDeliverBusiness _mailDeliverBusiness;

        public MailService()
        {
            this._mailRepository = this._mailRepository ?? new MailRepository();
            this._mailDeliverBusiness = new MailDeliverBusiness();
        }

        /// <summary>
        /// Gets the schedules.
        /// </summary>
        /// <param name="scheduleIds">The schedule ids.</param>
        /// <returns>Schedule list.</returns>
        public List<Schedule> GetSchedules(List<int> scheduleIds)
        {
            List<Schedule> schedules = new List<Schedule>();

            try
            {
                foreach (int scheduleId in scheduleIds)
                {
                    schedules.Add(this._mailRepository.GetScheduleById(new GetScheduleByIdCriteria() { MailId = scheduleId }));
                }
            }
            catch (Exception ex)
            {
                LogError(ex, scheduleIds);
            }

            return schedules;
        }

        /// <summary>
        /// Updates the job status.
        /// Just incase of status is Running, Retry, DataIsNotReady using this function.
        /// </summary>
        /// <param name="job">The job result message.</param>
        /// <returns>
        /// Command Result.
        /// </returns>
        public CommandResult UpdateJobStatus(Job job)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (job.JobStatus)
                {
                    case JobStatus.Running:
                        this.UpdateRunningJob(job);
                        break;
                    case JobStatus.DataIsNotReady:
                        this.UpdateDataNotReadyJob(job);
                        break;
                    case JobStatus.Success:
                        this.UpdateSuccessJob(job);
                        break;
                    case JobStatus.SuccessWithoutSendmail:
                        this.UpdateSuccessJobWithoutSendMail(job);
                        break;
                    case JobStatus.JobFailed:
                        this.UpdateFailedJob(job);
                        break;
                    case JobStatus.Abort:
                        this.UpdateAbortJob(job);
                        break;
                    default:
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                LogError(ex, job.Schedule);
            }

            return null;
        }

        public void AddMailTaskForRunningJob(Job job)
        {
            try
            {
                job.Schedule.TaskId = this._mailRepository.AddMailTask(new AddMailTaskCriteria()
                {
                    MailId = job.Schedule.Id,
                    SiteId = job.Schedule.SiteId,
                    ScheduledTime = job.ScheduledTime,
                    Status = (int)JobStatus.Running,
                    Attempt = job.Schedule.CurrentRetryTimes
                });
            }
            catch (Exception ex)
            {
                LogError(ex, job.Schedule);
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
                var schedule = this._mailRepository.GetScheduleById(new GetScheduleByIdCriteria() { MailId = mail.MailingListId });
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
                        // Leave the MailTask record for Job to handle.
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
                LogError(ex, mail.MailingListId);
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

        public void UpdateClientActiveTime(ClientInfomationMessage clientInfoMessage)
        {
            this._mailRepository.UpdateClientActiveTime(new UpdateClientActiveTimeCriteria() { MailIds = clientInfoMessage.MailIds });
        }
     
        /// <summary>
        /// Updates the running job.
        /// </summary>
        /// <param name="job">The job result message.</param>
        private void UpdateRunningJob(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)JobStatus.Running
            });

            this._mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
            {
                TaskId = job.Schedule.TaskId,
                RetryCount = job.Schedule.CurrentRetryTimes
            });

            if (job.Schedule.CurrentRetryTimes > 0 && job.TaskLog != null)
            {
                // If is retried then write log.
                this._mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = job.TaskLog.LogTime,
                    ExceptionMessage = job.TaskLog.Message,
                    StackTrace = job.TaskLog.StackTrace,
                    Status = (int)job.JobStatus,
                    TaskId = job.Schedule.TaskId
                });
            }
        }

        private void UpdateAbortJob(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)JobStatus.Abort
            });
        }

        /// <summary>
        /// Updates for the data not ready job.
        /// </summary>
        /// <param name="job">The job result message.</param>
        private void UpdateDataNotReadyJob(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)job.JobStatus
            });
        }

        /// <summary>
        /// Updates the success job.
        /// </summary>
        /// <param name="job">The job result message.</param>
        private void UpdateSuccessJob(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)job.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the job cannot be update to success when aborted. We will check Status again to make sure job is done.
            var mailTask = this._mailRepository.GetMailTask(new GetMailTaskByIdCriteria() { MailTaskId = job.Schedule.TaskId });
            if (mailTask.Status == (int)job.JobStatus)
            {
                int mailRecordId = this.SendMail(job.Mail);
                this._mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = job.Schedule.TaskId,
                    MSiteMailId = mailRecordId
                });
            }
        }

        /// <summary>
        /// Updates the success job.
        /// </summary>
        /// <param name="job">The job result message.</param>
        private void UpdateSuccessJobWithoutSendMail(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)job.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the job cannot be update to success when aborted. We will check Status again to make sure job is done.
            var mailTask = this._mailRepository.GetMailTask(new GetMailTaskByIdCriteria() { MailTaskId = job.Schedule.TaskId });
            if (mailTask.Status == (int)job.JobStatus)
            {
                this._mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = job.Schedule.TaskId,
                    MSiteMailId = 0
                });
            }
        }

        private void UpdateFailedJob(Job job)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = job.Schedule.TaskId,
                Status = (int)JobStatus.JobFailed
            });

            if (job != null && job.TaskLog != null)
            {
                this._mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = job.TaskLog.LogTime,
                    ExceptionMessage = job.TaskLog.Message,
                    StackTrace = job.TaskLog.StackTrace,
                    Status = (int)job.JobStatus,
                    TaskId = job.Schedule.TaskId
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