// --------------------------------------------------------------------------
// <copyright file="CheckRecipientInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Check Recipient Information Criteria.
    /// </summary>
    public class CheckRecipientInfoCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_InsertRecipient";
        }
    }
}
