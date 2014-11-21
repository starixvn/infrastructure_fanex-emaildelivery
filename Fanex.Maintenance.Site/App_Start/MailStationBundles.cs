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
            RegisterEditMailBundles(bundles);
            RegisterInsertMailBundles(bundles);
            RegisterCopyMailBundles(bundles);
        }

        private static void RegisterManageMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/ManageMailScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/Areas/MailStation/Views/ManageMail/Scripts/ManageMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/ManageMailStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/Views/ManageMail/Styles/ManageMail.less");
        }

        private static void RegisterEditMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/EditMail/EditMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/Areas/MailStation/Views/EditMail/Scripts/EditMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/EditMail/EditMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/Views/EditMail/Styles/EditMail.less");
        }

        private static void RegisterInsertMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/InsertMail/InsertMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/Areas/MailStation/Views/InsertMail/Scripts/InsertMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/InsertMail/InsertMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/Views/InsertMail/Styles/InsertMail.less");
        }

        private static void RegisterCopyMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/CopyMail/CopyMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/Areas/MailStation/Views/CopyMail/Scripts/CopyMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/CopyMail/CopyMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/Views/CopyMail/Styles/CopyMail.less");
        }
    }
}
