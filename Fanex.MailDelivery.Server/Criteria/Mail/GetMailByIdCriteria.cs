using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Server.Criteria.Mail
{
    public class GetMailByIdCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetMailByID";
        }
    }
}
