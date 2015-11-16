using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fanex.Common;
using Fanex.Data;
using Fanex.MailDelivery.Admin.Criteria.Group;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.MailDelivery.Services.Models;
using Fanex.Maintenance.Repository.Criteria.Misc;
using CreateMailCriteria = Fanex.MailDelivery.Admin.Criteria.Mail.CreateMailCriteria;
using CreateRecipientCriteria = Fanex.MailDelivery.Admin.Criteria.Recipient.CreateRecipientCriteria;
using DeleteMailCriteria = Fanex.MailDelivery.Admin.Criteria.Mail.DeleteMailCriteria;
using DeleteRecipientCriteria = Fanex.MailDelivery.Admin.Criteria.Recipient.DeleteRecipientCriteria;
using GetMailListCriteria = Fanex.MailDelivery.Admin.Criteria.Mail.GetMailListCriteria;
using RecipientByIdCriteria = Fanex.MailDelivery.Admin.Criteria.Recipient.RecipientByIdCriteria;
using RecipientListCriteria = Fanex.MailDelivery.Admin.Criteria.Recipient.RecipientListCriteria;
using UpdateMailCriteria = Fanex.MailDelivery.Admin.Criteria.Mail.UpdateMailCriteria;
using UpdateRecipientCriteria = Fanex.MailDelivery.Admin.Criteria.Recipient.UpdateRecipientCriteria;

namespace Fanex.MailDelivery.Admin.Repositories
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

        #endregion
    }
}
