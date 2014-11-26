// --------------------------------------------------------------------------
// <copyright file="MailStationAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.9.6">Created</change>
// </history>

namespace Fanex.Maintenance.Site
{
    using System.Web.Mvc;

    /// <summary>
    /// User Login Area.
    /// </summary>
    public class MailStationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MailStation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.MailDelivery.MailStation.Controllers" });
        }
    }
}