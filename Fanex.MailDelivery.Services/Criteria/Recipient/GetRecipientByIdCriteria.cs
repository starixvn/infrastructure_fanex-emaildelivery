using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Recipient
{
    /// <summary>
    /// Recipient By Identify Criteria.
    /// </summary>
    public class GetRecipientByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientById";
        }
    }
}
