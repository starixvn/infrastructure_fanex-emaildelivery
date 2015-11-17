// --------------------------------------------------------------------------
// <copyright file="UpdateMailGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Server.Criteria.Group
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateGroupCriteria : CriteriaBase
    {
        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_UpdateMailGroup";
        }
    }
}
