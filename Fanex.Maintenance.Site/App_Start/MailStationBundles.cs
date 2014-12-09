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
            RegisterNexCron(bundles);
            RegisterMagicSuggest(bundles);
            RegisterManageMailBundles(bundles);
            RegisterUpdateMailBundles(bundles);
            RegisterCreateMailBundles(bundles);
            RegisterCopyMailBundles(bundles);

            RegisterMailReportBundles(bundles);
            RegisterMailDetailBundles(bundles);
            RegisterMailLogBundles(bundles);

            RegisterManageRecipientBundles(bundles);
            RegisterEditRecipientBundles(bundles);
            RegisterInsertRecipientBundles(bundles);
            RegisterRelevantMailByRecipientBundles(bundles);
        }

        private static void RegisterNexCron(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/lib/NexCron/js").Include(
                "~/scripts/lib/NexCron/js/NexCron.js"));

            bundles.AddAndInclude(
                "~/scripts/lib/NexCron/css",
                "~/scripts/lib/NexCron/css/NexCron.css"
                );
        }

        private static void RegisterMagicSuggest(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/lib/MagicSuggest/js").Include(
                "~/scripts/lib/MagicSuggest/js/magicsuggest-min.js"));

            bundles.AddAndInclude(
                "~/scripts/lib/MagicSuggest/css",
                "~/scripts/lib/MagicSuggest/css/bootstrap.min.css",
                "~/scripts/lib/MagicSuggest/css/magicsuggest-min.css"
                );
        }

        private static void RegisterManageMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/ManageMailScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageMail/Scripts/MailManageController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/ManageMailStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageMail/Styles/ManageMail.less");

        }

        private static void RegisterUpdateMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/UpdateMail/UpdateMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/Areas/MailStation/ManageMail/UpdateMail/Scripts/UpdateMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/UpdateMail/UpdateMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/ManageMail/UpdateMail/Styles/UpdateMail.less");
        }

        private static void RegisterCreateMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/CreateMail/CreateMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/Areas/MailStation/ManageMail/CreateMail/Scripts/CreateMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/CreateMail/CreateMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/ManageMail/CreateMail/Styles/CreateMail.less");
        }

        private static void RegisterCopyMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/CopyMail/CopyMailScripts").Include(
                "~/scripts/lib/custom-datepicker.js",
                "~/scripts/lib/AutoComplete/jquery.highlight-3.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.client.js",
                "~/scripts/lib/MultiAutoComplete/MultiAutoCompleteHelper.js",
                "~/scripts/lib/MultiAutoComplete/jquery.fcbkcomplete.js",
                "~/Areas/MailStation/ManageMail/CopyMail/Scripts/CopyMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/CopyMail/CopyMailStyles",
                "~/themes/shared/formedit.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/scripts/lib/MultiAutoComplete/MultiAutoComplete.less",
                "~/Areas/MailStation/ManageMail/CopyMail/Styles/CopyMail.less");
        }

        private static void RegisterMailReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailReport/MailReportScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/custom-datepicker.js",
                "~/scripts/lib/Countdown/CountdownController.js",
                "~/scripts/lib/Popup/dialogbox.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/Report/MailReport/Scripts/MailReportController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailReport/MailReportStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/Countdown/CountDown.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/Report/MailReport/Styles/MailReport.less");
        }

        private static void RegisterMailLogBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/Report/MailLog/MailLogScripts").Include(
                "~/Areas/MailStation/Report/MailLog/Scripts/MailLogController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/Report/MailLog/MailLogStyles",
                "~/Areas/MailStation/Report/MailLog/Styles/MailLog.less");
        }

        private static void RegisterMailDetailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/Report/MailDetail/MailDetailScripts").Include(
                "~/Areas/MailStation/Report/MailDetail/Scripts/MailDetailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/Report/MailDetail/MailDetailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/Report/MailDetail/Styles/MailDetail.less");
        }

        private static void RegisterManageRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageRecipient/ManageRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/Popup/dialogbox.js",
                "~/scripts/lib/AutoComplete/jquery.highlight-3.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.client.js",
                "~/scripts/lib/MultiAutoComplete/MultiAutoCompleteHelper.js",
                "~/scripts/lib/MultiAutoComplete/jquery.fcbkcomplete.js",
                "~/Areas/MailStation/ManageRecipient/ManageRecipient/Scripts/ManageRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageRecipient/ManageRecipientStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/scripts/lib/MultiAutoComplete/MultiAutoComplete.less",
                "~/Areas/MailStation/ManageRecipient/ManageRecipient/Styles/ManageRecipient.less");
        }

        private static void RegisterEditRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/EditRecipient/EditRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageRecipient/EditRecipient/Scripts/EditRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/EditRecipient/EditRecipientStyles",
                "~/themes/shared/formedit.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageRecipient/EditRecipient/Styles/EditRecipient.less");
        }

        private static void RegisterInsertRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/InsertRecipient/InsertRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageRecipient/InsertRecipient/Scripts/InsertRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/InsertRecipient/InsertRecipientStyles",
                "~/themes/shared/formedit.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageRecipient/InsertRecipient/Styles/InsertRecipient.less");
        }

        private static void RegisterRelevantMailByRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/RelevantMailByRecipient/RelevantMailByRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/Areas/MailStation/Views/RelevantMailByRecipient/Scripts/RelevantMailByRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/RelevantMailByRecipient/RelevantMailByRecipientStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/Views/RelevantMailByRecipient/Styles/RelevantMailByRecipient.less");
        }
    }
}
