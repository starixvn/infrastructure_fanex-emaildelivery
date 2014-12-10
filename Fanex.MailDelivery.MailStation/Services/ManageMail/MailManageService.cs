namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criterias.MailManage;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Command;

    public class MailManageService : MailStationBaseService
    {

        public List<MailModel> MailList(GetMailListCriteria criteria)
        {
            List<MailModel> result = new List<MailModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<MailModel>().ToList();
            }

            return result;
        }

        public MailModel MailByID(ReadMailByIDCriteria criteria)
        {
            MailModel result = new MailModel();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                result = db.Query<MailModel>().ToList().FirstOrDefault();
            }

            return result;
        }

        public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CommandResult CreateMail(CreateMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CommandResult UpdateMail(UpdateMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
