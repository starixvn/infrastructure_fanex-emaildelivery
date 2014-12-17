// --------------------------------------------------------------------------
// <copyright file="CreateRecipientCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Insert Recipient Criteria.
    /// </summary>
    public class CreateRecipientCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string UserName { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Note { get; set; }

        [SpParam(SqlParamName = "IsActive")]
        public bool Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_InsertRecipient";
        }
    }
}
