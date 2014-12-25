// --------------------------------------------------------------------------
// <copyright file="MailModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Models
{
    using System;

    /// <summary>
    /// Mail Model.
    /// </summary>
    public class MailModel
    {
        public int MailId { get; set; }

        public string MailName { get; set; }

        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int TriggerType { get; set; }

        public string TriggerTypeName
        {
            get 
            {
                switch (this.TriggerType) { 
                    case 0:
                        return "Function";
                    case 1:
                        return "Schedule";
                    default:
                        return "Unknow";
                }
            }
        }

        public int RepeatType { get; set; }

        public string CronExpression { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastSent { get; set; }

        public DateTime? StartOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int Attempt { get; set; }

        public int Interval { get; set; }

        public string Description { get; set; }
    }
}
