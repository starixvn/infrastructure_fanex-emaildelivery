#region

using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models.Report;

#endregion

namespace Fanex.MailDelivery.Admin.Messaging.ManageReport
{
    public class GetReportListResponse
    {
        public IList<MailTaskModel> MailTaskModels { get; set; }
    }
}