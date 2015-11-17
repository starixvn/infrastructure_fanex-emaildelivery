using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Server.Criteria.Group
{
    public class GetGroupByIdCriteria: CriteriaBase
    {
        public int MailGroupId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_GetMailGroupByID";
        }
    }
}
