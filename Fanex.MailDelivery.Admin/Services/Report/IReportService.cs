using System;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;

namespace Fanex.MailDelivery.Admin.Services.Report
{
    public interface IReportService : IDisposable
    {
        GetReportListResponse GetReportList(GetReportListRequest request);
    }
}
