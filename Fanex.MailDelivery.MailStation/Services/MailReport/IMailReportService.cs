// --------------------------------------------------------------------------
// <copyright file="IMailReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services.MailReport
{
    using System.Collections.Generic;
    using Fanex.MailDeliver.Model;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;

    public interface IMailReportService
    {
        List<MailModel> MailReport(MailReportCriteria criteria);

        MailRecord MailDetail(MailDetailCriteria criteria);

        List<MailLogModel> MailLog(MailLogCriteria criteria);

        ResultModel RetrySendMail(RetrySendMailCriteria criteria);
    }
}
