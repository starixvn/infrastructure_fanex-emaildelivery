// --------------------------------------------------------------------------
// <copyright file="UserSiteController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.6">Created</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// User site controller.
    /// </summary>
    public class UserSiteController : BaseController
    {
        #region members

        private ISiteService _siteService;
        private ICurrentSession _currentSession;
        private IUserService _userService;

        #endregion members

        #region ctors

        public UserSiteController()
            : this(null, null, null)
        {
        }

        public UserSiteController(ISiteService siteService, ICurrentSession currentSession, IUserService userService)
        {
            _siteService = siteService ?? new SiteService();
            _currentSession = currentSession ?? new CurrentSession();
            _userService = userService ?? new UserService();
        }

        #endregion ctors

        #region actions

        /// <summary>
        /// Get sites.
        /// </summary>
        /// <returns>Return view site list.</returns>
        public ActionResult Site()
        {
            if (HttpContext.User.Identity.IsAuthenticated && _currentSession.UserId > 0)
            {
                var siteUrlList = _siteService.GetSiteUrlByUser(_currentSession.UserId, this.Environment);

                // for re-checkin
                ViewBag.OnUserId = _currentSession.OnUserId;

                return View(siteUrlList);
            }

            return RedirectToAction("logout", "login");
        }

        /// <summary>
        /// SSO to selected sites.
        /// </summary>
        /// <param name="id">Site url id.</param>
        /// <returns>Redirect to site.</returns>
        [HttpGet]
        public ActionResult Forward(string id)
        {
            int siteUrlId = Convert.ToInt32(id);
            var siteUrl = _siteService.GetSiteUrlById(siteUrlId);

            string time = _userService.UserSSOGetTime();

            return new RedirectResult(
                string.Format(
                    "{0}/userlogin/sso?userid={1}&onuserid={2}&time={3}",
                    siteUrl.Url.Trim(),
                    Cryptography.CfsEnCrypt(_currentSession.UserId.ToString()),
                    Cryptography.CfsEnCrypt(_currentSession.OnUserId.ToString()),
                    Cryptography.CfsEnCrypt(time)));
        }

        #endregion actions
    }
}