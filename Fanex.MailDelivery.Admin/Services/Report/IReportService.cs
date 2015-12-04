#region

using System;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;

#endregion

namespace Fanex.MailDelivery.Admin.Services.Report
{
    public interface IReportService : IDisposable
    {
        GetReportListResponse GetReportList(GetReportListRequest request);
    }
}