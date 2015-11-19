// --------------------------------------------------------------------------
// <copyright file="GetMailTaskByIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Criteria
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailTaskByIdCriteria : CriteriaBase
    {
        public int MailTaskId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_GetMailTask";
        }
    }
}
