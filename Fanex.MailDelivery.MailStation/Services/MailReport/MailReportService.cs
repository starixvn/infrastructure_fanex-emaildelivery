// --------------------------------------------------------------------------
// <copyright file="MailReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services.MailReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.Admin.Handlers;
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;
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
            List<MailTaskModel> result = new List<MailTaskModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<MailTaskModel>().ToList();
            }

            List<MailTaskDetailModel> listMailTaskDetail = new List<MailTaskDetailModel>();
            GetAllScheduleCriteria getAllScheduleCriteria = new GetAllScheduleCriteria() { MailName = criteria.MailName, SiteId = criteria.SiteId };
            using (ObjectDb db = new ObjectDb(getAllScheduleCriteria.GetSettingKey()))
            {
                db.AssignParamValues(getAllScheduleCriteria.GetSpParams());
                listMailTaskDetail = db.Query<MailTaskDetailModel>().ToList();
            }

            TimeSpan ts = new TimeSpan(23, 59, 59);
            criteria.ToDate += ts;
            if ((criteria.ToDate > DateTime.Now) && (criteria.Status == (int)JobStatus.Waiting || criteria.Status == -1))
            {
                foreach (MailTaskDetailModel schedule in listMailTaskDetail)
                {
                    if (schedule.LastSent == null || schedule.LastSent < criteria.ToDate)
                    {
                        DateTime fromDate = this.MaxDateTime(criteria.FromDate, DateTime.Today);
                        DateTime toDate = this.MaxDateTime(criteria.ToDate, DateTime.Today);
                        ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(schedule.CronExpression).Build();
                        //// Add 5 seconds to remove delay time between Scheduled Time and real Trigger Time.
                        DateTime lastSent = schedule.LastSent == null ? schedule.LastScheduledTime.AddSeconds(5) : schedule.LastSent.Value;
                        var listtriggerdate = TriggerUtils.ComputeFireTimesBetween(
                            (IOperableTrigger)trigger,
                            null,
                            new DateTimeOffset(this.MaxDateTime(fromDate, lastSent).ToUniversalTime()),
                            new DateTimeOffset(this.MinDateTime(toDate, schedule.EndOn).ToUniversalTime()));
                        foreach (var triggertime in listtriggerdate)
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
                                Status = (int)JobStatus.Waiting
                            };

                            result.Add(waitingTask);
                        }
                    }
                }
            }

            return result.OrderBy(m => m.Status).ThenByDescending(m => m.ScheduledTime).ToList();
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
            MailAdminHandler.Start();
            MailTaskModel mailTask = this.GetMailTaskByID(new GetMailTaskByIdCriteria() { MailTaskId = criteria.TaskId });
            Schedule schedule = this.GetScheduleByMailId(new GetScheduleByIdCriteria() { MailId = mailTask.MailId });
            schedule.TaskId = mailTask.MailTaskId;
            MailAdminHandler.PublishRetryJobMessage(schedule);
            MailAdminHandler.Stop();
        }

        public void AbortJob(int taskID)
        {
            MailAdminHandler.Start();
            MailAdminHandler.PublishAbortJobMessage(taskID);
            MailAdminHandler.Stop();
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
