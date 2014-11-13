// --------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.6">Created</change>
// </history>

namespace Fanex.Maintenance.Site
{
    using System.Web.Http;

    /// <summary>
    /// Web API configuration.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register route.
        /// </summary>
        /// <param name="config">HTTP configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}