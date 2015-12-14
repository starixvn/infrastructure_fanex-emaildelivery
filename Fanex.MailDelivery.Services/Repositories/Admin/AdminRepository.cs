using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fanex.Data;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;
using Fanex.Maintenance.Repository.Criteria.Misc;
using Quartz;
using Quartz.Spi;

namespace Fanex.MailDelivery.Services.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        #region Mail

        public List<MailModel> GetMailList(GetMailListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailModel>().ToList();
            }
        }

        public MailModel GetMailById(GetMailByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailModel>().FirstOrDefault();
            }
        }

        public ResultModel CreateMail(CreateMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultModel UpdateMail(UpdateMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultModel DeleteMail(DeleteMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        #endregion

        #region Recipient

        public List<RecipientModel> GetRecipientList(GetRecipientListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().ToList();
            }
        }

        public RecipientModel GetRecipientById(GetRecipientByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().FirstOrDefault();
            }
        }

        public ResultModel CreateRecipient(CreateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultModel UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultModel DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        #endregion

        #region Group
        // TODO: Refactor
        public IList<GroupModel> GetGroupList(GetGroupListCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<GroupModel>().ToList();
            }
        }

        public GroupModel GetGroupById(GetGroupByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<GroupModel>().FirstOrDefault();
            }
        }

        public ResultModel CreateGroup(CreateGroupCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public ResultModel UpdateGroup(UpdateGroupCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public ResultModel DeleteGroup(DeleteGroupCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public IList<MailTaskModel> GetReportList(GetReportListCriteria criteria)
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
            List<MailTaskDetailModel> listMailTaskDetail;
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

        #endregion

        public Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        private ResultModel GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];
            return new ResultModel()
            {
                ErrCode = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                ErrMsg = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}
