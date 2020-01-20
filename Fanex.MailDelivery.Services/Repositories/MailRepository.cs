using Fanex.Common;
using Fanex.Data;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.MailDelivery.Services.Repositories
{
    public interface IMailRepository
    {
        Schedule GetScheduleById(GetScheduleByMailIdCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailId criteria);

        void UpdateMailTask(UpdateMailTaskMSiteMailIdCriteria criteria);

        void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria);

        MailTaskModel GetMailTaskById(GetMailTaskByIdCriteria criteria);

        MailRecord GetMailRecordById(int mailId);

        void UpdateServiceCheckIn(ServiceMonitoringCheckInCriteria criteria);
    }

    public class MailRepository : IMailRepository
    {
        private readonly int clientNotifyTimeOutInSecond;

        public MailRepository(int clientNotifyTimeOutInSecond)
        {
            this.clientNotifyTimeOutInSecond = clientNotifyTimeOutInSecond;
        }

        public Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        public void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        public void UpdateServiceCheckIn(ServiceMonitoringCheckInCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

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
                    n.IsOnline = n.LastActive != null && n.LastActive.Value.AddSeconds(clientNotifyTimeOutInSecond) >= now;
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

        public MailRecord GetMailRecordById(int mailId)
        {
            var criteria = new MailRecorderGetMailById { MailId = mailId };
            var mailRecord = new MailRecord();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var dataSet = db.ExecuteDataSet();
                var dataTable = new DataTable();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    dataTable = dataSet.Tables[0];
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    mailRecord.SentSuccess = Converter.To<int>(row["SentSuccess"]);
                    mailRecord.SentFail = Converter.To<int>(row["SentFail"]);
                }

                return mailRecord;
            }
        }
    }
}