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
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Repositories.MailReport;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;

    /// <summary>
    /// Mail Report Service.
    /// </summary>
    public class MailReportService : IMailReportService
    {
        private IMailReportRepository _mailReportRepository;

        private IMailService _mailServer;

        public MailReportService()
            : this(null, null)
        {
        }

        public MailReportService(
            IMailReportRepository mailReportRepository,
            MailService mailService)
        {
            _mailReportRepository = mailReportRepository ?? new MailReportRepository();
            _mailServer = mailService ?? MailService.Create();
        }

        /// <summary>
        /// Gets the mail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail report.</returns>
        public List<MailModel> MailReport(MailReportCriteria criteria)
        {
            try
            {
                return _mailReportRepository.MailReport(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the mail detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail detail.</returns>
        public MailRecord MailDetail(MailDetailCriteria criteria)
        {
            try
            {
                return _mailServer.GetMail(criteria.MailId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the mail log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mail log.</returns>
        public List<MailLogModel> MailLog(MailLogCriteria criteria)
        {
            try
            {
                return _mailReportRepository.MailLog(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Retry send mail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Retry result.</returns>
        public ResultModel RetrySendMail(RetrySendMailCriteria criteria)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }
    }
}
