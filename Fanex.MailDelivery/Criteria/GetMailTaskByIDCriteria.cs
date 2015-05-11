// --------------------------------------------------------------------------
// <copyright file="GetMailTaskByIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailTaskByIDCriteria : CriteriaBase
    {
        public int MailTaskID { get; set; }

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
