namespace Fanex.Maintenance.Site.AppStart
{
    using System.Web.Optimization;

    /// <summary>
    /// Mail Station Bundles.
    /// </summary>
    public static class MailStationBundles
    {
        /// <summary>
        /// Registers all bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterAllBundles(BundleCollection bundles)
        {
            RegisterMultiSelect(bundles);
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
            RegisterUpdateRecipientBundles(bundles);
            RegisterCreateRecipientBundles(bundles);

            RegisterManageMailGroupBundles(bundles);
            RegisterCreateMailGroupBundles(bundles);
            RegisterUpdateMailGroupBundles(bundles);
        }

        private static void RegisterMultiSelect(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/lib/MultipleSelect/js").Include(
                "~/scripts/lib/MultipleSelect/jquery.multiselect.js"));

            bundles.AddAndInclude(
                "~/scripts/lib/MultipleSelect/css",
                "~/scripts/lib/MultipleSelect/jquery.multiselect.css");
        }

        private static void RegisterNexCron(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/lib/NexCron/js").Include(
                "~/scripts/lib/NexCron/js/NexCron.js"));

            bundles.AddAndInclude(
                "~/scripts/lib/NexCron/css",
                "~/scripts/lib/NexCron/css/NexCron.css");
        }

        private static void RegisterMagicSuggest(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/lib/MagicSuggest/js").Include(
                "~/scripts/lib/MagicSuggest/js/magicsuggest-min.js"));

            bundles.AddAndInclude(
                "~/scripts/lib/MagicSuggest/css",
                "~/scripts/lib/MagicSuggest/css/bootstrap.min.css",
                "~/scripts/lib/MagicSuggest/css/magicsuggest-min.css");
        }

        /// <summary>
        /// Registers the manage mail bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterManageMailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/ManageMail/ManageMail/ManageMailScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageMail/ManageMail/Scripts/MailManageController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/ManageMail/ManageMail/ManageMailStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageMail/ManageMail/Styles/ManageMail.less");
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
                "~/Areas/MailStation/ManageMail/CopyMail/Scripts/CopyMailController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/CopyMail/CopyMailStyles",
                "~/themes/shared/formedit.less",
                "~/Areas/MailStation/ManageMail/CopyMail/Styles/CopyMail.less");
        }

        /// <summary>
        /// Registers the mail report bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterMailReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailReport/MailReportScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/custom-datepicker.js",
                "~/scripts/lib/Countdown/CountdownController.js",
                "~/scripts/lib/Popup/dialogbox.js",
                "~/Areas/MailStation/Report/MailReport/Scripts/MailReportController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailReport/MailReportStyles",
                "~/themes/shared/contextmenu.less",
                "~/scripts/lib/Countdown/CountDown.less",
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

        /// <summary>
        /// Registers the manage recipient bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the update recipient bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterUpdateRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/UpdateRecipient/UpdateRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageRecipient/UpdateRecipient/Scripts/UpdateRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/UpdateRecipient/UpdateRecipientStyles",
                "~/themes/shared/formedit.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageRecipient/UpdateRecipient/Styles/UpdateRecipient.less");
        }

        /// <summary>
        /// Registers the create recipient bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCreateRecipientBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/CreateRecipient/CreateRecipientScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/scripts/lib/AutoComplete/jquery.autocomplete.js",
                "~/Areas/MailStation/ManageRecipient/CreateRecipient/Scripts/CreateRecipientController.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/CreateRecipient/CreateRecipientStyles",
                "~/themes/shared/formedit.less",
                "~/scripts/lib/AutoComplete/autocomplete.css",
                "~/Areas/MailStation/ManageRecipient/CreateRecipient/Styles/CreateRecipient.less");
        }

        /// <summary>
        /// Registers the manage mail group bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterManageMailGroupBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailGroup/MailGroupScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/Areas/MailStation/MailGroup/ManageMailGroup/Scripts/ManageMailGroup.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailGroup/MailGroupStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/MailGroup/ManageMailGroup/Styles/ManageMailGroup.less");
        }

        private static void RegisterCreateMailGroupBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailGroup/CreateMailGroupScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/Areas/MailStation/MailGroup/CreateMailGroup/Scripts/CreateMailGroup.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailGroup/CreateMailGroupStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/MailGroup/CreateMailGroup/Styles/CreateMailGroup.less");
        }

        private static void RegisterUpdateMailGroupBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MailStation/MailGroup/UpdateMailGroupScripts").Include(
                "~/scripts/lib/ContextMenu/ContextMenuController.js",
                "~/Areas/MailStation/MailGroup/UpdateMailGroup/Scripts/UpdateMailGroup.js"));

            bundles.AddAndInclude(
                "~/Areas/MailStation/MailGroup/UpdateMailGroupStyles",
                "~/themes/shared/contextmenu.less",
                "~/Areas/MailStation/MailGroup/UpdateMailGroup/Styles/UpdateMailGroup.less");
        }
    }
}
