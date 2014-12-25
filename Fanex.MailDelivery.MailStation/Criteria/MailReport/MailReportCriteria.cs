// --------------------------------------------------------------------------
// <copyright file="MailReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.NAP.Framework.Data;
    using System;

    /// <summary>
    /// Mail Report Criteria.
    /// </summary>
    public class MailReportCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }
    }
}
