using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.MailDelivery.Services.Criteria
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class AddMailTaskCriteria : CriteriaBase
    {
        public int MailTaskId { get; set; }

        public int MailId { get; set; }

        public int SiteId { get; set; }

        public DateTime ScheduledTime { get; set; }

        public DateTime? LastSent { get; set; }

        public int Status { get; set; }

        public int Attempt { get; set; }

        public int MSiteMailId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_InsertMailTask";
        }
    }
}
