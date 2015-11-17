using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fanex.Common;
using Fanex.Data;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Enumerations;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Configurations;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.Maintenance.Repository.Criteria.Misc;
using Quartz;
using Quartz.Spi;
using ClientStatusModel = Fanex.MailDelivery.Admin.Models.ClientStatusModel;
using CreateMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.CreateMailCriteria;
using CreateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.CreateRecipientCriteria;
using DeleteMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.DeleteMailCriteria;
using DeleteRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.DeleteRecipientCriteria;
using GetMailListCriteria = Fanex.MailDelivery.Server.Criteria.Mail.GetMailListCriteria;
using MailTask = Fanex.MailDelivery.Admin.Models.MailTask;
using RecipientByIdCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.RecipientByIdCriteria;
using RecipientListCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.RecipientListCriteria;
using UpdateMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.UpdateMailCriteria;
using UpdateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.UpdateRecipientCriteria;

namespace Fanex.MailDelivery.Server.Repositories
{
    /// <summary>
    /// Mail Repository.
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        public Schedule GetScheduleById(GetScheduleByIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        public List<ClientStatusModel> GetAllScheduleMail(GetAllScheduleMailCriteria criteria)
        {
            List<ClientStatusModel> restult = new List<ClientStatusModel>();
            DateTime now = DateTime.Now;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    ClientStatusModel element = new ClientStatusModel();
                    element.MailId = Converter.ToInt(reader["MailID"]);
                    element.MailName = Converter.ToString2(reader["MailName"]);
                    if (reader["LastActive"] != null)
                    {
                        element.LastActive = Converter.ToDateTime(reader["LastActive"]);
                    }

                    element.IsOnline = element.LastActive != null && element.LastActive.Value >= now.AddSeconds(-AppSettings.MailServiceSection.ClientNotifyTimeOutInSecond * 2);
                    restult.Add(element);
                }
            }

            return restult;
        }

        public int AddMailTask(AddMailTaskCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return Converter.ToInt(db.GetParameterValue("MailTaskID"));
            }
        }

        public void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }
        }

        public void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }
        }

        public void AddMailLog(AddMailLogCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }
        }

        /// <summary>
        /// Gets the mailing list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Mailing List.</returns>
        public MailingList GetMailingList(GetMailWithEmailAddressByMailId criteria)
        {
            var result = new MailingList();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();
                if (reader.Read())
                {
                    result.Id = Converter.ToInt(reader["MailId"]);
                    result.Name = Converter.ToString2(reader["MailName"]);
                    result.SiteId = Converter.ToInt(reader["SiteId"]);
                    result.Active = Converter.ToBool(reader["IsActive"]);
                    result.Group = Converter.ToInt(reader["MailGroupID"]);
                    result.TOs = Converter.ToString2(reader["MailTo"]).Split(';').ToList();
                    result.CCs = Converter.ToString2(reader["MailCc"]).Split(';').ToList();
                    result.BCCs = Converter.ToString2(reader["MailBcc"]).Split(';').ToList();
                }
            }

            return result;
        }

        public void UpdateMailTask(UpdateMailTaskMSiteMailIdCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }
        }

        public MailTask GetMailTask(GetMailTaskByIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailTask>().FirstOrDefault();
            }
        }

        public void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        public List<RecipientModel> GetListRecipient(RecipientListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().ToList();
            }
        }

        public ResultResponse CreateRecipient(CreateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public List<MailModel> GetListMail(GetMailListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailModel>().ToList();
            }
        }

        public ResultResponse CreateMail(CreateMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultResponse UpdateMail(UpdateMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        public ResultResponse DeleteMail(DeleteMailCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return GetCommandResult(hash);
        }

        /// <summary>
        /// Gets the recipient by identity.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient by identity.</returns>
        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().FirstOrDefault();
            }
        }

        private ResultResponse GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];
            return new ResultResponse()
            {
                ErrCode = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                ErrMsg = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }


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

        public ResultResponse CreateGroup(CreateGroupCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public ResultResponse UpdateGroup(UpdateGroupCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public ResultResponse DeleteGroup(DeleteGroupCriteria criteria)
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
    }
}
