// --------------------------------------------------------------------------
// <copyright file="GetMailListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailListCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int IsActive { get; set; }

        public int MailGroupID { get; set; }

        public int SiteID { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetMailList";
        }
    }
}
