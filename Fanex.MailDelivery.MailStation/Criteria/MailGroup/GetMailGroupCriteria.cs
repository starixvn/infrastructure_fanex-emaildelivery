// --------------------------------------------------------------------------
// <copyright file="GetMailGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailGroupCriteria : CriteriaBase
    {
        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_GetMailGroup";
        }
    }
}
