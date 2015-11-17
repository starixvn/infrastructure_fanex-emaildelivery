using System;

namespace Fanex.MailDelivery.Admin.Messaging.ManageReport
{
    public class GetReportListRequest
    {
        public string MailName { get; set; }

        public int SiteId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Status { get; set; }
    }
}
