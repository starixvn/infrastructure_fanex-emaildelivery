namespace Fanex.MailDelivery.MailStation.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Constants;
    using Fanex.MailDelivery.MailStation.Criterias.MailManage;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.MailDelivery.MailStation.Models;

    public class MailStationBaseService
    {
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

        public List<SelectListItem> Status(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.Active).ToList();
            }

            return MailStationConstants.Active;
        }

        public List<SelectListItem> Trigger(bool hasAllElement = true)
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
    }
}
