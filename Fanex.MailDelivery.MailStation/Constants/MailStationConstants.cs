using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.MailDelivery.MailStation.Models;

namespace Fanex.MailDelivery.MailStation.Constants
{
    public static class MailStationConstants
    {
        public readonly static List<SelectListItemExtra> Active;

        public readonly static List<SelectListItemExtra> TriggerTypes;

        public readonly static List<SelectListItemExtra> AllItem;

        public readonly static List<SelectListItemExtra> TaskStatus;

        static MailStationConstants()
        {
            AllItem = new List<SelectListItemExtra>();
            AllItem.Add(new SelectListItemExtra() { Text = "All", Value = "-1" });

            Active = new List<SelectListItemExtra>();
            Active.Add(new SelectListItemExtra() { Text = "Active", Value = "1" });
            Active.Add(new SelectListItemExtra() { Text = "Inactive", Value = "0" });

            TriggerTypes = new List<SelectListItemExtra>();
            TriggerTypes.Add(new SelectListItemExtra() { Selected = false, Text = "Function", Value = "0" });
            TriggerTypes.Add(new SelectListItemExtra() { Selected = false, Text = "Schedule", Value = "1" });

            TaskStatus = new List<SelectListItemExtra>();
            TaskStatus.Add(new SelectListItemExtra() { Text = "Waiting".ToUpper(), Value = "0" });
            TaskStatus.Add(new SelectListItemExtra() { Text = "Running".ToUpper(), Value = "1" });
            TaskStatus.Add(new SelectListItemExtra() { Text = "Sent".ToUpper(), Value = "2" });
            TaskStatus.Add(new SelectListItemExtra() { Text = "Data not ready".ToUpper(), Value = "3" });
            TaskStatus.Add(new SelectListItemExtra() { Text = "Failed".ToUpper(), Value = "4" });
        }
    }
}
