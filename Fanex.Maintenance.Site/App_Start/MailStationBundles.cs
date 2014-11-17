// --------------------------------------------------------------------------
// <copyright file="FeedbackBundles.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.Maintenance.Site.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Mail Station Bundles.
    /// </summary>
    public static class MailStationBundles
    {
        public static void RegisterAllBundles(BundleCollection bundles)
        {
            RegisterManageMailBundles(bundles);
        }

        private static void RegisterManageMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/ManageMailScripts").Include(
                "~/Areas/MailStation/Views/ManageMail/Scripts/ManageMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/ManageMailStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/Views/ManageMail/Styles/ManageMail.less");
        }
    }
}
