// --------------------------------------------------------------------------
// <copyright file="MailReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services.MailReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;

    /// <summary>
    /// Mail Report Service.
    /// </summary>
    public class MailReportService : MailStationBaseService
    {
        private IMailService _mailServer;

        public MailReportService()
        {
            _mailServer = _mailServer ?? MailService.Create();
        }

        /// <summary>
        /// Gets the mail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail report.</returns>
        public List<MailTaskModel> MailReport(MailReportCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailTaskModel>().ToList();
            }
        }

        /// <summary>
        /// Gets the mail detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail detail.</returns>
        public MailRecord MailDetail(MailDetailCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailRecord>().ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the mail log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail log.</returns>
        public List<MailLogModel> MailLog(MailLogCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailLogModel>().ToList();
            }
        }

        /// <summary>
        /// Retry send mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Retry result.</returns>
        public void RetrySendMail(RetrySendMailCriteria criteria)
        {

        }
    }
}
