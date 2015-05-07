namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDelivery.Admin.Handlers;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
    using Fanex.MailDelivery.MailStation.Criteria.MailManage;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Mail Manage Service.
    /// </summary>
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

        /// <summary>
        /// Updates the mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command Result.</returns>
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

        public void RunJobImmediately(int mailID)
        {
            MailAdminHandler.Start();
            Schedule schedule = this.GetScheduleByMailID(new GetScheduleByIDCriteria() { MailId = mailID });
            MailAdminHandler.PublishRetryJobMessage(schedule);
            MailAdminHandler.Stop();
        }
    }
}
