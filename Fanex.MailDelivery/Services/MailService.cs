namespace Fanex.MailDelivery.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDeliver.Business;
    using Fanex.MailDeliver.Business.Implement;
    using Fanex.MailDeliver.Business.Interface;
    using Fanex.MailDeliver.Core.Infrastructure.Logging;
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Criteria;
    using Fanex.MailDelivery.Repositories;
    using Fanex.NAP.Framework.Command;

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
                    schedules.Add(this._mailRepository.GetScheduleByID(new GetScheduleByIDCriteria() { MailId = scheduleId }));
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return schedules;
        }

        /// <summary>
        /// Updates the job status.
        /// Just incase of status is Running, Retry, DataIsNotReady using this function.
        /// </summary>
        /// <param name="jobResultMessage">The job result message.</param>
        /// <returns>
        /// Command Result.
        /// </returns>
        public CommandResult UpdateJobStatus(JobResultMessage jobResultMessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (jobResultMessage.JobStatus)
                {
                    case JobStatus.Running:
                        this.UpdateRunningJob(jobResultMessage);
                        break;
                    case JobStatus.Retry:
                        this.UpdateRetryJob(jobResultMessage);
                        break;
                    case JobStatus.DataIsNotReady:
                        this.UpdateDataNotReadyJob(jobResultMessage);
                        break;
                    case JobStatus.Success:
                        this.UpdateSuccessJob(jobResultMessage);
                        break;
                    case JobStatus.Failed:
                        this.UpdateFailedJob(jobResultMessage);
                        break;
                    case JobStatus.Abort:
                        this.UpdateAbortJob(jobResultMessage);
                        break;
                    default:
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

        public void AddMailTaskForRunningJob(JobResultMessage jobResultMessage)
        {
            jobResultMessage.Schedule.TaskId = this._mailRepository.AddMailTask(new AddMailTaskCriteria()
            {
                MailId = jobResultMessage.Schedule.Id,
                SiteId = jobResultMessage.Schedule.SiteId,
                ScheduledTime = jobResultMessage.RunningTime,
                Status = (int)JobStatus.Running,
                Attempt = jobResultMessage.Schedule.CurrentRetryTimes
            });
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
                var mailService = new MailDeliver();
                var mailRecord = this.ConvertFromMailToMailRecord(mail);
                mailId = (int)_mailDeliverBusiness.InsertMailRecord(mailRecord);
                var schedule = this._mailRepository.GetScheduleByID(new GetScheduleByIDCriteria() { MailId = mail.MailingListId });
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
                LogError(ex);
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
                LogExtension.LogMessage(string.Format("Sending mail: {0}. Error message: {1}", record.MailID, error));
            }
        }

        /// <summary>
        /// Gets the mailing list.
        /// </summary>
        /// <param name="mailingListId">The mailing list identifier.</param>
        /// <returns>Mailing List.</returns>
        public MailingList GetMailingList(int mailingListId)
        {
            try
            {
                return _mailRepository.GetMailingList(new GetMailWithEmailAddressByMailID() { MailId = mailingListId });
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the mailing list.
        /// </summary>
        /// <param name="mailingList">The mailing list.</param>
        /// <returns>Update result.</returns>
        public CommandResult UpdateMailingList(MailingList mailingList)
        {
            try
            {
                if (mailingList.MailType == MailType.Function)
                {
                    Console.WriteLine("Updated function mailing list:" + mailingList.TOs[0]);
                }
                else
                {
                    Console.WriteLine("Updated schedule mailing list:" + mailingList.TOs[0]);
                }

                return new CommandResult
                {
                    Code = 0,
                    Message = "Updated successfully!"
                };
            }
            catch (Exception ex)
            {
                LogError(ex);

                return new CommandResult
                {
                    Code = 1001,
                    Message = "Update unsuccessfully!"
                };
            }
        }

        /// <summary>
        /// Updates the running job.
        /// </summary>
        /// <param name="jobResultMessage">The job result message.</param>
        private void UpdateRunningJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                Status = (int)JobStatus.Running
            });

            this._mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                RetryCount = jobResultMessage.Schedule.CurrentRetryTimes
            });

            if (jobResultMessage.Schedule.CurrentRetryTimes > 0 && jobResultMessage.TaskLog != null)
            {
                // If is retried then write log.
                this._mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = jobResultMessage.TaskLog.LogTime,
                    ExceptionMessage = jobResultMessage.TaskLog.Message,
                    StackTrace = jobResultMessage.TaskLog.StackTrace,
                    Status = (int)jobResultMessage.JobStatus,
                    TaskID = jobResultMessage.Schedule.TaskId
                });
            }
        }

        private void UpdateAbortJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                Status = (int)JobStatus.Abort
            });
        }

        private void UpdateRetryJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                RetryCount = jobResultMessage.Schedule.CurrentRetryTimes
            });
        }

        /// <summary>
        /// Updates for the data not ready job.
        /// </summary>
        /// <param name="jobResultMessage">The job result message.</param>
        private void UpdateDataNotReadyJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                Status = (int)JobStatus.DataIsNotReady
            });

            this._mailRepository.AddMailLog(new AddMailLogCriteria()
            {
                LogTime = jobResultMessage.TaskLog.LogTime,
                ExceptionMessage = jobResultMessage.TaskLog.Message,
                StackTrace = jobResultMessage.TaskLog.StackTrace,
                Status = (int)jobResultMessage.JobStatus,
                TaskID = jobResultMessage.Schedule.TaskId
            });
        }

        /// <summary>
        /// Updates the success job.
        /// </summary>
        /// <param name="jobResultMessage">The job result message.</param>
        private void UpdateSuccessJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                Status = (int)JobStatus.Success,
                LastSent = DateTime.Now
            });

            // Because the job cannot be update to success when aborted. We will check Status again to make sure job is done.
            var mailTask = this._mailRepository.GetMailTask(new GetMailTaskByIDCriteria() { MailTaskID = jobResultMessage.Schedule.TaskId });
            if (mailTask.Status == (int)JobStatus.Success)
            {
                int mailRecordId = this.SendMail(jobResultMessage.Mail);
                this._mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIDCriteria()
                {
                    TaskID = jobResultMessage.Schedule.TaskId,
                    MSiteMailID = mailRecordId
                });
            }
        }

        private void UpdateFailedJob(JobResultMessage jobResultMessage)
        {
            this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskID = jobResultMessage.Schedule.TaskId,
                Status = (int)JobStatus.Failed
            });
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
            var mailingList = _mailRepository.GetMailingList(new GetMailWithEmailAddressByMailID() { MailId = mail.MailingListId });
            mailRecord.MailTo = string.Join(";", mailingList.TOs);
            mailRecord.MailCc = string.Join(";", mailingList.CCs);
            mailRecord.MailBcc = string.Join(";", mailingList.BCCs);
            return mailRecord;
        }
    }
}