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
    public class UpdateTaskRetryCounterCriteria : CriteriaBase
    {
        public int TaskID { get; set; }

        public int RetryCount { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_UpdateRetry";
        }
    }
}
