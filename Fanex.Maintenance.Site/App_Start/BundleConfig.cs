// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.8.30">Created</change>
// </history>

namespace Fanex.Maintenance.Site.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Bundle config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Register bundles.
        /// For more information on Bundling
        /// <see cref="http://go.microsoft.com/fwlink/?LinkId=254725"/>
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            // css
            bundles.AddAndInclude(
                "~/css/common",
                "~/themes/reset.css",
                "~/themes/fonts.css",
                "~/themes/jquery-ui/smoothness/jquery-ui-1.8.23.min.css",
                "~/themes/assets/css/prettify.css",
                "~/themes/assets/css/site.css",
                "~/scripts/lib/tipsy/tipsy-1.0.0.min.css",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.css",
                "~/scripts/lib/chosen/chosen.css");

            bundles.AddAndInclude(
                "~/css/mail-list",
                "~/themes/manage/manage.less",
                "~/themes/manage/RefreshCountDown.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            bundles.AddAndInclude(
                "~/css/manage",
                "~/themes/manage/manage.less");

            bundles.AddAndInclude(
                "~/css/database-report",
                "~/themes/manage/report/dataintegry.less");

            bundles.AddAndInclude(
                "~/css/log",
                "~/themes/manage/log/manage.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css",
                "~/scripts/lib/Select2/select2.css",
                "~/scripts/lib/Select2/select2-custom.css");

            bundles.AddAndInclude(
                "~/css/service-monitoring",
                "~/themes/manage/log/manage.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            bundles.AddAndInclude(
                "~/css/xml-editor",
                "~/themes/jquery-ui/XMLEditor/smoothness/jquery-ui-1.8.23.css",
                "~/scripts/lib/XMLEditor/css/main.css",
                "~/scripts/lib/XMLEditor/js/ext/GLR/css/GLR.messenger.css");

            // js
            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                "~/scripts/lib/jquery-1.8.0.min.js",
                "~/scripts/lib/jquery-ui-1.8.23.min.js",
                "~/scripts/lib/tipsy/tipsy-1.0.0.min.js",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.js",
                "~/scripts/Maintenance.js",
                "~/scripts/lib/chosen/chosen.jquery.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.js",
                "~/scripts/lib/jquery.cookie.js",
                "~/scripts/lib/RefreshCountDown.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-config").Include(
                "~/scripts/Mail/AddMailConfig.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-config-popup").Include(
                "~/scripts/Mail/AddMailConfigPopUp.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-setting-popup").Include(
                "~/scripts/Mail/AddMailSettingPopUp.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-function").Include(
                "~/scripts/Mail/AddMailFunction.js"));

            bundles.Add(new ScriptBundle("~/scripts/log").Include(
                "~/scripts/Logs/Index.js",
                "~/scripts/lib/Select2/select2.js",
                "~/scripts/lib/custom-datepicker.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.js"));

            bundles.Add(new ScriptBundle("~/scripts/recheckin").Include(
              "~/Areas/UserLogin/scripts/ReCheckIn.js"));

            bundles.Add(new ScriptBundle("~/scripts/connectionstring-database").Include(
              "~/scripts/Connection/Database.js"));

            bundles.Add(new ScriptBundle("~/scripts/connectionstring-alias").Include(
              "~/scripts/Connection/Alias.js"));

            bundles.Add(new ScriptBundle("~/scripts/connectionstring-aliasdatabase").Include(
              "~/scripts/Connection/AliasDatabase.js"));

            bundles.Add(new ScriptBundle("~/scripts/connectionstring-site").Include(
                "~/scripts/Connection/Site.js"));

            bundles.Add(new ScriptBundle("~/scripts/connectionstring-connection").Include(
                "~/scripts/Connection/Connection.js"));

            bundles.Add(new ScriptBundle("~/scripts/service-monitoring").Include(
                "~/scripts/ServiceMonitoring/ServiceMonitoring.js"));

            bundles.Add(new ScriptBundle("~/scripts/xml-editor").Include(
                "~/scripts/lib/jquery-1.8.0.min.js",
                "~/scripts/lib/jquery-ui-1.8.23.min.js",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.js",
                "~/scripts/Maintenance.js",
                "~/scripts/lib/XMLEditor/js/loc/xmlEditor.js",
                "~/scripts/lib/XMLEditor/js/ext/GLR/GLR.js",
                "~/scripts/lib/XMLEditor/js/ext/GLR/GLR.messenger.js"));

            RegisterSharedBundles(bundles);
            RegisterFanexGridBundles(bundles);
            MailStationBundles.RegisterAllBundles(bundles);
            bundles.FileSetOrderList.Clear();
        }

        public static void RegisterSharedBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/themes/sharedstyles",
                "~/themes/shared/sitestyle.less",
                "~/scripts/lib/Select2/select2.css",
                "~/scripts/lib/Select2/select2-custom.css");
        }

        public static void RegisterFanexGridBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/fanexgridscripts").Include(
                "~/scripts/lib/FanexGridV2/Fanex.Grid.js",
                "~/scripts/lib/FanexGridV2/Fanex.Pagination.js"));
            bundles.AddAndInclude(
                "~/themes/fanexgridstyles",
                "~/scripts/lib/FanexGridV2/Fanex.Grid.css",
                "~/scripts/lib/FanexGridV2/Fanex.Pagination.css",
                "~/scripts/lib/FanexGridV2/customizeFanexGridV2.less");
        }
    }
}