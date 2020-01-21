using Fanex.MailDeliver.Business;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Fanex.MailDelivery.Services.Services.MailService
{
    public interface IMailService
    {
        void UpdateJobStatus(JobMessage jobMessage);

        int SendMail(Mail mail);

        void AddMailTaskForRunningJob(JobMessage jobMessage);

        MailTaskModel GetMailTaskById(GetMailTaskByIdRequest request);

        GetScheduleListResponse GetScheduleList(GetScheduleListRequest request);

        void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest);
    }

    public class MailService : IMailService
    {
        private readonly IMailRepository mailRepository;
        private readonly IMailDeliverBusiness mailDeliverBusiness;
        private readonly RetryOnMailRecordService retryOnMailRecordService;
        private readonly LogService logService;

        public MailService(int sendMailRetryLimit) : this(null, sendMailRetryLimit)
        {
        }

        public MailService(LogService logService, int sendMailRetryLimit)
        {
            this.logService = logService ?? new LogServiceImpl();
            mailRepository = new MailRepository();
            mailDeliverBusiness = new MailDeliverBusiness();
            retryOnMailRecordService = new RetryOnMailRecordServiceImp(mailRepository, sendMailRetryLimit);
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

                schedules.Add(schedule);
            }

            return new GetScheduleListResponse { Schedules = schedules };
        }

        public void UpdateClientActiveTime(UpdateActiveTimeRequest updateActiveTimeRequest)
        {
            var criteria = new UpdateClientActiveTimeCriteria
            {
                MailIds = updateActiveTimeRequest.MailIds
            };

            mailRepository.UpdateClientActiveTime(criteria);
        }

        public void UpdateJobStatus(JobMessage jobMessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;

                switch (jobMessage.JobStatus)
                {
                    case JobStatus.Running:
                        UpdateRunningJob(jobMessage);
                        break;

                    case JobStatus.DataIsNotReady:
                        UpdateDataNotReadyJob(jobMessage);
                        break;

                    case JobStatus.Success:
                        UpdateSuccessJob(jobMessage);
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
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
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
                jobMessage.Schedule.TaskId = mailRepository.AddMailTask(new AddMailTaskCriteria()
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
            var result = new MailTaskModel();

            try
            {
                result = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria()
                {
                    MailTaskId = request.TaskId
                });
            }
            catch (Exception ex)
            {
                logService.LogError(ex, request.TaskId);
            }

            return result;
        }

        public int SendMail(Mail mail)
        {
            var mailId = 0;
            var schedule = mailRepository.GetScheduleByMailId(new GetScheduleByMailIdCriteria { MailId = mail.MailingListId });

            if (schedule.Active)
            {
                try
                {
                    var mailRecord = ConvertFromMailToMailRecord(mail);

                    ValidateMail(mailRecord);

                    mailId = (int)mailDeliverBusiness.InsertMailRecord(mailRecord);

                    if (mailId > 0)
                    {
                        logService.LogInfo($"Mail({mail.MailingListId}) are stored successfully with msite mail id {mailId}.");

                        mailRecord.IsReady = true;
                        mailRecord.MailID = mailId;

                        SendMailRecord(mailRecord);

                        // If is function mail
                        // Insert a MailTask record to database
                        // else
                        // Leave the MailTask record for JobRequest to handle.
                        if (schedule.TriggerTypeEnum == MailType.Function)
                        {
                            mailRepository.AddMailTask(new AddMailTaskCriteria()
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
                        throw new InvalidOperationException($"Failed to store Mail({mail.MailingListId}).");
                    }
                }
                catch (Exception ex)
                {
                    logService.LogError(ex, schedule);
                }
            }

            return mailId;
        }

        public void SendMailRecord(MailRecord record)
        {
            record = record.IsReady ? record : mailDeliverBusiness.GetMailRecord(record.MailID, true);

            if (record != null && record.IsReady)
            {
                string error = string.Empty;

                try
                {
                    MailExtension.SendMail(record, out error);
                }
                catch (Exception exception)
                {
                    logService.LogError(exception);
                }
                finally
                {
                    var InstanceCaller = new Thread(() => retryOnMailRecordService.Retry((int)record.MailID));
                    InstanceCaller.Start();
                }
            }
        }

        private void ValidateMail(MailRecord mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Subject))
            {
                throw new ArgumentNullException("mail.Subject", "Mail subject cannot be null or empty.");
            }
        }

        private void UpdateRunningJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Running
            });

            mailRepository.UpdateRetryCounter(new UpdateTaskRetryCounterCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                RetryCount = jobMessage.Schedule.CurrentRetryTimes
            });

            if (jobMessage.Schedule.CurrentRetryTimes > 0 && jobMessage.TaskLog != null)
            {
                // If is retried then write log.
                mailRepository.AddMailLog(new AddMailLogCriteria()
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
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.Abort
            });
        }

        private void UpdateDataNotReadyJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus
            });
        }

        private void UpdateSuccessJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the JobRequest cannot be update to success when aborted. We will check Status again to make sure JobRequest is done.
            var mailTask = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria() { MailTaskId = jobMessage.Schedule.TaskId });

            if (mailTask.Status == (int)jobMessage.JobStatus)
            {
                int mailRecordId = SendMail(jobMessage.Mail);

                mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = jobMessage.Schedule.TaskId,
                    MSiteMailId = mailRecordId
                });
            }
        }

        private void UpdateSuccessJobWithoutSendMail(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)jobMessage.JobStatus,
                LastSent = DateTime.Now
            });

            // Because the JobRequest cannot be update to success when aborted. We will check Status again to make sure JobRequest is done.
            var mailTask = mailRepository.GetMailTaskById(new GetMailTaskByIdCriteria() { MailTaskId = jobMessage.Schedule.TaskId });

            if (mailTask.Status == (int)jobMessage.JobStatus)
            {
                mailRepository.UpdateMailTask(new UpdateMailTaskMSiteMailIdCriteria()
                {
                    TaskId = jobMessage.Schedule.TaskId,
                    MSiteMailId = 0
                });
            }
        }

        private void UpdateFailedJob(JobMessage jobMessage)
        {
            mailRepository.UpdateMailTaskStatus(new UpdateMailTaskStatusCriteria()
            {
                TaskId = jobMessage.Schedule.TaskId,
                Status = (int)JobStatus.JobFailed
            });

            if (jobMessage.TaskLog != null)
            {
                mailRepository.AddMailLog(new AddMailLogCriteria()
                {
                    LogTime = jobMessage.TaskLog.LogTime,
                    ExceptionMessage = jobMessage.TaskLog.Message,
                    StackTrace = jobMessage.TaskLog.StackTrace,
                    Status = (int)jobMessage.JobStatus,
                    TaskId = jobMessage.Schedule.TaskId
                });
            }
        }

        private MailRecord ConvertFromMailToMailRecord(Mail mail)
        {
            var mailingList = mailRepository.GetMailingList(new GetMailWithEmailAddressByMailId() { MailId = mail.MailingListId });

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

                    destinationMailRecord.AlternateView.Add(new MailDeliver.Model.MailLinkedResource()
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

                    destinationMailRecord.Attachments.Add(new MailDeliver.Model.MailAttachment()
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