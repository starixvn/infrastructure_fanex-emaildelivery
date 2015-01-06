namespace Fanex.MailDelivery.MailStation.Constants
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Mail Station Constants.
    /// </summary>
    public static class MailStationConstants
    {
        public static readonly List<SelectListItem> Active;

        public static readonly List<SelectListItem> TriggerTypes;

        public static readonly List<SelectListItem> AllItem;

        public static readonly List<SelectListItem> TaskStatus;

        static MailStationConstants()
        {
            AllItem = new List<SelectListItem>();
            AllItem.Add(new SelectListItem() { Text = "All", Value = "-1" });

            Active = new List<SelectListItem>();
            Active.Add(new SelectListItem() { Text = "Active", Value = "1" });
            Active.Add(new SelectListItem() { Text = "Inactive", Value = "0" });

            TriggerTypes = new List<SelectListItem>();
            TriggerTypes.Add(new SelectListItem() { Selected = false, Text = "Function", Value = "0" });
            TriggerTypes.Add(new SelectListItem() { Selected = false, Text = "Schedule", Value = "1" });

            TaskStatus = new List<SelectListItem>();
            TaskStatus.Add(new SelectListItem() { Text = "Waiting".ToUpper(), Value = "0" });
            TaskStatus.Add(new SelectListItem() { Text = "Running".ToUpper(), Value = "1" });
            TaskStatus.Add(new SelectListItem() { Text = "Data not ready".ToUpper(), Value = "2" });
            ////TaskStatus.Add(new SelectListItem() { Text = "Failed".ToUpper(), Value = "3" });
            TaskStatus.Add(new SelectListItem() { Text = "Sent".ToUpper(), Value = "4" });
        }
    }
}
