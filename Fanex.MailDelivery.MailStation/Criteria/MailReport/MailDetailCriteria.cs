// --------------------------------------------------------------------------
// <copyright file="MailDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Mail Detail Criteria.
    /// </summary>
    public class MailDetailCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailDetail";
        }
    }
}
