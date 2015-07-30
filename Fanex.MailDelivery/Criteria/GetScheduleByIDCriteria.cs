namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetScheduleByIdCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_MailManagement_GetScheduleByMailId";
        }
    }
}
