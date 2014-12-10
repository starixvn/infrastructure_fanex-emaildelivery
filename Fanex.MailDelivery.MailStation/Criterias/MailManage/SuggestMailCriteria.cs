// --------------------------------------------------------------------------
// <copyright file="SuggestMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criterias.MailManage
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Suggest Mail Criteria.
    /// </summary>
    public class SuggestMailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "EmailAddress")]
        public string Query { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllSuggestedMails";
        }
    }
}
