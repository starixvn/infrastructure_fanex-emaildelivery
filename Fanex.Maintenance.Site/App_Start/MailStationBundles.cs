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

            RegisterMailReportBundles(bundles);
            RegisterMailDetailBundles(bundles);
            RegisterMailLogBundles(bundles);
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

        private static void RegisterMailReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailReport/MailReportScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/custom-datepicker.js",
                "~/scripts/lib/Countdown/CountdownController.js",
                "~/scripts/lib/Popup/dialogbox.js",
                "~/Areas/MailStation/Views/MailReport/Scripts/MailReportController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailReport/MailReportStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/Countdown/CountDown.less",
                "~/Areas/MailStation/Views/MailReport/Styles/MailReport.less");
        }

        private static void RegisterMailDetailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailDetail/MailDetailScripts").Include(
                "~/Areas/MailStation/Views/MailDetail/Scripts/MailDetailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailDetail/MailDetailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/Views/MailDetail/Styles/MailDetail.less");
        }

        private static void RegisterMailLogBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailLog/MailLogScripts").Include(
                "~/Areas/MailStation/Views/MailLog/Scripts/MailLogController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailLog/MailLogStyles",
                "~/Areas/MailStation/Views/MailLog/Styles/MailLog.less");
        }
    }
}
