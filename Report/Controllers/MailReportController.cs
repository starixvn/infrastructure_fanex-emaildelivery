// --------------------------------------------------------------------------
// <copyright file="MailReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Report.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Report Controller.
    /// </summary>
    public class MailReportController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Areas/MailStation/Views/MailReport/Index.cshtml");
        }

        public ActionResult MailReport()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RetrySendMail()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MailDetail()
        {
            var mailDetail = new Mail
            {
                Subject = "asdasd adasd a",
                Body = "<div class=\"bold-weight\">Dear all,</div><br> Please be informed the package is released:..."
            };

            return View("~/Areas/MailStation/Views/MailDetail/Index.cshtml", mailDetail);
        }

        public ActionResult MailLog()
        {
            var mailLog = new List<Mail>() {
                new Mail
                {
                    Subject = "asdasd adasd a",
                    Body = "<div class=\"bold-weight\">Dear all,</div><br> Please be informed the package is released:..."
                }
            };

            return View("~/Areas/MailStation/Views/MailLog/Index.cshtml", mailLog);
        }
    }
}
