using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Report;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IReportService
    {
        GetReportListResponse GetReportList(GetReportListRequest request);
    }

    public class ReportService : BaseAdminService, IReportService
    {
        public GetReportListResponse GetReportList(GetReportListRequest request)
        {
            IList<MailTaskModel> mailTaskModels = adminDataService.GetReportList(new GetReportListCriteria
            {
                MailName = request.MailName,
                SiteId = request.SiteId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = request.Status
            });

            return new GetReportListResponse { MailTaskModels = mailTaskModels };
        }
    }
}