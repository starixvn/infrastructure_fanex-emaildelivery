// --------------------------------------------------------------------------
// <copyright file="MailGroupModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.Services.Models
{
    /// <summary>
    /// Mail Log Model.
    /// </summary>
    public class GroupModel
    {
        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public int MailRelevantCount { get; set; }
    }
}
