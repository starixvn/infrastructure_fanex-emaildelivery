// -----------------------------------------------------------------------
// <copyright file="RecipientListCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Server.Criteria.Recipient
{
    /// <summary>
    /// Recipient List Criteria.
    /// </summary>
    public class GetRecipientListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string UserName { get; set; }

        [SpParam(SqlParamName = "Email")]
        public string Emails { get; set; }

        public string Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientList";
        }
    }
}
