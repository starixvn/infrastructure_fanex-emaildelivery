// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web.Context;

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly CurrentContext CurrentContext;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// </summary>
        /// <value>0 : users must login through NAP Hub.</value>
        public bool IsSiteLogin
        {
            get
            {
                return _issiteLogin;
            }
        }
    }
}