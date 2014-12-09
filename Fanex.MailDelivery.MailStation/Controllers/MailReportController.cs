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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.MailDelivery.MailStation.Constants;
    using Fanex.MailDelivery.MailStation.Criteria.MailReport;
    using Fanex.MailDelivery.MailStation.Services.MailReport;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mail Report Controller.
    /// </summary>
    public class MailReportController : Controller
    {
        private IMailReportService _mailReportService;

        public MailReportController()
            : this(null)
        {
        }

        public MailReportController(IMailReportService mailReportService)
        {
            _mailReportService = mailReportService ?? new MailReportService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/MailStation/Report/MailReport/Index.cshtml");
        }

        public ActionResult MailReport(MailReportCriteria criteria)
        {
            return Json(_mailReportService.MailReport(criteria));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult RetrySendMail(RetrySendMailCriteria criteria)
        {
            return Json(_mailReportService.RetrySendMail(criteria));
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
