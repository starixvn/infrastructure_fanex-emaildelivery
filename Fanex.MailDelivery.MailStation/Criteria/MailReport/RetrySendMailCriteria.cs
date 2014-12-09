// --------------------------------------------------------------------------
// <copyright file="RetrySendMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Retry Send Mail Criteria.
    /// </summary>
    public class RetrySendMailCriteria : CriteriaBase
    {
        public string MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }
    }
}
