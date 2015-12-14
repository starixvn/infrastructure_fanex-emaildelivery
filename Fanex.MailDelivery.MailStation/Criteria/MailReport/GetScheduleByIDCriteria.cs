using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
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
