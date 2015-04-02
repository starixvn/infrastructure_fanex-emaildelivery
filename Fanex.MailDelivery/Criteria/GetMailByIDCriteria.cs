// --------------------------------------------------------------------------
// <copyright file="GetMailingListTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailingListTypeCriteria : CriteriaBase
    {
        public int MailID { get; set; }

        public int MailType { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_GetMailingListType";
        }
    }
}
