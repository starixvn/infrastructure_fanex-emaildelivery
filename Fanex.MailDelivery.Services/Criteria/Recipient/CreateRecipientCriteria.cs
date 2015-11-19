// -----------------------------------------------------------------------
// <copyright file="CreateRecipientCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Recipient
{
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

        public bool IsNAPUser { get; set; }

        public int NAPUserID { get; set; }

        public string Note { get; set; }

        [SpParam(SqlParamName = "IsActive")]
        public bool Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_InsertRecipient";
        }
    }
}
