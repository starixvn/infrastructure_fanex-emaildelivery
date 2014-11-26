// --------------------------------------------------------------------------
// <copyright file="MailRecipientController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Mail Recipient Controller.
    /// </summary>
    public class MailRecipientController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Areas/MailStation/Views/ManageRecipient/Index.cshtml");
        }

        public ActionResult InsertRecipientView()
        {
            return View("~/Areas/MailStation/Views/InsertRecipient/Index.cshtml");
        }

        public ActionResult RecipientList()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditRecipientView()
        {
            var recipient = new Mail
            {
                Subject = "asdasd adasd a",
                Body = "<div class=\"bold-weight\">Dear all,</div><br> Please be informed the package is released:..."
            };

            return View("~/Areas/MailStation/Views/EditRecipient/Index.cshtml", recipient);
        }

        [HttpPost]
        public JsonResult EditRecipient()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertRecipient()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckRecipient()
        {
            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }
    }
}
