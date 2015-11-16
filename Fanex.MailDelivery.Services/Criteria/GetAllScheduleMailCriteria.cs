using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Services.Criteria
{
    /// <summary>
    /// Get all schedule mail.
    /// </summary>
    public class GetAllScheduleMailCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_GetAllScheduleMail";
        }
    }
}
