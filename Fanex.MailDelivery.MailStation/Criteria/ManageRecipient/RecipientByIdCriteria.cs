// --------------------------------------------------------------------------
// <copyright file="RecipientByIdCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Recipient By Identify Criteria.
    /// </summary>
    public class RecipientByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientById";
        }
    }
}
