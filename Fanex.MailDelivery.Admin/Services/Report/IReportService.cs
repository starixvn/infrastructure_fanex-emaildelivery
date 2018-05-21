using Fanex.MailDelivery.Admin.Messaging.ManageReport;

namespace Fanex.MailDelivery.Admin.Services.Report
{
    public interface IReportService
    {
        GetReportListResponse GetReportList(GetReportListRequest request);
    }
}