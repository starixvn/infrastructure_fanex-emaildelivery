using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class AddMailLogCriteria : CriteriaBase
    {
        public int TaskID { get; set; }

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
