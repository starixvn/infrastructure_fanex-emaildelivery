// --------------------------------------------------------------------------
// <copyright file="ReadMailByIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailByIdCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetMailByID";
        }
    }
}
