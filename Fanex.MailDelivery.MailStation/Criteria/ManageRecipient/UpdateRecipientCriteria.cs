// --------------------------------------------------------------------------
// <copyright file="UpdateRecipientCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
//   <change who="Andy.Nguyen" date="2015.04.01">Update Email IDs param.</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Recipient Criteria.
    /// </summary>
    public class UpdateRecipientCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        [SpParam(SqlParamName = "RecipientName")]
        public string Username { get; set; }

        public int EmailID1 { get; set; }

        public int EmailID2 { get; set; }

        public int EmailID3 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Note { get; set; }

        [SpParam(SqlParamName = "IsActive")]
        public bool Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_UpdateRecipient";
        }
    }
}