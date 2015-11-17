// --------------------------------------------------------------------------
// <copyright file="MailReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Server.Criteria.Report
{
    /// <summary>
    /// Mail Report Criteria.
    /// </summary>
    public class GetReportListCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int SiteId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }
    }
}
