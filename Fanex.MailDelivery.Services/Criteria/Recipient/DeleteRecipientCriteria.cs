// --------------------------------------------------------------------------
// <copyright file="DeleteRecipientCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2015.03.30">Created</change>
// </history>

using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Recipient
{
    /// <summary>
    /// Deletes Recipient Criteria.
    /// </summary>
    public class DeleteRecipientCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_DeleteRecipient";
        }
    }
}