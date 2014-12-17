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
        public int MailId;

        public string MailName { get; set; }

        public int MailGroupID { get; set; }

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

        public string CronExpression;

        public string MailFrom;

        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive;

        public DateTime LastSent { get; set; }

        public DateTime StartOn;

        public DateTime EndOn;

        public int Attempt;

        public int Interval;

        public string Description;

        public string LastSentString { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string ScheduledTimeString { get; set; }

        public string FromMail { get; set; }

        public string ToMails { get; set; }

        public string CCMails { get; set; }

        public string BCCMails { get; set; }

        public string MailDateString { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
