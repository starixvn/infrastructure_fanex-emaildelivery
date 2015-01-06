// --------------------------------------------------------------------------
// <copyright file="GetAllCategoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.NAP.Framework.Data;
    using System;
    using Fanex.MailDelivery.MailStation.Models;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class CreateMailCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int MailGroupID { get; set; }

        public int SiteID { get; set; }

        public int TriggerType { get; set; }

        public int RepeatType { get; set; }

        public string CronExpression { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int Attempt { get; set; }

        public int Interval { get; set; }

        public string Description { get; set; }

        public CreateMailCriteria()
        {
            this.IsActive = true;
        }

        public CreateMailCriteria(MailModel model)
            : this()
        {
            if (model != null)
            {
                this.MailName = model.MailName;
                this.MailGroupID = model.MailGroupId;
                this.SiteID = model.SiteId;
                this.TriggerType = model.TriggerType;
                this.RepeatType = model.RepeatType;
                this.CronExpression = model.CronExpression;
                this.MailFrom = model.MailFrom;
                this.MailTo = model.MailTo;
                this.MailCc = model.MailCc;
                this.MailBcc = model.MailBcc;
                this.IsActive = model.IsActive;
                this.StartOn = model.StartOn;
                this.EndOn = model.EndOn;
                this.Attempt = model.Attempt;
                this.Interval = model.Interval;
                this.Description = model.Description;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_CreateMail";
        }
    }
}
