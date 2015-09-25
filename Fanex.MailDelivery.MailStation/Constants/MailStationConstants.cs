namespace Fanex.MailDelivery.MailStation.Constants
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.MailDelivery.MailStation.Models;

    /// <summary>
    /// Mail station constants.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public static class MailStationConstants
    {
        private static readonly List<SelectListItemExtra> active = new List<SelectListItemExtra>()
        {
            new SelectListItemExtra() { Text = "Active", Value = "1", Selected = false },
            new SelectListItemExtra() { Text = "Inactive", Value = "0", Selected = false }
        };

        private static readonly List<SelectListItemExtra> triggerTypes = new List<SelectListItemExtra>() 
        {
            new SelectListItemExtra() { Selected = false, Text = "Function", Value = "0" },
            new SelectListItemExtra() { Selected = false, Text = "Schedule", Value = "1" }
        };

        private static readonly List<SelectListItemExtra> allItem = new List<SelectListItemExtra>()
        {
            new SelectListItemExtra() { Text = "All", Value = "-1" }
        };

        private static readonly List<SelectListItemExtra> taskStatus = new List<SelectListItemExtra>()
        {
            new SelectListItemExtra() { Text = "Waiting".ToUpper(), Value = "0" },
            new SelectListItemExtra() { Text = "Running".ToUpper(), Value = "1" },
            new SelectListItemExtra() { Text = "Data not ready".ToUpper(), Value = "2" },
            new SelectListItemExtra() { Text = "Job Failed".ToUpper(), Value = "3" },
            new SelectListItemExtra() { Text = "Sent".ToUpper(), Value = "4" },
            new SelectListItemExtra() { Text = "Aborted".ToUpper(), Value = "6" },
            new SelectListItemExtra() { Text = "Succeeded".ToUpper(), Value = "7" },
            new SelectListItemExtra() { Text = "Missing".ToUpper(), Value = "8" }
        };

        public static List<SelectListItemExtra> Active
        {
            get
            {
                return active;
            }
        }

        public static List<SelectListItemExtra> TriggerTypes
        {
            get
            {
                return triggerTypes;
            }
        }

        public static List<SelectListItemExtra> AllItem
        {
            get
            {
                return allItem;
            }
        }

        public static List<SelectListItemExtra> TaskStatus
        {
            get
            {
                return taskStatus;
            }
        }
    }
}
