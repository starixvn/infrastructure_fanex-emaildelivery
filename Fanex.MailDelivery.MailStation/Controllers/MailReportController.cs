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
    using Fanex.MailDelivery.MailStation.Extensions;
    using Fanex.MailDelivery.MailStation.Services;
    using Fanex.Maintenance.Controller;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mail Report Controller.
    /// </summary>
    [NapSuperAuth]
    public class MailReportController : BaseController
    {
        private MailReportService _mailReportService;

        public MailReportController()
        {
            _mailReportService = _mailReportService ?? new MailReportService();
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.ListStatus = _mailReportService.TaskStatus();
            ViewBag.ListSite = _mailReportService.Sites();
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/Report/MailReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailReport(MailReportCriteria criteria)
        {
            // MailAdminService.Start();
            // GetReportListRequest request = new GetReportListRequest
            // {
            //     MailName = criteria.MailName,
            //     SiteId = criteria.SiteId,
            //     FromDate = criteria.FromDate,
            //     ToDate = criteria.ToDate,
            //     Status = criteria.Status
            // };
               
            // GetReportListResponse response = MailAdminService.GetReportList(request);
               
            // MailAdminService.Stop();
            return Json(_mailReportService.MailReport(criteria));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailDetail(int mailId)
        {
            var mailDetail = _mailReportService.MailDetail(new MailDetailCriteria { MailId = mailId });

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/Report/MailDetail/Index.cshtml", mailDetail);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailLog(int mailId)
        {
            var mailLog = _mailReportService.MailLog(new MailLogCriteria { MailId = mailId });

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/Report/MailLog/Index.cshtml", mailLog);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
        public void AbortTask(int taskId)
        {
            _mailReportService.AbortJob(taskId);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult RetrySendMail(RetrySendMailCriteria criteria)
        {
            _mailReportService.RetrySendMail(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
        public ActionResult RetryMissingTask(RetryMissingTaskCriteria criteria)
        {
            _mailReportService.RetryMissingJob(criteria);
            return new EmptyResult();
        }
    }
}
