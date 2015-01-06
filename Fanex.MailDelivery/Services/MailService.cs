namespace Fanex.MailDelivery.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.Repositories;
    using Fanex.MailDelivery.Criteria;
    using Fanex.MailDelivery.Client.Messages;

    /// <summary>
    /// Mail Service.
    /// </summary>
    public class MailService : ServiceBase, IMailService
    {
        private IMailRepository _mailRepository;

        public MailService()
        {
            this._mailRepository = this._mailRepository ?? new MailRepository();
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
        /// <param name="schedule">The schedule.</param>
        /// <param name="jobStatus">The job status.</param>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateJobStatus(JobResultMessage jobResultMessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (jobResultMessage.JobStatus)
                {
                    case JobStatus.Running: 
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

                        break;
                    case JobStatus.Retry:
                        this._mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
                        {
                            TaskID = jobResultMessage.Schedule.TaskId,
                            RetryCount = jobResultMessage.Schedule.CurrentRetryTimes
                        });

                        break;
                    case JobStatus.DataIsNotReady:
                        this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
                        {
                            TaskID = jobResultMessage.Schedule.TaskId,
                            Status = (int)JobStatus.DataIsNotReady
                        });

                        this._mailRepository.AddMailLog(new AddMailLogCriteria() {
                            LogTime = jobResultMessage.TaskLog.LogTime,
                            ExceptionMessage = jobResultMessage.TaskLog.Message,
                            StackTrace = jobResultMessage.TaskLog.StackTrace,
                            Status = (int)jobResultMessage.JobStatus,
                            TaskID = jobResultMessage.Schedule.TaskId
                        });

                        break;
                    case JobStatus.Success:
                        this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
                        {
                            TaskID = jobResultMessage.Schedule.TaskId,
                            Status = (int)JobStatus.Success,
                            LastSent = DateTime.Now
                        });

                        this.SendMail(jobResultMessage.Mail);
                        break;
                    case JobStatus.Failed:
                        this._mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
                        {
                            TaskID = jobResultMessage.Schedule.TaskId,
                            Status = (int)JobStatus.Failed
                        });

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
        public CommandResult SendMail(Mail mail)
        {
            try
            {
                var mailService = new MailDeliver();
                var mailRecord = this.ConvertFromMailToMailRecord(mail);

                // If is function mail
                // Insert a MailTask record to database
                // else
                // Leave the MailTask record for Job to handle.
                var mailType = _mailRepository.GetMailingListType(new GetMailingListTypeCriteria() { MailID = mail.MailingListId });
                if (mailType == MailType.Function)
                {
                    _mailRepository.AddMailTask(new AddMailTaskCriteria() {
                        MailId = mail.MailingListId,
                        SiteId = mail.SiteId,
                        ScheduledTime = DateTime.Now,
                        LastSent = DateTime.Now,
                        Status =(int)JobStatus.Success
                    });
                }

                mailService.SendMail(mailRecord);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

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