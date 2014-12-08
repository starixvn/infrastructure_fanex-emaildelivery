// --------------------------------------------------------------------------
// <copyright file="MailManageController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.MailDelivery.MailStation.Services.ManageMail;
using Fanex.MailDelivery.MailStation.Criterias;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailManageController : Controller
    {
        private MailManageService mailManageService;
        
        public ActionResult Index()
        {
            return View("~/Areas/MailStation/ManageMail/Index.cshtml");
        }

        public JsonResult Sites()
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.Sites());
        }

        public JsonResult MailGroups()
        {
            mailManageService = new MailManageService();
            return Json(mailManageService.MailGroups());
        }

        public ActionResult InsertMail()
        {
            return View("~/Areas/MailStation/ManageMail/InsertMail/Index.cshtml");
        }

        public ActionResult CopyMail()
        {
            return View("~/Areas/MailStation/ManageMail/CopyMail/Index.cshtml");
        }

        public ActionResult MailList(GetMailListCriteria criteria)
        {

            return Json(new List<int>() { 1, 2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteMail()
        {
            return Json(new List<int>() { 3 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditMail(int mailId)
        {
            var mailInfo = new
            {
                Id = 1,
                MailName = "Name 1",
                Group = "Licensee",
                Site = "System Configuration",
                TriggerType = "By Function",
                To = "abc@gmail.com",
                CC = "",
                BCC = "",
                LastSent = "Sep 24,2014 06:08 AM",
                Active = true
            };

            return View("~/Areas/MailStation/ManageMail/EditMail/Index.cshtml", mailInfo);
        }

        [HttpPost]
        public JsonResult EditMail()
        {
            return Json(new List<int>() { 3 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SuggestMail(SuggestMailCriteria criteria)
        {
            var jsonResult = new
            {
                query = criteria.Query,
                suggestions = new List<string>()
                {
                    "louis1@nexcel.vn",
                    "louis2@nexcel.vn",
                    "louis3@nexcel.vn",
                    "louis4@nexcel.vn",
                    "louis5@nexcel.vn"
                }
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SuggestMultiMail(string tag)
        {
            var jsonResult = new List<dynamic>()
            {
               new {
                    key = "1",
                    value =   "louis1@nexcel.vn"
               },
               new {
                    key = "2",
                    value =   "louis2@nexcel.vn"
               },
               new {
                    key = "3",
                    value =   "louis3@nexcel.vn"
               },
               new {
                    key = "4",
                    value =   "louis4@nexcel.vn"
               },
               new {
                    key = "5",
                    value =   "louis5@nexcel.vn"
               }
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}
