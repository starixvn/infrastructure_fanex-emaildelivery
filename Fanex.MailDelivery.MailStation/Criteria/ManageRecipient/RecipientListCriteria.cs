// --------------------------------------------------------------------------
// <copyright file="RecipientListCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Recipient List Criteria.
    /// </summary>
    public class RecipientListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "Email")]
        public string Emails { get; set; }

        public string Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientList";
        }
    }
}
