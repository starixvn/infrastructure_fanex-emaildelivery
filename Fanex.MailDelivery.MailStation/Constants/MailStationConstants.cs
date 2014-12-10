using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fanex.MailDelivery.MailStation.Constants
{
    public static class MailStationConstants
    {
        public readonly static List<SelectListItem> Active;

        public readonly static List<SelectListItem> TriggerTypes;

        public readonly static List<SelectListItem> AllItem;

        static MailStationConstants()
        {
            AllItem = new List<SelectListItem>();
            AllItem.Add(new SelectListItem() { Text = "All", Value = "-1" });

            Active = new List<SelectListItem>();
            Active.Add(new SelectListItem() { Text = "Active", Value = "0" });
            Active.Add(new SelectListItem() { Text = "Inactive", Value = "1" });

            TriggerTypes = new List<SelectListItem>();
            TriggerTypes.Add(new SelectListItem() { Selected = false, Text = "Function", Value = "0" });
            TriggerTypes.Add(new SelectListItem() { Selected = false, Text = "Schedule", Value = "1" });
        }

    }
}
