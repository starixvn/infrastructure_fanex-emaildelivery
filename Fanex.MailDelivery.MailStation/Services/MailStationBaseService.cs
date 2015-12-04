namespace Fanex.MailDelivery.MailStation.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Constants;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Mail Station Base Service.
    /// </summary>
    public class MailStationBaseService
    {
        /// <summary>
        /// Sites the specified has all element.
        /// </summary>
        /// <param name="hasAllElement">If set to <c>true</c> [has all element].</param>
        /// <returns>
        /// List sites.
        /// </returns>
        public List<SelectListItem> Sites(bool hasAllElement = true)
        {
            GetAllSiteCriteria criteria = new GetAllSiteCriteria();
            List<SelectListItem> result = new List<SelectListItem>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<SelectListItem>().ToList();
            }

            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(result).ToList();
            }

            return result;
        }

        /// <summary>
        /// Mails the groups.
        /// </summary>
        /// <param name="hasAllElement">If set to <c>true</c> [has all element].</param>
        /// <returns>
        /// Mail Groups.
        /// </returns>
        public List<SelectListItem> MailGroups(bool hasAllElement = true)
        {
            GetAllMailGroupCriteria criteria = new GetAllMailGroupCriteria();
            List<SelectListItem> result = new List<SelectListItem>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<SelectListItem>().ToList();
            }

            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(result).ToList();
            }

            return result;
        }

        public List<SelectListItemExtra> Status(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.Active).ToList();
            }

            return MailStationConstants.Active.ToList();
        }

        public List<SelectListItemExtra> TaskStatus(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.TaskStatus).ToList();
            }

            return MailStationConstants.AllItem;
        }

        public List<SelectListItemExtra> Trigger(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.TriggerTypes).ToList();
            }

            return MailStationConstants.TriggerTypes;
        }

        [AllowAnonymous]
        public List<SuggestMailModel> SuggestMails(SuggestMailCriteria criteria)
        {
            List<SuggestMailModel> result = new List<SuggestMailModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<SuggestMailModel>().ToList();
            }

            return result;
        }

        public CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];
            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }

        public CommandResult ExecuteNonQuery(CriteriaBase criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public MailTaskModel GetMailTaskByID(GetMailTaskByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailTaskModel>().FirstOrDefault();
            }
        }

        // public Schedule GetScheduleByMailId(GetScheduleByIdCriteria criteria)
        // {
        //     using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
        //     {
        //         db.AssignParamValues(criteria.GetSpParams());
        //         return db.Query<Schedule>().FirstOrDefault();
        //     }
        // }
        public string GetTimeZone()
        {
            return string.Empty;
        }
    }
}
