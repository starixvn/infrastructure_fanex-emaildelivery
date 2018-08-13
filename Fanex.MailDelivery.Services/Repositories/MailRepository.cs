using Fanex.Common;
using Fanex.Data;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.MailDelivery.Services.Repositories
{
    /// <summary>
    /// Mail Repository.
    /// </summary>
    public class MailRepository : IMailRepository
    {
        public Schedule GetScheduleById(GetScheduleByMailIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        public List<ClientStatusModel> GetAllScheduleMail(GetAllScheduleMailCriteria criteria)
        {
            List<ClientStatusModel> restult;
            DateTime now;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                using (var multi = db.QueryMultiple())
                {
                    restult = multi.Read<ClientStatusModel>().ToList();
                    now = multi.Read<DateTime>().FirstOrDefault();
                }

                restult.ForEach(n =>
                {
                    n.IsOnline = n.LastActive != null && n.LastActive.Value.AddSeconds(AppSettings.MailServiceSection.ClientNotifyTimeOutInSecond) >= now;
                });
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

        public MailTaskModel GetMailTaskById(GetMailTaskByIdCriteria criteria)
        {
            MailTaskModel result = new MailTaskModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();
                if (reader.Read())
                {
                    result.MailTaskId = Converter.ToInt(reader["TaskID"]);
                    result.MailId = Converter.ToInt(reader["MailID"]);
                    result.SiteId = Converter.ToInt(reader["SiteID"]);
                    result.ScheduledTime = Converter.ToDateTime(reader["ScheduledTime"]);
                    result.LastSent = Converter.ToDateTime(reader["LastSent"]);
                    result.Status = Converter.ToInt(reader["Status"]);
                    result.Attempt = Converter.ToInt(reader["Attempt"]);
                    result.MSiteMailId = Converter.ToInt(reader["MSiteMailID"]);
                    result.IsUnableToRetry = Converter.ToBool(reader["IsUnableToRetry"]);
                }
            }

            return result;
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
