// --------------------------------------------------------------------------
// <copyright file="UserLogInAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.6">Created</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin
{
    using System.Web.Mvc;

    /// <summary>
    /// User Login Area.
    /// </summary>
    public class UserLoginAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserLogin";
            }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">Area Registration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_DefaultAreas",
                "UserLogin/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional });

            context.MapRoute(AreaName + "_Themes", "UserLogin/Themes/{resourceName}");
        }
    }
}