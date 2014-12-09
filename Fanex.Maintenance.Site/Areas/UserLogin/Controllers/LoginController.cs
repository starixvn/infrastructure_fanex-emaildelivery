// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Logging;
    using Fanex.MailDelivery.MailStation.Services;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private IGroupService _groupService;
        private ISiteService _siteService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;
        private IMaintenanceService _maintenanceService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IMaintenanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _groupService = groupService ?? new GroupService();
            _siteService = siteService ?? new SiteService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        #endregion ctors

        #region sign in

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (!NapSettings.Settings.IsNapHub && !this.IsSiteLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            if (maintenanceInfo.IsDowntime)
            {
                return RedirectToAction("Offline", "Common");
            }

            return View();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="username">Login username.</param>
        /// <param name="password">Login password.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = _userService.GetByUsername(username.Trim()) as User;

                try
                {
                    if (user != null)
                    {
                        password = Cryptography.CfsEnCode(password);

                        if (password == user.Password)
                        {
                            var group = user.Group == null ? null : _groupService.GetById(user.Group.Id);
                            var siteLevelList = _siteService.GetSiteLevelByUser(user.Id, Environment); // gets all sites of logging-in user

                            if ((group == null || group.IsActive) && siteLevelList.Where(x => x.IsActive).Any())
                            {
                                // check-in user
                                var criteria = new UserCheckInUserCriteria
                                {
                                    UserId = user.Id,
                                    SiteId = this.SiteId,
                                    NetSessionId = Session.SessionID,
                                    Ip = CurrentContext.IP,
                                    LoginDate = DateTime.Now,
                                    BrowserName = CurrentContext.BrowserName,
                                    CountryCode = CurrentContext.CountryCode,
                                    Domain = CurrentContext.DomainName,
                                    Platform = CurrentContext.Platform
                                };

                                _userService.UserCheckIn(ref criteria);

                                // saves user session
                                _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);
                                _currentSession.UserId = user.Id;
                                _currentSession.Username = username;
                                _currentSession.OnUserId = criteria.OnUserId;
                                _currentSession.SessionId = Session.SessionID.ToString();

                                // current site is not NAP Hub
                                if (this.SiteId != 0)
                                {
                                    CreateUserMenu(user);
                                }

                                CreateAuthInfo(user);
                                AppSession.CurrentUser = user;

                                // redirects user

                                // if the user login directly, not through NAP Hub
                                if (this.IsSiteLogin)
                                {
                                    // checks if logging-in user is active on this site
                                    bool userIsActiveOnThisSite = false;

                                    foreach (var siteLevel in siteLevelList)
                                    {
                                        if (siteLevel.Site.Id == this.SiteId && siteLevel.IsActive)
                                        {
                                            userIsActiveOnThisSite = true;
                                        }
                                    }

                                    if (userIsActiveOnThisSite)
                                    {
                                        _currentSession.MenuItemList = _menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    Environment,
                                                                                    true) as IList<MenuItem>;

                                        return Redirect(FormsAuthentication.DefaultUrl);
                                    }
                                    else
                                    {
                                        DoSignOut();
                                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                                    }
                                }
                                else
                                {
                                    // user has access to multiple sites
                                    if (siteLevelList.Where(s => s.IsActive).Count() > 1)
                                    {
                                        return RedirectToAction("Site", "UserSite");
                                    }

                                    // user has access to one site
                                    var site = siteLevelList.Where(s => s.IsActive).First().Site;

                                    var listSiteUrl = _siteService.GetSiteUrlBySiteId(site.Id, this.Environment).Where(u => u.IsActive).ToList();

                                    if (listSiteUrl.Count() > 1)
                                    {
                                        return RedirectToAction("Site", "UserSite");
                                    }

                                    return RedirectToAction("Forward", "UserSite", new { id = listSiteUrl.First().SiteUrlId });
                                }
                            }
                            else
                            {
                                // user account was suspended on all sites
                                if (siteLevelList.Count() > 0)
                                {
                                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                                }
                                else
                                {
                                    // logging-in user belongs to no site
                                    ViewBag.Message = ViewResources.You_have_no_authorization;
                                }
                            }
                        }
                        else
                        {
                            // wrong password
                            ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                        }
                    }
                    else
                    {
                        // user doesn't exist
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw;
                }
            }
            else
            {
                ViewBag.Message = "Invalid input!";
            }

            return View();
        }

        #endregion sign in

        #region sign out

        /// <summary>
        /// User log out.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion sign out

        #region re check in

        /// <summary>
        /// User recheck login.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return Json("-1");
            }

            // check if not nap hub
            if (this.SiteId != 0)
            {
                var siteStatus = _siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    return Json("1");
                }
            }

            UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(_currentSession.OnUserId);
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion re check in

        #region helpers

        /// <summary>
        /// Create user menu.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateUserMenu(User user)
        {
            _currentSession.MenuItemList = (IList<MenuItem>)_menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    Environment,
                                                                                    true);
        }

        /// <summary>
        /// Create authorization info.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion helpers

        /// <summary>
        /// User sign out.
        /// </summary>
        private void DoSignOut()
        {
            _userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = _currentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            Response.Cookies.Clear();
            _currentSession.ClearAll();
        }
    }
}