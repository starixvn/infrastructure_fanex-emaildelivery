namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetAllScheduleCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int SiteID { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Report_GetAllSchedule";
        }
    }
}
