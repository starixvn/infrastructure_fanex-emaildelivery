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
    public class UpdateMailTaskStatusCriteria : CriteriaBase
    {
        public int TaskID { get; set; }

        public int Status { get; set; }

        public DateTime? LastSent { get; set; } 

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_UpdateMailStatus";
        }
    }
}
