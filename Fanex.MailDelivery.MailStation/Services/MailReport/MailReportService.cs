// --------------------------------------------------------------------------
// <copyright file="MailReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

using Fanex.MailDelivery.Admin;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Core.Enumerations;

namespace Fanex.MailDelivery.MailStation.Services.MailReport
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;
    using Fanex.NAP.Core.Criteria.Misc;
    using Quartz;
    using Quartz.Spi;

    /// <summary>
    /// Mail Report Service.
    /// </summary>
    public class MailReportService : MailStationBaseService
    {
        private IMailService _mailServer;

        public MailReportService()
        {
            _mailServer = _mailServer ?? MailService.Create();
        }

        /// <summary>
        /// Gets the mail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail report.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public List<MailTaskModel> MailReport(MailReportCriteria criteria)
        {
            int oldStatus = criteria.Status;
            criteria.Status = -1;
            List<MailTaskModel> result = new List<MailTaskModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<MailTaskModel>().ToList();
            }

            criteria.Status = oldStatus;
            List<MailTaskDetailModel> listMailTaskDetail = new List<MailTaskDetailModel>();
            GetAllScheduleCriteria getAllScheduleCriteria = new GetAllScheduleCriteria() { MailName = criteria.MailName, SiteId = criteria.SiteId };
            using (ObjectDb db = new ObjectDb(getAllScheduleCriteria.GetSettingKey()))
            {
                db.AssignParamValues(getAllScheduleCriteria.GetSpParams());
                listMailTaskDetail = db.Query<MailTaskDetailModel>().ToList();
            }

            if (criteria.Status == -1 || criteria.Status == (int)JobStatus.Waiting || criteria.Status == (int)JobStatus.Missing)
            {
                TimeSpan ts = new TimeSpan(23, 59, 59);
                criteria.ToDate += ts;
                foreach (MailTaskDetailModel schedule in listMailTaskDetail)
                {
                    DateTime fromDate = this.MaxDateTime(criteria.FromDate, schedule.StartOn).ToUniversalTime();
                    DateTime toDate = this.MinDateTime(criteria.ToDate, schedule.EndOn).ToUniversalTime();
                    if (fromDate <= toDate)
                    {
                        ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(schedule.CronExpression).Build();
                        var listtriggerdate = TriggerUtils.ComputeFireTimesBetween(
                            (IOperableTrigger)trigger,
                            null,
                            new DateTimeOffset(fromDate),
                            new DateTimeOffset(toDate));
                        listtriggerdate = (from trigertime in listtriggerdate
                                           where !(from task in result
                                                   where task.MailId == schedule.MailId
                                                   select task.ScheduledTime)
                                               .Contains(trigertime.LocalDateTime)
                                           select trigertime).ToList();
                        foreach (var triggertime in listtriggerdate)
                        {
                            var status = triggertime.LocalDateTime.AddSeconds(5) > DateTime.Now ? (int)JobStatus.Waiting : (int)JobStatus.Missing;
                            if (criteria.Status == -1 || criteria.Status == status)
                            {
                                MailTaskModel waitingTask = new MailTaskModel()
                                {
                                    MailName = schedule.MailName,
                                    MailId = schedule.MailId,
                                    Attempt = schedule.Attempt,
                                    LastSent = null,
                                    ScheduledTime = triggertime.LocalDateTime,
                                    SiteId = schedule.SiteId,
                                    SiteName = schedule.SiteName,
                                    Status = status,
                                    TriggerType = (int)MailType.Schedule
                                };

                                result.Add(waitingTask);
                            }
                        }
                    }
                }
            }

            if (oldStatus != -1)
            {
                result = result.Where(m => m.Status == oldStatus).ToList();
            }

            return result.OrderBy(m => m.MailName).ThenByDescending(m => m.ScheduledTime).ThenByDescending(m => m.LastSent).ToList();
        }

        /// <summary>
        /// Gets the mail detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail detail.</returns>
        public MailRecord MailDetail(MailDetailCriteria criteria)
        {
            try
            {
                return _mailServer.GetMail(criteria.MailId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the mail log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail log.</returns>
        public List<MailLogModel> MailLog(MailLogCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailLogModel>().ToList();
            }
        }

        /// <summary>
        /// Retry send mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void RetrySendMail(RetrySendMailCriteria criteria)
        {
            MailAdminService.Start();
            MailTaskModel mailTask = this.GetMailTaskByID(new GetMailTaskByIdCriteria() { MailTaskId = criteria.TaskId });

            //TODO: Will be changed

            //Schedule schedule = this.GetScheduleByMailId(new GetScheduleByIdCriteria() { MailId = mailTask.MailId });
            //schedule.TaskId = mailTask.MailTaskId;
            //MailAdminService.PublishRetryJobMessage(schedule);
            MailAdminService.Stop();
        }

        public void AbortJob(int taskID)
        {
            MailAdminService.Start();

            AbortJobRequest request = new AbortJobRequest
            {
                TaskId = taskID,
            };

            MailAdminService.AbortJob(request);
            MailAdminService.Stop();
        }

        public void RetryMissingJob(RetryMissingTaskCriteria criteria)
        {
            // Add Missing Task to DB first.
            int mailTaskID;
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                mailTaskID = Converter.ToInt(db.GetParameterValue("MailTaskID"));
            }

            MailAdminService.Start();
            MailTaskModel mailTask = this.GetMailTaskByID(new GetMailTaskByIdCriteria() { MailTaskId = mailTaskID });

            //TODO: Will be changed 
            //Schedule schedule = this.GetScheduleByMailId(new GetScheduleByIdCriteria() { MailId = mailTask.MailId });
            //schedule.CurrentFireTime = criteria.ScheduledTime;
            //schedule.TaskId = mailTaskID;
            //MailAdminService.PublishRetryJobMessage(schedule);


            MailAdminService.Stop();
        }

        /// <summary>
        /// Get maximum date time.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The max date.</returns>
        private DateTime MaxDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                // Add 5 seconds for the number ratio.
                return date1 >= date2.Value ? date1 : date2.Value.AddSeconds(5);
            }
            else
            {
                return date1;
            }
        }

        /// <summary>
        /// Get minimum date time.
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>The min date.</returns>
        private DateTime MinDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                // Add 5 seconds for the number ratio.
                return date1 < date2.Value ? date1 : date2.Value.AddSeconds(5);
            }
            else
            {
                return date1;
            }
        }
    }
}
