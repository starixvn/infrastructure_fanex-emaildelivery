// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.20">Update Check Permission</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionService
    {
        private static IFunctionService _functionService;

        private static IUserService _userService;

        private static Dictionary<string, int> _roleAccess;

        static PermissionService()
        {
            _userService = new UserService();
            _functionService = new FunctionService();
        }

        /// <summary>
        /// GetRollAccess: get permission to access with each menu item of current user login.
        /// </summary>
        /// <param name="currentFunctions">The current functions.</param>
        /// <returns>
        /// Roll Access.
        /// </returns>
        public static Dictionary<string, int> GetRoleAccess(IEnumerable<FunctionPolicy> currentFunctions)
        {
            Dictionary<string, int> roleAccess = new Dictionary<string, int>();

            foreach (FunctionPolicy functionPolicy in currentFunctions)
            {
                if (functionPolicy.Policy >= (int)Constant.Policy.Read)
                {
                    roleAccess.Add(functionPolicy.Function.Key, functionPolicy.Policy);
                }
            }

            return roleAccess;
        }

        /// <summary>
        /// HasPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>
        /// Has Read Permission indicator.
        /// </returns>
        public static bool HasReadPermission(string functionName)
        {
            UpdateRoleAccess();
            return _roleAccess != null &&
                   _roleAccess.ContainsKey(functionName) &&
                   _roleAccess[functionName] == (int)Constant.Policy.Read;
        }

        public static bool HasHostPermission(string functionName)
        {
            UpdateRoleAccess();
            return _roleAccess != null &&
                   _roleAccess.ContainsKey(functionName) &&
                   _roleAccess[functionName] == (int)Constant.Policy.Host;
        }

        public static bool HasPermission(string functionName)
        {
            UpdateRoleAccess();
            return _roleAccess != null &&
                   _roleAccess.ContainsKey(functionName) &&
                   _roleAccess[functionName] >= (int)Constant.Policy.Read;
        }

        /// <summary>
        /// Updates the role access.
        /// </summary>
        private static void UpdateRoleAccess()
        {
            if (AppSession.CurrentUser == null)
            {
                return;
            }

            _roleAccess = GetRoleAccess(_functionService.GetByUserAndSiteIncludeGroup(
                AppSession.CurrentUser.Id,
                NapSettings.Settings.SiteId,
                NapSettings.Settings.Environment));
        }
    }
}