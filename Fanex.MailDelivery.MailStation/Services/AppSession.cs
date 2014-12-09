// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Bao" date="2012.10.18">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.22">Update Session Manager</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// Application Session.
    /// </summary>
    public static class AppSession
    {
        public const string CurrentLogInUser = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentLogInUser";

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(CurrentLogInUser, value);
            }
        }
    }
}
