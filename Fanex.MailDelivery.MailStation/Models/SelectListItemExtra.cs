﻿// -----------------------------------------------------------------------
// <copyright file="SelectListItemExtra.cs" company="">
// Inherits SelectListItem to get more property Active.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2015.03.27">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Models
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelectListItemExtra : SelectListItem
    {
        public bool active
        {
            get;
            set;
        }
    }
}