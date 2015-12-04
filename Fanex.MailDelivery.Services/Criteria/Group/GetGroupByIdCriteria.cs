using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Group
{
    public class GetGroupByIdCriteria : CriteriaBase
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
