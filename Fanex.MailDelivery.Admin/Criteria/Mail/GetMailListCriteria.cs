// -----------------------------------------------------------------------
// <copyright file="GetMailListCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Admin.Criteria.Mail
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailListCriteria : CriteriaBase
    {
        public GetMailListCriteria()
        {
            this.IsActive = -1;
            this.TriggerType = -1;
            this.SiteId = -1;
        }

        public string MailName { get; set; }

        public int IsActive { get; set; }

        public string MailGroupId { get; set; }

        public int SiteId { get; set; }

        public int TriggerType { get; set; }

        public string Emails { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetMailList";
        }
    }
}
