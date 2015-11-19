// --------------------------------------------------------------------------
// <copyright file="GetMailGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Group
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetGroupListCriteria : CriteriaBase
    {
        public string MailGroupName { get; set; }

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
