// --------------------------------------------------------------------------
// <copyright file="IMailReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Repositories.MailReport
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Models;

    public interface IMailReportRepository
    {
        List<MailModel> MailReport(MailReportCriteria criteria);

        List<MailLogModel> MailLog(MailLogCriteria criteria);
    }
}
