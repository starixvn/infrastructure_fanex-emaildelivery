// --------------------------------------------------------------------------
// <copyright file="RecipientModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Models
{
    /// <summary>
    /// Recipient Model.
    /// </summary>
    public class RecipientModel
    {
        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public bool IsNapUser { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public bool IsActive { get; set; }

        public int MailRelevant { get; set; }

        public string Note { get; set; }
    }
}
