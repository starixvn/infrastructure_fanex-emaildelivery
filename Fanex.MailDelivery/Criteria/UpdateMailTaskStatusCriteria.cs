namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateMailTaskMSiteMailIDCriteria : CriteriaBase
    {
        public int TaskID { get; set; }

        public int MSiteMailID { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_UpdateMailTaskMSiteMailID";
        }
    }
}
