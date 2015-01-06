// --------------------------------------------------------------------------
// <copyright file="GetMailTaskByIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Mail Report Criteria.
    /// </summary>
    public class GetMailTaskByIDCriteria : CriteriaBase
    {
        public int MailTaskID { get; set; }

        public override string GetSettingKey()
        {
            return "Mail_Report_GetMailTaskByID";
        }
    }
}
