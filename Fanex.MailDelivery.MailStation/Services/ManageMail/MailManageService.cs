namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Framework.Command;

    public class MailManageService : MailStationBaseService
    {

        public List<MailModel> MailList(GetMailListCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailModel>().ToList();
            }
        }

        public MailModel MailByID(ReadMailByIDCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailModel>().ToList().FirstOrDefault();
            }
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
