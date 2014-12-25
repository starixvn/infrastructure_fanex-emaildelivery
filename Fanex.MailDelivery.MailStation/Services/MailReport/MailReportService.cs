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
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;
    using Fanex.MailDelivery.Client.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Admin.Handlers;
    using Quartz;
    using Quartz.Spi;
    using Fanex.MailDelivery.Client.Enumerations;

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
        public List<MailTaskModel> MailReport(MailReportCriteria criteria)
        {
            List<MailTaskModel> result = new List<MailTaskModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<MailTaskModel>().ToList();
            }

            List<MailTaskDetailModel> listMailTaskDetail = new List<MailTaskDetailModel>();
            GetAllScheduleCriteria getAllScheduleCriteria = new GetAllScheduleCriteria() { MailName = criteria.MailName };
            using (ObjectDb db = new ObjectDb(getAllScheduleCriteria.GetSettingKey()))
            {
                db.AssignParamValues(getAllScheduleCriteria.GetSpParams());
                listMailTaskDetail = db.Query<MailTaskDetailModel>().ToList();
            }

            foreach (MailTaskDetailModel schedule in listMailTaskDetail)
            {
                if (schedule.LastSent != null && schedule.LastSent < criteria.ToDate)
                {
                    ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(schedule.CronExpression).Build();
                    var listtriggerdate = TriggerUtils.ComputeFireTimesBetween(
                        (IOperableTrigger)trigger,
                        null,
                        new DateTimeOffset(this.MaxDateTime(criteria.FromDate, schedule.LastSent)),
                        new DateTimeOffset(this.MinDateTime(criteria.ToDate, schedule.EndOn)));
                    foreach (var triggertime in listtriggerdate)
                    {
                        MailTaskModel waitingTask = new MailTaskModel()
                        {
                            MailName = schedule.MailName,
                            MailId = schedule.MailID,
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

            return result.OrderBy(m => m.MailName).ToList();
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

        public MailTaskModel GetMailTaskByID(GetMailTaskByIDCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailTaskModel>().FirstOrDefault();
            }
        }

        public Schedule GetScheduleByMailID(GetScheduleByIDCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        /// <summary>
        /// Retry send mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Retry result.</returns>
        public void RetrySendMail(RetrySendMailCriteria criteria)
        {
            MailAdminHandler.Start();
            MailTaskModel mailTask = this.GetMailTaskByID(new GetMailTaskByIDCriteria() { MailTaskID = criteria.TaskId });
            Schedule schedule = this.GetScheduleByMailID(new GetScheduleByIDCriteria() { MailId = mailTask.MailId });
            schedule.TaskId = mailTask.MailTaskId;
            MailAdminHandler.PublishRetryJobMessage(schedule);
            MailAdminHandler.Stop();
        }

        private DateTime MaxDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                return date1 >= date2.Value ? date1 : date2.Value;
            }
            else
            {
                return date1;
            }
        }

        private DateTime MinDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                return date1 < date2.Value ? date1 : date2.Value;
            }
            else
            {
                return date1;
            }

        }
    }
}
