// --------------------------------------------------------------------------
// <copyright file="MailReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Constants;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Services.MailReport;
    using Fanex.MailDelivery.MailStation.Services.ManageMail;
    using Fanex.Maintenance.Controller;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mail Report Controller.
    /// </summary>
    public class MailReportController : BaseController
    {
        private MailReportService _mailReportService;

        public MailReportController()
        {
            _mailReportService = _mailReportService ?? new MailReportService();
        }

        public ActionResult Index()
        {
            var mailManageService = new MailManageService();
            ViewBag.ListStatus = mailManageService.TaskStatus();
            return View("~/Areas/MailStation/Report/MailReport/Index.cshtml");
        }

        public ActionResult MailReport(MailReportCriteria criteria)
        {
            return Json(_mailReportService.MailReport(criteria));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult RetrySendMail(RetrySendMailCriteria criteria)
        {
            _mailReportService.RetrySendMail(criteria);
            return new EmptyResult();
        }

        public ActionResult MailDetail(int mailId)
        {
            var mailDetail = _mailReportService.MailDetail(new MailDetailCriteria
            {
                MailId = mailId
            });

            return View("~/Areas/MailStation/Report/MailDetail/Index.cshtml", mailDetail);
        }

        public ActionResult MailLog(int mailId)
        {
            var mailLog = _mailReportService.MailLog(new MailLogCriteria
            {
                MailId = mailId
            });

            return View("~/Areas/MailStation/Report/MailLog/Index.cshtml", mailLog);
        }
    }
}
