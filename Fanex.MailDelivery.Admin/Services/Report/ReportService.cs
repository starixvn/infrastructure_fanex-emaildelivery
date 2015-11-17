using Fanex.MailDelivery.Admin.Messaging.ManageReport;

namespace Fanex.MailDelivery.Admin.Services.Report
{
    public class ReportService : AdminService, IReportService
    {
        public GetReportListResponse GetReportList(GetReportListRequest request)
        {
            return Bus.Request<GetReportListRequest, GetReportListResponse>(request);
        }
    }
}