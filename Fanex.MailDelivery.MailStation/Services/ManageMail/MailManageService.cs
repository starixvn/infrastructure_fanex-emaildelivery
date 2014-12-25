namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Admin.Handlers;
    using System;

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
            MailAdminHandler.Start();
            MailAdminHandler.PublishScheduleChangeMessage(new Schedule()
            {
                Id = criteria.MailID,
                Active = criteria.IsActive,
                EndTime = criteria.EndOn,
                StartTime = criteria.StartOn ?? DateTime.Now,
                MaxRetryTimes = criteria.Attempt,
                Name = criteria.MailName,
                RepeatExpression = criteria.CronExpression,
                RetryIntervalInSeconds = criteria.Interval,
                SiteId = criteria.SiteID
            });

            MailAdminHandler.Stop();
            return ExecuteNonQuery(criteria);
        }

        public CommandResult DeleteMail(DeleteMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
