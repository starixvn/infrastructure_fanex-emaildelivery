using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Admin.Criteria.Report
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetAllScheduleCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int SiteId { get; set; }

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
