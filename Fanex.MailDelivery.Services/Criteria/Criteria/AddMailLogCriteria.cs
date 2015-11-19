using System;
using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Criteria
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class AddMailLogCriteria : CriteriaBase
    {
        public int TaskId { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public int Status { get; set; }

        public DateTime LogTime { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_InsertMailLogs";
        }
    }
}
