using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Data;
using Fanex.MailDelivery.Admin;
using Fanex.MailDelivery.Admin.Messaging.Schedule;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.NAP.Framework.Command;
using MailModel = Fanex.MailDelivery.MailStation.Models.MailModel;

namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
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

        public MailModel MailByID(ReadMailByIdCriteria criteria)
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
            MailAdminService.Start();
            MailAdminService.PublishScheduleChangeMessage(new ScheduleRequest
            {
                Id = criteria.MailId,
                Active = criteria.IsActive,
                EndTime = criteria.EndOn,
                StartTime = criteria.StartOn ?? DateTime.Now,
                MaxRetryTimes = criteria.Attempt,
                Name = criteria.MailName,
                RepeatExpression = criteria.CronExpression,
                RetryInterval = criteria.Interval,
                SiteId = criteria.SiteId
            });

            MailAdminService.Stop();
            return ExecuteNonQuery(criteria);
        }

        public CommandResult DeleteMail(DeleteMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public void RunJobImmediately(int mailID)
        {
            MailAdminService.Start();
            //TODO: Will be changed
            //Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailID });
            //MailAdminService.PublishRetryJobMessage(schedule);
            MailAdminService.Stop();
        }
    }
}
