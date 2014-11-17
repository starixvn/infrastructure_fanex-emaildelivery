// --------------------------------------------------------------------------
// <copyright file="MailController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.MailDelivery.Client.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    public class MailController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Areas/MailStation/Views/ManageMail/Index.cshtml");
        }

        public ActionResult InsertMail()
        {
            return View("~/Areas/MailStation/Views/InsertMail/Index.cshtml");
        }
    }
}
