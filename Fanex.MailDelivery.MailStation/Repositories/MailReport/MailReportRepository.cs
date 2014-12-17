// --------------------------------------------------------------------------
// <copyright file="MailReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Repositories.MailReport
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Constants;

    /// <summary>
    /// Mail Report Repository.
    /// </summary>
    public class MailReportRepository : IMailReportRepository
    {
        /// <summary>
        /// Gets the mail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail report.</returns>
        public List<MailModel> MailReport(MailReportCriteria criteria)
        {
            List<MailModel> result = new List<MailModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var statusId = Converter.ToInt(reader["Status"]);
                    var mailModel = new MailModel
                    {
                        MailId = Converter.ToInt(reader["ID"]),
                        MailName = Converter.ToString2(reader["MailName"]),
                        SiteName = Converter.ToString2(reader["SiteName"]),

                        LastSentString = Converter.ToString2(reader["LastSent"]),
                        StatusId = statusId,
                        StatusName = TaskStatusConstants.StatusName(statusId),
                        ScheduledTimeString = Converter.ToString2(reader["ScheduledTime"])
                    };

                    result.Add(mailModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the mail log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail log.</returns>
        public List<MailLogModel> MailLog(MailLogCriteria criteria)
        {
            List<MailLogModel> result = new List<MailLogModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var logModel = new MailLogModel
                    {
                        Id = Converter.ToInt(reader["ID"]),
                        StatusName = Converter.ToString2(reader["Status"]),
                        Message = Converter.ToString2(reader["ExceptionMessage"]),
                        LogTime = Converter.ToString2(reader["LogTime"]),
                        StackTrace = Converter.ToString2(reader["StackTrace"])
                    };

                    result.Add(logModel);
                }
            }

            return result;
        }
    }
}
